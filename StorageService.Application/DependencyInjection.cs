using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StorageService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}
