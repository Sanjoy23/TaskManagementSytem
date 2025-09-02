using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TaskManagementSystem.Features.Tasks.Commands;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models;
using TaskManagementSystem.Service.IService;
using System.Security.Claims;
using Serilog;
using TaskManagementSystem.Features.Tasks.Queries;

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
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("tasks")]
        public async Task<IActionResult> GetAll([FromQuery] TaskFilterParameters filterParams)
        {
            var result = await _mediator.Send(new GetAllTasksQuery(filterParams));

            return Ok(new
            {
                Status = true,
                Message = "Tasks retrieved successfully",
                Result = result.Data
            });
        }



        //[Authorize(Roles = "Employee,Admin,Manager")]
        //[HttpGet("tasks")]
        //public async Task<IActionResult> GetAllTasks([FromQuery] string? status = null,
        //    [FromQuery] string? assignedToUserId = null,
        //    [FromQuery] string? teamId = null,
        //    [FromQuery] DateTime? dueDate = null,
        //    [FromQuery] int? pageNumber = null,
        //    [FromQuery] int? pageSize = null,
        //    [FromQuery] string? sortBy = null,
        //    [FromQuery] bool sortDesc = false)
        //{
        //    var tasks = await _taskService.GetAllTasksAsync(status, assignedToUserId, teamId, dueDate, pageNumber, pageSize, sortBy, sortDesc);
        //    return Ok(tasks);
        //}
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("task/{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var task = await _mediator.Send(id);
            if (task == null)
            {
                return NotFound(new { Status = false, Message = "Task not found" });
            }
            return Ok(new
            {
                Status = true,
                Message = "Task retrive successfully",
                Result = task
            });
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
        public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskCommand model)
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
