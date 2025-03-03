using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23120_単当修正量
    /// </summary>
    [Serializable]
    [Table("t_23120_単当修正量")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(合併時識別コード), nameof(類区分), nameof(引受方式), nameof(補償割合コード), nameof(評価地区コード), nameof(階層区分))]
    public class T23120単当修正量 : ModelBase
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
        /// 合併時識別コード
        /// </summary>
        [Required]
        [Column("合併時識別コード", Order = 4)]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 6)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 7)]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 評価地区コード
        /// </summary>
        [Required]
        [Column("評価地区コード", Order = 8)]
        [StringLength(4)]
        public string 評価地区コード { get; set; }

        /// <summary>
        /// 階層区分
        /// </summary>
        [Required]
        [Column("階層区分", Order = 9)]
        [StringLength(3)]
        public string 階層区分 { get; set; }

        /// <summary>
        /// 悉皆調査面積
        /// </summary>
        [Column("悉皆調査面積")]
        public Decimal? 悉皆調査面積 { get; set; }

        /// <summary>
        /// 平均単収差
        /// </summary>
        [Column("平均単収差")]
        public Decimal? 平均単収差 { get; set; }

        /// <summary>
        /// 単当修正量
        /// </summary>
        [Column("単当修正量")]
        public Decimal? 単当修正量 { get; set; }

        /// <summary>
        /// 修正無しフラグ
        /// </summary>
        [Column("修正無しフラグ")]
        public Decimal? 修正無しフラグ { get; set; }

        /// <summary>
        /// 計算単位類区分
        /// </summary>
        [Column("計算単位類区分")]
        public Decimal? 計算単位類区分 { get; set; }

        /// <summary>
        /// 計算単位引受方式
        /// </summary>
        [Column("計算単位引受方式")]
        public Decimal? 計算単位引受方式 { get; set; }

        /// <summary>
        /// 計算単位補償割合
        /// </summary>
        [Column("計算単位補償割合")]
        public Decimal? 計算単位補償割合 { get; set; }

        /// <summary>
        /// 超過被害面積
        /// </summary>
        [Column("超過被害面積")]
        public Decimal? 超過被害面積 { get; set; }

        /// <summary>
        /// 悉皆評価面積
        /// </summary>
        [Column("悉皆評価面積")]
        public Decimal? 悉皆評価面積 { get; set; }

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
