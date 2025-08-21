namespace TaskManagementSystem.Models.DTOs
{
    public class TaskFilterParameters
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public List<string>? Statuses { get; set; }
        public List<int>? AssignedToUserIds { get; set; }
        public List<string>? TeamIds { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public string? SearchTerm { get; set; } // For title/description search
        public string? SortBy { get; set; } = "DueDate"; // Default sort
        public string? SortDirection { get; set; } = "desc"; // asc or desc
    }
}
