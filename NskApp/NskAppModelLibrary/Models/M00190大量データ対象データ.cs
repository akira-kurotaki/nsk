using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00190_大量データ対象データ
    /// </summary>
    [Serializable]
    [Table("m_00190_大量データ対象データ")]
    public class M00190大量データ対象データ : ModelBase
    {
        /// <summary>
        /// 対象データ区分
        /// </summary>
        [Required]
        [Key]
        [Column("対象データ区分", Order = 1)]
        [StringLength(2)]
        public string 対象データ区分 { get; set; }

        /// <summary>
        /// 業務区分
        /// </summary>
        [Column("業務区分")]
        [StringLength(1)]
        public string 業務区分 { get; set; }

        /// <summary>
        /// 受入対象データ名称
        /// </summary>
        [Column("受入対象データ名称")]
        [StringLength(20)]
        public string 受入対象データ名称 { get; set; }

        /// <summary>
        /// 受入対象データ略称
        /// </summary>
        [Column("受入対象データ略称")]
        [StringLength(20)]
        public string 受入対象データ略称 { get; set; }

        /// <summary>
        /// 受入バッチid
        /// </summary>
        [Column("受入バッチid")]
        [StringLength(12)]
        public string 受入バッチid { get; set; }

        /// <summary>
        /// 取込バッチid
        /// </summary>
        [Column("取込バッチid")]
        [StringLength(12)]
        public string 取込バッチid { get; set; }

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
