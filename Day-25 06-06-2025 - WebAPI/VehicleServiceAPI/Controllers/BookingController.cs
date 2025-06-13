using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        
        /// <summary>
        /// Retrieves all bookings. (Admin access only)
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
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
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
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
                return BadRequest(ModelState);
            
            try
            {
                // Extract the user ID from the JWT token claims.
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");
                
                int userId = int.Parse(userIdClaim.Value);
                var createdBooking = await _bookingService.CreateBookingAsync(userId, request);
                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
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
                return BadRequest(ModelState);
            
            try
            {
                var updatedBooking = await _bookingService.UpdateBookingAsync(id, request);
                return Ok(updatedBooking);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch(Exception ex)
            {
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
            try
            {
                bool success = await _bookingService.DeleteBookingAsync(id);
                if (!success)
                    return NotFound($"Booking with ID {id} not found.");
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
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
                    return Unauthorized("User ID not found in token.");
                
                int userId = int.Parse(userIdClaim.Value);
                var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
                return Ok(bookings);
            }
            catch(Exception ex)
            {
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
            try
            {
                var bookings = await _bookingService.GetBookingsByStatusAsync(status);
                return Ok(bookings);
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
