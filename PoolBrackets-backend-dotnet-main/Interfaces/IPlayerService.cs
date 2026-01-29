using PoolBrackets_backend_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Interfaces
{
    public interface IPlayerService
    {
        // --- CRUD CƠ BẢN ---
        Task<List<Player>> GetPlayersAsync();
        Task<Player?> GetPlayerByIdAsync(int id);
        Task<Player> AddPlayerAsync(Player player);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
        Task<bool> PlayerExistsAsync(int id);
        Task<List<Player>> GetPlayersByNameAsync(string name);

        // --- NGHIỆP VỤ GIẢI ĐẤU ---
        Task<List<Player>> GetActivePlayersByEventAsync(int eventId);
        Task RegisterPlayerToEventAsync(int playerId, int eventId);
        Task<bool> IsPlayerInEventAsync(int playerId, int eventId);
    }
}
