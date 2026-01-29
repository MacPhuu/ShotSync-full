using PoolBrackets_backend_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Interfaces
{
    public interface IPlayerHistoryRepository
    {
        // CRUD Cơ bản
        Task<IEnumerable<PlayerHistory>> GetAllAsync();
        Task<PlayerHistory?> GetByIdAsync(int id);
        Task<PlayerHistory> AddAsync(PlayerHistory history);
        Task UpdateAsync(PlayerHistory history);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<PlayerHistory>> GetHistoryByPlayerIdAsync(int playerId);
        Task LogMatchResultAsync(int playerId, int matchId, string result, int opponentId);
    }
}