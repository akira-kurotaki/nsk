using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10270_ÛàÚ×
    /// </summary>
    [Serializable]
    [Table("m_10270_ÛàÚ×")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(Þæª), nameof(Ûàøóû®), nameof(Áñæª), nameof(ånæR[h), nameof(N))]
    public class M10270ÛàÚ× : ModelBase
    {
        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("gR[h", Order = 1)]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("NY", Order = 2)]
        public short NY { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 3)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 4)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// Ûàøóû®
        /// </summary>
        [Required]
        [Column("Ûàøóû®", Order = 5)]
        [StringLength(2)]
        public string Ûàøóû® { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Required]
        [Column("Áñæª", Order = 6)]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// ånæR[h
        /// </summary>
        [Required]
        [Column("ånæR[h", Order = 7)]
        [StringLength(2)]
        public string ånæR[h { get; set; }

        /// <summary>
        /// N
        /// </summary>
        [Required]
        [Column("N", Order = 8)]
        [StringLength(2)]
        public string N { get; set; }

        /// <summary>
        /// ÊÏÛP¿_êÊ
        /// </summary>
        [Column("ÊÏÛP¿_êÊ")]
        public Decimal? ÊÏÛP¿_êÊ { get; set; }

        /// <summary>
        /// ÊÏÛP¿_hÐ
        /// </summary>
        [Column("ÊÏÛP¿_hÐ")]
        public Decimal? ÊÏÛP¿_hÐ { get; set; }

        /// <summary>
        /// ÊÏÛP¿_ÁÊ
        /// </summary>
        [Column("ÊÏÛP¿_ÁÊ")]
        public Decimal? ÊÏÛP¿_ÁÊ { get; set; }

        /// <summary>
        /// ÛÀxÊÏ
        /// </summary>
        [Column("ÛÀxÊÏ")]
        public Decimal? ÛÀxÊÏ { get; set; }

        /// <summary>
        /// ûÊÛP¿_êÊ
        /// </summary>
        [Column("ûÊÛP¿_êÊ")]
        public Decimal? ûÊÛP¿_êÊ { get; set; }

        /// <summary>
        /// ûÊÛP¿_hÐ
        /// </summary>
        [Column("ûÊÛP¿_hÐ")]
        public Decimal? ûÊÛP¿_hÐ { get; set; }

        /// <summary>
        /// ûÊÛP¿_ÁÊ
        /// </summary>
        [Column("ûÊÛP¿_ÁÊ")]
        public Decimal? ûÊÛP¿_ÁÊ { get; set; }

        /// <summary>
        /// ÛÀxûÊ
        /// </summary>
        [Column("ÛÀxûÊ")]
        public Decimal? ÛÀxûÊ { get; set; }

        /// <summary>
        /// î¶YàzÛP¿_êÊ
        /// </summary>
        [Column("î¶YàzÛP¿_êÊ")]
        public Decimal? î¶YàzÛP¿_êÊ { get; set; }

        /// <summary>
        /// î¶YàzÛP¿_hÐ
        /// </summary>
        [Column("î¶YàzÛP¿_hÐ")]
        public Decimal? î¶YàzÛP¿_hÐ { get; set; }

        /// <summary>
        /// î¶YàzÛP¿_ÁÊ
        /// </summary>
        [Column("î¶YàzÛP¿_ÁÊ")]
        public Decimal? î¶YàzÛP¿_ÁÊ { get; set; }

        /// <summary>
        /// ÛÀxî¶Yàz
        /// </summary>
        [Column("ÛÀxî¶Yàz")]
        public Decimal? ÛÀxî¶Yàz { get; set; }

        /// <summary>
        /// àzÛP¿_êÊ
        /// </summary>
        [Column("àzÛP¿_êÊ")]
        public Decimal? àzÛP¿_êÊ { get; set; }

        /// <summary>
        /// àzÛP¿_hÐ
        /// </summary>
        [Column("àzÛP¿_hÐ")]
        public Decimal? àzÛP¿_hÐ { get; set; }

        /// <summary>
        /// àzÛP¿_ÁÊ
        /// </summary>
        [Column("àzÛP¿_ÁÊ")]
        public Decimal? àzÛP¿_ÁÊ { get; set; }

        /// <summary>
        /// ÛÀx¤Ïàz
        /// </summary>
        [Column("ÛÀx¤Ïàz")]
        public Decimal? ÛÀx¤Ïàz { get; set; }

        /// <summary>
        /// [R[h
        /// </summary>
        [Column("[R[h")]
        [StringLength(1)]
        public string [R[h { get; set; }

        /// <summary>
        /// o^ú
        /// </summary>
        [Column("o^ú")]
        public DateTime? o^ú { get; set; }

        /// <summary>
        /// o^[Uid
        /// </summary>
        [Column("o^[Uid")]
        [StringLength(11)]
        public string o^[Uid { get; set; }

        /// <summary>
        /// XVú
        /// </summary>
        [Column("XVú")]
        public DateTime? XVú { get; set; }

        /// <summary>
        /// XV[Uid
        /// </summary>
        [Column("XV[Uid")]
        [StringLength(11)]
        public string XV[Uid { get; set; }
    }
}
