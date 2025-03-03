using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24080_規格別収穫量等配分計算
    /// </summary>
    [Serializable]
    [Table("t_24080_規格別収穫量等配分計算")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(産地別銘柄コード), nameof(営農対象外フラグ), nameof(類区分), nameof(精算区分))]
    public class T24080規格別収穫量等配分計算 : ModelBase
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
        /// 出荷数量等合計
        /// </summary>
        [Column("出荷数量等合計")]
        public Decimal? 出荷数量等合計 { get; set; }

        /// <summary>
        /// 規格等別出荷割合計
        /// </summary>
        [Column("規格等別出荷割合計")]
        public Decimal? 規格等別出荷割合計 { get; set; }

        /// <summary>
        /// 規格別数量合計
        /// </summary>
        [Column("規格別数量合計")]
        public Decimal? 規格別数量合計 { get; set; }

        /// <summary>
        /// 分割減収量合計
        /// </summary>
        [Column("分割減収量合計")]
        public Decimal? 分割減収量合計 { get; set; }

        /// <summary>
        /// 規格別割合計
        /// </summary>
        [Column("規格別割合計")]
        public Decimal? 規格別割合計 { get; set; }

        /// <summary>
        /// 分割減収量合計２
        /// </summary>
        [Column("分割減収量合計２")]
        public Decimal? 分割減収量合計２ { get; set; }

        /// <summary>
        /// 分割減収量合計３
        /// </summary>
        [Column("分割減収量合計３")]
        public Decimal? 分割減収量合計３ { get; set; }

        /// <summary>
        /// 分割減収量合計計
        /// </summary>
        [Column("分割減収量合計計")]
        public Decimal? 分割減収量合計計 { get; set; }

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
