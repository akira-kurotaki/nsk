using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// QA面談記録
    /// </summary>
    [Serializable]
    [Table("v_qa_mendan_kiroku")]
    [PrimaryKey(nameof(NogyoshaId), nameof(UketsukeNo))]
    public class VQaMendanKiroku : ModelBase
    {
        /// <summary>
        /// 農業者ID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 受付番号
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("uketsuke_no", Order = 2)]
        public int UketsukeNo { get; set; }

        /// <summary>
        /// 受付事項コード
        /// </summary>
        [Column("uketsuke_jiko_cd")]
        [StringLength(2)]
        public string UketsukeJikoCd { get; set; }

        /// <summary>
        /// 受付日
        /// </summary>
        [Column("uketsuke_date")]
        public DateTime? UketsukeDate { get; set; }

        /// <summary>
        /// 受理方法コード
        /// </summary>
        [Column("juri_cd")]
        [StringLength(2)]
        public string JuriCd { get; set; }

        /// <summary>
        /// 面談者（先方_自由入力）１
        /// </summary>
        [Column("mendan_senpou_fnm1")]
        [StringLength(50)]
        public string MendanSenpouFnm1 { get; set; }

        /// <summary>
        /// 面談者（先方_自由入力）２
        /// </summary>
        [Column("mendan_senpou_fnm2")]
        [StringLength(50)]
        public string MendanSenpouFnm2 { get; set; }

        /// <summary>
        /// 面談者（先方_自由入力）３
        /// </summary>
        [Column("mendan_senpou_fnm3")]
        [StringLength(50)]
        public string MendanSenpouFnm3 { get; set; }

        /// <summary>
        /// 面談者（先方_自由入力）４
        /// </summary>
        [Column("mendan_senpou_fnm4")]
        [StringLength(50)]
        public string MendanSenpouFnm4 { get; set; }

        /// <summary>
        /// 面談者（当方ID）1
        /// </summary>
        [Column("mendan_tohou_id1")]
        [StringLength(11)]
        public string MendanTohouId1 { get; set; }

        /// <summary>
        /// 面談者（当方ID）2
        /// </summary>
        [Column("mendan_tohou_id2")]
        [StringLength(11)]
        public string MendanTohouId2 { get; set; }

        /// <summary>
        /// 面談者（当方ID）3
        /// </summary>
        [Column("mendan_tohou_id3")]
        [StringLength(11)]
        public string MendanTohouId3 { get; set; }

        /// <summary>
        /// 面談者（当方_自由入力）４
        /// </summary>
        [Column("mendan_tohou_fnm4")]
        [StringLength(50)]
        public string MendanTohouFnm4 { get; set; }

        /// <summary>
        /// 事業等コード
        /// </summary>
        [Column("jigyoto_cd")]
        [StringLength(3)]
        public string JigyotoCd { get; set; }

        /// <summary>
        /// 事業細目コード
        /// </summary>
        [Column("jigyo_saimoku_cd")]
        [StringLength(4)]
        public string JigyoSaimokuCd { get; set; }

        /// <summary>
        /// 加入意向区分
        /// </summary>
        [Column("kanyu_ikou_kbn")]
        [StringLength(2)]
        public string KanyuIkouKbn { get; set; }

        /// <summary>
        /// 内容細目_制度
        /// </summary>
        [Column("naiyo_saimoku_seido")]
        [StringLength(1)]
        public string NaiyoSaimokuSeido { get; set; }

        /// <summary>
        /// 内容細目_引受
        /// </summary>
        [Column("naiyo_saimoku_hikiuke")]
        [StringLength(1)]
        public string NaiyoSaimokuHikiuke { get; set; }

        /// <summary>
        /// 内容細目_評価
        /// </summary>
        [Column("naiyo_saimoku_hyoka")]
        [StringLength(1)]
        public string NaiyoSaimokuHyoka { get; set; }

        /// <summary>
        /// 内容細目_事務処理
        /// </summary>
        [Column("naiyo_saimoku_jimu")]
        [StringLength(1)]
        public string NaiyoSaimokuJimu { get; set; }

        /// <summary>
        /// 内容細目_システム
        /// </summary>
        [Column("naiyo_saimoku_system")]
        [StringLength(1)]
        public string NaiyoSaimokuSystem { get; set; }

        /// <summary>
        /// 内容細目_業務内容
        /// </summary>
        [Column("naiyo_saimoku_gyomu")]
        [StringLength(1)]
        public string NaiyoSaimokuGyomu { get; set; }

        /// <summary>
        /// 内容細目_対処方法
        /// </summary>
        [Column("naiyo_saimoku_taisho")]
        [StringLength(1)]
        public string NaiyoSaimokuTaisho { get; set; }

        /// <summary>
        /// 内容細目_その他
        /// </summary>
        [Column("naiyo_saimoku_sonota")]
        [StringLength(1)]
        public string NaiyoSaimokuSonota { get; set; }

        /// <summary>
        /// 内容種類
        /// </summary>
        [Column("naiyo_shurui")]
        [StringLength(40)]
        public string NaiyoShurui { get; set; }

        /// <summary>
        /// 質問内容
        /// </summary>
        [Column("shitsumon_naiyo")]
        [StringLength(1500)]
        public string ShitsumonNaiyo { get; set; }

        /// <summary>
        /// 最終更新日（受付）
        /// </summary>
        [Column("uketsuke_last_update_date")]
        public DateTime? UketsukeLastUpdateDate { get; set; }

        /// <summary>
        /// 最終更新者ID（受付）
        /// </summary>
        [Column("uketsuke_last_update_user_id")]
        [StringLength(11)]
        public string UketsukeLastUpdateUserId { get; set; }

        /// <summary>
        /// 回答区分コード
        /// </summary>
        [Column("kaito_cd")]
        [StringLength(3)]
        public string KaitoCd { get; set; }

        /// <summary>
        /// 回答日付
        /// </summary>
        [Column("kaito_date")]
        public DateTime? KaitoDate { get; set; }

        /// <summary>
        /// 回答者ID
        /// </summary>
        [Column("kaito_user_id")]
        [StringLength(11)]
        public string KaitoUserId { get; set; }

        /// <summary>
        /// 回答内容
        /// </summary>
        [Column("kaito_naiyo")]
        [StringLength(1500)]
        public string KaitoNaiyo { get; set; }

        /// <summary>
        /// 備考（回答）
        /// </summary>
        [Column("kaito_biko")]
        [StringLength(300)]
        public string KaitoBiko { get; set; }

        /// <summary>
        /// 回答済区分
        /// </summary>
        [Column("kaitozumi_kbn")]
        [StringLength(1)]
        public string KaitozumiKbn { get; set; }

        /// <summary>
        /// 最終更新日（回答）
        /// </summary>
        [Column("kaito_last_update_date")]
        public DateTime? KaitoLastUpdateDate { get; set; }

        /// <summary>
        /// 最終更新者ID（回答）
        /// </summary>
        [Column("kaito_last_update_user_id")]
        [StringLength(11)]
        public string KaitoLastUpdateUserId { get; set; }

        /// <summary>
        /// 対処区分コード(経過)
        /// </summary>
        [Column("keika_taisho_cd")]
        [StringLength(3)]
        public string KeikaTaishoCd { get; set; }

        /// <summary>
        /// 対応日付（経過）
        /// </summary>
        [Column("keika_taio_date")]
        public DateTime? KeikaTaioDate { get; set; }

        /// <summary>
        /// 対応者ID（経過）
        /// </summary>
        [Column("keika_taio_user_id")]
        [StringLength(11)]
        public string KeikaTaioUserId { get; set; }

        /// <summary>
        /// 対応内容（経過）
        /// </summary>
        [Column("keika_taio_naiyo")]
        [StringLength(1500)]
        public string KeikaTaioNaiyo { get; set; }

        /// <summary>
        /// 備考（経過）
        /// </summary>
        [Column("keika_biko")]
        [StringLength(300)]
        public string KeikaBiko { get; set; }

        /// <summary>
        /// 最終更新日（経過）
        /// </summary>
        [Column("keika_last_update_date")]
        public DateTime? KeikaLastUpdateDate { get; set; }

        /// <summary>
        /// 最終更新者ID（経過）
        /// </summary>
        [Column("keika_last_update_user_id")]
        [StringLength(11)]
        public string KeikaLastUpdateUserId { get; set; }

        /// <summary>
        /// 対処区分コード(周知)
        /// </summary>
        [Column("shuchi_taisho_cd")]
        [StringLength(3)]
        public string ShuchiTaishoCd { get; set; }

        /// <summary>
        /// 対応日付（周知）
        /// </summary>
        [Column("shuchi_taio_date")]
        public DateTime? ShuchiTaioDate { get; set; }

        /// <summary>
        /// 対応者ID（周知）
        /// </summary>
        [Column("shuchi_taio_user_id")]
        [StringLength(11)]
        public string ShuchiTaioUserId { get; set; }

        /// <summary>
        /// 対応内容（周知）
        /// </summary>
        [Column("shuchi_taio_naiyo")]
        [StringLength(1500)]
        public string ShuchiTaioNaiyo { get; set; }

        /// <summary>
        /// 備考（周知）
        /// </summary>
        [Column("shuchi_biko")]
        [StringLength(300)]
        public string ShuchiBiko { get; set; }

        /// <summary>
        /// 最終更新日（周知）
        /// </summary>
        [Column("shuchi_last_update_date1")]
        public DateTime? ShuchiLastUpdateDate1 { get; set; }

        /// <summary>
        /// 最終更新者ID（周知）
        /// </summary>
        [Column("shuchi_last_update_user_id")]
        [StringLength(11)]
        public string ShuchiLastUpdateUserId { get; set; }

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
