using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20210_引受方式別判定
    /// </summary>
    [Serializable]
    [Table("m_20210_引受方式別判定")]
    [PrimaryKey(nameof(引受方式), nameof(圃場フラグ), nameof(被害判定コード))]
    public class M20210引受方式別判定 : ModelBase
    {
        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 1)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 圃場フラグ
        /// </summary>
        [Required]
        [Column("圃場フラグ", Order = 2)]
        [StringLength(1)]
        public string 圃場フラグ { get; set; }

        /// <summary>
        /// 被害判定コード
        /// </summary>
        [Required]
        [Column("被害判定コード", Order = 3)]
        [StringLength(1)]
        public string 被害判定コード { get; set; }

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
