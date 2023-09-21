using Carter;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using MinimalHelpers.OpenApi;
using OpenTelemetry.Metrics;

namespace StorageService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCarter();


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddMissingSchemas();
        });

        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
        });

        services.AddGrpcReflection();

        services.AddAntiforgery();

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
