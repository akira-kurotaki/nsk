using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21180_出荷数量等調査野帳情報_規格別
    /// </summary>
    [Serializable]
    [Table("t_21180_出荷数量等調査野帳情報_規格別")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(出荷評価地区コード), nameof(出荷階層区分コード), nameof(類区分), nameof(産地別銘柄コード), nameof(営農対象外フラグ), nameof(規格))]
    public class T21180出荷数量等調査野帳情報規格別 : ModelBase
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
        /// 出荷評価地区コード
        /// </summary>
        [Required]
        [Column("出荷評価地区コード", Order = 5)]
        [StringLength(4)]
        public string 出荷評価地区コード { get; set; }

        /// <summary>
        /// 出荷階層区分コード
        /// </summary>
        [Required]
        [Column("出荷階層区分コード", Order = 6)]
        [StringLength(3)]
        public string 出荷階層区分コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 7)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Required]
        [Column("産地別銘柄コード", Order = 8)]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Required]
        [Column("営農対象外フラグ", Order = 9)]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 規格
        /// </summary>
        [Required]
        [Column("規格", Order = 10)]
        [StringLength(2)]
        public string 規格 { get; set; }

        /// <summary>
        /// 出荷数量等
        /// </summary>
        [Column("出荷数量等")]
        public Decimal? 出荷数量等 { get; set; }

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
