using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10195_契約価格等設定_規格別
    /// </summary>
    [Serializable]
    [Table("m_10195_契約価格等設定_規格別")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(産地別銘柄コード), nameof(営農対象外フラグ), nameof(担い手区分), nameof(規格))]
    public class M10195契約価格等設定規格別 : ModelBase
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
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 4)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Required]
        [Column("産地別銘柄コード", Order = 5)]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Required]
        [Column("営農対象外フラグ", Order = 6)]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 担い手区分
        /// </summary>
        [Required]
        [Column("担い手区分", Order = 7)]
        [StringLength(1)]
        public string 担い手区分 { get; set; }

        /// <summary>
        /// 規格
        /// </summary>
        [Required]
        [Column("規格", Order = 8)]
        [StringLength(2)]
        public string 規格 { get; set; }

        /// <summary>
        /// 契約価格
        /// </summary>
        [Column("契約価格")]
        public Decimal? 契約価格 { get; set; }

        /// <summary>
        /// 品質指数
        /// </summary>
        [Column("品質指数")]
        public Decimal? 品質指数 { get; set; }

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
