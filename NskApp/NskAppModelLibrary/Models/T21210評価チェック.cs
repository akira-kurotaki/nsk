using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21210_評価チェック
    /// </summary>
    [Serializable]
    [Table("t_21210_評価チェック")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T21210評価チェック : ModelBase
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
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号", Order = 5)]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号", Order = 6)]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// ERRCODE
        /// </summary>
        [Column("ERRCODE")]
        [StringLength(2)]
        public string ERRCODE { get; set; }

        /// <summary>
        /// 類区分CK
        /// </summary>
        [Column("類区分CK")]
        [StringLength(1)]
        public string 類区分CK { get; set; }

        /// <summary>
        /// 評価面積CK
        /// </summary>
        [Column("評価面積CK")]
        [StringLength(1)]
        public string 評価面積CK { get; set; }

        /// <summary>
        /// 評価地区CK
        /// </summary>
        [Column("評価地区CK")]
        [StringLength(1)]
        public string 評価地区CK { get; set; }

        /// <summary>
        /// 階層区分CK
        /// </summary>
        [Column("階層区分CK")]
        [StringLength(1)]
        public string 階層区分CK { get; set; }

        /// <summary>
        /// 判定CK
        /// </summary>
        [Column("判定CK")]
        [StringLength(1)]
        public string 判定CK { get; set; }

        /// <summary>
        /// 悉皆単収CK
        /// </summary>
        [Column("悉皆単収CK")]
        [StringLength(1)]
        public string 悉皆単収CK { get; set; }

        /// <summary>
        /// 分割CK
        /// </summary>
        [Column("分割CK")]
        [StringLength(1)]
        public string 分割CK { get; set; }

        /// <summary>
        /// 一筆全半判定CK
        /// </summary>
        [Column("一筆全半判定CK")]
        [StringLength(1)]
        public string 一筆全半判定CK { get; set; }

        /// <summary>
        /// 入力日付CK
        /// </summary>
        [Column("入力日付CK")]
        [StringLength(1)]
        public string 入力日付CK { get; set; }

        /// <summary>
        /// ERRフラグ
        /// </summary>
        [Column("ERRフラグ")]
        [StringLength(1)]
        public string ERRフラグ { get; set; }

        /// <summary>
        /// WORフラグ
        /// </summary>
        [Column("WORフラグ")]
        [StringLength(1)]
        public string WORフラグ { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("削除フラグ")]
        [StringLength(1)]
        public string 削除フラグ { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Column("合併時識別コード")]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

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
        /// 補償割合コード
        /// </summary>
        [Column("補償割合コード")]
        [StringLength(1)]
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
    }
}
