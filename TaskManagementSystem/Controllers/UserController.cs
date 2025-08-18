using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> Create([FromBody] UserModel usermodel)
        {
            if (!ModelState.IsValid)
            {
                var messages = ModelState
              .SelectMany(modelState => modelState.Value.Errors)
              .Select(err => err.ErrorMessage)
              .ToList();

                return BadRequest(new UserResponse
                {
                    Status = false,
                    Message = "Bad Request"
                });
            }

            var userExist = await _userService.GetUserByEmailAsync(usermodel.Email);
            if (userExist != null)
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
                RoleId = usermodel.RoleId

            };
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, usermodel.Password);
            try
            {
                _userService.Add(user);
                return Ok(new UserResponse
                {
                    Status = true,
                    Message = "User Creation Completed"
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(new UserResponse{
                    Status = false,
                    Message = ex.Message
                });
            }
            
            
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserModel userModel)
        {
            var existingUser = await _userService.GetById(id);
            if (existingUser == null)
            {
                return NotFound(new UserResponse
                {
                    Status = false,
                    Message = "User not found"
                });
            }
            var passwordHasher = new PasswordHasher<User>();
            var newPassword = passwordHasher.HashPassword(existingUser, userModel.Password);

            existingUser.FullName = userModel.FullName ?? existingUser.FullName;
            existingUser.Email = userModel.Email ?? existingUser.Email; 
            existingUser.Password = newPassword ?? existingUser.Password ; 
            existingUser.RoleId = userModel.RoleId ?? existingUser.RoleId; 

            try
            {
                _userService.Update(existingUser);
                return Ok(new UserResponse
                {
                    Status = true,
                    Message = "User updated successfully"
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(new UserResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingUser = await _userService.GetById(id);
            if (existingUser == null)
            {
                return NotFound(new UserResponse
                {
                    Status = false,
                    Message = "User not found. Failed to delete."
                });
            }
            try
            {
                _userService.Delete(existingUser);
                return Ok(new UserResponse
                {
                    Status = true,
                    Message = "User deleted successfully."
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(new UserResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }

    }
}
