using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22015_組合員等別仮渡情報類
    /// </summary>
    [Serializable]
    [Table("t_22015_組合員等別仮渡情報類")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(類区分), nameof(仮渡回))]
    public class T22015組合員等別仮渡情報類 : ModelBase
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
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 4)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 仮渡回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("仮渡回", Order = 6)]
        public short 仮渡回 { get; set; }

        /// <summary>
        /// 仮渡対象フラグ
        /// </summary>
        [Column("仮渡対象フラグ")]
        [StringLength(1)]
        public string 仮渡対象フラグ { get; set; }

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
        /// 仮渡計算額
        /// </summary>
        [Column("仮渡計算額")]
        public Decimal? 仮渡計算額 { get; set; }

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
