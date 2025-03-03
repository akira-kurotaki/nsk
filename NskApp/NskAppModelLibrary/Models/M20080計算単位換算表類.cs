using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20080_計算単位換算表類
    /// </summary>
    [Serializable]
    [Table("m_20080_計算単位換算表類")]
    [Keyless]
    public class M20080計算単位換算表類 : ModelBase
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Column("共済目的コード")]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 計算単位類区分
        /// </summary>
        [Column("計算単位類区分")]
        [StringLength(2)]
        public string 計算単位類区分 { get; set; }

        /// <summary>
        /// 計算単位類名称
        /// </summary>
        [Column("計算単位類名称")]
        [StringLength(20)]
        public string 計算単位類名称 { get; set; }

        /// <summary>
        /// ソートキー
        /// </summary>
        [Column("ソートキー")]
        public short? ソートキー { get; set; }

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
