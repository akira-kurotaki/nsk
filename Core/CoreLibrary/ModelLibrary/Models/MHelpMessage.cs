using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// ヘルプメッセージマスタ
    /// </summary>
    [Serializable]
    [Table("m_help_message")]
    [PrimaryKey(nameof(TekiyoStartYmd), nameof(ScreenId), nameof(ItemNo))]
    public class MHelpMessage : ModelBase
    {
        /// <summary>
        /// 適用開始年月日
        /// </summary>
        [Required]
        [Column("tekiyo_start_ymd", Order = 1)]
        public DateTime TekiyoStartYmd { get; set; }

        /// <summary>
        /// 適用終了年月日
        /// </summary>
        [Column("tekiyo_end_ymd")]
        public DateTime? TekiyoEndYmd { get; set; }

        /// <summary>
        /// 画面ID
        /// </summary>
        [Required]
        [Column("screen_id", Order = 2)]
        [StringLength(9)]
        public string ScreenId { get; set; }

        /// <summary>
        /// 項目NO.
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("item_no", Order = 3)]
        public short ItemNo { get; set; }

        /// <summary>
        /// 項目名
        /// </summary>
        [Column("item_nm")]
        [StringLength(100)]
        public string ItemNm { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        [Column("message")]
        [StringLength(1000)]
        public string Message { get; set; }

        /// <summary>
        /// 表示フラグ
        /// </summary>
        [Required]
        [Column("display_flg")]
        [StringLength(1)]
        public string DisplayFlg { get; set; }
    }
}
