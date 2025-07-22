namespace BookingSystem.Models.DTOs;

public class EventDto
{

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public int Price { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string[] Tags{get;set;}

    public string Context {get;set;}= string.Empty;

    public int Ticketcount {get;set;}=0;

    public string Imageurl {get;set;}= string.Empty;

    public string CategoryName { get; set; }
}