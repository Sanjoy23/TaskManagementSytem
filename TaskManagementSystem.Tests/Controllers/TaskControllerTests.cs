using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Models;
using TaskManagementSystem.Service;

namespace TaskManagementSystem.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<TaskService> _taskServiceMock;
        private readonly TaskController _taskController;

        public TaskControllerTests()
        {
            _taskServiceMock = new Mock<TaskService>();
            _taskController = new TaskController(_taskServiceMock.Object);
        }

        //[Fact]
        //public async Task CreateTask_ShouldReturnCreated_WhenSuccessful()
        //{
        //    //arrange
        //    var taskModel = new TaskModel
        //    {
        //        Title = "Test Task",
        //        Description = "Desc",
        //        Status = "Open",
        //        AssignedToUserId = 1,
        //        CreatedByUserId = 2,
        //        TeamId = 3,
        //        DueDate = DateTime.UtcNow
        //    };

        //    var createdTask = new TaskModel
        //    {
        //        TeamId = 3,
        //        Title = taskModel.Title,
        //        Description = taskModel.Description,
        //    };
        //    _taskServiceMock
        //       .Setup(s => s.CreateTaskAsync(It.IsAny<TaskModel>()))
        //       .ReturnsAsync(createdTask);
        //}
    }
}
