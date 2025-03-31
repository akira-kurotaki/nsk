using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 外部金融機関マスタ
    /// </summary>
    [Serializable]
    [Table("v_gaibu_kinyukikan")]
    [PrimaryKey(nameof(KinyukikanCd), nameof(SortCd))]
    public class VGaibuKinyukikan : ModelBase
    {
        /// <summary>
        /// 金融機関コード
        /// </summary>
        [Required]
        [Column("kinyukikan_cd", Order = 1)]
        [StringLength(7)]
        public string KinyukikanCd { get; set; }

        /// <summary>
        /// 並びコード
        /// </summary>
        [Required]
        [Column("sort_cd", Order = 2)]
        [StringLength(1)]
        public string SortCd { get; set; }

        /// <summary>
        /// 銀行番号
        /// </summary>
        [Column("ginkou_no")]
        [StringLength(4)]
        public string GinkouNo { get; set; }

        /// <summary>
        /// 支店番号
        /// </summary>
        [Column("shiten_no")]
        [StringLength(3)]
        public string ShitenNo { get; set; }

        /// <summary>
        /// 金融機関名カナ
        /// </summary>
        [Column("kinyukikan_kana")]
        public string KinyukikanKana { get; set; }

        /// <summary>
        /// 金融機関名
        /// </summary>
        [Column("kinyukikan_nm")]
        public string KinyukikanNm { get; set; }

        /// <summary>
        /// 店舗名カナ
        /// </summary>
        [Column("mise_kana")]
        public string MiseKana { get; set; }

        /// <summary>
        /// 店舗名
        /// </summary>
        [Column("mise_nm")]
        public string MiseNm { get; set; }

        /// <summary>
        /// 店舗郵便番号
        /// </summary>
        [Column("mise_postal_cd")]
        [StringLength(8)]
        public string MisePostalCd { get; set; }

        /// <summary>
        /// 店舗所在地
        /// </summary>
        [Column("mise_address")]
        public string MiseAddress { get; set; }

        /// <summary>
        /// 店舗電話番号
        /// </summary>
        [Column("mise_tel")]
        [StringLength(17)]
        public string MiseTel { get; set; }

        /// <summary>
        /// 手形交換所番号
        /// </summary>
        [Column("tegata_no")]
        [StringLength(4)]
        public string TegataNo { get; set; }

        /// <summary>
        /// 内国為替制度加盟
        /// </summary>
        [Column("kawase_kamei_kbn")]
        [StringLength(1)]
        public string KawaseKameiKbn { get; set; }

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
