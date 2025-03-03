using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23130_調後悉皆評価
    /// </summary>
    [Serializable]
    [Table("t_23130_調後悉皆評価")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号), nameof(用途区分))]
    public class T23130調後悉皆評価 : ModelBase
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
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 7)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 分割減収量
        /// </summary>
        [Column("分割減収量")]
        public Decimal? 分割減収量 { get; set; }

        /// <summary>
        /// 単当修正量
        /// </summary>
        [Column("単当修正量")]
        public Decimal? 単当修正量 { get; set; }

        /// <summary>
        /// 組合等評価単収
        /// </summary>
        [Column("組合等評価単収")]
        public Decimal? 組合等評価単収 { get; set; }

        /// <summary>
        /// 組合等評価収量
        /// </summary>
        [Column("組合等評価収量")]
        public Decimal? 組合等評価収量 { get; set; }

        /// <summary>
        /// 収穫量
        /// </summary>
        [Column("収穫量")]
        public Decimal? 収穫量 { get; set; }

        /// <summary>
        /// 減収量
        /// </summary>
        [Column("減収量")]
        public Decimal? 減収量 { get; set; }

        /// <summary>
        /// 共済減収量
        /// </summary>
        [Column("共済減収量")]
        public Decimal? 共済減収量 { get; set; }

        /// <summary>
        /// 当初一筆全半減収量
        /// </summary>
        [Column("当初一筆全半減収量")]
        public Decimal? 当初一筆全半減収量 { get; set; }

        /// <summary>
        /// 当初計算日付
        /// </summary>
        [Column("当初計算日付")]
        public DateTime? 当初計算日付 { get; set; }

        /// <summary>
        /// 当初共済金
        /// </summary>
        [Column("当初共済金")]
        public Decimal? 当初共済金 { get; set; }

        /// <summary>
        /// 支払共済金
        /// </summary>
        [Column("支払共済金")]
        public Decimal? 支払共済金 { get; set; }

        /// <summary>
        /// 補正量
        /// </summary>
        [Column("補正量")]
        public Decimal? 補正量 { get; set; }

        /// <summary>
        /// 適用単当共済金額
        /// </summary>
        [Column("適用単当共済金額")]
        public Decimal? 適用単当共済金額 { get; set; }

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
