using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21130_税務申告全数調査
    /// </summary>
    [Serializable]
    [Table("t_21130_税務申告全数調査")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(組合員等コード), nameof(用途区分))]
    public class T21130税務申告全数調査 : ModelBase
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
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 5)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 6)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 収穫量
        /// </summary>
        [Column("収穫量")]
        public Decimal? 収穫量 { get; set; }

        /// <summary>
        /// 売上数量
        /// </summary>
        [Column("売上数量")]
        public Decimal? 売上数量 { get; set; }

        /// <summary>
        /// 事業消費数量
        /// </summary>
        [Column("事業消費数量")]
        public Decimal? 事業消費数量 { get; set; }

        /// <summary>
        /// 廃棄亡失数量
        /// </summary>
        [Column("廃棄亡失数量")]
        public Decimal? 廃棄亡失数量 { get; set; }

        /// <summary>
        /// 期末棚卸数量
        /// </summary>
        [Column("期末棚卸数量")]
        public Decimal? 期末棚卸数量 { get; set; }

        /// <summary>
        /// 期首棚卸数量
        /// </summary>
        [Column("期首棚卸数量")]
        public Decimal? 期首棚卸数量 { get; set; }

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
