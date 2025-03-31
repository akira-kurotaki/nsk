using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 共済加入資源_見込情報
    /// </summary>
    [Serializable]
    [Table("v_kyosai_kanyu_mikomi")]
    [PrimaryKey(nameof(NogyoshaId), nameof(KyosaiJohoNo))]
    public class VKyosaiKanyuMikomi : ModelBase
    {
        /// <summary>
        /// 農業者ID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 共済情報番号
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("kyosai_joho_no", Order = 2)]
        public int KyosaiJohoNo { get; set; }

        /// <summary>
        /// 共済事業コード
        /// </summary>
        [Column("kyosai_jigyo_cd")]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// 共済目的等コード
        /// </summary>
        [Column("kyosai_mokutekito_cd")]
        [StringLength(2)]
        public string KyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// 個別表示名
        /// </summary>
        [Column("kobetsu_display_nm")]
        [StringLength(10)]
        public string KobetsuDisplayNm { get; set; }

        /// <summary>
        /// 加入意向区分
        /// </summary>
        [Column("kanyu_ikou_kbn")]
        [StringLength(2)]
        public string KanyuIkouKbn { get; set; }

        /// <summary>
        /// 試算提示有無
        /// </summary>
        [Column("shisanteiji_flg")]
        [StringLength(1)]
        public string ShisanteijiFlg { get; set; }

        /// <summary>
        /// 資料送付有無
        /// </summary>
        [Column("shiryo_sofu_flg")]
        [StringLength(1)]
        public string ShiryoSofuFlg { get; set; }

        /// <summary>
        /// 資源量-農作、果樹、畑作（蚕繭除く）-a
        /// </summary>
        [Column("shigen_a")]
        public Decimal? ShigenA { get; set; }

        /// <summary>
        /// 資源量-農作物-主食用米
        /// </summary>
        [Column("shigen_nosaku_shushoku_flg")]
        [StringLength(1)]
        public string ShigenNosakuShushokuFlg { get; set; }

        /// <summary>
        /// 資源量-農作物-飼料用米
        /// </summary>
        [Column("shigen_nosaku_shiryo_flg")]
        [StringLength(1)]
        public string ShigenNosakuShiryoFlg { get; set; }

        /// <summary>
        /// 資源量-農作物-米粉用米
        /// </summary>
        [Column("shigen_nosaku_komeko_flg")]
        [StringLength(1)]
        public string ShigenNosakuKomekoFlg { get; set; }

        /// <summary>
        /// 資源量-農作物-大麦
        /// </summary>
        [Column("shigen_nosaku_oomugi_flg")]
        [StringLength(1)]
        public string ShigenNosakuOomugiFlg { get; set; }

        /// <summary>
        /// 資源量-農作物-小麦
        /// </summary>
        [Column("shigen_nosaku_komugi_flg")]
        [StringLength(1)]
        public string ShigenNosakuKomugiFlg { get; set; }

        /// <summary>
        /// 資源量-畑作物（蚕繭）-箱
        /// </summary>
        [Column("shigen_sanken_hako")]
        public Decimal? ShigenSankenHako { get; set; }

        /// <summary>
        /// 資源量-家畜（牛）-頭
        /// </summary>
        [Column("shigen_ushi_to")]
        public Decimal? ShigenUshiTo { get; set; }

        /// <summary>
        /// 資源量-家畜（牛）-乳用牛（授乳牛）
        /// </summary>
        [Column("shigen_ushi_nyu_junyu")]
        public Decimal? ShigenUshiNyuJunyu { get; set; }

        /// <summary>
        /// 資源量-家畜（牛）-乳用牛（育成乳牛）
        /// </summary>
        [Column("shigen_ushi_nyu_ikuseinyu")]
        public Decimal? ShigenUshiNyuIkuseinyu { get; set; }

        /// <summary>
        /// 資源量-家畜（牛）-肉用牛（繁殖用雄牛）
        /// </summary>
        [Column("shigen_ushi_niku_hanshoku_osu")]
        public Decimal? ShigenUshiNikuHanshokuOsu { get; set; }

        /// <summary>
        /// 資源量-家畜（牛）-肉用牛（育成肥育牛）
        /// </summary>
        [Column("shigen_ushi_niku_ikuseihiiku")]
        public Decimal? ShigenUshiNikuIkuseihiiku { get; set; }

        /// <summary>
        /// 資源量-家畜（牛）-乳用種種雄牛（育成乳牛）
        /// </summary>
        [Column("shigen_ushi_nyu_taneosu_ikuseinyu")]
        public Decimal? ShigenUshiNyuTaneosuIkuseinyu { get; set; }

        /// <summary>
        /// 資源量-家畜（牛）-肉用種種雄牛（育成乳牛）
        /// </summary>
        [Column("shigen_ushi_niku_taneosu_ikuseinyu")]
        public Decimal? ShigenUshiNikuTaneosuIkuseinyu { get; set; }

        /// <summary>
        /// 資源量-家畜（豚）-頭
        /// </summary>
        [Column("shigen_buta_to")]
        public Decimal? ShigenButaTo { get; set; }

        /// <summary>
        /// 資源量-家畜（豚）-種豚
        /// </summary>
        [Column("shigen_buta_tane")]
        public Decimal? ShigenButaTane { get; set; }

        /// <summary>
        /// 資源量-家畜（豚）-肉豚
        /// </summary>
        [Column("shigen_buta_niku")]
        public Decimal? ShigenButaNiku { get; set; }

        /// <summary>
        /// 資源量-家畜（馬）-頭
        /// </summary>
        [Column("shigen_uma_to")]
        public Decimal? ShigenUmaTo { get; set; }

        /// <summary>
        /// 資源量-家畜（馬）-繁殖用雌馬
        /// </summary>
        [Column("shigen_uma_hanshoku_mesu")]
        public Decimal? ShigenUmaHanshokuMesu { get; set; }

        /// <summary>
        /// 資源量-家畜（馬）-育成・肥育馬
        /// </summary>
        [Column("shigen_uma_ikusei_hiiku")]
        public Decimal? ShigenUmaIkuseiHiiku { get; set; }

        /// <summary>
        /// 資源量-家畜（馬）-種雄馬
        /// </summary>
        [Column("shigen_uma_taneosu")]
        public Decimal? ShigenUmaTaneosu { get; set; }

        /// <summary>
        /// 資源量-園芸施設共済-棟
        /// </summary>
        [Column("shigen_engei_to")]
        public Decimal? ShigenEngeiTo { get; set; }

        /// <summary>
        /// 資源量-園芸施設共済-農作物名
        /// </summary>
        [Column("shigen_engei_sakumotsu_nm")]
        [StringLength(30)]
        public string ShigenEngeiSakumotsuNm { get; set; }

        /// <summary>
        /// 資源量-園芸施設共済-施設詳細
        /// </summary>
        [Column("shigen_engei_shisetsu_shosai")]
        [StringLength(30)]
        public string ShigenEngeiShisetsuShosai { get; set; }

        /// <summary>
        /// 資源量-任意（建物）-棟
        /// </summary>
        [Column("shigen_tatemono_to")]
        public Decimal? ShigenTatemonoTo { get; set; }

        /// <summary>
        /// 資源量-任意（建物）-住宅
        /// </summary>
        [Column("shigen_tatemono_jutaku")]
        public Decimal? ShigenTatemonoJutaku { get; set; }

        /// <summary>
        /// 資源量-任意（建物）-農作業場
        /// </summary>
        [Column("shigen_tatemono_nosagyojo")]
        public Decimal? ShigenTatemonoNosagyojo { get; set; }

        /// <summary>
        /// 資源量-任意（建物）-納屋
        /// </summary>
        [Column("shigen_tatemono_naya")]
        public Decimal? ShigenTatemonoNaya { get; set; }

        /// <summary>
        /// 資源量-任意（建物）-その他
        /// </summary>
        [Column("shigen_tatemono_sonota")]
        public Decimal? ShigenTatemonoSonota { get; set; }

        /// <summary>
        /// 資源量-任意（農機具）-台
        /// </summary>
        [Column("shigen_nokigu_dai")]
        public Decimal? ShigenNokiguDai { get; set; }

        /// <summary>
        /// 資源量-任意（農機具）-トラクター
        /// </summary>
        [Column("shigen_nokigu_tractor")]
        public Decimal? ShigenNokiguTractor { get; set; }

        /// <summary>
        /// 資源量-任意（農機具）-ロータリー
        /// </summary>
        [Column("shigen_nokigu_rotary")]
        public Decimal? ShigenNokiguRotary { get; set; }

        /// <summary>
        /// 資源量-任意（農機具）-田植機
        /// </summary>
        [Column("shigen_nokigu_taueki")]
        public Decimal? ShigenNokiguTaueki { get; set; }

        /// <summary>
        /// 資源量-任意（農機具）-コンバイン
        /// </summary>
        [Column("shigen_nokigu_combine")]
        public Decimal? ShigenNokiguCombine { get; set; }

        /// <summary>
        /// 資源量-任意（農機具）-乾燥機
        /// </summary>
        [Column("shigen_nokigu_kansoki")]
        public Decimal? ShigenNokiguKansoki { get; set; }

        /// <summary>
        /// 資源量-任意（農機具）-その他
        /// </summary>
        [Column("shigen_nokigu_sonota")]
        public Decimal? ShigenNokiguSonota { get; set; }

        /// <summary>
        /// 資源量-その他-aなど
        /// </summary>
        [Column("shigen_sonota_a_etc")]
        public Decimal? ShigenSonotaAEtc { get; set; }

        /// <summary>
        /// 資源量-その他-名称
        /// </summary>
        [Column("shigen_sonota_meisho")]
        [StringLength(30)]
        public string ShigenSonotaMeisho { get; set; }

        /// <summary>
        /// 資源量-保管中農産物-加入
        /// </summary>
        [Column("shigen_hokan_kanyu_flg")]
        [StringLength(1)]
        public string ShigenHokanKanyuFlg { get; set; }

        /// <summary>
        /// 資源量-保管中農産物-Aタイプ
        /// </summary>
        [Column("shigen_hokan_a")]
        public Decimal? ShigenHokanA { get; set; }

        /// <summary>
        /// 資源量-保管中農産物-Bタイプ
        /// </summary>
        [Column("shigen_hokan_b")]
        public Decimal? ShigenHokanB { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ1
        /// </summary>
        [Column("kanyumuishi_riyu_flg1")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg1 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ2
        /// </summary>
        [Column("kanyumuishi_riyu_flg2")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg2 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ3
        /// </summary>
        [Column("kanyumuishi_riyu_flg3")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg3 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ4
        /// </summary>
        [Column("kanyumuishi_riyu_flg4")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg4 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ5
        /// </summary>
        [Column("kanyumuishi_riyu_flg5")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg5 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ6
        /// </summary>
        [Column("kanyumuishi_riyu_flg6")]
        [StringLength(1)]
        public string KanyumuishiRiyuFlg6 { get; set; }

        /// <summary>
        /// 加入無意思理由フラグ（その他）
        /// </summary>
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
