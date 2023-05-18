namespace StorageService.Application.Services;

public interface IFileService
{
    public Task<string> UploadFileAsync(string id, Stream stream, string filename, string mimeType, CancellationToken cancellationToken = default);
}
