namespace TaskManagementSystem.Models.DTOs
{
    public class SocketMessage
    {
        public string Type { get; set; } = string.Empty;   
        public string Sender { get; set; } = string.Empty; 
        public string Content { get; set; } = string.Empty; 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
