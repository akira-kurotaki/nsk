using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 職員マスタ
    /// </summary>
    [Serializable]
    [Table("v_syokuin")]
    [PrimaryKey(nameof(UserId), nameof(UserKanriNo))]
    public class VSyokuin : ISyokuin
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Required]
        [Column("user_id", Order = 1)]
        [StringLength(11)]
        public string UserId { get; set; }

        /// <summary>
        /// ユーザ管理番号
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("user_kanri_no", Order = 2)]
        public int UserKanriNo { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Column("todofuken_cd")]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Column("kumiaito_cd")]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("shisho_cd")]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// ユーザ管理権限
        /// </summary>
        [Required]
        [Column("user_kanri_kengen")]
        [StringLength(1)]
        public string UserKanriKengen { get; set; }

        /// <summary>
        /// バッチ管理権限
        /// </summary>
        [Required]
        [Column("batch_kanri_kengen")]
        [StringLength(1)]
        public string BatchKanriKengen { get; set; }

        /// <summary>
        /// ユーザ情報有効期間開始日
        /// </summary>
        [Required]
        [Column("user_yuko_start_ymd")]
        public DateTime UserYukoStartYmd { get; set; }

        /// <summary>
        /// ユーザ情報有効期間終了日
        /// </summary>
        [Column("user_yuko_end_ymd")]
        public DateTime? UserYukoEndYmd { get; set; }

        /// <summary>
        /// 氏名
        /// </summary>
        [Required]
        [Column("full_nm")]
        [StringLength(30)]
        public string FullNm { get; set; }

        /// <summary>
        /// フリガナ
        /// </summary>
        [Column("full_kana")]
        [StringLength(30)]
        public string FullKana { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [Column("tel")]
        [StringLength(13)]
        public string Tel { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Column("mail")]
        [StringLength(50)]
        public string Mail { get; set; }

        /// <summary>
        /// システム利用者区分
        /// </summary>
        [Column("system_riyo_kbn")]
        [StringLength(1)]
        public string SystemRiyoKbn { get; set; }

        /// <summary>
        /// 利用権限_加入状況管理
        /// </summary>
        [Column("kjk_kengen")]
        [StringLength(1)]
        public string KjkKengen { get; set; }

        /// <summary>
        /// 利用権限_農業者情報管理
        /// </summary>
        [Column("fim_kengen")]
        [StringLength(1)]
        public string FimKengen { get; set; }

        /// <summary>
        /// 利用権限_農作物共済
        /// </summary>
        [Column("nsk_kengen")]
        [StringLength(1)]
        public string NskKengen { get; set; }

        /// <summary>
        /// 利用権限_建物共済
        /// </summary>
        [Column("sml_kengen")]
        [StringLength(1)]
        public string SmlKengen { get; set; }

        /// <summary>
        /// 利用権限_収入保険
        /// </summary>
        [Column("syn_kengen")]
        [StringLength(1)]
        public string SynKengen { get; set; }

        /// <summary>
        /// 利用権限_園芸施設共済（共通申請）
        /// </summary>
        [Column("kyotsu_eng_kengen")]
        [StringLength(1)]
        public string KyotsuEngKengen { get; set; }

        /// <summary>
        /// 利用権限_家畜共済（共通申請）
        /// </summary>
        [Column("kyotsu_ktk_kengen")]
        [StringLength(1)]
        public string KyotsuKtkKengen { get; set; }

        /// <summary>
        /// 利用権限_農作物共済（共通申請連携）
        /// </summary>
        [Column("kyotsu_renkei_nsk_kengen")]
        [StringLength(1)]
        public string KyotsuRenkeiNskKengen { get; set; }

        /// <summary>
        /// 利用権限_畑作物共済（共通申請連携）
        /// </summary>
        [Column("kyotsu_renkei_hat_kengen")]
        [StringLength(1)]
        public string KyotsuRenkeiHatKengen { get; set; }

        /// <summary>
        /// 利用権限_果樹共済（共通申請連携）
        /// </summary>
        [Column("kyotsu_renkei_kju_kengen")]
        [StringLength(1)]
        public string KyotsuRenkeiKjuKengen { get; set; }

        /// <summary>
        /// システム利用者管理区分_収入保険
        /// </summary>
        [Column("syn_system_kanri_kbn")]
        [StringLength(1)]
        public string SynSystemKanriKbn { get; set; }

        /// <summary>
        /// システム利用者管理区分_家畜共済(共通申請)
        /// </summary>
        [Column("kyotsu_ktk_system_kanri_kbn")]
        [StringLength(1)]
        public string KyotsuKtkSystemKanriKbn { get; set; }

        /// <summary>
        /// システム利用者管理区分_園芸施設共済（共通申請）
        /// </summary>
        [Column("kyotsu_eng_system_kanri_kbn")]
        [StringLength(1)]
        public string KyotsuEngSystemKanriKbn { get; set; }

        /// <summary>
        /// 画面操作権限ID_加入状況管理
        /// </summary>
        [Column("kjk_screen_sosa_kengen_id")]
        public int? KjkScreenSosaKengenId { get; set; }

        /// <summary>
        /// 画面操作権限ID_農業者情報管理
        /// </summary>
        [Column("fim_screen_sosa_kengen_id")]
        public int? FimScreenSosaKengenId { get; set; }

        /// <summary>
        /// 画面操作権限ID_農作物共済
        /// </summary>
        [Column("nsk_screen_sosa_kengen_id")]
        public int? NskScreenSosaKengenId { get; set; }

        /// <summary>
        /// 画面操作権限ID_建物共済
        /// </summary>
        [Column("sml_screen_sosa_kengen_id")]
        public int? SmlScreenSosaKengenId { get; set; }

        /// <summary>
        /// 支所グループID_加入状況管理
        /// </summary>
        [Column("kjk_shisho_group_id")]
        public int? KjkShishoGroupId { get; set; }

        /// <summary>
        /// 支所グループID_農業者情報管理
        /// </summary>
        [Column("fim_shisho_group_id")]
        public int? FimShishoGroupId { get; set; }

        /// <summary>
        /// 支所グループID_農作物共済
        /// </summary>
        [Column("nsk_shisho_group_id")]
        public int? NskShishoGroupId { get; set; }

        /// <summary>
        /// 支所グループID_建物共済
        /// </summary>
        [Column("sml_shisho_group_id")]
        public int? SmlShishoGroupId { get; set; }

        /// <summary>
        /// 支所グループID_収入保険
        /// </summary>
        [Column("syn_shisho_group_id")]
        public int? SynShishoGroupId { get; set; }

        /// <summary>
        /// 秘密保持誓約書提出
        /// </summary>
        [Column("himitsu_seiyaku_flg")]
        [StringLength(1)]
        public string HimitsuSeiyakuFlg { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        [Column("pwd")]
        [StringLength(84)]
        public string Pwd { get; set; }

        /// <summary>
        /// パスワード最終更新年月日
        /// </summary>
        [Column("pwd_last_update_ymd")]
        public DateTime? PwdLastUpdateYmd { get; set; }

        /// <summary>
        /// ログイン日時
        /// </summary>
        [Column("login_date")]
        public DateTime? LoginDate { get; set; }

        /// <summary>
        /// 最終ログイン日時
        /// </summary>
        [Column("last_login_date")]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// ロックフラグ
        /// </summary>
        [Required]
        [Column("lock_flg")]
        [StringLength(1)]
        public string LockFlg { get; set; }

        /// <summary>
        /// ログイン失敗回数
        /// </summary>
        [Column("login_fail_cnt")]
        public short? LoginFailCnt { get; set; }

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
