using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10180_単位当たり共済金額設定
    /// </summary>
    [Serializable]
    [Table("m_10180_単位当たり共済金額設定")]
    [PrimaryKey(nameof(共済目的コード), nameof(類区分))]
    public class M10180単位当たり共済金額設定 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 3)]
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
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 4)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 単位当たり共済金額_担い手課税用
        /// </summary>
        [Column("単位当たり共済金額_担い手課税用")]
        public Decimal? 単位当たり共済金額_担い手課税用 { get; set; }

        /// <summary>
        /// 単位当たり共済金額_担い手免税用
        /// </summary>
        [Column("単位当たり共済金額_担い手免税用")]
        public Decimal? 単位当たり共済金額_担い手免税用 { get; set; }

        /// <summary>
        /// 単位当たり共済金額_担い手以外用
        /// </summary>
        [Column("単位当たり共済金額_担い手以外用")]
        public Decimal? 単位当たり共済金額_担い手以外用 { get; set; }

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
