using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DocumentSharingAPI.Models;
using DocumentSharingAPI.Models.Dtos;
using DocumentSharingAPI.Interfaces;
using DocumentSharingAPI.Services;
using System.Security.Cryptography;
using System.Text;
using System;

namespace DocumentSharingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;
        
        public AuthController(IUserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.UserName) || string.IsNullOrEmpty(registerDto.Password))
            {
                return BadRequest("Username and password are required.");
            }
            
            // Check if user already exists
            var existingUser = await _userRepository.GetUserByUserNameAsync(registerDto.UserName);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }
            
            // Hash the password (for demo purposes using SHA256; use stronger hashing in production)
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(registerDto.Password);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            var passwordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = string.IsNullOrEmpty(registerDto.Role) ? "User" : registerDto.Role
            };
            
            await _userRepository.AddUserAsync(user);
            return Ok("User registered successfully.");
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(loginDto.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            
            // Verify password
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(loginDto.Password);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            var passwordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            
            if (passwordHash != user.PasswordHash)
            {
                return Unauthorized("Invalid credentials.");
            }
            
            // Generate JWT token
            var token = _authService.GenerateToken(user);
            var response = new AuthResponseDto
            {
                Token = token,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            
            return Ok(response);
        }
    }
}
