using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00010_共済目的名称
    /// </summary>
    [Serializable]
    [Table("m_00010_共済目的名称")]
    public class M00010共済目的名称 : ModelBase
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Key]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 共済目的名称
        /// </summary>
        [Column("共済目的名称")]
        [StringLength(20)]
        public string 共済目的名称 { get; set; }

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
