using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    }
}
