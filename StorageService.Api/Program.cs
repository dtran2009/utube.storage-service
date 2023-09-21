using Carter;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MinimalHelpers.OpenApi;
using StorageService.Api;
using StorageService.Application;
using StorageService.Application.GRpc;
using StorageService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation(builder.Configuration)
        .AddInfrastructure(builder.Configuration)
        .AddApplication(builder.Configuration);

    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxRequestBodySize = 1 * 1024 * 1024 * 1024; // 100 MB
    });
};

var app = builder.Build();
{
    app.MapCarter();
    app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
    app.MapGrpcService<GrpcFileService>();
    app.MapGrpcReflectionService();

    app.MapPrometheusScrapingEndpoint();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors();
    app.Run();
}
