using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// APIパスフレーズマスタ
    /// </summary>
    [Serializable]
    [Table("m_api_passphrase")]
    [PrimaryKey(nameof(SystemKbn), nameof(ShoriNm), nameof(TodofukenCd))]
    public class MApiPassphrase : ModelBase
    {
        /// <summary>
        /// システム区分
        /// </summary>
        [Required]
        [Column("system_kbn", Order = 1)]
        [StringLength(2)]
        public string SystemKbn { get; set; }

        /// <summary>
        /// 処理名
        /// </summary>
        [Required]
        [Column("shori_nm", Order = 2)]
        [StringLength(30)]
        public string ShoriNm { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd", Order = 3)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// パスフレーズ
        /// </summary>
        [Required]
        [Column("passphrase")]
        [StringLength(100)]
        public string Passphrase { get; set; }

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
