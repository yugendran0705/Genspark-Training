using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;
using VehicleServiceAPI.Utils;

namespace VehicleServiceAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        public UserService(UserRepository userRepository, RoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return MapUserToDto(user);
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return MapUserToDto(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserCreationRequestDTO user)
        {
            var userEntity = await MapUserCreationRequestDtoToUser(user);
            var createdUser = await _userRepository.AddAsync(userEntity);
            return MapUserToDto(createdUser);
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UserDTO userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            // Update the desired properties.
            existingUser.Name = userDto.Name;
            existingUser.Email = userDto.Email;
            existingUser.Phone = userDto.Phone;
            existingUser.PasswordHash = userDto.PasswordHash; // Ensure proper hashing in real scenarios.
            existingUser.RoleId = userDto.RoleId;

            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return MapUserToDto(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => MapUserToDto(u));
        }
        #region Helper Mapping Methods

        // Convert a UserCreationRequestDTO to a User entity.
        private async Task<User> MapUserCreationRequestDtoToUser(UserCreationRequestDTO dto)
        {
            Role role = await _roleRepository.GetByIdAsync(dto.RoleId);
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = SecurityUtils.ComputeSha256Hash(dto.Password),
                RoleId = dto.RoleId,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Convert a domain User object to a UserDTO.
        private static UserDTO MapUserToDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                PasswordHash = user.PasswordHash,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                CreatedAt = user.CreatedAt
            };
        }
        #endregion
    }
}
