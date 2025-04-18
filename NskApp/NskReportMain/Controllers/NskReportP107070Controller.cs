using NskAppModelLibrary.Context;
using NskReportMain.Common;
using NskReportMain.Models.NSK_P107070;
using NskReportMain.ReportCreators.NSK_P107070;
using NskReportMain.Reports;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportLibrary.Core.Utility;
using System.Text;
using JoukenNameConst = NskCommonLibrary.Core.Consts.JoukenNameConst;
using NskCommonUtil = NskCommonLibrary.Core.Utility;
using CoreLibrary.Core.Consts;
using NskAppModelLibrary.Models;

namespace NskReportMain.Controllers
{
    public class NskReportP107070Controller : ReportController
    {

        #region メンバー定数
        /// <summary>
        /// 振込引落区分コード「現金」
        /// </summary>
        private static readonly string HIKIOTOSHI_KBN_CD_CASH = "4";

        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "P107070_徴収管理簿_制御処理";

        /// <summary>
        /// 条件名値：現金徴収のみ「有」
        /// </summary>
        public static readonly string JOUKEN_GENKIN_CHOSHU_ARI = "1";

        #endregion

        public NskReportP107070Controller(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        #region P107070_徴収管理簿_制御処理
        /// <summary>
        /// P107070_徴収管理簿_制御処理
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="batchId">バッチID</param>
        /// <returns>実行結果</returns>
        public ControllerResult ManageReports(string userId, string joukenId, string todofukenCd, string kumiaitoCd, string shishoCd, long? batchId)
        {
            //ファイル出力先（出力毎の可変部分）
            DateTime sysDate = DateUtil.GetSysDateTime();
            string filePath = batchId + ReportConst.SYMBOL_UNDERSCORE + sysDate.ToString("yyyyMMddHHmmss");

            // ログ出力(開始)
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CONTROLLER,
                joukenId,
                LOGGER_INFO_STR,
                string.Join(ReportConst.PARAM_SEPARATOR, new string[]{
                        ReportConst.PARAM_NAME_USER_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + userId,
                        ReportConst.PARAM_NAME_JOUKEN_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + joukenId,
                        ReportConst.PARAM_NAME_TODOFUKEN_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + todofukenCd,
                        ReportConst.PARAM_NAME_KUMIAITO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + kumiaitoCd,
                        ReportConst.PARAM_NAME_SHISHO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + shishoCd,
                        ReportConst.PARAM_NAME_FILE_PATH + ReportConst.PARAM_NAME_VALUE_SEPARATOR + filePath,
                        ReportConst.PARAM_NAME_BATCH_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + (batchId == null ? string.Empty : batchId.ToString())})));

