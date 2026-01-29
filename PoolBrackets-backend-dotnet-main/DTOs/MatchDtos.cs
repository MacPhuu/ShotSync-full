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
        public int FirstPlayerPoint { get; set; }
        public int SecondPlayerPoint { get; set; }
        public bool IsFinish { get; set; }
    }
}