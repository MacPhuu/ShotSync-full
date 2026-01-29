using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<List<Player>> GetPlayersAsync()
        {
            return await _playerRepository.GetPlayersAsync();
        }

        public async Task<Player?> GetPlayerByIdAsync(int id)
        {
            return await _playerRepository.GetPlayerByIdAsync(id);
        }

        public async Task<Player> AddPlayerAsync(Player player)
        {
            return await _playerRepository.AddPlayerAsync(player);
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            await _playerRepository.UpdatePlayerAsync(player);
        }

        public async Task DeletePlayerAsync(int id)
        {
            await _playerRepository.DeletePlayerAsync(id);
        }

        public async Task<bool> PlayerExistsAsync(int id)
        {
            return await _playerRepository.PlayerExistsAsync(id);
        }

        public async Task<List<Player>> GetPlayersByNameAsync(string name)
        {
            return await _playerRepository.GetPlayersByNameAsync(name);
        }

        public async Task<List<Player>> GetActivePlayersByEventAsync(int eventId)
        {
            return await _playerRepository.GetActivePlayersByEventAsync(eventId);
        }

        public async Task RegisterPlayerToEventAsync(int playerId, int eventId)
        {
            await _playerRepository.RegisterPlayerToEventAsync(playerId, eventId);
        }

        public async Task<bool> IsPlayerInEventAsync(int playerId, int eventId)
        {
            return await _playerRepository.IsPlayerInEventAsync(playerId, eventId);
        }
    }
}
