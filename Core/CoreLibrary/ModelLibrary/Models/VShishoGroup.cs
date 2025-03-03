using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 支所グループマスタ
    /// </summary>
    [Serializable]
    [Table("v_shisho_group")]
    public class VShishoGroup
    {
        /// <summary>
        /// 支所グループID
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("shisho_group_id", Order = 1)]
        public int ShishoGroupId { get; set; }

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
        /// 支所グループコード
        /// </summary>
        [Required]
        [Column("shisho_group_cd")]
        [StringLength(4)]
        public string ShishoGroupCd { get; set; }

        /// <summary>
        /// 支所グループ名
        /// </summary>
        [Column("shisho_group_nm")]
        [StringLength(20)]
        public string ShishoGroupNm { get; set; }

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
