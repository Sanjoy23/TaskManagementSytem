namespace TaskManagementSystem.Models.DTOs
{
    public class TaskUpdateRequestDto
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusId { get; set; } = string.Empty;
        public string AssignedToUserId { get; set; } = null!;
        public string CreatedByUserId { get; set; } = null!;
        public string TeamId { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }
}