            // 実行結果
            var result = new ControllerResult();
            try
            {
                // 引数チェック
                result = CheckParams(result, userId, joukenId, todofukenCd, kumiaitoCd);
                if (ReportConst.RESULT_FAILED.Equals(result.Result))
                {
                    return result;
                }

                // 帳票出力条件を取得する。
                // 条件ID、条件名称リストをキーにバッチ条件テーブルより帳票出力条件を取得する。
                // 取得結果が0件の場合はエラーとし、エラーメッセージを返す。
                List<string> jokenNames =
                [
                    JoukenNameConst.JOUKEN_NENSAN,                        // 年産
                    JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,            // 共済目的コード
                    JoukenNameConst.JOUKEN_DAICHIKU,                      // 大地区コード
                    JoukenNameConst.JOUKEN_SHOCHIKU_START,                // 小地区コード（開始）
                    JoukenNameConst.JOUKEN_SHOCHIKU_END,                  // 小地区コード（終了）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,           // 組合員等コード（開始）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,             // 組合員等コード（終了）
                    JoukenNameConst.JOUKEN_GENKIN_CHOSHU,                 // 現金徴収のみ
                    JoukenNameConst.JOUKEN_REPORT_NAME                    // 帳票名
                ];

                var reportJoukens = getJigyoDb<NskAppContext>().T01050バッチ条件s
                    .Where(t => t.バッチ条件id == joukenId && jokenNames.Contains(t.条件名称))
                    .ToList();
                if (reportJoukens.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME01645", "パラメータの取得");
                }

                // 取得したデータから作成対象とする帳票データの集約単位リストを取得する。
                // 取得結果が0件の場合は、エラーとし、エラーメッセージを返す。
                var pModel = GetReportDataList(reportJoukens, kumiaitoCd, shishoCd);
                if (pModel.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME10076", "0");
                }
                // 帳票PDF一時出力フォルダ作成
                // 定数取得(（定数ID：PrintTempFolder）)\バッチID_システム日時(yyyyMMddHHmmss)
                var printTempFolder = NskCommonUtil.FolderUtil.CreateTempFolder(sysDate, batchId.ToString());

                // 帳票オブジェクト
                var report = new BaseSectionReport();
                // 帳票出力処理を呼び出す。
                result = CreateP107070(reportJoukens, pModel, joukenId, kumiaitoCd, ref report);
                if (result.Result == ReportConst.RESULT_SUCCESS)
                {
                    // ファイル名はバッチ条件テーブルの帳票名 + ".pdf"とする。
                    string reportName = string.Empty;
                    var jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_REPORT_NAME).SingleOrDefault();
                    if (!string.IsNullOrEmpty(jouken?.条件値))
                    {
                        reportName = jouken.条件値;
                    }
                    pdfExport.Export(report.Document, Path.Combine(printTempFolder, reportName + ReportConst.REPORT_EXTENSION));

                    // PDF出力用一時フォルダをzip化（暗号化）する。
                    var zipFilePath = ZipUtil.CreateZip(printTempFolder);

                    // Zipファイルを引数：帳票パスに移動する
                    var toPath = Path.Combine((ConfigUtil.Get(CoreConst.REPORT_OUTPUT_FOLDER) ?? string.Empty), filePath);
                    NskCommonUtil.FolderUtil.MoveFile(zipFilePath, toPath, userId, (batchId ?? 0));

                    // PDF出力用一時フォルダを削除する
                    NskCommonUtil.FolderUtil.DeleteTempFolder(printTempFolder);
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

        #region 「P107070_徴収管理簿」を作成するメソッド
        /// <summary>
        /// 「P107070_徴収管理簿」を作成するメソッド
        /// </summary>
        /// <param name="reportJoukens">T01050バッチ条件リスト</param>
        /// <param name="model">帳票モデルリスト</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="report">帳票オブジェクト</param>
        /// <returns>実行結果</returns>
        private ControllerResult CreateP107070(
            List<T01050バッチ条件> reportJoukens,
            List<NSK_P107070Model> model,
            string joukenId,
            string kumiaitoCd,
            ref BaseSectionReport report)
        {
            // 実行結果
            var result = new ControllerResult();
            NSK_P107070Creator pCreator = new NSK_P107070Creator();
            CreatorResult creatorResult = pCreator.CreateReport(joukenId, kumiaitoCd, reportJoukens, model);
            // 失敗した場合
            if (creatorResult.Result == ReportConst.RESULT_FAILED)
            {
                return result.ControllerResultError(creatorResult.ErrorMessage, creatorResult.ErrorMessageId);
            }

            // ページ番号を描画する
            creatorResult.SectionReport = ReportPagerUtil.DrawReportPageNumber(creatorResult.SectionReport, ReportConst.REPORT_BOTTOM_MARGIN_STANDARD);
            // 出力レポートにP107070_徴収管理簿を入れる。
            report.Document.Pages.AddRange(creatorResult.SectionReport.Document.Pages);

            return result;
        }
        #endregion

        #region 引数チェック
        /// <summary>
        /// 引数チェック
        /// </summary>
        /// <param name="result">実行結果</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>実行結果</returns>
        private ControllerResult CheckParams(ControllerResult result, string userId, string joukenId, string todofukenCd, string kumiaitoCd)
        {
            // ■１．引数チェックする
            // ユーザIDがないの場合、エラーとし、エラーメッセージを返す
            if (string.IsNullOrEmpty(userId))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_USER_ID);
            }

