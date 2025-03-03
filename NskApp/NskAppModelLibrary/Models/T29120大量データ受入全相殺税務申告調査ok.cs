using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29120_大量データ受入_全相殺税務申告調査ok
    /// </summary>
    [Serializable]
    [Table("t_29120_大量データ受入_全相殺税務申告調査ok")]
    [PrimaryKey(nameof(受入履歴id), nameof(行番号))]
    public class T29120大量データ受入全相殺税務申告調査ok : ModelBase
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
        [Column("処理区分")]
        [StringLength(1)]
        public string 処理区分 { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Column("組合等コード")]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Column("年産")]
        public short? 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Column("共済目的コード")]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Column("組合員等コード")]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Column("用途区分")]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 売上数量
        /// </summary>
        [Column("売上数量")]
        public Decimal? 売上数量 { get; set; }

        /// <summary>
        /// 事業消費数量
        /// </summary>
        [Column("事業消費数量")]
        public Decimal? 事業消費数量 { get; set; }

        /// <summary>
        /// 廃棄亡失数量
        /// </summary>
        [Column("廃棄亡失数量")]
        public Decimal? 廃棄亡失数量 { get; set; }

        /// <summary>
        /// 期末棚卸数量
        /// </summary>
        [Column("期末棚卸数量")]
        public Decimal? 期末棚卸数量 { get; set; }

        /// <summary>
        /// 期首棚卸数量
        /// </summary>
        [Column("期首棚卸数量")]
        public Decimal? 期首棚卸数量 { get; set; }

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
