using NskReportMain.Common;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NskReportLibrary.Core.Consts;
using ReportService.Core;

namespace NskReport.Base
{
    /// <summary>
    /// リアルタイム帳票REST APIのコントローラーの基底クラス
    /// </summary>
    [ApiController]
    public abstract class ReportControllerBase : ControllerBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        protected Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 開始ログを出力する
        /// </summary>
        /// <param name="request"></param>
        protected void WriteStartLog(ReportRequest request)
        {
            // 接続元ユーザ
            HttpContext.Items["loginuser"] = request.userId;

            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_REPORT_OUTPUT,
                request.joukenId,
                string.Empty,
                string.Join(ReportConst.PARAM_SEPARATOR, new string[]{
                        ReportConst.PARAM_NAME_REPORT_CONTROL_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.reportControlId,
                        ReportConst.PARAM_NAME_USER_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.userId,
                        ReportConst.PARAM_NAME_JOUKEN_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.joukenId,
                        ReportConst.PARAM_NAME_TODOFUKEN_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.todofukenCd,
                        ReportConst.PARAM_NAME_KUMIAITO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.kumiaitoCd,
                        ReportConst.PARAM_NAME_SHISHO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.shishoCd})));
        }

        /// <summary>
        /// 終了ログを出力する
        /// </summary>
        /// <param name="request"></param>
        protected void WriteEndLog(ReportRequest request)
        {
            logger.Info(string.Format(ReportConst.METHOD_END_LOG, ReportConst.CLASS_NM_REPORT_OUTPUT, request.joukenId, request.reportControlId));
        }

        /// <summary>
        /// 共通チェック処理
        /// チェックエラーの場合は、復帰値のReportResult.ResultにReportConst.RESULT_FAILEDを設定する
        /// </summary>
        /// <param name="request"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected ReportResult CheckCommonRequest(ReportRequest request, ReportResult result)
        {
            string reportControlId = request.reportControlId;
            string userId = request.userId;
            int joukenId = request.joukenId;
            string todofukenCd = request.todofukenCd;
            string kumiaitoCd = request.kumiaitoCd;
            string shishoCd = request.shishoCd;

            // 引数チェックする
            // 帳票制御IDが未入力の場合、エラーとし、処理を終了する
            if (string.IsNullOrEmpty(reportControlId))
            {
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = "ME90009";
                result.ErrorMessage = MessageUtil.Get("ME90009", ReportConst.PARAM_NAME_REPORT_CONTROL_ID);
                logger.Error(string.Format(ReportConst.ERROR_LOG, joukenId, result.ErrorMessage));
                return result;
            }

            // ユーザIDが未入力の場合、エラーとし、処理を終了する
            if (string.IsNullOrEmpty(userId))
            {
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = "ME90009";
                result.ErrorMessage = MessageUtil.Get("ME90009", ReportConst.PARAM_NAME_USER_ID);
                logger.Error(string.Format(ReportConst.ERROR_LOG, joukenId, result.ErrorMessage));
                return result;
            }

            // 条件IDが未入力の場合、エラーとし、処理を終了する
            if (joukenId == 0)
            {
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = "ME90009";
                result.ErrorMessage = MessageUtil.Get("ME90009", ReportConst.PARAM_NAME_JOUKEN_ID);
                logger.Error(string.Format(ReportConst.ERROR_LOG, joukenId, result.ErrorMessage));
                return result;
            }

            // 都道府県コードが未入力の場合、エラーとし、処理を終了する
            if (string.IsNullOrEmpty(todofukenCd))
            {
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = "ME90009";
                result.ErrorMessage = MessageUtil.Get("ME90009", ReportConst.PARAM_NAME_TODOFUKEN_CD);
                logger.Error(string.Format(ReportConst.ERROR_LOG, joukenId, result.ErrorMessage));
                return result;
            }

            return result;
        }

        /// <summary>
        /// システム共通スキーマから所属に応じた都道府県別事業スキーマのDB接続先を取得する
        /// 取得に失敗した場合は、ReportResult.ResultにReportConst.RESULT_FAILEDを設定する
        /// </summary>
        /// <param name="request"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected DbConnectionInfo CheckDbConnectionInfo(ReportRequest request, ref ReportResult result)
        {
            // システム区分
            string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);

            // システム共通スキーマから所属に応じた都道府県別事業スキーマのDB接続先を取得する
            DbConnectionInfo dbInfo = DBUtil.GetDbConnectionInfo(systemKbn, request.todofukenCd, request.kumiaitoCd, request.shishoCd);
            if (dbInfo == null)
            {
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = "ME00055";
                result.ErrorMessage = MessageUtil.Get("ME01054", "システム接続情報",
                    string.Join(ReportConst.PARAM_SEPARATOR, new string[]{
                                    ReportConst.PARAM_NAME_TODOFUKEN_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.todofukenCd,
                                    ReportConst.PARAM_NAME_KUMIAITO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.kumiaitoCd,
                                    ReportConst.PARAM_NAME_SHISHO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + request.shishoCd}));
                logger.Error(string.Format(ReportConst.ERROR_LOG, request.joukenId, result.ErrorMessage));
            }
            return dbInfo;
        }

        /// <summary>
        /// リアル帳票出力サービスの戻り値を設定する
        /// </summary>
        /// <param name="request"></param>
        /// <param name="controllerResult"></param>
        /// <param name="result"></param>
        protected ReportResult SetResult(ReportRequest request, ControllerResult controllerResult, ReportResult result)
        {
            if (controllerResult.Result == ReportConst.RESULT_SUCCESS)
            {
                logger.Error(string.Format(ReportConst.SUCCESS_LOG, "条件ID：" + request.joukenId, "処理結果（0：成功、1：失敗）：" + controllerResult.Result));
                result.Result = ReportConst.RESULT_SUCCESS;
                result.ReportData = controllerResult.ReportData.ToArray();
            }
            else
            {
                logger.Error(string.Format(ReportConst.ERROR_LOG, "条件ID：" + request.joukenId, "処理結果（0：成功、1：失敗）：" + controllerResult.Result + ReportConst.SYMBOL_TOUTEN + controllerResult.ErrorMessage));
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = controllerResult.ErrorMessageId;
                result.ErrorMessage = controllerResult.ErrorMessage;
            }

            return result;
        }
    }
}
