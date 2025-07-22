using Microsoft.AspNetCore.Mvc;
using filestorage.services;
using filestorage.models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class TrainingVideoController : ControllerBase
{
    private readonly VideoService _videoService;

    public TrainingVideoController(VideoService videoService)
    {
        _videoService = videoService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadVideoRequest request)
    {
        var result = await _videoService.UploadVideoAsync(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var videos = await _videoService.GetAllVideosAsync();
        return Ok(videos);
    }
}
