using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24200_産地別銘柄別評価情報_営農
    /// </summary>
    [Serializable]
    [Table("t_24200_産地別銘柄別評価情報_営農")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(産地別銘柄コード), nameof(営農対象外フラグ), nameof(類区分), nameof(精算区分))]
    public class T24200産地別銘柄別評価情報営農 : ModelBase
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
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 7)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 精算区分
        /// </summary>
        [Required]
        [Column("精算区分", Order = 8)]
        [StringLength(1)]
        public string 精算区分 { get; set; }

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
        /// 販売収入相当額合計
        /// </summary>
        [Column("販売収入相当額合計")]
        public Decimal? 販売収入相当額合計 { get; set; }

        /// <summary>
        /// 数量払相当額合計
        /// </summary>
        [Column("数量払相当額合計")]
        public Decimal? 数量払相当額合計 { get; set; }

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
