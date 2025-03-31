using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21150_出荷収穫量評価
    /// </summary>
    [Serializable]
    [Table("t_21150_出荷収穫量評価")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T21150出荷収穫量評価 : ModelBase
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
        [Required]
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

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
        /// 被害判定コード
        /// </summary>
        [Column("被害判定コード")]
        [StringLength(1)]
        public string 被害判定コード { get; set; }

        /// <summary>
        /// 一筆全半判定
        /// </summary>
        [Column("一筆全半判定")]
        [StringLength(1)]
        public string 一筆全半判定 { get; set; }

        /// <summary>
        /// 損害評価面積
        /// </summary>
        [Column("損害評価面積")]
        public Decimal? 損害評価面積 { get; set; }

        /// <summary>
        /// 対象面積_割合
        /// </summary>
        [Column("対象面積_割合")]
        public Decimal? 対象面積_割合 { get; set; }

        /// <summary>
        /// 対象面積_数量
        /// </summary>
        [Column("対象面積_数量")]
        public Decimal? 対象面積_数量 { get; set; }

        /// <summary>
        /// 出荷単当収量_割合
        /// </summary>
        [Column("出荷単当収量_割合")]
        public Decimal? 出荷単当収量_割合 { get; set; }

        /// <summary>
        /// 出荷単当収量_数量
        /// </summary>
        [Column("出荷単当収量_数量")]
        public Decimal? 出荷単当収量_数量 { get; set; }

        /// <summary>
        /// 出荷収穫量
        /// </summary>
        [Column("出荷収穫量")]
        public Decimal? 出荷収穫量 { get; set; }

        /// <summary>
        /// 分割割合
        /// </summary>
        [Column("分割割合")]
        public Decimal? 分割割合 { get; set; }

        /// <summary>
        /// 分割割合_数量
        /// </summary>
        [Column("分割割合_数量")]
        public Decimal? 分割割合_数量 { get; set; }

        /// <summary>
        /// 分割減収量
        /// </summary>
        [Column("分割減収量")]
        public Decimal? 分割減収量 { get; set; }

        /// <summary>
        /// 分割事由
        /// </summary>
        [Column("分割事由")]
        [StringLength(255)]
        public string 分割事由 { get; set; }

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
