using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 名称M徴収理由
    /// </summary>
    [Serializable]
    [Table("v_choshu_riyu")]
    [PrimaryKey(nameof(ChoshuKbnCd), nameof(ChoshuRiyuCd))]
    public class VChoshuRiyu : ModelBase
    {
        /// <summary>
        /// 徴収区分コード
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("choshu_kbn_cd", Order = 1)]
        public Decimal ChoshuKbnCd { get; set; }

        /// <summary>
        /// 徴収理由コード
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("choshu_riyu_cd", Order = 2)]
        public Decimal ChoshuRiyuCd { get; set; }

        /// <summary>
        /// 徴収理由名
        /// </summary>
        [Required]
        [Column("choshu_riyu_nm")]
        [StringLength(20)]
        public string ChoshuRiyuNm { get; set; }

        /// <summary>
        /// 調査対象有無
        /// </summary>
        [Required]
        [Column("chosa_umu")]
        [StringLength(1)]
        public string ChosaUmu { get; set; }

        /// <summary>
        /// 農作物共済対象フラグ
        /// </summary>
        [Required]
        [Column("nsk_taisho_flg")]
        [StringLength(1)]
        public string NskTaishoFlg { get; set; }

        /// <summary>
        /// 畑作物共済対象フラグ
        /// </summary>
        [Required]
        [Column("hat_taisho_flg")]
        [StringLength(1)]
        public string HatTaishoFlg { get; set; }

        /// <summary>
        /// 果樹共済対象フラグ
        /// </summary>
        [Required]
        [Column("kju_taisho_flg")]
        [StringLength(1)]
        public string KjuTaishoFlg { get; set; }

        /// <summary>
        /// 家畜共済対象フラグ
        /// </summary>
        [Required]
        [Column("ktk_taisho_flg")]
        [StringLength(1)]
        public string KtkTaishoFlg { get; set; }

        /// <summary>
        /// 園芸施設共済対象フラグ
        /// </summary>
        [Required]
        [Column("eng_taisho_flg")]
        [StringLength(1)]
        public string EngTaishoFlg { get; set; }

        /// <summary>
        /// 任意共済対象フラグ
        /// </summary>
        [Required]
        [Column("nin_taisho_flg")]
        [StringLength(1)]
        public string NinTaishoFlg { get; set; }

        /// <summary>
        /// 予備フラグ
        /// </summary>
        [Required]
        [Column("yb_flg")]
        [StringLength(1)]
        public string YbFlg { get; set; }

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
