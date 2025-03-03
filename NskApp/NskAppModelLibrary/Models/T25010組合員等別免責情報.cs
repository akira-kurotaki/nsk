using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_25010_組合員等別免責情報
    /// </summary>
    [Serializable]
    [Table("t_25010_組合員等別免責情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(組合員等コード))]
    public class T25010組合員等別免責情報 : ModelBase
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
        /// 免責確定日
        /// </summary>
        [Column("免責確定日")]
        public DateTime? 免責確定日 { get; set; }

        /// <summary>
        /// 免責率
        /// </summary>
        [Column("免責率")]
        public Decimal? 免責率 { get; set; }

        /// <summary>
        /// 免責理由
        /// </summary>
        [Column("免責理由")]
        [StringLength(30)]
        public string 免責理由 { get; set; }

        /// <summary>
        /// 免責額
        /// </summary>
        [Column("免責額")]
        public Decimal? 免責額 { get; set; }

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
