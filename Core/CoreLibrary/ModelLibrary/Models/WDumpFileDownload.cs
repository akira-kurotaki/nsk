using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 一時ダンプファイル取得テーブルのエンティティクラス。
    /// </summary>
    [Serializable]
    [Table("w_dumpfile_download")]
    [PrimaryKey(nameof(Token), nameof(Renban))]
    public class WDumpFileDownload
    {
        /// <summary>
        /// トークン 
        /// </summary>
        [Column("token")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(24)]
        public string Token { get; set; }

        /// <summary>
        /// 連番 
        /// </summary>
        [Column("renban")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public short Renban { get; set; }

        /// <summary>
        /// ダンプファイルパス
        /// </summary>
        [Column("dmpfile_path")]
        [Required]
        [StringLength(330)]
        public string DmpfilePath { get; set; }

        /// <summary>
        /// ダウンロードフラグ
        /// </summary>
        [Column("download_flg")]
        [Required]
        [StringLength(1)]
        public string DownloadFlg { get; set; }

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