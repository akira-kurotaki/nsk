using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00020_類名称
    /// </summary>
    [Serializable]
    [Table("m_00020_類名称")]
    [PrimaryKey(nameof(共済目的コード), nameof(類区分))]
    public class M00020類名称 : ModelBase
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 2)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 類名称
        /// </summary>
        [Column("類名称")]
        [StringLength(40)]
        public string 類名称 { get; set; }

        /// <summary>
        /// 類短縮名称
        /// </summary>
        [Column("類短縮名称")]
        [StringLength(20)]
        public string 類短縮名称 { get; set; }

        /// <summary>
        /// 加入区分
        /// </summary>
        [Column("加入区分")]
        [StringLength(1)]
        public string 加入区分 { get; set; }

        /// <summary>
        /// 田畑区分
        /// </summary>
        [Column("田畑区分")]
        [StringLength(1)]
        public string 田畑区分 { get; set; }

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
        /// 加入区分インデックス以外フラグ
        /// </summary>
        [Column("加入区分インデックス以外フラグ")]
        [StringLength(1)]
        public string 加入区分インデックス以外フラグ { get; set; }

        /// <summary>
        /// 加入区分インデックスフラグ
        /// </summary>
        [Column("加入区分インデックスフラグ")]
        [StringLength(1)]
        public string 加入区分インデックスフラグ { get; set; }

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
