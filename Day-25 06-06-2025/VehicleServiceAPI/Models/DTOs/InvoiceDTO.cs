using System;

namespace VehicleServiceAPI.DTOs
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string ServiceDetails { get; set; }
        // public DateTime CreatedAt { get; set; }
    }
}
