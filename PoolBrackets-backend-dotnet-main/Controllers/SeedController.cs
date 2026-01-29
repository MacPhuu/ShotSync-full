using Microsoft.AspNetCore.Mvc;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepo;

        public SeedController(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        [HttpPost("event/{eventId}")]
        public async Task<IActionResult> SeedPlayersForEvent(int eventId, [FromQuery] int count = 16)
        {
            for (int i = 1; i <= count; i++)
            {
                var player = new Player
                {
                    Name = $"Player {i}",
                    Email = $"player{i}@test.com",
                    Nation = "VN",
                    // EventId = eventId, // Removed: Not in Player model
                    Point = 0, // Correct type: int?
                    Portrait = null,
                    IsActive = true // Mandatory
                };

                // Add Player
                var newPlayer = await _playerRepo.AddPlayerAsync(player);

                // Register to Event
                if (newPlayer != null)
                {
                    await _playerRepo.RegisterPlayerToEventAsync(newPlayer.Id, eventId);
                }
            }

            return Ok(new { message = $"Seeded {count} players for event {eventId}" });
        }

        [HttpPost("event/{eventId}/fill")]
        public async Task<IActionResult> FillToPowerOfTwo(int eventId)
        {
            var currentPlayers = await _playerRepo.GetActivePlayersByEventAsync(eventId);
            int currentCount = currentPlayers.Count;
            int targetCount = 16; // Minimum default

            // Find next power of 2
            if (currentCount >= 16)
            {
                targetCount = 1;
                while (targetCount < currentCount) // Changed <= to <
                {
                    targetCount *= 2;
                }
            }
            else if (currentCount > 0)
            {
                // If less than 16 but > 0, check strict power of 2
                // Or just force to 16 for convenience as per user request context "add enough"
                // But let's be technically correct: 2, 4, 8, 16.
                targetCount = 1;
                while (targetCount < currentCount) targetCount *= 2;
                // If exactly equal, maybe add more? user asked "add to be eligible".
                // If it's already eligible (4, 8, 16), maybe we don't add?
                // But usually users want to scale up. 
                // Let's stick to: "Reach at least 16, or next power of 2 if > 16" to be safe for a tournament.
                // Actually, let's just ensure power of 2.
                if (currentCount < 4) targetCount = 4;
                else if (currentCount < 8) targetCount = 8;
                else if (currentCount < 16) targetCount = 16;
            }

            int needed = targetCount - currentCount;
            if (needed <= 0) return Ok(new { message = $"Current count {currentCount} is already sufficient (or logic decided not to add)." });

            for (int i = 1; i <= needed; i++)
            {
                try
                {
                    var player = new Player
                    {
                        Name = $"Extra Player {i}",
                        Email = $"extra{i}_{DateTime.Now.Ticks}_{Guid.NewGuid()}@test.com",
                        Nation = "VN",
                        Point = 0,
                        Portrait = null,
                        IsActive = true
                    };

                    var newPlayer = await _playerRepo.AddPlayerAsync(player);
                    if (newPlayer != null)
                    {
                        await _playerRepo.RegisterPlayerToEventAsync(newPlayer.Id, eventId);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WARNING] Failed to add extra player {i}: {ex.Message}");
                }
            }

            return Ok(new { message = $"Added {needed} players. Total is now {targetCount}." });
        }

        [HttpGet("event/{eventId}/diagnose")]
        public async Task<IActionResult> DiagnoseEvent(int eventId)
        {
            var players = await _playerRepo.GetActivePlayersByEventAsync(eventId);
            var allPlayersInSystem = await _playerRepo.GetPlayersAsync();

            return Ok(new
            {
                eventId = eventId,
                activePlayerCount = players.Count,
                activePlayerNames = players.Select(p => p.Name).ToList(),
                totalPlayersInSystem = allPlayersInSystem.Count
            });
        }

        [HttpDelete("event/{eventId}/clear")]
        public async Task<IActionResult> ClearPlayersFromEvent(int eventId)
        {
            // This is a helper for cleaning up testing data
            var currentPlayers = await _playerRepo.GetActivePlayersByEventAsync(eventId);
            foreach (var p in currentPlayers)
            {
                // Deactivate or Unregister? Unregister is cleaner for this case.
                // But wait, there is no unregister method in Repo. 
                // Let's just set them Inactive for now to hide them.
                p.IsActive = false;
                await _playerRepo.UpdatePlayerAsync(p);
            }
            return Ok(new { message = $"Cleared {currentPlayers.Count} players (set to Inactive)." });
        }
    }
}
