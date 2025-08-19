namespace TaskManagementSystem.Models.DTOs
{
    public class TaskFilterParameters
    {
        public List<string>? Statuses { get; set; }
        public List<string>? AssignedToUserIds { get; set; }
        public List<string>? TeamIds { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
