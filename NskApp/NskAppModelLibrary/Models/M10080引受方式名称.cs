using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10080_引受方式名称
    /// </summary>
    [Serializable]
    [Table("m_10080_引受方式名称")]
    public class M10080引受方式名称 : ModelBase
    {
        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Key]
        [Column("引受方式", Order = 1)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 引受方式名称
        /// </summary>
        [Column("引受方式名称")]
        [StringLength(20)]
        public string 引受方式名称 { get; set; }

        /// <summary>
        /// 引受方式短縮名
        /// </summary>
        [Column("引受方式短縮名")]
        [StringLength(3)]
        public string 引受方式短縮名 { get; set; }

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
