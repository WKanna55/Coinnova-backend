using System.Threading.RateLimiting;
using Coinnova.API.Settings;
using Microsoft.AspNetCore.RateLimiting;

namespace Coinnova.API.Extensions;

public static class RateLimitConfig
{
    public static IServiceCollection AddRateLimitConfiguration(this IServiceCollection services)
    {
        var rateLimitSettings = new RateLimitSettings();

        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryafter))
                {
                    await context.HttpContext.Response.WriteAsync(
                        $"Too many request. Please try again later {retryafter.TotalSeconds} seconds",
                        cancellationToken: token);
                }
                else
                {
                    await context.HttpContext.Response.WriteAsync("Too many request. Please try later. ",
                        cancellationToken: token);
                }
            };
            options.AddFixedWindowLimiter("FixedWindowPolicy", config =>
            {
                
                config.Window = TimeSpan.FromSeconds(rateLimitSettings.WindowSeconds);
                config.PermitLimit = rateLimitSettings.PermitLimit;
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = rateLimitSettings.QueueLimit;
            });
        });

        return services;
    }
}