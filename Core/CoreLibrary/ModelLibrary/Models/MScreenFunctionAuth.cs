using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    [Serializable]
    [Table("m_screen_function_auth")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(JigyoCd), nameof(RiyoKbnCd))]
    public class MScreenFunctionAuth : ModelBase
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd")]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("kumiaito_cd")]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 事業コード
        /// </summary>
        [Required]
        [Column("jigyo_cd")]
        [StringLength(10)]
        public string JigyoCd { get; set; }

        /// <summary>
        /// 利用者区分コード
        /// </summary>
        [Required]
        [Column("riyo_kbn_cd")]
        [StringLength(10)]
        public string RiyoKbnCd { get; set; }

        /// <summary>
        /// 画面機能コード
        /// </summary>
        [Required]
        [Column("function_id")]
        [StringLength(10)]
        public string FunctionId { get; set; }

        /// <summary>
        /// 参照フラグ
        /// </summary>
        [Required]
        [Column("feference_flg")]
        [StringLength(1)]
        public string FeferenceFlg { get; set; }

        /// <summary>
        /// 更新フラグ
        /// </summary>
        [Required]
        [Column("update_flg")]
        [StringLength(1)]
        public string UpdateFlg { get; set; }

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
