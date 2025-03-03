using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_11030_個人設定類
    /// </summary>
    [Serializable]
    [Table("t_11030_個人設定類")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(類区分), nameof(引受区分))]
    public class T11030個人設定類 : ModelBase
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
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 引受区分
        /// </summary>
        [Required]
        [Column("引受区分", Order = 6)]
        [StringLength(2)]
        public string 引受区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Column("引受方式")]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Column("特約区分")]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Column("補償割合コード")]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 付保割合
        /// </summary>
        [Column("付保割合")]
        public Decimal? 付保割合 { get; set; }

        /// <summary>
        /// 種類区分
        /// </summary>
        [Column("種類区分")]
        [StringLength(1)]
        public string 種類区分 { get; set; }

        /// <summary>
        /// 作付時期
        /// </summary>
        [Column("作付時期")]
        [StringLength(1)]
        public string 作付時期 { get; set; }

        /// <summary>
        /// 田畑区分
        /// </summary>
        [Column("田畑区分")]
        [StringLength(1)]
        public string 田畑区分 { get; set; }

        /// <summary>
        /// 共済金額選択順位
        /// </summary>
        [Column("共済金額選択順位")]
        public Decimal? 共済金額選択順位 { get; set; }

        /// <summary>
        /// 収穫量確認方法
        /// </summary>
        [Column("収穫量確認方法")]
        [StringLength(2)]
        public string 収穫量確認方法 { get; set; }

        /// <summary>
        /// 全相殺基準単収
        /// </summary>
        [Column("全相殺基準単収")]
        public Decimal? 全相殺基準単収 { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Column("営農対象外フラグ")]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 担手農家区分
        /// </summary>
        [Column("担手農家区分")]
        [StringLength(1)]
        public string 担手農家区分 { get; set; }

        /// <summary>
        /// 全相殺受託者等名称
        /// </summary>
        [Column("全相殺受託者等名称")]
        [StringLength(30)]
        public string 全相殺受託者等名称 { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("備考")]
        [StringLength(30)]
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
