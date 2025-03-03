using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23030_平均単収差選択_抜取班
    /// </summary>
    [Serializable]
    [Table("t_23030_平均単収差選択_抜取班")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(支所コード), nameof(類区分), nameof(引受方式), nameof(補償割合コード), nameof(階層区分))]
    public class T23030平均単収差選択抜取班 : ModelBase
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
        /// 支所コード
        /// </summary>
        [Required]
        [Column("支所コード", Order = 4)]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 6)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 7)]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 階層区分
        /// </summary>
        [Required]
        [Column("階層区分", Order = 8)]
        [StringLength(3)]
        public string 階層区分 { get; set; }

        /// <summary>
        /// 平均単収差計算方法
        /// </summary>
        [Column("平均単収差計算方法")]
        [StringLength(1)]
        public string 平均単収差計算方法 { get; set; }

        /// <summary>
        /// 調整班平均単収差計算フラグ
        /// </summary>
        [Column("調整班平均単収差計算フラグ")]
        [StringLength(1)]
        public string 調整班平均単収差計算フラグ { get; set; }

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
