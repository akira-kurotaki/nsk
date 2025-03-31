using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_11100_引受gis
    /// </summary>
    [Serializable]
    [Table("t_11100_引受gis")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T11100引受gis : ModelBase
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
        /// rs区分
        /// </summary>
        [Column("rs区分")]
        [StringLength(2)]
        public string rs区分 { get; set; }

        /// <summary>
        /// 局都道府県コード
        /// </summary>
        [Column("局都道府県コード")]
        [StringLength(4)]
        public string 局都道府県コード { get; set; }

        /// <summary>
        /// 市区町村コード
        /// </summary>
        [Column("市区町村コード")]
        [StringLength(3)]
        public string 市区町村コード { get; set; }

        /// <summary>
        /// 大字コード
        /// </summary>
        [Column("大字コード")]
        [StringLength(8)]
        public string 大字コード { get; set; }

        /// <summary>
        /// 小字コード
        /// </summary>
        [Column("小字コード")]
        [StringLength(4)]
        public string 小字コード { get; set; }

        /// <summary>
        /// 地番
        /// </summary>
        [Column("地番")]
        [StringLength(16)]
        public string 地番 { get; set; }

        /// <summary>
        /// 枝番
        /// </summary>
        [Column("枝番")]
        [StringLength(14)]
        public string 枝番 { get; set; }

        /// <summary>
        /// 子番
        /// </summary>
        [Column("子番")]
        [StringLength(10)]
        public string 子番 { get; set; }

        /// <summary>
        /// 孫番
        /// </summary>
        [Column("孫番")]
        [StringLength(10)]
        public string 孫番 { get; set; }

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
