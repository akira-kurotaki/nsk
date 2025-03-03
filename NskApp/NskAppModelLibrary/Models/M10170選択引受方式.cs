using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10170_選択引受方式
    /// </summary>
    [Serializable]
    [Table("m_10170_選択引受方式")]
    [PrimaryKey(nameof(組合等コード), nameof(共済目的コード), nameof(引受方式), nameof(特約区分), nameof(補償割合コード), nameof(類区分))]
    public class M10170選択引受方式 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 1)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 2)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 3)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 4)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 5)]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 6)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 表示順
        /// </summary>
        [Column("表示順")]
        public short? 表示順 { get; set; }

        /// <summary>
        /// 引受方式名称
        /// </summary>
        [Column("引受方式名称")]
        [StringLength(20)]
        public string 引受方式名称 { get; set; }

        /// <summary>
        /// 特約区分名称
        /// </summary>
        [Column("特約区分名称")]
        [StringLength(20)]
        public string 特約区分名称 { get; set; }

        /// <summary>
        /// 補償割合名称
        /// </summary>
        [Column("補償割合名称")]
        [StringLength(40)]
        public string 補償割合名称 { get; set; }

        /// <summary>
        /// 類短縮名称
        /// </summary>
        [Column("類短縮名称")]
        [StringLength(20)]
        public string 類短縮名称 { get; set; }

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
