using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Models
{
    public class User
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RoleId { get; set; } = null!;
        public Role? Role { get; set; }

        public ICollection<TaskEntity> CreatedTasks { get; set; } = new List<TaskEntity>();
        public ICollection<TaskEntity> AssignedTasks { get; set; } = new List<TaskEntity>();
    }
}
