using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21100_施設計量
    /// </summary>
    [Serializable]
    [Table("t_21100_施設計量")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(組合員等コード), nameof(受託者等コード), nameof(荷口番号), nameof(品種コード))]
    public class T21100施設計量 : ModelBase
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
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 5)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 受託者等コード
        /// </summary>
        [Required]
        [Column("受託者等コード", Order = 6)]
        [StringLength(2)]
        public string 受託者等コード { get; set; }

        /// <summary>
        /// 荷口番号
        /// </summary>
        [Required]
        [Column("荷口番号", Order = 7)]
        [StringLength(6)]
        public string 荷口番号 { get; set; }

        /// <summary>
        /// 品種コード
        /// </summary>
        [Required]
        [Column("品種コード", Order = 8)]
        [StringLength(3)]
        public string 品種コード { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Column("用途区分")]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 収穫年月日
        /// </summary>
        [Column("収穫年月日")]
        public DateTime? 収穫年月日 { get; set; }

        /// <summary>
        /// 評価月日
        /// </summary>
        [Column("評価月日")]
        public DateTime? 評価月日 { get; set; }

        /// <summary>
        /// 施設搬入時の生重量
        /// </summary>
        [Column("施設搬入時の生重量")]
        public Decimal? 施設搬入時の生重量 { get; set; }

        /// <summary>
        /// 粗選後の生重量
        /// </summary>
        [Column("粗選後の生重量")]
        public Decimal? 粗選後の生重量 { get; set; }

        /// <summary>
        /// 施設計量水分含有率
        /// </summary>
        [Column("施設計量水分含有率")]
        public Decimal? 施設計量水分含有率 { get; set; }

        /// <summary>
        /// 乾燥後の重量歩合
        /// </summary>
        [Column("乾燥後の重量歩合")]
        public Decimal? 乾燥後の重量歩合 { get; set; }

        /// <summary>
        /// きょう雑物の重量歩合
        /// </summary>
        [Column("きょう雑物の重量歩合")]
        public Decimal? きょう雑物の重量歩合 { get; set; }

        /// <summary>
        /// きょう雑物等の混入率
        /// </summary>
        [Column("きょう雑物等の混入率")]
        public Decimal? きょう雑物等の混入率 { get; set; }

        /// <summary>
        /// もみすり歩合
        /// </summary>
        [Column("もみすり歩合")]
        public Decimal? もみすり歩合 { get; set; }

        /// <summary>
        /// 玄米重歩合
        /// </summary>
        [Column("玄米重歩合")]
        public Decimal? 玄米重歩合 { get; set; }

        /// <summary>
        /// 玄米重量
        /// </summary>
        [Column("玄米重量")]
        public Decimal? 玄米重量 { get; set; }

        /// <summary>
        /// 篩目幅による修正率
        /// </summary>
        [Column("篩目幅による修正率")]
        public Decimal? 篩目幅による修正率 { get; set; }

        /// <summary>
        /// 直接入力フラグ
        /// </summary>
        [Column("直接入力フラグ")]
        [StringLength(1)]
        public string 直接入力フラグ { get; set; }

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
