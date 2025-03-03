using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 組合等マスタ
    /// </summary>
    [Serializable]
    [Table("v_kumiaito")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd))]
    public class VKumiaito
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd", Order = 1)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("kumiaito_cd", Order = 2)]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 組合等正式名称
        /// </summary>
        [Required]
        [Column("kumiaito_nm")]
        [StringLength(25)]
        public string KumiaitoNm { get; set; }

        /// <summary>
        /// 組合等略称
        /// </summary>
        [Column("kumiaito_rnm")]
        [StringLength(12)]
        public string KumiaitoRnm { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        [Column("postal_cd")]
        [StringLength(7)]
        public string PostalCd { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [Column("address")]
        [StringLength(40)]
        public string Address { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [Column("tel")]
        [StringLength(13)]
        public string Tel { get; set; }

        /// <summary>
        /// FAX番号
        /// </summary>
        [Column("fax")]
        [StringLength(12)]
        public string Fax { get; set; }

        /// <summary>
        /// 組合等長名
        /// </summary>
        [Column("kumiaitocho_nm")]
        [StringLength(12)]
        public string KumiaitochoNm { get; set; }

        /// <summary>
        /// 代表者役職名
        /// </summary>
        [Column("daihyosha_yaku_nm")]
        [StringLength(12)]
        public string DaihyoshaYakuNm { get; set; }

        /// <summary>
        /// 代表者氏名
        /// </summary>
        [Column("daihyosha_nm")]
        [StringLength(30)]
        public string DaihyoshaNm { get; set; }

        /// <summary>
        /// インボイス登録番号
        /// </summary>
        [Column("invoice_no")]
        [StringLength(14)]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 農林水産大臣名
        /// </summary>
        [Column("norinsuisan_daijin_nm")]
        [StringLength(12)]
        public string NorinsuisanDaijinNm { get; set; }

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
