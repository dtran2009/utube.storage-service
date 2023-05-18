using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StorageService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddGrpc();

        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddCors(o => o.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
        }));

        return services;
    }
}
