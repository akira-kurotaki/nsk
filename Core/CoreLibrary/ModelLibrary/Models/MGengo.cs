using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 元号マスタ
    /// </summary>
    [Serializable]
    [Table("m_gengo")]
    public class MGengo : ModelBase
    {
        /// <summary>
        /// 元号
        /// </summary>
        [Required]
        [Key]
        [Column("gengo", Order = 1)]
        [StringLength(2)]
        public string Gengo { get; set; }

        /// <summary>
        /// 略号
        /// </summary>
        [Column("ryakugo")]
        [StringLength(1)]
        public string Ryakugo { get; set; }

        /// <summary>
        /// 英略号
        /// </summary>
        [Column("ryakugo_en")]
        [StringLength(1)]
        public string RyakugoEn { get; set; }

        /// <summary>
        /// 適用開始年月日
        /// </summary>
        [Column("tekiyo_start_ymd")]
        public DateTime? TekiyoStartYmd { get; set; }

        /// <summary>
        /// 適用終了年月日
        /// </summary>
        [Column("tekiyo_end_ymd")]
        public DateTime? TekiyoEndYmd { get; set; }

        /// <summary>
        /// 登録ユーザID
        /// </summary>
        [Column("insert_user_id")]
        [StringLength(11)]
        public string InsertUserId { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("insert_date")]
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// 更新ユーザID
        /// </summary>
        [Column("update_user_id")]
        [StringLength(11)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
