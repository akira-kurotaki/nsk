using CoreLibrary.Core.Pager;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F106.Models.D106010
{
    /// <summary>
    /// 引受計算処理（水稲）画面項目モデル（検索結果部分）
    /// </summary>
    [Serializable]
    public class D106010SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D106010SearchResult()
        {
            TableRecords = new List<D106010TableRecord>();
            EnterCtrlFlg = "0";
        }

        /// <summary>
        /// 引受回一覧
        /// </summary>
        public List<D106010TableRecord> TableRecords { get; set; }

        /// <summary>
        /// 引受回全件数
        /// </summary>
        public int TotalCount { get; set; }
    
        /// <summary>
        /// 実行ボタン制御フラグ
        /// </summary>
        public string EnterCtrlFlg { get; set; }
    }
}
