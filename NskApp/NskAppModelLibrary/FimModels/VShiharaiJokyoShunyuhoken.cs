using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// Š|‹à“™’¥û‹¤Ï‹àx•¥ó‹µiû“ü•ÛŒ¯j
    /// </summary>
    [Serializable]
    [Table("v_shiharai_jokyo_shunyuhoken")]
    [PrimaryKey(nameof(NogyoshaId), nameof(KanyuJokyoId), nameof(ChoshuShiharaiKbn), nameof(ChoshuShiharaiDate))]
    public class VShiharaiJokyoShunyuhoken : ModelBase
    {
        /// <summary>
        /// ”_‹ÆÒID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// ‰Á“üó‹µID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("kanyu_jokyo_id", Order = 2)]
        public int KanyuJokyoId { get; set; }

        /// <summary>
        /// ‹¤Ï–‹ÆƒR[ƒh
        /// </summary>
        [Column("kyosai_jigyo_cd")]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// ˆøó‹¤Ï–Ú“I“™ƒR[ƒh
        /// </summary>
        [Column("hikiuke_kyosai_mokutekito_cd")]
        [StringLength(15)]
        public string HikiukeKyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// ˆøó•û®‹æ•ª
        /// </summary>
        [Column("hikiuke_hoshiki_kbn")]
        [StringLength(2)]
        public string HikiukeHoshikiKbn { get; set; }

        /// <summary>
        /// ”NY^”N“x
        /// </summary>
        [Column("nensan_nendo")]
        public short? NensanNendo { get; set; }

        /// <summary>
        /// ˆøó‹æ•ª
        /// </summary>
        [Column("hikiuke_kbn")]
        [StringLength(3)]
        public string HikiukeKbn { get; set; }

        /// <summary>
        /// ’¥ûx•¥‹æ•ª
        /// </summary>
        [Required]
        [Column("choshu_shiharai_kbn", Order = 3)]
        [StringLength(1)]
        public string ChoshuShiharaiKbn { get; set; }

        /// <summary>
        /// ’¥ûx•¥”NŒ“ú
        /// </summary>
        [Required]
        [Column("choshu_shiharai_date", Order = 4)]
        public DateTime ChoshuShiharaiDate { get; set; }

        /// <summary>
        /// ‹¤ÏŠ|‹à“™^•ÛŒ¯—¿“™
        /// </summary>
        [Column("kyosai_kakekinto_hokenryo")]
        public long? KyosaiKakekintoHokenryo { get; set; }

        /// <summary>
        /// ‹¤ÏŠ|‹à“™^•ÛŒ¯—¿“™_‚¤‚¿”[‰®“™
        /// </summary>
        [Column("kyosai_kakekinto_hokenryo_nayato")]
        public long? KyosaiKakekintoHokenryoNayato { get; set; }

        /// <summary>
        /// ‘g‡ˆõ“™•‰’SŠ|‹à^‰Á“üÒ•‰’S•ÛŒ¯—¿
        /// </summary>
        [Column("kakekin_hokenryo")]
        public long? KakekinHokenryo { get; set; }

        /// <summary>
        /// ‰Á“üÒ•‰’SÏ—§‹à
        /// </summary>
        [Column("kanyusha_tsumitatekin")]
        public long? KanyushaTsumitatekin { get; set; }

        /// <summary>
        /// •Š‰Û‹à‡Œv^•t‰Á•ÛŒ¯—¿
        /// </summary>
        [Column("fukakin_fukahokenryo")]
        public long? FukakinFukahokenryo { get; set; }

        /// <summary>
        /// ‹¤Ï‹à^•ÛŒ¯‹à“™
        /// </summary>
        [Column("kyosaikin_hokenkinto")]
        public long? KyosaikinHokenkinto { get; set; }

        /// <summary>
        /// ‹¤Ï‹à^•ÛŒ¯‹à“™_·Šz
        /// </summary>
        [Column("kyosaikin_hokenkinto_sagaku")]
        public long? KyosaikinHokenkintoSagaku { get; set; }

        /// <summary>
        /// •ÛŒ¯‹à
        /// </summary>
        [Column("hokenkin")]
        public long? Hokenkin { get; set; }

        /// <summary>
        /// “Á–ñ•â?‹à
        /// </summary>
        [Column("tokuyaku_hotenkin")]
        public long? TokuyakuHotenkin { get; set; }

        /// <summary>
        /// “Á–ñ•â?‹à_‘ŒÉ•‰’SŠz
        /// </summary>
        [Column("tokuyaku_hotenkin_kokko")]
        public long? TokuyakuHotenkinKokko { get; set; }

        /// <summary>
        /// “Á–ñ•â?‹à_‰Á“üÒ•‰’SŠz
        /// </summary>
        [Column("tokuyaku_hotenkin_kanyusha")]
        public long? TokuyakuHotenkinKanyusha { get; set; }

        /// <summary>
        /// ‰¼•¥‹à^‚Â‚È‚¬‘‹à‘İ•t^ŠÒ‹àŠz
        /// </summary>
        [Column("karibarai_kashitsuke_shokan_amt")]
        public long? KaribaraiKashitsukeShokanAmt { get; set; }

        /// <summary>
        /// “o˜^ƒ†[ƒUID
        /// </summary>
        [Column("insert_user_id")]
        [StringLength(11)]
        public string InsertUserId { get; set; }

        /// <summary>
        /// “o˜^“ú
        /// </summary>
        [Column("insert_date")]
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// XVƒ†[ƒUID
        /// </summary>
        [Column("update_user_id")]
        [StringLength(11)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// XV“ú
        /// </summary>
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
