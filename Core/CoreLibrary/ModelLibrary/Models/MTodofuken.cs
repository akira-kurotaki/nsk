using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 都道府県マスタ
    /// </summary>
    [Serializable]
    [Table("m_todofuken")]
    public class MTodofuken : ModelBase
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Key]
        [Column("todofuken_cd", Order = 1)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        [Column("todofuken_nm")]
        [StringLength(10)]
        public string TodofukenNm { get; set; }

        /// <summary>
        /// 地域ブロックコード
        /// </summary>
        [Column("area_block_cd")]
        [StringLength(2)]
        public string AreaBlockCd { get; set; }

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
