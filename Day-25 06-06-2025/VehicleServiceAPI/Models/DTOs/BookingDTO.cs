namespace VehicleServiceAPI.Models.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public int VehicleId { get; set; }
    }
    public class CreateBookingDTO
    {
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public int VehicleId { get; set; }
    }

}
