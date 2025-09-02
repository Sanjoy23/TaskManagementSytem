using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Features.Users.Commands;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public UserController(IUserService userService, IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound(new UserResponse
                {
                    Status = false,
                    Message = "User not found"
                });
            }
            return Ok(new
            {
                FullName = user.FullName.ToString(),
                Email = user.Email.ToString(),
                RoleName = user.Role?.RoleName.ToString() ?? string.Empty,
            });
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Status)
            {
                return Ok(result);
            }
            else return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> Delete(DeleteUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
