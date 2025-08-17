﻿namespace TaskManagementSystem.Models.DTOs
{
    public class PagedResultDto<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
