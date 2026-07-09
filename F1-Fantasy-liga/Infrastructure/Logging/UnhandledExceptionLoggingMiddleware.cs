using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace F1_Fantasy_liga.Infrastructure.Logging
{
    public sealed class UnhandledExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.Extensions.Logging.ILogger<UnhandledExceptionLoggingMiddleware> _logger;

        public UnhandledExceptionLoggingMiddleware(RequestDelegate next, Microsoft.Extensions.Logging.ILogger<UnhandledExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var userName = GetUserName(context);

                _logger.LogError(
                    exception,
                    "Unhandled exception for {Method} {Path} by {User}",
                    context.Request.Method,
                    context.Request.Path,
                    userName);

                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = "An unexpected error occurred. Please try again later."
                    });

                    return;
                }

                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(
                    "<html><head><title>Error</title></head><body><h1>Something went wrong.</h1><p>Please try again later.</p></body></html>");
            }
        }

        private static string GetUserName(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    return context.User.Identity.Name;
                }

                var preferredName = context.User.FindFirstValue(ClaimTypes.Name)
                    ?? context.User.FindFirstValue(ClaimTypes.Email)
                    ?? context.User.FindFirstValue("name");

                if (!string.IsNullOrWhiteSpace(preferredName))
                {
                    return preferredName;
                }
            }

            return "Anonymous";
        }
    }
}