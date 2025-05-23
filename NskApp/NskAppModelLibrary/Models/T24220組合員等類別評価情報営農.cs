using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24220_組合員等類別評価情報_営農
    /// </summary>
    [Serializable]
    [Table("t_24220_組合員等類別評価情報_営農")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(類区分), nameof(精算区分))]
    public class T24220組合員等類別評価情報営農 : ModelBase
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
        /// 精算区分
        /// </summary>
        [Required]
        [Column("精算区分", Order = 6)]
        [StringLength(1)]
        public string 精算区分 { get; set; }

        /// <summary>
        /// 補償割合
        /// </summary>
        [Column("補償割合")]
        [StringLength(2)]
        public string 補償割合 { get; set; }

        /// <summary>
        /// 政府保険認定区分
        /// </summary>
        [Column("政府保険認定区分")]
        [StringLength(4)]
        public string 政府保険認定区分 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Column("引受面積")]
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 引受面積_営農対象外
        /// </summary>
        [Column("引受面積_営農対象外")]
        public Decimal? 引受面積_営農対象外 { get; set; }

        /// <summary>
        /// 引受面積_営農対象
        /// </summary>
        [Column("引受面積_営農対象")]
        public Decimal? 引受面積_営農対象 { get; set; }

        /// <summary>
        /// 被害面積_一筆全損
        /// </summary>
        [Column("被害面積_一筆全損")]
        public Decimal? 被害面積_一筆全損 { get; set; }

        /// <summary>
        /// 被害面積_一筆半損
        /// </summary>
        [Column("被害面積_一筆半損")]
        public Decimal? 被害面積_一筆半損 { get; set; }

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
        /// 生産金額_営農調整前
        /// </summary>
        [Column("生産金額_営農調整前")]
        public Decimal? 生産金額_営農調整前 { get; set; }

        /// <summary>
        /// 販売収入相当額合計
        /// </summary>
        [Column("販売収入相当額合計")]
        public Decimal? 販売収入相当額合計 { get; set; }

        /// <summary>
        /// 販売収入相当額合計_一筆全損
        /// </summary>
        [Column("販売収入相当額合計_一筆全損")]
        public Decimal? 販売収入相当額合計_一筆全損 { get; set; }

        /// <summary>
        /// 販売収入相当額合計_一筆半損
        /// </summary>
        [Column("販売収入相当額合計_一筆半損")]
        public Decimal? 販売収入相当額合計_一筆半損 { get; set; }

        /// <summary>
        /// 営農継続支払相当額
        /// </summary>
        [Column("営農継続支払相当額")]
        public Decimal? 営農継続支払相当額 { get; set; }

        /// <summary>
        /// 営農継続支払相当額１_一筆全損
        /// </summary>
        [Column("営農継続支払相当額１_一筆全損")]
        public Decimal? 営農継続支払相当額１_一筆全損 { get; set; }

        /// <summary>
        /// 営農継続支払相当額１_一筆半損
        /// </summary>
        [Column("営農継続支払相当額１_一筆半損")]
        public Decimal? 営農継続支払相当額１_一筆半損 { get; set; }

        /// <summary>
        /// 営農継続支払相当額２_一筆全損
        /// </summary>
        [Column("営農継続支払相当額２_一筆全損")]
        public Decimal? 営農継続支払相当額２_一筆全損 { get; set; }

        /// <summary>
        /// 営農継続支払相当額２_一筆半損
        /// </summary>
        [Column("営農継続支払相当額２_一筆半損")]
        public Decimal? 営農継続支払相当額２_一筆半損 { get; set; }

        /// <summary>
        /// 営農継続支払相当額_決定額__一筆全損
        /// </summary>
        [Column("営農継続支払相当額_決定額__一筆全損")]
        public Decimal? 営農継続支払相当額_決定額__一筆全損 { get; set; }

        /// <summary>
        /// 営農継続支払相当額_決定額_一筆半損
        /// </summary>
        [Column("営農継続支払相当額_決定額_一筆半損")]
        public Decimal? 営農継続支払相当額_決定額_一筆半損 { get; set; }

        /// <summary>
        /// 数量払相当額
        /// </summary>
        [Column("数量払相当額")]
        public Decimal? 数量払相当額 { get; set; }

        /// <summary>
        /// 出荷実績に対する数量払_一筆全損
        /// </summary>
        [Column("出荷実績に対する数量払_一筆全損")]
        public Decimal? 出荷実績に対する数量払_一筆全損 { get; set; }

        /// <summary>
        /// 出荷実績に対する数量払_一筆半損
        /// </summary>
        [Column("出荷実績に対する数量払_一筆半損")]
        public Decimal? 出荷実績に対する数量払_一筆半損 { get; set; }

        /// <summary>
        /// 調整後基準生産金額_一筆全損
        /// </summary>
        [Column("調整後基準生産金額_一筆全損")]
        public Decimal? 調整後基準生産金額_一筆全損 { get; set; }

        /// <summary>
        /// 調整後基準生産金額_一筆半損
        /// </summary>
        [Column("調整後基準生産金額_一筆半損")]
        public Decimal? 調整後基準生産金額_一筆半損 { get; set; }

        /// <summary>
        /// 生産金額
        /// </summary>
        [Column("生産金額")]
        public Decimal? 生産金額 { get; set; }

        /// <summary>
        /// 生産金額_一筆全損
        /// </summary>
        [Column("生産金額_一筆全損")]
        public Decimal? 生産金額_一筆全損 { get; set; }

        /// <summary>
        /// 生産金額_一筆半損
        /// </summary>
        [Column("生産金額_一筆半損")]
        public Decimal? 生産金額_一筆半損 { get; set; }

        /// <summary>
        /// 生産金額の減少額
        /// </summary>
        [Column("生産金額の減少額")]
        public Decimal? 生産金額の減少額 { get; set; }

        /// <summary>
        /// 生産金額の減少額_一筆全損
        /// </summary>
        [Column("生産金額の減少額_一筆全損")]
        public Decimal? 生産金額の減少額_一筆全損 { get; set; }

        /// <summary>
        /// 生産金額の減少額_一筆半損
        /// </summary>
        [Column("生産金額の減少額_一筆半損")]
        public Decimal? 生産金額の減少額_一筆半損 { get; set; }

        /// <summary>
        /// 生産金額の減少額_決定額
        /// </summary>
        [Column("生産金額の減少額_決定額")]
        public Decimal? 生産金額の減少額_決定額 { get; set; }

        /// <summary>
        /// 被害区分
        /// </summary>
        [Column("被害区分")]
        public Decimal? 被害区分 { get; set; }

        /// <summary>
        /// 組当支払対象フラグ
        /// </summary>
        [Column("組当支払対象フラグ")]
        [StringLength(1)]
        public string 組当支払対象フラグ { get; set; }

        /// <summary>
        /// 支払共済金_支払率調整前
        /// </summary>
        [Column("支払共済金_支払率調整前")]
        public Decimal? 支払共済金_支払率調整前 { get; set; }

        /// <summary>
        /// 支払共済金_支払率調整前_内超過被害
        /// </summary>
        [Column("支払共済金_支払率調整前_内超過被害")]
        public Decimal? 支払共済金_支払率調整前_内超過被害 { get; set; }

        /// <summary>
        /// 支払共済金_支払率調整前_内一筆全損
        /// </summary>
        [Column("支払共済金_支払率調整前_内一筆全損")]
        public Decimal? 支払共済金_支払率調整前_内一筆全損 { get; set; }

        /// <summary>
        /// 支払共済金_支払率調整前_内一筆半損
        /// </summary>
        [Column("支払共済金_支払率調整前_内一筆半損")]
        public Decimal? 支払共済金_支払率調整前_内一筆半損 { get; set; }

        /// <summary>
        /// 支払共済金
        /// </summary>
        [Column("支払共済金")]
        public Decimal? 支払共済金 { get; set; }

        /// <summary>
        /// 支払共済金_内超過被害
        /// </summary>
        [Column("支払共済金_内超過被害")]
        public Decimal? 支払共済金_内超過被害 { get; set; }

        /// <summary>
        /// 支払共済金_内一筆全損
        /// </summary>
        [Column("支払共済金_内一筆全損")]
        public Decimal? 支払共済金_内一筆全損 { get; set; }

        /// <summary>
        /// 支払共済金_内一筆半損
        /// </summary>
        [Column("支払共済金_内一筆半損")]
        public Decimal? 支払共済金_内一筆半損 { get; set; }

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
