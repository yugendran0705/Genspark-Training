namespace BookingSystem.Models.DTOs;

public class TicketDto
{
    public string EventName { get; set; }
    public int Quantity { get; set; }
    public bool UseWallet { get; set; } = false;
}