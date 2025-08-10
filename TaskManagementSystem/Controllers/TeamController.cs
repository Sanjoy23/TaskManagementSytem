using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("teams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("teams/{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound(new { Status = false, Message = "Team not found" });
            }
            return Ok(team);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("teams")]
        public async Task<IActionResult> CreateTeam([FromBody] TeamModel teamModel)
        {
            //var teamExist = await _teamService.GetTeamByNameAsync();
            var team = new Team
            {
                Name = teamModel.Name,
                Description = teamModel.Description
            };

            var createdTeam = await _teamService.CreateTeamAsync(team);

            if (createdTeam == null)
            {
                return BadRequest(new { Status = false, Message = "Failed to create team" });
            }

            return Ok(new { Status = true, Message = "Team created successfully", Team = createdTeam });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("teams/{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamModel teamModel)
        {
            var existingTeam = await _teamService.GetTeamByIdAsync(id);
            if (existingTeam == null)
            {
                return NotFound(new { Status = false, Message = "Team not found" });
            }

            existingTeam.Name = teamModel.Name == "" ? existingTeam.Name : teamModel.Name;
            existingTeam.Description = teamModel.Description == "" ? existingTeam.Description : teamModel.Description;

            var updatedTeam = await _teamService.UpdateTeamAsync(existingTeam);
            if (updatedTeam == null)
            {
                return BadRequest(new { Status = false, Message = "Failed to update team" });
            }

            return Ok(new { Status = true, Message = "Team updated successfully", Team = updatedTeam });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("teams/{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var existingTeam = await _teamService.GetTeamByIdAsync(id);
            if (existingTeam == null)
            {
                return NotFound(new { Status = false, Message = "Team not found" });
            }

            var success = await _teamService.DeleteTeamAsync(id);
            if (!success)
            {
                return BadRequest(new { Status = false, Message = "Failed to delete team" });
            }

            return Ok(new { Status = true, Message = "Team deleted successfully" });
        }


    }
}
