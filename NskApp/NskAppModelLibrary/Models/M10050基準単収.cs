using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10050_基準単収
    /// </summary>
    [Serializable]
    [Table("m_10050_基準単収")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(支所コード))]
    public class M10050基準単収 : ModelBase
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
        /// 県通知指示単収
        /// </summary>
        [Column("県通知指示単収")]
        public Decimal? 県通知指示単収 { get; set; }

        /// <summary>
        /// 目標値
        /// </summary>
        [Column("目標値")]
        public Decimal? 目標値 { get; set; }

        /// <summary>
        /// 最高収量
        /// </summary>
        [Column("最高収量")]
        public Decimal? 最高収量 { get; set; }

        /// <summary>
        /// 等級個数
        /// </summary>
        [Column("等級個数")]
        public Decimal? 等級個数 { get; set; }

        /// <summary>
        /// 巾
        /// </summary>
        [Column("巾")]
        public Decimal? 巾 { get; set; }

        /// <summary>
        /// 篩目修正係数
        /// </summary>
        [Column("篩目修正係数")]
        public Decimal? 篩目修正係数 { get; set; }

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
