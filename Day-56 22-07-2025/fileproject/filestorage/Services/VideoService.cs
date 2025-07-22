namespace filestorage.services;
using filestorage.models;
using filestorage.models.DTOs;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.EntityFrameworkCore;
using filestorage.Contexts;
using Azure.Storage.Blobs.Models;



public class VideoService 
{
    private readonly TrainingDbContext _context;
    private readonly IConfiguration _config;

    public VideoService(TrainingDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<TrainingVideo> UploadVideoAsync(UploadVideoRequest request)
    {
        var containerName = "videos";
        var connectionString = _config["AzureBlob:ConnectionString"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();

        var fileName = Guid.NewGuid() + Path.GetExtension(request.VideoFile.FileName);
        var blobClient = containerClient.GetBlobClient(fileName);

        using (var stream = request.VideoFile.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders
            {     
                ContentType = "video/mp4"
            });
        }

        // Generate SAS token for blob
        var sasUri = GetSasUri(blobClient);
        Console.WriteLine($"Blob URL: {sasUri}");

        var video = new TrainingVideo
        {
            Title = request.Title,
            Description = request.Description,
            UploadDate = DateTime.UtcNow,
            BlobUrl = sasUri.ToString()
        };

        _context.TrainingVideos.Add(video);
        await _context.SaveChangesAsync();

        return video;
    }

    public async Task<List<TrainingVideo>> GetAllVideosAsync()
    {
        return await _context.TrainingVideos.ToListAsync();
    }

    private Uri GetSasUri(BlobClient blobClient)
    {
        if (!blobClient.CanGenerateSasUri)
            throw new InvalidOperationException("BlobClient must be authorized with Shared Key credentials.");

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        return blobClient.GenerateSasUri(sasBuilder);
    }
}
