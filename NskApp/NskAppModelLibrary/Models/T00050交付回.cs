using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_00050_交付回
    /// </summary>
    [Serializable]
    [Table("t_00050_交付回")]
    [PrimaryKey(nameof(共済目的コード), nameof(年産), nameof(負担金交付区分), nameof(交付回))]
    public class T00050交付回 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 3)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

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
        [Column("負担金交付区分", Order = 4)]
        [StringLength(1)]
        public string 負担金交付区分 { get; set; }

        /// <summary>
        /// 交付回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("交付回", Order = 5)]
        public short 交付回 { get; set; }

        /// <summary>
        /// 紐づけ報告回
        /// </summary>
        [Column("紐づけ報告回")]
        public short? 紐づけ報告回 { get; set; }

        /// <summary>
        /// 交付計算実施日
        /// </summary>
        [Column("交付計算実施日")]
        public DateTime? 交付計算実施日 { get; set; }

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
