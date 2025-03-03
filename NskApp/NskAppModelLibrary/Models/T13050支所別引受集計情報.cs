using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_13050_支所別引受集計情報
    /// </summary>
    [Serializable]
    [Table("t_13050_支所別引受集計情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(引受回), nameof(類区分), nameof(支所コード), nameof(合併時識別コード), nameof(引受方式), nameof(特約区分), nameof(補償割合コード))]
    public class T13050支所別引受集計情報 : ModelBase
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
        [StringLength(3)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Required]
        [Column("支所コード", Order = 6)]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Required]
        [Column("合併時識別コード", Order = 7)]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 8)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 9)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 10)]
        [StringLength(1)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 組合等計引受戸数
        /// </summary>
        [Column("組合等計引受戸数")]
        public Decimal? 組合等計引受戸数 { get; set; }

        /// <summary>
        /// 組合等計引受面積
        /// </summary>
        [Column("組合等計引受面積")]
        public Decimal? 組合等計引受面積 { get; set; }

        /// <summary>
        /// 組合等計基準収穫量
        /// </summary>
        [Column("組合等計基準収穫量")]
        public Decimal? 組合等計基準収穫量 { get; set; }

        /// <summary>
        /// 組合等計引受収量
        /// </summary>
        [Column("組合等計引受収量")]
        public Decimal? 組合等計引受収量 { get; set; }

        /// <summary>
        /// 組合等計共済金額
        /// </summary>
        [Column("組合等計共済金額")]
        public Decimal? 組合等計共済金額 { get; set; }

        /// <summary>
        /// 組合等計基準共済掛金
        /// </summary>
        [Column("組合等計基準共済掛金")]
        public Decimal? 組合等計基準共済掛金 { get; set; }

        /// <summary>
        /// 組合等計共済掛金
        /// </summary>
        [Column("組合等計共済掛金")]
        public Decimal? 組合等計共済掛金 { get; set; }

        /// <summary>
        /// 組合等計農作物交付対象負担金額
        /// </summary>
        [Column("組合等計農作物交付対象負担金額")]
        public Decimal? 組合等計農作物交付対象負担金額 { get; set; }

        /// <summary>
        /// 組合等計組合員等負担共済掛金
        /// </summary>
        [Column("組合等計組合員等負担共済掛金")]
        public Decimal? 組合等計組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 組合等計農作物通常責任共済金額
        /// </summary>
        [Column("組合等計農作物通常責任共済金額")]
        public Decimal? 組合等計農作物通常責任共済金額 { get; set; }

        /// <summary>
        /// 組合等計農作物異常責任共済金額
        /// </summary>
        [Column("組合等計農作物異常責任共済金額")]
        public Decimal? 組合等計農作物異常責任共済金額 { get; set; }

        /// <summary>
        /// 組合等計異常責任共済掛金
        /// </summary>
        [Column("組合等計異常責任共済掛金")]
        public Decimal? 組合等計異常責任共済掛金 { get; set; }

        /// <summary>
        /// 異常責任保険金額
        /// </summary>
        [Column("異常責任保険金額")]
        public Decimal? 異常責任保険金額 { get; set; }

        /// <summary>
        /// 組合等計保険金額
        /// </summary>
        [Column("組合等計保険金額")]
        public Decimal? 組合等計保険金額 { get; set; }

        /// <summary>
        /// 組合等計保険料
        /// </summary>
        [Column("組合等計保険料")]
        public Decimal? 組合等計保険料 { get; set; }

        /// <summary>
        /// 組合等計引受総筆数
        /// </summary>
        [Column("組合等計引受総筆数")]
        public Decimal? 組合等計引受総筆数 { get; set; }

        /// <summary>
        /// 等級単収引受面積計
        /// </summary>
        [Column("等級単収引受面積計")]
        public Decimal? 等級単収引受面積計 { get; set; }

        /// <summary>
        /// 等級単収基準収穫量計
        /// </summary>
        [Column("等級単収基準収穫量計")]
        public Decimal? 等級単収基準収穫量計 { get; set; }

        /// <summary>
        /// 実行基準単収
        /// </summary>
        [Column("実行基準単収")]
        public Decimal? 実行基準単収 { get; set; }

        /// <summary>
        /// 最高基準単収
        /// </summary>
        [Column("最高基準単収")]
        public Decimal? 最高基準単収 { get; set; }

        /// <summary>
        /// 最低基準単収
        /// </summary>
        [Column("最低基準単収")]
        public Decimal? 最低基準単収 { get; set; }

        /// <summary>
        /// 組合等平均単当共済金額
        /// </summary>
        [Column("組合等平均単当共済金額")]
        public Decimal? 組合等平均単当共済金額 { get; set; }

        /// <summary>
        /// 組合等計賦課金
        /// </summary>
        [Column("組合等計賦課金")]
        public Decimal? 組合等計賦課金 { get; set; }

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
