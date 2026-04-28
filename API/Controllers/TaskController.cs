using API.Caching;
using API.Services.IServices;
using Application.Features;
using Application.Models;
using Domain.Entities;
using Domain.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        
        private readonly IMediator _mediator;
        private readonly IWebSocketService _webSocketService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IDistributedCache _cache;

        public TaskController( IMediator mediator, IWebSocketService webSocketService,
            INotificationRepository notificationRepository, IDistributedCache cache)
        {
            
            _mediator = mediator;
            _webSocketService = webSocketService;
            _notificationRepository = notificationRepository;
            _cache = cache;
        }
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("tasks")]
        public async Task<IActionResult> GetAll([FromQuery] TaskFilterParameters filterParams)
        {
            var cachedData = await _cache.GetRecordAsync<List<TaskResponse>>("AllTasks");
            if(cachedData != null)
            {
                return Ok(new
                {
                    Status = true,
                    Message = "Tasks retrieved successfully (from cache)",
                    Result = cachedData
                });
            }
            var result = await _mediator.Send(new GetAllTasksQuery(filterParams));
            await _cache.SetRecordAsync("AllTasks", result.Data, TimeSpan.FromMinutes(5));
            return Ok(new
            {
                Status = true,
                Message = "Tasks retrieved successfully",
                Result = result.Data
            });
        }


        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("task/{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var query = new GetTaskByIdQuery(id);
            var task = await _mediator.Send(query);
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
        [EnableRateLimiting("FixedPolicey")] // added rate limiter at specific endpoints.
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
        public async Task<IActionResult> UpdateStatus(string taskId, string statusId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // need to do _taskService.UpdateStatus(userId, taskId, statusId);
                await _webSocketService.NotifyClientsAsync("Task updated successfully.");
                var notification = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Title = "Task Status Updated",
                    Message = $"Task {taskId} status changed to {statusId}"
                };
                _notificationRepository.Add(notification);
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
