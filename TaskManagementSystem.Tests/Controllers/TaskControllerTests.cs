using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Features.Tasks.Commands;
using TaskManagementSystem.Features.Tasks.Queries;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Repository.IRepository;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IWebSocketService> _webSocketServiceMock;
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _mediatorMock = new Mock<IMediator>();
            _webSocketServiceMock = new Mock<IWebSocketService>();
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _cacheMock = new Mock<IDistributedCache>();
            _taskController = new TaskController(
                _taskServiceMock.Object,
                _mediatorMock.Object,
                _webSocketServiceMock.Object,
                _notificationRepositoryMock.Object,
                _cacheMock.Object);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var taskModel = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Desc",
                StatusId = "Open",
                AssignedToUserId = "5",
                CreatedByUserId = "3",
                TeamId = "7",
                DueDate = DateTime.UtcNow
            };

            var createResult = new CreateTaskResult { Status = true, Message = "Successfully Created." };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createResult);

            // Act
            var result = await _taskController.CreateTask(taskModel);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as CreateTaskResult;
            response.Should().NotBeNull();
            response!.Status.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.Is<CreateTaskCommand>(c => c == taskModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnBadRequest_WhenTaskAlreadyExists()
        {
            // Arrange
            var taskModel = new CreateTaskCommand
            {
                Title = "Existing Task",
                Description = "Desc",
                StatusId = "Open",
                AssignedToUserId = "5",
                CreatedByUserId = "3",
                TeamId = "7",
                DueDate = DateTime.UtcNow
            };

            var createResult = new CreateTaskResult { Status = false, Message = "Task already exists." };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createResult);

            // Act
            var result = await _taskController.CreateTask(taskModel);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
            _mediatorMock.Verify(m => m.Send(It.Is<CreateTaskCommand>(c => c == taskModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnOk_WhenTaskExists()
        {
            // Arrange
            var taskId = "1";
            var taskEntity = new TaskResponse
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description"
            };

            _mediatorMock
                .Setup(s => s.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskEntity);

            // Act
            var result = await _taskController.GetTaskById(taskId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new
            {
                Status = true,
                Message = "Task retrive successfully",
                Result = taskEntity
            });
        }
    }
}