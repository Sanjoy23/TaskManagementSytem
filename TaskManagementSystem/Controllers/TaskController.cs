using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TaskManagementSystem.Features.Tasks.Commands;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMediator _mediator;

        public TaskController(ITaskService taskService, IMediator mediator)
        {
            _taskService = taskService;
            _mediator = mediator;
        }

        [HttpGet("all-tasks")]
        public IActionResult GetAll([FromQuery] TaskFilterParameters filterParams)
        {
            Expression<Func<TaskEntity, bool>>? filter = null;

            if ((filterParams.Statuses != null && filterParams.Statuses.Any()) ||
                (filterParams.AssignedToUserIds != null && filterParams.AssignedToUserIds.Any()) ||
                (filterParams.TeamIds != null && filterParams.TeamIds.Any()) ||
                filterParams.DueDate.HasValue)
            {
                filter = t =>
                    (filterParams.Statuses == null || !filterParams.Statuses.Any() || (t.Status != null && filterParams.Statuses.Contains(t.Status.Name))) &&
                    (filterParams.AssignedToUserIds == null || !filterParams.AssignedToUserIds.Any() || filterParams.AssignedToUserIds.Contains(t.AssignedToUserId)) &&
                    (filterParams.TeamIds == null || !filterParams.TeamIds.Any() || (t.Team != null && filterParams.TeamIds.Contains(t.Team.Name))) &&
                    (!filterParams.DueDate.HasValue || t.DueDate.Date == filterParams.DueDate.Value.Date);
            }

            var tasks = _taskService.GetAllTasks(
                filter: filter,
                orderBy: q => q.OrderByDescending(t => t.DueDate) // example sorting
            );

            return Ok(new
            {
                Status = true,
                Message = "Tasks retrieved successfully",
                Result = tasks.ToList()
            });
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

        [HttpGet("task/{id}")]
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
        [HttpPost("task")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPut("task/{id}")]
        public  async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskCommand model)
        {
            if (model.Id != id.ToString())
            {
                return BadRequest(new { Status = false, Message = "Route ID doesn't match model ID" });
            }
            var result = await _mediator.Send(model);
            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("task/{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var result = await _mediator.Send(new DeleteTaskCommand { TaskId = id });

            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }

        }
    }
}
