using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_15020_組合等交付
    /// </summary>
    [Serializable]
    [Table("t_15020_組合等交付")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(負担金交付区分), nameof(交付回))]
    public class T15020組合等交付 : ModelBase
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
        /// 負担金交付区分
        /// </summary>
        [Required]
        [Column("負担金交付区分", Order = 3)]
        [StringLength(2)]
        public string 負担金交付区分 { get; set; }

        /// <summary>
        /// 交付回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("交付回", Order = 4)]
        public short 交付回 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Column("引受面積")]
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 引受収量
        /// </summary>
        [Column("引受収量")]
        public Decimal? 引受収量 { get; set; }

        /// <summary>
        /// 共済金額
        /// </summary>
        [Column("共済金額")]
        public Decimal? 共済金額 { get; set; }

        /// <summary>
        /// 保険料総額
        /// </summary>
        [Column("保険料総額")]
        public Decimal? 保険料総額 { get; set; }

        /// <summary>
        /// 組合等別国庫負担金
        /// </summary>
        [Column("組合等別国庫負担金")]
        public Decimal? 組合等別国庫負担金 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金
        /// </summary>
        [Column("組合員等負担共済掛金")]
        public Decimal? 組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金徴収済額
        /// </summary>
        [Column("組合員等負担共済掛金徴収済額")]
        public Decimal? 組合員等負担共済掛金徴収済額 { get; set; }

        /// <summary>
        /// 共済掛金徴収割合
        /// </summary>
        [Column("共済掛金徴収割合")]
        public Decimal? 共済掛金徴収割合 { get; set; }

        /// <summary>
        /// 組合等交付金の金額
        /// </summary>
        [Column("組合等交付金の金額")]
        public Decimal? 組合等交付金の金額 { get; set; }

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
