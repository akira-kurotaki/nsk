using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10250_賦課金_組合設定
    /// </summary>
    [Serializable]
    [Table("m_10250_賦課金_組合設定")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード))]
    public class M10250賦課金組合設定 : ModelBase
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
        /// 徴収単位固定徴収額_使用フラグ
        /// </summary>
        [Column("徴収単位固定徴収額_使用フラグ")]
        [StringLength(1)]
        public string 徴収単位固定徴収額_使用フラグ { get; set; }

        /// <summary>
        /// 徴収単位固定徴収額
        /// </summary>
        [Column("徴収単位固定徴収額")]
        public Decimal? 徴収単位固定徴収額 { get; set; }

        /// <summary>
        /// 徴収単位賦課金上限額_使用フラグ
        /// </summary>
        [Column("徴収単位賦課金上限額_使用フラグ")]
        [StringLength(1)]
        public string 徴収単位賦課金上限額_使用フラグ { get; set; }

        /// <summary>
        /// 徴収単位賦課金上限額
        /// </summary>
        [Column("徴収単位賦課金上限額")]
        public Decimal? 徴収単位賦課金上限額 { get; set; }

        /// <summary>
        /// 農家負担共済掛金_上限フラグ
        /// </summary>
        [Column("農家負担共済掛金_上限フラグ")]
        [StringLength(1)]
        public string 農家負担共済掛金_上限フラグ { get; set; }

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
