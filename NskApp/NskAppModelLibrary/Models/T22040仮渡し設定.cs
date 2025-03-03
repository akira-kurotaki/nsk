using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22040_仮渡し設定
    /// </summary>
    [Serializable]
    [Table("t_22040_仮渡し設定")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(引受方式), nameof(補償割合), nameof(特約区分))]
    public class T22040仮渡し設定 : ModelBase
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
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 4)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 5)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 補償割合
        /// </summary>
        [Required]
        [Column("補償割合", Order = 6)]
        [StringLength(2)]
        public string 補償割合 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 7)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 仮渡実施被害割合
        /// </summary>
        [Column("仮渡実施被害割合")]
        public Decimal? 仮渡実施被害割合 { get; set; }

        /// <summary>
        /// 仮渡支払率
        /// </summary>
        [Column("仮渡支払率")]
        public Decimal? 仮渡支払率 { get; set; }

        /// <summary>
        /// 仮渡端数処理コード
        /// </summary>
        [Column("仮渡端数処理コード")]
        [StringLength(1)]
        public string 仮渡端数処理コード { get; set; }

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
