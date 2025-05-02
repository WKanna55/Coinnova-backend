using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Coinnova.Application.Mappings;

public static class MapsterConfig
{
    public static void AddMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly()); // escanea todos los IRegister

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>(); // inyectable IMapper
    }
}