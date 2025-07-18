using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceAPI.Models
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public required Booking Booking { get; set; }
        
        [Required]
        public decimal Amount { get; set; }

        public bool DiscountFlag { get; set; } = false;

        public int DiscountPercentage { get; set; } = 0;

        public required string ServiceDetails { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        
        // public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
