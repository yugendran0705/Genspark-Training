namespace BookingSystem.Services;

using BookingSystem.Models;
using BookingSystem.Repositories;
using BookingSystem.Interfaces;
using BookingSystem.Models.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class EventService : IEventService
{
    private readonly IRepository<string, Event> _eventRepository;
    private readonly IRepository<string, Category> _categoryRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IWalletService _walletService;
    private readonly IRepository<int, Ticket> _ticketRepository;

    public EventService(
        IRepository<string, Event> eventRepository,
        IRepository<string, Category> categoryRepository,
        IRepository<int, Ticket> ticketRepository,
        IHttpContextAccessor httpContextAccessor,
        IWalletService walletService)
    {
        _eventRepository = eventRepository;
        _categoryRepository = categoryRepository;
        _ticketRepository = ticketRepository;
        _httpContextAccessor = httpContextAccessor;
        _walletService = walletService;
    }

    public async Task<IEnumerable<Event>> GetAllEvents()
    {
        var allEvents = await _eventRepository.GetAll();
        return allEvents.Where(e => !e.IsCancelled);
    }

    public async Task<Event?> GetEventByName(string EventName)
    {
        return await _eventRepository.Get(EventName);
    }
    
    public async Task<Event> GetEventById(int id)
    {
        var allEvents = await _eventRepository.GetAll();
        return allEvents.FirstOrDefault(e => e.Id == id)!;   
    }

    public async Task<IEnumerable<Event>> GetAllEventsByEmail(string email)
    {
        var allEvents = await _eventRepository.GetAll();
        return allEvents.Where(e => e.CreatorEmail.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    

    public async Task<Event> CreateEvent(EventDto eventDto)
    {
        var existingCategory = await _categoryRepository.Get(eventDto.CategoryName);
        string? username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (existingCategory == null)
        {
            var newCategory = new Category { Name = eventDto.CategoryName };
            newCategory = await _categoryRepository.Add(newCategory);
            var newEvent = new Event
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                Date = eventDto.Date,
                CategoryId = newCategory.Id,
                CreatorEmail = username ?? string.Empty,
                Price = eventDto.Price,
                Address = eventDto.Address ?? "",
                City = eventDto.City ?? "",
                Context=eventDto.Context,
                Imageurl=eventDto.Imageurl,
                Ticketcount=eventDto.Ticketcount,
                IsCancelled = false
            };
            return await _eventRepository.Add(newEvent);
        }

        var newEvent2 = new Event
        {
            Title = eventDto.Title,
            Description = eventDto.Description,
            Date = eventDto.Date,
            CategoryId = existingCategory.Id,
            CreatorEmail = username ?? string.Empty,
            Price = eventDto.Price,
            Address = eventDto.Address ?? "",
            City = eventDto.City ?? "",
            Context=eventDto.Context,
            Imageurl=eventDto.Imageurl,
            Ticketcount=eventDto.Ticketcount,
            IsCancelled = false

        };
        return await _eventRepository.Add(newEvent2);


    }

    public async Task<Event?> UpdateEvent(string eventName, EventDto eventDto)
    {
        var existingEvent = await _eventRepository.Get(eventName);
        if (existingEvent == null)
        {
            throw new NotImplementedException($"Event '{eventName}' does not exist.");
        }

        var existingCategory = await _categoryRepository.Get(eventDto.CategoryName);
        if (existingCategory == null)
        {
            throw new ArgumentException($"Category '{eventDto.CategoryName}' does not exist.");
        }

        existingEvent.Title = eventDto.Title;
        existingEvent.Description = eventDto.Description;
        existingEvent.Date = eventDto.Date;
        existingEvent.CategoryId = existingCategory.Id;
        existingEvent.Price = eventDto.Price;
        existingEvent.Address = eventDto.Address;
        existingEvent.City = eventDto.City;
        existingEvent.Imageurl = eventDto.Imageurl;
        existingEvent.Context = eventDto.Context;
        existingEvent.Ticketcount = eventDto.Ticketcount;
        existingEvent.CreatorEmail = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;


        return await _eventRepository.Update(eventName, existingEvent);
    }

    public async Task<Event> CancelEvent(string eventName)
    {
        var existingEvent = await _eventRepository.Get(eventName);
        if (existingEvent == null)
        {
            throw new NotImplementedException($"Event '{eventName}' does not exist.");
        }

        // Mark event as cancelled
        existingEvent.IsCancelled = true;
        await _eventRepository.Update(eventName, existingEvent);

        // Refund logic
        var allTickets = await _ticketRepository.GetAll();
        var eventTickets = allTickets.Where(t => t.EventId == existingEvent.Id && !t.IsCancelled);

        foreach (var ticket in eventTickets)
        {
            // Refund ticket amount to user's wallet
            await _walletService.AddAmountToWallet(ticket.CustomerEmail, ticket.Total);

            // Mark ticket as cancelled (optional, if event cancelled means tickets are void)
            ticket.IsCancelled = true;
            await _ticketRepository.Update(ticket.Id, ticket);
        }

        return existingEvent;
    }


    public async Task<Event> DeleteEvent(string eventName)
    {
        return await _eventRepository.Delete(eventName);
    }
    public async Task<IEnumerable<EventDto>> GetEventsByCategoryAsync(string category)
    {
        var existingCategory = await _categoryRepository.Get(category);
        if (existingCategory == null)
        {
            throw new ArgumentException($"Category '{category}' does not exist.");
        }
        var categoryid = existingCategory.Id;
        var allEvents = await _eventRepository.GetAll();
        return allEvents.Where(e => e.CategoryId == categoryid).OrderBy(e => e.Date).Select(e => new EventDto { Title = e.Title, Date = e.Date, Description = e.Description, Price = e.Price, CategoryName = category }).ToList();
    }
    public async Task<IEnumerable<Event>> GetEventsByPriceRange(int min, int max)
    {
        var allEvents = await _eventRepository.GetAll();
        return allEvents.Where(e => e.Price >= min && e.Price <= max);
    }
}