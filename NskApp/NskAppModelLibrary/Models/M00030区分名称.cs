using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00030_区分名称
    /// </summary>
    [Serializable]
    [Table("m_00030_区分名称")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(区分コード))]
    public class M00030区分名称 : ModelBase
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
        /// 区分コード
        /// </summary>
        [Required]
        [Column("区分コード", Order = 4)]
        [StringLength(2)]
        public string 区分コード { get; set; }

        /// <summary>
        /// 区分名称
        /// </summary>
        [Column("区分名称")]
        [StringLength(30)]
        public string 区分名称 { get; set; }

        /// <summary>
        /// 引受フラグ
        /// </summary>
        [Column("引受フラグ")]
        [StringLength(1)]
        public string 引受フラグ { get; set; }

        /// <summary>
        /// 転作フラグ
        /// </summary>
        [Column("転作フラグ")]
        [StringLength(1)]
        public string 転作フラグ { get; set; }

        /// <summary>
        /// 参酌フラグ
        /// </summary>
        [Column("参酌フラグ")]
        [StringLength(1)]
        public string 参酌フラグ { get; set; }

        /// <summary>
        /// エラータイプ
        /// </summary>
        [Column("エラータイプ")]
        [StringLength(1)]
        public string エラータイプ { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(20)]
        public string 備考 { get; set; }

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
