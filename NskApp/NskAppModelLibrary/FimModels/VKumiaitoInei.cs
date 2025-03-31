using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 組合等印影
    /// </summary>
    [Serializable]
    [Table("v_kumiaito_inei")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(IneiNo))]
    public class VKumiaitoInei : ModelBase
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
        /// 印影番号
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("inei_no", Order = 3)]
        public int IneiNo { get; set; }

        /// <summary>
        /// 印影種別
        /// </summary>
        [Column("inei_sbt")]
        [StringLength(2)]
        public string IneiSbt { get; set; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        [Column("file_path")]
        public string FilePath { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        [Column("file_nm")]
        [StringLength(100)]
        public string FileNm { get; set; }

        /// <summary>
        /// ファイルサイズ
        /// </summary>
        [Column("file_size")]
        public int? FileSize { get; set; }

        /// <summary>
        /// ハッシュ値
        /// </summary>
        [Column("hash")]
        [StringLength(64)]
        public string Hash { get; set; }

        /// <summary>
        /// 印影備考
        /// </summary>
        [Column("inei_biko")]
        [StringLength(300)]
        public string IneiBiko { get; set; }

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
