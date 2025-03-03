using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12130_最低付保割合
    /// </summary>
    [Serializable]
    [Table("t_12130_最低付保割合")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード))]
    public class T12130最低付保割合 : ModelBase
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
        /// 最低付保割合
        /// </summary>
        [Column("最低付保割合")]
        [StringLength(2)]
        public string 最低付保割合 { get; set; }

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
