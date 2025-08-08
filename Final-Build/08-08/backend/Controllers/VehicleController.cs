using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all vehicles.
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetAllVehicles()
        {
            _logger.LogInformation("Getting all vehicles (AdminOnly)");
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                _logger.LogInformation("Retrieved {Count} vehicles", vehicles.Count());
                return Ok(vehicles);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in GetAllVehicles");
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in GetAllVehicles");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllVehicles");
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Policy = "UserAccess")]
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetAllVehiclesByUser()
        {
            _logger.LogInformation("Getting vehicles for current user");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID not found in token.");
                    return Unauthorized("User ID not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);

                var vehicles = await _vehicleService.GetVehicleByUserAsync(userId);
                _logger.LogInformation("User {UserId} retrieved {Count} vehicles", userId, vehicles.Count());
                return Ok(vehicles);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in GetAllVehiclesByUser");
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in GetAllVehiclesByUser");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllVehiclesByUser");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a vehicle by its ID.
        /// </summary>
        /// <param name="id">The vehicle's ID.</param>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDTO>> GetVehicleById(int id)
        {
            _logger.LogInformation("Getting vehicle by id {Id}", id);
            try
            {
                var vehicleDto = await _vehicleService.GetVehicleByIdAsync(id);
                _logger.LogInformation("Vehicle {Id} retrieved", id);
                return Ok(vehicleDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in GetVehicleById for id {Id}", id);
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in GetVehicleById for id {Id}", id);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetVehicleById for id {Id}", id);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new vehicle.
        /// </summary>
        /// <param name="request">The creation request containing vehicle details.</param>
        [Authorize(Policy = "UserAccess")]
        [HttpPost]
        public async Task<ActionResult<VehicleDTO>> CreateVehicle([FromBody] CreateVehicleDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in CreateVehicle");
                return BadRequest(ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID not found in token.");
                    return Unauthorized("User ID not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);

                var createdVehicle = await _vehicleService.CreateVehicleAsync(userId, request);
                _logger.LogInformation("User {UserId} created vehicle {VehicleId}", userId, createdVehicle.Id);
                return CreatedAtAction(nameof(GetVehicleById),
                                       new { id = createdVehicle.Id },
                                       createdVehicle);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in CreateVehicle");
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in CreateVehicle");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateVehicle");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing vehicle.
        /// </summary>
        /// <param name="id">The vehicle's ID.</param>
        /// <param name="request">The update request containing the new vehicle details.</param>
        [Authorize(Policy = "UserAccess")]
        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDTO>> UpdateVehicle(int id, [FromBody] UpdateVehicleDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in UpdateVehicle for id {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID not found in token.");
                    return Unauthorized("User ID not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);

                var updatedVehicle = await _vehicleService.UpdateVehicleAsync(id, userId, request);
                _logger.LogInformation("User {UserId} updated vehicle {VehicleId}", userId, id);
                return Ok(updatedVehicle);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in UpdateVehicle for id {Id}", id);
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in UpdateVehicle for id {Id}", id);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateVehicle for id {Id}", id);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes (or removes) a vehicle by its ID.
        /// </summary>
        /// <param name="id">The vehicle's ID.</param>
        [Authorize(Policy = "UserAccess")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            _logger.LogInformation("Attempting to delete vehicle {Id}", id);
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID not found in token.");
                    return Unauthorized("User ID not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);

                var success = await _vehicleService.DeleteVehicleAsync(id, userId);
                if (!success)
                {
                    _logger.LogWarning("Vehicle with id {Id} not found for deletion", id);
                    return NotFound($"Vehicle with id {id} not found.");
                }
                _logger.LogInformation("User {UserId} deleted vehicle {VehicleId}", userId, id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation in DeleteVehicle for id {Id}", id);
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access in DeleteVehicle for id {Id}", id);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteVehicle for id {Id}", id);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
