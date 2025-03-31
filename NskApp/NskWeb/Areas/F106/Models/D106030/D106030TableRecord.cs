using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F106.Models.D106030
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（引受回一覧データ）
    /// </summary>
    [Serializable]
    public class D106030TableRecord
    {
        /// <summary>
        /// 支所コード
        /// </summary>
        [Display(Name = "支所コード")]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 支所名
        /// </summary>
        [Display(Name = "支所名")]
        public string ShishoNm { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        [Display(Name = "引受回")]
        public int? HikiukeCnt { get; set; }

        /// <summary>
        /// 確定引受回
        /// </summary>
        [Display(Name = "確定引受回")]
        public int? KakuteiHikiukeCnt { get; set; }

        /// <summary>
        /// 報告回
        /// </summary>
        [Display(Name = "報告回")]
        public int? HoukokuCnt { get; set; }

        /// <summary>
        /// 引受計算実施日
        /// </summary>
        [Display(Name = "引受計算実施日")]
        public DateTime? HikiukeKeisanDate { get; set; }
    }
}
