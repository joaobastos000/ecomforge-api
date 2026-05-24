using EcomForge.Application.Abstractions;
using EcomForge.Modules.AI.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace EcomForge.Modules;

public static class DependencyInjection
{
    public static IServiceCollection AddModules(this IServiceCollection services)
    {
        services.AddScoped<IEcommerceAiPlugin, EcommerceAiPlugin>();
        return services;
    }
}
