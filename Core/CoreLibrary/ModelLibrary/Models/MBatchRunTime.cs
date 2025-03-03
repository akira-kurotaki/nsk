using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// バッチ実行可能時間帯
    /// </summary>
    [Serializable]
    [Table("m_batch_run_time")]
    public class MBatchRunTime : ModelBase
    {
        /// <summary>
        /// バッチ種類
        /// </summary>
        [Required]
        [Key]
        [Column("batch_type", Order = 1)]
        [StringLength(1)]
        public string BatchType { get; set; }

        /// <summary>
        /// 実行可能時刻開始
        /// </summary>
        [Required]
        [Column("batch_run_start_time")]
        [StringLength(8)]
        public string BatchRunStartTime { get; set; }

        /// <summary>
        /// 実行可能時刻終了
        /// </summary>
        [Required]
        [Column("batch_run_end_time")]
        [StringLength(8)]
        public string BatchRunEndTime { get; set; }

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
