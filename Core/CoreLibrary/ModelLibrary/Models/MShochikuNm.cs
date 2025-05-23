using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 名称M小地区
    /// </summary>
    [Serializable]
    [Table("m_shochiku_nm")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(DaichikuCd), nameof(ShochikuCd))]
    public class MShochikuNm : ModelBase
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
        /// 大地区コード
        /// </summary>
        [Required]
        [Column("daichiku_cd", Order = 3)]
        [StringLength(2)]
        public string DaichikuCd { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        [Required]
        [Column("shochiku_cd", Order = 4)]
        [StringLength(4)]
        public string ShochikuCd { get; set; }

        /// <summary>
        /// 小地区名
        /// </summary>
        [Column("shochiku_nm")]
        [StringLength(10)]
        public string ShochikuNm { get; set; }

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
