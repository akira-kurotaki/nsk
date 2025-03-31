using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 収入保険資源_見込情報
    /// </summary>
    [Serializable]
    [Table("v_shunyuhoken_mikomi")]
    public class VShunyuhokenMikomi : ModelBase
    {
        /// <summary>
        /// 農業者ID (FK)
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 生産販売品目コード1
        /// </summary>
        [Column("shh_shurui_cd1")]
        [StringLength(3)]
        public string ShhShuruiCd1 { get; set; }

        /// <summary>
        /// 生産販売品目名1（自由入力）
        /// </summary>
        [Column("shh_fnm1")]
        [StringLength(30)]
        public string ShhFnm1 { get; set; }

        /// <summary>
        /// 生産販売品目コード2
        /// </summary>
        [Column("shh_shurui_cd2")]
        [StringLength(3)]
        public string ShhShuruiCd2 { get; set; }

        /// <summary>
        /// 生産販売品目名2（自由入力）
        /// </summary>
        [Column("shh_fnm2")]
        [StringLength(30)]
        public string ShhFnm2 { get; set; }

        /// <summary>
        /// 生産販売品目コード3
        /// </summary>
        [Column("shh_shurui_cd3")]
        [StringLength(3)]
        public string ShhShuruiCd3 { get; set; }

        /// <summary>
        /// 生産販売品目名3（自由入力）
        /// </summary>
        [Column("shh_fnm3")]
        [StringLength(30)]
        public string ShhFnm3 { get; set; }

        /// <summary>
        /// 生産販売品目名（その他自由入力）
        /// </summary>
        [Column("shh_sonota_fnm")]
        [StringLength(30)]
        public string ShhSonotaFnm { get; set; }

        /// <summary>
        /// 類似制度加入状況(農作物共済）
        /// </summary>
        [Required]
        [Column("ruiji_nosaku_flg")]
        [StringLength(1)]
        public string RuijiNosakuFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(農作物_水稲）
        /// </summary>
        [Required]
        [Column("ruiji_nosaku_suito_flg")]
        [StringLength(1)]
        public string RuijiNosakuSuitoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(農作物_麦）
        /// </summary>
        [Required]
        [Column("ruiji_nosaku_mugi_flg")]
        [StringLength(1)]
        public string RuijiNosakuMugiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(農作物_陸稲）
        /// </summary>
        [Required]
        [Column("ruiji_nosaku_rikuto_flg")]
        [StringLength(1)]
        public string RuijiNosakuRikutoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜共済）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_flg")]
        [StringLength(1)]
        public string RuijiKachikuFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_牛）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_ushi_flg")]
        [StringLength(1)]
        public string RuijiKachikuUshiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_牛_死廃）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_ushi_shihai_flg")]
        [StringLength(1)]
        public string RuijiKachikuUshiShihaiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_牛_病傷）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_ushi_byosho_flg")]
        [StringLength(1)]
        public string RuijiKachikuUshiByoshoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_馬）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_uma_flg")]
        [StringLength(1)]
        public string RuijiKachikuUmaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_馬_死廃）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_uma_shihai_flg")]
        [StringLength(1)]
        public string RuijiKachikuUmaShihaiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_馬_病傷）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_uma_byosho_flg")]
        [StringLength(1)]
        public string RuijiKachikuUmaByoshoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_豚）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_buta_flg")]
        [StringLength(1)]
        public string RuijiKachikuButaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_豚_死廃）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_buta_shihai_flg")]
        [StringLength(1)]
        public string RuijiKachikuButaShihaiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(家畜_豚_病傷）
        /// </summary>
        [Required]
        [Column("ruiji_kachiku_buta_byosho_flg")]
        [StringLength(1)]
        public string RuijiKachikuButaByoshoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹共済）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_flg")]
        [StringLength(1)]
        public string RuijiKajuFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_うんしゅうみかん）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_unshumikan_flg")]
        [StringLength(1)]
        public string RuijiKajuUnshumikanFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_なつみかん）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_natsumikan_flg")]
        [StringLength(1)]
        public string RuijiKajuNatsumikanFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_いよかん）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_iyokan_flg")]
        [StringLength(1)]
        public string RuijiKajuIyokanFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_かんきつ）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_kankitsu_flg")]
        [StringLength(1)]
        public string RuijiKajuKankitsuFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_りんご）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_ringo_flg")]
        [StringLength(1)]
        public string RuijiKajuRingoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_ぶどう）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_budo_flg")]
        [StringLength(1)]
        public string RuijiKajuBudoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_なし）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_nashi_flg")]
        [StringLength(1)]
        public string RuijiKajuNashiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_もも）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_momo_flg")]
        [StringLength(1)]
        public string RuijiKajuMomoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_おうとう）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_oto_flg")]
        [StringLength(1)]
        public string RuijiKajuOtoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_びわ）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_biwa_flg")]
        [StringLength(1)]
        public string RuijiKajuBiwaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_かき）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_kaki_flg")]
        [StringLength(1)]
        public string RuijiKajuKakiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_くり）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_kuri_flg")]
        [StringLength(1)]
        public string RuijiKajuKuriFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_うめ）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_ume_flg")]
        [StringLength(1)]
        public string RuijiKajuUmeFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_すもも）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_sumomo_flg")]
        [StringLength(1)]
        public string RuijiKajuSumomoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_キウイ）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_kiwi_flg")]
        [StringLength(1)]
        public string RuijiKajuKiwiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_パイン）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_pine_flg")]
        [StringLength(1)]
        public string RuijiKajuPineFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_その他）
        /// </summary>
        [Required]
        [Column("ruiji_kaju_sonota_flg")]
        [StringLength(1)]
        public string RuijiKajuSonotaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(果樹_その他_自由入力）
        /// </summary>
        [Column("ruiji_kaju_sonota_fnm")]
        [StringLength(30)]
        public string RuijiKajuSonotaFnm { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物共済）
        /// </summary>
        [Required]
        [Column("ruiji_hata_flg")]
        [StringLength(1)]
        public string RuijiHataFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_ばれいしょ）
        /// </summary>
        [Required]
        [Column("ruiji_hata_bareisho_flg")]
        [StringLength(1)]
        public string RuijiHataBareishoFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_大豆）
        /// </summary>
        [Required]
        [Column("ruiji_hata_daizu_flg")]
        [StringLength(1)]
        public string RuijiHataDaizuFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_小豆）
        /// </summary>
        [Required]
        [Column("ruiji_hata_azuki_flg")]
        [StringLength(1)]
        public string RuijiHataAzukiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_いんげん）
        /// </summary>
        [Required]
        [Column("ruiji_hata_ingen_flg")]
        [StringLength(1)]
        public string RuijiHataIngenFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_てん菜）
        /// </summary>
        [Required]
        [Column("ruiji_hata_tensai_flg")]
        [StringLength(1)]
        public string RuijiHataTensaiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_さとうきび）
        /// </summary>
        [Required]
        [Column("ruiji_hata_satokibi_flg")]
        [StringLength(1)]
        public string RuijiHataSatokibiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_茶）
        /// </summary>
        [Required]
        [Column("ruiji_hata_cha_flg")]
        [StringLength(1)]
        public string RuijiHataChaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_そば）
        /// </summary>
        [Required]
        [Column("ruiji_hata_soba_flg")]
        [StringLength(1)]
        public string RuijiHataSobaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_スイートコーン）
        /// </summary>
        [Required]
        [Column("ruiji_hata_sweetcorn_flg")]
        [StringLength(1)]
        public string RuijiHataSweetcornFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_たまねぎ）
        /// </summary>
        [Required]
        [Column("ruiji_hata_tamanegi_flg")]
        [StringLength(1)]
        public string RuijiHataTamanegiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_かぼちゃ）
        /// </summary>
        [Required]
        [Column("ruiji_hata_kabocha_flg")]
        [StringLength(1)]
        public string RuijiHataKabochaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_ホップ）
        /// </summary>
        [Required]
        [Column("ruiji_hata_hop_flg")]
        [StringLength(1)]
        public string RuijiHataHopFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_蚕繭）
        /// </summary>
        [Required]
        [Column("ruiji_hata_sanken_flg")]
        [StringLength(1)]
        public string RuijiHataSankenFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_その他）
        /// </summary>
        [Required]
        [Column("ruiji_hata_sonota_flg")]
        [StringLength(1)]
        public string RuijiHataSonotaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(畑作物_その他_自由入力）
        /// </summary>
        [Column("ruiji_hata_sonota_fnm")]
        [StringLength(30)]
        public string RuijiHataSonotaFnm { get; set; }

        /// <summary>
        /// 類似制度加入状況(園芸施設共済）
        /// </summary>
        [Required]
        [Column("ruiji_engei_flg")]
        [StringLength(1)]
        public string RuijiEngeiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(園芸_自由入力）
        /// </summary>
        [Column("ruiji_engei_fnm")]
        [StringLength(30)]
        public string RuijiEngeiFnm { get; set; }

        /// <summary>
        /// 類似制度加入状況(ナラシ対策）
        /// </summary>
        [Required]
        [Column("ruiji_narashi_flg")]
        [StringLength(1)]
        public string RuijiNarashiFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(野菜価格安定対策）
        /// </summary>
        [Required]
        [Column("ruiji_yasaikakaku_flg")]
        [StringLength(1)]
        public string RuijiYasaikakakuFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(加工原料乳経営安定対策）
        /// </summary>
        [Required]
        [Column("ruiji_kakonyu_flg")]
        [StringLength(1)]
        public string RuijiKakonyuFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(いぐさ・畳表）
        /// </summary>
        [Required]
        [Column("ruiji_igusa_flg")]
        [StringLength(1)]
        public string RuijiIgusaFlg { get; set; }

        /// <summary>
        /// 類似制度加入状況(加入なし）
        /// </summary>
        [Required]
        [Column("ruiji_kanyunashi_flg")]
        [StringLength(1)]
        public string RuijiKanyunashiFlg { get; set; }

        /// <summary>
        /// 青色申告フラグ
        /// </summary>
        [Required]
        [Column("zeishinkoku_aoiro_flg")]
        [StringLength(1)]
        public string ZeishinkokuAoiroFlg { get; set; }

        /// <summary>
        /// 複式簿記フラグ
        /// </summary>
        [Required]
        [Column("fukushiki_flg")]
        [StringLength(1)]
        public string FukushikiFlg { get; set; }

        /// <summary>
        /// 簡易簿記フラグ
        /// </summary>
        [Required]
        [Column("kani_flg")]
        [StringLength(1)]
        public string KaniFlg { get; set; }

        /// <summary>
        /// 現金主義フラグ
        /// </summary>
        [Required]
        [Column("genkinshugi_flg")]
        [StringLength(1)]
        public string GenkinshugiFlg { get; set; }

        /// <summary>
        /// 白色申告フラグ
        /// </summary>
        [Required]
        [Column("zeishinkoku_shiroiro_flg")]
        [StringLength(1)]
        public string ZeishinkokuShiroiroFlg { get; set; }

        /// <summary>
        /// 青色申告予定フラグ
        /// </summary>
        [Required]
        [Column("aoiro_shinkoku_yotei_flg")]
        [StringLength(1)]
        public string AoiroShinkokuYoteiFlg { get; set; }

        /// <summary>
        /// 青色申告予定なしフラグ
        /// </summary>
        [Required]
        [Column("aoiro_shinkoku_yoteinashi_flg")]
        [StringLength(1)]
        public string AoiroShinkokuYoteinashiFlg { get; set; }

        /// <summary>
        /// 認定農業者フラグ
        /// </summary>
        [Required]
        [Column("nintei_nogyo_flg")]
        [StringLength(1)]
        public string NinteiNogyoFlg { get; set; }

        /// <summary>
        /// 認定新規就農者フラグ
        /// </summary>
        [Required]
        [Column("nintei_shuno_flg")]
        [StringLength(1)]
        public string NinteiShunoFlg { get; set; }

        /// <summary>
        /// 農業次世代人材投資資金交付対象者フラグ
        /// </summary>
        [Required]
        [Column("jinzai_toshishikin_taisho_flg")]
        [StringLength(1)]
        public string JinzaiToshishikinTaishoFlg { get; set; }

        /// <summary>
        /// 加工品の生産販売フラグ
        /// </summary>
        [Required]
        [Column("kakohin_seisanhanbai_flg")]
        [StringLength(1)]
        public string KakohinSeisanhanbaiFlg { get; set; }

        /// <summary>
        /// リスク発生時対応区分
        /// </summary>
        [Column("risk_taio_kbn")]
        [StringLength(1)]
        public string RiskTaioKbn { get; set; }

        /// <summary>
        /// 意見
        /// </summary>
        [Column("iken")]
        [StringLength(500)]
        public string Iken { get; set; }

        /// <summary>
        /// 試算提示有無
        /// </summary>
        [Required]
        [Column("shisanteiji_flg")]
        [StringLength(1)]
        public string ShisanteijiFlg { get; set; }

        /// <summary>
        /// 資料送付有無
        /// </summary>
        [Required]
        [Column("shiryo_sofu_flg")]
        [StringLength(1)]
        public string ShiryoSofuFlg { get; set; }

        /// <summary>
        /// 基準収入
        /// </summary>
        [Column("kijun_shunyu")]
        public long? KijunShunyu { get; set; }

        /// <summary>
        /// 加入申請進捗状況_税申告提出あり
        /// </summary>
        [Required]
        [Column("shinchoku_zeishinkoku_flg")]
        [StringLength(1)]
        public string ShinchokuZeishinkokuFlg { get; set; }

        /// <summary>
        /// 加入申請進捗状況_申請書類提出あり
        /// </summary>
        [Required]
        [Column("shinchoku_shinseishorui_flg")]
        [StringLength(1)]
        public string ShinchokuShinseishoruiFlg { get; set; }

        /// <summary>
        /// 全国連への提出期限
        /// </summary>
        [Column("zenkokuren_kigen")]
        public DateTime? ZenkokurenKigen { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ1
        /// </summary>
        [Required]
        [Column("kanyumuishi_riyu_flg1")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg1 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ2
        /// </summary>
        [Required]
        [Column("kanyumuishi_riyu_flg2")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg2 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ3
        /// </summary>
        [Required]
        [Column("kanyumuishi_riyu_flg3")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg3 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ4
        /// </summary>
        [Required]
        [Column("kanyumuishi_riyu_flg4")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg4 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ5
        /// </summary>
        [Required]
        [Column("kanyumuishi_riyu_flg5")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg5 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ6
        /// </summary>
        [Required]
        [Column("kanyumuishi_riyu_flg6")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg6 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ（その他）
        /// </summary>
        [Required]
        [Column("kanyumuishi_riyu_sonota_flg")]
        [StringLength(1)]
        public string KanyumuishiRiyuSonotaFlg { get; set; }

        /// <summary>
        /// 加入無意思理由（その他_自由入力）
        /// </summary>
        [Column("kanyumuishi_riyu_sonota_fnm")]
        [StringLength(30)]
        public string KanyumuishiRiyuSonotaFnm { get; set; }

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
