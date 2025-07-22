namespace BookingSystem.Repositories;

using BookingSystem.Contexts;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WalletRepository : Repository<string, Wallet>
{
    public WalletRepository(BookingDbContext context) : base(context)
    {
    }

    public override async Task<Wallet> Get(string key)
    {
        var wallet = await _bookingdbcontext.Wallets
            .Include(w => w.Customer)
            .SingleOrDefaultAsync(w => w.CustomerEmail == key);
        return wallet ?? throw new Exception("Wallet not found for the provided CustomerEmail.");
    }

    public override async Task<IEnumerable<Wallet>> GetAll()
    {
        var wallets = await _bookingdbcontext.Wallets
            .Include(w => w.Customer)
            .ToListAsync();

        if (wallets.Count == 0)
            throw new Exception("No Wallets in the database.");

        return wallets;
    }
}
