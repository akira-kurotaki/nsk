using NskReportMain.Common;
using NskReportMain.Models.P0041;
using NskReportMain.ReportCreators.P0041;
using CoreLibrary.Core.Dto;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document.Section;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;

namespace NskReportMain.Controllers
{
    public class NskReport0001Controller : ReportController
    {
        #region メンバー定数
        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "H004_申請の新規・継続用制御処理";
        #endregion

        public NskReport0001Controller(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        #region 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="batchFlag">バッチフラグ（リアルタイム帳票：0、バッチ帳票：1）</param>
        /// <param name="filePath">帳票パス</param>
        /// <returns>実行結果</returns>
        public ControllerResult ManageReports(string userId, int joukenId, int batchFlag, string filePath)
        {
            // ログ出力(開始)
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CONTROLLER,
                joukenId,
                LOGGER_INFO_STR,
                string.Join(ReportConst.PARAM_SEPARATOR, new string[]{
                        ReportConst.PARAM_NAME_USER_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + userId,
                        ReportConst.PARAM_NAME_JOUKEN_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + joukenId,
                        ReportConst.PARAM_NAME_BATCH_FLAG + ReportConst.PARAM_NAME_VALUE_SEPARATOR + batchFlag,
                        ReportConst.PARAM_NAME_FILE_PATH + ReportConst.PARAM_NAME_VALUE_SEPARATOR + filePath })));

            // 実行結果
            var result = new ControllerResult();
            try
            {
                int keiyakuId = 9999;

                // 全帳票オブジェクト
                var report = new SectionReport();

                P0041Model p0041Model = new P0041Model();
                result = CreateP0041(new List<P0041Model>() { p0041Model }, keiyakuId, joukenId, ref report);
                if (result.Result == ReportConst.RESULT_FAILED)
                {
                    return result;
                }

                // 処理結果を返す
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    pdfExport.Export(report.Document, memoryStream);

                    result.Result = ReportConst.RESULT_SUCCESS;
                    result.ReportData = memoryStream;
                }

            }
            catch (Exception)
            {
                // 法定帳票などと制御クラスの構造を統一するため、ここで例外をキャッチする
                throw;
            }

            // ログ出力(終了)
            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CONTROLLER,
                joukenId,
                LOGGER_INFO_STR));

            return result;
        }
        #endregion

        #region 「P0041_加入申請書※R6年度以降用」を作成するメソッド
        /// <summary>
        /// 「P0041_加入申請書※R6年度以降用」を作成するメソッド
        /// </summary>
        /// <param name="model">帳票モデルリスト</param>
        /// <param name="keiyakuId">契約ID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="report">帳票オブジェクト</param>
        /// <returns>実行結果</returns>
        private ControllerResult CreateP0041(
            List<P0041Model> model,
            int keiyakuId,
            int joukenId,
            ref SectionReport report)
        {
            // 実行結果
            var result = new ControllerResult();

            P0041Creator creator = new P0041Creator();
            //CreatorResult creatorResult = creator.CreateReport(joukenId, model, ReportConst.PRINT_PATTERN_REGIST_COPY_KANYUSHAHIKAE_NONE);
            CreatorResult creatorResult = creator.CreateReport(joukenId, model, "3");
            if (creatorResult.Result == ReportConst.RESULT_FAILED)
            {
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = creatorResult.ErrorMessageId;
                result.ErrorMessage = creatorResult.ErrorMessage;
                return result;
            }
            //report.Document.Pages.AddRange(creatorResult.SectionReport.Document.Pages.Clone() as PagesCollection);
            report.Document.Pages.AddRange(creatorResult.SectionReport.Document.Pages);

            return result;
        }
        #endregion

    }
}