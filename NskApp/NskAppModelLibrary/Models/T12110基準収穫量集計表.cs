using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12110_基準収穫量集計表
    /// </summary>
    [Serializable]
    [Table("t_12110_基準収穫量集計表")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(支所コード), nameof(大地区コード), nameof(小地区コード), nameof(収量等級コード), nameof(品種コード), nameof(参酌コード))]
    public class T12110基準収穫量集計表 : ModelBase
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
        /// 大地区コード
        /// </summary>
        [Required]
        [Column("大地区コード", Order = 6)]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        [Required]
        [Column("小地区コード", Order = 7)]
        [StringLength(4)]
        public string 小地区コード { get; set; }

        /// <summary>
        /// 収量等級コード
        /// </summary>
        [Required]
        [Column("収量等級コード", Order = 8)]
        [StringLength(3)]
        public string 収量等級コード { get; set; }

        /// <summary>
        /// 品種コード
        /// </summary>
        [Required]
        [Column("品種コード", Order = 9)]
        [StringLength(3)]
        public string 品種コード { get; set; }

        /// <summary>
        /// 参酌コード
        /// </summary>
        [Required]
        [Column("参酌コード", Order = 10)]
        [StringLength(3)]
        public string 参酌コード { get; set; }

        /// <summary>
        /// 筆数
        /// </summary>
        [Column("筆数")]
        public Decimal? 筆数 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Column("引受面積")]
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 基準収穫量_修正前
        /// </summary>
        [Column("基準収穫量_修正前")]
        public Decimal? 基準収穫量_修正前 { get; set; }

        /// <summary>
        /// 基準収穫量_100p
        /// </summary>
        [Column("基準収穫量_100p")]
        public Decimal? 基準収穫量_100p { get; set; }

        /// <summary>
        /// 基準収穫量_修正後
        /// </summary>
        [Column("基準収穫量_修正後")]
        public Decimal? 基準収穫量_修正後 { get; set; }

        /// <summary>
        /// 収量
        /// </summary>
        [Column("収量")]
        public Decimal? 収量 { get; set; }

        /// <summary>
        /// 品種名等
        /// </summary>
        [Column("品種名等")]
        [StringLength(20)]
        public string 品種名等 { get; set; }

        /// <summary>
        /// 品種係数
        /// </summary>
        [Column("品種係数")]
        public Decimal? 品種係数 { get; set; }

        /// <summary>
        /// 参酌係数
        /// </summary>
        [Column("参酌係数")]
        public Decimal? 参酌係数 { get; set; }

        /// <summary>
        /// 基準修正量
        /// </summary>
        [Column("基準修正量")]
        public Decimal? 基準修正量 { get; set; }

        /// <summary>
        /// 修正量
        /// </summary>
        [Column("修正量")]
        public Decimal? 修正量 { get; set; }

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
    }
}
