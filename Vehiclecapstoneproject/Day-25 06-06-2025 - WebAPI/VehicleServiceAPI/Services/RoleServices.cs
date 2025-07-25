using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;

namespace VehicleServiceAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Retrieves a role by its ID and converts it to a DTO.
        /// </summary>
        public async Task<RoleDTO> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return MapRoleToDto(role);
        }

        /// <summary>
        /// Retrieves all roles as a collection of DTOs.
        /// </summary>
        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => MapRoleToDto(r));
        }

        /// <summary>
        /// Creates a new role from the provided creation request DTO.
        /// </summary>
        public async Task<RoleDTO> CreateRoleAsync(string role)
        {
            var roleEntity = MapRoleCreationRequestDtoToRole(role);
            var createdRole = await _roleRepository.AddAsync(roleEntity);
            return MapRoleToDto(createdRole);
        }

        /// <summary>
        /// Updates an existing role with the provided data.
        /// </summary>
        public async Task<RoleDTO> UpdateRoleAsync(int id, string role)
        {
            var existingRole = await _roleRepository.GetByIdAsync(id);
            existingRole.RoleName = role;
            var updatedRole = await _roleRepository.UpdateAsync(existingRole);
            return MapRoleToDto(updatedRole);
        }

        /// <summary>
        /// Deletes (or removes) a role by its ID.
        /// </summary>
        public async Task<bool> DeleteRoleAsync(int id)
        {
            return await _roleRepository.DeleteAsync(id);
        }

        #region Helper Mapping Methods

        // Maps Domain Model to DTO
        private static RoleDTO MapRoleToDto(Role role)
        {
            return new RoleDTO
            {
                Id = role.Id,
                RoleName = role.RoleName
            };
        }

        // Maps Role Creation DTO to Domain Model
        private static Role MapRoleCreationRequestDtoToRole(string role)
        {
            return new Role
            {
                RoleName = role
            };
        }

        #endregion
    }
}
