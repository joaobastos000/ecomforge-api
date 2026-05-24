using EcomForge.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EcomForge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<AuthService>();
        services.AddScoped<ProductService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<CartService>();
        services.AddScoped<OrderService>();
        return services;
    }
}
