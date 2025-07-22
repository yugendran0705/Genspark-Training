namespace BookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using BookingSystem.Contexts;

public class TicketRepository : Repository<int, Ticket>
{
    private readonly BookingDbContext _bookingdbcontext;

    public TicketRepository(BookingDbContext context) : base(context)
    {
        _bookingdbcontext = context;
    }

    public override async Task<Ticket> Get(int key)
    {
        var ticket = await _bookingdbcontext.Tickets.SingleOrDefaultAsync(a => a.Id == key);
        if (ticket == null)
            throw new Exception("No Ticket with the given ID");

        return ticket;
    }

    public override async Task<IEnumerable<Ticket>> GetAll()
    {
        var  tickets=  _bookingdbcontext.Tickets.Include(t=>t.Event);
        if (tickets.Count()==0)
            throw new Exception("No Tickets in the database");

        return await tickets.ToListAsync();
    }
}
