using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// バッチダウンロードファイル
    /// </summary>
    [Serializable]
    [Table("t_batch_download_file")]
    [PrimaryKey(nameof(BatchId), nameof(Renban))]
    public class TBatchDownloadFile : ModelBase
    {
        /// <summary>
        /// バッチID
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("batch_id", Order = 1)]
        public long BatchId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("renban", Order = 2)]
        public short Renban { get; set; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        [Required]
        [Column("file_path")]
        [StringLength(256)]
        public string FilePath { get; set; }

        /// <summary>
        /// ハッシュ値
        /// </summary>
        [Required]
        [Column("hash")]
        [StringLength(64)]
        public string Hash { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        [Required]
        [Column("file_nm")]
        [StringLength(100)]
        public string FileNm { get; set; }

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
