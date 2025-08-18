﻿namespace TaskManagementSystem.Models
{
    public class Role
    {
        public string Id { get; set; } = null!;
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
