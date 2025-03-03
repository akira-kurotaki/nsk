using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20190_出荷階層区分名称
    /// </summary>
    [Serializable]
    [Table("m_20190_出荷階層区分名称")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(出荷評価地区コード), nameof(出荷階層区分))]
    public class M20190出荷階層区分名称 : ModelBase
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
        /// 出荷評価地区コード
        /// </summary>
        [Required]
        [Column("出荷評価地区コード", Order = 4)]
        [StringLength(4)]
        public string 出荷評価地区コード { get; set; }

        /// <summary>
        /// 出荷階層区分
        /// </summary>
        [Required]
        [Column("出荷階層区分", Order = 5)]
        [StringLength(3)]
        public string 出荷階層区分 { get; set; }

        /// <summary>
        /// 出荷階層区分名称
        /// </summary>
        [Column("出荷階層区分名称")]
        [StringLength(20)]
        public string 出荷階層区分名称 { get; set; }

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
