namespace VehicleServiceAPI.Models.DTOs
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public required string OwnerName { get; set; }
        public required string OwnerEmail { get; set; }
        public required string OwnerPhone { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }
    }
    public class CreateVehicleDTO
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }
    }

    public class UpdateVehicleDTO
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
