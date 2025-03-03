using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22010_組合員等別仮渡情報
    /// </summary>
    [Serializable]
    [Table("t_22010_組合員等別仮渡情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(仮渡回))]
    public class T22010組合員等別仮渡情報 : ModelBase
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
        /// 仮渡回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("仮渡回", Order = 5)]
        public short 仮渡回 { get; set; }

        /// <summary>
        /// 仮渡対象フラグ
        /// </summary>
        [Column("仮渡対象フラグ")]
        [StringLength(1)]
        public string 仮渡対象フラグ { get; set; }

        /// <summary>
        /// 仮渡計算額
        /// </summary>
        [Column("仮渡計算額")]
        public Decimal? 仮渡計算額 { get; set; }

        /// <summary>
        /// 仮渡支払額
        /// </summary>
        [Column("仮渡支払額")]
        public Decimal? 仮渡支払額 { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(100)]
        public string 備考 { get; set; }

        /// <summary>
        /// 仮渡支払対象フラグ
        /// </summary>
        [Column("仮渡支払対象フラグ")]
        [StringLength(1)]
        public string 仮渡支払対象フラグ { get; set; }

        /// <summary>
        /// 仮渡支払日
        /// </summary>
        [Column("仮渡支払日")]
        public DateTime? 仮渡支払日 { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Column("合併時識別コード")]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Column("大地区コード")]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        [Column("小地区コード")]
        [StringLength(4)]
        public string 小地区コード { get; set; }

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
