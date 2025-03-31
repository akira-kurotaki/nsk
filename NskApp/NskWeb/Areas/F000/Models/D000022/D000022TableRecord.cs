using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000022
{
    /// <summary>
    /// 産地別銘柄コード検索(子画面)（検索結果取得）
    /// </summary>
    [Serializable]
    public class D000022TableRecord
    {
        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Display(Name = "産地別銘柄コード")]
        public string SanchiMeigaraCd { get; set; }

        /// <summary>
        /// 産地別銘柄名称
        /// </summary>
        [Display(Name = "産地別銘柄名称")]
        public string SanchiMeigaraNm { get; set; }
    }
}
