using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// ÁüóµiûüÛ¯j
    /// </summary>
    [Serializable]
    [Table("v_kanyu_jokyo_shunyuhoken")]
    [PrimaryKey(nameof(NogyoshaId), nameof(KanyuJokyoId))]
    public class VKanyuJokyoShunyuhoken : ModelBase
    {
        /// <summary>
        /// _ÆÒID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// ÁüóµID
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
        /// øó¤ÏÚI¼
        /// </summary>
        [Column("hikiuke_kyosai_mokutekito_nm")]
        [StringLength(20)]
        public string HikiukeKyosaiMokutekitoNm { get; set; }

        /// <summary>
        /// øóû®æª
        /// </summary>
        [Column("hikiuke_hoshiki_kbn")]
        [StringLength(2)]
        public string HikiukeHoshikiKbn { get; set; }

        /// <summary>
        /// øóû®¼
        /// </summary>
        [Column("hikiuke_hoshiki_nm")]
        [StringLength(20)]
        public string HikiukeHoshikiNm { get; set; }

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
        /// øóæª¼Ì
        /// </summary>
        [Column("hikiuke_kbn_nm")]
        [StringLength(20)]
        public string HikiukeKbnNm { get; set; }

        /// <summary>
        /// øóJnNú
        /// </summary>
        [Column("hikiuke_start_date")]
        public DateTime? HikiukeStartDate { get; set; }

        /// <summary>
        /// øóI¹Nú
        /// </summary>
        [Column("hikiuke_end_date")]
        public DateTime? HikiukeEndDate { get; set; }

        /// <summary>
        /// ÀÑ\³Fú
        /// </summary>
        [Column("shinkoku_ymd")]
        public DateTime? ShinkokuYmd { get; set; }

        /// <summary>
        /// ÁüóµXe[^X
        /// </summary>
        [Column("kanyu_jokyo_sts")]
        [StringLength(1)]
        public string KanyuJokyoSts { get; set; }

        /// <summary>
        /// ¤Ïæª
        /// </summary>
        [Column("kyosai_kbn")]
        [StringLength(2)]
        public string KyosaiKbn { get; set; }

        /// <summary>
        /// ¤Ïæª¼
        /// </summary>
        [Column("kyosai_kbn_nm")]
        [StringLength(20)]
        public string KyosaiKbnNm { get; set; }

        /// <summary>
        /// ¤ÏÚIEíÞ
        /// </summary>
        [Column("kyosai_mokuteki_shuruito")]
        [StringLength(30)]
        public string KyosaiMokutekiShuruito { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Column("hikiuke_hoshikito")]
        [StringLength(20)]
        public string HikiukeHoshikito { get; set; }

        /// <summary>
        /// oc`Ô
        /// </summary>
        [Column("keiei_keitai_cd")]
        [StringLength(1)]
        public string KeieiKeitaiCd { get; set; }

        /// <summary>
        /// øóM
        /// </summary>
        [Column("hikiuke_fudesu")]
        public Decimal? HikiukeFudesu { get; set; }

        /// <summary>
        /// {nÊÏ^knÊÏ^Í|ÊÏ
        /// </summary>
        [Column("honchi_kochi_saibai_menseki")]
        public Decimal? HonchiKochiSaibaiMenseki { get; set; }

        /// <summary>
        /// øóÊÏ
        /// </summary>
        [Column("hikiuke_menseki")]
        public Decimal? HikiukeMenseki { get; set; }

        /// <summary>
        /// øóûÊ
        /// </summary>
        [Column("hikiuke_shuryo")]
        public Decimal? HikiukeShuryo { get; set; }

        /// <summary>
        /// {ª
        /// </summary>
        [Column("shiyo_tosu")]
        public Decimal? ShiyoTosu { get; set; }

        /// <summary>
        /// øóª
        /// </summary>
        [Column("hikiuke_tosu")]
        public Decimal? HikiukeTosu { get; set; }

        /// <summary>
        /// ÷ÌÁüæª
        /// </summary>
        [Column("jutai_kanyu_kbn")]
        public string JutaiKanyuKbn { get; set; }

        /// <summary>
        /// ÷ÌÁü
        /// </summary>
        [Column("jutai_kanyu")]
        public Decimal? JutaiKanyu { get; set; }

        /// <summary>
        /// Ñ{ÝtO
        /// </summary>
        [Column("futaishisetsu_flg")]
        [StringLength(1)]
        public string FutaishisetsuFlg { get; set; }

        /// <summary>
        /// {Ýà_ì¨tO
        /// </summary>
        [Column("shisetsunai_nosakumotsu_flg")]
        [StringLength(1)]
        public string ShisetsunaiNosakumotsuFlg { get; set; }

        /// <summary>
        /// Ýu^L
        /// </summary>
        [Column("secchi_shoyu_munesu")]
        public Decimal? SecchiShoyuMunesu { get; set; }

        /// <summary>
        /// øó
        /// </summary>
        [Column("hikiuke_munesu")]
        public Decimal? HikiukeMunesu { get; set; }

        /// <summary>
        /// Lä
        /// </summary>
        [Column("shoyu_daisu")]
        public Decimal? ShoyuDaisu { get; set; }

        /// <summary>
        /// øóä
        /// </summary>
        [Column("hikiuke_daisu")]
        public Decimal? HikiukeDaisu { get; set; }

        /// <summary>
        /// ^CvA_û
        /// </summary>
        [Column("type_a_kuchisu")]
        public Decimal? TypeAKuchisu { get; set; }

        /// <summary>
        /// ^CvB_û
        /// </summary>
        [Column("type_b_kuchisu")]
        public Decimal? TypeBKuchisu { get; set; }

        /// <summary>
        /// Næª
        /// </summary>
        [Column("nengetsu_kbn")]
        [StringLength(1)]
        public string NengetsuKbn { get; set; }

        /// <summary>
        /// N
        /// </summary>
        [Column("nengetsu")]
        public string Nengetsu { get; set; }

        /// <summary>
        /// wJn
        /// </summary>
        [Column("kobai_start_month")]
        [StringLength(7)]
        public string KobaiStartMonth { get; set; }

        /// <summary>
        /// wÇÒæª
        /// </summary>
        [Column("kodokusha_kbn")]
        [StringLength(1)]
        public string KodokushaKbn { get; set; }

        /// <summary>
        /// wÇÒ
        /// </summary>
        [Column("kodokusha")]
        public string Kodokusha { get; set; }

        /// <summary>
        /// î¶Yàz^îûüàz
        /// </summary>
        [Column("kijun_seisan_shunyu_amt")]
        public long? KijunSeisanShunyuAmt { get; set; }

        /// <summary>
        /// ¤Ïàz^âàz
        /// </summary>
        [Column("kyosai_hosho_amt")]
        public long? KyosaiHoshoAmt { get; set; }

        /// <summary>
        /// ¤Ï¿z
        /// </summary>
        [Column("kyosai_kagaku")]
        public long? KyosaiKagaku { get; set; }

        /// <summary>
        /// ¤Ï|à
        /// </summary>
        [Column("kyosai_kakekin")]
        public long? KyosaiKakekin { get; set; }

        /// <summary>
        /// Û¯¿
        /// </summary>
        [Column("hokenryo")]
        public long? Hokenryo { get; set; }

        /// <summary>
        /// Ï§à
        /// </summary>
        [Column("tsumitatekin")]
        public long? Tsumitatekin { get; set; }

        /// <summary>
        /// ÉS_¤Ï|à
        /// </summary>
        [Column("kokko_kyosai_kakekin")]
        public long? KokkoKyosaiKakekin { get; set; }

        /// <summary>
        /// ÉS_Û¯¿
        /// </summary>
        [Column("kokko_hokenryo")]
        public long? KokkoHokenryo { get; set; }

        /// <summary>
        /// ÉS_Ï§à
        /// </summary>
        [Column("kokko_tsumitatekin")]
        public long? KokkoTsumitatekin { get; set; }

        /// <summary>
        /// _ÆS_¤Ï|à
        /// </summary>
        [Column("noka_kyosai_kakekin")]
        public long? NokaKyosaiKakekin { get; set; }

        /// <summary>
        /// _ÆS_Û¯¿
        /// </summary>
        [Column("noka_hokenryo")]
        public long? NokaHokenryo { get; set; }

        /// <summary>
        /// _ÆS_Ï§à
        /// </summary>
        [Column("noka_tsumitatekin")]
        public long? NokaTsumitatekin { get; set; }

        /// <summary>
        /// Ûàv^tÁÛ¯¿
        /// </summary>
        [Column("fukakin_fukahokenryo")]
        public long? FukakinFukahokenryo { get; set; }

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
