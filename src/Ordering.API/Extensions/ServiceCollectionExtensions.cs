using FluentValidation;
using Ordering.API.Models.Input;
using Ordering.API.Validators.Input;

namespace Ordering.API.Extensions;

/// <summary>
/// ServiceCollectionExtensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the application.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterApiDependencies(this IServiceCollection services)
    {
        services.AddScoped<IValidator<OrderInput>, OrderInputValidator>();
        services.AddScoped<IValidator<OrderItemInput>, OrderItemInputValidator>();

        return services;
    }
}
