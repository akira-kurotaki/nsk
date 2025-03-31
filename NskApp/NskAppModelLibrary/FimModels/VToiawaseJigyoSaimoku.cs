using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 問合せ事業細目マスタ
    /// </summary>
    [Serializable]
    [Table("v_toiawase_jigyo_saimoku")]
    [PrimaryKey(nameof(JigyotoCd), nameof(JigyoSaimokuCd))]
    public class VToiawaseJigyoSaimoku : ModelBase
    {
        /// <summary>
        /// 事業等コード
        /// </summary>
        [Required]
        [Column("jigyoto_cd", Order = 1)]
        [StringLength(3)]
        public string JigyotoCd { get; set; }

        /// <summary>
        /// 事業細目コード
        /// </summary>
        [Required]
        [Column("jigyo_saimoku_cd", Order = 2)]
        [StringLength(4)]
        public string JigyoSaimokuCd { get; set; }

        /// <summary>
        /// 事業細目名
        /// </summary>
        [Column("jigyo_saimoku_nm")]
        [StringLength(50)]
        public string JigyoSaimokuNm { get; set; }

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
