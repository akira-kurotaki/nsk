using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21080_農単抜取調査
    /// </summary>
    [Serializable]
    [Table("t_21080_農単抜取調査")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号), nameof(階層区分))]
    public class T21080農単抜取調査 : ModelBase
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
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号", Order = 5)]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号", Order = 6)]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 階層区分
        /// </summary>
        [Required]
        [Column("階層区分", Order = 7)]
        [StringLength(3)]
        public string 階層区分 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 損害評価面積
        /// </summary>
        [Column("損害評価面積")]
        public Decimal? 損害評価面積 { get; set; }

        /// <summary>
        /// 評価地区コード
        /// </summary>
        [Column("評価地区コード")]
        [StringLength(4)]
        public string 評価地区コード { get; set; }

        /// <summary>
        /// 申告収穫量
        /// </summary>
        [Column("申告収穫量")]
        public Decimal? 申告収穫量 { get; set; }

        /// <summary>
        /// 抜取単収
        /// </summary>
        [Column("抜取単収")]
        public Decimal? 抜取単収 { get; set; }

        /// <summary>
        /// 抜取収穫量
        /// </summary>
        [Column("抜取収穫量")]
        public Decimal? 抜取収穫量 { get; set; }

        /// <summary>
        /// 分割割合
        /// </summary>
        [Column("分割割合")]
        public Decimal? 分割割合 { get; set; }

        /// <summary>
        /// 被害判定コード
        /// </summary>
        [Column("被害判定コード")]
        [StringLength(1)]
        public string 被害判定コード { get; set; }

        /// <summary>
        /// 一筆全半判定
        /// </summary>
        [Column("一筆全半判定")]
        [StringLength(1)]
        public string 一筆全半判定 { get; set; }

        /// <summary>
        /// 分割事由
        /// </summary>
        [Column("分割事由")]
        [StringLength(255)]
        public string 分割事由 { get; set; }

        /// <summary>
        /// 共済事故
        /// </summary>
        [Column("共済事故")]
        [StringLength(2)]
        public string 共済事故 { get; set; }

        /// <summary>
        /// 災害種類
        /// </summary>
        [Column("災害種類")]
        [StringLength(2)]
        public string 災害種類 { get; set; }

        /// <summary>
        /// 災害発生年月日
        /// </summary>
        [Column("災害発生年月日")]
        public DateTime? 災害発生年月日 { get; set; }

        /// <summary>
        /// 評価年月日
        /// </summary>
        [Column("評価年月日")]
        public DateTime? 評価年月日 { get; set; }

        /// <summary>
        /// 評価者
        /// </summary>
        [Column("評価者")]
        [StringLength(20)]
        public string 評価者 { get; set; }

        /// <summary>
        /// 悉皆反映済フラグ
        /// </summary>
        [Column("悉皆反映済フラグ")]
        [StringLength(1)]
        public string 悉皆反映済フラグ { get; set; }

        /// <summary>
        /// 申告単収
        /// </summary>
        [Column("申告単収")]
        public Decimal? 申告単収 { get; set; }

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
