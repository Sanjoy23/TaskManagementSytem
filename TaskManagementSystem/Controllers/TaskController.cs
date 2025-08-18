using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            [FromQuery] string? assignedToUserId = null,
            [FromQuery] string? teamId = null,
            [FromQuery] DateTime? dueDate = null,
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDesc = false)
        {
            var tasks = await _taskService.GetAllTasksAsync(status, assignedToUserId, teamId, dueDate, pageNumber, pageSize, sortBy, sortDesc);
            return Ok(tasks);
        }

        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var task = await _taskService.GetById(id);
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

                _taskService.Add(taskModel);
                return Ok(new
                {
                    Status = true,
                    Message = "Successfully Created."
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(new
                {
                    Status = false,
                    Message = ex.Message.ToString()
                });
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPut("tasks/{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskUpdateRequestDto model)
        {
            try
            {
                _taskService.Update(model);
                return Ok(new
                {
                    Status = true,
                    Message = "Task updated successfully"
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(new { Status = false, Message = "Failed to update task" });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var existingTask = await _taskService.GetById(id);
            if (existingTask == null)
            {
                return NotFound(new { Status = false, Message = "Task not found" });
            }

            try
            {
                _taskService.Delete(existingTask);
                return Ok(new
                {
                    Status = true,
                    Message = "Task is deleted."
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(new
                {
                    Status = false,
                    Message = ex.Message.ToString()
                });
            }

        }
        [Authorize(Roles = "Employee")]
        [HttpPut("status/")]
        public IActionResult UpdateStatus(string taskId, string statusId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _taskService.UpdateStatus(userId, taskId, statusId);
                return Ok(new 
                { 
                    Status = true,
                    Message = "Status Changed successfully"
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(new
                {
                    Status = false,
                    Message = ex.Message.ToString()
                });
            }

        }
    }
}
