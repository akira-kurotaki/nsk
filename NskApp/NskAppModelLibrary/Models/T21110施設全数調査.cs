using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21110_施設全数調査
    /// </summary>
    [Serializable]
    [Table("t_21110_施設全数調査")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(用途区分), nameof(組合員等コード))]
    public class T21110施設全数調査 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 1)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("年産", Order = 2)]
        public short 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 3)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 4)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        [Required]
        [Column("用途区分", Order = 5)]
        [StringLength(3)]
        public string 用途区分 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 6)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 施設搬入収穫量
        /// </summary>
        [Column("施設搬入収穫量")]
        public Decimal? 施設搬入収穫量 { get; set; }

        /// <summary>
        /// 分割減収量
        /// </summary>
        [Column("分割減収量")]
        public Decimal? 分割減収量 { get; set; }

        /// <summary>
        /// 皆無面積
        /// </summary>
        [Column("皆無面積")]
        public Decimal? 皆無面積 { get; set; }

        /// <summary>
        /// 皆無基準収穫量
        /// </summary>
        [Column("皆無基準収穫量")]
        public Decimal? 皆無基準収穫量 { get; set; }

        /// <summary>
        /// 不能面積
        /// </summary>
        [Column("不能面積")]
        public Decimal? 不能面積 { get; set; }

        /// <summary>
        /// 不能基準収穫量
        /// </summary>
        [Column("不能基準収穫量")]
        public Decimal? 不能基準収穫量 { get; set; }

        /// <summary>
        /// 不能収穫量
        /// </summary>
        [Column("不能収穫量")]
        public Decimal? 不能収穫量 { get; set; }

        /// <summary>
        /// 転作等面積
        /// </summary>
        [Column("転作等面積")]
        public Decimal? 転作等面積 { get; set; }

        /// <summary>
        /// 転作等基準収穫量
        /// </summary>
        [Column("転作等基準収穫量")]
        public Decimal? 転作等基準収穫量 { get; set; }

        /// <summary>
        /// 転作等収穫量
        /// </summary>
        [Column("転作等収穫量")]
        public Decimal? 転作等収穫量 { get; set; }

        /// <summary>
        /// 用途別基準収穫量
        /// </summary>
        [Column("用途別基準収穫量")]
        public Decimal? 用途別基準収穫量 { get; set; }

        /// <summary>
        /// 不能耕地搬入収穫量
        /// </summary>
        [Column("不能耕地搬入収穫量")]
        public Decimal? 不能耕地搬入収穫量 { get; set; }

        /// <summary>
        /// 不能耕地搬入収穫量補正量
        /// </summary>
        [Column("不能耕地搬入収穫量補正量")]
        public Decimal? 不能耕地搬入収穫量補正量 { get; set; }

        /// <summary>
        /// 組合員等収穫量補正量
        /// </summary>
        [Column("組合員等収穫量補正量")]
        public Decimal? 組合員等収穫量補正量 { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("登録日時")]
        public DateTime? 登録日時 { get; set; }

        /// <summary>
        /// 登録ユーザid
        /// </summary>
        [Column("登録ユーザid")]
        [StringLength(11)]
        public string 登録ユーザid { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("更新日時")]
        public DateTime? 更新日時 { get; set; }

        /// <summary>
        /// 更新ユーザid
        /// </summary>
        [Column("更新ユーザid")]
        [StringLength(11)]
        public string 更新ユーザid { get; set; }
    }
}
