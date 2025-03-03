using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_01050_バッチ条件
    /// </summary>
    [Serializable]
    [Table("t_01050_バッチ条件")]
    [PrimaryKey(nameof(バッチ条件id), nameof(連番))]
    public class T01050バッチ条件 : ModelBase
    {
        /// <summary>
        /// バッチ条件id
        /// </summary>
        [Required]
        [Column("バッチ条件id", Order = 1)]
        [StringLength(36)]
        public string バッチ条件id { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("連番", Order = 2)]
        public int 連番 { get; set; }

        /// <summary>
        /// 条件名称
        /// </summary>
        [Column("条件名称")]
        [StringLength(30)]
        public string 条件名称 { get; set; }

        /// <summary>
        /// 表示用条件値
        /// </summary>
        [Column("表示用条件値")]
        [StringLength(30)]
        public string 表示用条件値 { get; set; }

        /// <summary>
        /// 条件値
        /// </summary>
        [Column("条件値")]
        [StringLength(30)]
        public string 条件値 { get; set; }

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
