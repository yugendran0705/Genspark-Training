using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class VehicleRepository : IRepository<Vehicle>
    {
        private readonly VehicleServiceDbContext _context;

        public VehicleRepository(VehicleServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle> GetByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Owner)
                .FirstOrDefaultAsync(v => v.Id == id) ?? throw new InvalidOperationException($"Vehicle not found.");
            return vehicle;
        }

        public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(vehicle.Id) ?? throw new InvalidOperationException($"Vehicle not found.");
            
            _context.Entry(existingVehicle).CurrentValues.SetValues(vehicle);
            await _context.SaveChangesAsync();
            return existingVehicle;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return false;
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Owner)
                .ToListAsync();
        }
    }
}
