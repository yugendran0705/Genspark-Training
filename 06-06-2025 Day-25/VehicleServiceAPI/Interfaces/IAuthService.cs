using System.Threading.Tasks;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateUser(string email, string password);

        Task<string> RefreshToken(string token);
    }
}
