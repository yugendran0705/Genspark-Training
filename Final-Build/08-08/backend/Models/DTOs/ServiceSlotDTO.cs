using System;

namespace VehicleServiceAPI.Models.DTOs
{
    public class ServiceSlotDTO
    {
        public int Id { get; set; }
        public DateTime SlotDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public int MechanicID { get; set; }
        public string MechanicName { get; set; } = string.Empty;
        public bool isDeleted { get; set; }
    }

    public class CreateServiceSlotDTO
    {
        public DateTime SlotDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public int MechanicID { get; set; }
    }

    public class UpdateServiceSlotDTO
    {
        public int Id { get; set; }
        public DateTime SlotDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public int MechanicID { get; set; }
    }
}
