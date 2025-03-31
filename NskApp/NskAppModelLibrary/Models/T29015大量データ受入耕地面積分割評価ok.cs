using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29015_大量データ受入_耕地面積分割評価ok
    /// </summary>
    [Serializable]
    [Table("t_29015_大量データ受入_耕地面積分割評価ok")]
    [PrimaryKey(nameof(受入履歴id), nameof(行番号), nameof(処理区分), nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号), nameof(行番号_耕地毎))]
    public class T29015大量データ受入耕地面積分割評価ok : ModelBase
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
        [Column("処理区分", Order = 3)]
        [StringLength(1)]
        public string 処理区分 { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 4)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("年産", Order = 5)]
        public short 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 6)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 7)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号", Order = 8)]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号", Order = 9)]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 行番号_耕地毎
        /// </summary>
        [Required]
        [Column("行番号_耕地毎", Order = 10)]
        [StringLength(2)]
        public string 行番号_耕地毎 { get; set; }

        /// <summary>
        /// 分割耕地判定コード
        /// </summary>
        [Column("分割耕地判定コード")]
        public Decimal? 分割耕地判定コード { get; set; }

        /// <summary>
        /// 分割対象面積割合
        /// </summary>
        [Column("分割対象面積割合")]
        public Decimal? 分割対象面積割合 { get; set; }

        /// <summary>
        /// 分割対象面積
        /// </summary>
        [Column("分割対象面積")]
        public Decimal? 分割対象面積 { get; set; }

        /// <summary>
        /// 出荷単当収量_割合
        /// </summary>
        [Column("出荷単当収量_割合")]
        public Decimal? 出荷単当収量_割合 { get; set; }

        /// <summary>
        /// 出荷単当収量_数量
        /// </summary>
        [Column("出荷単当収量_数量")]
        public Decimal? 出荷単当収量_数量 { get; set; }

        /// <summary>
        /// 分割減収量_割合
        /// </summary>
        [Column("分割減収量_割合")]
        public Decimal? 分割減収量_割合 { get; set; }

        /// <summary>
        /// 分割減収量_数量
        /// </summary>
        [Column("分割減収量_数量")]
        public Decimal? 分割減収量_数量 { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(255)]
        public string 備考 { get; set; }

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
