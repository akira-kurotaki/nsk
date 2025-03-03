using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22050_仮渡評価計算チェック_pq
    /// </summary>
    [Serializable]
    [Table("t_22050_仮渡評価計算チェック_pq")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T22050仮渡評価計算チェックPq : ModelBase
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
        /// 評価ERRフラグ
        /// </summary>
        [Column("評価ERRフラグ")]
        [StringLength(1)]
        public string 評価ERRフラグ { get; set; }

        /// <summary>
        /// 評価WARフラグ
        /// </summary>
        [Column("評価WARフラグ")]
        [StringLength(1)]
        public string 評価WARフラグ { get; set; }

        /// <summary>
        /// 評価ERRNO
        /// </summary>
        [Column("評価ERRNO")]
        [StringLength(30)]
        public string 評価ERRNO { get; set; }

        /// <summary>
        /// 評価SUBJECT
        /// </summary>
        [Column("評価SUBJECT")]
        [StringLength(100)]
        public string 評価SUBJECT { get; set; }

        /// <summary>
        /// 評価ERRメッセージ
        /// </summary>
        [Column("評価ERRメッセージ")]
        [StringLength(512)]
        public string 評価ERRメッセージ { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Column("産地別銘柄コード")]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Column("営農対象外フラグ")]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

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
