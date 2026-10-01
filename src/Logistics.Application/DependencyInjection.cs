using Microsoft.Extensions.DependencyInjection;

namespace Logistics.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Automatically scans the current assembly to find and wire up all MediatR Handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
