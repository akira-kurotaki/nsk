using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_13010_組合等引受_合計部
    /// </summary>
    [Serializable]
    [Table("t_13010_組合等引受_合計部")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(報告回), nameof(類区分), nameof(合併時識別コード), nameof(引受方式), nameof(特約区分), nameof(補償割合コード))]
    public class T13010組合等引受合計部 : ModelBase
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
        /// 報告回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("報告回", Order = 4)]
        public short 報告回 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(3)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Required]
        [Column("合併時識別コード", Order = 6)]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 7)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 8)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 9)]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 組合等計引受実戸数
        /// </summary>
        [Column("組合等計引受実戸数")]
        public Decimal? 組合等計引受実戸数 { get; set; }

        /// <summary>
        /// 組合等計引受延戸数
        /// </summary>
        [Column("組合等計引受延戸数")]
        public Decimal? 組合等計引受延戸数 { get; set; }

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
        /// 組合等計基準生産金額
        /// </summary>
        [Column("組合等計基準生産金額")]
        public Decimal? 組合等計基準生産金額 { get; set; }

        /// <summary>
        /// 組合等計共済限度額
        /// </summary>
        [Column("組合等計共済限度額")]
        public Decimal? 組合等計共済限度額 { get; set; }

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
        /// 共済掛金国庫負担割合
        /// </summary>
        [Column("共済掛金国庫負担割合")]
        public Decimal? 共済掛金国庫負担割合 { get; set; }

        /// <summary>
        /// 組合等計交付対象負担金額
        /// </summary>
        [Column("組合等計交付対象負担金額")]
        public Decimal? 組合等計交付対象負担金額 { get; set; }

        /// <summary>
        /// 組合等計組合員等負担共済掛金
        /// </summary>
        [Column("組合等計組合員等負担共済掛金")]
        public Decimal? 組合等計組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 通常標準被害率
        /// </summary>
        [Column("通常標準被害率")]
        public Decimal? 通常標準被害率 { get; set; }

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
        /// 組合等交付金額
        /// </summary>
        [Column("組合等交付金額")]
        public Decimal? 組合等交付金額 { get; set; }

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
