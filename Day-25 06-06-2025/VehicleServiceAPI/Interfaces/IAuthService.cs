using System.Threading.Tasks;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> AuthenticateUserAsync(string email, string password);

        Task<LoginResponseDTO> RefreshTokenAsync(string token);
    }
}
