using System.Text.Json;

namespace Coinnova.API.Middlewares;

public class NotFoundMiddleware
{
    private readonly RequestDelegate _next;

    public NotFoundMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        // Si aún no se manejó la respuesta (404 por ruta no encontrada)
        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            var response = new { error = "La ruta que estás buscando no existe." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}