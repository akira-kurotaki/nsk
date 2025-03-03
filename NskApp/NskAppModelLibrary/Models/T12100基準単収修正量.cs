using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12100_基準単収修正量
    /// </summary>
    [Serializable]
    [Table("t_12100_基準単収修正量")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(支所コード))]
    public class T12100基準単収修正量 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 1)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("年産", Order = 2)]
        public short 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 3)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 4)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Required]
        [Column("支所コード", Order = 5)]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 適用引受回
        /// </summary>
        [Column("適用引受回")]
        public short? 適用引受回 { get; set; }

        /// <summary>
        /// 基準修正量
        /// </summary>
        [Column("基準修正量")]
        public Decimal? 基準修正量 { get; set; }

        /// <summary>
        /// 前回修正量
        /// </summary>
        [Column("前回修正量")]
        public Decimal? 前回修正量 { get; set; }

        /// <summary>
        /// 修正量
        /// </summary>
        [Column("修正量")]
        public Decimal? 修正量 { get; set; }

        /// <summary>
        /// 基準単収修正量計算実施日
        /// </summary>
        [Column("基準単収修正量計算実施日")]
        public DateTime? 基準単収修正量計算実施日 { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("登録日時")]
        public DateTime? 登録日時 { get; set; }

        /// <summary>
        /// 登録ユーザid
        /// </summary>
        [Column("登録ユーザid")]
        [StringLength(11)]
        public string 登録ユーザid { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("更新日時")]
        public DateTime? 更新日時 { get; set; }

        /// <summary>
        /// 更新ユーザid
        /// </summary>
        [Column("更新ユーザid")]
        [StringLength(11)]
        public string 更新ユーザid { get; set; }
    }
}
