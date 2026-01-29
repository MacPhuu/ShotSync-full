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
    public class PlayerHistoryRepository : IPlayerHistoryRepository
    {
        private readonly AppDbContext _context;

        public PlayerHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        #region 1. CRUD IMPLEMENTATION (Sửa lỗi thiếu AddAsync, UpdateAsync)

        public async Task<IEnumerable<PlayerHistory>> GetAllAsync()
        {
            return await _context.PlayerHistories.AsNoTracking().ToListAsync();
        }

        public async Task<PlayerHistory?> GetByIdAsync(int id)
        {
            return await _context.PlayerHistories.FindAsync(id);
        }

        public async Task<PlayerHistory> AddAsync(PlayerHistory history)
        {
            _context.PlayerHistories.Add(history);
            await _context.SaveChangesAsync();
            return history;
        }

        public async Task UpdateAsync(PlayerHistory history)
        {
            _context.Entry(history).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var history = await _context.PlayerHistories.FindAsync(id);
            if (history == null) return false;

            _context.PlayerHistories.Remove(history);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region 2. BUSINESS LOGIC (Sửa lỗi thiếu GetHistoryByPlayerIdAsync, LogMatchResultAsync)

        public async Task<IEnumerable<PlayerHistory>> GetHistoryByPlayerIdAsync(int playerId)
        {
            return await _context.PlayerHistories
                .Where(h => h.PlayerId == playerId)
                .OrderByDescending(h => h.MatchDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task LogMatchResultAsync(int playerId, int matchId, string result, int opponentId)
        {
            var history = new PlayerHistory
            {
                PlayerId = playerId,
                MatchId = matchId,
                Result = result,
                OpponentId = opponentId,
                MatchDate = DateTime.Now
            };
            _context.PlayerHistories.Add(history);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}