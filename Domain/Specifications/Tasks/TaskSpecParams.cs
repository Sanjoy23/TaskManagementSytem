namespace Domain.Specifications.Tasks
{
    public class TaskSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 20;
        public  int PageSize
        {
            get => _pageSize;
            set => _pageSize = value;
        }

        public string? StatusId { get; set; }
        public string? TeamId { get; set; }

        public string Sort { get; set; }
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
