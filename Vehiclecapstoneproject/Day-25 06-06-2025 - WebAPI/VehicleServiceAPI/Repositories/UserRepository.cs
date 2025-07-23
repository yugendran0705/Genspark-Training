using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Repositories
{
    public class UserRepository(VehicleServiceDbContext context) : IRepository<User>
    {
        private readonly VehicleServiceDbContext _context = context;

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id) ?? throw new InvalidOperationException($"User not found.");
            return user;
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id) ?? throw new InvalidOperationException($"User not found.");
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.RoleId = user.RoleId;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true; // Soft delete
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<User> GetAdminAsync()
        {
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
            var adminUser = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.RoleId == adminRole.Id && !u.IsDeleted);
            return adminUser;
        }

        public async Task<IEnumerable<User>> GetMechanicsAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.RoleId == 2 && !u.IsDeleted)
                .ToListAsync();
        }
    }
}
