using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00220_共済目的対応
    /// </summary>
    [Serializable]
    [Table("m_00220_共済目的対応")]
    [PrimaryKey(nameof(共済事業コード), nameof(共済目的コード_fim), nameof(振込区分), nameof(共済目的コード_nsk))]
    public class M00220共済目的対応 : ModelBase
    {
        /// <summary>
        /// 共済事業コード
        /// </summary>
        [Required]
        [Column("共済事業コード", Order = 1)]
        [StringLength(2)]
        public string 共済事業コード { get; set; }

        /// <summary>
        /// 共済目的コード_fim
        /// </summary>
        [Required]
        [Column("共済目的コード_fim", Order = 2)]
        [StringLength(2)]
        public string 共済目的コード_fim { get; set; }

        /// <summary>
        /// 振込区分
        /// </summary>
        [Required]
        [Column("振込区分", Order = 3)]
        [StringLength(1)]
        public string 振込区分 { get; set; }

        /// <summary>
        /// 共済目的コード_nsk
        /// </summary>
        [Required]
        [Column("共済目的コード_nsk", Order = 4)]
        [StringLength(2)]
        public string 共済目的コード_nsk { get; set; }

        /// <summary>
        /// 採用順位
        /// </summary>
        [Column("採用順位")]
        [StringLength(2)]
        public string 採用順位 { get; set; }

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
