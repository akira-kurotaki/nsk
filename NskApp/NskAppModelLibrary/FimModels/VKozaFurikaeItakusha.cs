using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// ûÀUÖÏõÒ}X^
    /// </summary>
    [Serializable]
    [Table("v_koza_furikae_itakusha")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(KanriNo))]
    public class VKozaFurikaeItakusha : ModelBase
    {
        /// <summary>
        /// s¹{§R[h
        /// </summary>
        [Required]
        [Column("todofuken_cd", Order = 1)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("kumiaito_cd", Order = 2)]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// Çmn
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("kanri_no", Order = 3)]
        public short KanriNo { get; set; }

        /// <summary>
        /// xR[h
        /// </summary>
        [Column("shisho_cd")]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// ¤ÏÆR[h
        /// </summary>
        [Column("kyosai_jigyo_cd")]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Column("kyosai_mokutekito_cd")]
        [StringLength(2)]
        public string KyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// pr
        /// </summary>
        [Column("yoto")]
        [StringLength(30)]
        public string Yoto { get; set; }

        /// <summary>
        /// àZ@ÖR[h
        /// </summary>
        [Column("kinyukikan_cd")]
        [StringLength(7)]
        public string KinyukikanCd { get; set; }

        /// <summary>
        /// ÏõÒR[h
        /// </summary>
        [Column("itakusha_cd")]
        [StringLength(10)]
        public string ItakushaCd { get; set; }

        /// <summary>
        /// ÏõÒ¼
        /// </summary>
        [Column("itakusha_nm")]
        [StringLength(30)]
        public string ItakushaNm { get; set; }

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
