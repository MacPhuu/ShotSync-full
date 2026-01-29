using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<Event>> GetEventsAsync()
        {
            return await _eventRepository.GetEventsAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _eventRepository.GetEventByIdAsync(id);
        }

        public async Task<Event> AddEventAsync(Event eventObj)
        {
            return await _eventRepository.AddEventAsync(eventObj);
        }

        public async Task UpdateEventAsync(Event eventObj)
        {
            await _eventRepository.UpdateEventAsync(eventObj);
        }

        public async Task DeleteEventAsync(int id)
        {
            await _eventRepository.DeleteEventAsync(id);
        }

        public async Task<bool> EventExistsAsync(int id)
        {
            return await _eventRepository.EventExistsAsync(id);
        }

        public async Task<IEnumerable<Event>> GetEventsByNameAsync(string name)
        {
            return await _eventRepository.GetEventsByNameAsync(name);
        }

        public async Task<IEnumerable<Event>> GetEventsByHostIdAsync(int hostId)
        {
            return await _eventRepository.GetEventsByHostIdAsync(hostId);
        }

        public async Task RegisterPlayerByEmailAsync(int eventId, string email)
        {
            await _eventRepository.RegisterPlayerByEmailAsync(eventId, email);
        }
    }
}
