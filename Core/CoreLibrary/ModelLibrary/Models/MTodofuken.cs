using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// s¹{§}X^
    /// </summary>
    [Serializable]
    [Table("m_todofuken")]
    public class MTodofuken : ModelBase
    {
        /// <summary>
        /// s¹{§R[h
        /// </summary>
        [Required]
        [Key]
        [Column("todofuken_cd", Order = 1)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// s¹{§¼
        /// </summary>
        [Column("todofuken_nm")]
        [StringLength(10)]
        public string TodofukenNm { get; set; }

        /// <summary>
        /// næubNR[h
        /// </summary>
        [Column("area_block_cd")]
        [StringLength(2)]
        public string AreaBlockCd { get; set; }

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
