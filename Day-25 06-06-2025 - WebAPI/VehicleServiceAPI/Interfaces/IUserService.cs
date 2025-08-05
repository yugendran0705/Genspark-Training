using System.Threading.Tasks;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> CreateUserAsync(UserCreationRequestDTO userDto);
        Task<UserDTO> UpdateUserAsync(int id, UserUpdateRequestDTO userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<IEnumerable<UserDTO>> GetAllMechanicsAsync();
        Task<UserDTO> UpdateUserRoleAsync(UpdateUserRequestDTO updateRequest);
    }
}
