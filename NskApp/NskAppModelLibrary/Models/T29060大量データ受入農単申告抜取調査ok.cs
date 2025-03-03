using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29060_大量データ受入_農単申告抜取調査ok
    /// </summary>
    [Serializable]
    [Table("t_29060_大量データ受入_農単申告抜取調査ok")]
    [PrimaryKey(nameof(受入履歴id), nameof(行番号))]
    public class T29060大量データ受入農単申告抜取調査ok : ModelBase
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
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号")]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号")]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 階層区分
        /// </summary>
        [Column("階層区分")]
        [StringLength(3)]
        public string 階層区分 { get; set; }

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
        /// 抜取調査班コード
        /// </summary>
        [Column("抜取調査班コード")]
        [StringLength(3)]
        public string 抜取調査班コード { get; set; }

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
    }
}
