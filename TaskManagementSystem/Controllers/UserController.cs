using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new UserResponse
                {
                    Status = false,
                    Message = "User not found"
                });
            }
            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> Create([FromBody] UserModel usermodel)
        {
            var userExist = await _userService.GetUserByEmailAsync(usermodel.Email);
            if(userExist != null)
            {
                return BadRequest(new UserResponse
                {
                    Status = false,
                    Message = "Email already Exist. Please try with another one."
                });
            }
            var user = new User
            {
                FullName = usermodel.FullName,
                Email = usermodel.Email,
                Password = usermodel.Password,
                Role = usermodel.Role

            };
            var result = _userService.CreateUserAsync(user);
            if (result == null) {
                Log.Error(result.ToString());
                return BadRequest(new UserResponse
                {
                    Status = false,
                    Message = "Internal Server Problem"
                });
            }
            return Ok(new UserResponse
            {
                Status = true,
                Message = "User Creation Completed"
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserModel userModel)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new UserResponse
                {
                    Status = false,
                    Message = "User not found"
                });
            }

            existingUser.FullName = userModel.FullName == ""? existingUser.FullName : userModel.FullName;
            existingUser.Email = userModel.Email == "null" ? existingUser.Email : userModel.Email; ;
            existingUser.Password = userModel.Password == "" ? existingUser.Password : userModel.Password; ;
            existingUser.Role = userModel.Role == "" ? existingUser.Role : userModel.Role; ;

            var updatedUser = await _userService.UpdateUserAsync(existingUser);

            if (updatedUser == null)
            {
                return BadRequest(new UserResponse
                {
                    Status = false,
                    Message = "Failed to update user"
                });
            }

            return Ok(new UserResponse
            {
                Status = true,
                Message = "User updated successfully"
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new UserResponse
                {
                    Status = false,
                    Message = "User not found"
                });
            }

            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return BadRequest(new UserResponse
                {
                    Status = false,
                    Message = "Failed to delete user"
                });
            }

            return Ok(new UserResponse
            {
                Status = true,
                Message = "User deleted successfully"
            });
        }

    }
}
