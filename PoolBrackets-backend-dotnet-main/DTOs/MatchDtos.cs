namespace PoolBrackets_backend_dotnet.DTOs
{
    public class UpdateScoreDto
    {
        public int FirstPlayerScore { get; set; }
        public int SecondPlayerScore { get; set; }
    }

    public class MatchResponseDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string? RoundName { get; set; }
        public int RoundType { get; set; }
        public string? TableNumber { get; set; }
        public int? FirstPlayerId { get; set; }
        public string? FirstPlayerName { get; set; }
        public int FirstPlayerPoint { get; set; }
        public int? SecondPlayerId { get; set; }
        public string? SecondPlayerName { get; set; }
        public int SecondPlayerPoint { get; set; }
        public bool IsFinish { get; set; }
        public bool IsStart { get; set; }
        public int NextMatchPosition { get; set; }
    }
}