using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12060_産地別銘柄別引受情報
    /// </summary>
    [Serializable]
    [Table("t_12060_産地別銘柄別引受情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(引受回), nameof(類区分), nameof(産地別銘柄コード), nameof(営農対象外フラグ))]
    public class T12060産地別銘柄別引受情報 : ModelBase
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
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 4)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("引受回", Order = 5)]
        public short 引受回 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 6)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Required]
        [Column("産地別銘柄コード", Order = 7)]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Required]
        [Column("営農対象外フラグ", Order = 8)]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Column("大地区コード")]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        [Column("小地区コード")]
        [StringLength(4)]
        public string 小地区コード { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("支所コード")]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 合併時識別
        /// </summary>
        [Column("合併時識別")]
        [StringLength(3)]
        public string 合併時識別 { get; set; }

        /// <summary>
        /// 引受筆数
        /// </summary>
        [Column("引受筆数")]
        public Decimal? 引受筆数 { get; set; }

        /// <summary>
        /// 調整後平均収穫量合計
        /// </summary>
        [Column("調整後平均収穫量合計")]
        public Decimal? 調整後平均収穫量合計 { get; set; }

        /// <summary>
        /// 調整後平均単収
        /// </summary>
        [Column("調整後平均単収")]
        public Decimal? 調整後平均単収 { get; set; }

        /// <summary>
        /// 単位当たり共済金額
        /// </summary>
        [Column("単位当たり共済金額")]
        public Decimal? 単位当たり共済金額 { get; set; }

        /// <summary>
        /// 仕向割合_民間
        /// </summary>
        [Column("仕向割合_民間")]
        public Decimal? 仕向割合_民間 { get; set; }

        /// <summary>
        /// 引受面積_民間
        /// </summary>
        [Column("引受面積_民間")]
        public Decimal? 引受面積_民間 { get; set; }

        /// <summary>
        /// 引受面積合計
        /// </summary>
        [Column("引受面積合計")]
        public Decimal? 引受面積合計 { get; set; }

        /// <summary>
        /// 単位当たり基準生産金額_民間
        /// </summary>
        [Column("単位当たり基準生産金額_民間")]
        public Decimal? 単位当たり基準生産金額_民間 { get; set; }

        /// <summary>
        /// 基準生産金額_民間
        /// </summary>
        [Column("基準生産金額_民間")]
        public Decimal? 基準生産金額_民間 { get; set; }

        /// <summary>
        /// 基準生産金額合計
        /// </summary>
        [Column("基準生産金額合計")]
        public Decimal? 基準生産金額合計 { get; set; }

        /// <summary>
        /// 基準単収_民間
        /// </summary>
        [Column("基準単収_民間")]
        public Decimal? 基準単収_民間 { get; set; }

        /// <summary>
        /// 基準収穫量_民間
        /// </summary>
        [Column("基準収穫量_民間")]
        public Decimal? 基準収穫量_民間 { get; set; }

        /// <summary>
        /// 基準収穫量合計
        /// </summary>
        [Column("基準収穫量合計")]
        public Decimal? 基準収穫量合計 { get; set; }

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
