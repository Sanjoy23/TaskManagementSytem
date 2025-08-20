using TaskManagementSystem.Models.DTOs;

namespace TaskManagementSystem.Models.ResponseDtos
{
    public class TaskResponse
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusId { get; set; } = null!;
        public TasksStatus? Status { get; set; }
        public string AssignedToUserId { get; set; } = null!;
        public string CreatedByUserId { get; set; } = null!;

        public UserDto? AssignedToUser { get; set; }
        public UserDto? CreatedByUser { get; set; }

        public string TeamId { get; set; } = null!;

        public TeamDto? Team { get; set; }
        public DateTime DueDate { get; set; }
    }
}
