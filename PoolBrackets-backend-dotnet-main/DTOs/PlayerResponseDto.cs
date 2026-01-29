namespace PoolBrackets_backend_dotnet.DTOs
{
    /// <summary>
    /// DTO for Player GET responses
    /// </summary>
    public class PlayerResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Nation { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
