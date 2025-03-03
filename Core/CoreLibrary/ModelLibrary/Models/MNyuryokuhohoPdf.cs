using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 入力方法PDFマスタ
    /// </summary>
    [Serializable]
    [Table("m_nyuryokuhoho_pdf")]
    [PrimaryKey(nameof(ScreenId), nameof(TekiyoStartYmd))]
    public class MNyuryokuhohoPdf : ModelBase
    {
        /// <summary>
        /// 画面ID
        /// </summary>
        [Required]
        [Column("screen_id", Order = 1)]
        [StringLength(9)]
        public string ScreenId { get; set; }

        /// <summary>
        /// 適用開始年月日
        /// </summary>
        [Required]
        [Column("tekiyo_start_ymd", Order = 2)]
        public DateTime TekiyoStartYmd { get; set; }

        /// <summary>
        /// 適用終了年月日
        /// </summary>
        [Column("tekiyo_end_ymd")]
        public DateTime? TekiyoEndYmd { get; set; }

        /// <summary>
        /// PDFファイルパス
        /// </summary>
        [Column("pdf_file_path")]
        public string PdfFilePath { get; set; }

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
