using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDTO> GetRoleByIdAsync(int id);
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> CreateRoleAsync(string role);
        Task<RoleDTO> UpdateRoleAsync(int id, string role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
