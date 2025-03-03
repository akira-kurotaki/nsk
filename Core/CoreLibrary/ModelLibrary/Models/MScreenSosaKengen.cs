using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 画面操作権限マスタ
    /// </summary>
    [Serializable]
    [Table("m_screen_sosa_kengen")]
    public class MScreenSosaKengen : ModelBase
    {
        /// <summary>
        /// 画面操作権限ID
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("screen_sosa_kengen_id", Order = 1)]
        public int ScreenSosaKengenId { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd")]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("kumiaito_cd")]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// システム区分
        /// </summary>
        [Required]
        [Column("system_kbn")]
        [StringLength(2)]
        public string SystemKbn { get; set; }

        /// <summary>
        /// 画面操作権限区分コード
        /// </summary>
        [Required]
        [Column("screen_sosa_kengen_cd")]
        [StringLength(4)]
        public string ScreenSosaKengenCd { get; set; }

        /// <summary>
        /// 画面操作権限区分名
        /// </summary>
        [Column("screen_sosa_kengen_nm")]
        [StringLength(20)]
        public string ScreenSosaKengenNm { get; set; }

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
