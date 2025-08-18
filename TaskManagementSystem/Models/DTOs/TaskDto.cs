namespace TaskManagementSystem.Models.DTOs
{
    public class TaskDto
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string AssignedToUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public string TeamId { get; set; }
        public DateTime DueDate { get; set; }
        public UserDto? AssignedToUser { get; set; }
        public UserDto? CreatedByUser { get; set; }
        public TeamDto? Team { get; set; }
    }
}
