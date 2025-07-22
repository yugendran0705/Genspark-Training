namespace BookingSystem.Controllers;

using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Models.DTOs;
using BookingSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using BookingSystem.Misc;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IHubContext<EventHub> _hubContext;


    public EventsController(IEventService eventService, ILogger<AuthenticationController> logger, IHubContext<EventHub> hubContext)
    {
        _eventService = eventService;
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
    {
        try
        {
            var events = await _eventService.GetAllEvents();
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events");
            return BadRequest("An error occurred while retrieving events.");
        }
    }

    [HttpGet("{eventName}")]
    public async Task<ActionResult<Event>> GetEventByName(string eventName)
    {
        try
        {
            var ev = await _eventService.GetEventByName(eventName);
            if (ev == null)
            {
                _logger.LogWarning("Event with name {EventName} not found", eventName);
                return NotFound();
            }
            return Ok(ev);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event with name {EventName}", eventName);
            return BadRequest("An error occurred while retrieving the event.");
        }
    }

    [HttpGet("admin/{email}")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEventByEmail(string email)
    {
        try
        {
            var ev = await _eventService.GetAllEventsByEmail(email);
            if (ev == null)
            {
                _logger.LogWarning("Event with name {EventName} not found", email);
                return NotFound();
            }
            return Ok(ev);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event with name {EventName}", email);
            return BadRequest("An error occurred while retrieving the event.");
        }
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Event>> GetEventById(int id)
    {
        try
        {
            var ev = await _eventService.GetEventById(id);
            if (ev == null)
            {
                _logger.LogWarning("Event with ID {Id} not found", id);
                return NotFound();
            }
            return Ok(ev);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event with ID {Id}", id);
            return BadRequest("An error occurred while retrieving the event.");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Event>> CreateEvent([FromBody] EventDto eventDto)
    {
        try
        {
            var createdEvent = await _eventService.CreateEvent(eventDto);
            _logger.LogInformation("Event {EventName} created successfully", createdEvent.Title);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "New event added", $"{createdEvent.Title}", $"{createdEvent.Description}", $"{eventDto.CategoryName}",$"{createdEvent.Imageurl}");
            return CreatedAtAction(nameof(GetEventByName), new { eventName = createdEvent.Title }, createdEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{eventName}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Event>> UpdateEvent(string eventName, [FromBody] EventDto eventDto)
    {
        try
        {
            var updatedEvent = await _eventService.UpdateEvent(eventName, eventDto);
            if (updatedEvent == null)
            {
                _logger.LogWarning("Event with name {EventName} not found for update", eventName);
                return NotFound();
            }
            _logger.LogInformation("Event {EventName} Updated successfully", updatedEvent.Title);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Event Updated(check your ticket)", $"{updatedEvent.Title}", $"{updatedEvent.Description}", $"{eventDto.CategoryName}",$"{updatedEvent.Imageurl}");

            return Ok(updatedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event with name {EventName}", eventName);
            return BadRequest(ex.Message);
        }
    }




    [HttpDelete("{eventName}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Event>> DeleteEvent(string eventName)
    {
        try
        {
            var deletedEvent = await _eventService.CancelEvent(eventName);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Deleted Event", $"{deletedEvent.Title}", $"{deletedEvent.Description}", $"{deletedEvent.CategoryId}");
            return Ok(deletedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event with name {EventName}", eventName);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEventsByCategory(string category)
    {
        try
        {
            var events = await _eventService.GetEventsByCategoryAsync(category);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events by category {Category}", category);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("price")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEventsByPriceRange([FromQuery] int min, [FromQuery] int max)
    {
        try
        {
            var events = await _eventService.GetEventsByPriceRange(min, max);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events by price range {Min} - {Max}", min, max);
            return BadRequest(ex.Message);
        }
    }
}