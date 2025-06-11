using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        
        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        [HttpGet("email")]
        public async Task<ActionResult<UserDTO>> GetByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userRequest">The request object containing new user data.</param>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Create([FromBody] UserCreationRequestDTO userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var createdUser = await _userService.CreateUserAsync(userRequest);
                return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <param name="userDto">The updated user data.</param>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> Update(int id, [FromBody] UserDTO userDto)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, userDto);
                return Ok(updatedUser);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        /// <summary>
        /// Soft-deletes a user.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound($"User with id {id} not found.");
            }
            return NoContent();
        }
    }
}
