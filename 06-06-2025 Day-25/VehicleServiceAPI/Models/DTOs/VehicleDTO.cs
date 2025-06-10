namespace VehicleServiceAPI.DTOs
{
    public class VehicleDTO
    {
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }

        public class CreateVehicleDTO
        {
            public int UserId { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public string RegistrationNumber { get; set; }
        }
    }
}
