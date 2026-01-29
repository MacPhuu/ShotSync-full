using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Repositories;
using PoolBrackets_backend_dotnet.Services; // Namespace chứa ITournamentService
using System;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Controllers
{
    [ApiController]
    [Route("api/matches")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;
        // Đã Inject thêm Service để xử lý logic đẩy người thắng
        private readonly ITournamentService _tournamentService;

        public MatchesController(IMatchService matchService, ITournamentService tournamentService)
        {
            _matchService = matchService;
            _tournamentService = tournamentService;
        }

        [HttpGet("by-event/{eventId}")]
        public async Task<IActionResult> GetMatchesByEvent(int eventId)
        {
            var matches = await _matchService.GetMatchesByEventAsync(eventId);

            var dtos = matches.Select(m => new MatchResponseDto
            {
                Id = m.Id,
                EventId = m.EventId,
                RoundName = m.RoundName,
                RoundType = m.RoundType,
                TableNumber = m.TableNumber,
                FirstPlayerId = m.FirstPlayerId,
                FirstPlayerName = m.FirstPlayer?.Name ?? "TBD",
                FirstPlayerPoint = m.FirstPlayerPoint,
                SecondPlayerId = m.SecondPlayerId,
                SecondPlayerName = m.SecondPlayer?.Name ?? "TBD",
                SecondPlayerPoint = m.SecondPlayerPoint,
                IsFinish = m.IsFinish,
                IsStart = m.IsStart,
                NextMatchPosition = m.NextMatchPosition
            });

            return Ok(dtos);
        }

        // 2. Cập nhật tỷ số (Live Score)
        // PUT: api/matches/{id}/score
        // Quyền: Chỉ Admin (1) và Host (2) được nhập điểm
        [Authorize(Roles = "1,2")]
        [HttpPut("{id}/score")]
        public async Task<IActionResult> UpdateScore(int id, [FromBody] UpdateScoreDto dto)
        {
            // Lấy trận đấu từ DB dựa trên ID trên URL
            var match = await _matchService.GetMatchByIdAsync(id);

            if (match == null) return NotFound(new { message = "Trận đấu không tồn tại." });
            if (match.IsFinish) return BadRequest(new { message = "Trận đấu đã kết thúc, không thể cập nhật điểm." });

            // Cập nhật điểm từ DTO
            match.FirstPlayerPoint = dto.FirstPlayerScore;
            match.SecondPlayerPoint = dto.SecondPlayerScore;

            // Lưu xuống DB
            await _matchService.UpdateMatchAsync(match);

            return Ok(new { message = "Đã cập nhật tỷ số.", matchId = id });
        }

        // 3. Kết thúc trận đấu
        // POST: api/matches/{id}/finish
        // Quyền: Chỉ Admin (1) và Host (2) được kết thúc trận
        [Authorize(Roles = "1,2")]
        [HttpPost("{id}/finish")]
        public async Task<IActionResult> FinishMatch(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);

            if (match == null) return NotFound(new { message = "Trận đấu không tồn tại." });
            if (match.IsFinish) return BadRequest(new { message = "Trận đấu này đã kết thúc rồi." });

            try
            {
                // B1: Đánh dấu kết thúc trận đấu trong DB
                match.IsFinish = true;
                await _matchService.UpdateMatchAsync(match);

                // B2: Gọi Service "Bộ não" để xử lý người thắng & cập nhật sơ đồ
                // (Đẩy người thắng vào trận tiếp theo)
                await _tournamentService.ProcessMatchResultAsync(id);

                return Ok(new { message = "Trận đấu kết thúc. Sơ đồ giải đấu đã được cập nhật.", matchId = id, isFinish = true });
            }
            catch (Exception ex)
            {
                // Trường hợp lỗi khi xử lý logic giải đấu, có thể cần rollback hoặc log lại
                return BadRequest(new { message = "Lỗi khi xử lý kết quả trận đấu: " + ex.Message });
            }
        }
    }
}