using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class ServiceSlotRepository : IRepository<ServiceSlot>
    {
        private readonly VehicleServiceDbContext _context;

        public ServiceSlotRepository(VehicleServiceDbContext context)
        {
            _context = context;
        }

        // Retrieve a service slot by ID, including the associated mechanic (User)
        public async Task<ServiceSlot> GetByIdAsync(int id)
        {
            var serviceSlot = await _context.ServiceSlots
                .Include(s => s.Mechanic)
                .FirstOrDefaultAsync(s => s.Id == id) ?? throw new InvalidOperationException($"Service Slot not found.");
            return serviceSlot;
        }

        // Retrieve all service slots, ensuring soft-deleted entries are excluded
        public async Task<IEnumerable<ServiceSlot>> GetAllAsync()
        {
            return await _context.ServiceSlots
                .Include(s => s.Mechanic)
                .ToListAsync();
        }

        // Add a new service slot record
        public async Task<ServiceSlot> AddAsync(ServiceSlot serviceSlot)
        {
            _context.ServiceSlots.Add(serviceSlot);
            await _context.SaveChangesAsync();
            return serviceSlot;
        }

        // Update an existing service slot record
        public async Task<ServiceSlot> UpdateAsync(ServiceSlot serviceSlot)
        {
            var existingSlot = await _context.ServiceSlots.FindAsync(serviceSlot.Id) ?? throw new InvalidOperationException($"Service Slot not found.");
            existingSlot.SlotDateTime = serviceSlot.SlotDateTime;
            existingSlot.Status = serviceSlot.Status;
            existingSlot.MechanicID = serviceSlot.MechanicID;

            _context.ServiceSlots.Update(existingSlot);
            await _context.SaveChangesAsync();
            return existingSlot;
        }

        // Soft delete a service slot
        public async Task<bool> DeleteAsync(int id)
        {
            var serviceSlot = await _context.ServiceSlots.FindAsync(id);
            if (serviceSlot == null)
            {
                return false;
            }

            serviceSlot.IsDeleted = true;
            _context.ServiceSlots.Update(serviceSlot);
            await _context.SaveChangesAsync();
            return true;
        }

        // Retrieve all available service slots (excluding booked or deleted ones)
        public async Task<IEnumerable<ServiceSlot>> GetAvailableSlotsAsync()
        {
            return await _context.ServiceSlots
                .Include(s => s.Mechanic)
                .Where(s => s.Status == "available" && !s.IsDeleted)
                .ToListAsync();
        }

        // Retrieve all service slots assigned to a specific mechanic
        public async Task<IEnumerable<ServiceSlot>> GetSlotsByMechanicIdAsync(int mechanicId)
        {
            return await _context.ServiceSlots
                .Where(s => s.MechanicID == mechanicId && !s.IsDeleted)
                .ToListAsync();
        }
    }
}
