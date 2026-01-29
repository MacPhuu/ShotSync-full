using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Models.Enums;
using PoolBrackets_backend_dotnet.Services;
using PoolBrackets_backend_dotnet.Repositories;
using PoolBrackets_backend_dotnet.Interfaces;

namespace PoolBrackets_backend_dotnet.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ITournamentService _tournamentService;

        public EventsController(IEventService eventService, ITournamentService tournamentService)
        {
            _eventService = eventService;
            _tournamentService = tournamentService;
        }

        #region 1. NGHIỆP VỤ GIẢI ĐẤU (TOURNAMENT OPERATIONS)

        // API: Khai mạc giải đấu (Chỉ Admin và Host được phép)
        // POST: api/events/{id}/start
        [Authorize(Roles = "1,2")]
        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartTournament(int id)
        {
            var eventObj = await _eventService.GetEventByIdAsync(id);
            if (eventObj == null) return NotFound(new { message = "Không tìm thấy giải đấu." });

            if (eventObj.IsHappen) return BadRequest(new { message = "Giải đấu này đã được khai mạc rồi." });

            try
            {
                // 1. Gọi Service để tạo sơ đồ (Bracket)
                await _tournamentService.GenerateBracketAsync(id, eventObj.NumberOfPlayers);

                // 2. Cập nhật trạng thái giải đấu
                // Status: 0 = Upcoming, 1 = Ongoing, 2 = Finished
                eventObj.Status = (EventStatus)1;
                eventObj.IsHappen = true;
                eventObj.UpdateAt = DateTime.Now;

                await _eventService.UpdateEventAsync(eventObj);

                return Ok(new { message = "Khai mạc giải thành công! Sơ đồ thi đấu đã được tạo." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi khai mạc: " + ex.Message });
            }
        }

        // API: Chuyển sang vòng Knock-out (Chỉ Admin và Host được phép)
        // POST: api/events/{id}/start-knockout
        [Authorize(Roles = "1,2")]
        [HttpPost("{id}/start-knockout")]
        public async Task<IActionResult> StartKnockout(int id)
        {
            var eventObj = await _eventService.GetEventByIdAsync(id);
            if (eventObj == null) return NotFound(new { message = "Không tìm thấy giải đấu." });

            try
            {
                await _tournamentService.GenerateKnockoutPhaseAsync(id);
                return Ok(new { message = "Đã tạo sơ đồ vòng Knock-out thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region 2. CRUD CƠ BẢN

        // GET: api/events (Ai cũng xem được -> Public)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _eventService.GetEventsAsync();
            return Ok(events);
        }

        // GET: api/events/5 (Ai cũng xem được -> Public)
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventObj = await _eventService.GetEventByIdAsync(id);
            if (eventObj == null) return NotFound();
            return Ok(eventObj);
        }

        // POST: api/events (Chỉ Admin và Host được tạo giải)
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(AddEventDto dto)
        {
            var newEvent = new Event
            {
                Name = dto.Name,
                HostId = int.Parse(User.FindFirst("id")?.Value ?? "0"),
                Venue = dto.Venue,
                Location = dto.Location,
                Date = dto.Date,
                IsDisplayed = true,
                NumberOfPlayers = dto.NumberOfPlayers ?? 32,
                Slogan = dto.Slogan,
                Format = dto.Format,
                EntryFee = dto.EntryFee,
                TotalPrize = dto.TotalPrize,
                Description = dto.Description,
                IsHappen = false,

                // Mặc định tạo mới là Upcoming (0)
                Status = (EventStatus)0,

                CreatedAt = DateTime.Now,
                UpdateAt = DateTime.Now,
            };

            var createdEvent = await _eventService.AddEventAsync(newEvent);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
        }

        // PUT: api/events/5 (Chỉ Admin và Host được sửa)
        [Authorize(Roles = "1,2")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event eventObj)
        {
            if (id != eventObj.Id) return BadRequest("ID không khớp.");

            try
            {
                // Đảm bảo không bị mất ngày tạo
                var existingEvent = await _eventService.GetEventByIdAsync(id);
                if (existingEvent == null) return NotFound();

                // Cập nhật các trường cho phép
                existingEvent.Name = eventObj.Name;
                existingEvent.Venue = eventObj.Venue;
                existingEvent.Location = eventObj.Location;
                existingEvent.Date = eventObj.Date;
                existingEvent.NumberOfPlayers = eventObj.NumberOfPlayers;
                existingEvent.UpdateAt = DateTime.Now;

                // Status và IsHappen nên được cập nhật qua các API nghiệp vụ riêng (Start/Finish)
                // Tuy nhiên nếu Host muốn sửa tay thì vẫn cho phép gán đè:
                existingEvent.Status = eventObj.Status;

                await _eventService.UpdateEventAsync(existingEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi cập nhật: " + ex.Message });
            }

            return Ok(new { message = "Cập nhật thành công." });
        }

        // DELETE: api/events/5 (Chỉ Admin và Host được xóa)
        [Authorize(Roles = "1,2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!await _eventService.EventExistsAsync(id)) return NotFound();

            await _eventService.DeleteEventAsync(id);
            return NoContent();
        }

        // GET: api/events/search/Pool (Public)
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByName(string name)
        {
            var events = await _eventService.GetEventsByNameAsync(name);
            if (!events.Any()) return NotFound(new { message = "Không tìm thấy giải đấu nào." });
            return Ok(events);
        }

        // GET: api/events/host/{hostId} (Public)
        [HttpGet("host/{hostId}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByHost(int hostId)
        {
            var events = await _eventService.GetEventsByHostIdAsync(hostId);
            return Ok(events);
        }

        // POST: api/events/{id}/register-player (Chỉ Admin và Host được thêm)
        [Authorize(Roles = "1,2")]
        [HttpPost("{id}/register-player")]
        public async Task<IActionResult> RegisterPlayer(int id, [FromBody] string email)
        {
            try
            {
                // Basic validation for email format could be added here
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest(new { message = "Email is required." });
                }

                await _eventService.RegisterPlayerByEmailAsync(id, email);
                return Ok(new { message = "Player registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}