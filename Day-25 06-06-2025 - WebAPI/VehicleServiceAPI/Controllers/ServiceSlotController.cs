using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Misc;
using Microsoft.AspNetCore.SignalR;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ServiceSlotController : ControllerBase
    {
        private readonly IServiceSlotService _serviceSlotService;
        private readonly ILogger<ServiceSlotController> _logger;
        private readonly IHubContext<EventHub> _hubContext;
        
        public ServiceSlotController(
            IServiceSlotService serviceSlotService, 
            ILogger<ServiceSlotController> logger,
            IHubContext<EventHub> hubContext)
        {
            _serviceSlotService = serviceSlotService;
            _logger = logger;
            _hubContext = hubContext;
        }
        
        /// <summary>
        /// Retrieves all service slots. (Admin access only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceSlotDTO>>> GetAllServiceSlots()
        {
            try
            {
                _logger.LogInformation("Getting all service slots");
                var slots = await _serviceSlotService.GetAllServiceSlotsAsync();
                return Ok(slots);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "GetAllServiceSlots: No slots found");
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all service slots");
                return BadRequest(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Retrieves a service slot by its ID.
        /// </summary>
        /// <param name="id">The service slot's ID.</param>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceSlotDTO>> GetServiceSlotById(int id)
        {
            try
            {
                _logger.LogInformation("Getting service slot {Id}", id);
                var slot = await _serviceSlotService.GetServiceSlotByIdAsync(id);
                return Ok(slot);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Service slot {Id} not found", id);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service slot {Id}", id);
                return BadRequest(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Creates a new service slot.
        /// </summary>
        /// <param name="request">The creation request containing service slot details.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<ServiceSlotDTO>> CreateServiceSlot([FromBody] CreateServiceSlotDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateServiceSlot: Invalid model state {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }
            
            try
            {
                var createdSlot = await _serviceSlotService.CreateServiceSlotAsync(request);
                _logger.LogInformation("Service slot created successfully with id {Id}", createdSlot.Id);
                
                // Notify all connected clients that a new slot has been created.
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "New slot added", $"{createdSlot.SlotDateTime}", $"{createdSlot.MechanicName}", $"{createdSlot.Status}");
                
                return CreatedAtAction(nameof(GetServiceSlotById), new { id = createdSlot.Id }, createdSlot);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "CreateServiceSlot: Service slot creation failed");
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateServiceSlot: Error occurred while creating slot");
                return BadRequest(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Updates an existing service slot.
        /// </summary>
        /// <param name="id">The service slot's ID.</param>
        /// <param name="request">The update request containing the new service slot details.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceSlotDTO>> UpdateServiceSlot(int id, [FromBody] UpdateServiceSlotDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateServiceSlot: Invalid model state for id {Id}", id);
                return BadRequest(ModelState);
            }
            
            try
            {
                var updatedSlot = await _serviceSlotService.UpdateServiceSlotAsync(id, request);
                _logger.LogInformation("Service slot {Id} updated successfully", id);
                return Ok(updatedSlot);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "UpdateServiceSlot: Service slot {Id} not found", id);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateServiceSlot: Error occurred while updating slot {Id}", id);
                return BadRequest(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Deletes (soft-deletes) a service slot by its ID.
        /// </summary>
        /// <param name="id">The service slot's ID.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceSlot(int id)
        {
            try
            {
                _logger.LogInformation("Deleting service slot {Id}", id);
                bool success = await _serviceSlotService.DeleteServiceSlotAsync(id);
                if (!success)
                {
                    _logger.LogWarning("DeleteServiceSlot: Service slot {Id} not found", id);
                    return NotFound(new { error = $"Service slot with id {id} not found." });
                }
                _logger.LogInformation("Service slot {Id} deleted successfully", id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "DeleteServiceSlot: Service slot {Id} not found", id);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteServiceSlot: Error occurred while deleting slot {Id}", id);
                return BadRequest(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Retrieves all available service slots.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ServiceSlotDTO>>> GetAvailableSlots()
        {
            try
            {
                _logger.LogInformation("Getting available service slots");
                var availableSlots = await _serviceSlotService.GetAvailableSlotsAsync();
                return Ok(availableSlots);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "No available service slots found");
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available service slots");
                return BadRequest(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Retrieves all service slots assigned to a specific mechanic.
        /// </summary>
        /// <param name="mechanicId">The mechanic's ID.</param>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("mechanic/{mechanicId}")]
        public async Task<ActionResult<IEnumerable<ServiceSlotDTO>>> GetSlotsByMechanicId(int mechanicId)
        {
            try
            {
                _logger.LogInformation("Getting service slots for mechanic {MechanicId}", mechanicId);
                var slots = await _serviceSlotService.GetSlotsByMechanicIdAsync(mechanicId);
                return Ok(slots);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Service slots not found for mechanic {MechanicId}", mechanicId);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving slots for mechanic {MechanicId}", mechanicId);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
