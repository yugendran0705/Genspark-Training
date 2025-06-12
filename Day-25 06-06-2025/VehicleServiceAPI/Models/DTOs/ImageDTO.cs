using System;

namespace VehicleServiceAPI.Models.DTOs
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public int BookingId { get; set; }
        public int VehicleID { get; set; }
    }
}
