using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12030_用途チェック
    /// </summary>
    [Serializable]
    [Table("t_12030_用途チェック")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(類区分), nameof(統計単位地域コード), nameof(用途区分))]
    public class T12030用途チェック : ModelBase
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
        /// 統計単位地域コード
        /// </summary>
        [Required]
        [Column("統計単位地域コード", Order = 6)]
        [StringLength(5)]
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 7)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 引受ERRフラグ
        /// </summary>
        [Column("引受ERRフラグ")]
        [StringLength(1)]
        public string 引受ERRフラグ { get; set; }

        /// <summary>
        /// 引受WARフラグ
        /// </summary>
        [Column("引受WARフラグ")]
        [StringLength(1)]
        public string 引受WARフラグ { get; set; }

        /// <summary>
        /// 引受ERRNO
        /// </summary>
        [Column("引受ERRNO")]
        [StringLength(30)]
        public string 引受ERRNO { get; set; }

        /// <summary>
        /// 引受SUBJECT
        /// </summary>
        [Column("引受SUBJECT")]
        [StringLength(100)]
        public string 引受SUBJECT { get; set; }

        /// <summary>
        /// 引受ERRメッセージ
        /// </summary>
        [Column("引受ERRメッセージ")]
        [StringLength(512)]
        public string 引受ERRメッセージ { get; set; }

        /// <summary>
        /// 評価ERRフラグ
        /// </summary>
        [Column("評価ERRフラグ")]
        [StringLength(1)]
        public string 評価ERRフラグ { get; set; }

        /// <summary>
        /// 評価WARフラグ
        /// </summary>
        [Column("評価WARフラグ")]
        [StringLength(1)]
        public string 評価WARフラグ { get; set; }

        /// <summary>
        /// 評価ERRNO
        /// </summary>
        [Column("評価ERRNO")]
        [StringLength(30)]
        public string 評価ERRNO { get; set; }

        /// <summary>
        /// 評価SUBJECT
        /// </summary>
        [Column("評価SUBJECT")]
        [StringLength(100)]
        public string 評価SUBJECT { get; set; }

        /// <summary>
        /// 評価ERRメッセージ
        /// </summary>
        [Column("評価ERRメッセージ")]
        [StringLength(512)]
        public string 評価ERRメッセージ { get; set; }

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
