namespace BookingSystem.Repositories;

using BookingSystem.Contexts;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;


public class UserRepository : Repository<string, User>
{
    public UserRepository(BookingDbContext context) : base(context)
    {
    }

    public override async Task<User> Get(string key)
    {
        var user = await _bookingdbcontext.Users.SingleOrDefaultAsync(u => u.Email == key);
        return user ?? null;
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        var users = _bookingdbcontext.Users;
        if (users.Count() == 0)
            throw new Exception("No User in the database");
        return await users.ToListAsync();
    }
}