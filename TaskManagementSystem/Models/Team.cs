namespace TaskManagementSystem.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
