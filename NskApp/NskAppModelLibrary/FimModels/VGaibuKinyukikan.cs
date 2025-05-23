using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// OàZ@Ö}X^
    /// </summary>
    [Serializable]
    [Table("v_gaibu_kinyukikan")]
    [PrimaryKey(nameof(KinyukikanCd), nameof(SortCd))]
    public class VGaibuKinyukikan : ModelBase
    {
        /// <summary>
        /// àZ@ÖR[h
        /// </summary>
        [Required]
        [Column("kinyukikan_cd", Order = 1)]
        [StringLength(7)]
        public string KinyukikanCd { get; set; }

        /// <summary>
        /// ÀÑR[h
        /// </summary>
        [Required]
        [Column("sort_cd", Order = 2)]
        [StringLength(1)]
        public string SortCd { get; set; }

        /// <summary>
        /// âsÔ
        /// </summary>
        [Column("ginkou_no")]
        [StringLength(4)]
        public string GinkouNo { get; set; }

        /// <summary>
        /// xXÔ
        /// </summary>
        [Column("shiten_no")]
        [StringLength(3)]
        public string ShitenNo { get; set; }

        /// <summary>
        /// àZ@Ö¼Ji
        /// </summary>
        [Column("kinyukikan_kana")]
        public string KinyukikanKana { get; set; }

        /// <summary>
        /// àZ@Ö¼
        /// </summary>
        [Column("kinyukikan_nm")]
        public string KinyukikanNm { get; set; }

        /// <summary>
        /// XÜ¼Ji
        /// </summary>
        [Column("mise_kana")]
        public string MiseKana { get; set; }

        /// <summary>
        /// XÜ¼
        /// </summary>
        [Column("mise_nm")]
        public string MiseNm { get; set; }

        /// <summary>
        /// XÜXÖÔ
        /// </summary>
        [Column("mise_postal_cd")]
        [StringLength(8)]
        public string MisePostalCd { get; set; }

        /// <summary>
        /// XÜÝn
        /// </summary>
        [Column("mise_address")]
        public string MiseAddress { get; set; }

        /// <summary>
        /// XÜdbÔ
        /// </summary>
        [Column("mise_tel")]
        [StringLength(17)]
        public string MiseTel { get; set; }

        /// <summary>
        /// è`ð·Ô
        /// </summary>
        [Column("tegata_no")]
        [StringLength(4)]
        public string TegataNo { get; set; }

        /// <summary>
        /// à×Ö§xÁ¿
        /// </summary>
        [Column("kawase_kamei_kbn")]
        [StringLength(1)]
        public string KawaseKameiKbn { get; set; }

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
