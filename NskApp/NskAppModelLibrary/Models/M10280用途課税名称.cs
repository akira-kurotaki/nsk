using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10280_用途課税名称
    /// </summary>
    [Serializable]
    [Table("m_10280_用途課税名称")]
    [PrimaryKey(nameof(共済目的コード), nameof(用途区分), nameof(課税単価区分))]
    public class M10280用途課税名称 : ModelBase
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 2)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 課税単価区分
        /// </summary>
        [Required]
        [Column("課税単価区分", Order = 3)]
        [StringLength(1)]
        public string 課税単価区分 { get; set; }

        /// <summary>
        /// 用途課税名称
        /// </summary>
        [Column("用途課税名称")]
        [StringLength(30)]
        public string 用途課税名称 { get; set; }

        /// <summary>
        /// 用途課税短縮名称
        /// </summary>
        [Column("用途課税短縮名称")]
        [StringLength(10)]
        public string 用途課税短縮名称 { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Column("営農対象外フラグ")]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

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