            // 条件IDがないの場合、エラーとし、エラーメッセージを返す
            if (string.IsNullOrEmpty(joukenId))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_JOUKEN_ID);
            }

            // 都道府県コードがない場合、エラーとし、エラーメッセージを返す。
            if (string.IsNullOrEmpty(todofukenCd))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_TODOFUKEN_CD);
            }

            // 組合等コードがない場合、エラーとし、エラーメッセージを返す。
            if (string.IsNullOrEmpty(kumiaitoCd))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_KUMIAITO_CD);
            }

            return result;
        }
        #endregion

        #region 帳票データ取得メソッド
        /// <summary>
        /// 作成対象とする帳票データを取得する。
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>帳票データ</returns>
        private List<NSK_P107070Model> GetReportDataList(List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            //WITH句を取得する
            GetWith(sql);
            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetReportDataTableCondition(sql);
            // 検索条件を取得する
            GetQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters);
           // ソート順を追加する
            GetOrderby(sql);

            logger.Info("作成対象とする帳票データを取得する。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<NSK_P107070Model>(sql.ToString(), parameters.ToArray()).ToList();
            
        }
        #endregion

        #region WITH句取得
        /// <summary>
        /// WITH句を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetWith(StringBuilder sql)
        {
            sql.Append("WITH HJ AS ( ");
            sql.Append("    SELECT");
            sql.Append("        t_12040_組合員等別引受情報.*");
            sql.Append("    FROM");
            sql.Append("        t_12040_組合員等別引受情報");
            sql.Append("    INNER JOIN (");
            sql.Append("        SELECT ");
            sql.Append("            t_00010_引受回.組合等コード");
            sql.Append("            , t_00010_引受回.年産");
            sql.Append("            , t_00010_引受回.共済目的コード");
            sql.Append("            , t_00010_引受回.支所コード");
            sql.Append("            , MAX(t_00010_引受回.引受回) AS \"引受回\" ");
            sql.Append("        FROM");
            sql.Append("            t_00010_引受回");
            sql.Append("        GROUP BY ");
            sql.Append("            t_00010_引受回.組合等コード");
            sql.Append("            , t_00010_引受回.年産");
            sql.Append("            , t_00010_引受回.共済目的コード");
            sql.Append("            , t_00010_引受回.支所コード");
            sql.Append("        ) HK ");
            sql.Append("    ON  t_12040_組合員等別引受情報.組合等コード = HK.組合等コード");
            sql.Append("    AND t_12040_組合員等別引受情報.年産 = HK.年産");
            sql.Append("    AND t_12040_組合員等別引受情報.共済目的コード = HK.共済目的コード");
            sql.Append("    AND t_12040_組合員等別引受情報.支所コード = HK.支所コード");
            sql.Append("    AND t_12040_組合員等別引受情報.引受回 = HK.引受回");
            sql.Append("    WHERE t_12040_組合員等別引受情報.類区分 = '0' ");
            sql.Append("    AND   t_12040_組合員等別引受情報.統計単位地域コード = '0' ");
            sql.Append(") ");
        }
        #endregion

        #region 検索項目取得
        /// <summary>
        /// 検索項目を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetCondition(StringBuilder sql)
        {
            sql.Append("SELECT ");
            sql.Append("    m_00010_共済目的名称.共済目的名称 AS \"共済目的名称\" ");
            sql.Append("    , v_kumiaito.kumiaito_nm AS \"組合等正式名称\" ");
            sql.Append("    , v_daichiku_nm.daichiku_cd AS \"大地区コード\" ");
            sql.Append("    , v_daichiku_nm.daichiku_nm AS \"大地区名\" ");
            sql.Append("    , v_shochiku_nm.shochiku_cd AS \"小地区コード\" ");
            sql.Append("    , v_shochiku_nm.shochiku_nm AS \"小地区名\" ");
            sql.Append("    , HJ.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , v_nogyosha.hojin_full_nm AS \"氏名又は法人名\" ");
            sql.Append("    , COALESCE(HJ.組合員等負担共済掛金, 0) AS \"組合員等負担共済掛金\" ");
            sql.Append("    , COALESCE(HJ.一般賦課金, 0) AS \"一般賦課金\" ");
            sql.Append("    , COALESCE(HJ.組合員等割, 0) AS \"組合員等割\" ");
            sql.Append("    , COALESCE(HJ.特別賦課金, 0) AS \"特別賦課金\" ");
            sql.Append("    , COALESCE(HJ.防災賦課金, 0) AS \"防災賦課金\" ");
            sql.Append("    , COALESCE(HJ.賦課金計, 0) AS \"賦課金計\" ");
            sql.Append("    , COALESCE(HJ.納入額, 0) AS \"納入額\" ");
            sql.Append("    , COALESCE(CJ.前回迄徴収額, 0) AS \"前回迄徴収額\" ");
            sql.Append("    , COALESCE(CJ.今回迄徴収額, 0) AS \"今回迄徴収額\" ");
            sql.Append("    , COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) AS \"前回迄引受解除徴収賦課金額\" ");
            sql.Append("    , COALESCE(HJ.賦課金計, 0) - (COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0)) AS \"賦課金計差額\" ");
            sql.Append("    , CASE WHEN");
            sql.Append("              HJ.解除フラグ = '1'");
            sql.Append("            THEN");
            sql.Append("              COALESCE(HJ.引受解除返還賦課金額, 0) - (COALESCE(HJ.賦課金計, 0) - (COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0))) ");   //t_12040_組合員等別引受情報.引受解除返還賦課金額 -  賦課金計差額
            sql.Append("            ELSE");
            sql.Append("              0");
            sql.Append("      END AS \"今回引受解除徴収賦課金額\" ");
            sql.Append("    , COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) ");    //CJ.前回迄引受解除徴収賦課金額 + 今回引受解除徴収賦課金額
            sql.Append("          + CASE WHEN");
            sql.Append("                    HJ.解除フラグ = '1'");
            sql.Append("                  THEN");
            sql.Append("                    COALESCE(HJ.引受解除返還賦課金額, 0) - (COALESCE(HJ.賦課金計, 0) - (COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0))) ");   //t_12040_組合員等別引受情報.引受解除返還賦課金額 -  賦課金計差額
            sql.Append("                  ELSE");
            sql.Append("                    0");
            sql.Append("             END ");
            sql.Append("       AS \"今回迄引受解除徴収賦課金額\" ");
            sql.Append("    , t_12090_組合員等別徴収情報.徴収年月日 AS \"徴収年月日\" ");
            sql.Append("    , v_nogyosha_kinyukikan.furikomi_hikiotoshi_cd AS \"振込引落区分コード\" ");
            sql.Append("    , v_hanyokubun.kbn_nm AS \"徴収区分\" ");
            sql.Append("    , HJ.解除フラグ AS \"解除フラグ\" ");
            //以下は帳票出力用項目、設定は帳票作成クラスで実施
            sql.Append("    , '' AS \"年産_表示\" ");
            sql.Append("    , ''  AS \"日付_表示\" ");
            sql.Append("    , ''  AS \"組合等コード_表示\" ");
            sql.Append("    , 0  AS \"特別防災賦課金\" ");
            sql.Append("    , 0  AS \"徴収済額\" ");
            sql.Append("    , 0  AS \"今回徴収額\" ");
            sql.Append("    , 0  AS \"還付額\" ");
            sql.Append("    , '' AS \"徴収年月日_表示\" ");
            sql.Append("    , 0  AS \"未納額\" ");
            sql.Append("    , 0  AS \"TOTAL徴収額\" ");
            sql.Append("    , '' AS \"備考\" ");
        }
        #endregion

        #region 検索テーブル取得
        /// <summary>
        /// 検索テーブルを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetReportDataTableCondition(StringBuilder sql)
        {
            sql.Append("FROM ");
            sql.Append("    HJ ");
            sql.Append("LEFT JOIN ( ");
            sql.Append("    SELECT ");
            sql.Append("        t_12090_組合員等別徴収情報.組合等コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.年産 ");
            sql.Append("        , t_12090_組合員等別徴収情報.共済目的コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.組合員等コード ");
            sql.Append("        , MAX(t_12090_組合員等別徴収情報.引受回) AS \"徴収引受回\" ");
            sql.Append("        , SUM(t_12090_組合員等別徴収情報.徴収金額) AS \"今回迄徴収額\" ");
            sql.Append("        , SUM(CASE WHEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.引受回 < HJ.引受回 ");
            sql.Append("                   THEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.徴収金額 ");
            sql.Append("                   ELSE 0 END) AS \"前回迄徴収額\" ");
            sql.Append("        , SUM(CASE WHEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.引受回 < HJ.引受回 ");
            sql.Append("                   THEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.内賦課金 ");
            sql.Append("                   ELSE 0 END) AS \"前回迄内賦課金\" ");
            sql.Append("        , SUM(CASE WHEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.引受回 < HJ.引受回 ");
            sql.Append("                   THEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.引受解除徴収賦課金額 ");
            sql.Append("                   ELSE 0 END) AS \"前回迄引受解除徴収賦課金額\" ");
            sql.Append("    FROM t_12090_組合員等別徴収情報 ");
            sql.Append("    LEFT JOIN HJ ");
            sql.Append("        ON  t_12090_組合員等別徴収情報.組合等コード = HJ.組合等コード ");
            sql.Append("        AND t_12090_組合員等別徴収情報.年産 = HJ.年産 ");
            sql.Append("        AND t_12090_組合員等別徴収情報.共済目的コード = HJ.共済目的コード ");
            sql.Append("        AND t_12090_組合員等別徴収情報.組合員等コード = HJ.組合員等コード ");
            sql.Append("    GROUP BY ");
            sql.Append("        t_12090_組合員等別徴収情報.組合等コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.年産 ");
            sql.Append("        , t_12090_組合員等別徴収情報.共済目的コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.組合員等コード ");
            sql.Append("    ) AS CJ ");
            sql.Append("    ON HJ.組合等コード = CJ.組合等コード ");
            sql.Append("    AND HJ.年産 = CJ.年産 ");
            sql.Append("    AND HJ.共済目的コード = CJ.共済目的コード ");
            sql.Append("    AND HJ.組合員等コード = CJ.組合員等コード ");
            sql.Append("LEFT JOIN t_12090_組合員等別徴収情報 ");
            sql.Append("    ON CJ.組合等コード = t_12090_組合員等別徴収情報.組合等コード ");
            sql.Append("    AND CJ.年産 = t_12090_組合員等別徴収情報.年産 ");
            sql.Append("    AND CJ.共済目的コード = t_12090_組合員等別徴収情報.共済目的コード ");
            sql.Append("    AND CJ.徴収引受回 = t_12090_組合員等別徴収情報.引受回 ");
            sql.Append("    AND CJ.組合員等コード = t_12090_組合員等別徴収情報.組合員等コード ");
            sql.Append("LEFT JOIN v_nogyosha ");
            sql.Append("    ON HJ.組合等コード = v_nogyosha.kumiaito_cd ");
            sql.Append("    AND HJ.組合員等コード = v_nogyosha.kumiaiinto_cd ");
            sql.Append("LEFT JOIN v_daichiku_nm ");
            sql.Append("    ON HJ.組合等コード = v_daichiku_nm.kumiaito_cd ");
            sql.Append("    AND HJ.大地区コード = v_daichiku_nm.daichiku_cd ");
            sql.Append("LEFT JOIN v_shochiku_nm ");
            sql.Append("    ON HJ.組合等コード = v_shochiku_nm.kumiaito_cd ");
            sql.Append("    AND HJ.大地区コード = v_shochiku_nm.daichiku_cd ");
            sql.Append("    AND HJ.小地区コード = v_shochiku_nm.shochiku_cd ");
            sql.Append("LEFT JOIN v_nogyosha_kinyukikan ");
            sql.Append("    ON v_nogyosha.nogyosha_id = v_nogyosha_kinyukikan.nogyosha_id ");
            sql.Append("INNER JOIN (");
            sql.Append("    SELECT ");
            sql.Append("       共済事業コード ");
            sql.Append("       , 共済目的コード_FIM ");
            sql.Append("       , 振込区分 ");
            sql.Append("    FROM m_00220_共済目的対応 AS KT1 ");
            sql.Append("    WHERE 共済目的コード_NSK = @KyosaiMokutekiCd ");
            sql.Append("    AND 採用順位 = (");
            sql.Append("                    SELECT ");
            sql.Append("                       MIN (採用順位) ");
            sql.Append("                    FROM m_00220_共済目的対応 AS KT2 ");
            sql.Append("                    WHERE KT2.共済事業コード = KT1.共済事業コード ");
            sql.Append("                    AND KT2.共済目的コード_FIM = KT1.共済目的コード_FIM ");
            sql.Append("         ) ");
            sql.Append("    ) AS KT ");
            sql.Append("    ON  v_nogyosha_kinyukikan.kyosai_jigyo_cd = KT.共済事業コード ");
            sql.Append("    AND v_nogyosha_kinyukikan.kyosai_mokutekito_cd = KT.共済目的コード_FIM ");
            sql.Append("    AND v_nogyosha_kinyukikan.furikomi_hikiotoshi_cd = KT.振込区分 ");
            sql.Append("LEFT JOIN v_hanyokubun ");
            sql.Append("    ON v_nogyosha_kinyukikan.furikomi_hikiotoshi_cd = v_hanyokubun.kbn_cd ");
            sql.Append("    AND v_hanyokubun.kbn_sbt = 'furikomi_hikiotoshi_cd' ");
            sql.Append("INNER JOIN m_00010_共済目的名称 ");
            sql.Append("    ON HJ.共済目的コード = m_00010_共済目的名称.共済目的コード ");
            sql.Append("INNER JOIN v_kumiaito ");
            sql.Append("    ON HJ.組合等コード = v_kumiaito.kumiaito_cd ");
        }
        #endregion

        #region 検索条件を取得する
        /// <summary>
        /// 検索条件を取得する。
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="reportJoukens">検索条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="parameters">パラメータ</param>
        private void GetQueryCondition_Where(StringBuilder sql, List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd, List<NpgsqlParameter> parameters)
        {
            sql.Append("WHERE '1' = '1'");

            T01050バッチ条件 jouken = null;

            // 組合等
            if (!string.IsNullOrEmpty(kumiaitoCd))
            {
                sql.Append("AND HJ.組合等コード = @KumiaitoCd ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd", kumiaitoCd));
            }

            // 年産
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_NENSAN).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND HJ.年産 = @Nensan ");
                parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(jouken.条件値)));
            }

            // 共済目的コード
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND HJ.共済目的コード = @KyosaiMokutekiCd ");
                parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", jouken.条件値));
            }

            // 支所
            if (!string.IsNullOrEmpty(shishoCd))
            {
                sql.Append("AND HJ.支所コード = @ShishoCd ");
                parameters.Add(new NpgsqlParameter("@ShishoCd", shishoCd));
            }

            // 大地区
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_DAICHIKU).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND HJ.大地区コード = @DaichikuCd ");
                parameters.Add(new NpgsqlParameter("@DaichikuCd", jouken.条件値));
            }

            // 小地区（開始）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_SHOCHIKU_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND HJ.小地区コード >= @ShochikuCdFrom ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdFrom", jouken.条件値));
            }

            // 小地区（終了）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_SHOCHIKU_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND HJ.小地区コード <= @ShochikuCdTo ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdTo", jouken.条件値));
            }

            // 組合員等コード（開始）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND HJ.組合員等コード >= @KumiaiintoCdFrom ");
                parameters.Add(new NpgsqlParameter("@KumiaiintoCdFrom", jouken.条件値));
            }

            // 組合員等コード（終了）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND HJ.組合員等コード <= @KumiaiintoCdTo ");
                parameters.Add(new NpgsqlParameter("@KumiaiintoCdTo", jouken.条件値));
            }

            // 振込引落区分コード
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_GENKIN_CHOSHU).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値) && JOUKEN_GENKIN_CHOSHU_ARI.Equals(jouken?.条件値))
            {
                sql.Append("AND v_nogyosha_kinyukikan.furikomi_hikiotoshi_cd = '" + HIKIOTOSHI_KBN_CD_CASH + "' ");
            }

            sql.Append("AND (COALESCE(HJ.納入額, 0) > 0 OR COALESCE(CJ.前回迄徴収額, 0) > 0) ");
        }
        #endregion

        #region クエリ式にソート順設定
        /// <summary>
        /// ソート順設定処理
        /// </summary>
        /// <param name="sql">sql</param>
        private void GetOrderby(StringBuilder sql)
        {
            sql.Append("ORDER BY ");
            sql.Append("    HJ.支所コード ASC ");
            sql.Append("    , HJ.大地区コード ASC ");
            sql.Append("    , HJ.小地区コード ASC ");
            sql.Append("    , HJ.組合員等コード ASC ");
        }
        #endregion
    }
}
