using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24070_耕地別一筆全損半損評価情報
    /// </summary>
    [Serializable]
    [Table("t_24070_耕地別一筆全損半損評価情報　")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号), nameof(精算区分))]
    public class T24070耕地別一筆全損半損評価情報　 : ModelBase
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
        /// 精算区分
        /// </summary>
        [Required]
        [Column("精算区分", Order = 7)]
        [StringLength(1)]
        public string 精算区分 { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Column("産地別銘柄コード")]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Column("営農対象外フラグ")]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 一筆全半損区分
        /// </summary>
        [Column("一筆全半損区分")]
        [StringLength(1)]
        public string 一筆全半損区分 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 補償割合
        /// </summary>
        [Column("補償割合")]
        [StringLength(2)]
        public string 補償割合 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Column("引受面積")]
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 被害判定コード
        /// </summary>
        [Column("被害判定コード")]
        [StringLength(1)]
        public string 被害判定コード { get; set; }

        /// <summary>
        /// 基準収穫量
        /// </summary>
        [Column("基準収穫量")]
        public Decimal? 基準収穫量 { get; set; }

        /// <summary>
        /// 基準生産金額
        /// </summary>
        [Column("基準生産金額")]
        public Decimal? 基準生産金額 { get; set; }

        /// <summary>
        /// 半損耕地生産金額
        /// </summary>
        [Column("半損耕地生産金額")]
        public Decimal? 半損耕地生産金額 { get; set; }

        /// <summary>
        /// 営農継続支払相当額対象フラグ
        /// </summary>
        [Column("営農継続支払相当額対象フラグ")]
        [StringLength(1)]
        public string 営農継続支払相当額対象フラグ { get; set; }

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
