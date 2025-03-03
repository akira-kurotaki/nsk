using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_19050_大量データ受入_組合員等類別設定ok
    /// </summary>
    [Serializable]
    [Table("t_19050_大量データ受入_組合員等類別設定ok")]
    [PrimaryKey(nameof(受入履歴id), nameof(行番号))]
    public class T19050大量データ受入組合員等類別設定ok : ModelBase
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
        [StringLength(2)]
        public string 付保割合 { get; set; }

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
        [StringLength(2)]
        public string 田畑区分 { get; set; }

        /// <summary>
        /// 共済金額選択順位
        /// </summary>
        [Column("共済金額選択順位")]
        [StringLength(2)]
        public string 共済金額選択順位 { get; set; }

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
        /// 営農支払以外フラグ
        /// </summary>
        [Column("営農支払以外フラグ")]
        [StringLength(1)]
        public string 営農支払以外フラグ { get; set; }

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
    }
}
