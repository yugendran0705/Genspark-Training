using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceAPI.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int OwnerId { get; set; }
        public required User Owner { get; set; }

        [Required]
        public required string Make { get; set; }

        public required string Model { get; set; }

        public int Year { get; set; }

        public required string RegistrationNumber { get; set; }
        
        public  bool IsDeleted { get; set; } = false;

        // public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
