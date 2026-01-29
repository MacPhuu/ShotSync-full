using PoolBrackets_backend_dotnet.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.Models
{
    [Table("matches")]
    public class Match
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        #region RELATIONSHIPS (LIÊN KẾT)

        [Required]
        [Column("event_id")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event? Event { get; set; }

        [Column("first_player_id")]
        public int? FirstPlayerId { get; set; }

        [ForeignKey("FirstPlayerId")]
        public virtual Player? FirstPlayer { get; set; }

        [Column("second_player_id")]
        public int? SecondPlayerId { get; set; }

        [ForeignKey("SecondPlayerId")]
        public virtual Player? SecondPlayer { get; set; }

        #endregion

        #region MATCH SPECS (THÔNG SỐ TRẬN ĐẤU)

        [Column("table_number")]
        public string? TableNumber { get; set; }

        [Column("first_player_point")]
        public int FirstPlayerPoint { get; set; } = 0;

        [Column("second_player_point")]
        public int SecondPlayerPoint { get; set; } = 0;

        [Column("race_to")]
        public int RaceTo { get; set; } = 9;

        #endregion

        #region STATUS & LOGIC (TRẠNG THÁI & NGHIỆP VỤ)

        [Column("is_start")]
        public bool IsStart { get; set; } = false;

        [Column("is_finish")]
        public bool IsFinish { get; set; } = false;

        [Column("round_name")]
        public string? RoundName { get; set; }

        /// <summary>
        /// Phân loại nhánh: 1 - Nhánh thắng (Winners), 2 - Nhánh thua (Losers), 3 - Knock-out
        /// </summary>
        [Column("round_type")]
        public int RoundType { get; set; } 

        #endregion

        #region TOURNAMENT TREE (CÂY SƠ ĐỒ)

        [Column("next_match_id_win")]
        public int? NextMatchIdWin { get; set; }

        [Column("next_match_id_lose")]
        public int? NextMatchIdLose { get; set; }

        /// <summary>
        /// Vị trí người thắng/thua sẽ nhảy vào ở trận kế tiếp: 1 hoặc 2
        /// </summary>
        [Column("next_match_position")]
        public int NextMatchPosition { get; set; }

        #endregion

        #region TIMESTAMPS

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? UpdateAt { get; set; }

        #endregion
    }
}