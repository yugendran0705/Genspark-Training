using System.Threading.Tasks;
using DocumentSharingAPI.Models;

namespace DocumentSharingAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> AddUserAsync(User user);
    }
}
