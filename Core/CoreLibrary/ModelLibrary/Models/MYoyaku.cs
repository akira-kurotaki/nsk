using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 予約処理マスタ
    /// </summary>
    [Serializable]
    [Table("m_yoyaku")]
    [PrimaryKey(nameof(SystemKbn), nameof(BatchNm))]
    public class MYoyaku : ModelBase
    {
        /// <summary>
        /// システム区分
        /// </summary>
        [Required]
        [Column("system_kbn", Order = 1)]
        [StringLength(2)]
        public string SystemKbn { get; set; }

        /// <summary>
        /// バッチ名
        /// </summary>
        [Required]
        [Column("batch_nm", Order = 2)]
        [StringLength(30)]
        public string BatchNm { get; set; }

        /// <summary>
        /// 実行対象バッチパス
        /// </summary>
        [Required]
        [Column("batch_path")]
        [StringLength(300)]
        public string BatchPath { get; set; }

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
