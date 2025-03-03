using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24020_当初計算経過筆
    /// </summary>
    [Serializable]
    [Table("t_24020_当初計算経過筆")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T24020当初計算経過筆 : ModelBase
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
        /// 用途区分
        /// </summary>
        [Column("用途区分")]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 営農対象用フラグ
        /// </summary>
        [Column("営農対象用フラグ")]
        [StringLength(1)]
        public string 営農対象用フラグ { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Column("営農対象外フラグ")]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 営農交付対象フラグ
        /// </summary>
        [Column("営農交付対象フラグ")]
        [StringLength(1)]
        public string 営農交付対象フラグ { get; set; }

        /// <summary>
        /// 被害判定コード
        /// </summary>
        [Column("被害判定コード")]
        [StringLength(1)]
        public string 被害判定コード { get; set; }

        /// <summary>
        /// 被害面積
        /// </summary>
        [Column("被害面積")]
        public Decimal? 被害面積 { get; set; }

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
        /// 引受単収
        /// </summary>
        [Column("引受単収")]
        public Decimal? 引受単収 { get; set; }

        /// <summary>
        /// 引受収量
        /// </summary>
        [Column("引受収量")]
        public Decimal? 引受収量 { get; set; }

        /// <summary>
        /// 引受単当共済金額
        /// </summary>
        [Column("引受単当共済金額")]
        public Decimal? 引受単当共済金額 { get; set; }

        /// <summary>
        /// 単当修正量
        /// </summary>
        [Column("単当修正量")]
        public Decimal? 単当修正量 { get; set; }

        /// <summary>
        /// 悉皆単収
        /// </summary>
        [Column("悉皆単収")]
        public Decimal? 悉皆単収 { get; set; }

        /// <summary>
        /// 悉皆収穫量
        /// </summary>
        [Column("悉皆収穫量")]
        public Decimal? 悉皆収穫量 { get; set; }

        /// <summary>
        /// 分割割合
        /// </summary>
        [Column("分割割合")]
        public Decimal? 分割割合 { get; set; }

        /// <summary>
        /// 組合等評価単収
        /// </summary>
        [Column("組合等評価単収")]
        public Decimal? 組合等評価単収 { get; set; }

        /// <summary>
        /// 組合等評価収量
        /// </summary>
        [Column("組合等評価収量")]
        public Decimal? 組合等評価収量 { get; set; }

        /// <summary>
        /// 交付対象共済単価
        /// </summary>
        [Column("交付対象共済単価")]
        public Decimal? 交付対象共済単価 { get; set; }

        /// <summary>
        /// 交付対象外共済単価
        /// </summary>
        [Column("交付対象外共済単価")]
        public Decimal? 交付対象外共済単価 { get; set; }

        /// <summary>
        /// 数量払相当額
        /// </summary>
        [Column("数量払相当額")]
        public Decimal? 数量払相当額 { get; set; }

        /// <summary>
        /// 告示単価超最高額
        /// </summary>
        [Column("告示単価超最高額")]
        public Decimal? 告示単価超最高額 { get; set; }

        /// <summary>
        /// 告示単価下最高額
        /// </summary>
        [Column("告示単価下最高額")]
        public Decimal? 告示単価下最高額 { get; set; }

        /// <summary>
        /// 数量払単価
        /// </summary>
        [Column("数量払単価")]
        public Decimal? 数量払単価 { get; set; }

        /// <summary>
        /// 分岐単収
        /// </summary>
        [Column("分岐単収")]
        public Decimal? 分岐単収 { get; set; }

        /// <summary>
        /// 分岐収量
        /// </summary>
        [Column("分岐収量")]
        public Decimal? 分岐収量 { get; set; }

        /// <summary>
        /// 当年収穫量
        /// </summary>
        [Column("当年収穫量")]
        public Decimal? 当年収穫量 { get; set; }

        /// <summary>
        /// 不能実収量
        /// </summary>
        [Column("不能実収量")]
        public Decimal? 不能実収量 { get; set; }

        /// <summary>
        /// 分岐超
        /// </summary>
        [Column("分岐超")]
        public Decimal? 分岐超 { get; set; }

        /// <summary>
        /// 分岐以下
        /// </summary>
        [Column("分岐以下")]
        public Decimal? 分岐以下 { get; set; }

        /// <summary>
        /// 調整後当年収穫量
        /// </summary>
        [Column("調整後当年収穫量")]
        public Decimal? 調整後当年収穫量 { get; set; }

        /// <summary>
        /// 減収量
        /// </summary>
        [Column("減収量")]
        public Decimal? 減収量 { get; set; }

        /// <summary>
        /// 共済減収量
        /// </summary>
        [Column("共済減収量")]
        public Decimal? 共済減収量 { get; set; }

        /// <summary>
        /// 分割減収量
        /// </summary>
        [Column("分割減収量")]
        public Decimal? 分割減収量 { get; set; }

        /// <summary>
        /// 一筆全半判定
        /// </summary>
        [Column("一筆全半判定")]
        [StringLength(1)]
        public string 一筆全半判定 { get; set; }

        /// <summary>
        /// 一筆全半減収量
        /// </summary>
        [Column("一筆全半減収量")]
        public Decimal? 一筆全半減収量 { get; set; }

        /// <summary>
        /// 共済金
        /// </summary>
        [Column("共済金")]
        public Decimal? 共済金 { get; set; }

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
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 評価地区コード
        /// </summary>
        [Column("評価地区コード")]
        [StringLength(4)]
        public string 評価地区コード { get; set; }

        /// <summary>
        /// 階層区分
        /// </summary>
        [Column("階層区分")]
        [StringLength(3)]
        public string 階層区分 { get; set; }

        /// <summary>
        /// 全相殺計算方法
        /// </summary>
        [Column("全相殺計算方法")]
        [StringLength(1)]
        public string 全相殺計算方法 { get; set; }

        /// <summary>
        /// 当初評価判定コード
        /// </summary>
        [Column("当初評価判定コード")]
        [StringLength(2)]
        public string 当初評価判定コード { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Column("統計単位地域コード")]
        [StringLength(5)]
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 作付時期
        /// </summary>
        [Column("作付時期")]
        [StringLength(1)]
        public string 作付時期 { get; set; }

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
