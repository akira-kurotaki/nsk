using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_13030_組合等引受_告示単価毎明細部
    /// </summary>
    [Serializable]
    [Table("t_13030_組合等引受_告示単価毎明細部")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(報告回), nameof(類区分), nameof(合併時識別コード), nameof(引受方式), nameof(特約区分), nameof(補償割合コード), nameof(作付時期), nameof(用途区分), nameof(単当共済金額順位))]
    public class T13030組合等引受告示単価毎明細部 : ModelBase
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
        /// 報告回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("報告回", Order = 4)]
        public short 報告回 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Required]
        [Column("合併時識別コード", Order = 6)]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 7)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 8)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 9)]
        [StringLength(1)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 作付時期
        /// </summary>
        [Required]
        [Column("作付時期", Order = 10)]
        [StringLength(1)]
        public string 作付時期 { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 11)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 単当共済金額順位
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("単当共済金額順位", Order = 12)]
        public Decimal 単当共済金額順位 { get; set; }

        /// <summary>
        /// 単当共済金額
        /// </summary>
        [Column("単当共済金額")]
        public Decimal? 単当共済金額 { get; set; }

        /// <summary>
        /// 引受戸数
        /// </summary>
        [Column("引受戸数")]
        public Decimal? 引受戸数 { get; set; }

        /// <summary>
        /// 引受筆数
        /// </summary>
        [Column("引受筆数")]
        public Decimal? 引受筆数 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Column("引受面積")]
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 基準収穫量
        /// </summary>
        [Column("基準収穫量")]
        public Decimal? 基準収穫量 { get; set; }

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
