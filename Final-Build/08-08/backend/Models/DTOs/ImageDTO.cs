using System;

namespace VehicleServiceAPI.Models.DTOs
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int VehicleId { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Base64Data { get; set; }  = string.Empty;
    }

    public class CreateImageDTO
    {
        public string Base64Data { get; set; }  = string.Empty;
        public int BookingId { get; set; }
        public int VehicleID { get; set; }
    }
    
    public class UpdateImageDTO
    {
        public int Id { get; set; }   
        public string Base64Data { get; set; }  = string.Empty;
        public int BookingId { get; set; }
        public int VehicleID { get; set; }
    }
}
