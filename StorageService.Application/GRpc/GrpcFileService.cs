using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using StorageService.Application.Commands;
using StorageService.Application.Enums;
using StorageService.Application.Protos;
using static StorageService.Application.Protos.GrpcFileService;

namespace StorageService.Application.GRpc;

public class GrpcFileService : GrpcFileServiceBase
{
    private readonly ISender _sender;

    public GrpcFileService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<Empty> UploadFile(IAsyncStreamReader<UploadFileRequest> requestStream, ServerCallContext context)
    {
        using var stream = new MemoryStream();
        var videoId = string.Empty;
        UploadFileType uploadType = UploadFileType.Thumbnail;
        var mimeType = string.Empty;
        var fileName = string.Empty;

        while (!context.CancellationToken.IsCancellationRequested
            && await requestStream.MoveNext())
        {
            videoId = requestStream.Current.VideoId;
            uploadType = requestStream.Current.Type;
            mimeType = requestStream.Current.MimeType;
            fileName = requestStream.Current.FileName;

            await stream.WriteAsync(requestStream.Current.Chunk.Data.ToByteArray());
        }

        stream.Seek(0, SeekOrigin.Begin);

        var command = new UploadFileCommand(stream, (UploadTypes)(int)uploadType, mimeType, videoId, fileName);
        await _sender.Send(command, context.CancellationToken);
        return await Task.FromResult(new Empty());
    }
}
