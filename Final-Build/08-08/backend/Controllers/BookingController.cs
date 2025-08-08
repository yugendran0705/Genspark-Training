using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all bookings. (Admin access only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetAllBookings()
        {
            _logger.LogInformation("Getting all bookings (Admin access).");
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                _logger.LogInformation("Retrieved {Count} bookings.", bookings?.Count() ?? 0);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all bookings.");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a booking by its ID.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBookingById(int id)
        {
            _logger.LogInformation("Getting booking by ID: {Id}", id);
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                _logger.LogInformation("Booking with ID {Id} retrieved.", id);
                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Booking with ID {Id} not found.", id);
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving booking with ID {Id}.", id);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new booking using the currently authenticated user's ID.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpPost]
        public async Task<ActionResult<BookingDTO>> CreateBooking([FromBody] CreateBookingDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for booking creation.");
                return BadRequest(ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID not found in token during booking creation.");
                    return Unauthorized("User ID not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);
                _logger.LogInformation("Creating booking for user ID {UserId}.", userId);
                var createdBooking = await _bookingService.CreateBookingAsync(userId, request);
                _logger.LogInformation("Booking created with ID {BookingId} for user ID {UserId}.", createdBooking.Id, userId);
                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation during booking creation.");
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating booking.");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all bookings made by mechanic by their ID. (Mechanic access only)
        /// </summary>
        [Authorize(Policy = "MechanicAccess")]
        [HttpGet("mechanic")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookingsByMechanicId()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID not found in token during .");
                    return Unauthorized("User ID not found in token.");
                }

                int mechanicId = int.Parse(userIdClaim.Value);
                var bookings = await _bookingService.GetBookingsByMechanicIdAsync(mechanicId);
                _logger.LogInformation("Retrieved {Count} bookings for mechanic ID {MechanicId}.", bookings?.Count() ?? 0, mechanicId);
                return Ok(bookings);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Mechanic not found.");
                return NotFound(new { error = ex.Message });
            }   
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings for mechanic.");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing booking.
        /// </summary>
        [Authorize(Policy = "MechanicAccess")]
        [HttpPut("{id}")]
        public async Task<ActionResult<BookingDTO>> UpdateBooking(int id, [FromBody] UpdateBookingDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for booking update.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating booking with ID {Id}.", id);
                var updatedBooking = await _bookingService.UpdateBookingAsync(id, request);
                _logger.LogInformation("Booking with ID {Id} updated.", id);
                return Ok(updatedBooking);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Booking with ID {Id} not found for update.", id);
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogWarning("Unauthorized access attempt to update booking with ID {Id}.", id);
                return Forbid();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating booking with ID {Id}.", id);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Soft-deletes a booking by its ID.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            _logger.LogInformation("Deleting booking with ID {Id}.", id);
            try
            {
                bool success = await _bookingService.DeleteBookingAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Booking with ID {Id} not found for deletion.", id);
                    return NotFound($"Booking with ID {id} not found.");
                }
                _logger.LogInformation("Booking with ID {Id} deleted.", id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation during deletion of booking with ID {Id}.", id);
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting booking with ID {Id}.", id);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates the service details of a booking.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpPut("details")]
        public async Task<ActionResult<BookingDTO>> UpdateServiceDetails([FromBody] BookingServiceDetailsDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for service details update.");
                return BadRequest(ModelState);
            }
            try
            {
                _logger.LogInformation("Updating service details for booking ID {BookingId}.", request.BookingId);
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID not found in token during service details update.");
                    return Unauthorized("User ID not found in token.");
                }
                int UserId = int.Parse(userIdClaim.Value);
                _logger.LogInformation("User ID {UserId} is updating service details for booking ID {BookingId}.", UserId, request.BookingId);
                var updatedBooking = await _bookingService.UpdateServiceDetailsAsync(UserId,request);
                _logger.LogInformation("Service details updated for booking ID {BookingId}.", request.BookingId);
                return Ok(updatedBooking);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Booking with ID {BookingId} not found for service details update.", request.BookingId);
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating service details for booking ID {BookingId}.", request.BookingId);
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
            /// Retrieves all bookings made by the currently authenticated user.
            /// </summary>
            [Authorize(Policy = "UserAccess")]
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookingsByCurrentUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID not found in token during retrieval of user bookings.");
                    return Unauthorized("User ID not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);
                _logger.LogInformation("Getting bookings for user ID {UserId}.", userId);
                var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
                _logger.LogInformation("Retrieved {Count} bookings for user ID {UserId}.", bookings?.Count() ?? 0, userId);
                return Ok(bookings);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings for current user.");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all bookings filtered by status. (Admin access only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookingsByStatus(string status)
        {
            _logger.LogInformation("Getting bookings by status: {Status}", status);
            try
            {
                var bookings = await _bookingService.GetBookingsByStatusAsync(status);
                _logger.LogInformation("Retrieved {Count} bookings with status {Status}.", bookings?.Count() ?? 0, status);
                return Ok(bookings);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings by status {Status}.", status);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
