using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 名称M支所
    /// </summary>
    [Serializable]
    [Table("v_shisho_nm")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(ShishoCd))]
    public class VShishoNm
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
        /// 支所コード
        /// </summary>
        [Required]
        [Column("shisho_cd", Order = 3)]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 支所名
        /// </summary>
        [Column("shisho_nm")]
        [StringLength(10)]
        public string ShishoNm { get; set; }

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
