using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000021
{
    /// <summary>
    /// 品種コード検索(子画面)（検索結果取得）
    /// </summary>
    [Serializable]
    public class D000021TableRecord
    {
        /// <summary>
        /// 品種コード
        /// </summary>
        [Display(Name = "品種コード")]
        public string HinshuCd { get; set; }

        /// <summary>
        /// 品種名称
        /// </summary>
        [Display(Name = "品種名称")]
        public string HinshuNm { get; set; }
    }
}
