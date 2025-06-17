using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly ILogger<ImageController> _logger;
        // We assume that images are saved under wwwroot/uploads.
        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ImageController(IImageService imageService, ILogger<ImageController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        /// <summary>
        /// Returns image metadata (without file content).
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageMetadata(int id)
        {
            _logger.LogInformation("Getting metadata for image with ID {ImageId}", id);
            var imageDto = await _imageService.GetImageByIdAsync(id);
            if (imageDto == null)
            {
                _logger.LogWarning("Image with ID {ImageId} not found.", id);
                return NotFound("Image not found.");
            }
            // Return metadata without the file stream.
            _logger.LogInformation("Successfully retrieved metadata for image with ID {ImageId}", id);
            return Ok(new
            {
                imageDto.Id,
                imageDto.BookingId,
                VehicleID = imageDto.VehicleID,
                imageDto.RegistrationNumber,
                FileName = imageDto.File?.FileName
            });
        }
        
        /// <summary>
        /// Returns the actual image file so that the browser can view it.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetImageFile(int id)
        {
            _logger.LogInformation("Getting file for image with ID {ImageId}", id);
            var imageDto = await _imageService.GetImageByIdAsync(id);
            if (imageDto == null || imageDto.File == null)
            {
                _logger.LogWarning("Image or file for image ID {ImageId} not found.", id);
                return NotFound("Image not found.");
            }
            
            var fullPath = Path.Combine(_uploadsFolder, imageDto.File.FileName);
            if (!System.IO.File.Exists(fullPath))
            {
                _logger.LogWarning("File {FileName} for image ID {ImageId} not found on disk.", imageDto.File.FileName, id);
                return NotFound("File not found on disk.");
            }

            var contentType = GetContentType(fullPath);
            _logger.LogInformation("Returning file {FileName} for image ID {ImageId} with content type {ContentType}", imageDto.File.FileName, id, contentType);
            return PhysicalFile(fullPath, contentType);
        }

        /// <summary>
        /// Uploads a new image file and saves its metadata.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] CreateImageDTO imageUploadDto)
        {
            if (imageUploadDto.File == null || imageUploadDto.File.Length == 0)
            {
                _logger.LogWarning("No file uploaded in the request.");
                return BadRequest("No file uploaded.");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageUploadDto.File.FileName);
            var filePath = Path.Combine(_uploadsFolder, fileName);

            try
            {
                _logger.LogInformation("Saving uploaded file to {FilePath}", filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageUploadDto.File.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file to disk at {FilePath}", filePath);
                return StatusCode(500, "Error saving file.");
            }

            try
            {
                _logger.LogInformation("Saving image metadata to database.");
                var createdImage = await _imageService.CreateImageAsync(imageUploadDto);

                _logger.LogInformation("Image uploaded successfully with ID {ImageId}", createdImage.Id);
                return CreatedAtAction(nameof(GetImageMetadata), new { id = createdImage.Id }, new
                {
                    createdImage.Id,
                    createdImage.BookingId,
                    createdImage.VehicleID,
                    createdImage.RegistrationNumber,
                    FileName = fileName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving image metadata to database.");
                // Optionally, delete the file if metadata save fails
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return StatusCode(500, "Error saving image metadata.");
            }
        }
        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path).ToLowerInvariant();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
