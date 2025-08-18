namespace TaskManagementSystem.Models
{
    public class TaskEntity
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusId { get; set; } = null!;
        public TasksStatus? Status { get; set; } 
        public string AssignedToUserId { get; set; } = null!;
        public string CreatedByUserId { get; set; } = null!;

        public User? AssignedToUser { get; set; }
        public User? CreatedByUser { get; set; }

        public string TeamId { get; set; } = null!;

        public Team? Team { get; set; }
        public DateTime DueDate { get; set; }
    }
}
