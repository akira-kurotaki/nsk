using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24170_­{ÄÛ¯FèæªÊ]¿îñ
    /// </summary>
    [Serializable]
    [Table("t_24170_­{ÄÛ¯FèæªÊ]¿îñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(¹¯Ê), nameof(­{Û¯Fèæª), nameof(â), nameof(c_²®tO))]
    public class T24170­{ÄÛ¯FèæªÊ]¿îñ : ModelBase
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
        /// ¹¯Ê
        /// </summary>
        [Required]
        [Column("¹¯Ê", Order = 4)]
        [StringLength(3)]
        public string ¹¯Ê { get; set; }

        /// <summary>
        /// ­{Û¯Fèæª
        /// </summary>
        [Required]
        [Column("­{Û¯Fèæª", Order = 5)]
        [StringLength(4)]
        public string ­{Û¯Fèæª { get; set; }

        /// <summary>
        /// â
        /// </summary>
        [Required]
        [Column("â", Order = 6)]
        [StringLength(2)]
        public string â { get; set; }

        /// <summary>
        /// c_²®tO
        /// </summary>
        [Required]
        [Column("c_²®tO", Order = 7)]
        [StringLength(1)]
        public string c_²®tO { get; set; }

        /// <summary>
        /// øóÀË
        /// </summary>
        [Column("øóÀË")]
        public Decimal? øóÀË { get; set; }

        /// <summary>
        /// øóÊÏ
        /// </summary>
        [Column("øóÊÏ")]
        public Decimal? øóÊÏ { get; set; }

        /// <summary>
        /// ¤Ïàz
        /// </summary>
        [Column("¤Ïàz")]
        public Decimal? ¤Ïàz { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_íQgõ
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_íQgõ")]
        public Decimal? ¤Ïàx¥ÎÛ_íQgõ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_øóÊÏ
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_øóÊÏ")]
        public Decimal? ¤Ïàx¥ÎÛ_øóÊÏ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¸ûÊ
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¸ûÊ")]
        public Decimal? ¤Ïàx¥ÎÛ_¸ûÊ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¶YàzÌ¸­z
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¶YàzÌ¸­z")]
        public Decimal? ¤Ïàx¥ÎÛ_¶YàzÌ¸­z { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_íQgõ_êMS¹
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_íQgõ_êMS¹")]
        public Decimal? ¤Ïàx¥ÎÛ_íQgõ_êMS¹ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_íQÊÏ_êMS¹
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_íQÊÏ_êMS¹")]
        public Decimal? ¤Ïàx¥ÎÛ_íQÊÏ_êMS¹ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êMS¹
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êMS¹")]
        public Decimal? ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êMS¹ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_íQgõ_êM¼¹
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_íQgõ_êM¼¹")]
        public Decimal? ¤Ïàx¥ÎÛ_íQgõ_êM¼¹ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_íQÊÏ_êM¼¹
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_íQÊÏ_êM¼¹")]
        public Decimal? ¤Ïàx¥ÎÛ_íQÊÏ_êM¼¹ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êM¼¹
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êM¼¹")]
        public Decimal? ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êM¼¹ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_íQgõ_êMS¼¹v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_íQgõ_êMS¼¹v")]
        public Decimal? ¤Ïàx¥ÎÛ_íQgõ_êMS¼¹v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_íQÊÏ_êMS¼¹v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_íQÊÏ_êMS¼¹v")]
        public Decimal? ¤Ïàx¥ÎÛ_íQÊÏ_êMS¼¹v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êMS¼¹v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êMS¼¹v")]
        public Decimal? ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_êMS¼¹v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_íQgõ_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_íQgõ_v")]
        public Decimal? ¤Ïàx¥ÎÛ_íQgõ_v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_ÊÏ_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_ÊÏ_v")]
        public Decimal? ¤Ïàx¥ÎÛ_ÊÏ_v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¸ûÊ_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¸ûÊ_v")]
        public Decimal? ¤Ïàx¥ÎÛ_¸ûÊ_v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¶YàzÌ¸­z_v")]
        public Decimal? ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_v { get; set; }

        /// <summary>
        /// x¥¤Ïà_x¥¦²®O
        /// </summary>
        [Column("x¥¤Ïà_x¥¦²®O")]
        public Decimal? x¥¤Ïà_x¥¦²®O { get; set; }

        /// <summary>
        /// x¥¤Ïà_x¥¦²®O_êMS¹
        /// </summary>
        [Column("x¥¤Ïà_x¥¦²®O_êMS¹")]
        public Decimal? x¥¤Ïà_x¥¦²®O_êMS¹ { get; set; }

        /// <summary>
        /// x¥¤Ïà_x¥¦²®O_êM¼¹
        /// </summary>
        [Column("x¥¤Ïà_x¥¦²®O_êM¼¹")]
        public Decimal? x¥¤Ïà_x¥¦²®O_êM¼¹ { get; set; }

        /// <summary>
        /// x¥¤Ïà_x¥¦²®O_v
        /// </summary>
        [Column("x¥¤Ïà_x¥¦²®O_v")]
        public Decimal? x¥¤Ïà_x¥¦²®O_v { get; set; }

        /// <summary>
        /// x¥¤Ïà
        /// </summary>
        [Column("x¥¤Ïà")]
        public Decimal? x¥¤Ïà { get; set; }

        /// <summary>
        /// x¥¤Ïà_êMS¹
        /// </summary>
        [Column("x¥¤Ïà_êMS¹")]
        public Decimal? x¥¤Ïà_êMS¹ { get; set; }

        /// <summary>
        /// x¥¤Ïà_êM¼¹
        /// </summary>
        [Column("x¥¤Ïà_êM¼¹")]
        public Decimal? x¥¤Ïà_êM¼¹ { get; set; }

        /// <summary>
        /// x¥¤Ïà_v
        /// </summary>
        [Column("x¥¤Ïà_v")]
        public Decimal? x¥¤Ïà_v { get; set; }

        /// <summary>
        /// ÊíÓC¤Ïàz
        /// </summary>
        [Column("ÊíÓC¤Ïàz")]
        public Decimal? ÊíÓC¤Ïàz { get; set; }

        /// <summary>
        /// _ì¨ÊíÓC¤Ïàz_Áè
        /// </summary>
        [Column("_ì¨ÊíÓC¤Ïàz_Áè")]
        public Decimal? _ì¨ÊíÓC¤Ïàz_Áè { get; set; }

        /// <summary>
        /// ÙíªÛ¯à©z
        /// </summary>
        [Column("ÙíªÛ¯à©z")]
        public Decimal? ÙíªÛ¯à©z { get; set; }

        /// <summary>
        /// ÙíªÛ¯à©z_Áè
        /// </summary>
        [Column("ÙíªÛ¯à©z_Áè")]
        public Decimal? ÙíªÛ¯à©z_Áè { get; set; }

        /// <summary>
        /// _ì¨ÙíÓCÛ¯àz
        /// </summary>
        [Column("_ì¨ÙíÓCÛ¯àz")]
        public Decimal? _ì¨ÙíÓCÛ¯àz { get; set; }

        /// <summary>
        /// ÊíªÛ¯à©z
        /// </summary>
        [Column("ÊíªÛ¯à©z")]
        public Decimal? ÊíªÛ¯à©z { get; set; }

        /// <summary>
        /// x¥Û¯à©z
        /// </summary>
        [Column("x¥Û¯à©z")]
        public Decimal? x¥Û¯à©z { get; set; }

        /// <summary>
        /// x¥ÄÛ¯à©z
        /// </summary>
        [Column("x¥ÄÛ¯à©z")]
        public Decimal? x¥ÄÛ¯à©z { get; set; }

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
    }
}
