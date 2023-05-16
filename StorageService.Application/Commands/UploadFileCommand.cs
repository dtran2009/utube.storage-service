using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using UTube.Common.Events;
using StorageService.Application.Dto;
using StorageService.Application.Services;

namespace StorageService.Application.Commands;

public record UploadFileCommand(IFormFile inputFile) : IRequest<UploadFileResponse>;


public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileResponse>
{
    private readonly IFileService _fileService;
    private readonly IBus _bus;

    public UploadFileCommandHandler(IFileService fileService, IBus bus)
    {
        _fileService = fileService;
        _bus = bus;
    }

    public async Task<UploadFileResponse> Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid().ToString().ToLower();
        var filePath = await _fileService.UploadFileAsync(id, command.inputFile, cancellationToken);

        await _bus.Publish(new VideoUploadedEvent(id, filePath)).ConfigureAwait(false);

        return await Task.FromResult(new UploadFileResponse(id, filePath));
    }
}
