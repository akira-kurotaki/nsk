using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12040_組合員等別引受情報
    /// </summary>
    [Serializable]
    [Table("t_12040_組合員等別引受情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(引受回), nameof(組合員等コード), nameof(類区分), nameof(統計単位地域コード))]
    public class T12040組合員等別引受情報 : ModelBase
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
        /// 支所コード
        /// </summary>
        [Column("支所コード")]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 耕地筆数
        /// </summary>
        [Column("耕地筆数")]
        public Decimal? 耕地筆数 { get; set; }

        /// <summary>
        /// 耕地面積計
        /// </summary>
        [Column("耕地面積計")]
        public Decimal? 耕地面積計 { get; set; }

        /// <summary>
        /// 耕地面積実計
        /// </summary>
        [Column("耕地面積実計")]
        public Decimal? 耕地面積実計 { get; set; }

        /// <summary>
        /// 全面転作フラグ
        /// </summary>
        [Column("全面転作フラグ")]
        [StringLength(1)]
        public string 全面転作フラグ { get; set; }

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
        /// 危険段階地域区分
        /// </summary>
        [Column("危険段階地域区分")]
        [StringLength(5)]
        public string 危険段階地域区分 { get; set; }

        /// <summary>
        /// 危険段階区分
        /// </summary>
        [Column("危険段階区分")]
        [StringLength(3)]
        public string 危険段階区分 { get; set; }

        /// <summary>
        /// 共済掛金標準率
        /// </summary>
        [Column("共済掛金標準率")]
        public Decimal? 共済掛金標準率 { get; set; }

        /// <summary>
        /// 基準共済掛金率
        /// </summary>
        [Column("基準共済掛金率")]
        public Decimal? 基準共済掛金率 { get; set; }

        /// <summary>
        /// 共済掛金率
        /// </summary>
        [Column("共済掛金率")]
        public Decimal? 共済掛金率 { get; set; }

        /// <summary>
        /// 付保割合
        /// </summary>
        [Column("付保割合")]
        public Decimal? 付保割合 { get; set; }

        /// <summary>
        /// 国庫負担割合
        /// </summary>
        [Column("国庫負担割合")]
        public Decimal? 国庫負担割合 { get; set; }

        /// <summary>
        /// 通常標準被害率
        /// </summary>
        [Column("通常標準被害率")]
        public Decimal? 通常標準被害率 { get; set; }

        /// <summary>
        /// 危険段階別通常標準被害率
        /// </summary>
        [Column("危険段階別通常標準被害率")]
        public Decimal? 危険段階別通常標準被害率 { get; set; }

        /// <summary>
        /// 最低共済金額
        /// </summary>
        [Column("最低共済金額")]
        public Decimal? 最低共済金額 { get; set; }

        /// <summary>
        /// 保険料基礎率
        /// </summary>
        [Column("保険料基礎率")]
        public Decimal? 保険料基礎率 { get; set; }

        /// <summary>
        /// 危険段階別保険料基礎率
        /// </summary>
        [Column("危険段階別保険料基礎率")]
        public Decimal? 危険段階別保険料基礎率 { get; set; }

        /// <summary>
        /// 再保険料基礎率
        /// </summary>
        [Column("再保険料基礎率")]
        public Decimal? 再保険料基礎率 { get; set; }

        /// <summary>
        /// 危険段階別再保険料基礎率
        /// </summary>
        [Column("危険段階別再保険料基礎率")]
        public Decimal? 危険段階別再保険料基礎率 { get; set; }

        /// <summary>
        /// 基準生産金額
        /// </summary>
        [Column("基準生産金額")]
        public Decimal? 基準生産金額 { get; set; }

        /// <summary>
        /// 共済限度額
        /// </summary>
        [Column("共済限度額")]
        public Decimal? 共済限度額 { get; set; }

        /// <summary>
        /// 共済金額
        /// </summary>
        [Column("共済金額")]
        public Decimal? 共済金額 { get; set; }

        /// <summary>
        /// 危険段階基準共済掛金率
        /// </summary>
        [Column("危険段階基準共済掛金率")]
        public Decimal? 危険段階基準共済掛金率 { get; set; }

        /// <summary>
        /// 危険段階共済掛金率
        /// </summary>
        [Column("危険段階共済掛金率")]
        public Decimal? 危険段階共済掛金率 { get; set; }

        /// <summary>
        /// 基準共済掛金
        /// </summary>
        [Column("基準共済掛金")]
        public Decimal? 基準共済掛金 { get; set; }

        /// <summary>
        /// 共済掛金
        /// </summary>
        [Column("共済掛金")]
        public Decimal? 共済掛金 { get; set; }

        /// <summary>
        /// 交付対象負担金
        /// </summary>
        [Column("交付対象負担金")]
        public Decimal? 交付対象負担金 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金
        /// </summary>
        [Column("組合員等負担共済掛金")]
        public Decimal? 組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 一般賦課金
        /// </summary>
        [Column("一般賦課金")]
        public Decimal? 一般賦課金 { get; set; }

        /// <summary>
        /// 防災賦課金
        /// </summary>
        [Column("防災賦課金")]
        public Decimal? 防災賦課金 { get; set; }

        /// <summary>
        /// 特別賦課金
        /// </summary>
        [Column("特別賦課金")]
        public Decimal? 特別賦課金 { get; set; }

        /// <summary>
        /// 組合員等割
        /// </summary>
        [Column("組合員等割")]
        public Decimal? 組合員等割 { get; set; }

        /// <summary>
        /// 賦課金計
        /// </summary>
        [Column("賦課金計")]
        public Decimal? 賦課金計 { get; set; }

        /// <summary>
        /// 納入額
        /// </summary>
        [Column("納入額")]
        public Decimal? 納入額 { get; set; }

        /// <summary>
        /// 徴収額
        /// </summary>
        [Column("徴収額")]
        public Decimal? 徴収額 { get; set; }

        /// <summary>
        /// 延滞金
        /// </summary>
        [Column("延滞金")]
        public Decimal? 延滞金 { get; set; }

        /// <summary>
        /// 通常責任共済金額
        /// </summary>
        [Column("通常責任共済金額")]
        public Decimal? 通常責任共済金額 { get; set; }

        /// <summary>
        /// 異常責任共済金額
        /// </summary>
        [Column("異常責任共済金額")]
        public Decimal? 異常責任共済金額 { get; set; }

        /// <summary>
        /// 異常責任共済掛金
        /// </summary>
        [Column("異常責任共済掛金")]
        public Decimal? 異常責任共済掛金 { get; set; }

        /// <summary>
        /// 通常責任保険歩合
        /// </summary>
        [Column("通常責任保険歩合")]
        public Decimal? 通常責任保険歩合 { get; set; }

        /// <summary>
        /// 再保険金額基礎額
        /// </summary>
        [Column("再保険金額基礎額")]
        public Decimal? 再保険金額基礎額 { get; set; }

        /// <summary>
        /// 再保険金額
        /// </summary>
        [Column("再保険金額")]
        public Decimal? 再保険金額 { get; set; }

        /// <summary>
        /// 再保険料
        /// </summary>
        [Column("再保険料")]
        public Decimal? 再保険料 { get; set; }

        /// <summary>
        /// 保険金額
        /// </summary>
        [Column("保険金額")]
        public Decimal? 保険金額 { get; set; }

        /// <summary>
        /// 保険料
        /// </summary>
        [Column("保険料")]
        public Decimal? 保険料 { get; set; }

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
        /// 合併時識別コード
        /// </summary>
        [Column("合併時識別コード")]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 継続特約フラグ
        /// </summary>
        [Column("継続特約フラグ")]
        [StringLength(1)]
        public string 継続特約フラグ { get; set; }

        /// <summary>
        /// 引受対象フラグ
        /// </summary>
        [Column("引受対象フラグ")]
        [StringLength(1)]
        public string 引受対象フラグ { get; set; }

        /// <summary>
        /// 未加入フラグ
        /// </summary>
        [Column("未加入フラグ")]
        [StringLength(1)]
        public string 未加入フラグ { get; set; }

        /// <summary>
        /// 解除フラグ
        /// </summary>
        [Column("解除フラグ")]
        [StringLength(1)]
        public string 解除フラグ { get; set; }

        /// <summary>
        /// 引受解除日付
        /// </summary>
        [Column("引受解除日付")]
        public DateTime? 引受解除日付 { get; set; }

        /// <summary>
        /// 引受解除返還賦課金額
        /// </summary>
        [Column("引受解除返還賦課金額")]
        public Decimal? 引受解除返還賦課金額 { get; set; }

        /// <summary>
        /// 全相殺特例フラグ
        /// </summary>
        [Column("全相殺特例フラグ")]
        [StringLength(1)]
        public string 全相殺特例フラグ { get; set; }

        /// <summary>
        /// 収穫量確認方法
        /// </summary>
        [Column("収穫量確認方法")]
        [StringLength(2)]
        public string 収穫量確認方法 { get; set; }

        /// <summary>
        /// 青申フラグ
        /// </summary>
        [Column("青申フラグ")]
        [StringLength(1)]
        public string 青申フラグ { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Column("営農対象外フラグ")]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 担手農家区分
        /// </summary>
        [Column("担手農家区分")]
        [StringLength(1)]
        public string 担手農家区分 { get; set; }

        /// <summary>
        /// 統計単収
        /// </summary>
        [Column("統計単収")]
        public Decimal? 統計単収 { get; set; }

        /// <summary>
        /// 計算日付
        /// </summary>
        [Column("計算日付")]
        public DateTime? 計算日付 { get; set; }

        /// <summary>
        /// 解除引受回
        /// </summary>
        [Column("解除引受回")]
        public short? 解除引受回 { get; set; }

        /// <summary>
        /// 解除理由コード
        /// </summary>
        [Column("解除理由コード")]
        [StringLength(1)]
        public string 解除理由コード { get; set; }

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
