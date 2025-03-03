using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00150_帳票管理
    /// </summary>
    [Serializable]
    [Table("m_00150_帳票管理")]
    public class M00150帳票管理 : ModelBase
    {
        /// <summary>
        /// 帳票管理id
        /// </summary>
        [Required]
        [Key]
        [Column("帳票管理id", Order = 1)]
        [StringLength(12)]
        public string 帳票管理id { get; set; }

        /// <summary>
        /// 帳票ファイル区分
        /// </summary>
        [Column("帳票ファイル区分")]
        [StringLength(1)]
        public string 帳票ファイル区分 { get; set; }

        /// <summary>
        /// 業務区分
        /// </summary>
        [Column("業務区分")]
        [StringLength(1)]
        public string 業務区分 { get; set; }

        /// <summary>
        /// 業務分類
        /// </summary>
        [Column("業務分類")]
        [StringLength(2)]
        public string 業務分類 { get; set; }

        /// <summary>
        /// 帳票ファイル名称
        /// </summary>
        [Column("帳票ファイル名称")]
        [StringLength(50)]
        public string 帳票ファイル名称 { get; set; }

        /// <summary>
        /// 出力バッチid
        /// </summary>
        [Column("出力バッチid")]
        [StringLength(12)]
        public string 出力バッチid { get; set; }

        /// <summary>
        /// 出力指示画面id
        /// </summary>
        [Column("出力指示画面id")]
        [StringLength(12)]
        public string 出力指示画面id { get; set; }

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
