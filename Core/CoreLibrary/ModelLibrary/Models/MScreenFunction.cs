using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 画面機能マスタ（仮）
    /// </summary>
    [Serializable]
    [Table("m_screen_function")]
    [PrimaryKey(nameof(JigyoCd), nameof(FunctionId))]
    public class MScreenFunction : ModelBase
    {
        /// <summary>
        /// 事業コード
        /// </summary>
        [Required]
        [Column("jigyo_cd")]
        [StringLength(10)]
        public string JigyoCd { get; set; }

        /// <summary>
        /// 画面機能コード
        /// </summary>
        [Required]
        [Column("function_id")]
        [StringLength(10)]
        public string FunctionId { get; set; }

        /// <summary>
        /// 画面機能名
        /// </summary>
        [Required]
        [Column("function_nm")]
        [StringLength(100)]
        public string FunctionNm { get; set; }

        /// <summary>
        /// 利用者区分コード
        /// </summary>
        [Required]
        [Column("riyo_kbn_cd")]
        [StringLength(10)]
        public string RiyoKbnCd { get; set; }

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
