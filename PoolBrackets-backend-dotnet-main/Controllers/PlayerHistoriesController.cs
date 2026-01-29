using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Controllers
{
    [Route("api/player-histories")]
    [ApiController]
    public class PlayerHistoriesController : ControllerBase
    {
        private readonly IPlayerHistoryRepository _playerHistoryRepository;

        public PlayerHistoriesController(IPlayerHistoryRepository playerHistoryRepository)
        {
            _playerHistoryRepository = playerHistoryRepository;
        }

        #region READ (Public Access)

        // GET: api/player-histories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerHistory>>> GetAll()
        {
            var histories = await _playerHistoryRepository.GetAllAsync();
            return Ok(histories);
        }

        // GET: api/player-histories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerHistory>> GetById(int id)
        {
            var history = await _playerHistoryRepository.GetByIdAsync(id);
            if (history == null) return NotFound(new { message = "Không tìm thấy lịch sử đấu này." });
            return Ok(history);
        }

        // GET: api/player-histories/player/10
        [HttpGet("player/{playerId}")]
        public async Task<ActionResult<IEnumerable<PlayerHistory>>> GetByPlayerId(int playerId)
        {
            var histories = await _playerHistoryRepository.GetHistoryByPlayerIdAsync(playerId);
            return Ok(histories);
        }

        #endregion

        #region WRITE (Admin & Host Only)

        // POST: api/player-histories
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public async Task<ActionResult<PlayerHistory>> Add(PlayerHistoryDto dto)
        {
            var newHistory = new PlayerHistory
            {
                PlayerId = dto.PlayerId,
                MatchId = dto.MatchId,
                Result = dto.Result,
                OpponentId = dto.OpponentId,
                EventId = dto.EventId,
                MatchDate = DateTime.Now
            };

            var createdHistory = await _playerHistoryRepository.AddAsync(newHistory);
            return CreatedAtAction(nameof(GetById), new { id = createdHistory.Id }, createdHistory);
        }

        // PUT: api/player-histories/5
        [Authorize(Roles = "1,2")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PlayerHistoryDto dto)
        {
            var existingHistory = await _playerHistoryRepository.GetByIdAsync(id);
            if (existingHistory == null) return NotFound(new { message = "Không tìm thấy lịch sử đấu." });

            // Cập nhật dữ liệu (Cho phép sửa kết quả nếu nhập sai)
            existingHistory.Result = dto.Result;
            existingHistory.OpponentId = dto.OpponentId;
            existingHistory.MatchId = dto.MatchId;
            existingHistory.EventId = dto.EventId;
            // Không cập nhật MatchDate để giữ nguyên thời điểm diễn ra

            await _playerHistoryRepository.UpdateAsync(existingHistory);
            return Ok(new { message = "Cập nhật thành công.", data = existingHistory });
        }

        // DELETE: api/player-histories/5
        [Authorize(Roles = "1,2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _playerHistoryRepository.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Không tìm thấy lịch sử đấu để xóa." });
            return NoContent();
        }

        #endregion
    }
}