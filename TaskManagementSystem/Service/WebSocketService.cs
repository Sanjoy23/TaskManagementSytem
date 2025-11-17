using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Service
{
    public class WebSocketService : IWebSocketService
    {
        // Use ConcurrentDictionary for thread-safe collection with easy removal
        private static readonly ConcurrentDictionary<WebSocket, byte> _connections = new();

        public async Task AddSocketAsync(WebSocket webSocket)
        {
            _connections.TryAdd(webSocket, 0);
            var buffer = new byte[1024 * 4];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await RemoveConnectionAsync(webSocket);
                        await webSocket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Closed by client",
                            CancellationToken.None);
                        break;
                    }
                    
                }
            }
            catch (WebSocketException ex)
            {
                // Handle WebSocket-specific exceptions
                await RemoveConnectionAsync(webSocket);
                if (webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.Aborted)
                {
                    try
                    {
                        await webSocket.CloseAsync(
                            WebSocketCloseStatus.InternalServerError,
                            ex.Message,
                            CancellationToken.None);
                    }
                    catch { /* Socket already closed */ }
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                await RemoveConnectionAsync(webSocket);
                if (webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.Aborted)
                {
                    try
                    {
                        await webSocket.CloseAsync(
                            WebSocketCloseStatus.InternalServerError,
                            "Internal server error",
                            CancellationToken.None);
                    }
                    catch { /* Socket already closed */ }
                }
            }
            finally
            {
                await RemoveConnectionAsync(webSocket);
                webSocket?.Dispose();
            }
        }

        private async Task RemoveConnectionAsync(WebSocket webSocket)
        {
            _connections.TryRemove(webSocket, out _);
            await Task.CompletedTask;
        }

        public async Task BroadcastAsync(SocketMessage message)
        {
            var serializeMessage = JsonConvert.SerializeObject(message);
            await NotifyClientsAsync(serializeMessage);
        }

        public async Task NotifyClientsAsync(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var tasks = new List<Task>();

            foreach (var connection in _connections.Keys)
            {
                if (connection != null && connection.State == WebSocketState.Open)
                {
                    tasks.Add(SendMessageAsync(connection, messageBytes));
                }
            }

            await Task.WhenAll(tasks);
        }

        private async Task SendMessageAsync(WebSocket webSocket, byte[] messageBytes)
        {
            try
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(messageBytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
            }
            catch (WebSocketException)
            {
                await RemoveConnectionAsync(webSocket);
            }
            catch (Exception)
            {
                await RemoveConnectionAsync(webSocket);
            }
        }

        public int GetActiveConnectionCount()
        {
            return _connections.Keys.Count(c => c != null && c.State == WebSocketState.Open);
        }
    }
}
