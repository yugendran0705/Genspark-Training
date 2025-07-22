namespace BookingSystem.Interfaces;
using BookingSystem.Models;
using BookingSystem.Models.DTOs;
public interface ITicketService
{
    public Task<Ticket> BookTicket(TicketDto ticket);
    public Task<Ticket> GetTicketById(int id);
    public Task<Ticket> CancelTicketById(int id);
    public Task<IEnumerable<Ticket>> GetTicketByUser(string email);
}