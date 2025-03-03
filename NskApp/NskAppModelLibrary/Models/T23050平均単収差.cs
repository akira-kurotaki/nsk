using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23050_平均単収差
    /// </summary>
    [Serializable]
    [Table("t_23050_平均単収差")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(支所コード), nameof(類区分), nameof(引受方式), nameof(補償割合コード), nameof(評価地区コード), nameof(階層区分))]
    public class T23050平均単収差 : ModelBase
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
        /// 支所コード
        /// </summary>
        [Required]
        [Column("支所コード", Order = 4)]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 6)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 7)]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 評価地区コード
        /// </summary>
        [Required]
        [Column("評価地区コード", Order = 8)]
        [StringLength(4)]
        public string 評価地区コード { get; set; }

        /// <summary>
        /// 階層区分
        /// </summary>
        [Required]
        [Column("階層区分", Order = 9)]
        [StringLength(3)]
        public string 階層区分 { get; set; }

        /// <summary>
        /// 悉皆調査面積
        /// </summary>
        [Column("悉皆調査面積")]
        public Decimal? 悉皆調査面積 { get; set; }

        /// <summary>
        /// 抜取筆数
        /// </summary>
        [Column("抜取筆数")]
        public Decimal? 抜取筆数 { get; set; }

        /// <summary>
        /// 実測筆数
        /// </summary>
        [Column("実測筆数")]
        public Decimal? 実測筆数 { get; set; }

        /// <summary>
        /// 検見単収合計
        /// </summary>
        [Column("検見単収合計")]
        public Decimal? 検見単収合計 { get; set; }

        /// <summary>
        /// 抜取筆の悉皆単収合計
        /// </summary>
        [Column("抜取筆の悉皆単収合計")]
        public Decimal? 抜取筆の悉皆単収合計 { get; set; }

        /// <summary>
        /// 実測単収合計
        /// </summary>
        [Column("実測単収合計")]
        public Decimal? 実測単収合計 { get; set; }

        /// <summary>
        /// 実測筆の検見単収合計
        /// </summary>
        [Column("実測筆の検見単収合計")]
        public Decimal? 実測筆の検見単収合計 { get; set; }

        /// <summary>
        /// 平均単収差
        /// </summary>
        [Column("平均単収差")]
        public Decimal? 平均単収差 { get; set; }

        /// <summary>
        /// 調整平均単収差
        /// </summary>
        [Column("調整平均単収差")]
        public Decimal? 調整平均単収差 { get; set; }

        /// <summary>
        /// 決定平均単収差
        /// </summary>
        [Column("決定平均単収差")]
        public Decimal? 決定平均単収差 { get; set; }

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
