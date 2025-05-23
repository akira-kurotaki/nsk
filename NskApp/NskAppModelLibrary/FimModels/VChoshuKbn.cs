using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// ¼ÌM¥ûæª
    /// </summary>
    [Serializable]
    [Table("v_choshu_kbn")]
    public class VChoshuKbn : ModelBase
    {
        /// <summary>
        /// ¥ûæªR[h
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("choshu_kbn_cd", Order = 1)]
        public Decimal ChoshuKbnCd { get; set; }

        /// <summary>
        /// ¥ûæª¼_Ið
        /// </summary>
        [Required]
        [Column("choshu_kbn_select")]
        [StringLength(6)]
        public string ChoshuKbnSelect { get; set; }

        /// <summary>
        /// ¥ûæª¼_\¦
        /// </summary>
        [Required]
        [Column("choshu_kbn_display")]
        [StringLength(3)]
        public string ChoshuKbnDisplay { get; set; }

        /// <summary>
        /// ¥ûÒüÍ§ätO
        /// </summary>
        [Required]
        [Column("choshu_nyuryoku_flg")]
        [StringLength(1)]
        public string ChoshuNyuryokuFlg { get; set; }

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
