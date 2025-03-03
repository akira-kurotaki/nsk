using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20040_平均単収差計算名称
    /// </summary>
    [Serializable]
    [Table("m_20040_平均単収差計算名称")]
    public class M20040平均単収差計算名称 : ModelBase
    {
        /// <summary>
        /// 平均単収差計算方法
        /// </summary>
        [Required]
        [Key]
        [Column("平均単収差計算方法", Order = 1)]
        [StringLength(1)]
        public string 平均単収差計算方法 { get; set; }

        /// <summary>
        /// 平均単収差計算名称
        /// </summary>
        [Column("平均単収差計算名称")]
        [StringLength(20)]
        public string 平均単収差計算名称 { get; set; }

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
