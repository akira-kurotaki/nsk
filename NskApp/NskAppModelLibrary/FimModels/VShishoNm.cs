using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// ¼ÌMx
    /// </summary>
    [Serializable]
    [Table("v_shisho_nm")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(ShishoCd))]
    public class VShishoNm : ModelBase
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
        /// xR[h
        /// </summary>
        [Required]
        [Column("shisho_cd", Order = 3)]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// x¼
        /// </summary>
        [Column("shisho_nm")]
        [StringLength(10)]
        public string ShishoNm { get; set; }

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
