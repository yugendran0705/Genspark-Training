using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class VehicleRepository
    {
        private readonly VehicleServiceDbContext _context;

        public VehicleRepository(VehicleServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles
                .Include(v => v.Owner)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle?> UpdateVehicleAsync(Vehicle vehicle)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(vehicle.Id);
            if (existingVehicle == null)
            {
                return null;
            }

            _context.Entry(existingVehicle).CurrentValues.SetValues(vehicle);
            await _context.SaveChangesAsync();
            return existingVehicle;
        }

        public async Task<bool> DeleteVehicleAsync(int id)
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
        
        public async Task<List<Vehicle>> GetAllVehicleAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Owner)
                .ToListAsync();
        }
    }
}
