using CoreLibrary.Core.Utility;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using System.IO;

namespace NskReportMain.Common
{
    /// <summary>
    /// Controllerの実行結果
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/07
    /// 作成者：KAN RI
    /// </remarks>
    public class ControllerResult
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
        public MemoryStream ReportData { get; set; }
    }

    /// <summary>
    /// Controllerの実行結果の拡張機能
    /// </summary>
    public static class ControllerResultExtensions
    {
        /// <summary>
        /// 引数チェックエラーの実行結果を作成する。
        /// </summary>
        /// <param name="result">CreatorResult インスタンス</param>
        /// <param name="errorMessageId">メッセージID</param>
        /// <param name="args">メッセージパラメータ</param>
        /// <returns>エラーリザルト</returns>
        public static ControllerResult ControllerResultError(this ControllerResult result, string errorMessge, string errorMessageId, params string[] args)
        {
            result.Result = ReportConst.RESULT_FAILED;
            result.ErrorMessageId = errorMessageId;
            result.ErrorMessage = !string.IsNullOrEmpty(errorMessge) ? errorMessge : 
                (!string.IsNullOrEmpty(errorMessageId) ? MessageUtil.Get(errorMessageId, args) : string.Empty);
            return result;
        }
    }
}
