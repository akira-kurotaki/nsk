using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29010_大量データ受入_分割情報OK
    /// </summary>
    [Serializable]
    [Table("t_29010_大量データ受入_分割情報OK")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(処理区分), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号), nameof(被害判定コード))]
    public class T29010大量データ受入分割情報ok : ModelBase
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
        /// 処理区分
        /// </summary>
        [Required]
        [Column("処理区分", Order = 4)]
        [StringLength(1)]
        public string 処理区分 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 5)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号", Order = 6)]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号", Order = 7)]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 被害判定コード
        /// </summary>
        [Required]
        [Column("被害判定コード", Order = 8)]
        [StringLength(1)]
        public string 被害判定コード { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Column("大地区コード")]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        [Column("小地区コード")]
        [StringLength(4)]
        public string 小地区コード { get; set; }

        /// <summary>
        /// 分割対象面積
        /// </summary>
        [Column("分割対象面積")]
        public Decimal? 分割対象面積 { get; set; }

        /// <summary>
        /// 分割単当減収量_出荷割合
        /// </summary>
        [Column("分割単当減収量_出荷割合")]
        public Decimal? 分割単当減収量_出荷割合 { get; set; }

        /// <summary>
        /// 分割単当減収量_引受割合
        /// </summary>
        [Column("分割単当減収量_引受割合")]
        public Decimal? 分割単当減収量_引受割合 { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Column("産地別銘柄コード")]
        [StringLength(5)]
        public string 産地別銘柄コード { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(10)]
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
