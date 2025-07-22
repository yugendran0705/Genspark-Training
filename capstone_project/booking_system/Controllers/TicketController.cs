namespace BookingSystem.Controllers;

using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Models.DTOs;
using BookingSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;

[Route("api/v1/[controller]")]
[ApiController]

public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly IEventService _eventService;
    private readonly ILogger<AuthenticationController> _logger;

    public TicketController(ITicketService ticketService,IEventService eventService, ILogger<AuthenticationController> logger)
    {
        _ticketService = ticketService;
        _eventService=eventService;
        _logger = logger;
    }

    [HttpPost("book")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<Ticket>> BookTicket([FromBody] TicketDto ticketDto)
    {
        try
        {
            var ticket = await _ticketService.BookTicket(ticketDto);
            var currentEvent= await _eventService.GetEventByName(ticketDto.EventName);

            // 1. Create a PDF document
            using var stream = new MemoryStream();
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Use a built-in PDF font that works in Docker (no system dependency)
            var font = new XFont("DejaVu Sans", 12, XFontStyle.Regular);
            var boldFont = new XFont("DejaVu Sans", 12, XFontStyle.Bold);

            // 2. Draw border and title
            gfx.DrawRectangle(XPens.SteelBlue, 30, 30, page.Width - 60, page.Height - 60);
            gfx.DrawString("Event Ticket", boldFont, XBrushes.DarkBlue, new XPoint(40, 50));

            // 3. Structured layout
            int y = 90;
            int lineHeight = 25;

            void DrawRow(string label, string value)
{
                gfx.DrawString(label, boldFont, XBrushes.Black, new XPoint(50, y));
                gfx.DrawString(value, font, XBrushes.DarkSlateGray, new XPoint(200, y));
                y += lineHeight;
            }

            DrawRow("Ticket ID:", ticket.Id.ToString());
            DrawRow("Event name:", ticketDto.EventName);
            DrawRow("Location:", $"{currentEvent.Address}, {currentEvent.City}");
            DrawRow("Quantity:", ticket.Quantity.ToString());
            DrawRow("Total Price:", $"₹ {ticket.Total}");
            DrawRow("Customer Email:", ticket.CustomerEmail);
            DrawRow("Booked At:", ticket.BookingDate.ToString("f"));

            

            // 4. Save and return
            document.Save(stream, false);
            stream.Position = 0;

            var fileName = $"Ticket_{ticket.Id}.pdf";

            // Optional: Save to Desktop
            // var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // var desktopFilePath = Path.Combine(desktopPath, fileName);
            // await System.IO.File.WriteAllBytesAsync(desktopFilePath, stream.ToArray());

            // Return file as download
            return File(stream.ToArray(), "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error booking ticket for event {EventName}", ticketDto.EventName);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<Ticket>> GetTicketById(int id)
    {
        try
        {
            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket with ID {Id} not found", id);
                return NotFound();
            }
            return Ok(ticket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ticket with ID {Id}", id);
            return NotFound(ex.Message);
        }
    }

    [HttpGet("gettickets/{email}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketByUser(string email)
    {
        try
        {
            var tickets = await _ticketService.GetTicketByUser(email);
            if (tickets == null)
            {
                _logger.LogWarning("Ticket Notfound", email);
                return NotFound();
            }
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tickets booked from the given email.", email);
            return NotFound(ex.Message);
        }
    }


    [HttpDelete("{id}/cancel")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<Ticket>> CancelTicketById(int id)
    {
        try
        {
            var ticket = await _ticketService.CancelTicketById(id);
            return Ok(ticket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling ticket with ID {Id}", id);
            return NotFound(ex.Message);
        }
    }

}