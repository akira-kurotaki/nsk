using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00060_全相殺計算方法名称
    /// </summary>
    [Serializable]
    [Table("m_00060_全相殺計算方法名称")]
    public class M00060全相殺計算方法名称 : ModelBase
    {
        /// <summary>
        /// 全相殺計算方法
        /// </summary>
        [Required]
        [Key]
        [Column("全相殺計算方法", Order = 1)]
        [StringLength(1)]
        public string 全相殺計算方法 { get; set; }

        /// <summary>
        /// 全相殺計算方法名称
        /// </summary>
        [Column("全相殺計算方法名称")]
        [StringLength(30)]
        public string 全相殺計算方法名称 { get; set; }

        /// <summary>
        /// 圃場フラグ
        /// </summary>
        [Column("圃場フラグ")]
        [StringLength(1)]
        public string 圃場フラグ { get; set; }

        /// <summary>
        /// 施設フラグ
        /// </summary>
        [Column("施設フラグ")]
        [StringLength(1)]
        public string 施設フラグ { get; set; }

        /// <summary>
        /// 売渡フラグ
        /// </summary>
        [Column("売渡フラグ")]
        [StringLength(1)]
        public string 売渡フラグ { get; set; }

        /// <summary>
        /// 売渡判別時
        /// </summary>
        [Column("売渡判別時")]
        [StringLength(1)]
        public string 売渡判別時 { get; set; }

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
