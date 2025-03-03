using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 汎用区分マスタ
    /// </summary>
    [Serializable]
    [Table("m_hanyokubun")]
    [PrimaryKey(nameof(KbnSbt), nameof(KbnCd))]
    public class MHanyokubun : ModelBase
    {
        /// <summary>
        /// 区分種別
        /// </summary>
        [Required]
        [Column("kbn_sbt", Order = 1)]
        [StringLength(100)]
        public string KbnSbt { get; set; }

        /// <summary>
        /// 区分コード
        /// </summary>
        [Required]
        [Column("kbn_cd", Order = 2)]
        [StringLength(3)]
        public string KbnCd { get; set; }

        /// <summary>
        /// 区分名称
        /// </summary>
        [Column("kbn_nm")]
        [StringLength(40)]
        public string KbnNm { get; set; }

        /// <summary>
        /// 区分略称
        /// </summary>
        [Column("kbn_rnm")]
        [StringLength(20)]
        public string KbnRnm { get; set; }

        /// <summary>
        /// ソート順
        /// </summary>
        [Column("sort")]
        public short? Sort { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Required]
        [Column("delete_flg")]
        [StringLength(1)]
        public string DeleteFlg { get; set; }

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
