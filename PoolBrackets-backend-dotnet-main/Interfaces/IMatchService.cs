using PoolBrackets_backend_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Interfaces
{
    public interface IMatchService
    {
        // Lấy danh sách trận đấu theo giải
        Task<IEnumerable<Match>> GetMatchesByEventAsync(int eventId);

        // Lấy chi tiết 1 trận đấu
        Task<Match?> GetMatchByIdAsync(int id);

        // Cập nhật thông tin trận đấu
        Task UpdateMatchAsync(Match match);

        // Thêm trận đấu mới
        Task AddMatchAsync(Match match);
    }
}
