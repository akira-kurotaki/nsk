using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00120_料率
    /// </summary>
    [Serializable]
    [Table("m_00120_料率")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(合併時識別コード), nameof(類区分), nameof(引受方式), nameof(特約区分), nameof(補償割合コード), nameof(統計単位地域コード))]
    public class M00120料率 : ModelBase
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
        /// 合併時識別コード
        /// </summary>
        [Required]
        [Column("合併時識別コード", Order = 4)]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 6)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 7)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 8)]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Required]
        [Column("統計単位地域コード", Order = 9)]
        [StringLength(5)]
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 責任保険歩合
        /// </summary>
        [Column("責任保険歩合")]
        public Decimal? 責任保険歩合 { get; set; }

        /// <summary>
        /// 共済掛金標準率
        /// </summary>
        [Column("共済掛金標準率")]
        public Decimal? 共済掛金標準率 { get; set; }

        /// <summary>
        /// 通常標準被害率
        /// </summary>
        [Column("通常標準被害率")]
        public Decimal? 通常標準被害率 { get; set; }

        /// <summary>
        /// 保険料基礎率
        /// </summary>
        [Column("保険料基礎率")]
        public Decimal? 保険料基礎率 { get; set; }

        /// <summary>
        /// 異常標準被害率
        /// </summary>
        [Column("異常標準被害率")]
        public Decimal? 異常標準被害率 { get; set; }

        /// <summary>
        /// 再保険料基礎率
        /// </summary>
        [Column("再保険料基礎率")]
        public Decimal? 再保険料基礎率 { get; set; }

        /// <summary>
        /// 国庫負担割合
        /// </summary>
        [Column("国庫負担割合")]
        public Decimal? 国庫負担割合 { get; set; }

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
