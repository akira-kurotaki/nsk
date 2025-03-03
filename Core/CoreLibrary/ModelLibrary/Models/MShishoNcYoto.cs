using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 支所別農畜用途マスタ
    /// </summary>
    [Serializable]
    [Table("m_shisho_nc_yoto")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(ShishoCd), nameof(NcShuruiCd), nameof(NcHimmokuCd), nameof(NcYotoCd))]
    public class MShishoNcYoto : ModelBase
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        //[Key]
        [Column("todofuken_cd", Order = 1)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        //[Key]
        [Column("kumiaito_cd", Order = 2)]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Required]
        //[Key]
        [Column("shisho_cd", Order = 3)]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 農畜種類コード
        /// </summary>
        [Required]
        //[Key]
        [Column("nc_shurui_cd", Order = 4)]
        [StringLength(2)]
        public string NcShuruiCd { get; set; }

        /// <summary>
        /// 農畜品目コード
        /// </summary>
        [Required]
        //[Key]
        [Column("nc_himmoku_cd", Order = 5)]
        [StringLength(3)]
        public string NcHimmokuCd { get; set; }

        /// <summary>
        /// 農畜用途コード
        /// </summary>
        [Required]
        //[Key]
        [Column("nc_yoto_cd", Order = 6)]
        [StringLength(2)]
        public string NcYotoCd { get; set; }

        /// <summary>
        /// 支所別農畜用途コード
        /// </summary>
        [Column("shisho_nc_yoto_cd")]
        [StringLength(2)]
        public string ShishoNcYotoCd { get; set; }

        /// <summary>
        /// 非表示フラグ
        /// </summary>
        [Required]
        [Column("hidden_flg")]
        [StringLength(1)]
        public string HiddenFlg { get; set; }

        /// <summary>
        /// 登録ユーザID
        /// </summary>
        [Column("insert_user_id")]
        [StringLength(11)]
        public string InsertUserId { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("insert_date")]
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// 更新ユーザID
        /// </summary>
        [Column("update_user_id")]
        [StringLength(11)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
