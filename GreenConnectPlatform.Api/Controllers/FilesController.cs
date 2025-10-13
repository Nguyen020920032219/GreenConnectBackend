using GreenConnectPlatform.Bussiness.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;

    public FilesController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // Tạo một tên file duy nhất để tránh trùng lặp
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        await using var stream = file.OpenReadStream();

        var downloadUrl = await _fileStorageService.UploadFileAsync(
            stream,
            fileName,
            file.ContentType
        );

        return Ok(new { Url = downloadUrl });
    }
}