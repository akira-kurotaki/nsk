using CoreLibrary.Core.Utility;
using GrapeCity.ActiveReports;
using NskReportLibrary.Core.Consts;

namespace NskReportMain.Common
{
    /// <summary>
    /// Creatorの実行結果
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/07
    /// 作成者：KAN RI
    /// </remarks>
    public class CreatorResult
    {
        /// <summary>
        /// 処理結果（正常：0、エラー：1）
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// エラーメッセージId
        /// </summary>
        public string ErrorMessageId { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 帳票データ
        /// </summary>
        public SectionReport SectionReport { get; set; }

        /// <summary>
        /// 帳票インスタンスのメモリを解放するメソッド
        /// </summary>
        public void DisposeReportMemory()
        {
            // レポートインスタンスをDisposeする
            if (SectionReport != null)
            {
                SectionReport.Document.Dispose();
                SectionReport.Dispose();
                SectionReport = null;
            }
        }
    }

    /// <summary>
    /// 実行結果を作成する拡張機能
    /// </summary>
    public static class CreatorResultExtensions
    {
        /// <summary>
        /// 引数チェックエラーの実行結果を作成する。
        /// </summary>
        /// <param name="result">CreatorResult インスタンス</param>
        /// <param name="errorMessageId">メッセージID</param>
        /// <param name="args">メッセージパラメータ</param>
        /// <returns>エラーリザルト</returns>
        public static CreatorResult CreateResultError(this CreatorResult result, string errorMessageId, params string[] args)
        {
            result.Result = ReportConst.RESULT_FAILED;
            result.ErrorMessageId = errorMessageId;
            result.ErrorMessage = MessageUtil.Get(errorMessageId, args);
            return result;
        }
    }
}
