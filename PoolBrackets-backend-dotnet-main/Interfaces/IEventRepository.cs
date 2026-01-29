using System.Collections.Generic;
using System.Threading.Tasks;
using PoolBrackets_backend_dotnet.Models; // Chỉ dùng Models

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetEventsAsync();

    Task<Event?> GetEventByIdAsync(int id);

    Task<Event> AddEventAsync(Event eventObj);

    Task UpdateEventAsync(Event eventObj);
    Task DeleteEventAsync(int id);
    Task<bool> EventExistsAsync(int id);
    Task<IEnumerable<Event>> GetEventsByNameAsync(string name);
    Task<IEnumerable<Event>> GetEventsByHostIdAsync(int hostId);
    Task RegisterPlayerByEmailAsync(int eventId, string email);
}