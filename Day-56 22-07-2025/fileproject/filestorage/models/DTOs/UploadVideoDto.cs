namespace filestorage.models.DTOs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class UploadVideoRequest
{
    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required]
    public IFormFile VideoFile { get; set; }
}
