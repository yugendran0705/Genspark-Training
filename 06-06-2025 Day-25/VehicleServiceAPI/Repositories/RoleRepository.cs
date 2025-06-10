using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class RoleRepository(VehicleServiceDbContext context)
    {
        private readonly VehicleServiceDbContext _context = context;

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return false;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}