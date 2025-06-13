using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        // We assume that images are saved under wwwroot/uploads.
        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Returns image metadata (without file content).
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageMetadata(int id)
        {
            var imageDto = await _imageService.GetImageByIdAsync(id);
            if (imageDto == null)
            {
                return NotFound("Image not found.");
            }
            // Return metadata without the file stream.
            return Ok(new
            {
                imageDto.Id,
                imageDto.BookingId,
                VehicleID = imageDto.VehicleID,
                imageDto.RegistrationNumber,
                // For debugging you may also send the FileName from the IFormFile.
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
            var imageDto = await _imageService.GetImageByIdAsync(id);
            if (imageDto == null || imageDto.File == null)
            {
                return NotFound("Image not found.");
            }
            
            // Reconstruct the full physical path.
            // Note: imageDto.File.FileName contains the generated file name (e.g. "abc123.jpg").
            var fullPath = Path.Combine(_uploadsFolder, imageDto.File.FileName);
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("File not found on disk.");
            }

            // Determine the MIME type based on the file extension.
            var contentType = GetContentType(fullPath);
            
            // Return the file as a PhysicalFileResult.
            return PhysicalFile(fullPath, contentType);
        }

        /// <summary>
        /// Helper method to determine MIME type based on file extension.
        /// </summary>
        
        /// <summary>
        /// Uploads a new image file and saves its metadata.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] CreateImageDTO imageUploadDto)
        {
            if (imageUploadDto.File == null || imageUploadDto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageUploadDto.File.FileName);
            var filePath = Path.Combine(_uploadsFolder, fileName);

            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageUploadDto.File.CopyToAsync(stream);
            }

            // Save metadata using the service
            var createdImage = await _imageService.CreateImageAsync(imageUploadDto);

            return CreatedAtAction(nameof(GetImageMetadata), new { id = createdImage.Id }, new
            {
                createdImage.Id,
                createdImage.BookingId,
                createdImage.VehicleID,
                createdImage.RegistrationNumber,
                FileName = fileName
            });
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
