using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20060_計算割合
    /// </summary>
    [Serializable]
    [Table("m_20060_計算割合")]
    [PrimaryKey(nameof(引受方式), nameof(補償割合コード))]
    public class M20060計算割合 : ModelBase
    {
        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 1)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 2)]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 不能耕地収穫割合
        /// </summary>
        [Column("不能耕地収穫割合")]
        public Decimal? 不能耕地収穫割合 { get; set; }

        /// <summary>
        /// 支払開始損害割合
        /// </summary>
        [Column("支払開始損害割合")]
        public Decimal? 支払開始損害割合 { get; set; }

        /// <summary>
        /// 支払開始損害割合名称
        /// </summary>
        [Column("支払開始損害割合名称")]
        [StringLength(20)]
        public string 支払開始損害割合名称 { get; set; }

        /// <summary>
        /// 支払開始損害割合名称特例
        /// </summary>
        [Column("支払開始損害割合名称特例")]
        [StringLength(20)]
        public string 支払開始損害割合名称特例 { get; set; }

        /// <summary>
        /// 全損耕地支払開始割合
        /// </summary>
        [Column("全損耕地支払開始割合")]
        public Decimal? 全損耕地支払開始割合 { get; set; }

        /// <summary>
        /// 全損不能減収割合
        /// </summary>
        [Column("全損不能減収割合")]
        public Decimal? 全損不能減収割合 { get; set; }

        /// <summary>
        /// 半損耕地支払開始割合
        /// </summary>
        [Column("半損耕地支払開始割合")]
        public Decimal? 半損耕地支払開始割合 { get; set; }

        /// <summary>
        /// 特例支払開始割合
        /// </summary>
        [Column("特例支払開始割合")]
        public Decimal? 特例支払開始割合 { get; set; }

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
