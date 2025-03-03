using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12020_引受チェック
    /// </summary>
    [Serializable]
    [Table("t_12020_引受チェック")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T12020引受チェック : ModelBase
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
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号", Order = 6)]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号", Order = 7)]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 耕地面積チェック
        /// </summary>
        [Column("耕地面積チェック")]
        [StringLength(1)]
        public string 耕地面積チェック { get; set; }

        /// <summary>
        /// 引受面積チェック
        /// </summary>
        [Column("引受面積チェック")]
        [StringLength(1)]
        public string 引受面積チェック { get; set; }

        /// <summary>
        /// 転作等面積チェック
        /// </summary>
        [Column("転作等面積チェック")]
        [StringLength(1)]
        public string 転作等面積チェック { get; set; }

        /// <summary>
        /// 受委託区分チェック
        /// </summary>
        [Column("受委託区分チェック")]
        [StringLength(1)]
        public string 受委託区分チェック { get; set; }

        /// <summary>
        /// 受委託者チェック
        /// </summary>
        [Column("受委託者チェック")]
        [StringLength(1)]
        public string 受委託者チェック { get; set; }

        /// <summary>
        /// 田畑区分チェック
        /// </summary>
        [Column("田畑区分チェック")]
        [StringLength(1)]
        public string 田畑区分チェック { get; set; }

        /// <summary>
        /// 区分チェック
        /// </summary>
        [Column("区分チェック")]
        [StringLength(1)]
        public string 区分チェック { get; set; }

        /// <summary>
        /// 種類チェック
        /// </summary>
        [Column("種類チェック")]
        [StringLength(1)]
        public string 種類チェック { get; set; }

        /// <summary>
        /// 類区分チェック
        /// </summary>
        [Column("類区分チェック")]
        [StringLength(1)]
        public string 類区分チェック { get; set; }

        /// <summary>
        /// 品種チェック
        /// </summary>
        [Column("品種チェック")]
        [StringLength(1)]
        public string 品種チェック { get; set; }

        /// <summary>
        /// 収量等級チェック
        /// </summary>
        [Column("収量等級チェック")]
        [StringLength(1)]
        public string 収量等級チェック { get; set; }

        /// <summary>
        /// 実量単収チェック
        /// </summary>
        [Column("実量単収チェック")]
        [StringLength(1)]
        public string 実量単収チェック { get; set; }

        /// <summary>
        /// 統計単収チェック
        /// </summary>
        [Column("統計単収チェック")]
        [StringLength(1)]
        public string 統計単収チェック { get; set; }

        /// <summary>
        /// 参酌チェック
        /// </summary>
        [Column("参酌チェック")]
        [StringLength(1)]
        public string 参酌チェック { get; set; }

        /// <summary>
        /// 関連チェック
        /// </summary>
        [Column("関連チェック")]
        [StringLength(1)]
        public string 関連チェック { get; set; }

        /// <summary>
        /// fimチェック
        /// </summary>
        [Column("fimチェック")]
        [StringLength(1)]
        public string fimチェック { get; set; }

        /// <summary>
        /// 各種条件チェック
        /// </summary>
        [Column("各種条件チェック")]
        [StringLength(1)]
        public string 各種条件チェック { get; set; }

        /// <summary>
        /// 引受方式チェック
        /// </summary>
        [Column("引受方式チェック")]
        [StringLength(1)]
        public string 引受方式チェック { get; set; }

        /// <summary>
        /// 支所コードチェック
        /// </summary>
        [Column("支所コードチェック")]
        [StringLength(1)]
        public string 支所コードチェック { get; set; }

        /// <summary>
        /// 用途区分チェック
        /// </summary>
        [Column("用途区分チェック")]
        [StringLength(1)]
        public string 用途区分チェック { get; set; }

        /// <summary>
        /// 産地別銘柄チェック
        /// </summary>
        [Column("産地別銘柄チェック")]
        [StringLength(1)]
        public string 産地別銘柄チェック { get; set; }

        /// <summary>
        /// 調整係数チェック
        /// </summary>
        [Column("調整係数チェック")]
        [StringLength(1)]
        public string 調整係数チェック { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("削除フラグ")]
        [StringLength(1)]
        public string 削除フラグ { get; set; }

        /// <summary>
        /// ERRフラグ
        /// </summary>
        [Column("ERRフラグ")]
        [StringLength(1)]
        public string ERRフラグ { get; set; }

        /// <summary>
        /// WARフラグ
        /// </summary>
        [Column("WARフラグ")]
        [StringLength(1)]
        public string WARフラグ { get; set; }

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
        /// 補償割合コード
        /// </summary>
        [Column("補償割合コード")]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

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
