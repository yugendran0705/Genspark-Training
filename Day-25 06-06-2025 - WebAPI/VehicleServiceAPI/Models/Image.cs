using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceAPI.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public required string FilePath { get; set; }
        
        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public required Vehicle Vehicle { get; set; }
        
        public bool IsDeleted { get; set; } = false;

        // public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
