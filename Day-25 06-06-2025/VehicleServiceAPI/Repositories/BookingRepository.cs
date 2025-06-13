using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private readonly VehicleServiceDbContext _context;

        public BookingRepository(VehicleServiceDbContext context)
        {
            _context = context;
        }

        // Retrieve a booking by ID, including foreign key relationships
        public async Task<Booking> GetByIdAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.ServiceSlot)
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(b => b.Id == id) ?? throw new InvalidOperationException($"Booking not found.");
            return booking;
        }

        // Retrieve all bookings, including related data
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.ServiceSlot)
                .Include(b => b.Vehicle)
                .ToListAsync();
        }

        // Create a new booking record
        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        // Update an existing booking record
        public async Task<Booking> UpdateAsync(Booking booking)
        {
            var existingBooking = await _context.Bookings.FindAsync(booking.Id) ?? throw new InvalidOperationException($"Booking not found.");
            existingBooking.Status = booking.Status;
            existingBooking.SlotId = booking.SlotId;
            existingBooking.VehicleId = booking.VehicleId;

            _context.Bookings.Update(existingBooking);
            await _context.SaveChangesAsync();
            return existingBooking;
        }

        // Soft delete a booking
        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return false;
            }

            booking.IsDeleted = true; // Soft delete
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        // Retrieve all bookings made by a specific user
        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.ServiceSlot)
                .Include(b => b.Vehicle)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        // Retrieve all bookings by status (e.g., "pending", "completed")
        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.ServiceSlot)
                .Include(b => b.Vehicle)
                .Where(b => b.Status == status)
                .ToListAsync();
        }
    }
}
