using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        
        /// <summary>
        /// Retrieves all invoices (Admin access only).
        /// </summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDTO>>> GetAllInvoices()
        {
            try
            {
                var invoices = await _invoiceService.GetAllInvoicesAsync();
                return Ok(invoices);
            }
            catch(Exception ex)
            {
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
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
                return Ok(invoice);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
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
                return BadRequest(ModelState);
            
            try
            {
                var invoice = await _invoiceService.CreateInvoiceAsync(request);
                return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch(Exception ex)
            {
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
                return BadRequest(ModelState);
            
            try
            {
                var invoice = await _invoiceService.UpdateInvoiceAsync(request);
                return Ok(invoice);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch(Exception ex)
            {
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
            try
            {
                bool success = await _invoiceService.DeleteInvoiceAsync(id);
                if (!success)
                    return NotFound(new { error = $"Invoice with ID {id} not found." });
                return NoContent();
            }
            catch(Exception ex)
            {
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
            try
            {
                var invoices = await _invoiceService.GetInvoicesByBookingIdAsync(bookingId);
                return Ok(invoices);
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
