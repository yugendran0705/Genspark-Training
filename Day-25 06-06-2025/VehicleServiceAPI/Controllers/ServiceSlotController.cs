using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ServiceSlotController : ControllerBase
    {
        private readonly IServiceSlotService _serviceSlotService;
        
        public ServiceSlotController(IServiceSlotService serviceSlotService)
        {
            _serviceSlotService = serviceSlotService;
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
                var slots = await _serviceSlotService.GetAllServiceSlotsAsync();
                return Ok(slots);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
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
                var slot = await _serviceSlotService.GetServiceSlotByIdAsync(id);
                return Ok(slot);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
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
                return BadRequest(ModelState);
            }
            
            try
            {
                var createdSlot = await _serviceSlotService.CreateServiceSlotAsync(request);
                return CreatedAtAction(nameof(GetServiceSlotById), new { id = createdSlot.Id }, createdSlot);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
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
                return BadRequest(ModelState);
            }
            
            try
            {
                var updatedSlot = await _serviceSlotService.UpdateServiceSlotAsync(id, request);
                return Ok(updatedSlot);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
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
                bool success = await _serviceSlotService.DeleteServiceSlotAsync(id);
                if (!success)
                {
                    return NotFound($"Service slot with id {id} not found.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
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
                var availableSlots = await _serviceSlotService.GetAvailableSlotsAsync();
                return Ok(availableSlots);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
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
                var slots = await _serviceSlotService.GetSlotsByMechanicIdAsync(mechanicId);
                return Ok(slots);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
