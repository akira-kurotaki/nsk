using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 支所グループ詳細マスタ
    /// </summary>
    [Serializable]
    [Table("v_shisho_group_shosai")]
    [PrimaryKey(nameof(ShishoGroupId), nameof(ShishoCd))]
    public class VShishoGroupShosai
    {
        /// <summary>
        /// 支所グループID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("shisho_group_id", Order = 1)]
        public int ShishoGroupId { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Required]
        [Column("shisho_cd", Order = 2)]
        [StringLength(2)]
        public string ShishoCd { get; set; }

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
