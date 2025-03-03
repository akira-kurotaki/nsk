using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// バックアップ対象情報
    /// </summary>
    [Serializable]
    [Table("m_backup_info")]
    [PrimaryKey(nameof(TodofukenCd))]
    public class MBackupInfo
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("todofuken_cd", Order = 1)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// バックアップ対象情報
        /// </summary>
        [Required]
        [Column("backup_info")]
        [StringLength(1024)]
        public string BackupInfo { get; set; }

        /// <summary>
        /// ダンプ出力先パス
        /// </summary>
        [Required]
        [Column("dmp_output_path")]
        [StringLength(300)]
        public string DmpOutputPath { get; set; }

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