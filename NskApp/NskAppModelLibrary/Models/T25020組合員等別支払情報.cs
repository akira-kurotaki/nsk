using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_25020_組合員等別支払情報
    /// </summary>
    [Serializable]
    [Table("t_25020_組合員等別支払情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(支払回))]
    public class T25020組合員等別支払情報 : ModelBase
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
        /// 支払回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("支払回", Order = 5)]
        public short 支払回 { get; set; }

        /// <summary>
        /// 支払共済金額
        /// </summary>
        [Column("支払共済金額")]
        public Decimal? 支払共済金額 { get; set; }

        /// <summary>
        /// 仮渡支払額
        /// </summary>
        [Column("仮渡支払額")]
        public Decimal? 仮渡支払額 { get; set; }

        /// <summary>
        /// 免責額
        /// </summary>
        [Column("免責額")]
        public Decimal? 免責額 { get; set; }

        /// <summary>
        /// 実支払金額
        /// </summary>
        [Column("実支払金額")]
        public Decimal? 実支払金額 { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(100)]
        public string 備考 { get; set; }

        /// <summary>
        /// 決定計算日付
        /// </summary>
        [Column("決定計算日付")]
        public DateTime? 決定計算日付 { get; set; }

        /// <summary>
        /// 支払年月日
        /// </summary>
        [Column("支払年月日")]
        public DateTime? 支払年月日 { get; set; }

        /// <summary>
        /// 支払通知書発行日
        /// </summary>
        [Column("支払通知書発行日")]
        public DateTime? 支払通知書発行日 { get; set; }

        /// <summary>
        /// 印刷済フラグ
        /// </summary>
        [Column("印刷済フラグ")]
        [StringLength(1)]
        public string 印刷済フラグ { get; set; }

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
