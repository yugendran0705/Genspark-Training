namespace BookingSystem.Models;

public class Wallet
{
    public int Id { get; set; }
    public int balance { get; set; }
    public string CustomerEmail { get; set; } = string.Empty; // FK to Customer.Email
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public Customer? Customer { get; set; }
}

