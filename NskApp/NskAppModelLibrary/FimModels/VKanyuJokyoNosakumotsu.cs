using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// ‰Á“üó‹µi”_ì•¨‹¤Ïj
    /// </summary>
    [Serializable]
    [Table("v_kanyu_jokyo_nosakumotsu")]
    [PrimaryKey(nameof(NogyoshaId), nameof(KanyuJokyoId))]
    public class VKanyuJokyoNosakumotsu : ModelBase
    {
        /// <summary>
        /// ”_‹ÆÒID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// ‰Á“üó‹µID
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
        /// ˆøó‹¤Ï–Ú“I“™–¼
        /// </summary>
        [Column("hikiuke_kyosai_mokutekito_nm")]
        [StringLength(20)]
        public string HikiukeKyosaiMokutekitoNm { get; set; }

        /// <summary>
        /// ˆøó•û®‹æ•ª
        /// </summary>
        [Column("hikiuke_hoshiki_kbn")]
        [StringLength(2)]
        public string HikiukeHoshikiKbn { get; set; }

        /// <summary>
        /// ˆøó•û®–¼
        /// </summary>
        [Column("hikiuke_hoshiki_nm")]
        [StringLength(20)]
        public string HikiukeHoshikiNm { get; set; }

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
        /// ˆøó‹æ•ª–¼Ì
        /// </summary>
        [Column("hikiuke_kbn_nm")]
        [StringLength(20)]
        public string HikiukeKbnNm { get; set; }

        /// <summary>
        /// ˆøóŠJn”NŒ“ú
        /// </summary>
        [Column("hikiuke_start_date")]
        public DateTime? HikiukeStartDate { get; set; }

        /// <summary>
        /// ˆøóI—¹”NŒ“ú
        /// </summary>
        [Column("hikiuke_end_date")]
        public DateTime? HikiukeEndDate { get; set; }

        /// <summary>
        /// ÀÑ\³”F“ú
        /// </summary>
        [Column("shinkoku_ymd")]
        public DateTime? ShinkokuYmd { get; set; }

        /// <summary>
        /// ‰Á“üó‹µƒXƒe[ƒ^ƒX
        /// </summary>
        [Column("kanyu_jokyo_sts")]
        [StringLength(1)]
        public string KanyuJokyoSts { get; set; }

        /// <summary>
        /// ‹¤Ï‹æ•ª
        /// </summary>
        [Column("kyosai_kbn")]
        [StringLength(2)]
        public string KyosaiKbn { get; set; }

        /// <summary>
        /// ‹¤Ï‹æ•ª–¼
        /// </summary>
        [Column("kyosai_kbn_nm")]
        [StringLength(20)]
        public string KyosaiKbnNm { get; set; }

        /// <summary>
        /// ‹¤Ï–Ú“IEí—Ş“™
        /// </summary>
        [Column("kyosai_mokuteki_shuruito")]
        [StringLength(30)]
        public string KyosaiMokutekiShuruito { get; set; }

        /// <summary>
        /// ˆøó•û®“™
        /// </summary>
        [Column("hikiuke_hoshikito")]
        [StringLength(20)]
        public string HikiukeHoshikito { get; set; }

        /// <summary>
        /// Œo‰cŒ`‘Ô
        /// </summary>
        [Column("keiei_keitai_cd")]
        [StringLength(1)]
        public string KeieiKeitaiCd { get; set; }

        /// <summary>
        /// ˆøó•M”
        /// </summary>
        [Column("hikiuke_fudesu")]
        public Decimal? HikiukeFudesu { get; set; }

        /// <summary>
        /// –{’n–ÊÏ^k’n–ÊÏ^Í”|–ÊÏ
        /// </summary>
        [Column("honchi_kochi_saibai_menseki")]
        public Decimal? HonchiKochiSaibaiMenseki { get; set; }

        /// <summary>
        /// ˆøó–ÊÏ
        /// </summary>
        [Column("hikiuke_menseki")]
        public Decimal? HikiukeMenseki { get; set; }

        /// <summary>
        /// ˆøóû—Ê
        /// </summary>
        [Column("hikiuke_shuryo")]
        public Decimal? HikiukeShuryo { get; set; }

        /// <summary>
        /// ”—{“ª”
        /// </summary>
        [Column("shiyo_tosu")]
        public Decimal? ShiyoTosu { get; set; }

        /// <summary>
        /// ˆøó“ª”
        /// </summary>
        [Column("hikiuke_tosu")]
        public Decimal? HikiukeTosu { get; set; }

        /// <summary>
        /// ÷‘Ì‰Á“ü‹æ•ª
        /// </summary>
        [Column("jutai_kanyu_kbn")]
        public string JutaiKanyuKbn { get; set; }

        /// <summary>
        /// ÷‘Ì‰Á“ü
        /// </summary>
        [Column("jutai_kanyu")]
        public Decimal? JutaiKanyu { get; set; }

        /// <summary>
        /// •‘Ñ{İƒtƒ‰ƒO
        /// </summary>
        [Column("futaishisetsu_flg")]
        [StringLength(1)]
        public string FutaishisetsuFlg { get; set; }

        /// <summary>
        /// {İ“à”_ì•¨ƒtƒ‰ƒO
        /// </summary>
        [Column("shisetsunai_nosakumotsu_flg")]
        [StringLength(1)]
        public string ShisetsunaiNosakumotsuFlg { get; set; }

        /// <summary>
        /// İ’u“”^Š—L“”
        /// </summary>
        [Column("secchi_shoyu_munesu")]
        public Decimal? SecchiShoyuMunesu { get; set; }

        /// <summary>
        /// ˆøó“”
        /// </summary>
        [Column("hikiuke_munesu")]
        public Decimal? HikiukeMunesu { get; set; }

        /// <summary>
        /// Š—L‘ä”
        /// </summary>
        [Column("shoyu_daisu")]
        public Decimal? ShoyuDaisu { get; set; }

        /// <summary>
        /// ˆøó‘ä”
        /// </summary>
        [Column("hikiuke_daisu")]
        public Decimal? HikiukeDaisu { get; set; }

        /// <summary>
        /// ƒ^ƒCƒvA_Œû”
        /// </summary>
        [Column("type_a_kuchisu")]
        public Decimal? TypeAKuchisu { get; set; }

        /// <summary>
        /// ƒ^ƒCƒvB_Œû”
        /// </summary>
        [Column("type_b_kuchisu")]
        public Decimal? TypeBKuchisu { get; set; }

        /// <summary>
        /// ”NŒ‹æ•ª
        /// </summary>
        [Column("nengetsu_kbn")]
        [StringLength(1)]
        public string NengetsuKbn { get; set; }

        /// <summary>
        /// ”NŒ
        /// </summary>
        [Column("nengetsu")]
        public string Nengetsu { get; set; }

        /// <summary>
        /// w”ƒŠJnŒ
        /// </summary>
        [Column("kobai_start_month")]
        [StringLength(7)]
        public string KobaiStartMonth { get; set; }

        /// <summary>
        /// w“ÇÒ‹æ•ª
        /// </summary>
        [Column("kodokusha_kbn")]
        [StringLength(1)]
        public string KodokushaKbn { get; set; }

        /// <summary>
        /// w“ÇÒ
        /// </summary>
        [Column("kodokusha")]
        public string Kodokusha { get; set; }

        /// <summary>
        /// Šî€¶Y‹àŠz^Šî€û“ü‹àŠz
        /// </summary>
        [Column("kijun_seisan_shunyu_amt")]
        public long? KijunSeisanShunyuAmt { get; set; }

        /// <summary>
        /// ‹¤Ï‹àŠz^•â‹àŠz
        /// </summary>
        [Column("kyosai_hosho_amt")]
        public long? KyosaiHoshoAmt { get; set; }

        /// <summary>
        /// ‹¤Ï‰¿Šz
        /// </summary>
        [Column("kyosai_kagaku")]
        public long? KyosaiKagaku { get; set; }

        /// <summary>
        /// ‹¤ÏŠ|‹à
        /// </summary>
        [Column("kyosai_kakekin")]
        public long? KyosaiKakekin { get; set; }

        /// <summary>
        /// •ÛŒ¯—¿
        /// </summary>
        [Column("hokenryo")]
        public long? Hokenryo { get; set; }

        /// <summary>
        /// Ï—§‹à
        /// </summary>
        [Column("tsumitatekin")]
        public long? Tsumitatekin { get; set; }

        /// <summary>
        /// ‘ŒÉ•‰’S_‹¤ÏŠ|‹à
        /// </summary>
        [Column("kokko_kyosai_kakekin")]
        public long? KokkoKyosaiKakekin { get; set; }

        /// <summary>
        /// ‘ŒÉ•‰’S_•ÛŒ¯—¿
        /// </summary>
        [Column("kokko_hokenryo")]
        public long? KokkoHokenryo { get; set; }

        /// <summary>
        /// ‘ŒÉ•‰’S_Ï—§‹à
        /// </summary>
        [Column("kokko_tsumitatekin")]
        public long? KokkoTsumitatekin { get; set; }

        /// <summary>
        /// ”_‰Æ•‰’S_‹¤ÏŠ|‹à
        /// </summary>
        [Column("noka_kyosai_kakekin")]
        public long? NokaKyosaiKakekin { get; set; }

        /// <summary>
        /// ”_‰Æ•‰’S_•ÛŒ¯—¿
        /// </summary>
        [Column("noka_hokenryo")]
        public long? NokaHokenryo { get; set; }

        /// <summary>
        /// ”_‰Æ•‰’S_Ï—§‹à
        /// </summary>
        [Column("noka_tsumitatekin")]
        public long? NokaTsumitatekin { get; set; }

        /// <summary>
        /// •Š‰Û‹à‡Œv^•t‰Á•ÛŒ¯—¿
        /// </summary>
        [Column("fukakin_fukahokenryo")]
        public long? FukakinFukahokenryo { get; set; }

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
