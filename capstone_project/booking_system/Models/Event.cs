namespace BookingSystem.Models;
public class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }
    public int Price { get; set; }

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Context {get;set;}= string.Empty;

    public int Ticketcount {get;set;}=0;


    public string Imageurl {get;set;}= string.Empty;

    public int CategoryId { get; set; }
    public bool IsCancelled { get; set; }

    public string CreatorEmail { get; set; } = string.Empty; // FK to Admin.Email
    public Admin? CreatedBy { get; set; }

    public Category? Category { get; set; }
    public ICollection<Ticket>? Tickets { get; set; }
}
