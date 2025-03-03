using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// 取込対象マスタ
    /// </summary>
    [Serializable]
    [Table("m_torikomi")]
    public class MTorikomi : ModelBase
    {
        /// <summary>
        /// 取込対象コード
        /// </summary>
        [Required]
        [Key]
        [Column("torikomi_cd", Order = 1)]
        [StringLength(2)]
        public string TorikomiCd { get; set; }

        /// <summary>
        /// 取込対象名
        /// </summary>
        [Column("torikomi_nm")]
        [StringLength(30)]
        public string TorikomiNm { get; set; }

        /// <summary>
        /// ロック種類
        /// </summary>
        [Column("lock_type")]
        [StringLength(1)]
        public string LockType { get; set; }

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
