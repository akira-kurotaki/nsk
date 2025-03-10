using NskReport.Base;
using NskReportMain.Common;
using NskReportMain.Controllers;
using CoreLibrary.Core.Dto;
using Microsoft.AspNetCore.Mvc;
using NskReportLibrary.Core.Consts;
using ReportService.Core;

namespace NskReport.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class P103040Controller : ReportControllerBase
    {
        /// <summary>
        /// 帳票出力処理（P103040_基準統計単収一覧）
        /// </summary>
        /// <param name="request">帳票出力条件</param>
        /// <returns>リアル帳票出力サービスの実行結果</returns>
        [HttpPost]
        public ActionResult<ReportResult> CreateReport(ReportRequest request)
        {
            string reportControlId = request.reportControlId;
            string userId = request.userId;
            // TODO: NSKが使用するバッチ条件テーブルのバッチ条件IDは文字列型
            string joukenId = request.joukenId;
            string todofukenCd = request.todofukenCd;
            string kumiaitoCd = request.kumiaitoCd;
            string shishoCd = request.shishoCd;
            // TODO: 以下の2つはクライアント側の条件にない。要追加
            string filePath = request.filePath;
            long batchId  = request.batchId;

            // 開始ログ出力
            WriteStartLog(request);

            // 戻り値
            ReportResult result = new ReportResult();

            // 引数チェック
            result = CheckCommonRequest(request, result);
            if (result.Result != ReportConst.RESULT_SUCCESS)
            {
                return Ok(result);
            }

            // 引数チェック
            DbConnectionInfo dbInfo = CheckDbConnectionInfo(request, ref result);
            if (result.Result != ReportConst.RESULT_SUCCESS)
            {
                return Ok(result);
            }

            // 該当帳票出力制御クラスを呼び出す
            // ■ 帳票出力制御クラスは以下の戻り値を返却する
            // ・処理結果（正常：0、エラー：1）
            // ・エラーメッセージ
            // ・帳票データ（メモリストリーム）

            // 帳票作成の実行結果
            var controllerResult = new ControllerResult();

            using (var reportController = new NskReportP103040Controller(dbInfo))
            {
                controllerResult = reportController.ManageReports(userId, joukenId, todofukenCd, kumiaitoCd, shishoCd, filePath, batchId);
            }

            // 戻り値を設定する
            result = SetResult(request, controllerResult, result);

            // 終了ログ出力
            WriteEndLog(request);

            return result;
        }
    }
}
