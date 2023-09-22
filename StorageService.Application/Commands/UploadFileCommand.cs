using MassTransit;
using MediatR;
using MimeTypes;
using StorageService.Application.Dto;
using StorageService.Application.Enums;
using StorageService.Application.Services;
using StorageService.Application.Setting;
using UTube.Common.Events;

namespace StorageService.Application.Commands;

public record UploadFileCommand(
    Stream stream,
    UploadTypes types,
    string mimeType,
    string videoId,
    string? fileName = null) : IRequest<UploadFileResponse>;


public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileResponse>
{
    private readonly IBus _bus;
    private readonly IFileService _fileService;
    private readonly StorageSetting _storageSetting;

    public UploadFileCommandHandler(IFileService fileService, IBus bus, StorageSetting storageSetting)
    {
        _fileService = fileService;
        _bus = bus;
        _storageSetting = storageSetting;
    }


    public async Task<UploadFileResponse> Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        var videoId = command.types == UploadTypes.VIDEO ?
            Guid.NewGuid().ToString().ToLower() : command.videoId;

        string fileName = GenerateFileName(videoId, command.types, command.fileName, command.mimeType);

        var objectPath = await _fileService.UploadFileAsync(videoId, command.stream, fileName, command.mimeType, cancellationToken);
        var objectUrl = _storageSetting.GetObjectUrl(objectPath);

        if (command.types == UploadTypes.VIDEO)
        {
            await _bus.Publish(new VideoUploadedEvent(videoId, objectPath, objectUrl)).ConfigureAwait(false);
        }
        
        command.stream.Dispose();
        var response = new UploadFileResponse(videoId, objectPath, _storageSetting.GetObjectUrl(objectPath));
        return await Task.FromResult(response);
    }

    private string GenerateFileName(string videoId, UploadTypes types, string? fileName, string mimeType)
    {
        if (types == UploadTypes.THUMBNAIL)
        {
            fileName = Guid.NewGuid() + MimeTypeMap.GetExtension(mimeType);
        }
        else
        {
            fileName = string.IsNullOrEmpty(fileName) ? videoId + MimeTypeMap.GetExtension(mimeType) : fileName;
        }

        return $"{videoId}/" + types switch
        {
            UploadTypes.VIDEO => $"",
            UploadTypes.THUMBNAIL => $"thumbnail/",
            UploadTypes.FHD_1080 => $"FHD/",
            UploadTypes.HD_720 => $"HD/",
            UploadTypes.SD_480 => $"SD/",
            _ => $"temp/"
        } + fileName;
    }
}
