using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_00060_請求回
    /// </summary>
    [Serializable]
    [Table("t_00060_請求回")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード))]
    public class T00060請求回 : ModelBase
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
        /// 請求回
        /// </summary>
        [Column("請求回")]
        public short? 請求回 { get; set; }

        /// <summary>
        /// 計算対象フラグ
        /// </summary>
        [Column("計算対象フラグ")]
        [StringLength(1)]
        public string 計算対象フラグ { get; set; }

        /// <summary>
        /// 請求日付
        /// </summary>
        [Column("請求日付")]
        public DateTime? 請求日付 { get; set; }

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
