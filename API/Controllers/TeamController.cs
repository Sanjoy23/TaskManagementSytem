using Application.Features.Teams.Commands;
using Application.Features.Teams.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
       
        private readonly IMediator _mediator;

        public TeamController( IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("teams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var response = await _mediator.Send(new GetAllTeamQuery());
            if(response.Status)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("teams/{id}")]
        public async Task<IActionResult> GetTeamById(string id)
        {
            var response = await _mediator.Send(new GetTeamByIdQuery(id));
            if (response.Status)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("teams")]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status)
            {
                return Ok(response);
            }
            return Conflict(response); // 409
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("teams/{id}")]
        public async Task<IActionResult> UpdateTeam(string id, [FromBody] UpdateTeamCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("teams/{id}")]
        public async Task<IActionResult> DeleteTeam(DeleteTeamCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status)
            {
                return Ok(response);
            }
            return Conflict(response);
        }
    }
}
