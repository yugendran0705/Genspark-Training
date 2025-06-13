using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleServiceAPI.Models
{
    public class ServiceSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime SlotDateTime { get; set; }

        public string Status { get; set; } = "available";

        [ForeignKey("User")]
        public int MechanicID { get; set; }
        public required User Mechanic { get; set; }
        
        public bool IsDeleted { get; set; } = false;

        // public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
