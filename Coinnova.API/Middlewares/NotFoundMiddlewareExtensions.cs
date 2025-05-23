namespace Coinnova.API.Middlewares;

public static class NotFoundMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomNotFound(this IApplicationBuilder app)
    {
        return app.UseMiddleware<NotFoundMiddleware>();
    }
}