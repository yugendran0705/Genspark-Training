using Microsoft.AspNetCore.Mvc;
using System.IO;
using FirstApi.Interfaces;
using FirstApi.Services;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly string _basePath = "local_filestorage";
    private readonly IFileService fs;

    public FileController(IFileService fileService)
    {
        fs = fileService;
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    [HttpPost("create")]
    public IActionResult CreateFile(string fileName, [FromBody] string content)
    {
        var path = Path.Combine(_basePath, fileName);
        if (System.IO.File.Exists(path))
            return Conflict("File already exists.");
        fs.CreateFile(path, content);
        return Ok("File created.");
    }

    
    [HttpGet("read")]
    public IActionResult ReadFile(string fileName)
    {
        var path = Path.Combine(_basePath, fileName);
        if (!System.IO.File.Exists(path))
            return NotFound("File not found.");
        var content = fs.ReadFile(path);
        return Ok(content);
    }

    
}