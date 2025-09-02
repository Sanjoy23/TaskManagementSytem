using System.Net.WebSockets;
using TaskManagementSystem.Models.DTOs;

namespace TaskManagementSystem.Service.IService
{
    public interface IWebSocketService
    {
        Task AddSocketAsync(WebSocket webSocket);
        Task BroadcastAsync(SocketMessage message);
        Task NotifyClientsAsync(string message);
    }
}
