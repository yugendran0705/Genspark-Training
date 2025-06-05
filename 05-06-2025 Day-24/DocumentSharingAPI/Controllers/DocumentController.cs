using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DocumentSharingAPI.Models;
using DocumentSharingAPI.Models.Dtos;
using DocumentSharingAPI.Interfaces;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace DocumentSharingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IWebHostEnvironment _env;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(INotificationService notificationService,
                                  IWebHostEnvironment env,
                                  IDocumentRepository documentRepository,
                                  ILogger<DocumentController> logger)
        {
            _notificationService = notificationService;
            _env = env;
            _documentRepository = documentRepository;
            _logger = logger;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "HRAdmin")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadDto uploadDto)
        {
            if (uploadDto.File == null || uploadDto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Ensure the uploads directory exists
            var uploadsPath = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Sanitize the filename for security purposes
            var fileName = Path.GetFileName(uploadDto.File.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadDto.File.CopyToAsync(stream);
            }

            // Create a document record
            var document = new Document
            {
                FileName = fileName,
                FilePath = filePath,
                UploadedAt = DateTime.UtcNow
            };

            await _documentRepository.AddDocumentAsync(document);
            await _notificationService.NotifyDocumentUploadAsync(document);

            return Ok(new { message = "File uploaded and notification sent." });
        }
    }
}
