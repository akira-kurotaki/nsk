using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// システム接続情報マスタ
    /// </summary>
    [Serializable]
    [Table("m_system_connection")]
    [PrimaryKey(nameof(SystemKbn), nameof(TodofukenCd), nameof(KumiaitoCd), nameof(ShishoCd))]
    public class MSystemConnection : ModelBase
    {
        /// <summary>
        /// システム区分
        /// </summary>
        [Required]
        [Column("system_kbn", Order = 1)]
        [StringLength(2)]
        public string SystemKbn { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd", Order = 2)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("kumiaito_cd", Order = 3)]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Required]
        [Column("shisho_cd", Order = 4)]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// DB接続情報
        /// </summary>
        [Required]
        [Column("connection_string")]
        [StringLength(1000)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// デフォルトスキーマ
        /// </summary>
        [Required]
        [Column("default_schema")]
        [StringLength(63)]
        public string DefaultSchema { get; set; }

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
