using System.ComponentModel.DataAnnotations.Schema;
using PoolBrackets_backend_dotnet.Models.Enums; // Nhớ using Enum

namespace PoolBrackets_backend_dotnet.Models
{
    [Table("events")]
    public class Event
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public required string Name { get; set; }

        [Column("venue")]
        public required string Venue { get; set; }

        [Column("location")]
        public required string Location { get; set; }

        [Column("date")]
        public required DateTime Date { get; set; }

        [Column("number_of_players")]
        public required int NumberOfPlayers { get; set; }

        [Column("slogan")]
        public string? Slogan { get; set; }

        [Column("format")]
        public string? Format { get; set; }

        [Column("entry_fee")]
        public decimal? EntryFee { get; set; }

        [Column("total_prize")]
        public decimal? TotalPrize { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("status")]
        public EventStatus Status { get; set; } = EventStatus.Draft; // Mặc định là Nháp

        [Column("is_displayed")]
        public required bool IsDisplayed { get; set; }

        [Column("is_happen")]
        public bool IsHappen { get; set; } = false;

        [Column("create_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Column("update_at")]
        public DateTime? UpdateAt { get; set; }

        [Column("host_id")]
        public int? HostId { get; set; }

        [ForeignKey("HostId")]
        public User? Host { get; set; }
    }
}