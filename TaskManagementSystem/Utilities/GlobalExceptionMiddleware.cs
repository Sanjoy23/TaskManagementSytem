using Serilog;
using System.Net;

namespace TaskManagementSystem.Utilities
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }

        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            Log.Error(ex, ex.ToString());
            return httpContext.Response.WriteAsJsonAsync(new
            {
                Status = httpContext.Response.StatusCode,
                Message = "Internal Server Error. Contact Admin"
            });
        }
    }
}
