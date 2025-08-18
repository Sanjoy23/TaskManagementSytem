namespace TaskManagementSystem.Models
{
    public class TasksStatus
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
