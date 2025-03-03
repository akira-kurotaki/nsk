using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12140_引受耕地削除データ保持
    /// </summary>
    [Serializable]
    [Table("t_12140_引受耕地削除データ保持")]
    [Keyless]
    public class T12140引受耕地削除データ保持 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード")]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [Column("年産")]
        public short 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード")]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード")]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号")]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号")]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 地名地番
        /// </summary>
        [Column("地名地番")]
        [StringLength(40)]
        public string 地名地番 { get; set; }

        /// <summary>
        /// 削除日時
        /// </summary>
        [Column("削除日時")]
        public DateTime? 削除日時 { get; set; }

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
