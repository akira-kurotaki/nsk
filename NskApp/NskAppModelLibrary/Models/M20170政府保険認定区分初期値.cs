using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20170_政府保険認定区分初期値
    /// </summary>
    [Serializable]
    [Table("m_20170_政府保険認定区分初期値")]
    [PrimaryKey(nameof(共済目的コード), nameof(引受方式), nameof(類区分), nameof(青申フラグ), nameof(その他コード), nameof(区分フラグ))]
    public class M20170政府保険認定区分初期値 : ModelBase
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 2)]
        [StringLength(4)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 3)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 青申フラグ
        /// </summary>
        [Required]
        [Column("青申フラグ", Order = 4)]
        [StringLength(3)]
        public string 青申フラグ { get; set; }

        /// <summary>
        /// その他コード
        /// </summary>
        [Required]
        [Column("その他コード", Order = 5)]
        [StringLength(4)]
        public string その他コード { get; set; }

        /// <summary>
        /// 区分フラグ
        /// </summary>
        [Required]
        [Column("区分フラグ", Order = 6)]
        [StringLength(3)]
        public string 区分フラグ { get; set; }

        /// <summary>
        /// 政府保険認定区分
        /// </summary>
        [Column("政府保険認定区分")]
        [StringLength(3)]
        public string 政府保険認定区分 { get; set; }

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
