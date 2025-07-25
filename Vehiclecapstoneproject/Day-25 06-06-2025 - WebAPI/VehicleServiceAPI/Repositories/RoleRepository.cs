using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Interfaces;

namespace VehicleServiceAPI.Repositories
{
    public class RoleRepository(VehicleServiceDbContext context) : IRepository<Role>
    {
        private readonly VehicleServiceDbContext _context = context;

        public async Task<Role> GetByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id) ?? throw new InvalidOperationException($"Role not found.");
            return role;
        }

        public async Task<Role> AddAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteAsync(int id)
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

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var existingRole = await _context.Roles.FindAsync(role.Id) ?? throw new InvalidOperationException($"Role not found.");
            existingRole.RoleName = role.RoleName;
            _context.Roles.Update(existingRole);
            await _context.SaveChangesAsync();
            return existingRole;
        }
    }
}