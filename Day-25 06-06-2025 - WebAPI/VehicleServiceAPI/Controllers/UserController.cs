using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        
        /// <summary>
        /// Retrieves all mechanics (Admin access only).
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("mechanics")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllMechanics()
        {
            try
            {
            _logger?.LogInformation("Admin requested all mechanics.");
            var users = await _userService.GetAllMechanicsAsync();
            return Ok(users);
            }
            catch (InvalidOperationException ex)
            {
            _logger?.LogWarning(ex, "Invalid operation while retrieving all mechanics.");
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger?.LogWarning(ex, "Unauthorized access while retrieving all mechanics.");
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger?.LogError(ex, "Unexpected error while retrieving all mechanics.");
            return BadRequest(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Retrieves all users (Admin access only).
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            try
            {
            _logger?.LogInformation("Admin requested all users.");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
            }
            catch (InvalidOperationException ex)
            {
            _logger?.LogWarning(ex, "Invalid operation while retrieving all users.");
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger?.LogWarning(ex, "Unauthorized access while retrieving all users.");
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger?.LogError(ex, "Unexpected error while retrieving all users.");
            return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Retrieves the profile of the currently logged-in user.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            // Extract user Id from the JWT token claims.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
            _logger?.LogWarning("User ID not found in token while retrieving profile.");
            return Unauthorized("User ID not found in token.");
            }
            
            int userId = int.Parse(userIdClaim.Value);
            
            try
            {
            _logger?.LogInformation("User {UserId} requested their profile.", userId);
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
            _logger?.LogWarning(ex, "Invalid operation while retrieving user profile for user {UserId}.", userId);
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger?.LogWarning(ex, "Unauthorized access while retrieving user profile for user {UserId}.", userId);
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger?.LogError(ex, "Unexpected error while retrieving user profile for user {UserId}.", userId);
            return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Create([FromBody] UserCreationRequestDTO userRequest)
        {
            if (!ModelState.IsValid)
            {
            _logger?.LogWarning("Invalid model state while creating user.");
            return BadRequest(ModelState);
            }
            
            try
            {
            _logger?.LogInformation("Creating a new user with email: {Email}", userRequest.Email);
            var createdUser = await _userService.CreateUserAsync(userRequest);
            // Return the created profile; note for creation, the input is used.
            _logger?.LogInformation("User created successfully with ID: {UserId}", createdUser.Id);
            return CreatedAtAction(nameof(GetProfile), null, createdUser);
            }
            catch (InvalidOperationException ex)
            {
            _logger?.LogWarning(ex, "Invalid operation while creating user.");
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger?.LogWarning(ex, "Unauthorized access while creating user.");
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger?.LogError(ex, "Unexpected error while creating user.");
            return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Updates the profile of the currently logged-in user.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpPut("")]
        public async Task<ActionResult<UserDTO>> UpdateProfile([FromBody] UserUpdateRequestDTO userDto)
        {
            // Extract user Id from the JWT token claims.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
            _logger?.LogWarning("User ID not found in token while updating profile.");
            return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            
            try
            {
            _logger?.LogInformation("User {UserId} requested profile update.", userId);
            var updatedUser = await _userService.UpdateUserAsync(userId, userDto);
            _logger?.LogInformation("User {UserId} profile updated successfully.", userId);
            return Ok(updatedUser);
            }
            catch (InvalidOperationException ex)
            {
            _logger?.LogWarning(ex, "Invalid operation while updating user profile for user {UserId}.", userId);
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger?.LogWarning(ex, "Unauthorized access while updating user profile for user {UserId}.", userId);
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger?.LogError(ex, "Unexpected error while updating user profile for user {UserId}.", userId);
            return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates the role of a user (Admin access only).
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("role")]
        public async Task<ActionResult<UserDTO>> UpdateUserRole([FromBody] UpdateUserRequestDTO updateRequest)
        {
            if (!ModelState.IsValid)
            {
            _logger?.LogWarning("Invalid model state while updating user role.");
            return BadRequest(ModelState);
            }   
            try
            {
            _logger?.LogInformation("Admin requested to update role for user ID: {UserId}", updateRequest.UserId);
            var updatedUser = await _userService.UpdateUserRoleAsync(updateRequest);
            _logger?.LogInformation("User role updated successfully for user ID: {UserId}", updateRequest.UserId);
            return Ok(updatedUser);
            }
            catch (InvalidOperationException ex)
            {
            _logger?.LogWarning(ex, "Invalid operation while updating user role for user ID: {UserId}.", updateRequest.UserId);
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger?.LogWarning(ex, "Unauthorized access while updating user role for user ID: {UserId}.", updateRequest.UserId);
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger?.LogError(ex, "Unexpected error while updating user role for user ID: {UserId}.", updateRequest.UserId);
            return BadRequest(new { error = ex.Message  });
            }   
        }

        /// <summary>
            /// Soft-deletes the profile of the currently logged-in user.
            /// </summary>
            [Authorize(Policy = "UserAccess")]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteProfile()
        {
            try
            {
            // Extract user Id from the JWT token claims.
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger?.LogWarning("User ID not found in token while deleting profile.");
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);

            _logger?.LogInformation("User {UserId} requested profile deletion.", userId);
            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
            {
                _logger?.LogWarning("User with id {UserId} not found for deletion.", userId);
                return NotFound($"User with id {userId} not found.");
            }
            _logger?.LogInformation("User {UserId} profile deleted successfully.", userId);
            return NoContent();
            }
            catch (InvalidOperationException ex)
            {
            _logger?.LogWarning(ex, "Invalid operation while deleting user profile.");
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger?.LogWarning(ex, "Unauthorized access while deleting user profile.");
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger?.LogError(ex, "Unexpected error while deleting user profile.");
            return BadRequest(new { error = ex.Message });
            }
        }
    }
}
