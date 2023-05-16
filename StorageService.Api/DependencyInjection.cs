using Microsoft.AspNetCore.Http.Features;

namespace StorageService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddControllers();

        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
        });

        return services;
    }
}
