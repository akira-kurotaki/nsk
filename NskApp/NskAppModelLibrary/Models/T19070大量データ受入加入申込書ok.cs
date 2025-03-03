using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_19070_大量データ受入_加入申込書ok
    /// </summary>
    [Serializable]
    [Table("t_19070_大量データ受入_加入申込書ok")]
    [PrimaryKey(nameof(受入履歴id), nameof(行番号))]
    public class T19070大量データ受入加入申込書ok : ModelBase
    {
        /// <summary>
        /// 受入履歴id
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("受入履歴id", Order = 1)]
        public long 受入履歴id { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("行番号", Order = 2)]
        public Decimal 行番号 { get; set; }

        /// <summary>
        /// 範囲
        /// </summary>
        [Required]
        [Column("範囲")]
        [StringLength(2)]
        public string 範囲 { get; set; }

        /// <summary>
        /// 年産範囲
        /// </summary>
        [Required]
        [Column("年産範囲")]
        public short 年産範囲 { get; set; }

        /// <summary>
        /// 共済目的コード範囲
        /// </summary>
        [Column("共済目的コード範囲")]
        [StringLength(2)]
        public string 共済目的コード範囲 { get; set; }

        /// <summary>
        /// 抽出区分
        /// </summary>
        [Column("抽出区分")]
        [StringLength(1)]
        public string 抽出区分 { get; set; }

        /// <summary>
        /// 範囲パラメータ１
        /// </summary>
        [Column("範囲パラメータ１")]
        [StringLength(13)]
        public string 範囲パラメータ１ { get; set; }

        /// <summary>
        /// 範囲パラメータ２
        /// </summary>
        [Column("範囲パラメータ２")]
        [StringLength(13)]
        public string 範囲パラメータ２ { get; set; }

        /// <summary>
        /// 範囲パラメータ３
        /// </summary>
        [Column("範囲パラメータ３")]
        [StringLength(4)]
        public string 範囲パラメータ３ { get; set; }

        /// <summary>
        /// 日付
        /// </summary>
        [Column("日付")]
        public DateTime? 日付 { get; set; }

        /// <summary>
        /// gisデータ出力のタイプ
        /// </summary>
        [Column("gisデータ出力のタイプ")]
        [StringLength(1)]
        public string gisデータ出力のタイプ { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Column("共済目的コード")]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Column("組合員等コード")]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 耕地番号
        /// </summary>
        [Column("耕地番号")]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Column("分筆番号")]
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
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(40)]
        public string 備考 { get; set; }

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
        /// 品種コード
        /// </summary>
        [Column("品種コード")]
        [StringLength(3)]
        public string 品種コード { get; set; }

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
        /// 修正日付
        /// </summary>
        [Column("修正日付")]
        public DateTime? 修正日付 { get; set; }

        /// <summary>
        /// 計算日付
        /// </summary>
        [Column("計算日付")]
        public DateTime? 計算日付 { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Column("年産")]
        public short? 年産 { get; set; }

        /// <summary>
        /// 実量基準単収
        /// </summary>
        [Column("実量基準単収")]
        public Decimal? 実量基準単収 { get; set; }

        /// <summary>
        /// rs区分
        /// </summary>
        [Column("rs区分")]
        [StringLength(2)]
        public string rs区分 { get; set; }

        /// <summary>
        /// 局都道府県コード
        /// </summary>
        [Column("局都道府県コード")]
        [StringLength(4)]
        public string 局都道府県コード { get; set; }

        /// <summary>
        /// 市区町村コード
        /// </summary>
        [Column("市区町村コード")]
        [StringLength(3)]
        public string 市区町村コード { get; set; }

        /// <summary>
        /// 大字コード
        /// </summary>
        [Column("大字コード")]
        [StringLength(8)]
        public string 大字コード { get; set; }

        /// <summary>
        /// 小字コード
        /// </summary>
        [Column("小字コード")]
        [StringLength(4)]
        public string 小字コード { get; set; }

        /// <summary>
        /// 地番
        /// </summary>
        [Column("地番")]
        [StringLength(16)]
        public string 地番 { get; set; }

        /// <summary>
        /// 枝番
        /// </summary>
        [Column("枝番")]
        [StringLength(14)]
        public string 枝番 { get; set; }

        /// <summary>
        /// 子番
        /// </summary>
        [Column("子番")]
        [StringLength(10)]
        public string 子番 { get; set; }

        /// <summary>
        /// 孫番
        /// </summary>
        [Column("孫番")]
        [StringLength(10)]
        public string 孫番 { get; set; }

        /// <summary>
        /// 統計市町村コード
        /// </summary>
        [Column("統計市町村コード")]
        [StringLength(5)]
        public string 統計市町村コード { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Column("統計単位地域コード")]
        [StringLength(5)]
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 統計単収
        /// </summary>
        [Column("統計単収")]
        public Decimal? 統計単収 { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Column("用途区分")]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Column("産地別銘柄コード")]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 受委託者コード
        /// </summary>
        [Column("受委託者コード")]
        [StringLength(13)]
        public string 受委託者コード { get; set; }

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
