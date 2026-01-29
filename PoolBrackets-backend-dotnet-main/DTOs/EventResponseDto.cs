namespace PoolBrackets_backend_dotnet.DTOs
{
    /// <summary>
    /// DTO for Event GET responses
    /// </summary>
    public class EventResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Venue { get; set; }
        public string? Location { get; set; }
        public DateTime? Date { get; set; }
        public bool IsDisplayed { get; set; }
        public int NumberOfPlayers { get; set; }
        public bool IsHappen { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
