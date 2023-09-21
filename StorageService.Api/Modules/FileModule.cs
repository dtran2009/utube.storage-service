using Carter;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using StorageService.Application.Commands;
using StorageService.Application.Enums;

namespace StorageService.Api.Controllers;

public class FileModule : CarterModule
{
    public FileModule() : base("/api/files")
    {
        WithTags("Files");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/upload", async (IFormFile file, CancellationToken cancellationToken, [FromServices] ISender sender, [FromServices] IAntiforgery antiforgery) =>
        {
            var command = new UploadFileCommand(file.OpenReadStream(), UploadTypes.VIDEO, file.ContentType, string.Empty);
            var response = await sender.Send(command, cancellationToken);
            return TypedResults.Ok(response);
        })
            .DisableAntiforgery()
            .WithOpenApi(o =>
            {
                o.Description = "API to upload MP4 file";
                return o;
            });
    }
}
