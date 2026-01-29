namespace PoolBrackets_backend_dotnet.DTOs
{
    /// <summary>
    /// DTO for User GET responses - excludes sensitive password field
    /// </summary>
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Nation { get; set; }
        public string? Avatar { get; set; }
        public int Role { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
