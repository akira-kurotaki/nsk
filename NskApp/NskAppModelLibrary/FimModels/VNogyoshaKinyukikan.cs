using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 農業者金融機関
    /// </summary>
    [Serializable]
    [Table("v_nogyosha_kinyukikan")]
    [PrimaryKey(nameof(NogyoshaId), nameof(KyosaiJigyoCd), nameof(KyosaiMokutekitoCd), nameof(FurikomiHikiotoshiCd))]
    public class VNogyoshaKinyukikan : ModelBase
    {
        /// <summary>
        /// 農業者ID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 共済事業コード
        /// </summary>
        [Required]
        [Column("kyosai_jigyo_cd", Order = 2)]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// 共済目的等コード
        /// </summary>
        [Required]
        [Column("kyosai_mokutekito_cd", Order = 3)]
        [StringLength(2)]
        public string KyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// 振込引落区分コード
        /// </summary>
        [Required]
        [Column("furikomi_hikiotoshi_cd", Order = 4)]
        [StringLength(2)]
        public string FurikomiHikiotoshiCd { get; set; }

        /// <summary>
        /// 自動振替処理の有無
        /// </summary>
        [Column("jidofurikae_cd")]
        [StringLength(1)]
        public string JidofurikaeCd { get; set; }

        /// <summary>
        /// 金融機関コード
        /// </summary>
        [Column("kinyukikan_cd")]
        [StringLength(7)]
        public string KinyukikanCd { get; set; }

        /// <summary>
        /// 預金種別
        /// </summary>
        [Column("yokin_sbt")]
        [StringLength(1)]
        public string YokinSbt { get; set; }

        /// <summary>
        /// 口座番号
        /// </summary>
        [Column("koza_num")]
        [StringLength(10)]
        public string KozaNum { get; set; }

        /// <summary>
        /// 口座名義人カナ
        /// </summary>
        [Column("kozameigi_kana")]
        [StringLength(30)]
        public string KozameigiKana { get; set; }

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
