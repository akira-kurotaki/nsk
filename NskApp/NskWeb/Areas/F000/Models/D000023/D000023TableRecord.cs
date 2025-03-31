using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000023
{
    /// <summary>
    /// 統計単位地域コード検索(子画面)（検索結果取得）
    /// </summary>
    [Serializable]
    public class D000023TableRecord
    {
        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Display(Name = "統計単位地域コード")]
        public string TokeiTaniChikiCd { get; set; }

        /// <summary>
        /// 統計単位地域名称
        /// </summary>
        [Display(Name = "統計単位地域名称")]
        public string TokeiTaniChikiNm { get; set; }
    }
}
