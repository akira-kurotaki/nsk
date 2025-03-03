using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24110_産地別銘柄別評価情報_規格別
    /// </summary>
    [Serializable]
    [Table("t_24110_産地別銘柄別評価情報_規格別")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(産地別銘柄コード), nameof(営農対象外フラグ), nameof(類区分), nameof(精算区分), nameof(規格))]
    public class T24110産地別銘柄別評価情報規格別 : ModelBase
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
        /// 産地別銘柄コード
        /// </summary>
        [Required]
        [Column("産地別銘柄コード", Order = 5)]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Required]
        [Column("営農対象外フラグ", Order = 6)]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 7)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 精算区分
        /// </summary>
        [Required]
        [Column("精算区分", Order = 8)]
        [StringLength(1)]
        public string 精算区分 { get; set; }

        /// <summary>
        /// 規格
        /// </summary>
        [Required]
        [Column("規格", Order = 9)]
        [StringLength(2)]
        public string 規格 { get; set; }

        /// <summary>
        /// 収穫量
        /// </summary>
        [Column("収穫量")]
        public Decimal? 収穫量 { get; set; }

        /// <summary>
        /// 分割減収量
        /// </summary>
        [Column("分割減収量")]
        public Decimal? 分割減収量 { get; set; }

        /// <summary>
        /// 分割後収穫量
        /// </summary>
        [Column("分割後収穫量")]
        public Decimal? 分割後収穫量 { get; set; }

        /// <summary>
        /// 品質指数
        /// </summary>
        [Column("品質指数")]
        public Decimal? 品質指数 { get; set; }

        /// <summary>
        /// 調整後収穫量
        /// </summary>
        [Column("調整後収穫量")]
        public Decimal? 調整後収穫量 { get; set; }

        /// <summary>
        /// ｋｇ当たり単価
        /// </summary>
        [Column("ｋｇ当たり単価")]
        public Decimal? ｋｇ当たり単価 { get; set; }

        /// <summary>
        /// 生産金額
        /// </summary>
        [Column("生産金額")]
        public Decimal? 生産金額 { get; set; }

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
