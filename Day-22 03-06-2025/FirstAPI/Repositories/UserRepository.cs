namespace FirstApi.Repositories;

using FirstApi.Interfaces;
using FirstApi.Contexts;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
public class UserRepository : Repository<string, User>
{
    public UserRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<User> Get(string key)
    {
        return await _clinicContext.Users.SingleOrDefaultAsync(u => u.Username == key);
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        return await _clinicContext.Users.ToListAsync();
    }


}