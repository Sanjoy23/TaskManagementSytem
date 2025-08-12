using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize(Roles = "Employee,Admin,Manager")]
        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks([FromQuery] string? status = null,
            [FromQuery] int? assignedToUserId = null,
            [FromQuery] int? teamId = null,
            [FromQuery] DateTime? dueDate = null,                                                                           
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDesc = false)
        {
            var tasks = await _taskService.GetAllTasksAsync(status, assignedToUserId, teamId, dueDate, pageNumber,pageSize,sortBy, sortDesc);
            return Ok(tasks);
        }

        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound(new { Status = false, Message = "Task not found" });
            }
            return Ok(task);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel taskModel)
        {
            var taskExist = await _taskService.GetTaskByTitleAsync(taskModel.Title); // Add await
            if (taskExist != null)
            {
                return BadRequest(new UserResponse
                {
                    Status = false,
                    Message = "Same Task is created already"
                });
            }

            try
            {
                var taskEntity = new TaskEntity
                {
                    Title = taskModel.Title,
                    Description = taskModel.Description,
                    Status = taskModel.Status,
                    AssignedToUserId = taskModel.AssignedToUserId,
                    CreatedByUserId = taskModel.CreatedByUserId,
                    TeamId = taskModel.TeamId,
                    DueDate = taskModel.DueDate
                };

                var createdTask = await _taskService.CreateTaskAsync(taskEntity);

                if (createdTask == null)
                {
                    return BadRequest(new UserResponse { Status = false, Message = "Failed to create task" });
                }

                return Ok(new UserResponse
                {
                    Status = true,
                    Message = "Successfully Created."
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating task");
                return StatusCode(500, new UserResponse { Status = false, Message = "Internal server error" });
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("tasks/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel taskModel)
        {
            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound(new { Status = false, Message = "Task not found" });
            }

            var taskEntity = new TaskEntity
            {
                Id = existingTask.Id,
                Title = existingTask.Title,
                Description = existingTask.Description,
                Status = existingTask.Status,
                AssignedToUserId = taskModel.AssignedToUserId,
                CreatedByUserId = taskModel.CreatedByUserId,
                TeamId = taskModel.TeamId,
                DueDate = taskModel.DueDate
            };

            var updatedTask = await _taskService.UpdateTaskAsync(taskEntity);
            if (updatedTask == null)
            {
                return BadRequest(new { Status = false, Message = "Failed to update task" });
            }

            return Ok(new { Status = true, Message = "Task updated successfully", Task = updatedTask });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound(new { Status = false, Message = "Task not found" });
            }

            var success = await _taskService.DeleteTaskAsync(id);
            if (!success)
            {
                return BadRequest(new { Status = false, Message = "Failed to delete task" });
            }

            return Ok(new { Status = true, Message = "Task deleted successfully" });
        }


    }
}
