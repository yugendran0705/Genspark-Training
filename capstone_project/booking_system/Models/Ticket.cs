namespace BookingSystem.Models;
public class Ticket
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public int Quantity { get; set; }

    public int Total { get; set; } 

    public string CustomerEmail { get; set; } = string.Empty; // FK to Customer.Email
    public bool IsCancelled { get; set; }

    public DateTime BookingDate { get; set; }

    public Event? Event { get; set; }
    public Customer? User { get; set; }
}
