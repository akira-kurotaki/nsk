using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20160_政府保険認定区分
    /// </summary>
    [Serializable]
    [Table("m_20160_政府保険認定区分")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(引受方式), nameof(類区分), nameof(識別コード))]
    public class M20160政府保険認定区分 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 1)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("年産", Order = 2)]
        public short 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 3)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 4)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 識別コード
        /// </summary>
        [Required]
        [Column("識別コード", Order = 6)]
        [StringLength(2)]
        public string 識別コード { get; set; }

        /// <summary>
        /// 青申フラグ
        /// </summary>
        [Column("青申フラグ")]
        [StringLength(1)]
        public string 青申フラグ { get; set; }

        /// <summary>
        /// 政府保険認定区分
        /// </summary>
        [Column("政府保険認定区分")]
        [StringLength(4)]
        public string 政府保険認定区分 { get; set; }

        /// <summary>
        /// 正式名称
        /// </summary>
        [Column("正式名称")]
        [StringLength(30)]
        public string 正式名称 { get; set; }

        /// <summary>
        /// 短縮名称
        /// </summary>
        [Column("短縮名称")]
        [StringLength(20)]
        public string 短縮名称 { get; set; }

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
