using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Controllers
{
    [Route("ws")]
    [ApiController]
    //[AllowAnonymous] // Allow WebSocket connections without authentication for testing
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketService _webSocketService;

        public WebSocketController(IWebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if(HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _webSocketService.AddSocketAsync(webSocket);
                return new EmptyResult(); 
            }
            else
            {
                // Return helpful message when called via HTTP (e.g., from Swagger)
                return BadRequest(new 
                { 
                    Error = "This endpoint requires a WebSocket connection",
                    Message = "Swagger UI cannot test WebSocket endpoints. Use one of the following:",
                    Instructions = new[]
                    {
                        "1. Use the websocket-test.html file in your browser",
                        "2. Use browser console: new WebSocket('ws://localhost:5012/ws')",
                        "3. Use a WebSocket client tool (e.g., Postman, WebSocket King)",
                        "4. Use the /ws/status endpoint to check connection count",
                        "5. Use the /ws/test-broadcast endpoint to send test messages"
                    },
                    WebSocketUrl = "ws://localhost:5012/ws"
                });
            }
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var activeConnections = _webSocketService.GetActiveConnectionCount();
            return Ok(new
            {
                Status = "WebSocket Service is running",
                ActiveConnections = activeConnections,
                WebSocketEndpoint = "ws://localhost:5012/ws",
                Message = activeConnections > 0 
                    ? $"{activeConnections} client(s) currently connected"
                    : "No active connections. Connect using a WebSocket client."
            });
        }

        [HttpPost("test-broadcast")]
        public async Task<IActionResult> TestBroadcast([FromBody] SocketMessage message)
        {
            if (message == null)
            {
                message = new SocketMessage
                {
                    Type = "Test",
                    Sender = "System",
                    Content = "Test broadcast message",
                    Timestamp = DateTime.UtcNow
                };
            }

            await _webSocketService.BroadcastAsync(message);
            return Ok(new { Status = true, Message = "Message broadcasted to all connected clients" });
        }
    }
}
