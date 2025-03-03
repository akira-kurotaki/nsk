using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Common.NskMenu
{
    /// <summary>
    /// メニュー構成情報
    /// </summary>
    [Serializable]
    public class NskMenuStruct
    {
        /// <summary>
        /// 第1階層メニューグループ
        /// </summary>
        [Column("第1階層メニューグループ")]
        public string 第1階層メニューグループ { get; set; }

        /// <summary>
        /// メニューレベル
        /// </summary>
        [Column("lvl")]
        public int MenuLevel { get; set; }


        /// <summary>
        /// ソートキー
        /// </summary>
        [Column("sortkey")]
        public string SortKey { get; set; }

        /// <summary>
        /// メニューID
        /// </summary>
        [Column("メニューID")]
        public string メニューID { get; set; }

        /// <summary>
        /// メニュー名
        /// </summary>
        [Column("メニュー名称")]
        public string メニュー名称 { get; set; }

        /// <summary>
        /// 親メニューID
        /// </summary>
        [Column("親メニューid")]
        public string 親メニューid { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Column("共済目的コード")]
        public string? 共済目的コード { get; set; }

        /// <summary>
        /// 画面ID
        /// </summary>
        [Column("画面id")]
        public string 画面id { get; set; }

        /// <summary>
        /// 処理ID
        /// </summary>
        [Column("処理id")]
        public string 処理id { get; set; }
    }
}
