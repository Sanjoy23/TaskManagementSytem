using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Features.Users.Commands;
using TaskManagementSystem.Features.Users.Queries;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Tests.Controllers;

public class UserControllerTest
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UserController _userController;

    public UserControllerTest()
    {
        _userServiceMock = new Mock<IUserService>();
        _mediatorMock = new Mock<IMediator>();
        _userController = new UserController(_userServiceMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task Create_WithSuccessfulResponse_ReturnsOkResult()
    {
        var command = new CreateUserCommand
        {
            FullName = "John Doe",
            Email = "john@example.com",
            Password = "Password123!",
            RoleId = "admin-role-id"
        };
        var response = new CreateUserResponse { Status = true, Message = "User created successfully." };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.Create(command);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeOfType<CreateUserResponse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(m => m.Send(It.Is<CreateUserCommand>(c => c == command), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_WithFailedResponse_ReturnsBadRequest()
    {
        var command = new CreateUserCommand
        {
            FullName = "John Doe",
            Email = "existing@example.com",
            Password = "Password123!",
            RoleId = "admin-role-id"
        };
        var response = new CreateUserResponse { Status = false, Message = "User already exists." };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.Create(command);

        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var value = badRequestResult.Value.Should().BeOfType<CreateUserResponse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(m => m.Send(It.Is<CreateUserCommand>(c => c == command), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_WithSuccessfulResponse_ReturnsOkResult()
    {
        var response = new GetAllUserRespnse
        {
            Status = true,
            Message = "Users retrieved."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.GetAll();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeOfType<GetAllUserRespnse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllUserQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_WithFailedResponse_ReturnsNotFound()
    {
        var response = new GetAllUserRespnse
        {
            Status = false,
            Message = "No users found."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.GetAll();

        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var value = notFoundResult.Value.Should().BeOfType<GetAllUserRespnse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllUserQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WithSuccessfulResponse_ReturnsOkResult()
    {
        var id = "user-123";
        var response = new GetUserByIdResponse
        {
            Status = true,
            Message = "User found."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.GetById(id);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeOfType<GetUserByIdResponse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetUserByIdQuery>(q => q.UserId == id), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetById_WithFailedResponse_ReturnsNotFound()
    {
        var id = "missing-user";
        var response = new GetUserByIdResponse
        {
            Status = false,
            Message = "User not found."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.GetById(id);

        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var value = notFoundResult.Value.Should().BeOfType<GetUserByIdResponse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(
            m => m.Send(It.Is<GetUserByIdQuery>(q => q.UserId == id), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Update_WithSuccessfulResponse_ReturnsOkResult()
    {
        var id = "user-123";
        var command = new UpdateUserCommand
        {
            Id = id,
            FullName = "Updated Name",
            Email = "updated@example.com",
            RoleId = "manager-role-id"
        };
        var response = new UserUpdateResponse
        {
            Status = true,
            Message = "User updated."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.Update(id, command);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeOfType<UserUpdateResponse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(m => m.Send(It.Is<UpdateUserCommand>(c => c == command), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WithFailedResponse_ReturnsBadRequest()
    {
        var id = "user-123";
        var command = new UpdateUserCommand
        {
            Id = id,
            FullName = "Updated Name",
            Email = "updated@example.com",
            RoleId = "manager-role-id"
        };
        var response = new UserUpdateResponse
        {
            Status = false,
            Message = "Update failed."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.Update(id, command);

        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var value = badRequestResult.Value.Should().BeOfType<UserUpdateResponse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(m => m.Send(It.Is<UpdateUserCommand>(c => c == command), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WithSuccessfulResponse_ReturnsOkResult()
    {
        var command = new DeleteUserCommand { Id = "user-123" };
        var response = new UserDeleteResponse
        {
            Status = true,
            Message = "User deleted."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.Delete(command);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeOfType<UserDeleteResponse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(m => m.Send(It.Is<DeleteUserCommand>(c => c == command), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WithFailedResponse_ReturnsBadRequest()
    {
        var command = new DeleteUserCommand { Id = "user-123" };
        var response = new UserDeleteResponse
        {
            Status = false,
            Message = "Delete failed."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _userController.Delete(command);

        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var value = badRequestResult.Value.Should().BeOfType<UserDeleteResponse>().Subject;
        value.Should().BeEquivalentTo(response);
        _mediatorMock.Verify(m => m.Send(It.Is<DeleteUserCommand>(c => c == command), It.IsAny<CancellationToken>()), Times.Once);
    }
}
