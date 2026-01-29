using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IEventService _eventService;

        public PlayersController(IPlayerService playerService, IEventService eventService)
        {
            _playerService = playerService;
            _eventService = eventService;
        }

        #region 1. CRUD API (QUẢN LÝ HỒ SƠ VĐV)

        // GET: api/players (Public)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return Ok(await _playerService.GetPlayersAsync());
        }

        // GET: api/players/5 (Public)
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);
            if (player == null) return NotFound(new { message = "Không tìm thấy vận động viên." });
            return Ok(player);
        }

        // POST: api/players
        // Quyền: User, Host, Admin đều được tạo hồ sơ
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            // Set mặc định
            player.CreatedAt = DateTime.Now;
            player.UpdateAt = DateTime.Now;
            player.IsActive = true;

            var createdPlayer = await _playerService.AddPlayerAsync(player);
            return CreatedAtAction(nameof(GetPlayer), new { id = createdPlayer.Id }, createdPlayer);
        }

        // PUT: api/players/5
        // Quyền: User (sửa mình), Host, Admin
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.Id) return BadRequest(new { message = "ID không khớp." });

            try
            {
                // Logic an toàn: Lấy cũ -> Cập nhật -> Lưu
                var existingPlayer = await _playerService.GetPlayerByIdAsync(id);
                if (existingPlayer == null) return NotFound(new { message = "VĐV không tồn tại." });

                existingPlayer.Name = player.Name;
                existingPlayer.Nation = player.Nation;
                existingPlayer.IsActive = player.IsActive;
                existingPlayer.UpdateAt = DateTime.Now;

                await _playerService.UpdatePlayerAsync(existingPlayer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi cập nhật: " + ex.Message });
            }

            return NoContent();
        }

        // DELETE: api/players/5
        // Quyền: CHỈ Admin (1) và Host (2) được xóa
        [Authorize(Roles = "1,2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            if (!await _playerService.PlayerExistsAsync(id))
                return NotFound(new { message = "VĐV không tồn tại." });

            await _playerService.DeletePlayerAsync(id);
            return NoContent();
        }

        // GET: api/players/search/Nam (Public)
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByName(string name)
        {
            var players = await _playerService.GetPlayersByNameAsync(name);
            if (!players.Any()) return NotFound(new { message = "Không tìm thấy VĐV nào." });
            return Ok(players);
        }
        #endregion

        #region 2. INTEGRATION API (ĐĂNG KÝ GIẢI)

        // POST: api/players/register
        // Quyền: User (tự đăng ký), Host, Admin
        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPlayerToEvent(RegisterPlayerDto dto)
        {
            // 1. Validate dữ liệu
            if (!await _playerService.PlayerExistsAsync(dto.PlayerId))
                return NotFound(new { message = "Vận động viên không tồn tại." });

            if (!await _eventService.EventExistsAsync(dto.EventId))
                return NotFound(new { message = "Giải đấu không tồn tại." });

            // 2. Kiểm tra trùng lặp
            if (await _playerService.IsPlayerInEventAsync(dto.PlayerId, dto.EventId))
                return BadRequest(new { message = "Vận động viên này đã đăng ký tham gia giải đấu rồi." });

            // 3. Thực hiện đăng ký
            await _playerService.RegisterPlayerToEventAsync(dto.PlayerId, dto.EventId);

            return Ok(new { message = "Đăng ký tham gia giải đấu thành công!" });
        }

        // GET: api/players/by-event/{eventId} (Public)
        [HttpGet("by-event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByEvent(int eventId)
        {
            var players = await _playerService.GetActivePlayersByEventAsync(eventId);
            return Ok(players);
        }

        #endregion
    }
}