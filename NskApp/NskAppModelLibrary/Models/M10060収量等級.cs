using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10060_収量等級
    /// </summary>
    [Serializable]
    [Table("m_10060_収量等級")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(支所コード), nameof(収量等級コード))]
    public class M10060収量等級 : ModelBase
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
        /// 種類区分
        /// </summary>
        [Column("種類区分")]
        [StringLength(1)]
        public string 種類区分 { get; set; }

        /// <summary>
        /// 作付時期
        /// </summary>
        [Column("作付時期")]
        [StringLength(1)]
        public string 作付時期 { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Required]
        [Column("支所コード", Order = 5)]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 収量等級コード
        /// </summary>
        [Required]
        [Column("収量等級コード", Order = 6)]
        [StringLength(3)]
        public string 収量等級コード { get; set; }

        /// <summary>
        /// 収量
        /// </summary>
        [Column("収量")]
        [StringLength(4)]
        public string 収量 { get; set; }

        /// <summary>
        /// 計算単収
        /// </summary>
        [Column("計算単収")]
        public Decimal? 計算単収 { get; set; }

        /// <summary>
        /// 適用単収
        /// </summary>
        [Column("適用単収")]
        public Decimal? 適用単収 { get; set; }

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
