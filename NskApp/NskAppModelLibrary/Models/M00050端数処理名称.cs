using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00050_端数処理名称
    /// </summary>
    [Serializable]
    [Table("m_00050_端数処理名称")]
    public class M00050端数処理名称 : ModelBase
    {
        /// <summary>
        /// 端数処理コード
        /// </summary>
        [Required]
        [Key]
        [Column("端数処理コード", Order = 1)]
        [StringLength(1)]
        public string 端数処理コード { get; set; }

        /// <summary>
        /// 端数処理名称
        /// </summary>
        [Column("端数処理名称")]
        [StringLength(20)]
        public string 端数処理名称 { get; set; }

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
