namespace BookingSystem.Models;
public class Admin
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty; // FK to User

    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;

    public User? User { get; set; } // Navigational property

    public ICollection<Event>? CreatedEvents { get; set; }
}
