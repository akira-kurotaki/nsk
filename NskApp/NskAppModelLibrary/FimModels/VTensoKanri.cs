using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 転送管理
    /// </summary>
    [Serializable]
    [Table("v_tenso_kanri")]
    [PrimaryKey(nameof(FileId), nameof(KyosaiJigyoCd), nameof(TensoCd), nameof(FileNo))]
    public class VTensoKanri : ModelBase
    {
        /// <summary>
        /// ファイルID
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("file_id", Order = 1)]
        public int FileId { get; set; }

        /// <summary>
        /// 共済事業コード
        /// </summary>
        [Required]
        [Column("kyosai_jigyo_cd", Order = 2)]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// 転送区分コード
        /// </summary>
        [Required]
        [Column("tenso_cd", Order = 3)]
        [StringLength(3)]
        public string TensoCd { get; set; }

        /// <summary>
        /// ファイル番号
        /// </summary>
        [Required]
        [Column("file_no", Order = 4)]
        [StringLength(2)]
        public string FileNo { get; set; }

        /// <summary>
        /// 転送元ファイルフルパス
        /// </summary>
        [Column("tensomoto_file_fullpath")]
        public string TensomotoFileFullpath { get; set; }

        /// <summary>
        /// 転送先ファイルフルパス
        /// </summary>
        [Column("tensosaki_file_fullpath")]
        public string TensosakiFileFullpath { get; set; }

        /// <summary>
        /// 転送先パス
        /// </summary>
        [Column("tensosaki_path")]
        public string TensosakiPath { get; set; }

        /// <summary>
        /// 転送備考
        /// </summary>
        [Column("tenso_biko")]
        [StringLength(200)]
        public string TensoBiko { get; set; }

        /// <summary>
        /// 転送結果ステータス
        /// </summary>
        [Column("tenso_kekka_sts")]
        [StringLength(1)]
        public string TensoKekkaSts { get; set; }

        /// <summary>
        /// 転送結果記号
        /// </summary>
        [Column("tenso_kekka_kigo")]
        [StringLength(4)]
        public string TensoKekkaKigo { get; set; }

        /// <summary>
        /// データ転送日時
        /// </summary>
        [Column("tenso_date")]
        public DateTime? TensoDate { get; set; }

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
