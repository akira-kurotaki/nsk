using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_19080_大量データ受入_組合員等類別平均単収ok
    /// </summary>
    [Serializable]
    [Table("t_19080_大量データ受入_組合員等類別平均単収ok")]
    [PrimaryKey(nameof(受入履歴id), nameof(行番号))]
    public class T19080大量データ受入組合員等類別平均単収ok : ModelBase
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("行番号", Order = 2)]
        public Decimal 行番号 { get; set; }

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
        /// 組合員等コード
        /// </summary>
        [Column("組合員等コード")]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 引受区分
        /// </summary>
        [Column("引受区分")]
        [StringLength(2)]
        public string 引受区分 { get; set; }

        /// <summary>
        /// 全相殺基準単収
        /// </summary>
        [Column("全相殺基準単収")]
        public Decimal? 全相殺基準単収 { get; set; }

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
