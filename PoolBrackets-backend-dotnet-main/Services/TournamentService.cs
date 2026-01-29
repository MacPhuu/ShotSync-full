using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IMatchRepository _matchRepo;
        private readonly IPlayerRepository _playerRepo;
        private readonly IEventRepository _eventRepo;

        public TournamentService(IMatchRepository matchRepo, IPlayerRepository playerRepo, IEventRepository eventRepo)
        {
            _matchRepo = matchRepo;
            _playerRepo = playerRepo;
            _eventRepo = eventRepo;
        }

        #region 1. TẠO SƠ ĐỒ THI ĐẤU (HYBRID: 2 MẠNG -> 1 MẠNG)

        public async Task GenerateBracketAsync(int eventId, int numberOfPlayers)
        {
            // 1. Kiểm tra tồn tại và tính hợp lệ
            var existingMatches = await _matchRepo.GetMatchesByEventAsync(eventId);
            if (existingMatches.Any()) 
                throw new InvalidOperationException("Sơ đồ thi đấu đã được tạo cho giải này.");

            if (!IsPowerOfTwo(numberOfPlayers))
                throw new Exception("Số lượng vận động viên phải là lũy thừa của 2 (16, 32, 64, 128).");

            // 2. Lấy danh sách VĐV và xáo trộn
            var players = (await _playerRepo.GetActivePlayersByEventAsync(eventId)).ToList();
            var rng = new Random();
            var shuffledPlayers = players.OrderBy(a => rng.Next()).ToList();

            // --- GIAI ĐOẠN 1: VÒNG LOẠI 2 MẠNG (QUALIFYING) ---
            // Mục tiêu: Lọc từ N xuống N/2 người vào vòng Knock-out
            
            // 1.1. Winners Round 1 (Ví dụ 32 VĐV -> 16 trận)
            var winnersR1 = new List<Match>();
            int matchesR1Count = numberOfPlayers / 2;
            for (int i = 0; i < matchesR1Count; i++)
            {
                var m = await CreateMatch(eventId, "Vòng loại 1 (Nhánh thắng)", 1, 9, true);
                m.FirstPlayerId = (i * 2 < shuffledPlayers.Count) ? shuffledPlayers[i * 2].Id : null;
                m.SecondPlayerId = (i * 2 + 1 < shuffledPlayers.Count) ? shuffledPlayers[i * 2 + 1].Id : null;
                await _matchRepo.UpdateMatchAsync(m);
                winnersR1.Add(m);
            }

            // 1.2. Winners Qualify (Thắng trận này là vào vòng Knock-out)
            var winnersQualify = new List<Match>();
            for (int i = 0; i < matchesR1Count / 2; i++)
            {
                var m = await CreateMatch(eventId, "Vòng loại 2 (Nhánh thắng)", 1, 9, false);
                winnersQualify.Add(m);
                await LinkMatches(winnersR1[i * 2], m, 1, true); // Thắng R1 vào vị trí 1
                await LinkMatches(winnersR1[i * 2 + 1], m, 2, true); // Thắng R1 vào vị trí 2
            }

            // 1.3. Losers Round 1 (Những người thua ở Winners R1 gặp nhau)
            var losersR1 = new List<Match>();
            for (int i = 0; i < matchesR1Count / 2; i++)
            {
                var m = await CreateMatch(eventId, "Vòng loại 1 (Nhánh thua)", 2, 9, false);
                losersR1.Add(m);
                await LinkMatches(winnersR1[i * 2], m, 1, false); // Thua R1 rơi xuống vị trí 1
                await LinkMatches(winnersR1[i * 2 + 1], m, 2, false); // Thua R1 rơi xuống vị trí 2
            }

            // 1.4. Losers Qualify (Thắng trận này là vào vòng Knock-out qua "vé vớt")
            var losersQualify = new List<Match>();
            for (int i = 0; i < matchesR1Count / 2; i++)
            {
                var m = await CreateMatch(eventId, "Vòng loại 2 (Nhánh thua)", 2, 9, false);
                losersQualify.Add(m);
                // Người thắng ở Nhánh thua R1 đấu với người thua ở Nhánh thắng Qualify
                await LinkMatches(losersR1[i], m, 1, true);
                await LinkMatches(winnersQualify[i], m, 2, false);
            }

            // --- GIAI ĐOẠN 2: VÒNG CHUNG KẾT (SINGLE ELIMINATION) ---
            // Số lượng người hội quân = (Thắng nhánh thắng) + (Thắng nhánh thua) = N/2
            var qualifiers = winnersQualify.Concat(losersQualify).ToList();
            await BuildKnockoutTree(eventId, numberOfPlayers / 2, qualifiers);
        }

        private async Task BuildKnockoutTree(int eventId, int participants, List<Match> seeds)
        {
            int rounds = (int)Math.Log2(participants);
            List<Match> currentRoundMatches = new List<Match>();

            // Vòng Knock-out 1 (1/8 hoặc 1/4 tùy số lượng)
            int firstKOCount = participants / 2;
            for (int i = 0; i < firstKOCount; i++)
            {
                int race = GetRaceToByRound(rounds, 1);
                var m = await CreateMatch(eventId, GetRoundName(rounds, 1), 3, race, false);
                currentRoundMatches.Add(m);

                // Nối từ các trận Qualifying
                await LinkMatches(seeds[i * 2], m, 1, true);
                await LinkMatches(seeds[i * 2 + 1], m, 2, true);
            }

            // Các vòng tiếp theo cho đến Chung kết
            for (int r = 2; r <= rounds; r++)
            {
                List<Match> nextRound = new List<Match>();
                int matchesCount = currentRoundMatches.Count / 2;
                for (int i = 0; i < matchesCount; i++)
                {
                    int race = GetRaceToByRound(rounds, r);
                    var m = await CreateMatch(eventId, GetRoundName(rounds, r), 3, race, false);
                    nextRound.Add(m);

                    await LinkMatches(currentRoundMatches[i * 2], m, 1, true);
                    await LinkMatches(currentRoundMatches[i * 2 + 1], m, 2, true);
                }
                currentRoundMatches = nextRound;
            }
        }

        #endregion

        #region 2. XỬ LÝ KẾT QUẢ (MATCH LOGIC)

        public async Task ProcessMatchResultAsync(int matchId)
        {
            var match = await _matchRepo.GetMatchByIdAsync(matchId);
            if (match == null || !match.IsFinish) return;

            int? winnerId = (match.FirstPlayerPoint > match.SecondPlayerPoint) ? match.FirstPlayerId : match.SecondPlayerId;
            int? loserId = (match.FirstPlayerPoint > match.SecondPlayerPoint) ? match.SecondPlayerId : match.FirstPlayerId;

            // 2.1. Đẩy người thắng lên trận tiếp theo
            if (winnerId != null && match.NextMatchIdWin != null)
            {
                await PushPlayerToNextMatch(winnerId.Value, match.NextMatchIdWin.Value, match.NextMatchPosition);
            }

            // 2.2. Đẩy người thua xuống nhánh thua (Chỉ dành cho Qualifying)
            if (loserId != null && match.NextMatchIdLose != null)
            {
                // Đối với nhánh thua, vị trí mặc định theo NextMatchPosition của trận gốc
                await PushPlayerToNextMatch(loserId.Value, match.NextMatchIdLose.Value, match.NextMatchPosition);
            }
        }

        private async Task PushPlayerToNextMatch(int playerId, int nextMatchId, int position)
        {
            var nextMatch = await _matchRepo.GetMatchByIdAsync(nextMatchId);
            if (nextMatch == null) return;

            if (position == 1) nextMatch.FirstPlayerId = playerId;
            else nextMatch.SecondPlayerId = playerId;

            // Nếu đủ 2 người thì kích hoạt trận đấu
            if (nextMatch.FirstPlayerId != null && nextMatch.SecondPlayerId != null)
                nextMatch.IsStart = true;

            nextMatch.UpdateAt = DateTime.Now;
            await _matchRepo.UpdateMatchAsync(nextMatch);
        }

        #endregion

        #region 3. HELPERS

        private async Task<Match> CreateMatch(int eventId, string name, int type, int race, bool start)
        {
            var m = new Match
            {
                EventId = eventId,
                RoundName = name,
                RoundType = type, 
                RaceTo = race,
                IsStart = start,
                IsFinish = false,
                CreatedAt = DateTime.Now
            };

            // BƯỚC CHỈNH SỬA:
            await _matchRepo.AddMatchAsync(m); // Gọi hàm thêm vào DB (không return dòng này)
            return m; // Trả về đối tượng m đã có ID sau khi SaveChanges
        }

        private async Task LinkMatches(Match current, Match next, int position, bool win)
        {
            if (win) current.NextMatchIdWin = next.Id;
            else current.NextMatchIdLose = next.Id;

            current.NextMatchPosition = position;
            await _matchRepo.UpdateMatchAsync(current);
        }

        private int GetRaceToByRound(int totalRounds, int currentRound)
        {
            if (currentRound == totalRounds) return 13; // Chung kết
            if (currentRound >= totalRounds - 1) return 12; // Tứ kết & Bán kết
            return 9; // Các vòng trước đó
        }

        private string GetRoundName(int totalRounds, int currentRound)
        {
            if (currentRound == totalRounds) return "Chung kết";
            if (currentRound == totalRounds - 1) return "Bán kết";
            if (currentRound == totalRounds - 2) return "Tứ kết";
            return $"Vòng loại trực tiếp 1/{Math.Pow(2, totalRounds - currentRound + 1)}";
        }

        private bool IsPowerOfTwo(int n) => n > 0 && (n & (n - 1)) == 0;

        public async Task GenerateKnockoutPhaseAsync(int eventId) => await Task.CompletedTask;

        #endregion
    }
}