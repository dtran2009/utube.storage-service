using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageService.Application.Commands;
using StorageService.Application.Dto;
using StorageService.Application.Enums;

namespace StorageService.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly ISender _sender;

    public FileController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Upload file to the storage
    /// </summary>
    /// <param name="file">Video file</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(UploadFileResponse), 200)]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var command = new UploadFileCommand(file.OpenReadStream(), UploadTypes.VIDEO, file.ContentType, string.Empty);
        var response = await _sender.Send(command, cancellationToken);
        return new OkObjectResult(response);
    }
}
