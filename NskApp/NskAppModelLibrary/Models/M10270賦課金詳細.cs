using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10270_賦課金詳細
    /// </summary>
    [Serializable]
    [Table("m_10270_賦課金詳細")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(引受方式), nameof(特約区分), nameof(大地区コード), nameof(ランク))]
    public class M10270賦課金詳細 : ModelBase
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
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 5)]
        [StringLength(2)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 6)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Required]
        [Column("大地区コード", Order = 7)]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// ランク
        /// </summary>
        [Required]
        [Column("ランク", Order = 8)]
        [StringLength(2)]
        public string ランク { get; set; }

        /// <summary>
        /// 面積割賦課単価_一般
        /// </summary>
        [Column("面積割賦課単価_一般")]
        public Decimal? 面積割賦課単価_一般 { get; set; }

        /// <summary>
        /// 面積割賦課単価_防災
        /// </summary>
        [Column("面積割賦課単価_防災")]
        public Decimal? 面積割賦課単価_防災 { get; set; }

        /// <summary>
        /// 面積割賦課単価_特別
        /// </summary>
        [Column("面積割賦課単価_特別")]
        public Decimal? 面積割賦課単価_特別 { get; set; }

        /// <summary>
        /// 賦課限度面積
        /// </summary>
        [Column("賦課限度面積")]
        public Decimal? 賦課限度面積 { get; set; }

        /// <summary>
        /// 収量割賦課単価_一般
        /// </summary>
        [Column("収量割賦課単価_一般")]
        public Decimal? 収量割賦課単価_一般 { get; set; }

        /// <summary>
        /// 収量割賦課単価_防災
        /// </summary>
        [Column("収量割賦課単価_防災")]
        public Decimal? 収量割賦課単価_防災 { get; set; }

        /// <summary>
        /// 収量割賦課単価_特別
        /// </summary>
        [Column("収量割賦課単価_特別")]
        public Decimal? 収量割賦課単価_特別 { get; set; }

        /// <summary>
        /// 賦課限度収量
        /// </summary>
        [Column("賦課限度収量")]
        public Decimal? 賦課限度収量 { get; set; }

        /// <summary>
        /// 基準生産金額割賦課単価_一般
        /// </summary>
        [Column("基準生産金額割賦課単価_一般")]
        public Decimal? 基準生産金額割賦課単価_一般 { get; set; }

        /// <summary>
        /// 基準生産金額割賦課単価_防災
        /// </summary>
        [Column("基準生産金額割賦課単価_防災")]
        public Decimal? 基準生産金額割賦課単価_防災 { get; set; }

        /// <summary>
        /// 基準生産金額割賦課単価_特別
        /// </summary>
        [Column("基準生産金額割賦課単価_特別")]
        public Decimal? 基準生産金額割賦課単価_特別 { get; set; }

        /// <summary>
        /// 賦課限度基準生産金額
        /// </summary>
        [Column("賦課限度基準生産金額")]
        public Decimal? 賦課限度基準生産金額 { get; set; }

        /// <summary>
        /// 金額割賦課単価_一般
        /// </summary>
        [Column("金額割賦課単価_一般")]
        public Decimal? 金額割賦課単価_一般 { get; set; }

        /// <summary>
        /// 金額割賦課単価_防災
        /// </summary>
        [Column("金額割賦課単価_防災")]
        public Decimal? 金額割賦課単価_防災 { get; set; }

        /// <summary>
        /// 金額割賦課単価_特別
        /// </summary>
        [Column("金額割賦課単価_特別")]
        public Decimal? 金額割賦課単価_特別 { get; set; }

        /// <summary>
        /// 賦課限度共済金額
        /// </summary>
        [Column("賦課限度共済金額")]
        public Decimal? 賦課限度共済金額 { get; set; }

        /// <summary>
        /// 端数処理コード
        /// </summary>
        [Column("端数処理コード")]
        [StringLength(1)]
        public string 端数処理コード { get; set; }

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
