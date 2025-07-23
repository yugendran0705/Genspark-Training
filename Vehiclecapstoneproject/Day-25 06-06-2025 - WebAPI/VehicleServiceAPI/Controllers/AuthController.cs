using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        
        /// <summary>
        /// Authenticates a user with email and password. Returns an access token and a refresh token.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>A JWT access token and refresh token wrapped in a LoginResponseDTO.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
            _logger.LogWarning("Invalid login request model state.");
            return BadRequest(ModelState);
            }
            
            try
            {
            _logger.LogInformation("Attempting to authenticate user with email: {Email}", request.Email);
            // Authenticate the user and generate tokens.
            LoginResponseDTO response = await _authService.AuthenticateUserAsync(request.Email, request.Password);
            _logger.LogInformation("User authenticated successfully: {Email}", request.Email);
            return Ok(response);
            }
            catch (UnauthorizedAccessException uaEx)
            {
            _logger.LogWarning("Unauthorized login attempt for email: {Email}. Reason: {Reason}", request.Email, uaEx.Message);
            return Unauthorized(new { error = uaEx.Message });
            }
            catch (Exception ex)
            {
            _logger.LogError(ex, "Error occurred during login for email: {Email}", request.Email);
            return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Refreshes the access token using a valid refresh token.
        /// </summary>
        /// <param name="request">The refresh token request containing the refresh token.</param>
        /// <returns>A new JWT access token and refresh token wrapped in a LoginResponseDTO.</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                // Refresh the token.
                LoginResponseDTO response = await _authService.RefreshTokenAsync(request.RefreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                return Unauthorized(new { error = uaEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
