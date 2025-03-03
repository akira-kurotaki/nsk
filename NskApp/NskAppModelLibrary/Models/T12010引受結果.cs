using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12010_引受結果
    /// </summary>
    [Serializable]
    [Table("t_12010_引受結果")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T12010引受結果 : ModelBase
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
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号", Order = 5)]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号", Order = 6)]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 地名地番
        /// </summary>
        [Column("地名地番")]
        [StringLength(40)]
        public string 地名地番 { get; set; }

        /// <summary>
        /// 耕地面積
        /// </summary>
        [Column("耕地面積")]
        public Decimal? 耕地面積 { get; set; }

        /// <summary>
        /// 基準単収
        /// </summary>
        [Column("基準単収")]
        public Decimal? 基準単収 { get; set; }

        /// <summary>
        /// 基準収穫量
        /// </summary>
        [Column("基準収穫量")]
        public Decimal? 基準収穫量 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Column("引受面積")]
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 転作等面積
        /// </summary>
        [Column("転作等面積")]
        public Decimal? 転作等面積 { get; set; }

        /// <summary>
        /// 受委託区分
        /// </summary>
        [Column("受委託区分")]
        [StringLength(1)]
        public string 受委託区分 { get; set; }

        /// <summary>
        /// 受委託者コード
        /// </summary>
        [Column("受委託者コード")]
        [StringLength(13)]
        public string 受委託者コード { get; set; }

        /// <summary>
        /// 田畑区分
        /// </summary>
        [Column("田畑区分")]
        [StringLength(1)]
        public string 田畑区分 { get; set; }

        /// <summary>
        /// 区分コード
        /// </summary>
        [Column("区分コード")]
        [StringLength(2)]
        public string 区分コード { get; set; }

        /// <summary>
        /// 種類コード
        /// </summary>
        [Column("種類コード")]
        [StringLength(2)]
        public string 種類コード { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Column("用途区分")]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 品種コード
        /// </summary>
        [Column("品種コード")]
        [StringLength(3)]
        public string 品種コード { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Column("産地別銘柄コード")]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 収量等級コード
        /// </summary>
        [Column("収量等級コード")]
        [StringLength(3)]
        public string 収量等級コード { get; set; }

        /// <summary>
        /// 参酌コード
        /// </summary>
        [Column("参酌コード")]
        [StringLength(3)]
        public string 参酌コード { get; set; }

        /// <summary>
        /// 計算日付
        /// </summary>
        [Column("計算日付")]
        public DateTime? 計算日付 { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Column("統計単位地域コード")]
        [StringLength(5)]
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 共済金額選択区分
        /// </summary>
        [Column("共済金額選択区分")]
        [StringLength(2)]
        public string 共済金額選択区分 { get; set; }

        /// <summary>
        /// 共済金額単価
        /// </summary>
        [Column("共済金額単価")]
        public Decimal? 共済金額単価 { get; set; }

        /// <summary>
        /// 調整係数
        /// </summary>
        [Column("調整係数")]
        public Decimal? 調整係数 { get; set; }

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
        /// 合併時識別コード
        /// </summary>
        [Column("合併時識別コード")]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 平均単収
        /// </summary>
        [Column("平均単収")]
        public Decimal? 平均単収 { get; set; }

        /// <summary>
        /// 調整後平均単収
        /// </summary>
        [Column("調整後平均単収")]
        public Decimal? 調整後平均単収 { get; set; }

        /// <summary>
        /// 調整後平均収穫量
        /// </summary>
        [Column("調整後平均収穫量")]
        public Decimal? 調整後平均収穫量 { get; set; }

        /// <summary>
        /// 基準生産金額
        /// </summary>
        [Column("基準生産金額")]
        public Decimal? 基準生産金額 { get; set; }

        /// <summary>
        /// 生産金額
        /// </summary>
        [Column("生産金額")]
        public Decimal? 生産金額 { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(40)]
        public string 備考 { get; set; }

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
