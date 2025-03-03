using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_50010_農作物実績マスタ
    /// </summary>
    [Serializable]
    [Table("t_50010_農作物実績マスタ")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(事業コード), nameof(共済目的コード), nameof(組合員等コード), nameof(類区分), nameof(統計単位地域コード))]
    public class T50010農作物実績マスタ : ModelBase
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
        /// 事業コード
        /// </summary>
        [Required]
        [Column("事業コード", Order = 3)]
        [StringLength(2)]
        public string 事業コード { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 4)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

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
        /// 合併時識別コード
        /// </summary>
        [Column("合併時識別コード")]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Column("引受方式")]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Column("特約区分")]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Column("補償割合コード")]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

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
        /// 引受筆数
        /// </summary>
        [Column("引受筆数")]
        public Decimal? 引受筆数 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Column("引受面積")]
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 基準収穫量
        /// </summary>
        [Column("基準収穫量")]
        public Decimal? 基準収穫量 { get; set; }

        /// <summary>
        /// 引受収量
        /// </summary>
        [Column("引受収量")]
        public Decimal? 引受収量 { get; set; }

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
        /// 共済限度額
        /// </summary>
        [Column("共済限度額")]
        public Decimal? 共済限度額 { get; set; }

        /// <summary>
        /// 危険段階区分
        /// </summary>
        [Column("危険段階区分")]
        public Decimal? 危険段階区分 { get; set; }

        /// <summary>
        /// 告示料率
        /// </summary>
        [Column("告示料率")]
        public Decimal? 告示料率 { get; set; }

        /// <summary>
        /// 共済掛金率
        /// </summary>
        [Column("共済掛金率")]
        public Decimal? 共済掛金率 { get; set; }

        /// <summary>
        /// 国庫負担共済掛金
        /// </summary>
        [Column("国庫負担共済掛金")]
        public Decimal? 国庫負担共済掛金 { get; set; }

        /// <summary>
        /// 農家負担共済掛金
        /// </summary>
        [Column("農家負担共済掛金")]
        public Decimal? 農家負担共済掛金 { get; set; }

        /// <summary>
        /// 共済減収量
        /// </summary>
        [Column("共済減収量")]
        public Decimal? 共済減収量 { get; set; }

        /// <summary>
        /// 支払共済金
        /// </summary>
        [Column("支払共済金")]
        public Decimal? 支払共済金 { get; set; }

        /// <summary>
        /// 実支払共済金
        /// </summary>
        [Column("実支払共済金")]
        public Decimal? 実支払共済金 { get; set; }

        /// <summary>
        /// 特例共済減収量
        /// </summary>
        [Column("特例共済減収量")]
        public Decimal? 特例共済減収量 { get; set; }

        /// <summary>
        /// 特例支払共済金
        /// </summary>
        [Column("特例支払共済金")]
        public Decimal? 特例支払共済金 { get; set; }

        /// <summary>
        /// 特例実支払共済金
        /// </summary>
        [Column("特例実支払共済金")]
        public Decimal? 特例実支払共済金 { get; set; }

        /// <summary>
        /// 収穫量確認方法
        /// </summary>
        [Column("収穫量確認方法")]
        [StringLength(2)]
        public string 収穫量確認方法 { get; set; }

        /// <summary>
        /// 営農対象区分
        /// </summary>
        [Column("営農対象区分")]
        [StringLength(1)]
        public string 営農対象区分 { get; set; }

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
