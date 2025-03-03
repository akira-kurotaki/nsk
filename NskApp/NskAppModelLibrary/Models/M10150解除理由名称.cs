using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10150_解除理由名称
    /// </summary>
    [Serializable]
    [Table("m_10150_解除理由名称")]
    public class M10150解除理由名称 : ModelBase
    {
        /// <summary>
        /// 解除理由コード
        /// </summary>
        [Required]
        [Key]
        [Column("解除理由コード", Order = 1)]
        [StringLength(2)]
        public string 解除理由コード { get; set; }

        /// <summary>
        /// 解除理由名称
        /// </summary>
        [Column("解除理由名称")]
        [StringLength(20)]
        public string 解除理由名称 { get; set; }

        /// <summary>
        /// 解除フラグ
        /// </summary>
        [Column("解除フラグ")]
        [StringLength(1)]
        public string 解除フラグ { get; set; }

        /// <summary>
        /// 選択未納フラグ
        /// </summary>
        [Column("選択未納フラグ")]
        [StringLength(1)]
        public string 選択未納フラグ { get; set; }

        /// <summary>
        /// 選択既納フラグ
        /// </summary>
        [Column("選択既納フラグ")]
        [StringLength(1)]
        public string 選択既納フラグ { get; set; }

        /// <summary>
        /// 返還掛金フラグ
        /// </summary>
        [Column("返還掛金フラグ")]
        [StringLength(1)]
        public string 返還掛金フラグ { get; set; }

        /// <summary>
        /// 返還賦課金フラグ
        /// </summary>
        [Column("返還賦課金フラグ")]
        [StringLength(1)]
        public string 返還賦課金フラグ { get; set; }

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
