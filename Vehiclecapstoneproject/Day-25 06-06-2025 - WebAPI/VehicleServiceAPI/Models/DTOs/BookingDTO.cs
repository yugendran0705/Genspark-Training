namespace VehicleServiceAPI.Models.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int SlotId { get; set; }
        public DateTime SlotDateTime { get; set; }
        public string SlotStatus { get; set; }
        public int MechanicID { get; set; }
        public string MechanicName { get; set; }
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class CreateBookingDTO
    {
        public int SlotId { get; set; }
        public int VehicleId { get; set; }
    }

    public class UpdateBookingDTO
    {
        public string Status { get; set; }
    }

}
