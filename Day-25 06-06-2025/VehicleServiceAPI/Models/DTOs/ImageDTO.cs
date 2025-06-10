using System;

namespace VehicleServiceAPI.DTOs
{
    public class ImageDTO
    {
        public int ImageId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
