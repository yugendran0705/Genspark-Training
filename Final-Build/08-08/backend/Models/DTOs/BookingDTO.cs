namespace VehicleServiceAPI.Models.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int SlotId { get; set; }
        public DateTime SlotDateTime { get; set; }
        public string SlotStatus { get; set; } = string.Empty;
        public int MechanicID { get; set; }
        public string MechanicName { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public string ServiceDetails { get; set; } = string.Empty;
    }
    public class CreateBookingDTO
    {
        public int SlotId { get; set; }
        public int VehicleId { get; set; }
    }

    public class UpdateBookingDTO
    {
        public string Status { get; set; } = string.Empty;
    }

    public class BookingServiceDetailsDTO
    {
        public int BookingId { get; set; }
        public string ServiceDetails { get; set; } = string.Empty;
    }

}
