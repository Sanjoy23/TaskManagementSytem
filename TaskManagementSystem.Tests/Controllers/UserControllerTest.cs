using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Tests.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _userController;
        public UserControllerTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);
        }
        [Fact]
        public async Task Create_WithValidUser_ReturnsOkResult()
        {
            //Arrange
            var userModel = new UserModel
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Password = "Password123",
                RoleId = "User"
            };
            _userServiceMock
                .Setup(s => s.GetUserByEmailAsync(userModel.Email))
                .ReturnsAsync((User)null);

            _userServiceMock
                .Setup(s => s.Add(It.IsAny<User>()))
                .Verifiable();
            //ACT
            var result = await _userController.Create(userModel);
            //ASSERT
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<UserResponse>().Subject;

            response.Status.Should().BeTrue();
            response.Message.Should().Be("User Creation Completed");

            _userServiceMock.Verify(s => s.GetUserByEmailAsync(userModel.Email), Times.Once);
            _userServiceMock.Verify(s => s.Add(It.IsAny<User>()), Times.Once);


        }
        [Fact]
        public async Task Create_WithExistingEmail_ReturnsBadRequest()
        {
            //ARRANGE
            var userModel = new UserModel
            {
                FullName = "John Doe",
                Email = "existing@example.com",
                Password = "Password123",
                RoleId = "User"
            };

            var existingUser = new User
            {
                Id = "1",
                Email = "existing@example.com",
                FullName = "Existing User"
            };
            _userServiceMock.Setup(s => s.GetUserByEmailAsync(userModel.Email))
                .ReturnsAsync(existingUser);

            //ACT
            var result = await _userController.Create(userModel);

            //ASSERT
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badRequestResult.Value.Should().BeOfType<UserResponse>().Subject;

            response.Status.Should().BeFalse();
            response.Message.Should().Be("Email already Exist. Please try with another one.");

            _userServiceMock.Verify(s => s.GetUserByEmailAsync(userModel.Email), Times.Once);
            _userServiceMock.Verify(s => s.Add(It.IsAny<User>()), Times.Never);

        }

        [Fact]
        public async Task Create_WithInvalidModel_ReturnsBadRequest()
        {
            // ARRANGE
            var userModel = new UserModel
            {
                Email = "",
                Password = "123"
            };

            _userController.ModelState.AddModelError("Email", "Email is required");
            _userController.ModelState.AddModelError("FullName", "Full name is required");

            // ACT
            var result = await _userController.Create(userModel);

            // ASSERT
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badRequestResult.Value.Should().BeOfType<UserResponse>().Subject;

            response.Status.Should().BeFalse();
            response.Message.Should().Be("Bad Request");

            _userServiceMock.Verify(s => s.GetUserByEmailAsync(It.IsAny<string>()), Times.Never);
            _userServiceMock.Verify(s => s.Add(It.IsAny<User>()), Times.Never);
        }
    }
}
