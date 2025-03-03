using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10030_危険段階共済掛金区分
    /// </summary>
    [Serializable]
    [Table("m_10030_危険段階共済掛金区分")]
    [PrimaryKey(nameof(共済事業危険段階), nameof(共済目的危険段階), nameof(危険段階共済掛金区分))]
    public class M10030危険段階共済掛金区分 : ModelBase
    {
        /// <summary>
        /// 共済事業危険段階
        /// </summary>
        [Required]
        [Column("共済事業危険段階", Order = 1)]
        [StringLength(2)]
        public string 共済事業危険段階 { get; set; }

        /// <summary>
        /// 共済目的危険段階
        /// </summary>
        [Required]
        [Column("共済目的危険段階", Order = 2)]
        [StringLength(2)]
        public string 共済目的危険段階 { get; set; }

        /// <summary>
        /// 危険段階共済掛金区分
        /// </summary>
        [Required]
        [Column("危険段階共済掛金区分", Order = 3)]
        [StringLength(16)]
        public string 危険段階共済掛金区分 { get; set; }

        /// <summary>
        /// 危険段階共済掛金区分名称
        /// </summary>
        [Column("危険段階共済掛金区分名称")]
        [StringLength(60)]
        public string 危険段階共済掛金区分名称 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Column("共済目的コード")]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

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
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

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
