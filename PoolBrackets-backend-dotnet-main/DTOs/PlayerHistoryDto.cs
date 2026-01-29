namespace PoolBrackets_backend_dotnet.DTOs
{
    public class PlayerHistoryDto
    {
        public int PlayerId { get; set; }
        
        // --- FIX LỖI CS1061: Thêm các trường thiếu ---
        public int MatchId { get; set; }
        
        public string Result { get; set; } = string.Empty;
        
        public int OpponentId { get; set; }

        public int EventId { get; set; }
    }
}