namespace BookingSystem.Models;
public class Customer
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty; // FK to User

    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;

    public string? Address { get; set; } = string.Empty;
    public Wallet? Wallet { get; set; } // Navigation property to Wallet
    public User? User { get; set; }

    public ICollection<Ticket>? Tickets { get; set; }
}
