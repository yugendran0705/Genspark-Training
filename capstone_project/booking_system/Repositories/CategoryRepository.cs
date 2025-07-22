namespace BookingSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using BookingSystem.Contexts;

public class CategoryRepository : Repository<string, Category>
{
    private readonly BookingDbContext _bookingdbcontext;

    public CategoryRepository(BookingDbContext context) : base(context)
    {
        _bookingdbcontext = context;
    }

    public override async Task<Category> Get(string key)
    {
        var events = await _bookingdbcontext.Categories.SingleOrDefaultAsync(c => c.Name == key);

        return events ;
    }

    public override async Task<IEnumerable<Category>> GetAll()
    {
        var events = await _bookingdbcontext.Categories.ToListAsync();
        if (!events.Any())
            throw new Exception("No Customer in the database");

        return events;
    }
}