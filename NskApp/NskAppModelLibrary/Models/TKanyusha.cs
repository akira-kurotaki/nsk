using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// 加入者情報
    /// </summary>
    [Serializable]
    [Table("t_kanyusha")]
    [PrimaryKey(nameof(NogyoshaId), nameof(Nendo))]
    public class TKanyusha : ModelBase
    {
        /// <summary>
        /// 農業者ID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 加入者管理コード
        /// </summary>
        [Column("kanyusha_cd")]
        [StringLength(13)]
        public string KanyushaCd { get; set; }

        /// <summary>
        /// 対象年度
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nendo", Order = 2)]
        public short Nendo { get; set; }

        /// <summary>
        /// 耕地郵便番号
        /// </summary>
        [Column("kouchi_postal_cd")]
        [StringLength(7)]
        public string KouchiPostalCd { get; set; }

        /// <summary>
        /// 耕地住所フリガナ
        /// </summary>
        [Column("kouchi_address_kana")]
        [StringLength(60)]
        public string KouchiAddressKana { get; set; }

        /// <summary>
        /// 耕地住所
        /// </summary>
        [Column("kouchi_address")]
        [StringLength(40)]
        public string KouchiAddress { get; set; }

        /// <summary>
        /// 耕地面積
        /// </summary>
        [Column("kouchi_menseki")]
        public Decimal? KouchiMenseki { get; set; }

        /// <summary>
        /// 耕地形態
        /// </summary>
        [Column("kouchi_keitai_cd")]
        [StringLength(1)]
        public string KouchiKeitaiCd { get; set; }

        /// <summary>
        /// 個人情報取扱フラグ
        /// </summary>
        [Column("kojinjoho_toriatsukai_flg")]
        [StringLength(1)]
        public string KojinjohoToriatsukaiFlg { get; set; }

        /// <summary>
        /// 加入申請年月日
        /// </summary>
        [Column("kanyu_shinsei_ymd")]
        public DateTime? KanyuShinseiYmd { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("biko")]
        [StringLength(300)]
        public string Biko { get; set; }


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
