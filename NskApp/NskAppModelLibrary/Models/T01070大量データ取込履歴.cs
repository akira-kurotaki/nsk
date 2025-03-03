using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_01070_大量データ取込履歴
    /// </summary>
    [Serializable]
    [Table("t_01070_大量データ取込履歴")]
    public class T01070大量データ取込履歴 : ModelBase
    {
        /// <summary>
        /// 取込履歴id
        /// </summary>
        [Required]
        [Key]
        [Column("取込履歴id", Order = 1)]
        [StringLength(19)]
        public string 取込履歴id { get; set; }

        /// <summary>
        /// 受入履歴id
        /// </summary>
        [Required]
        [Column("受入履歴id")]
        [StringLength(19)]
        public string 受入履歴id { get; set; }

        /// <summary>
        /// データ登録日時
        /// </summary>
        [Required]
        [Column("データ登録日時")]
        public DateTime データ登録日時 { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード")]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Required]
        [Column("ステータス")]
        [StringLength(2)]
        public string ステータス { get; set; }

        /// <summary>
        /// 対象データ区分
        /// </summary>
        [Column("対象データ区分")]
        [StringLength(2)]
        public string 対象データ区分 { get; set; }

        /// <summary>
        /// 対象件数
        /// </summary>
        [Column("対象件数")]
        public int? 対象件数 { get; set; }

        /// <summary>
        /// エラー件数
        /// </summary>
        [Column("エラー件数")]
        public int? エラー件数 { get; set; }

        /// <summary>
        /// エラーリスト名
        /// </summary>
        [Column("エラーリスト名")]
        [StringLength(100)]
        public string エラーリスト名 { get; set; }

        /// <summary>
        /// エラーリストパス
        /// </summary>
        [Column("エラーリストパス")]
        public string エラーリストパス { get; set; }

        /// <summary>
        /// エラーリストハッシュ値
        /// </summary>
        [Column("エラーリストハッシュ値")]
        [StringLength(64)]
        public string エラーリストハッシュ値 { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        [Column("コメント")]
        [StringLength(100)]
        public string コメント { get; set; }

        /// <summary>
        /// 開始日時
        /// </summary>
        [Column("開始日時")]
        public DateTime? 開始日時 { get; set; }

        /// <summary>
        /// 終了日時
        /// </summary>
        [Column("終了日時")]
        public DateTime? 終了日時 { get; set; }

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
