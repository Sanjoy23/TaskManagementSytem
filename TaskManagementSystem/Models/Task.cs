namespace TaskManagementSystem.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }

        public User? AssignedToUser { get; set; }
        public User? CreatedByUser { get; set; }

        public int TeamId { get; set; }

        public Team? Team { get; set; }
        public DateTime DueDate { get; set; }
    }
}
