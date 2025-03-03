using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00070_収穫量確認方法名称
    /// </summary>
    [Serializable]
    [Table("m_00070_収穫量確認方法名称")]
    public class M00070収穫量確認方法名称 : ModelBase
    {
        /// <summary>
        /// 収穫量確認方法
        /// </summary>
        [Required]
        [Key]
        [Column("収穫量確認方法", Order = 1)]
        [StringLength(2)]
        public string 収穫量確認方法 { get; set; }

        /// <summary>
        /// 収穫量確認方法名称
        /// </summary>
        [Column("収穫量確認方法名称")]
        [StringLength(30)]
        public string 収穫量確認方法名称 { get; set; }

        /// <summary>
        /// 青申区分
        /// </summary>
        [Column("青申区分")]
        [StringLength(1)]
        public string 青申区分 { get; set; }

        /// <summary>
        /// 全相殺特例フラグ
        /// </summary>
        [Column("全相殺特例フラグ")]
        [StringLength(1)]
        public string 全相殺特例フラグ { get; set; }

        /// <summary>
        /// 賦課金引受方式
        /// </summary>
        [Column("賦課金引受方式")]
        [StringLength(2)]
        public string 賦課金引受方式 { get; set; }

        /// <summary>
        /// 加入申込区分
        /// </summary>
        [Column("加入申込区分")]
        [StringLength(1)]
        public string 加入申込区分 { get; set; }

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
