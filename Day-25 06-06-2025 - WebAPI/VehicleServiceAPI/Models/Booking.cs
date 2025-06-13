using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceAPI.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public required User User { get; set; }

        [ForeignKey("ServiceSlot")]
        public int SlotId { get; set; }
        public required ServiceSlot ServiceSlot { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public required Vehicle Vehicle { get; set; }

        public string Status { get; set; } = "pending";

        public  bool IsDeleted { get; set; } = false;

        // public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
