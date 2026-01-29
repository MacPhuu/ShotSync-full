using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Data;
using PoolBrackets_backend_dotnet.Models;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }
    public async Task<Event> AddEventAsync(Event eventObj)
    {
        _context.Events.Add(eventObj);
        await _context.SaveChangesAsync();
        return eventObj;
    }

    public async Task UpdateEventAsync(Event eventObj)
    {
        _context.Entry(eventObj).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEventAsync(int id)
    {
        var eventObj = await _context.Events.FindAsync(id);
        if (eventObj != null)
        {
            _context.Events.Remove(eventObj);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> EventExistsAsync(int id)
    {
        return await _context.Events.AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetEventsByNameAsync(string name)
    {
        return await _context.Events
            .Where(e => e.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByHostIdAsync(int hostId)
    {
        return await _context.Events
            .Where(e => e.HostId == hostId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task RegisterPlayerByEmailAsync(int eventId, string email)
    {
        // 1. Find Player directly by Email
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Email == email);
        if (player == null)
        {
            throw new System.Exception("Player with this email not found.");
        }

        // 2. Check if already registered
        bool isRegistered = await _context.PlayerInEvents
            .AnyAsync(pie => pie.EventId == eventId && pie.PlayerId == player.Id);

        if (isRegistered)
        {
            throw new System.Exception("Player is already registered for this event.");
        }

        // 3. Add to PlayerInEvents
        var playerInEvent = new PlayerInEvent
        {
            EventId = eventId,
            PlayerId = player.Id
        };

        _context.PlayerInEvents.Add(playerInEvent);
        await _context.SaveChangesAsync();
    }
}