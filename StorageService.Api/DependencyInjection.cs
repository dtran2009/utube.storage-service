using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;

namespace StorageService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "UTube.StorageService", Version = "v1" });
        });

        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
        });

        services.AddGrpcReflection();

        services.AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter(config =>
                {
                    config.ScrapeResponseCacheDurationMilliseconds = 1000;
                }));

        return services;
    }
}
