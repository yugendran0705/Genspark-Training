namespace BookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using BookingSystem.Contexts;

public class CustomerRepository : Repository<string, Customer>
{
    private readonly BookingDbContext _bookingdbcontext;

    public CustomerRepository(BookingDbContext context) : base(context)
    {
        _bookingdbcontext = context;
    }

    public override async Task<Customer> Get(string key)
    {
        var customer = await _bookingdbcontext.Customers.Include(c=>c.Tickets).SingleOrDefaultAsync(c => c.Email == key);

        return customer ?? throw new Exception("No Customer with the given mail ID");
    }

    public override async Task<IEnumerable<Customer>> GetAll()
    {
        var customers = await _bookingdbcontext.Customers.ToListAsync();
        if (!customers.Any())
            throw new Exception("No Customer in the database");

        return customers;
    }
}
