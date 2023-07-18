using Microsoft.AspNetCore.Server.Kestrel.Core;
using StorageService.Api;
using StorageService.Application;
using StorageService.Application.GRpc;
using StorageService.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerUI;

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
    app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
    app.MapGrpcService<GrpcFileService>();
    app.MapGrpcReflectionService();

    app.MapPrometheusScrapingEndpoint();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Storage Service API V1");
        c.RoutePrefix = string.Empty;
        c.DocExpansion(DocExpansion.List);
    });

    app.MapControllers();
    app.UseCors();

    app.Run();
}
