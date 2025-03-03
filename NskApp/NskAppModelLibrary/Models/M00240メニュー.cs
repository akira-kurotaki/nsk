using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00240_メニュー
    /// </summary>
    [Serializable]
    [Table("m_00240_メニュー")]
    public class M00240メニュー : ModelBase
    {
        /// <summary>
        /// メニューid
        /// </summary>
        [Required]
        [Key]
        [Column("メニューid", Order = 1)]
        [StringLength(9)]
        public string メニューid { get; set; }

        /// <summary>
        /// メニュー名称
        /// </summary>
        [Column("メニュー名称")]
        [StringLength(30)]
        public string メニュー名称 { get; set; }

        /// <summary>
        /// 親メニューid
        /// </summary>
        [Column("親メニューid")]
        [StringLength(9)]
        public string 親メニューid { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Column("共済目的コード")]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 画面id
        /// </summary>
        [Column("画面id")]
        [StringLength(9)]
        public string 画面id { get; set; }

        /// <summary>
        /// 処理id
        /// </summary>
        [Column("処理id")]
        [StringLength(30)]
        public string 処理id { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("並び順")]
        public Decimal? 並び順 { get; set; }

        /// <summary>
        /// 第1階層メニューグループ
        /// </summary>
        [Column("第1階層メニューグループ")]
        [StringLength(2)]
        public string 第1階層メニューグループ { get; set; }

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
    }
}
