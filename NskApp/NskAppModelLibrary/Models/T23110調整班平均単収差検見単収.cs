using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23110_調整班平均単収差_検見単収
    /// </summary>
    [Serializable]
    [Table("t_23110_調整班平均単収差_検見単収")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T23110調整班平均単収差検見単収 : ModelBase
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
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号", Order = 5)]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号", Order = 6)]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 修正検見単収
        /// </summary>
        [Column("修正検見単収")]
        public Decimal? 修正検見単収 { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("支所コード")]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Column("引受方式")]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Column("補償割合コード")]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 抜取調査班コード
        /// </summary>
        [Column("抜取調査班コード")]
        [StringLength(3)]
        public string 抜取調査班コード { get; set; }

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
