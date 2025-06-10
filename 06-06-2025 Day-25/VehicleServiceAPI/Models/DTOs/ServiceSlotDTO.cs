using System;

namespace VehicleServiceAPI.DTOs
{
    public class ServiceSlotDTO
    {
        public int SlotId { get; set; }
        public DateTime SlotDateTime { get; set; }
        public string Status { get; set; }
    }
}
