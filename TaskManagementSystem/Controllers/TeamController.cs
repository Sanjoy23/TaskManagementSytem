using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
            try
            {
                var teams = await _teamService.GetAll();
                return Ok(teams);
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

        [Authorize(Roles = "Admin")]
        [HttpGet("teams/{id}")]
        public async Task<IActionResult> GetTeamById(string id)
        {
            try
            {
                var team = await _teamService.GetById(id);
                return Ok(team);
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

        [Authorize(Roles = "Admin")]
        [HttpPost("teams")]
        public  IActionResult CreateTeam([FromBody] TeamModel teamModel)
        {
            //var teamExist = await _teamService.GetTeamByNameAsync();
            var team = new Team
            {
                Id = Guid.NewGuid().ToString(),
                Name = teamModel.Name,
                Description = teamModel.Description
            };
            try
            {
                _teamService.Add(team);
                return Ok(new { Status = true, Message = "Team created successfully" });
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

        [Authorize(Roles = "Admin")]
        [HttpPut("teams/{id}")]
        public async Task<IActionResult> UpdateTeam(string id, [FromBody] TeamModel teamModel)
        {
            var existingTeam = await _teamService.GetById(id);
            if (existingTeam == null)
            {
                return NotFound(new { Status = false, Message = "Team not found" });
            }

            existingTeam.Name = teamModel.Name == "" ? existingTeam.Name : teamModel.Name;
            existingTeam.Description = teamModel.Description == ""
                                    ? existingTeam.Description : teamModel.Description;

            try
            {
                _teamService.Update(existingTeam);
                return Ok(new
                {
                    Status = true,
                    Message = "Team updated successfully"
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("teams/{id}")]
        public async Task<IActionResult> DeleteTeam(string id)
        {
            var existingTeam = await _teamService.GetById(id);
            if (existingTeam == null)
            {
                return NotFound(new { Status = false, Message = "Team not found" });
            }

            try
            {
                _teamService.Delete(existingTeam);
                return Ok(new { Status = true, Message = "Team deleted successfully" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(new { Status = false, Message = "Failed to delete team" });
            }     
        }


    }
}
