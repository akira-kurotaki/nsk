using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// ”_’{í—Şƒ}ƒXƒ^
    /// </summary>
    [Serializable]
    [Table("m_nc_shurui")]
    public class MNcShurui : ModelBase
    {
        /// <summary>
        /// ”_’{í—ŞƒR[ƒh
        /// </summary>
        [Required]
        [Key]
        [Column("nc_shurui_cd", Order = 1)]
        [StringLength(2)]
        public string NcShuruiCd { get; set; }

        /// <summary>
        /// ”_’{í—Ş–¼
        /// </summary>
        [Column("nc_shurui_nm")]
        [StringLength(18)]
        public string NcShuruiNm { get; set; }

        /// <summary>
        /// ”_’{Y‹æ•ª
        /// </summary>
        [Required]
        [Column("nochikusan_kbn")]
        [StringLength(3)]
        public string NochikusanKbn { get; set; }

        /// <summary>
        /// íœƒtƒ‰ƒO
        /// </summary>
        [Required]
        [Column("delete_flg")]
        [StringLength(1)]
        public string DeleteFlg { get; set; }

        /// <summary>
        /// “o˜^ƒ†[ƒUID
        /// </summary>
        [Column("insert_user_id")]
        [StringLength(11)]
        public string InsertUserId { get; set; }

        /// <summary>
        /// “o˜^“ú
        /// </summary>
        [Column("insert_date")]
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// XVƒ†[ƒUID
        /// </summary>
        [Column("update_user_id")]
        [StringLength(11)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// XV“ú
        /// </summary>
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
