using System;

namespace PoolBrackets_backend_dotnet.DTOs
{
    /// <summary>
    /// Standard error response model for consistent API error handling
    /// </summary>
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
