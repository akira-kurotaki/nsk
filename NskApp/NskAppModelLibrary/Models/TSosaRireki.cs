using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// 操作履歴
    /// </summary>
    [Serializable]
    [Table("t_sosa_rireki")]
    [Keyless]
    public class TSosaRireki : ModelBase
    {
        /// <summary>
        /// 農業者ID (FK)
        /// </summary>
        [Required]
        [Column("nogyosha_id")]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 操作日時
        /// </summary>
        [Required]
        [Column("sosa_date")]
        public DateTime SosaDate { get; set; }

        /// <summary>
        /// ユーザID
        /// </summary>
        [Column("user_id")]
        [StringLength(11)]
        public string UserId { get; set; }

        /// <summary>
        /// 画面ID
        /// </summary>
        [Column("screen_id")]
        [StringLength(9)]
        public string ScreenId { get; set; }

        /// <summary>
        /// システム利用者区分
        /// </summary>
        [Column("system_riyo_kbn")]
        [StringLength(1)]
        public string SystemRiyoKbn { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("shisho_cd")]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        [Column("sosa_naiyo")]
        [StringLength(100)]
        public string SosaNaiyo { get; set; }

        /// <summary>
        /// 表示色
        /// </summary>
        [Column("display_color")]
        [StringLength(1)]
        public string DisplayColor { get; set; }
    }
}
