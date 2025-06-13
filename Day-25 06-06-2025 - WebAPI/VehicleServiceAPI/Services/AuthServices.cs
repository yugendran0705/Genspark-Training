using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;
using VehicleServiceAPI.Utils; 

namespace VehicleServiceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticate user, validate password, and generate Access Token & Refresh Token.
        /// </summary>
        public async Task<LoginResponseDTO> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !SecurityUtils.VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var accessToken = SecurityUtils.GenerateJwtToken(user, _configuration);
            var refreshToken = SecurityUtils.GenerateRefreshJwtToken(user,_configuration);

            return new LoginResponseDTO { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        /// <summary>
        /// Refresh Access Token using a valid Refresh Token.
        /// </summary>
        public async Task<LoginResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            int userId = SecurityUtils.ExtractUserIdFromToken(refreshToken);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            var newAccessToken = SecurityUtils.GenerateJwtToken(user, _configuration);
            var newRefreshToken = SecurityUtils.GenerateRefreshJwtToken(user, _configuration);

            return new LoginResponseDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }

        
    }
}
