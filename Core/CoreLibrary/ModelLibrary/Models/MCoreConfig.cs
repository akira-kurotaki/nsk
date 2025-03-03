using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// システム設定値マスタ
    /// </summary>
    [Serializable]
    [Table("m_core_config")]
    public class MCoreConfig : ModelBase
    {
        /// <summary>
        /// 検索キー
        /// </summary>
        [Required]
        [Key]
        [Column("search_key", Order = 1)]
        [StringLength(10)]
        public string SearchKey { get; set; }

        /// <summary>
        /// 設定値
        /// </summary>
        [Required]
        [Column("config_value")]
        [StringLength(100)]
        public string ConfigValue { get; set; }

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
