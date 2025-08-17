using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Service.IService; // Add this import

namespace TaskManagementSystem.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock; // Change to ITaskService
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>(); // Mock the interface
            _taskController = new TaskController(_taskServiceMock.Object);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var taskModel = new TaskModel
            {
                Title = "Test Task",
                Description = "Desc",
                Status = "Open",
                AssignedToUserId = 5,
                CreatedByUserId = 3,
                TeamId = 7,
                DueDate = DateTime.UtcNow
            };

            // Setup mocks
            _taskServiceMock
                .Setup(s => s.GetTaskByTitleAsync(taskModel.Title))
                .ReturnsAsync((TaskEntity?)null); // No existing task

            _taskServiceMock
                .Setup(s => s.Add(It.IsAny<TaskEntity>()))
                .Verifiable();

            // Act
            var result = await _taskController.CreateTask(taskModel);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as UserResponse;
            response.Should().NotBeNull();
            response!.Status.Should().BeTrue();
            response.Message.Should().Be("Successfully Created.");
        }

        [Fact]
        public async Task CreateTask_ShouldReturnBadRequest_WhenTaskAlreadyExists()
        {
            // Arrange
            var taskModel = new TaskModel
            {
                Title = "Existing Task",
                Description = "Desc",
                Status = "Open",
                AssignedToUserId = 5,
                CreatedByUserId = 3,
                TeamId = 7,
                DueDate = DateTime.UtcNow
            };

            var existingTask = new TaskEntity
            {
                Id = 1,
                Title = taskModel.Title
            };

            _taskServiceMock
                .Setup(s => s.GetTaskByTitleAsync(taskModel.Title))
                .ReturnsAsync(existingTask); // Task already exists

            // Act
            var result = await _taskController.CreateTask(taskModel);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnOk_WhenTaskExists()
        {
            // Arrange
            var taskId = 1;
            var taskEntity = new TaskDto
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description"
            };

            _taskServiceMock
                .Setup(s => s.GetTaskByIdAsync(taskEntity.Id))
                .ReturnsAsync(taskEntity);

            // Act
            var result = await _taskController.GetTaskById(taskId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(taskEntity);
        }
    }
}