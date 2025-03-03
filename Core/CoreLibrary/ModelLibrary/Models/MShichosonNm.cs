using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 名称M市町村
    /// </summary>
    [Serializable]
    [Table("m_shichoson_nm")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(ShichosonCd))]
    public class MShichosonNm : ModelBase
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
        /// 市町村コード
        /// </summary>
        [Required]
        [Column("shichoson_cd", Order = 3)]
        [StringLength(5)]
        public string ShichosonCd { get; set; }

        /// <summary>
        /// 市町村名
        /// </summary>
        [Column("shichoson_nm")]
        [StringLength(10)]
        public string ShichosonNm { get; set; }

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
