using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_15010_交付徴収
    /// </summary>
    [Serializable]
    [Table("t_15010_交付徴収")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(負担金交付区分), nameof(共済目的コード), nameof(交付回))]
    public class T15010交付徴収 : ModelBase
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
        /// 負担金交付区分
        /// </summary>
        [Required]
        [Column("負担金交付区分", Order = 3)]
        [StringLength(2)]
        public string 負担金交付区分 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 4)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 交付回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("交付回", Order = 5)]
        public short 交付回 { get; set; }

        /// <summary>
        /// 掛金徴収済額
        /// </summary>
        [Required]
        [Column("掛金徴収済額")]
        public Decimal 掛金徴収済額 { get; set; }

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
