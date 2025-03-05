using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F502.Models.D502010
{
    /// <summary>
    /// 農作物共済加入状況データ取込画面項目モデル
    /// </summary>
    [Serializable]
    public class D502010TableRecord
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的
        /// </summary>
        [Display(Name = "共済目的")]
        public string 共済目的 { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Display(Name = "年産")]
        public int 年産 { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        [Display(Name = "引受回")]
        public int 引受回 { get; set; }

        /// <summary>
        /// 入力済み耕地筆数
        /// </summary>
        [Numeric]
        public Decimal? 入力済み耕地筆数 { get; set; }

        /// <summary>
        /// 組合員等毎設定
        /// </summary>
        [Display(Name = "組合員等毎設定")]
        public string 組合員等毎設定 { get; set; }

        /// <summary>
        /// 類区分毎設定
        /// </summary>
        [Display(Name = "類区分毎設定")]
        public string 類区分毎設定 { get; set; }

        /// <summary>
        /// 料率設定
        /// </summary>
        [Display(Name = "料率設定")]
        public string 料率設定 { get; set; }

        /// <summary>
        /// 引受計算実施日
        /// </summary>
        [Display(Name = "引受計算実施日")]
        public DateTime? 引受計算実施日 { get; set; }
    }
}
