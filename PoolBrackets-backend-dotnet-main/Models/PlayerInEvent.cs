using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.Models 
{
    [Table("player_in_events")]
    public class PlayerInEvent 
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("player_id")]
        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player? Player { get; set; }

        [Column("event_id")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}