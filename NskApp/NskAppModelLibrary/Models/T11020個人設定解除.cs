using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_11020_個人設定解除
    /// </summary>
    [Serializable]
    [Table("t_11020_個人設定解除")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード))]
    public class T11020個人設定解除 : ModelBase
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
        /// 解除引受回
        /// </summary>
        [Column("解除引受回")]
        public short? 解除引受回 { get; set; }

        /// <summary>
        /// 解除申出日付
        /// </summary>
        [Column("解除申出日付")]
        public DateTime? 解除申出日付 { get; set; }

        /// <summary>
        /// 引受解除日付
        /// </summary>
        [Column("引受解除日付")]
        public DateTime? 引受解除日付 { get; set; }

        /// <summary>
        /// 引受解除返還賦課金額
        /// </summary>
        [Column("引受解除返還賦課金額")]
        public Decimal? 引受解除返還賦課金額 { get; set; }

        /// <summary>
        /// 解除理由コード
        /// </summary>
        [Column("解除理由コード")]
        [StringLength(2)]
        public string 解除理由コード { get; set; }

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
