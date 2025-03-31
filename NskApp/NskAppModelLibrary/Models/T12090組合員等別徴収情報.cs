using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12090_組合員等別徴収情報
    /// </summary>
    [Serializable]
    [Table("t_12090_組合員等別徴収情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(引受回), nameof(組合員等コード))]
    public class T12090組合員等別徴収情報 : ModelBase
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
        /// 引受回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("引受回", Order = 4)]
        public short 引受回 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 5)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 徴収区分コード
        /// </summary>
        [Column("徴収区分コード")]
        [StringLength(1)]
        public string 徴収区分コード { get; set; }

        /// <summary>
        /// 自動振替フラグ
        /// </summary>
        [Column("自動振替フラグ")]
        [StringLength(1)]
        public string 自動振替フラグ { get; set; }

        /// <summary>
        /// 徴収理由コード
        /// </summary>
        [Column("徴収理由コード")]
        [StringLength(2)]
        public string 徴収理由コード { get; set; }

        /// <summary>
        /// 徴収年月日
        /// </summary>
        [Column("徴収年月日")]
        public DateTime? 徴収年月日 { get; set; }

        /// <summary>
        /// 徴収者
        /// </summary>
        [Column("徴収者")]
        [StringLength(40)]
        public string 徴収者 { get; set; }

        /// <summary>
        /// 徴収金額
        /// </summary>
        [Column("徴収金額")]
        public Decimal? 徴収金額 { get; set; }

        /// <summary>
        /// 内農家負担掛金
        /// </summary>
        [Column("内農家負担掛金")]
        public Decimal? 内農家負担掛金 { get; set; }

        /// <summary>
        /// 内賦課金
        /// </summary>
        [Column("内賦課金")]
        public Decimal? 内賦課金 { get; set; }

        /// <summary>
        /// 引受解除徴収賦課金額
        /// </summary>
        [Column("引受解除徴収賦課金額")]
        public Decimal? 引受解除徴収賦課金額 { get; set; }

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
