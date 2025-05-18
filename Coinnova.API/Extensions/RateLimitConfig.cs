using System.Threading.RateLimiting;
using Coinnova.API.Settings;
using Microsoft.AspNetCore.RateLimiting;

namespace Coinnova.API.Extensions;

public static class RateLimitConfig
{
    public static IServiceCollection AddRateLimitConfiguration(this IServiceCollection services)
    {
        var globalLimitSettings = new RateLimitSettings();
        globalLimitSettings.SetGlobalLimit();

        var loginLimitSettings = new RateLimitSettings();
        loginLimitSettings.SetLoginLimit();

        services.AddRateLimiter(options =>
        {
            // habilitar error 429
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
            
            // rate limiter global
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = globalLimitSettings.PermitLimit,
                        QueueLimit = globalLimitSettings.QueueLimit,
                        Window = TimeSpan.FromSeconds(globalLimitSettings.WindowSeconds)
                    }));

            //ConfigureFixedWindowLimiter(options, "GlobalFixedWindow", globalLimitSettings.WindowSeconds,
            //    globalLimitSettings.PermitLimit, globalLimitSettings.QueueLimit);
            ConfigureFixedWindowLimiter(options, "LoginFixedWindow", loginLimitSettings.WindowSeconds,
                loginLimitSettings.PermitLimit, loginLimitSettings.QueueLimit);
            
        });

        return services;
    }
    
    /*
     * Metodo para crear rate limit especifico por parametros 
     */
    private static void ConfigureFixedWindowLimiter(RateLimiterOptions options, string policyName, int windowSeconds, int permitLimit, int queueLimit)
    {
        options.AddFixedWindowLimiter(policyName, config =>
        {
            config.Window = TimeSpan.FromSeconds(windowSeconds);
            config.PermitLimit = permitLimit;
            config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            config.QueueLimit = queueLimit;
        });
    }
}