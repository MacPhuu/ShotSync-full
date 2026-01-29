using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.Models
{
    [Table("player_histories")]
    public class PlayerHistory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("player_id")]
        public int PlayerId { get; set; }

        [Column("event_id")]
        public int EventId { get; set; }

        [Column("match_id")]
        public int MatchId { get; set; }

        [Column("result")]
        public string Result { get; set; } = string.Empty;

        [Column("opponent_id")]
        public int OpponentId { get; set; }

        [Column("match_date")]
        public DateTime MatchDate { get; set; }
        
        [ForeignKey("PlayerId")]
        public Player? Player { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}