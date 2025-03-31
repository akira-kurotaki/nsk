using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00140_名称
    /// </summary>
    [Serializable]
    [Table("m_00140_名称")]
    [PrimaryKey(nameof(名称グループコード), nameof(名称コード))]
    public class M00140名称 : ModelBase
    {
        /// <summary>
        /// 名称グループコード
        /// </summary>
        [Required]
        [Column("名称グループコード", Order = 1)]
        [StringLength(30)]
        public string 名称グループコード { get; set; }

        /// <summary>
        /// 名称コード
        /// </summary>
        [Required]
        [Column("名称コード", Order = 2)]
        [StringLength(10)]
        public string 名称コード { get; set; }

        /// <summary>
        /// 名称グループ名称
        /// </summary>
        [Column("名称グループ名称")]
        [StringLength(50)]
        public string 名称グループ名称 { get; set; }

        /// <summary>
        /// 正式名称
        /// </summary>
        [Column("正式名称")]
        [StringLength(100)]
        public string 正式名称 { get; set; }

        /// <summary>
        /// 略称名称
        /// </summary>
        [Column("略称名称")]
        [StringLength(50)]
        public string 略称名称 { get; set; }

        /// <summary>
        /// 記号
        /// </summary>
        [Column("記号")]
        [StringLength(2)]
        public string 記号 { get; set; }

        /// <summary>
        /// 利用可能フラグ
        /// </summary>
        [Column("利用可能フラグ")]
        [StringLength(1)]
        public string 利用可能フラグ { get; set; }

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
