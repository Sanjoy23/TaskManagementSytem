using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Service.IService;
using ModelTask = TaskManagementSystem.Models.Task;

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
        [Authorize(Roles = "Employee, Admin, Manager")]
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
        [Authorize(Roles = "Manager")]
        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel taskModel)
        {
            // Basic validation can be added here

            var taskEntity = new ModelTask
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
                return BadRequest(new { Status = false, Message = "Failed to create task" });
            }

            return Ok(new { Status = true, Message = "Task created successfully", Task = createdTask });
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

            existingTask.Title = taskModel.Title;
            existingTask.Description = taskModel.Description;
            existingTask.Status = taskModel.Status;
            existingTask.AssignedToUserId = taskModel.AssignedToUserId;
            existingTask.CreatedByUserId = taskModel.CreatedByUserId;
            existingTask.TeamId = taskModel.TeamId;
            existingTask.DueDate = taskModel.DueDate;

            var updatedTask = await _taskService.UpdateTaskAsync(existingTask);
            if (updatedTask == null)
            {
                return BadRequest(new { Status = false, Message = "Failed to update task" });
            }

            return Ok(new { Status = true, Message = "Task updated successfully", Task = updatedTask });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("tasks/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> DeleteTask(int id)
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
