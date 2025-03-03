using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29100_大量データ受入_全相殺施設搬入調査ok
    /// </summary>
    [Serializable]
    [Table("t_29100_大量データ受入_全相殺施設搬入調査ok")]
    [PrimaryKey(nameof(受入履歴id), nameof(行番号))]
    public class T29100大量データ受入全相殺施設搬入調査ok : ModelBase
    {
        /// <summary>
        /// 受入履歴id
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("受入履歴id", Order = 1)]
        public long 受入履歴id { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        [Required]
        [Column("行番号", Order = 2)]
        [StringLength(6)]
        public string 行番号 { get; set; }

        /// <summary>
        /// 処理区分
        /// </summary>
        [Required]
        [Column("処理区分")]
        [StringLength(1)]
        public string 処理区分 { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード")]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [Column("年産")]
        public short 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード")]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード")]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 受託者等コード
        /// </summary>
        [Required]
        [Column("受託者等コード")]
        [StringLength(2)]
        public string 受託者等コード { get; set; }

        /// <summary>
        /// 荷口番号
        /// </summary>
        [Required]
        [Column("荷口番号")]
        [StringLength(6)]
        public string 荷口番号 { get; set; }

        /// <summary>
        /// 品種コード
        /// </summary>
        [Column("品種コード")]
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
