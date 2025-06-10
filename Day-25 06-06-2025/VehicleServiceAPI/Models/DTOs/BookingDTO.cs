using System;

namespace VehicleServiceAPI.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public class CreateBookingDTO
        {
            public int UserId { get; set; }
            public int SlotId { get; set; }
            public int VehicleId { get; set; }
        }
    }
}
