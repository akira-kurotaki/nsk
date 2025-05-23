using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// ¢ÑÇ}X^
    /// </summary>
    [Serializable]
    [Table("v_setaikanri")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd))]
    public class VSetaikanri : ModelBase
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
        /// ¢ÑÇÀ{tO
        /// </summary>
        [Required]
        [Column("setaikanri_flg")]
        [StringLength(1)]
        public string SetaikanriFlg { get; set; }

        /// <summary>
        /// ¢ÑÇæª
        /// </summary>
        [Column("setaikanri_kbn")]
        [StringLength(1)]
        public string SetaikanriKbn { get; set; }

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
