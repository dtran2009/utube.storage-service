using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageService.Application.Commands;
using StorageService.Application.Services;

namespace StorageService.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly ISender _sender;

    public FileController(IFileService fileService, ISender sender)
    {
        _fileService = fileService;
        _sender = sender;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync([FromForm]UploadFileCommand command, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(command, cancellationToken);
        return new OkObjectResult(response);
    }
}
