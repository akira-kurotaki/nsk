using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24180_­{ÄÛ¯FèæªÞæªÊ¹Q]¿îñ
    /// </summary>
    [Serializable]
    [Table("t_24180_­{ÄÛ¯FèæªÞæªÊ¹Q]¿îñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(¹¯Ê), nameof(¿ñ), nameof(­{Û¯Fèæª), nameof(â), nameof(Þæª), nameof(c_²®tO))]
    public class T24180­{ÄÛ¯FèæªÞæªÊ¹Q]¿îñ : ModelBase
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
        /// ¿ñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("¿ñ", Order = 5)]
        public short ¿ñ { get; set; }

        /// <summary>
        /// ­{Û¯Fèæª
        /// </summary>
        [Required]
        [Column("­{Û¯Fèæª", Order = 6)]
        [StringLength(4)]
        public string ­{Û¯Fèæª { get; set; }

        /// <summary>
        /// â
        /// </summary>
        [Required]
        [Column("â", Order = 7)]
        [StringLength(2)]
        public string â { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 8)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// c_²®tO
        /// </summary>
        [Required]
        [Column("c_²®tO", Order = 9)]
        [StringLength(1)]
        public string c_²®tO { get; set; }

        /// <summary>
        /// øóË
        /// </summary>
        [Column("øóË")]
        public Decimal? øóË { get; set; }

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
        /// îûnÊ
        /// </summary>
        [Column("îûnÊ")]
        public Decimal? îûnÊ { get; set; }

        /// <summary>
        /// î¶Yàz
        /// </summary>
        [Column("î¶Yàz")]
        public Decimal? î¶Yàz { get; set; }

        /// <summary>
        /// ¤ÏÀxz
        /// </summary>
        [Column("¤ÏÀxz")]
        public Decimal? ¤ÏÀxz { get; set; }

        /// <summary>
        /// x¥¤Ïà_x¥¦²®O
        /// </summary>
        [Column("x¥¤Ïà_x¥¦²®O")]
        public Decimal? x¥¤Ïà_x¥¦²®O { get; set; }

        /// <summary>
        /// x¥¤Ïà_x¥¦²®O_à´ßíQ
        /// </summary>
        [Column("x¥¤Ïà_x¥¦²®O_à´ßíQ")]
        public Decimal? x¥¤Ïà_x¥¦²®O_à´ßíQ { get; set; }

        /// <summary>
        /// x¥¤Ïà_x¥¦²®O_àêMS¹
        /// </summary>
        [Column("x¥¤Ïà_x¥¦²®O_àêMS¹")]
        public Decimal? x¥¤Ïà_x¥¦²®O_àêMS¹ { get; set; }

        /// <summary>
        /// x¥¤Ïà_x¥¦²®O_àêM¼¹
        /// </summary>
        [Column("x¥¤Ïà_x¥¦²®O_àêM¼¹")]
        public Decimal? x¥¤Ïà_x¥¦²®O_àêM¼¹ { get; set; }

        /// <summary>
        /// x¥¤Ïà
        /// </summary>
        [Column("x¥¤Ïà")]
        public Decimal? x¥¤Ïà { get; set; }

        /// <summary>
        /// x¥¤Ïà_à´ßíQ
        /// </summary>
        [Column("x¥¤Ïà_à´ßíQ")]
        public Decimal? x¥¤Ïà_à´ßíQ { get; set; }

        /// <summary>
        /// x¥¤Ïà_àêMS¹
        /// </summary>
        [Column("x¥¤Ïà_àêMS¹")]
        public Decimal? x¥¤Ïà_àêMS¹ { get; set; }

        /// <summary>
        /// x¥¤Ïà_àêM¼¹
        /// </summary>
        [Column("x¥¤Ïà_àêM¼¹")]
        public Decimal? x¥¤Ïà_àêM¼¹ { get; set; }

        /// <summary>
        /// ¤Ïàz
        /// </summary>
        [Column("¤Ïàz")]
        public Decimal? ¤Ïàz { get; set; }

        /// <summary>
        /// ÊíÓC¤Ïàz
        /// </summary>
        [Column("ÊíÓC¤Ïàz")]
        public Decimal? ÊíÓC¤Ïàz { get; set; }

        /// <summary>
        /// ÊíÓCÛ¯à
        /// </summary>
        [Column("ÊíÓCÛ¯à")]
        public Decimal? ÊíÓCÛ¯à { get; set; }

        /// <summary>
        /// _ì¨ÊíWíQ¦
        /// </summary>
        [Column("_ì¨ÊíWíQ¦")]
        public Decimal? _ì¨ÊíWíQ¦ { get; set; }

        /// <summary>
        /// _ì¨ÙíÓCÛ¯àz
        /// </summary>
        [Column("_ì¨ÙíÓCÛ¯àz")]
        public Decimal? _ì¨ÙíÓCÛ¯àz { get; set; }

        /// <summary>
        /// x¥Û¯à
        /// </summary>
        [Column("x¥Û¯à")]
        public Decimal? x¥Û¯à { get; set; }

        /// <summary>
        /// Àx¥¤Ïà
        /// </summary>
        [Column("Àx¥¤Ïà")]
        public Decimal? Àx¥¤Ïà { get; set; }

        /// <summary>
        /// Àx¥¤Ïà_à´ßíQ
        /// </summary>
        [Column("Àx¥¤Ïà_à´ßíQ")]
        public Decimal? Àx¥¤Ïà_à´ßíQ { get; set; }

        /// <summary>
        /// Àx¥¤Ïà_àêMS¹
        /// </summary>
        [Column("Àx¥¤Ïà_àêMS¹")]
        public Decimal? Àx¥¤Ïà_àêMS¹ { get; set; }

        /// <summary>
        /// Àx¥¤Ïà_àêM¼¹
        /// </summary>
        [Column("Àx¥¤Ïà_àêM¼¹")]
        public Decimal? Àx¥¤Ïà_àêM¼¹ { get; set; }

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
        /// ¤Ïàx¥ÎÛ_îûnÊ
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_îûnÊ")]
        public Decimal? ¤Ïàx¥ÎÛ_îûnÊ { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¤ÏÀxz
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¤ÏÀxz")]
        public Decimal? ¤Ïàx¥ÎÛ_¤ÏÀxz { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_î¶Yàz
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_î¶Yàz")]
        public Decimal? ¤Ïàx¥ÎÛ_î¶Yàz { get; set; }

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
        /// ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¶YàzÌ¸­z_v")]
        public Decimal? ¤Ïàx¥ÎÛ_¶YàzÌ¸­z_v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_øóÊÏ_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_øóÊÏ_v")]
        public Decimal? ¤Ïàx¥ÎÛ_øóÊÏ_v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_îûnÊ_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_îûnÊ_v")]
        public Decimal? ¤Ïàx¥ÎÛ_îûnÊ_v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_î¶Yàz_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_î¶Yàz_v")]
        public Decimal? ¤Ïàx¥ÎÛ_î¶Yàz_v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¤ÏÀxz_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¤ÏÀxz_v")]
        public Decimal? ¤Ïàx¥ÎÛ_¤ÏÀxz_v { get; set; }

        /// <summary>
        /// ¤Ïàx¥ÎÛ_¸ûÊ_v
        /// </summary>
        [Column("¤Ïàx¥ÎÛ_¸ûÊ_v")]
        public Decimal? ¤Ïàx¥ÎÛ_¸ûÊ_v { get; set; }

        /// <summary>
        /// x¥Û¯àùóÌz
        /// </summary>
        [Column("x¥Û¯àùóÌz")]
        public Decimal? x¥Û¯àùóÌz { get; set; }

        /// <summary>
        /// x¥Û¯à¡ñ¿z
        /// </summary>
        [Column("x¥Û¯à¡ñ¿z")]
        public Decimal? x¥Û¯à¡ñ¿z { get; set; }

        /// <summary>
        /// x¥ÄÛ¯à
        /// </summary>
        [Column("x¥ÄÛ¯à")]
        public Decimal? x¥ÄÛ¯à { get; set; }

        /// <summary>
        /// àzíQ¦
        /// </summary>
        [Column("àzíQ¦")]
        public Decimal? àzíQ¦ { get; set; }

        /// <summary>
        /// ÆÓË
        /// </summary>
        [Column("ÆÓË")]
        public Decimal? ÆÓË { get; set; }

        /// <summary>
        /// ÆÓz
        /// </summary>
        [Column("ÆÓz")]
        public Decimal? ÆÓz { get; set; }

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
