using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// ¼ÌM¥ûR
    /// </summary>
    [Serializable]
    [Table("v_choshu_riyu")]
    [PrimaryKey(nameof(ChoshuKbnCd), nameof(ChoshuRiyuCd))]
    public class VChoshuRiyu : ModelBase
    {
        /// <summary>
        /// ¥ûæªR[h
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("choshu_kbn_cd", Order = 1)]
        public Decimal ChoshuKbnCd { get; set; }

        /// <summary>
        /// ¥ûRR[h
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("choshu_riyu_cd", Order = 2)]
        public Decimal ChoshuRiyuCd { get; set; }

        /// <summary>
        /// ¥ûR¼
        /// </summary>
        [Required]
        [Column("choshu_riyu_nm")]
        [StringLength(20)]
        public string ChoshuRiyuNm { get; set; }

        /// <summary>
        /// ²¸ÎÛL³
        /// </summary>
        [Required]
        [Column("chosa_umu")]
        [StringLength(1)]
        public string ChosaUmu { get; set; }

        /// <summary>
        /// _ì¨¤ÏÎÛtO
        /// </summary>
        [Required]
        [Column("nsk_taisho_flg")]
        [StringLength(1)]
        public string NskTaishoFlg { get; set; }

        /// <summary>
        /// ¨ì¨¤ÏÎÛtO
        /// </summary>
        [Required]
        [Column("hat_taisho_flg")]
        [StringLength(1)]
        public string HatTaishoFlg { get; set; }

        /// <summary>
        /// Ê÷¤ÏÎÛtO
        /// </summary>
        [Required]
        [Column("kju_taisho_flg")]
        [StringLength(1)]
        public string KjuTaishoFlg { get; set; }

        /// <summary>
        /// Æ{¤ÏÎÛtO
        /// </summary>
        [Required]
        [Column("ktk_taisho_flg")]
        [StringLength(1)]
        public string KtkTaishoFlg { get; set; }

        /// <summary>
        /// |{Ý¤ÏÎÛtO
        /// </summary>
        [Required]
        [Column("eng_taisho_flg")]
        [StringLength(1)]
        public string EngTaishoFlg { get; set; }

        /// <summary>
        /// CÓ¤ÏÎÛtO
        /// </summary>
        [Required]
        [Column("nin_taisho_flg")]
        [StringLength(1)]
        public string NinTaishoFlg { get; set; }

        /// <summary>
        /// \õtO
        /// </summary>
        [Required]
        [Column("yb_flg")]
        [StringLength(1)]
        public string YbFlg { get; set; }

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
