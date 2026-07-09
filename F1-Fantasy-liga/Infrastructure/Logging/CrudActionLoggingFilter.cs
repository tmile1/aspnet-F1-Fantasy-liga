using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace F1_Fantasy_liga.Infrastructure.Logging
{
    public sealed class CrudActionLoggingFilter : IAsyncActionFilter
    {
        private readonly Microsoft.Extensions.Logging.ILogger<CrudActionLoggingFilter> _logger;

        public CrudActionLoggingFilter(Microsoft.Extensions.Logging.ILogger<CrudActionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!ShouldLog(context))
            {
                await next();
                return;
            }

            var controllerName = GetControllerName(context);
            var actionName = GetActionName(context);
            var userName = GetUserName(context.HttpContext);

            _logger.LogInformation(
                "Starting action {Controller}.{Action} for {User}",
                controllerName,
                actionName,
                userName);

            var executedContext = await next();

            _logger.LogInformation(
                "Finished action {Controller}.{Action} for {User} with status {StatusCode}",
                controllerName,
                actionName,
                userName,
                executedContext.HttpContext.Response.StatusCode);
        }

        private static bool ShouldLog(ActionExecutingContext context)
        {
            var actionName = GetActionName(context);
            var method = context.HttpContext.Request.Method;

            return actionName.Equals("Create", StringComparison.OrdinalIgnoreCase)
                || actionName.Equals("Edit", StringComparison.OrdinalIgnoreCase)
                || actionName.Equals("Delete", StringComparison.OrdinalIgnoreCase)
                || HttpMethods.IsPost(method)
                || HttpMethods.IsPut(method);
        }

        private static string GetControllerName(ActionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                return controllerActionDescriptor.ControllerName;
            }

            return context.ActionDescriptor.RouteValues.TryGetValue("controller", out var controllerName)
                ? controllerName ?? "UnknownController"
                : "UnknownController";
        }

        private static string GetActionName(ActionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                return controllerActionDescriptor.ActionName;
            }

            return context.ActionDescriptor.RouteValues.TryGetValue("action", out var actionName)
                ? actionName ?? "UnknownAction"
                : "UnknownAction";
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