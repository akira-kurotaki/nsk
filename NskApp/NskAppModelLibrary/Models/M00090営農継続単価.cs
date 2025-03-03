using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00090_営農継続単価
    /// </summary>
    [Serializable]
    [Table("m_00090_営農継続単価")]
    [PrimaryKey(nameof(共済目的コード), nameof(適用年月))]
    public class M00090営農継続単価 : ModelBase
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 適用年月
        /// </summary>
        [Required]
        [Column("適用年月", Order = 2)]
        [StringLength(7)]
        public string 適用年月 { get; set; }

        /// <summary>
        /// 営農継続単価
        /// </summary>
        [Column("営農継続単価")]
        public Decimal? 営農継続単価 { get; set; }

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
