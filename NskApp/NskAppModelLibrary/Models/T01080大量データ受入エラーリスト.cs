using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_01080_大量データ受入_エラーリスト
    /// </summary>
    [Serializable]
    [Table("t_01080_大量データ受入_エラーリスト")]
    [PrimaryKey(nameof(処理区分), nameof(履歴id), nameof(枝番))]
    public class T01080大量データ受入エラーリスト : ModelBase
    {
        /// <summary>
        /// 処理区分
        /// </summary>
        [Required]
        [Column("処理区分", Order = 1)]
        [StringLength(1)]
        public string 処理区分 { get; set; }

        /// <summary>
        /// 履歴id
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("履歴id", Order = 2)]
        public long 履歴id { get; set; }

        /// <summary>
        /// 枝番
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("枝番", Order = 3)]
        public long 枝番 { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        [Column("行番号")]
        [StringLength(7)]
        public string 行番号 { get; set; }

        /// <summary>
        /// エラー内容
        /// </summary>
        [Column("エラー内容")]
        public string エラー内容 { get; set; }

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
