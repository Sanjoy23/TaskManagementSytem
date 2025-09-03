namespace TaskManagementSystem.Models.DTOs
{

    public class UserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleId { get; set; } = null!;
        public Role? Role { get; set; }
        public ICollection<TaskEntity> CreatedTasks { get; set; } = new List<TaskEntity>();
        public ICollection<TaskEntity> AssignedTasks { get; set; } = new List<TaskEntity>();
    }

}
