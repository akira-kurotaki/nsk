using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 利用支所グループマスタ（仮）
    /// </summary>
    [Serializable]
    [Table("m_riyo_shisho_group")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(RiyoShishoGroup))]
    public class MRiyoShishoGroup : ModelBase
    {
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
        /// 利用支所グループ
        /// </summary>
        [Required]
        [Column("riyo_shisho_group")]
        [StringLength(10)]
        public string RiyoShishoGroup { get; set; }

        /// <summary>
        /// 利用支所グループ名
        /// </summary>
        [Required]
        [Column("riyo_shisho_group_nm")]
        [StringLength(10)]
        public string RiyoShishoGroupNm { get; set; }

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
