using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Interfaces
{
    /// <summary>
    /// Giao diện điều hướng logic giải đấu (Bộ não của hệ thống sơ đồ)
    /// </summary>
    public interface ITournamentService
    {
        /// <summary>
        /// Tự động sinh sơ đồ thi đấu Hybrid (Double Elimination Qualifying -> Single Elimination)
        /// </summary>
        /// <param name="eventId">ID của giải đấu</param>
        /// <param name="numberOfPlayers">Tổng số vận động viên (Phải là lũy thừa của 2)</param>
        Task GenerateBracketAsync(int eventId, int numberOfPlayers);

        /// <summary>
        /// Xử lý kết quả trận đấu: Đẩy người thắng lên nhánh trên và người thua xuống nhánh dưới (nếu có)
        /// </summary>
        /// <param name="matchId">ID của trận đấu vừa kết thúc</param>
        Task ProcessMatchResultAsync(int matchId);

        /// <summary>
        /// Khởi tạo giai đoạn Knock-out (Dùng cho các giải đấu chia giai đoạn riêng biệt)
        /// </summary>
        /// <param name="eventId">ID của giải đấu</param>
        Task GenerateKnockoutPhaseAsync(int eventId);

        Task ResetBracketAsync(int eventId);
    }
}