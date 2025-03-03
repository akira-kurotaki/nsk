using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 農業者情報
    /// </summary>
    [Serializable]
    [Table("v_nogyosha")]
    public class VNogyosha
    {
        /// <summary>
        /// 農業者ID
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 農業者管理コード
        /// </summary>
        [Required]
        [Column("nogyosha_cd")]
        [StringLength(13)]
        public string NogyoshaCd { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Column("kumiaiinto_cd")]
        [StringLength(13)]
        public string KumiaiintoCd { get; set; }

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
        /// 市町村コード
        /// </summary>
        [Column("shichoson_cd")]
        [StringLength(5)]
        public string ShichosonCd { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Column("daichiku_cd")]
        [StringLength(2)]
        public string DaichikuCd { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        [Column("shochiku_cd")]
        [StringLength(4)]
        public string ShochikuCd { get; set; }

        /// <summary>
        /// 経営形態
        /// </summary>
        [Column("keiei_keitai_cd")]
        [StringLength(1)]
        public string KeieiKeitaiCd { get; set; }

        /// <summary>
        /// 法人種類
        /// </summary>
        [Column("hojin_shurui_cd")]
        [StringLength(1)]
        public string HojinShuruiCd { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        [Required]
        [Column("hojin_full_nm")]
        [StringLength(30)]
        public string HojinFullNm { get; set; }

        /// <summary>
        /// 氏名又は法人名フリガナ
        /// </summary>
        [Column("hojin_full_kana")]
        [StringLength(30)]
        public string HojinFullKana { get; set; }

        /// <summary>
        /// 代表者氏名
        /// </summary>
        [Column("daihyosha_nm")]
        [StringLength(30)]
        public string DaihyoshaNm { get; set; }

        /// <summary>
        /// 代表者氏名フリガナ
        /// </summary>
        [Column("daihyosha_kana")]
        [StringLength(30)]
        public string DaihyoshaKana { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        [Column("postal_cd")]
        [StringLength(7)]
        public string PostalCd { get; set; }

        /// <summary>
        /// 住所カナ
        /// </summary>
        [Column("address_kana")]
        [StringLength(60)]
        public string AddressKana { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [Column("address")]
        [StringLength(40)]
        public string Address { get; set; }

        /// <summary>
        /// 耕地畜舎等住所カナ
        /// </summary>
        [Column("kochito_address_kana")]
        [StringLength(60)]
        public string KochitoAddressKana { get; set; }

        /// <summary>
        /// 耕地畜舎等住所
        /// </summary>
        [Column("kochito_address")]
        [StringLength(40)]
        public string KochitoAddress { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        [Column("gender")]
        [StringLength(1)]
        public string Gender { get; set; }

        /// <summary>
        /// 生年月日
        /// </summary>
        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [Column("tel")]
        [StringLength(13)]
        public string Tel { get; set; }

        /// <summary>
        /// 携帯電話番号
        /// </summary>
        [Column("cell")]
        [StringLength(14)]
        public string Cell { get; set; }

        /// <summary>
        /// FAX番号
        /// </summary>
        [Column("fax")]
        [StringLength(12)]
        public string Fax { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Column("mail")]
        [StringLength(50)]
        public string Mail { get; set; }

        /// <summary>
        /// ホームページ
        /// </summary>
        [Column("website")]
        [StringLength(50)]
        public string Website { get; set; }

        /// <summary>
        /// 世帯主区分
        /// </summary>
        [Column("setai_nushi_kbn")]
        [StringLength(1)]
        public string SetaiNushiKbn { get; set; }

        /// <summary>
        /// 組合員等資格区分
        /// </summary>
        [Column("kumiaiinto_shikaku_kbn")]
        [StringLength(1)]
        public string KumiaiintoShikakuKbn { get; set; }

        /// <summary>
        /// 組合員等名簿出力区分
        /// </summary>
        [Column("kumiaiinto_output_kbn")]
        [StringLength(1)]
        public string KumiaiintoOutputKbn { get; set; }

        /// <summary>
        /// 組合員等賦課金共済事業コード
        /// </summary>
        [Column("fukakin_kyosai_jigyo_cd")]
        [StringLength(2)]
        public string FukakinKyosaiJigyoCd { get; set; }

        /// <summary>
        /// 組合員等賦課金共済目的等コード
        /// </summary>
        [Column("fukakin_kyosai_mokutekito_cd")]
        [StringLength(2)]
        public string FukakinKyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// JAコード
        /// </summary>
        [Column("ja_cd")]
        [StringLength(4)]
        public string JaCd { get; set; }

        /// <summary>
        /// 再生協議会コード
        /// </summary>
        [Column("saisei_kyogikai_cd")]
        [StringLength(3)]
        public string SaiseiKyogikaiCd { get; set; }

        /// <summary>
        /// 共済新聞購読区分
        /// </summary>
        [Column("kyosai_shinbun_kbn")]
        [StringLength(1)]
        public string KyosaiShinbunKbn { get; set; }

        /// <summary>
        /// 耕種BCP区分
        /// </summary>
        [Column("koshu_bcp_kbn")]
        [StringLength(1)]
        public string KoshuBcpKbn { get; set; }

        /// <summary>
        /// 園芸BCP区分
        /// </summary>
        [Column("engei_bcp_kbn")]
        [StringLength(1)]
        public string EngeiBcpKbn { get; set; }

        /// <summary>
        /// 畜産BCP区分
        /// </summary>
        [Column("chikusan_bcp_kbn")]
        [StringLength(1)]
        public string ChikusanBcpKbn { get; set; }

        /// <summary>
        /// 加入年月日
        /// </summary>
        [Column("kanyu_ymd")]
        public DateTime? KanyuYmd { get; set; }

        /// <summary>
        /// 脱退年月日
        /// </summary>
        [Column("dattai_ymd")]
        public DateTime? DattaiYmd { get; set; }

        /// <summary>
        /// 死亡（解散）連絡受付年月日
        /// </summary>
        [Column("shibo_renraku_ymd")]
        public DateTime? ShiboRenrakuYmd { get; set; }

        /// <summary>
        /// 死亡（解散）年月日
        /// </summary>
        [Column("shibo_ymd")]
        public DateTime? ShiboYmd { get; set; }

        /// <summary>
        /// 郵送先住所フラグ
        /// </summary>
        [Column("yusosaki_flg")]
        [StringLength(1)]
        public string YusosakiFlg { get; set; }

        /// <summary>
        /// 文書配布職員対応フラグ
        /// </summary>
        [Column("yuso_shokuin_taio_flg")]
        [StringLength(1)]
        public string YusoShokuinTaioFlg { get; set; }

        /// <summary>
        /// 郵送先郵便番号
        /// </summary>
        [Column("yuso_postal_cd")]
        [StringLength(7)]
        public string YusoPostalCd { get; set; }

        /// <summary>
        /// 郵送先住所
        /// </summary>
        [Column("yuso_address")]
        [StringLength(40)]
        public string YusoAddress { get; set; }

        /// <summary>
        /// 郵送先氏名又は法人名
        /// </summary>
        [Column("yuso_hojin_full_nm")]
        [StringLength(30)]
        public string YusoHojinFullNm { get; set; }

        /// <summary>
        /// 郵送先代表者氏名
        /// </summary>
        [Column("yuso_daihyosha_nm")]
        [StringLength(30)]
        public string YusoDaihyoshaNm { get; set; }

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
