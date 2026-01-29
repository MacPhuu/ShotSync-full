using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.Models
{
    [Table("players")]
    public class Player
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
        [Column("email")]
        public required string Email { get; set; }
        [Column("nation")]
        public required string Nation { get; set; }
        [Column("portrait")]
        public string? Portrait { get; set; }
        [Column("point")]
        public int? Point { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }  // Nullable if some players are not system users yet, but for this feature we need it.

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Column("is_active")]
        public required bool IsActive { get; set; }
        [Column("create_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [Column("update_at")]
        public DateTime? UpdateAt { get; set; }

    }
}
