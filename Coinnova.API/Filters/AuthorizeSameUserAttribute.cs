using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Coinnova.API.Filters;

public sealed class AuthorizeSameUserAttribute : Attribute, IAsyncAuthorizationFilter
{
    public string RouteIdName { get; init; } = "id";
    public string ClaimType   { get; init; } = ClaimTypes.NameIdentifier;
    public string[] ExemptRoles { get; init; } = Array.Empty<string>();

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // 1. Autenticación
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // 2. Roles exentos
        if (ExemptRoles.Any(r => user.IsInRole(r)))
            return;

        // 3. Claim de usuario
        var claimUserId = user.FindFirstValue(ClaimType);
        if (string.IsNullOrWhiteSpace(claimUserId))
        {
            context.Result = new ForbidResult();
            return;
        }

        // 4. ID de la ruta
        if (!context.RouteData.Values.TryGetValue(RouteIdName, out var routeValue) ||
            routeValue is null)
        {
            context.Result = new ForbidResult();
            return;
        }

        if (claimUserId != routeValue.ToString())
        {
            context.Result = new ForbidResult();
            return;
        }

        await Task.CompletedTask;
    }
}