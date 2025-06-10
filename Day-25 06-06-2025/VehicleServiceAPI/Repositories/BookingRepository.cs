using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class BookingRepository
    {
        private readonly VehicleServiceDbContext _context;

        public BookingRepository(VehicleServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}