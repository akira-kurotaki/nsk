using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_00030_当初評価確定
    /// </summary>
    [Serializable]
    [Table("t_00030_当初評価確定")]
    [PrimaryKey(nameof(共済目的コード), nameof(年産), nameof(支所コード), nameof(引受方式), nameof(政府保険認定区分))]
    public class T00030当初評価確定 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 3)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("年産", Order = 2)]
        public short 年産 { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Required]
        [Column("支所コード", Order = 4)]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 5)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 政府保険認定区分
        /// </summary>
        [Required]
        [Column("政府保険認定区分", Order = 6)]
        [StringLength(4)]
        public string 政府保険認定区分 { get; set; }

        /// <summary>
        /// 当初評価高計算実施日
        /// </summary>
        [Column("当初評価高計算実施日")]
        public DateTime? 当初評価高計算実施日 { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        [Column("件数")]
        public int? 件数 { get; set; }

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
