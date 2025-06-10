using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Interfaces;

namespace VehicleServiceAPI.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private readonly VehicleServiceDbContext _context;

        public BookingRepository(VehicleServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id)
                ?? throw new InvalidOperationException($"Booking not found.");
        }
    }

    public interface IRepository
    {
    }
}