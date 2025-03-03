using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// システムロック
    /// </summary>
    [Serializable]
    [Table("t_sys_lock")]
    [PrimaryKey(nameof(SystemKbn), nameof(TodofukenCd), nameof(LockDate))]
    public class TSysLock : ModelBase
    {
        /// <summary>
        /// システム区分
        /// </summary>
        [Required]
        [Column("system_kbn", Order = 1)]
        [StringLength(2)]
        public string SystemKbn { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd", Order = 2)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// ロック開始日時
        /// </summary>
        [Required]
        [Column("lock_date", Order = 3)]
        public DateTime LockDate { get; set; }

        /// <summary>
        /// ロック終了日時
        /// </summary>
        [Column("lock_end_date")]
        public DateTime? LockEndDate { get; set; }

        /// <summary>
        /// ロック実行ユーザ
        /// </summary>
        [Required]
        [Column("lock_user_id")]
        [StringLength(11)]
        public string LockUserId { get; set; }

        /// <summary>
        /// ロック実行処理
        /// </summary>
        [Required]
        [Column("lock_shori")]
        [StringLength(30)]
        public string LockShori { get; set; }
    }
}
