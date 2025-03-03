using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 申請お知らせ
    /// </summary>
    [Serializable]
    [Table("t_shinsei_oshirase")]
    [PrimaryKey(nameof(NogyoshaId), nameof(OshiraseDate))]
    public class TShinseiOshirase : ModelBase
    {
        /// <summary>
        /// 農業者ID
        /// </summary>
        [Required]
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// お知らせ日付
        /// </summary>
        [Required]
        //[Key]
        [Column("oshirase_date", Order = 2)]
        public DateTime OshiraseDate { get; set; }

        /// <summary>
        /// 申請お知らせ内容
        /// </summary>
        [Column("shinsei_ohirase_naiyo")]
        [StringLength(1000)]
        public string ShinseiOhiraseNaiyo { get; set; }
    }
}
