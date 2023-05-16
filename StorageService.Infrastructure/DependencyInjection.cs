using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using StorageService.Application.Services;
using StorageService.Infrastructure.Services;
using StorageService.Infrastructure.Settings;

namespace StorageService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        #region Minio

        var minioSetting = new MinioSetting();
        configuration.GetSection(nameof(MinioSetting)).Bind(minioSetting);
        services.AddSingleton(minioSetting);

        services.AddSingleton<MinioClient>(_ =>
        {
            var minioClient = new MinioClient()
            .WithEndpoint(minioSetting.Endpoint)
            .WithCredentials(minioSetting.AccessKey, minioSetting.Secretkey)
            .WithSSL(minioSetting.UseSSL)
            .Build();
            minioClient.SetTraceOn();
            return minioClient;
        });

        #endregion

        #region MassTransit

        var rabbitMQSetting = new RabbitMQSetting();
        configuration.GetSection(nameof(RabbitMQSetting)).Bind(rabbitMQSetting);
        services.AddSingleton(rabbitMQSetting);

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMQSetting.Endpoint, rabbitMQSetting.VirtualHost, h =>
                {
                    h.Username(rabbitMQSetting.Username);
                    h.Password(rabbitMQSetting.Password);
                });
            });
        });

        #endregion

        services.AddTransient<IFileService, MinioFileService>();

        return services;
    }
}
