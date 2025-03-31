using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 農業者共通申請利用管理
    /// </summary>
    [Serializable]
    [Table("v_nogyosha_kyotsu_kanri")]
    public class VNogyoshaKyotsuKanri : ModelBase
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
        /// 共通申請紐付けフラグ
        /// </summary>
        [Required]
        [Column("himozuke_flg")]
        [StringLength(1)]
        public string HimozukeFlg { get; set; }

        /// <summary>
        /// 経営体ID
        /// </summary>
        [Column("keieitai_id")]
        [StringLength(12)]
        public string KeieitaiId { get; set; }

        /// <summary>
        /// 同一経営体確認フラグ
        /// </summary>
        [Required]
        [Column("douitsu_keieita_kakunin_flg")]
        [StringLength(1)]
        public string DouitsuKeieitaKakuninFlg { get; set; }

        /// <summary>
        /// 紐付け実施年月日
        /// </summary>
        [Column("himozuke_jisshi_ymd")]
        public DateTime? HimozukeJisshiYmd { get; set; }

        /// <summary>
        /// 紐付け担当者ID
        /// </summary>
        [Column("himozuke_tantosha_id")]
        [StringLength(11)]
        public string HimozukeTantoshaId { get; set; }

        /// <summary>
        /// 紐付けメモ
        /// </summary>
        [Column("himozuke_memo")]
        [StringLength(300)]
        public string HimozukeMemo { get; set; }

        /// <summary>
        /// 収入保険通知メール区分
        /// </summary>
        [Column("syn_tsuchi_mail_kbn")]
        [StringLength(1)]
        public string SynTsuchiMailKbn { get; set; }

        /// <summary>
        /// 家畜共済通知メール区分
        /// </summary>
        [Column("ktk_tsuchi_mail_kbn")]
        [StringLength(1)]
        public string KtkTsuchiMailKbn { get; set; }

        /// <summary>
        /// 園芸施設共済通知メール区分
        /// </summary>
        [Column("eng_tsuchi_mail_kbn")]
        [StringLength(1)]
        public string EngTsuchiMailKbn { get; set; }

        /// <summary>
        /// 収入保険オンライン申請有効化フラグ
        /// </summary>
        [Required]
        [Column("syn_online_yuko_flg")]
        [StringLength(1)]
        public string SynOnlineYukoFlg { get; set; }

        /// <summary>
        /// 農作物共済オンライン申請有効化フラグ
        /// </summary>
        [Required]
        [Column("nsk_online_yuko_flg")]
        [StringLength(1)]
        public string NskOnlineYukoFlg { get; set; }

        /// <summary>
        /// 家畜共済オンライン申請有効化フラグ
        /// </summary>
        [Required]
        [Column("ktk_online_yuko_flg")]
        [StringLength(1)]
        public string KtkOnlineYukoFlg { get; set; }

        /// <summary>
        /// 果樹共済オンライン申請有効化フラグ
        /// </summary>
        [Required]
        [Column("kju_online_yuko_flg")]
        [StringLength(1)]
        public string KjuOnlineYukoFlg { get; set; }

        /// <summary>
        /// 畑作物共済オンライン申請有効化フラグ
        /// </summary>
        [Required]
        [Column("hat_online_yuko_flg")]
        [StringLength(1)]
        public string HatOnlineYukoFlg { get; set; }

        /// <summary>
        /// 園芸施設共済オンライン申請有効化フラグ
        /// </summary>
        [Required]
        [Column("eng_online_yuko_flg")]
        [StringLength(1)]
        public string EngOnlineYukoFlg { get; set; }

        /// <summary>
        /// 経営形態氏名等選択フラグ
        /// </summary>
        [Column("keiei_keitai_shimei_flg")]
        [StringLength(1)]
        public string KeieiKeitaiShimeiFlg { get; set; }

        /// <summary>
        /// 郵便番号選択フラグ
        /// </summary>
        [Column("postal_cd_flg")]
        [StringLength(1)]
        public string PostalCdFlg { get; set; }

        /// <summary>
        /// 住所選択フラグ
        /// </summary>
        [Column("address_flg")]
        [StringLength(1)]
        public string AddressFlg { get; set; }

        /// <summary>
        /// 性別選択フラグ
        /// </summary>
        [Column("gender_flg")]
        [StringLength(1)]
        public string GenderFlg { get; set; }

        /// <summary>
        /// 生年月日選択フラグ
        /// </summary>
        [Column("date_of_birth_flg")]
        [StringLength(1)]
        public string DateOfBirthFlg { get; set; }

        /// <summary>
        /// 電話番号選択フラグ
        /// </summary>
        [Column("tel_flg")]
        [StringLength(1)]
        public string TelFlg { get; set; }

        /// <summary>
        /// FAX番号選択フラグ
        /// </summary>
        [Column("fax_flg")]
        [StringLength(1)]
        public string FaxFlg { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Required]
        [Column("delete_flg")]
        [StringLength(1)]
        public string DeleteFlg { get; set; }

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
