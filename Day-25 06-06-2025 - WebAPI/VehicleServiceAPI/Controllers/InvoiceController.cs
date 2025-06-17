using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService invoiceService, ILogger<InvoiceController> logger)
        {
            _invoiceService = invoiceService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all invoices (Admin access only).
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDTO>>> GetAllInvoices()
        {
            _logger.LogInformation("Getting all invoices.");
            try
            {
                var invoices = await _invoiceService.GetAllInvoicesAsync();
                _logger.LogInformation("Retrieved {Count} invoices.", invoices.Count());
                return Ok(invoices);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all invoices.");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves an invoice by its ID.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDTO>> GetInvoiceById(int id)
        {
            _logger.LogInformation("Getting invoice by ID: {Id}", id);
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
                _logger.LogInformation("Invoice with ID {Id} retrieved.", id);
                return Ok(invoice);
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invoice with ID {Id} not found.", id);
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invoice with ID {Id}.", id);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new invoice. Invoice creation is allowed only when the associated booking is completed.
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<InvoiceDTO>> CreateInvoice([FromBody] CreateInvoiceDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for invoice creation.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new invoice for booking ID: {BookingId}", request.BookingId);
            try
            {
                var invoice = await _invoiceService.CreateInvoiceAsync(request);
                _logger.LogInformation("Invoice created with ID: {Id}", invoice.Id);
                return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to create invoice for booking ID: {BookingId}", request.BookingId);
                return BadRequest(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice for booking ID: {BookingId}", request.BookingId);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing invoice.
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<ActionResult<InvoiceDTO>> UpdateInvoice([FromBody] UpdateInvoiceDTO request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for invoice update.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating invoice with ID: {Id}", request.Id);
            try
            {
                var invoice = await _invoiceService.UpdateInvoiceAsync(request);
                _logger.LogInformation("Invoice with ID {Id} updated.", request.Id);
                return Ok(invoice);
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invoice with ID {Id} not found for update.", request.Id);
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating invoice with ID {Id}.", request.Id);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Soft-deletes an invoice by its ID.
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            _logger.LogInformation("Deleting invoice with ID: {Id}", id);
            try
            {
                bool success = await _invoiceService.DeleteInvoiceAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Invoice with ID {Id} not found for deletion.", id);
                    return NotFound(new { error = $"Invoice with ID {id} not found." });
                }
                _logger.LogInformation("Invoice with ID {Id} deleted.", id);
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting invoice with ID {Id}.", id);
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all invoices associated with a specific booking.
        /// </summary>
        [Authorize(Policy = "UserAccess")]
        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<IEnumerable<InvoiceDTO>>> GetInvoicesByBookingId(int bookingId)
        {
            _logger.LogInformation("Getting invoices for booking ID: {BookingId}", bookingId);
            try
            {
                var invoices = await _invoiceService.GetInvoicesByBookingIdAsync(bookingId);
                _logger.LogInformation("Retrieved {Count} invoices for booking ID: {BookingId}", invoices.Count(), bookingId);
                return Ok(invoices);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invoices for booking ID: {BookingId}", bookingId);
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
