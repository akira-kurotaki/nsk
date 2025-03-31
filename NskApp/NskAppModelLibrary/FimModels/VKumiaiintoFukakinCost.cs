using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 組合員等割賦課金単価マスタ
    /// </summary>
    [Serializable]
    [Table("v_kumiaiinto_fukakin_cost")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(DaichikuCd))]
    public class VKumiaiintoFukakinCost : ModelBase
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
        /// 適用年月日前
        /// </summary>
        [Column("tekiyo_ymd_before")]
        public long? TekiyoYmdBefore { get; set; }

        /// <summary>
        /// 適用年月日後
        /// </summary>
        [Column("tekiyo_ymd_after")]
        public long? TekiyoYmdAfter { get; set; }

        /// <summary>
        /// 適用年月日
        /// </summary>
        [Column("tekiyo_ymd")]
        public DateTime? TekiyoYmd { get; set; }

        /// <summary>
        /// 処理年月日
        /// </summary>
        [Column("shori_ymd")]
        [StringLength(10)]
        public string ShoriYmd { get; set; }

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
