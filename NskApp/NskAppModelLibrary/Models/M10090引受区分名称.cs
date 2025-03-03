using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10090_引受区分名称
    /// </summary>
    [Serializable]
    [Table("m_10090_引受区分名称")]
    [PrimaryKey(nameof(共済目的コード), nameof(引受区分))]
    public class M10090引受区分名称 : ModelBase
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 引受区分
        /// </summary>
        [Required]
        [Column("引受区分", Order = 2)]
        [StringLength(2)]
        public string 引受区分 { get; set; }

        /// <summary>
        /// 引受区分名称
        /// </summary>
        [Column("引受区分名称")]
        [StringLength(20)]
        public string 引受区分名称 { get; set; }

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
