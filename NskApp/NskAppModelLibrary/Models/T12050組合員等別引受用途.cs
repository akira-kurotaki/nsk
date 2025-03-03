using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12050_組合員等別引受用途
    /// </summary>
    [Serializable]
    [Table("t_12050_組合員等別引受用途")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(引受回), nameof(組合員等コード), nameof(類区分), nameof(統計単位地域コード), nameof(作付時期), nameof(用途区分))]
    public class T12050組合員等別引受用途 : ModelBase
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
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 6)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Required]
        [Column("統計単位地域コード", Order = 7)]
        [StringLength(5)]
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 作付時期
        /// </summary>
        [Required]
        [Column("作付時期", Order = 8)]
        [StringLength(1)]
        public string 作付時期 { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 9)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 種類区分
        /// </summary>
        [Column("種類区分")]
        [StringLength(1)]
        public string 種類区分 { get; set; }

        /// <summary>
        /// 田畑区分
        /// </summary>
        [Column("田畑区分")]
        [StringLength(1)]
        public string 田畑区分 { get; set; }

        /// <summary>
        /// 引受筆数
        /// </summary>
        [Column("引受筆数")]
        public Decimal? 引受筆数 { get; set; }

        /// <summary>
        /// 引受面積計
        /// </summary>
        [Column("引受面積計")]
        public Decimal? 引受面積計 { get; set; }

        /// <summary>
        /// 基準収穫量計
        /// </summary>
        [Column("基準収穫量計")]
        public Decimal? 基準収穫量計 { get; set; }

        /// <summary>
        /// 引受収量
        /// </summary>
        [Column("引受収量")]
        public Decimal? 引受収量 { get; set; }

        /// <summary>
        /// 共済金額選択順位
        /// </summary>
        [Column("共済金額選択順位")]
        public Decimal? 共済金額選択順位 { get; set; }

        /// <summary>
        /// 共済金額単価
        /// </summary>
        [Column("共済金額単価")]
        public Decimal? 共済金額単価 { get; set; }

        /// <summary>
        /// 共済金額
        /// </summary>
        [Column("共済金額")]
        public Decimal? 共済金額 { get; set; }

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
