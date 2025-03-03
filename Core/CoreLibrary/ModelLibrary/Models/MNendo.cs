using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 年度マスタ
    /// </summary>
    [Serializable]
    [Table("m_nendo")]
    public class MNendo : ModelBase
    {
        /// <summary>
        /// 年度
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nendo", Order = 1)]
        public short Nendo { get; set; }

        /// <summary>
        /// 年度表示1
        /// </summary>
        [Column("nendo_disp1")]
        [StringLength(30)]
        public string NendoDisp1 { get; set; }

        /// <summary>
        /// 年度表示2
        /// </summary>
        [Column("nendo_disp2")]
        [StringLength(30)]
        public string NendoDisp2 { get; set; }

        /// <summary>
        /// 年度表示3
        /// </summary>
        [Column("nendo_disp3")]
        [StringLength(30)]
        public string NendoDisp3 { get; set; }

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
