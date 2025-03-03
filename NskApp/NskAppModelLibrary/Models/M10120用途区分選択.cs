using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10120_用途区分選択
    /// </summary>
    [Serializable]
    [Table("m_10120_用途区分選択")]
    [PrimaryKey(nameof(共済目的コード), nameof(類区分), nameof(作付時期), nameof(用途区分))]
    public class M10120用途区分選択 : ModelBase
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
        /// 種類区分
        /// </summary>
        [Column("種類区分")]
        [StringLength(1)]
        public string 種類区分 { get; set; }

        /// <summary>
        /// 作付時期
        /// </summary>
        [Required]
        [Column("作付時期", Order = 3)]
        [StringLength(1)]
        public string 作付時期 { get; set; }

        /// <summary>
        /// 田畑区分
        /// </summary>
        [Column("田畑区分")]
        [StringLength(1)]
        public string 田畑区分 { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 4)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

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
