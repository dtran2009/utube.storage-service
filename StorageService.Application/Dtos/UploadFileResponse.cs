namespace StorageService.Application.Dto;

public record UploadFileResponse(
    string videoId, 
    string objectPath, 
    string objectUrl);