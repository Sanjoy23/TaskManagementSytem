namespace Domain.Entities
{
    public class Team
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
