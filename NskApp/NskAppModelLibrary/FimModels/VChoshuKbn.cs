using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 名称M徴収区分
    /// </summary>
    [Serializable]
    [Table("v_choshu_kbn")]
    public class VChoshuKbn : ModelBase
    {
        /// <summary>
        /// 徴収区分コード
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("choshu_kbn_cd", Order = 1)]
        public Decimal ChoshuKbnCd { get; set; }

        /// <summary>
        /// 徴収区分名_選択
        /// </summary>
        [Required]
        [Column("choshu_kbn_select")]
        [StringLength(6)]
        public string ChoshuKbnSelect { get; set; }

        /// <summary>
        /// 徴収区分名_表示
        /// </summary>
        [Required]
        [Column("choshu_kbn_display")]
        [StringLength(3)]
        public string ChoshuKbnDisplay { get; set; }

        /// <summary>
        /// 徴収者入力制御フラグ
        /// </summary>
        [Required]
        [Column("choshu_nyuryoku_flg")]
        [StringLength(1)]
        public string ChoshuNyuryokuFlg { get; set; }

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
