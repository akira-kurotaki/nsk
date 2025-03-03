using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10210_単当共済金額用途
    /// </summary>
    [Serializable]
    [Table("m_10210_単当共済金額用途")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(種類区分), nameof(作付時期), nameof(引受区分), nameof(用途区分), nameof(課税単価区分), nameof(単当共済金額順位))]
    public class M10210単当共済金額用途 : ModelBase
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
        /// 種類区分
        /// </summary>
        [Required]
        [Column("種類区分", Order = 4)]
        [StringLength(1)]
        public string 種類区分 { get; set; }

        /// <summary>
        /// 作付時期
        /// </summary>
        [Required]
        [Column("作付時期", Order = 5)]
        [StringLength(1)]
        public string 作付時期 { get; set; }

        /// <summary>
        /// 引受区分
        /// </summary>
        [Required]
        [Column("引受区分", Order = 6)]
        [StringLength(2)]
        public string 引受区分 { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 7)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 課税単価区分
        /// </summary>
        [Required]
        [Column("課税単価区分", Order = 8)]
        [StringLength(1)]
        public string 課税単価区分 { get; set; }

        /// <summary>
        /// 単当共済金額順位
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("単当共済金額順位", Order = 9)]
        public Decimal 単当共済金額順位 { get; set; }

        /// <summary>
        /// 単当共済金額
        /// </summary>
        [Column("単当共済金額")]
        public Decimal? 単当共済金額 { get; set; }

        /// <summary>
        /// 含数量払フラグ
        /// </summary>
        [Column("含数量払フラグ")]
        [StringLength(1)]
        public string 含数量払フラグ { get; set; }

        /// <summary>
        /// 推奨フラグ
        /// </summary>
        [Column("推奨フラグ")]
        [StringLength(1)]
        public string 推奨フラグ { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(50)]
        public string 備考 { get; set; }

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
