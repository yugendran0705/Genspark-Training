namespace BookingSystem.Models.DTOs;

public class CustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public int WalletBalance { get; set; }
}