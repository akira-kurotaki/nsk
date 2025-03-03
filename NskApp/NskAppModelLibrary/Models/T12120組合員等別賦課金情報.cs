using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12120_組合員等別賦課金情報
    /// </summary>
    [Serializable]
    [Table("t_12120_組合員等別賦課金情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(引受回), nameof(組合員等コード), nameof(類区分), nameof(賦課金引受方式), nameof(特約区分))]
    public class T12120組合員等別賦課金情報 : ModelBase
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
        /// 引受回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("引受回", Order = 4)]
        public short 引受回 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 5)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 6)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 賦課金引受方式
        /// </summary>
        [Required]
        [Column("賦課金引受方式", Order = 7)]
        [StringLength(2)]
        public string 賦課金引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 8)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Column("大地区コード")]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// 一般賦課金
        /// </summary>
        [Column("一般賦課金")]
        public Decimal? 一般賦課金 { get; set; }

        /// <summary>
        /// 防災賦課金
        /// </summary>
        [Column("防災賦課金")]
        public Decimal? 防災賦課金 { get; set; }

        /// <summary>
        /// 特別賦課金
        /// </summary>
        [Column("特別賦課金")]
        public Decimal? 特別賦課金 { get; set; }

        /// <summary>
        /// 防災単価
        /// </summary>
        [Column("防災単価")]
        public Decimal? 防災単価 { get; set; }

        /// <summary>
        /// 特別単価
        /// </summary>
        [Column("特別単価")]
        public Decimal? 特別単価 { get; set; }

        /// <summary>
        /// 賦課金上限額
        /// </summary>
        [Column("賦課金上限額")]
        public Decimal? 賦課金上限額 { get; set; }

        /// <summary>
        /// 実賦課金合計
        /// </summary>
        [Column("実賦課金合計")]
        public Decimal? 実賦課金合計 { get; set; }

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
