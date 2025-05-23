using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// _ÆÒîñ
    /// </summary>
    [Serializable]
    [Table("t_nogyosha")]
    public class TNogyosha : ModelBase
    {
        /// <summary>
        /// _ÆÒID
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// _ÆÒÇR[h
        /// </summary>
        [Required]
        [Column("nogyosha_cd")]
        [StringLength(13)]
        public string NogyoshaCd { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Column("kumiaiinto_cd")]
        [StringLength(13)]
        public string KumiaiintoCd { get; set; }

        /// <summary>
        /// s¹{§R[h
        /// </summary>
        [Column("todofuken_cd")]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// gR[h
        /// </summary>
        [Column("kumiaito_cd")]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// xR[h
        /// </summary>
        [Column("shisho_cd")]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// s¬ºR[h
        /// </summary>
        [Column("shichoson_cd")]
        [StringLength(5)]
        public string ShichosonCd { get; set; }

        /// <summary>
        /// ånæR[h
        /// </summary>
        [Column("daichiku_cd")]
        [StringLength(2)]
        public string DaichikuCd { get; set; }

        /// <summary>
        /// ¬næR[h
        /// </summary>
        [Column("shochiku_cd")]
        [StringLength(4)]
        public string ShochikuCd { get; set; }

        /// <summary>
        /// oc`Ô
        /// </summary>
        [Column("keiei_keitai_cd")]
        [StringLength(1)]
        public string KeieiKeitaiCd { get; set; }

        /// <summary>
        /// @líÞ
        /// </summary>
        [Column("hojin_shurui_cd")]
        [StringLength(1)]
        public string HojinShuruiCd { get; set; }

        /// <summary>
        /// ¼Í@l¼
        /// </summary>
        [Required]
        [Column("hojin_full_nm")]
        [StringLength(30)]
        public string HojinFullNm { get; set; }

        /// <summary>
        /// ¼Í@l¼tKi
        /// </summary>
        [Column("hojin_full_kana")]
        [StringLength(30)]
        public string HojinFullKana { get; set; }

        /// <summary>
        /// ã\Ò¼
        /// </summary>
        [Column("daihyosha_nm")]
        [StringLength(30)]
        public string DaihyoshaNm { get; set; }

        /// <summary>
        /// ã\Ò¼tKi
        /// </summary>
        [Column("daihyosha_kana")]
        [StringLength(30)]
        public string DaihyoshaKana { get; set; }

        /// <summary>
        /// XÖÔ
        /// </summary>
        [Column("postal_cd")]
        [StringLength(7)]
        public string PostalCd { get; set; }

        /// <summary>
        /// ZJi
        /// </summary>
        [Column("address_kana")]
        [StringLength(60)]
        public string AddressKana { get; set; }

        /// <summary>
        /// Z
        /// </summary>
        [Column("address")]
        [StringLength(40)]
        public string Address { get; set; }

        /// <summary>
        /// kn{ÉZJi
        /// </summary>
        [Column("kochito_address_kana")]
        [StringLength(60)]
        public string KochitoAddressKana { get; set; }

        /// <summary>
        /// kn{ÉZ
        /// </summary>
        [Column("kochito_address")]
        [StringLength(40)]
        public string KochitoAddress { get; set; }

        /// <summary>
        /// «Ê
        /// </summary>
        [Column("gender")]
        [StringLength(1)]
        public string Gender { get; set; }

        /// <summary>
        /// ¶Nú
        /// </summary>
        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// dbÔ
        /// </summary>
        [Column("tel")]
        [StringLength(13)]
        public string Tel { get; set; }

        /// <summary>
        /// gÑdbÔ
        /// </summary>
        [Column("cell")]
        [StringLength(14)]
        public string Cell { get; set; }

        /// <summary>
        /// FAXÔ
        /// </summary>
        [Column("fax")]
        [StringLength(12)]
        public string Fax { get; set; }

        /// <summary>
        /// [AhX
        /// </summary>
        [Column("mail")]
        [StringLength(50)]
        public string Mail { get; set; }

        /// <summary>
        /// z[y[W
        /// </summary>
        [Column("website")]
        [StringLength(50)]
        public string Website { get; set; }

        /// <summary>
        /// ¢Ñåæª
        /// </summary>
        [Column("setai_nushi_kbn")]
        [StringLength(1)]
        public string SetaiNushiKbn { get; set; }

        /// <summary>
        /// gõiæª
        /// </summary>
        [Column("kumiaiinto_shikaku_kbn")]
        [StringLength(1)]
        public string KumiaiintoShikakuKbn { get; set; }

        /// <summary>
        /// gõ¼ëoÍæª
        /// </summary>
        [Column("kumiaiinto_output_kbn")]
        [StringLength(1)]
        public string KumiaiintoOutputKbn { get; set; }

        /// <summary>
        /// gõÛà¤ÏÆR[h
        /// </summary>
        [Column("fukakin_kyosai_jigyo_cd")]
        [StringLength(2)]
        public string FukakinKyosaiJigyoCd { get; set; }

        /// <summary>
        /// gõÛà¤ÏÚIR[h
        /// </summary>
        [Column("fukakin_kyosai_mokutekito_cd")]
        [StringLength(2)]
        public string FukakinKyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// JAR[h
        /// </summary>
        [Column("ja_cd")]
        [StringLength(4)]
        public string JaCd { get; set; }

        /// <summary>
        /// Ä¶¦cïR[h
        /// </summary>
        [Column("saisei_kyogikai_cd")]
        [StringLength(3)]
        public string SaiseiKyogikaiCd { get; set; }

        /// <summary>
        /// ¤ÏV·wÇæª
        /// </summary>
        [Column("kyosai_shinbun_kbn")]
        [StringLength(1)]
        public string KyosaiShinbunKbn { get; set; }

        /// <summary>
        /// kíBCPæª
        /// </summary>
        [Column("koshu_bcp_kbn")]
        [StringLength(1)]
        public string KoshuBcpKbn { get; set; }

        /// <summary>
        /// |BCPæª
        /// </summary>
        [Column("engei_bcp_kbn")]
        [StringLength(1)]
        public string EngeiBcpKbn { get; set; }

        /// <summary>
        /// {YBCPæª
        /// </summary>
        [Column("chikusan_bcp_kbn")]
        [StringLength(1)]
        public string ChikusanBcpKbn { get; set; }

        /// <summary>
        /// ÁüNú
        /// </summary>
        [Column("kanyu_ymd")]
        public DateTime? KanyuYmd { get; set; }

        /// <summary>
        /// EÞNú
        /// </summary>
        [Column("dattai_ymd")]
        public DateTime? DattaiYmd { get; set; }

        /// <summary>
        /// SiðUjAótNú
        /// </summary>
        [Column("shibo_renraku_ymd")]
        public DateTime? ShiboRenrakuYmd { get; set; }

        /// <summary>
        /// SiðUjNú
        /// </summary>
        [Column("shibo_ymd")]
        public DateTime? ShiboYmd { get; set; }

        /// <summary>
        /// XæZtO
        /// </summary>
        [Column("yusosaki_flg")]
        [StringLength(1)]
        public string YusosakiFlg { get; set; }

        /// <summary>
        /// ¶zzEõÎtO
        /// </summary>
        [Column("yuso_shokuin_taio_flg")]
        [StringLength(1)]
        public string YusoShokuinTaioFlg { get; set; }

        /// <summary>
        /// XæXÖÔ
        /// </summary>
        [Column("yuso_postal_cd")]
        [StringLength(7)]
        public string YusoPostalCd { get; set; }

        /// <summary>
        /// XæZ
        /// </summary>
        [Column("yuso_address")]
        [StringLength(40)]
        public string YusoAddress { get; set; }

        /// <summary>
        /// Xæ¼Í@l¼
        /// </summary>
        [Column("yuso_hojin_full_nm")]
        [StringLength(30)]
        public string YusoHojinFullNm { get; set; }

        /// <summary>
        /// Xæã\Ò¼
        /// </summary>
        [Column("yuso_daihyosha_nm")]
        [StringLength(30)]
        public string YusoDaihyoshaNm { get; set; }

        /// <summary>
        /// ítO
        /// </summary>
        [Required]
        [Column("delete_flg")]
        [StringLength(1)]
        public string DeleteFlg { get; set; }

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
