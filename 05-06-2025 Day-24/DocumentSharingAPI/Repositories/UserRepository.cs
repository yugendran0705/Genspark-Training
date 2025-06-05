using System.Threading.Tasks;
using DocumentSharingAPI.Models;
using DocumentSharingAPI.Interfaces;
using DocumentSharingAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace DocumentSharingAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
        
        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
