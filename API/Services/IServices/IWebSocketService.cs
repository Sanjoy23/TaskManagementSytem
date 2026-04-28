using Application.Models;
using System.Net.WebSockets;

namespace API.Services.IServices
{
    public interface IWebSocketService
    {
        Task AddSocketAsync(WebSocket webSocket);
        Task BroadcastAsync(SocketMessage message);
        Task NotifyClientsAsync(string message);
        int GetActiveConnectionCount();
    }
}
