using Microsoft.AspNetCore.Http;

namespace DocumentSharingAPI.Models.Dtos
{
    public class DocumentUploadDto
    {
        public IFormFile File { get; set; }
    }
}
