using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> GetMatchesByEventAsync(int eventId)
        {
            return await _matchRepository.GetMatchesByEventAsync(eventId);
        }

        public async Task<Match?> GetMatchByIdAsync(int id)
        {
            return await _matchRepository.GetMatchByIdAsync(id);
        }

        public async Task UpdateMatchAsync(Match match)
        {
            await _matchRepository.UpdateMatchAsync(match);
        }

        public async Task AddMatchAsync(Match match)
        {
            await _matchRepository.AddMatchAsync(match);
        }
    }
}
