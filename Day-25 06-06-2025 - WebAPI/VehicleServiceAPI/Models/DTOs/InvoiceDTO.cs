using System;

namespace VehicleServiceAPI.Models.DTOs
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public int SlotId { get; set; }
        public DateTime SlotDateTime { get; set; }
        public int MechanicID { get; set; }
        public string MechanicName { get; set; }
        public int VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public string ServiceStatus { get; set; }
        public decimal Amount { get; set; }
        public string ServiceDetails { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class CreateInvoiceDTO
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public required string ServiceDetails { get; set; }
    }

    public class UpdateInvoiceDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public required string ServiceDetails { get; set; }
    }
}
