using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22020_仮渡計算組合員等別
    /// </summary>
    [Serializable]
    [Table("t_22020_仮渡計算組合員等別")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(組合員等コード), nameof(統計単位地域コード), nameof(仮渡回))]
    public class T22020仮渡計算組合員等別 : ModelBase
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
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 4)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 5)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Required]
        [Column("統計単位地域コード", Order = 6)]
        [StringLength(5)]
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 仮渡回
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("仮渡回", Order = 7)]
        public short 仮渡回 { get; set; }

        /// <summary>
        /// 共済金額単価
        /// </summary>
        [Column("共済金額単価")]
        public Decimal? 共済金額単価 { get; set; }

        /// <summary>
        /// 一筆全損皆無筆数
        /// </summary>
        [Column("一筆全損皆無筆数")]
        public Decimal? 一筆全損皆無筆数 { get; set; }

        /// <summary>
        /// 一筆全損皆無基準収穫量
        /// </summary>
        [Column("一筆全損皆無基準収穫量")]
        public Decimal? 一筆全損皆無基準収穫量 { get; set; }

        /// <summary>
        /// 一筆全損皆無減収量
        /// </summary>
        [Column("一筆全損皆無減収量")]
        public Decimal? 一筆全損皆無減収量 { get; set; }

        /// <summary>
        /// 一筆全損不能筆数
        /// </summary>
        [Column("一筆全損不能筆数")]
        public Decimal? 一筆全損不能筆数 { get; set; }

        /// <summary>
        /// 一筆全損不能基準収穫量
        /// </summary>
        [Column("一筆全損不能基準収穫量")]
        public Decimal? 一筆全損不能基準収穫量 { get; set; }

        /// <summary>
        /// 一筆全損不能減収量
        /// </summary>
        [Column("一筆全損不能減収量")]
        public Decimal? 一筆全損不能減収量 { get; set; }

        /// <summary>
        /// 一筆全損支払開始減収量
        /// </summary>
        [Column("一筆全損支払開始減収量")]
        public Decimal? 一筆全損支払開始減収量 { get; set; }

        /// <summary>
        /// 一筆全損共済減収量
        /// </summary>
        [Column("一筆全損共済減収量")]
        public Decimal? 一筆全損共済減収量 { get; set; }

        /// <summary>
        /// 一筆全損仮渡共済金見込額
        /// </summary>
        [Column("一筆全損仮渡共済金見込額")]
        public Decimal? 一筆全損仮渡共済金見込額 { get; set; }

        /// <summary>
        /// 一筆半損被害筆数
        /// </summary>
        [Column("一筆半損被害筆数")]
        public Decimal? 一筆半損被害筆数 { get; set; }

        /// <summary>
        /// 一筆半損被害基準収穫量
        /// </summary>
        [Column("一筆半損被害基準収穫量")]
        public Decimal? 一筆半損被害基準収穫量 { get; set; }

        /// <summary>
        /// 一筆半損被害減収量
        /// </summary>
        [Column("一筆半損被害減収量")]
        public Decimal? 一筆半損被害減収量 { get; set; }

        /// <summary>
        /// 一筆半損支払開始減収量
        /// </summary>
        [Column("一筆半損支払開始減収量")]
        public Decimal? 一筆半損支払開始減収量 { get; set; }

        /// <summary>
        /// 一筆半損共済減収量
        /// </summary>
        [Column("一筆半損共済減収量")]
        public Decimal? 一筆半損共済減収量 { get; set; }

        /// <summary>
        /// 一筆半損仮渡共済金見込額
        /// </summary>
        [Column("一筆半損仮渡共済金見込額")]
        public Decimal? 一筆半損仮渡共済金見込額 { get; set; }

        /// <summary>
        /// 仮渡共済金見込額
        /// </summary>
        [Column("仮渡共済金見込額")]
        public Decimal? 仮渡共済金見込額 { get; set; }

        /// <summary>
        /// 青申選択フラグ
        /// </summary>
        [Column("青申選択フラグ")]
        [StringLength(1)]
        public string 青申選択フラグ { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Column("合併時識別コード")]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Column("大地区コード")]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        [Column("小地区コード")]
        [StringLength(4)]
        public string 小地区コード { get; set; }

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
