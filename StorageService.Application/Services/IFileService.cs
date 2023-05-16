using Microsoft.AspNetCore.Http;

namespace StorageService.Application.Services;

public interface IFileService
{
    public Task<string> UploadFileAsync(string id, IFormFile inputFile, CancellationToken cancellationToken = default);
}
