using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23060_平均単収差_抜取班集計
    /// </summary>
    [Serializable]
    [Table("t_23060_平均単収差_抜取班集計")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(支所コード), nameof(類区分), nameof(引受方式), nameof(補償割合コード), nameof(抜取調査班コード))]
    public class T23060平均単収差抜取班集計 : ModelBase
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
        /// 抜取調査班コード
        /// </summary>
        [Required]
        [Column("抜取調査班コード", Order = 8)]
        [StringLength(3)]
        public string 抜取調査班コード { get; set; }

        /// <summary>
        /// 実測筆数
        /// </summary>
        [Column("実測筆数")]
        public Decimal? 実測筆数 { get; set; }

        /// <summary>
        /// 抜取調査検見単収
        /// </summary>
        [Column("抜取調査検見単収")]
        public Decimal? 抜取調査検見単収 { get; set; }

        /// <summary>
        /// 実測単収
        /// </summary>
        [Column("実測単収")]
        public Decimal? 実測単収 { get; set; }

        /// <summary>
        /// 検見単収修正係数
        /// </summary>
        [Column("検見単収修正係数")]
        public Decimal? 検見単収修正係数 { get; set; }

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
