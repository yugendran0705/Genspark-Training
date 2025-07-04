using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;
using System.Security.Claims;

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
        /// Returns the actual image file so that the browser can view it.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID not found in token during booking creation.");
                return Unauthorized("User ID not found in token.");
            }
            _logger.LogInformation("Getting file for image with ID {ImageId}", id);
            var imageDto = await _imageService.GetImageByIdAsync(id);
            return Ok(imageDto);
        }

        [Authorize(Policy = "UserAccess")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogWarning("User ID not found in token during booking creation.");
                return Unauthorized("User ID not found in token.");
            }
            _logger.LogInformation("Getting file for image with ID {ImageId}", id);
            var imageDto = await _imageService.DeleteImageAsync(id);
            return Ok(imageDto);
        }

        /// <summary>
        /// Uploads a new image file and saves its metadata.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpPost]
        public async Task<IActionResult> UploadImage(CreateImageDTO imageUploadDto)
        {
            try
            {
                // var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                // if (userIdClaim == null)
                // {
                //     _logger.LogWarning("User ID not found in token during booking creation.");
                //     return Unauthorized("User ID not found in token.");
                // }
                // _logger.LogInformation("Saving image to database.");
                var createdImage = await _imageService.CreateImageAsync(imageUploadDto);

                return CreatedAtAction(nameof(GetImage), new { id = createdImage.Id }, createdImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving image to database.");
                return StatusCode(500, "Error saving image.");
            }
        }

        [Authorize(Policy = "UserAccess")]
        [HttpGet("booking/{id}")]
        public async Task<IActionResult> GetImageFilesByBookingId(int id)
        {
            _logger.LogInformation("Getting files for images with Booking ID {BookingId}", id);
            var images = await _imageService.GetImagesByBookingIdAsync(id);
            
            return Ok(images);
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
