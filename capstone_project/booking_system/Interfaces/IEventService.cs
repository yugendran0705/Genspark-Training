namespace BookingSystem.Interfaces;
//start building event service crud and then craete category creating  in it as well
using BookingSystem.Models;
using BookingSystem.Models.DTOs;
public interface IEventService
{
    Task<Event> CreateEvent(EventDto eventDto);
    Task<Event> GetEventByName(string eventName);
    Task<IEnumerable<Event>> GetAllEvents();
    Task<Event> UpdateEvent(string eventName, EventDto eventDto);
    Task<Event> CancelEvent(string eventName);
    Task<Event> DeleteEvent(string eventName);
    Task<IEnumerable<Event>> GetAllEventsByEmail(string email);
    Task<Event> GetEventById(int id);
    Task<IEnumerable<EventDto>> GetEventsByCategoryAsync(string category);
    public Task<IEnumerable<Event>> GetEventsByPriceRange(int min, int max);
}