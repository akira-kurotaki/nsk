using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_14010_規模別分布
    /// </summary>
    [Serializable]
    [Table("t_14010_規模別分布")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(引受回), nameof(類区分), nameof(合併時識別コード), nameof(大地区コード), nameof(引受面積区分), nameof(引受方式))]
    public class T14010規模別分布 : ModelBase
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
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Required]
        [Column("合併時識別コード", Order = 6)]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Required]
        [Column("大地区コード", Order = 7)]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// 引受面積区分
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("引受面積区分", Order = 8)]
        public Decimal 引受面積区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 9)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 合併特例有無フラグ
        /// </summary>
        [Column("合併特例有無フラグ")]
        [StringLength(1)]
        public string 合併特例有無フラグ { get; set; }

        /// <summary>
        /// 引受戸数
        /// </summary>
        [Column("引受戸数")]
        public Decimal? 引受戸数 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Column("引受面積")]
        public Decimal? 引受面積 { get; set; }

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
