using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Data;
using PoolBrackets_backend_dotnet.Models;

namespace PoolBrackets_backend_dotnet.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;

        public PlayerRepository(AppDbContext context)
        {
            _context = context;
        }

        #region CRUD PLAYER (Basic)
        public async Task<List<Player>> GetPlayersAsync()
        {
            return await _context.Players.AsNoTracking().ToListAsync();
        }

        public async Task<Player?> GetPlayerByIdAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }

        public async Task<Player> AddPlayerAsync(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return player;
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            _context.Entry(player).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlayerAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PlayerExistsAsync(int id)
        {
            return await _context.Players.AnyAsync(e => e.Id == id);
        }

        public async Task<List<Player>> GetPlayersByNameAsync(string name)
        {
            return await _context.Players
                .Where(p => p.Name.Contains(name))
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region EVENT INTEGRATION (Tích hợp giải đấu)

        // Lấy danh sách VĐV tham gia giải
        public async Task<List<Player>> GetActivePlayersByEventAsync(int eventId)
        {
            return await _context.PlayerInEvents
                .Where(pie => pie.EventId == eventId)
                .Include(pie => pie.Player) // Join bảng Player
                .Select(pie => pie.Player!)
                .Where(p => p.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        // Đăng ký VĐV vào giải
        public async Task RegisterPlayerToEventAsync(int playerId, int eventId)
        {
            var registration = new PlayerInEvent
            {
                PlayerId = playerId,
                EventId = eventId
            };
            _context.PlayerInEvents.Add(registration);
            await _context.SaveChangesAsync();
        }

        // Kiểm tra VĐV đã có trong giải chưa
        public async Task<bool> IsPlayerInEventAsync(int playerId, int eventId)
        {
            return await _context.PlayerInEvents
                .AnyAsync(pie => pie.PlayerId == playerId && pie.EventId == eventId);
        }

        #endregion
    }
}