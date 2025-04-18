using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// _{pr}X^
    /// </summary>
    [Serializable]
    [Table("m_nc_yoto")]
    [PrimaryKey(nameof(NcShuruiCd), nameof(NcHimmokuCd), nameof(NcYotoCd))]
    public class MNcYoto : ModelBase
    {
        /// <summary>
        /// _{íÞR[h
        /// </summary>
        [Required]
        //[Key]
        [Column("nc_shurui_cd", Order = 1)]
        [StringLength(2)]
        public string NcShuruiCd { get; set; }

        /// <summary>
        /// _{iÚR[h
        /// </summary>
        [Required]
        //[Key]
        [Column("nc_himmoku_cd", Order = 2)]
        [StringLength(3)]
        public string NcHimmokuCd { get; set; }

        /// <summary>
        /// _{prR[h
        /// </summary>
        [Required]
        //[Key]
        [Column("nc_yoto_cd", Order = 3)]
        [StringLength(2)]
        public string NcYotoCd { get; set; }

        /// <summary>
        /// _{pr¼
        /// </summary>
        [Column("nc_yoto_nm")]
        [StringLength(18)]
        public string NcYotoNm { get; set; }

        /// <summary>
        /// ß\l®tO
        /// </summary>
        [Required]
        [Column("kakoshinkoku_yoshiki_flg")]
        [StringLength(1)]
        public string KakoshinkokuYoshikiFlg { get; set; }

        /// <summary>
        /// âtH[l®tO
        /// </summary>
        [Required]
        [Column("hojoform_yoshiki_flg")]
        [StringLength(1)]
        public string HojoformYoshikiFlg { get; set; }

        /// <summary>
        /// c_væl®tO
        /// </summary>
        [Required]
        [Column("einokeikaku_yoshiki_flg")]
        [StringLength(1)]
        public string EinokeikakuYoshikiFlg { get; set; }

        /// <summary>
        /// ûüZl®tO
        /// </summary>
        [Required]
        [Column("shunyushisan_yoshiki_flg")]
        [StringLength(1)]
        public string ShunyushisanYoshikiFlg { get; set; }

        /// <summary>
        /// ocÚWl®tO
        /// </summary>
        [Required]
        [Column("keieimokuhyo_yoshiki_flg")]
        [StringLength(1)]
        public string KeieimokuhyoYoshikiFlg { get; set; }

        /// <summary>
        /// ítO
        /// </summary>
        [Required]
        [Column("delete_flg")]
        [StringLength(1)]
        public string DeleteFlg { get; set; }

        /// <summary>
        /// o^[UID
        /// </summary>
        [Column("insert_user_id")]
        [StringLength(11)]
        public string InsertUserId { get; set; }

        /// <summary>
        /// o^ú
        /// </summary>
        [Column("insert_date")]
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// XV[UID
        /// </summary>
        [Column("update_user_id")]
        [StringLength(11)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// XVú
        /// </summary>
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
