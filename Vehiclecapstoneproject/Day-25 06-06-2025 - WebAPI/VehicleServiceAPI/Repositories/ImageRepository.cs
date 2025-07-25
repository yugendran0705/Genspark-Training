using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class ImageRepository : IRepository<Image>
    {
        private readonly VehicleServiceDbContext _context;

        public ImageRepository(VehicleServiceDbContext context)
        {
            _context = context;
        }

        // Retrieve an image by ID, including associated booking and vehicle
        public async Task<Image> GetByIdAsync(int id)
        {
            var image = await _context.Images
                .Include(i => i.Booking)
                .Include(i => i.Vehicle)
                .FirstOrDefaultAsync(i => i.Id == id) ?? throw new InvalidOperationException($"Image not found.");
            return image;
        }

        // Retrieve all images, ensuring soft-deleted entries are excluded
        public async Task<IEnumerable<Image>> GetAllAsync()
        {
            return await _context.Images
                .Include(i => i.Booking)
                .Include(i => i.Vehicle)
                .ToListAsync();
        }

        // Add a new image record
        public async Task<Image> AddAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        // Update an existing image record
        public async Task<Image> UpdateAsync(Image image)
        {
            var existingImage = await _context.Images.FindAsync(image.Id) ?? throw new InvalidOperationException($"Image not found.");
            existingImage.Base64Data = image.Base64Data;
            existingImage.BookingId = image.BookingId;
            existingImage.VehicleId = image.VehicleId;

            _context.Images.Update(existingImage);
            await _context.SaveChangesAsync();
            return existingImage;
        }

        // Soft delete an image record
        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return false;
            }

            image.IsDeleted = true;
            _context.Images.Update(image);
            await _context.SaveChangesAsync();
            return true;
        }

        // Retrieve all images associated with a specific booking
        public async Task<IEnumerable<Image>> GetImagesByBookingIdAsync(int bookingId)
        {
            return await _context.Images
                .Where(i => i.BookingId == bookingId && !i.IsDeleted)
                .ToListAsync();
        }

        // Retrieve all images related to a specific vehicle
        public async Task<IEnumerable<Image>> GetImagesByVehicleIdAsync(int vehicleId)
        {
            return await _context.Images
                .Where(i => i.VehicleId == vehicleId && !i.IsDeleted)
                .ToListAsync();
        }
    }
}
