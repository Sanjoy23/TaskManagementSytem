using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _mediator.Send(new GetAllUserQuery());
            if (results.Status)
            {
                return Ok(results);
            }
            return NotFound(results);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery(id));
            if (response.Status)
            {
                return Ok(response);
            }
            return NotFound(response);
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
