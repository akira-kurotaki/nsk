using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10140_種類名称
    /// </summary>
    [Serializable]
    [Table("m_10140_種類名称")]
    [PrimaryKey(nameof(共済目的コード), nameof(種類コード))]
    public class M10140種類名称 : ModelBase
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 1)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 種類コード
        /// </summary>
        [Required]
        [Column("種類コード", Order = 2)]
        [StringLength(2)]
        public string 種類コード { get; set; }

        /// <summary>
        /// 種類名称
        /// </summary>
        [Column("種類名称")]
        [StringLength(20)]
        public string 種類名称 { get; set; }

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
        /// 水稲表示用フラグ
        /// </summary>
        [Column("水稲表示用フラグ")]
        [StringLength(1)]
        public string 水稲表示用フラグ { get; set; }

        /// <summary>
        /// 陸稲表示用フラグ
        /// </summary>
        [Column("陸稲表示用フラグ")]
        [StringLength(1)]
        public string 陸稲表示用フラグ { get; set; }

        /// <summary>
        /// 麦表示用フラグ
        /// </summary>
        [Column("麦表示用フラグ")]
        [StringLength(1)]
        public string 麦表示用フラグ { get; set; }

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
