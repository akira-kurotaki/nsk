using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// |à¥û¤Ïàx¥óµi_ì¨¤Ïj
    /// </summary>
    [Serializable]
    [Table("v_shiharai_jokyo_nosakumotsu")]
    [PrimaryKey(nameof(NogyoshaId), nameof(KanyuJokyoId), nameof(ChoshuShiharaiKbn), nameof(ChoshuShiharaiDate))]
    public class VShiharaiJokyoNosakumotsu : ModelBase
    {
        /// <summary>
        /// _ÆÒID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// ÁüóµID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("kanyu_jokyo_id", Order = 2)]
        public int KanyuJokyoId { get; set; }

        /// <summary>
        /// ¤ÏÆR[h
        /// </summary>
        [Column("kyosai_jigyo_cd")]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// øó¤ÏÚIR[h
        /// </summary>
        [Column("hikiuke_kyosai_mokutekito_cd")]
        [StringLength(15)]
        public string HikiukeKyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// øóû®æª
        /// </summary>
        [Column("hikiuke_hoshiki_kbn")]
        [StringLength(2)]
        public string HikiukeHoshikiKbn { get; set; }

        /// <summary>
        /// NY^Nx
        /// </summary>
        [Column("nensan_nendo")]
        public short? NensanNendo { get; set; }

        /// <summary>
        /// øóæª
        /// </summary>
        [Column("hikiuke_kbn")]
        [StringLength(3)]
        public string HikiukeKbn { get; set; }

        /// <summary>
        /// ¥ûx¥æª
        /// </summary>
        [Required]
        [Column("choshu_shiharai_kbn", Order = 3)]
        [StringLength(1)]
        public string ChoshuShiharaiKbn { get; set; }

        /// <summary>
        /// ¥ûx¥Nú
        /// </summary>
        [Required]
        [Column("choshu_shiharai_date", Order = 4)]
        public DateTime ChoshuShiharaiDate { get; set; }

        /// <summary>
        /// ¤Ï|à^Û¯¿
        /// </summary>
        [Column("kyosai_kakekinto_hokenryo")]
        public long? KyosaiKakekintoHokenryo { get; set; }

        /// <summary>
        /// ¤Ï|à^Û¯¿_¤¿[®
        /// </summary>
        [Column("kyosai_kakekinto_hokenryo_nayato")]
        public long? KyosaiKakekintoHokenryoNayato { get; set; }

        /// <summary>
        /// gõS|à^ÁüÒSÛ¯¿
        /// </summary>
        [Column("kakekin_hokenryo")]
        public long? KakekinHokenryo { get; set; }

        /// <summary>
        /// ÁüÒSÏ§à
        /// </summary>
        [Column("kanyusha_tsumitatekin")]
        public long? KanyushaTsumitatekin { get; set; }

        /// <summary>
        /// Ûàv^tÁÛ¯¿
        /// </summary>
        [Column("fukakin_fukahokenryo")]
        public long? FukakinFukahokenryo { get; set; }

        /// <summary>
        /// ¤Ïà^Û¯à
        /// </summary>
        [Column("kyosaikin_hokenkinto")]
        public long? KyosaikinHokenkinto { get; set; }

        /// <summary>
        /// ¤Ïà^Û¯à_·z
        /// </summary>
        [Column("kyosaikin_hokenkinto_sagaku")]
        public long? KyosaikinHokenkintoSagaku { get; set; }

        /// <summary>
        /// Û¯à
        /// </summary>
        [Column("hokenkin")]
        public long? Hokenkin { get; set; }

        /// <summary>
        /// Áñâ?à
        /// </summary>
        [Column("tokuyaku_hotenkin")]
        public long? TokuyakuHotenkin { get; set; }

        /// <summary>
        /// Áñâ?à_ÉSz
        /// </summary>
        [Column("tokuyaku_hotenkin_kokko")]
        public long? TokuyakuHotenkinKokko { get; set; }

        /// <summary>
        /// Áñâ?à_ÁüÒSz
        /// </summary>
        [Column("tokuyaku_hotenkin_kanyusha")]
        public long? TokuyakuHotenkinKanyusha { get; set; }

        /// <summary>
        /// ¼¥à^ÂÈ¬àÝt^Òàz
        /// </summary>
        [Column("karibarai_kashitsuke_shokan_amt")]
        public long? KaribaraiKashitsukeShokanAmt { get; set; }

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
