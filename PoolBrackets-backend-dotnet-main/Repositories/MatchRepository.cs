using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Data;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _context;

        public MatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Match?> GetMatchByIdAsync(int id)
        {
            return await _context.Matches
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Match>> GetMatchesByEventAsync(int eventId)
        {
            return await _context.Matches
                .Where(m => m.EventId == eventId)
                .Include(m => m.FirstPlayer)
                .Include(m => m.SecondPlayer)
                .OrderBy(m => m.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateMatchAsync(Match match)
        {
            _context.Entry(match).State = EntityState.Modified;
            match.UpdateAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task AddMatchAsync(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }
    }
}