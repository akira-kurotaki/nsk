using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 口座振替委託者マスタ
    /// </summary>
    [Serializable]
    [Table("v_koza_furikae_itakusha")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(KanriNo))]
    public class VKozaFurikaeItakusha : ModelBase
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
        /// 管理ＮＯ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("kanri_no", Order = 3)]
        public short KanriNo { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("shisho_cd")]
        [StringLength(2)]
        public string ShishoCd { get; set; }

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
        /// 用途
        /// </summary>
        [Column("yoto")]
        [StringLength(30)]
        public string Yoto { get; set; }

        /// <summary>
        /// 金融機関コード
        /// </summary>
        [Column("kinyukikan_cd")]
        [StringLength(7)]
        public string KinyukikanCd { get; set; }

        /// <summary>
        /// 委託者コード
        /// </summary>
        [Column("itakusha_cd")]
        [StringLength(10)]
        public string ItakushaCd { get; set; }

        /// <summary>
        /// 委託者名
        /// </summary>
        [Column("itakusha_nm")]
        [StringLength(30)]
        public string ItakushaNm { get; set; }

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
