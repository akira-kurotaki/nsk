using NskAppModelLibrary.Context;
using NskReportMain.Common;
using NskReportMain.Models.NSK_P107010;
using NskReportMain.ReportCreators.NSK_P107010;
using NskReportMain.Reports;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using System.Text;
using JoukenNameConst = NskCommonLibrary.Core.Consts.JoukenNameConst;
using NskCoreConst = NskCommonLibrary.Core.Consts.CoreConst;
using NskCommonUtil = NskCommonLibrary.Core.Utility;
using NskAppModelLibrary.Models;

namespace NskReportMain.Controllers
{
    public class NskReportP107010Controller : ReportController
    {

        #region メンバー定数
        /// <summary>
        /// 印影種別「組合員」
        /// </summary>
        private static readonly string INEI_SBT_KUMIAIINTO = "01";

        /// <summary>
        /// 引受方式「インデックス」
        /// </summary>
        private static readonly string HIKIUKE_HOUSIKI_INDEX = "6";

        /// <summary>
        /// 帳票ID
        /// </summary>
        private static readonly string REPORT_ID = "NSK_107010P";

        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "P107010_加入承諾書兼共済掛金等払込通知書（６号）_制御処理";
        #endregion

        public NskReportP107010Controller(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        #region P107010_加入承諾書兼共済掛金等払込通知書（６号）_制御処理
        /// <summary>
        /// P107010_加入承諾書兼共済掛金等払込通知書（６号）_制御処理
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
                // 条件名称リスト
                List<string> jokenNames =
                [
                    JoukenNameConst.JOUKEN_NENSAN,                        // 年産
                    JoukenNameConst.JOUKEN_DAICHIKU,                      // 大地区コード
                    JoukenNameConst.JOUKEN_SHOCHIKU_START,                // 小地区コード（開始）
                    JoukenNameConst.JOUKEN_SHOCHIKU_END,                  // 小地区コード（終了）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,           // 組合員等コード（開始）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,             // 組合員等コード（終了）
                    JoukenNameConst.JOUKEN_HAKKO_DATE,                    // 発行年月日
                    JoukenNameConst.JOUKEN_KUMIAICHO_STAMP,               // 組合長印の押印
                    JoukenNameConst.JOUKEN_KYOSAI_SEIRITSU_DATE,          // 共済関係成立日
                    JoukenNameConst.JOUKEN_HARAIKOMI_KIGEN,               // 払込期限
                    JoukenNameConst.JOUKEN_KOZA_FURIKAE_DATE,             // 口座振替日
                    JoukenNameConst.JOUKEN_REPORT_NAME                    // 帳票名
                ];
                var reportJoukens = getJigyoDb<NskAppContext>().T01050バッチ条件s
                    .Where(t => t.バッチ条件id == joukenId && jokenNames.Contains(t.条件名称))
                    .ToList();
                if (reportJoukens.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME01645", "パラメータの取得");
                }

                // 様式文言を取得するため、バッチ条件から年産取得
                short nensan = 0;
                var jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_NENSAN).SingleOrDefault();
                if (!string.IsNullOrEmpty(jouken?.条件値))
                {
                    nensan = short.Parse(jouken.条件値);
                }
                // 様式文言を取得する。
                // 取得結果が0件の場合は、エラーとし、エラーメッセージを返す。
                List<int> mongonNos = [1, 2, 3, 4, 5, 6];
                var pMongonList = getJigyoDb<NskAppContext>().M00210様式文言s
                        .Where(t => t.組合等コード == kumiaitoCd　&&
                                    t.年産 == nensan &&
                                    t.帳票id == REPORT_ID)
                        .ToList();
                if (pMongonList.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME01645", "様式文言の取得");
                }

                // 組合員等コードヘッダーへの出力対象データを取得する。
                // 取得結果が0件の場合は、エラーとし、エラーメッセージを返す。
                var pHeaderList = GetReportHeaderDataList(reportJoukens, kumiaitoCd, shishoCd);
                if (pHeaderList.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME10076", "0");
                }

                // 加入承諾書兼共済掛金等払込通知書Sub1への出力対象データを取得する。
                // 取得結果が0件の場合は、エラーとし、エラーメッセージを返す。
                var pSub1List = GetReportSub1DataList(reportJoukens, kumiaitoCd, shishoCd);
                if (pSub1List.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME10076", "0");
                }

                // 加入承諾書兼共済掛金等払込通知書Sub1への出力対象データを取得する。
                // 取得結果が0件の場合は、エラーとし、エラーメッセージを返す。
                var pSub2List = GetReportSub2DataList(reportJoukens, kumiaitoCd, shishoCd);
                if (pSub2List.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME10076", "0");
                }

                // 帳票PDF一時出力フォルダ作成
                // 定数取得(（定数ID：PrintTempFolder）)\バッチID_システム日時(yyyyMMddHHmmss)
                var printTempFolder = NskCommonUtil.FolderUtil.CreateTempFolder(sysDate, batchId.ToString());

                // 帳票オブジェクト
                var report = new BaseSectionReport();
                // 帳票出力処理を呼び出す。
                result = CreateP107010(reportJoukens, pMongonList, pHeaderList, pSub1List, pSub2List, joukenId, ref report);
                if (result.Result == ReportConst.RESULT_SUCCESS)
                {
                    // ファイル名はバッチ条件テーブルの帳票名 + ".pdf"とする。
                    string reportName = string.Empty;
                    jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_REPORT_NAME).SingleOrDefault();
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

        #region 「P107010_加入承諾書兼共済掛金等払込通知書（６号）」を作成するメソッド
        /// <summary>
        /// 「P107010_加入承諾書兼共済掛金等払込通知書（６号）」を作成するメソッド
        /// </summary>
        /// <param name="reportJoukens">T01050バッチ条件モデルリスト</param>
        /// <param name="mongonModel">M00210様式文言モデルリスト</param>
        /// <param name="headerModel">組合員等コードヘッダーモデルリスト</param>
        /// <param name="sub1Model">加入承諾書兼共済掛金等払込通知書Sub1モデルリスト</param>
        /// <param name="sub2Model">組加入承諾書兼共済掛金等払込通知書Sub2モデルリスト</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="report">帳票オブジェクト</param>
        /// <returns>実行結果</returns>
        private ControllerResult CreateP107010(
            List<T01050バッチ条件> reportJoukens,
            List<M00210様式文言> mongonModel,
            List<NSK_P107010HeaderModel> headerModel,
            List<NSK_P107010Sub1Model> sub1Model,
            List<NSK_P107010Sub2Model> sub2Model,
            string joukenId,
            ref BaseSectionReport report)
        {
            // 実行結果
            var result = new ControllerResult();
            NSK_P107010Creator pCreator = new NSK_P107010Creator();
            CreatorResult creatorResult = pCreator.CreateReport(joukenId, reportJoukens, mongonModel, headerModel, sub1Model, sub2Model);
            // 失敗した場合
            if (creatorResult.Result == ReportConst.RESULT_FAILED)
            {
                return result.ControllerResultError(creatorResult.ErrorMessage, creatorResult.ErrorMessageId);
            }

            // 出力レポートにP107010_加入承諾書兼共済掛金等払込通知書（６号）を入れる。
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

        #region 帳票データ取得SQL WITH句群

        #region 帳票データ取得SQL WITH句 HJ(組合員等コードヘッダー用)
        private void GetWithHJHeader(StringBuilder sql)
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

        #region 帳票データ取得SQL WITH句 HJ
        /// <summary>
        /// WITH句 HJを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetWithHJ(StringBuilder sql)
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
            sql.Append(") ");
        }

        #endregion

        #region 帳票データ取得SQL WITH句 SUI_KEY
        /// <summary>
        /// WITH句 SUI_KEYを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetWithSuiKey(StringBuilder sql)
        {
            sql.Append(", SUI_KEY AS ( ");
            sql.Append("    SELECT");
            sql.Append("        *");
            sql.Append("    FROM");
            sql.Append("        ( SELECT");
            sql.Append("              HJ.組合等コード");
            sql.Append("              , HJ.年産");
            sql.Append("              , HJ.共済目的コード");
            sql.Append("              , HJ.引受回");
            sql.Append("              , HJ.支所コード");
            sql.Append("              , HJ.類区分");
            sql.Append("              , HJ.統計単位地域コード");
            sql.Append("              , HJ.組合員等コード");
            sql.Append("              , HJ.大地区コード");
            sql.Append("              , HJ.小地区コード");
            sql.Append("              , HJ.補償割合コード");
            sql.Append("              , t_12050_組合員等別引受用途.作付時期");
            sql.Append("              , t_12050_組合員等別引受用途.種類区分");
            sql.Append("              , HJ.特約区分");
            sql.Append("              , HJ.引受方式");
            sql.Append("              , t_12050_組合員等別引受用途.共済金額選択順位");
            sql.Append("              , HJ.収穫量確認方法");
            sql.Append("              , ROW_NUMBER() OVER");
            sql.Append("                   ( PARTITION BY");
            sql.Append("                         HJ.組合等コード");
            sql.Append("                         , HJ.年産");
            sql.Append("                         , HJ.共済目的コード");
            sql.Append("                         , HJ.引受回");
            sql.Append("                         , HJ.支所コード");
            sql.Append("                         , HJ.類区分");
            sql.Append("                         , HJ.組合員等コード");
            sql.Append("                         , HJ.引受方式");
            sql.Append("                         , HJ.特約区分");
            sql.Append("                         , HJ.補償割合コード");
            sql.Append("                      ORDER BY");
            sql.Append("                         HJ.組合等コード");
            sql.Append("                         ,  HJ.年産");
            sql.Append("                         ,  HJ.共済目的コード");
            sql.Append("                         ,  HJ.引受回");
            sql.Append("                         ,  HJ.支所コード");
            sql.Append("                         ,  HJ.類区分");
            sql.Append("                         ,  HJ.組合員等コード");
            sql.Append("                         ,  HJ.引受方式");
            sql.Append("                         ,  HJ.特約区分");
            sql.Append("                         ,  HJ.補償割合コード");
            sql.Append("                         ,  t_12050_組合員等別引受用途.作付時期");
            sql.Append("                         ,  t_12050_組合員等別引受用途.種類区分");
            sql.Append("                ) AS SRT");
            sql.Append("        FROM");
            sql.Append("            HJ");
            sql.Append("        LEFT JOIN t_12050_組合員等別引受用途");
            sql.Append("            ON  HJ.組合等コード   = t_12050_組合員等別引受用途.組合等コード");
            sql.Append("            AND HJ.年産           = t_12050_組合員等別引受用途.年産");
            sql.Append("            AND HJ.共済目的コード = t_12050_組合員等別引受用途.共済目的コード");
            sql.Append("            AND HJ.引受回         = t_12050_組合員等別引受用途.引受回");
            sql.Append("            AND HJ.組合員等コード = t_12050_組合員等別引受用途.組合員等コード");
            sql.Append("            AND HJ.類区分         = t_12050_組合員等別引受用途.類区分");
            sql.Append("        WHERE ");
            sql.Append("                HJ.共済目的コード =  '" + ((int)NskCoreConst.KyosaiMokutekiCdNumber.Suitou).ToString() + "'");
            sql.Append("            AND HJ.類区分         <> '0'");
            sql.Append("            AND HJ.引受方式       =  '" + HIKIUKE_HOUSIKI_INDEX + "'");
            sql.Append("       )");
            sql.Append("    WHERE SRT = 1");
            sql.Append(") ");
        }
        #endregion

        #region 帳票データ取得SQL WITH句 SUI_ITEMS
        /// <summary>
        /// WITH句 SUI_ITEMSを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetWithSuiItems(StringBuilder sql)
        {
            sql.Append(", SUI_ITEMS AS ( ");
            sql.Append("    SELECT");
            sql.Append("        ITEMS.組合等コード");
            sql.Append("        , ITEMS.年産");
            sql.Append("        , ITEMS.共済目的コード");
            sql.Append("        , ITEMS.引受回");
            sql.Append("        , ITEMS.支所コード");
            sql.Append("        , ITEMS.類区分");
            sql.Append("        , ITEMS.組合員等コード");
            sql.Append("        , ITEMS.引受方式");
            sql.Append("        , ITEMS.特約区分");
            sql.Append("        , ITEMS.補償割合コード");
            sql.Append("        , ITEMS.作付時期");
            sql.Append("        , ITEMS.種類区分");
            sql.Append("        , ITEMS.引受区分名称");
            sql.Append("        , ITEMS.共済金額選択順位");
            sql.Append("    FROM");
            sql.Append("        ( SELECT");
            sql.Append("              HJ.組合等コード");
            sql.Append("              , HJ.年産");
            sql.Append("              , HJ.共済目的コード");
            sql.Append("              , HJ.引受回");
            sql.Append("              , HJ.支所コード");
            sql.Append("              , HJ.類区分");
            sql.Append("              , HJ.組合員等コード");
            sql.Append("              , t_12050_組合員等別引受用途.作付時期");
            sql.Append("              , t_12050_組合員等別引受用途.種類区分");
            sql.Append("              , m_10090_引受区分名称.引受区分名称");
            sql.Append("              , t_12050_組合員等別引受用途.共済金額選択順位");
            sql.Append("              , HJ.引受方式");
            sql.Append("              , HJ.特約区分");
            sql.Append("              , HJ.補償割合コード");
            sql.Append("              , ROW_NUMBER() OVER");
            sql.Append("                   ( PARTITION BY");
            sql.Append("                        HJ.組合等コード");
            sql.Append("                        , HJ.年産");
            sql.Append("                        , HJ.共済目的コード");
            sql.Append("                        , HJ.引受回");
            sql.Append("                        , HJ.支所コード");
            sql.Append("                        , HJ.類区分");
            sql.Append("                        , HJ.組合員等コード");
            sql.Append("                        , HJ.引受方式");
            sql.Append("                        , HJ.特約区分");
            sql.Append("                        , HJ.補償割合コード");
            sql.Append("                     ORDER BY");
            sql.Append("                        HJ.組合等コード");
            sql.Append("                        , HJ.年産");
            sql.Append("                        , HJ.共済目的コード");
            sql.Append("                        , HJ.引受回");
            sql.Append("                        , HJ.支所コード");
            sql.Append("                        , HJ.類区分");
            sql.Append("                        , HJ.組合員等コード");
            sql.Append("                        , HJ.引受方式");
            sql.Append("                        , HJ.特約区分");
            sql.Append("                        , HJ.補償割合コード");
            sql.Append("                        , t_12050_組合員等別引受用途.作付時期");
            sql.Append("                        , t_12050_組合員等別引受用途.種類区分");
            sql.Append("                    ) SRT");
            sql.Append("          FROM");
            sql.Append("              HJ");
            sql.Append("          LEFT JOIN t_12050_組合員等別引受用途");
            sql.Append("              ON  HJ.組合等コード   = t_12050_組合員等別引受用途.組合等コード");
            sql.Append("              AND HJ.年産           = t_12050_組合員等別引受用途.年産");
            sql.Append("              AND HJ.共済目的コード = t_12050_組合員等別引受用途.共済目的コード");
            sql.Append("              AND HJ.引受回         = t_12050_組合員等別引受用途.引受回");
            sql.Append("              AND HJ.組合員等コード = t_12050_組合員等別引受用途.組合員等コード");
            sql.Append("              AND HJ.類区分         = t_12050_組合員等別引受用途.類区分");
            sql.Append("          INNER JOIN                 m_10090_引受区分名称");
            sql.Append("              ON  t_12050_組合員等別引受用途.共済目的コード = m_10090_引受区分名称.共済目的コード");
            sql.Append("              AND t_12050_組合員等別引受用途.種類区分       = m_10090_引受区分名称.種類区分");
            sql.Append("              AND t_12050_組合員等別引受用途.作付時期       = m_10090_引受区分名称.作付時期");
            sql.Append("          WHERE");
            sql.Append("                  HJ.共済目的コード =  '" + ((int)NskCoreConst.KyosaiMokutekiCdNumber.Suitou).ToString() + "'");
            sql.Append("              AND HJ.類区分         <> '0'");
            sql.Append("              AND HJ.引受方式       =  '" +  HIKIUKE_HOUSIKI_INDEX + "'");
            sql.Append("          GROUP BY");
            sql.Append("              HJ.組合等コード");
            sql.Append("              , HJ.年産");
            sql.Append("              , HJ.共済目的コード");
            sql.Append("              , HJ.引受回");
            sql.Append("              , HJ.支所コード");
            sql.Append("              , HJ.類区分");
            sql.Append("              , HJ.組合員等コード");
            sql.Append("              , t_12050_組合員等別引受用途.作付時期");
            sql.Append("              , t_12050_組合員等別引受用途.種類区分");
            sql.Append("              , m_10090_引受区分名称.引受区分名称");
            sql.Append("              , t_12050_組合員等別引受用途.共済金額選択順位");
            sql.Append("              , HJ.引受方式");
            sql.Append("              , HJ.特約区分");
            sql.Append("              , HJ.補償割合コード");
            sql.Append("    ) ITEMS");
            sql.Append("    WHERE");
            sql.Append("        ITEMS.SRT <> 1");
            sql.Append(") ");
        }
        #endregion

        #region 帳票データ取得SQL WITH句 MUGI_KEY
        /// <summary>
        /// WITH句 MUGI_KEYを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetWithMugiKey(StringBuilder sql)
        {
            sql.Append(", MUGI_KEY AS ( ");
            sql.Append("    SELECT");
            sql.Append("        *");
            sql.Append("    FROM");
            sql.Append("        ( SELECT");
            sql.Append("             HJ.組合等コード");
            sql.Append("             , HJ.年産");
            sql.Append("             , HJ.共済目的コード");
            sql.Append("             , HJ.引受回");
            sql.Append("             , HJ.支所コード");
            sql.Append("             , HJ.類区分");
            sql.Append("             , HJ.統計単位地域コード");
            sql.Append("             , HJ.組合員等コード");
            sql.Append("             , HJ.大地区コード");
            sql.Append("             , HJ.小地区コード");
            sql.Append("             , HJ.補償割合コード");
            sql.Append("             , t_12050_組合員等別引受用途.作付時期");
            sql.Append("             , t_12050_組合員等別引受用途.種類区分");
            sql.Append("             , t_12050_組合員等別引受用途.用途区分");
            sql.Append("             , HJ.特約区分");
            sql.Append("             , HJ.引受方式");
            sql.Append("             , t_12050_組合員等別引受用途.共済金額選択順位");
            sql.Append("             , HJ.収穫量確認方法");
            sql.Append("             , ROW_NUMBER()  OVER");
            sql.Append("                    ( PARTITION BY");
            sql.Append("                          HJ.組合等コード");
            sql.Append("                          , HJ.年産");
            sql.Append("                          , HJ.共済目的コード");
            sql.Append("                          , HJ.引受回");
            sql.Append("                          , HJ.支所コード");
            sql.Append("                          , HJ.類区分");
            sql.Append("                          , HJ.組合員等コード");
            sql.Append("                          , HJ.引受方式");
            sql.Append("                          , HJ.特約区分");
            sql.Append("                          , HJ.補償割合コード");
            sql.Append("                      ORDER BY");
            sql.Append("                          HJ.組合等コード");
            sql.Append("                          , HJ.年産");
            sql.Append("                          , HJ.共済目的コード");
            sql.Append("                          , HJ.引受回");
            sql.Append("                          , HJ.支所コード");
            sql.Append("                          , HJ.類区分");
            sql.Append("                          , HJ.組合員等コード");
            sql.Append("                          , HJ.引受方式");
            sql.Append("                          , HJ.特約区分");
            sql.Append("                          , HJ.補償割合コード");
            sql.Append("                          , t_12050_組合員等別引受用途.作付時期");
            sql.Append("                          , t_12050_組合員等別引受用途.種類区分");
            sql.Append("                          , t_12050_組合員等別引受用途.用途区分");
            sql.Append("                     ) SRT");
            sql.Append("          FROM");
            sql.Append("              HJ");
            sql.Append("          LEFT JOIN t_12050_組合員等別引受用途");
            sql.Append("              ON  HJ.組合等コード    = t_12050_組合員等別引受用途.組合等コード");
            sql.Append("              AND HJ.年産            = t_12050_組合員等別引受用途.年産");
            sql.Append("              AND HJ.共済目的コード  = t_12050_組合員等別引受用途.共済目的コード");
            sql.Append("              AND HJ.引受回          = t_12050_組合員等別引受用途.引受回");
            sql.Append("              AND HJ.組合員等コード  = t_12050_組合員等別引受用途.組合員等コード");
            sql.Append("              AND HJ.類区分          = t_12050_組合員等別引受用途.類区分");
            sql.Append("          WHERE");
            sql.Append("                  HJ.共済目的コード =  '" + ((int)NskCoreConst.KyosaiMokutekiCdNumber.Mugi).ToString() + "'");
            sql.Append("              AND HJ.類区分         <> '0'");
            sql.Append("        )");
            sql.Append("    WHERE");
            sql.Append("        SRT = 1");
            sql.Append(") ");
        }
        #endregion

        #region 帳票データ取得SQL WITH句 MUGI_ITEMS
        /// <summary>
        /// WITH句 MUGI_ITEMSを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetWithMugiItems(StringBuilder sql)
        {
            sql.Append(", MUGI_ITEMS AS ( ");
            sql.Append("    SELECT");
            sql.Append("        ITEMS.組合等コード");
            sql.Append("        , ITEMS.年産");
            sql.Append("        , ITEMS.共済目的コード");
            sql.Append("        , ITEMS.引受回");
            sql.Append("        , ITEMS.支所コード");
            sql.Append("        , ITEMS.類区分");
            sql.Append("        , ITEMS.組合員等コード");
            sql.Append("        , ITEMS.引受方式");
            sql.Append("        , ITEMS.特約区分");
            sql.Append("        , ITEMS.補償割合コード");
            sql.Append("        , ITEMS.作付時期");
            sql.Append("        , ITEMS.種類区分");
            sql.Append("        , ITEMS.用途区分");
            sql.Append("        , ITEMS.用途名称");
            sql.Append("        , ITEMS.共済金額選択順位");
            sql.Append("    FROM");
            sql.Append("        ( SELECT");
            sql.Append("              HJ.組合等コード");
            sql.Append("              , HJ.年産");
            sql.Append("              , HJ.共済目的コード");
            sql.Append("              , HJ.引受回");
            sql.Append("              , HJ.支所コード");
            sql.Append("              , HJ.類区分");
            sql.Append("              , HJ.組合員等コード");
            sql.Append("              , t_12050_組合員等別引受用途.作付時期");
            sql.Append("              , t_12050_組合員等別引受用途.種類区分");
            sql.Append("              , t_12050_組合員等別引受用途.用途区分");
            sql.Append("              , m_10110_用途区分名称.用途名称");
            sql.Append("              , t_12050_組合員等別引受用途.共済金額選択順位");
            sql.Append("              , HJ.引受方式");
            sql.Append("              , HJ.特約区分");
            sql.Append("              , HJ.補償割合コード");
            sql.Append("              , ROW_NUMBER() OVER");
            sql.Append("                    ( PARTITION BY");
            sql.Append("                          HJ.組合等コード");
            sql.Append("                          , HJ.年産");
            sql.Append("                          , HJ.共済目的コード");
            sql.Append("                          , HJ.引受回");
            sql.Append("                          , HJ.支所コード");
            sql.Append("                          , HJ.類区分");
            sql.Append("                          , HJ.組合員等コード");
            sql.Append("                          , HJ.引受方式");
            sql.Append("                          , HJ.特約区分");
            sql.Append("                          , HJ.補償割合コード");
            sql.Append("                      ORDER BY");
            sql.Append("                          HJ.組合等コード");
            sql.Append("                          , HJ.年産");
            sql.Append("                          , HJ.共済目的コード");
            sql.Append("                          , HJ.引受回");
            sql.Append("                          , HJ.支所コード");
            sql.Append("                          , HJ.類区分");
            sql.Append("                          , HJ.組合員等コード");
            sql.Append("                          , HJ.引受方式");
            sql.Append("                          , HJ.特約区分");
            sql.Append("                          , HJ.補償割合コード");
            sql.Append("                          , t_12050_組合員等別引受用途.作付時期");
            sql.Append("                          , t_12050_組合員等別引受用途.種類区分");
            sql.Append("                          , t_12050_組合員等別引受用途.用途区分");
            sql.Append("                    ) SRT");
            sql.Append("            FROM");
            sql.Append("                HJ");
            sql.Append("            LEFT JOIN t_12050_組合員等別引受用途");
            sql.Append("                ON  HJ.組合等コード   = t_12050_組合員等別引受用途.組合等コード");
            sql.Append("                AND HJ.年産           = t_12050_組合員等別引受用途.年産");
            sql.Append("                AND HJ.共済目的コード = t_12050_組合員等別引受用途.共済目的コード");
            sql.Append("                AND HJ.引受回         = t_12050_組合員等別引受用途.引受回");
            sql.Append("                AND HJ.組合員等コード = t_12050_組合員等別引受用途.組合員等コード");
            sql.Append("                AND HJ.類区分         = t_12050_組合員等別引受用途.類区分");
            sql.Append("            INNER JOIN m_10110_用途区分名称");
            sql.Append("                ON  t_12050_組合員等別引受用途.共済目的コード = m_10110_用途区分名称.共済目的コード");
            sql.Append("                AND t_12050_組合員等別引受用途.用途区分       = m_10110_用途区分名称.用途区分");
            sql.Append("            WHERE");
            sql.Append("                    HJ.共済目的コード =  '" + ((int)NskCoreConst.KyosaiMokutekiCdNumber.Mugi).ToString() + "'");
            sql.Append("                AND HJ.類区分         <> '0'");
            sql.Append("            GROUP BY");
            sql.Append("                HJ.組合等コード");
            sql.Append("                , HJ.年産");
            sql.Append("                , HJ.共済目的コード");
            sql.Append("                , HJ.引受回");
            sql.Append("                , HJ.支所コード");
            sql.Append("                , HJ.類区分");
            sql.Append("                , HJ.組合員等コード");
            sql.Append("                , t_12050_組合員等別引受用途.作付時期");
            sql.Append("                , t_12050_組合員等別引受用途.種類区分");
            sql.Append("                , t_12050_組合員等別引受用途.用途区分");
            sql.Append("                , m_10110_用途区分名称.用途名称");
            sql.Append("                , t_12050_組合員等別引受用途.共済金額選択順位");
            sql.Append("                , HJ.引受方式");
            sql.Append("                , HJ.特約区分");
            sql.Append("                , HJ.補償割合コード");
            sql.Append("        )  ITEMS");
            sql.Append("    WHERE");
            sql.Append("        ITEMS.SRT <> 1");
            sql.Append(") ");
        }
        #endregion

        #endregion

        #region 帳票データ取得SQL 共通WHERE句
        private void GetCommonQueryCondition_Where(StringBuilder sql, List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd, List<NpgsqlParameter> parameters, string tableName)
        {
            sql.Append("WHERE '1' = '1' ");

            T01050バッチ条件 jouken = null;

            // 組合等コード
            if (!string.IsNullOrEmpty(kumiaitoCd))
            {
                sql.Append("AND " + tableName + ".組合等コード = @KumiaitoCd ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd", kumiaitoCd));
            }

            // 年産
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_NENSAN).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND " + tableName + ".年産 = @Nensan ");
                parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(jouken.条件値)));
            }

            // 支所
            if (!string.IsNullOrEmpty(shishoCd))
            {
                sql.Append("AND " + tableName + ".支所コード = @ShishoCd ");
                parameters.Add(new NpgsqlParameter("@ShishoCd", shishoCd));
            }

            // 大地区
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_DAICHIKU).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND " + tableName + " .大地区コード = @DaichikuCd ");
                parameters.Add(new NpgsqlParameter("@DaichikuCd", jouken.条件値));
            }

            // 小地区（開始）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_SHOCHIKU_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND " + tableName +  ".小地区コード >= @ShochikuCdFrom ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdFrom", jouken.条件値));
            }

            // 小地区（終了）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_SHOCHIKU_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND " + tableName + ".小地区コード <= @ShochikuCdTo ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdTo", jouken.条件値));
            }

            // 組合員等コード（開始）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND " + tableName + ".組合員等コード >= @KumiaiintoCdFrom ");
                parameters.Add(new NpgsqlParameter("@KumiaiintoCdFrom", jouken.条件値));
            }

            // 組合員等コード（終了）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND " + tableName + ".組合員等コード <= @KumiaiintoCdTo ");
                parameters.Add(new NpgsqlParameter("@KumiaiintoCdTo", jouken.条件値));
            }
        }

        #endregion

        #region 帳票データ取得メソッド群（組合員等コードヘッダー）

        #region 帳票データ取得メソッド（組合員等コードヘッダー）
        /// <summary>
        /// 作成対象とする帳票データを取得する（組合員等コードヘッダー）
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>帳票データ</returns>
        private List<NSK_P107010HeaderModel> GetReportHeaderDataList(List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            //WITH句を取得する
            GetWithHJHeader(sql);
            // 検索項目を取得する
            GetHeaderCondition(sql);
            // 検索テーブルを取得する
            GetReportHeaderDataTableCondition(sql);
            // 検索条件を取得する
            GetHeaderQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters);
            // ソート順を追加する
            GetHeaderOrderby(sql);

            logger.Info("作成対象とする帳票データを取得する。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<NSK_P107010HeaderModel>(sql.ToString(), parameters.ToArray()).ToList();
        }

        #endregion

        #region 検索項目取得（組合員等コードヘッダー）
        /// <summary>
        /// 検索項目を取得する（組合員等コードヘッダー）
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetHeaderCondition(StringBuilder sql)
        {
            sql.Append("SELECT ");
            sql.Append("    HJ.組合等コード  AS \"組合等コード\" ");
            sql.Append("    , HJ.年産 AS \"年産\" ");
            sql.Append("    , HJ.共済目的コード AS \"共済目的コード\" ");
            sql.Append("    , HJ.引受回 AS \"引受回\" ");
            sql.Append("    , HJ.支所コード AS \"支所コード\" ");
            sql.Append("    , HJ.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , m_00010_共済目的名称.共済目的名称 AS \"共済目的名称\" ");
            sql.Append("    , CASE WHEN v_nogyosha.postal_cd <> '' AND v_nogyosha.postal_cd IS NOT NULL THEN LEFT(v_nogyosha.postal_cd, 3) || '-' || RIGHT(v_nogyosha.postal_cd, 4) ELSE v_nogyosha.postal_cd END AS \"郵便番号\" ");
            sql.Append("    , v_nogyosha.address AS \"申込者住所\" ");
            sql.Append("    , v_nogyosha.hojin_full_nm AS \"氏名又は法人名\" ");
            sql.Append("    , v_kumiaito.address AS \"組合住所\" ");
            sql.Append("    , v_kumiaito.kumiaito_nm AS \"組合等正式名称\" ");
            sql.Append("    , v_kumiaito.daihyosha_yaku_nm AS \"代表者役職名\" ");
            sql.Append("    , v_kumiaito.daihyosha_nm AS \"代表者氏名\" ");
            sql.Append("    , v_kumiaito_inei.file_path AS \"ファイルパス\" ");
            sql.Append("    , v_kumiaito_inei.file_nm AS \"ファイル名\" ");
            sql.Append("    , COALESCE(HJ.組合員等負担共済掛金, 0) AS \"組合員等負担共済掛金\" ");
            sql.Append("    , COALESCE(HJ.賦課金計, 0) AS \"賦課金計\" ");
            sql.Append("    , COALESCE(CJ.今回迄徴収額, 0) AS \"今回迄徴収額\" ");
            sql.Append("    , COALESCE(HJ.賦課金計, 0) - (COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0)) AS \"賦課金計差額\" ");
            sql.Append("    , CASE WHEN");
            sql.Append("              HJ.解除フラグ = '1'");
            sql.Append("            THEN");
            sql.Append("              COALESCE(HJ.引受解除返還賦課金額, 0) - (COALESCE(HJ.賦課金計, 0) - (COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0))) ");   //HJ.引受解除返還賦課金額 -  賦課金計差額
            sql.Append("            ELSE");
            sql.Append("              0");
            sql.Append("      END AS \"今回引受解除徴収賦課金額\" ");
            sql.Append("    , COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) ");    //CJ.前回迄引受解除徴収賦課金額 + 今回引受解除徴収賦課金額
            sql.Append("          + CASE WHEN");
            sql.Append("                    HJ.解除フラグ = '1'");
            sql.Append("                  THEN");
            sql.Append("                    COALESCE(HJ.引受解除返還賦課金額, 0) - (COALESCE(HJ.賦課金計, 0) - (COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0))) ");   //HJ.引受解除返還賦課金額 -  賦課金計差額
            sql.Append("                  ELSE");
            sql.Append("                    0");
            sql.Append("             END ");
            sql.Append("      AS \"今回迄引受解除徴収賦課金額\" ");
            sql.Append("    , HJ.継続特約フラグ AS \"継続特約フラグ\" ");
            sql.Append("    , HJ.大地区コード AS \"大地区コード\" ");
            sql.Append("    , HJ.小地区コード AS \"小地区コード\" ");
            // 以下は帳票出力用項目、設定は帳票作成クラスで実施
            sql.Append("    , '' AS  \"年産_表示\" ");
            sql.Append("    , '' AS  \"申込日_表示\" ");
            sql.Append("    , FALSE AS \"組合代表者印_表示有無\" ");
            sql.Append("    , '' AS  \"共済関係成立日_表示\" ");
            sql.Append("    , '' AS  \"文書_上段_表示\" ");
            sql.Append("    , '' AS  \"文書_耕地等情報_表示\" ");
            sql.Append("    , '' AS  \"文書_下段_表示\" ");
            sql.Append("    , 0 AS  \"負担共済掛金事務費賦課金の合計_表示\" ");
            sql.Append("    , 0 AS  \"既納入額_表示\" ");
            sql.Append("    , 0 AS  \"納入額_表示\" ");
            sql.Append("    , '' AS  \"払込期限_表示\" ");
            sql.Append("    , '' AS  \"口座振替日_表示\" ");
            sql.Append("    , FALSE AS  \"自動継続特約の有_表示有無\" ");
            sql.Append("    , FALSE AS  \"自動継続特約の無_表示有無\" ");
            sql.Append("    , FALSE AS  \"SUB1_表示有無\" ");
            sql.Append("    , FALSE AS  \"SUB2_表示有無\" ");
            sql.Append("    , '' AS  \"ページ\" ");
        }
        #endregion

        #region 検索テーブル取得(組合員等コードヘッダー)
        /// <summary>
        /// 検索テーブルを取得する(組合員等コードヘッダー)
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetReportHeaderDataTableCondition(StringBuilder sql)
        {
            sql.Append("FROM ");
            sql.Append("    HJ ");
            sql.Append("INNER JOIN  m_00010_共済目的名称 ");
            sql.Append("    ON HJ.共済目的コード = m_00010_共済目的名称.共済目的コード ");
            sql.Append("INNER JOIN v_nogyosha ");
            sql.Append("    ON HJ.組合等コード = v_nogyosha.kumiaito_cd ");
            sql.Append("    AND HJ.組合員等コード = v_nogyosha.kumiaiinto_cd ");
            sql.Append("INNER JOIN v_kumiaito ");
            sql.Append("    ON HJ.組合等コード = v_kumiaito.kumiaito_cd ");
            sql.Append("LEFT JOIN  v_kumiaito_inei ");
            sql.Append("    ON HJ.組合等コード = v_kumiaito_inei.kumiaito_cd ");
            sql.Append("    AND v_kumiaito_inei.inei_sbt = '" + INEI_SBT_KUMIAIINTO + "' ");
            sql.Append("LEFT JOIN ( ");
            sql.Append("    SELECT ");
            sql.Append("        t_12090_組合員等別徴収情報.組合等コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.年産 ");
            sql.Append("        , t_12090_組合員等別徴収情報.共済目的コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.組合員等コード ");
            sql.Append("        , SUM(t_12090_組合員等別徴収情報.徴収金額) AS \"今回迄徴収額\" ");
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
            sql.Append(") AS CJ ");
            sql.Append("    ON  HJ.組合等コード = CJ.組合等コード ");
            sql.Append("    AND HJ.年産 = CJ.年産 ");
            sql.Append("    AND HJ.共済目的コード = CJ.共済目的コード ");
            sql.Append("    AND HJ.組合員等コード = CJ.組合員等コード ");
        }
        #endregion

        #region 検索条件を取得する(組合員等コードヘッダー)
        /// <summary>
        /// 検索条件を取得する。(組合員等コードヘッダー)
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="reportJoukens">検索条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="parameters">パラメータ</param>
        private void GetHeaderQueryCondition_Where(StringBuilder sql, List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd, List<NpgsqlParameter> parameters)
        {
            //共通条件設定
            GetCommonQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters, "HJ");

            sql.Append("AND (COALESCE(HJ.納入額, 0) ");
            sql.Append("      - (COALESCE(CJ.今回迄徴収額, 0) ");
            // 今回迄引受解除徴収賦課金額
            sql.Append("      - (COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) ");
            sql.Append("          + CASE WHEN");
            sql.Append("                    HJ.解除フラグ = '1'");
            sql.Append("                 THEN");
            sql.Append("                    COALESCE(HJ.引受解除返還賦課金額, 0) - (COALESCE(HJ.賦課金計, 0) - (COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0))) ");
            sql.Append("                 ELSE");
            sql.Append("                    0");
            sql.Append("             END)) ");
            sql.Append("    ) <> 0 ");
        }
        #endregion

        #region クエリ式にソート順設定(組合員等コードヘッダー)
        /// <summary>
        /// ソート順設定処理(組合員等コードヘッダー)
        /// </summary>
        /// <param name="sql">sql</param>
        private void GetHeaderOrderby(StringBuilder sql)
        {
            sql.Append("ORDER BY ");
            sql.Append("    HJ.支所コード ASC ");
            sql.Append("    , HJ.大地区コード ASC ");
            sql.Append("    , HJ.小地区コード ASC ");
            sql.Append("    , HJ.組合員等コード ASC ");
            sql.Append("    , HJ.共済目的コード ASC ");
            sql.Append("    , HJ.類区分 ASC ");
        }
        #endregion

        #endregion

        #region 帳票データ取得メソッド群（加入承諾書兼共済掛金等払込通知書Sub1）

        #region 帳票データ取得メソッド（加入承諾書兼共済掛金等払込通知書Sub1）
        /// <summary>
        /// 作成対象とする帳票データを取得する（加入承諾書兼共済掛金等払込通知書Sub1）
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>帳票データ</returns>
        private List<NSK_P107010Sub1Model> GetReportSub1DataList(List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            //WITH句を取得する
            GetWithHJ(sql);
            GetWithSuiKey(sql);
            GetWithSuiItems(sql);
            GetWithMugiKey(sql);
            GetWithMugiItems(sql);
            // 陸稲SQLを取得する
            GetSub1RikuCondition(sql);
            GetReportSub1RikuDataTableCondition(sql);
            GetSub1RikuQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters);
            // 水稲(INDEX以外)SQLを取得する
            GetSub1SuiOtherCondition(sql);
            GetReportSub1SuiOtherDataTableCondition(sql);
            GetSub1SuiOtherQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters);
            // 水稲(INDEX)SQLを取得する
            GetSub1SuiIndexCondition(sql);
            GetReportSub1SuiIndexDataTableCondition(sql);
            GetSub1SuiIndexQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters);
            GetSub1SuiOtherGroupby(sql);
            // 麦SQLを取得する
            GetSub1MugiCondition(sql);
            GetReportSub1MugiDataTableCondition(sql);
            GetSub1MugiQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters);
            GetSub1MugiGroupby(sql);

            logger.Info("作成対象とする帳票データを取得する。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<NSK_P107010Sub1Model>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region 検索項目取得（加入承諾書兼共済掛金等払込通知書Sub1 陸稲）
        /// <summary>
        /// 検索項目を取得する（加入承諾書兼共済掛金等払込通知書Sub1 陸稲）
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetSub1RikuCondition(StringBuilder sql)
        {
            sql.Append("SELECT ");
            sql.Append("    HJ.組合等コード  AS \"組合等コード\" ");
            sql.Append("    , HJ.年産 AS \"年産\" ");
            sql.Append("    , HJ.共済目的コード AS \"共済目的コード\" ");
            sql.Append("    , HJ.支所コード AS \"支所コード\" ");
            sql.Append("    , HJ.引受回 AS \"引受回\" ");
            sql.Append("    , HJ.類区分 AS \"類区分\"");
            sql.Append("    , HJ.統計単位地域コード AS \"統計単位地域コード\"");
            sql.Append("    , HJ.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , m_00020_類名称.類短縮名称 AS \"類短縮名称\"");
            sql.Append("    , m_10080_引受方式名称.引受方式名称 AS \"引受方式名称\"");
            sql.Append("    , HJ.補償割合コード AS \"補償割合コード\"");
            sql.Append("    , m_20030_補償割合名称.補償割合短縮名称 AS \"補償割合短縮名称\"");
            sql.Append("    , HJ.特約区分 AS \"特約区分\"");
            sql.Append("    , HJ.共済金額選択順位 AS \"共済金額選択順位\"");
            sql.Append("    , '' AS \"備考\"");
            sql.Append("    , m_00070_収穫量確認方法名称.加入申込区分 AS \"加入申込区分\"");
            //以下は帳票出力用項目、設定は帳票作成クラスで実施
            sql.Append("    , '' AS \"一筆半損特約の有無_表示\"");
            sql.Append("    , '' AS \"全相殺方式等の収穫量の確認方法_表示\"");
            sql.Append("FROM ");
        }
        #endregion

        #region 検索テーブル取得(加入承諾書兼共済掛金等払込通知書Sub1 陸稲)
        /// <summary>
        /// 検索テーブルを取得する(加入承諾書兼共済掛金等払込通知書Sub1 陸稲)
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetReportSub1RikuDataTableCondition(StringBuilder sql)
        {
            sql.Append("HJ ");
            sql.Append("INNER JOIN m_00020_類名称 ");
            sql.Append("    ON  HJ.共済目的コード = m_00020_類名称.共済目的コード ");
            sql.Append("    AND HJ.類区分         = m_00020_類名称.類区分 ");
            sql.Append("INNER JOIN m_10080_引受方式名称 ");
            sql.Append("    ON  HJ.引受方式       = m_10080_引受方式名称.引受方式 ");
            sql.Append("INNER JOIN m_20030_補償割合名称 ");
            sql.Append("    ON  HJ.補償割合コード = m_20030_補償割合名称.補償割合コード ");
            sql.Append("LEFT JOIN m_00070_収穫量確認方法名称 ");
            sql.Append("    ON  HJ.収穫量確認方法 = m_00070_収穫量確認方法名称.収穫量確認方法 ");
        }
        #endregion

        #region 検索条件を取得する(加入承諾書兼共済掛金等払込通知書Sub1 陸稲)
        /// <summary>
        /// 検索条件を取得する。(加入承諾書兼共済掛金等払込通知書Sub1 陸稲)
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="reportJoukens">検索条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSub1RikuQueryCondition_Where(StringBuilder sql, List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd, List<NpgsqlParameter> parameters)
        {
            //共通条件設定
            GetCommonQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters, "HJ");

            sql.Append("AND HJ.共済目的コード =  '" + ((int)NskCoreConst.KyosaiMokutekiCdNumber.Rikutou).ToString() + "'");
            sql.Append("AND HJ.類区分 <> '0' ");
        }
        #endregion

        #region 検索項目取得（加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス以外）
        /// <summary>
        /// 検索項目を取得する（加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス以外）
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetSub1SuiOtherCondition(StringBuilder sql)
        {
            sql.Append("UNION ALL ");
            sql.Append("SELECT ");
            sql.Append("    HJ.組合等コード  AS \"組合等コード\" ");
            sql.Append("    , HJ.年産 AS \"年産\" ");
            sql.Append("    , HJ.共済目的コード AS \"共済目的コード\" ");
            sql.Append("    , HJ.支所コード AS \"支所コード\" ");
            sql.Append("    , HJ.引受回 AS \"引受回\" ");
            sql.Append("    , HJ.類区分 AS \"類区分\"");
            sql.Append("    , HJ.統計単位地域コード AS \"統計単位地域コード\"");
            sql.Append("    , HJ.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , m_00020_類名称.類短縮名称 AS \"類短縮名称\"");
            sql.Append("    , m_10080_引受方式名称.引受方式名称 AS \"引受方式名称\"");
            sql.Append("    , HJ.補償割合コード AS \"補償割合コード\"");
            sql.Append("    , m_20030_補償割合名称.補償割合短縮名称 AS \"補償割合短縮名称\"");
            sql.Append("    , HJ.特約区分 AS \"特約区分\"");
            sql.Append("    , HJ.共済金額選択順位 AS \"共済金額選択順位\"");
            sql.Append("    , '' AS \"備考\"");
            sql.Append("    , m_00070_収穫量確認方法名称.加入申込区分 AS \"加入申込区分\"");
            //以下は帳票出力用項目、設定は帳票作成クラスで実施
            sql.Append("    , '' AS \"一筆半損特約の有無_表示\"");
            sql.Append("    , '' AS \"全相殺方式等の収穫量の確認方法_表示\"");
            sql.Append("FROM ");
        }
        #endregion

        #region 検索テーブル取得(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス以外)
        /// <summary>
        /// 検索テーブルを取得する(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス以外)
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetReportSub1SuiOtherDataTableCondition(StringBuilder sql)
        {
            sql.Append("HJ ");
            sql.Append("INNER JOIN m_00020_類名称 ");
            sql.Append("    ON  HJ.共済目的コード = m_00020_類名称.共済目的コード ");
            sql.Append("    AND HJ.類区分         = m_00020_類名称.類区分 ");
            sql.Append("INNER JOIN m_10080_引受方式名称");
            sql.Append("    ON  HJ.引受方式       = m_10080_引受方式名称.引受方式 ");
            sql.Append("INNER JOIN m_20030_補償割合名称 ");
            sql.Append("    ON  HJ.補償割合コード = m_20030_補償割合名称.補償割合コード ");
            sql.Append("LEFT JOIN m_00070_収穫量確認方法名称 ");
            sql.Append("    ON  HJ.収穫量確認方法 = m_00070_収穫量確認方法名称.収穫量確認方法 ");
        }
        #endregion

        #region 検索条件を取得する(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス以外)
        /// <summary>
        /// 検索条件を取得する。(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス以外)
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="reportJoukens">検索条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSub1SuiOtherQueryCondition_Where(StringBuilder sql, List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd, List<NpgsqlParameter> parameters)
        {
            //共通条件設定
            GetCommonQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters, "HJ");

            sql.Append("AND HJ.共済目的コード =  '" + ((int)NskCoreConst.KyosaiMokutekiCdNumber.Suitou).ToString() + "' ");
            sql.Append("AND HJ.引受方式 <> '"+ HIKIUKE_HOUSIKI_INDEX + "' ");
            sql.Append("AND HJ.類区分 <> '0' ");
        }
        #endregion

        #region 検索項目取得（加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス）
        /// <summary>
        /// 検索項目を取得する（加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス）
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetSub1SuiIndexCondition(StringBuilder sql)
        {
            sql.Append("UNION ALL ");
            sql.Append("SELECT ");
            sql.Append("    SUI_KEY.組合等コード  AS \"組合等コード\" ");
            sql.Append("    , SUI_KEY.年産 AS \"年産\" ");
            sql.Append("    , SUI_KEY.共済目的コード AS \"共済目的コード\" ");
            sql.Append("    , SUI_KEY.支所コード AS \"支所コード\" ");
            sql.Append("    , SUI_KEY.引受回 AS \"引受回\" ");
            sql.Append("    , SUI_KEY.類区分 AS \"類区分\"");
            sql.Append("    , SUI_KEY.統計単位地域コード AS \"統計単位地域コード\"");
            sql.Append("    , SUI_KEY.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , m_00020_類名称.類短縮名称 AS \"類短縮名称\"");
            sql.Append("    , m_10080_引受方式名称.引受方式名称 AS \"引受方式名称\"");
            sql.Append("    , SUI_KEY.補償割合コード AS \"補償割合コード\"");
            sql.Append("    , m_20030_補償割合名称.補償割合短縮名称 AS \"補償割合短縮名称\"");
            sql.Append("    , SUI_KEY.特約区分 AS \"特約区分\"");
            sql.Append("    , SUI_KEY.共済金額選択順位 AS \"共済金額選択順位\"");
            sql.Append("    , STRING_AGG(");
            sql.Append("         CASE ");
            sql.Append("             WHEN SUI_ITEMS.引受区分名称 IS NOT NULL ");
            sql.Append("                THEN SUI_ITEMS.引受区分名称 || 'は第' || SUI_ITEMS.共済金額選択順位 || '位'");
            sql.Append("             ELSE ''");
            sql.Append("         END");
            sql.Append("         , ', ' ");
            sql.Append("         ORDER BY SUI_ITEMS.作付時期, SUI_ITEMS.種類区分 ");
            sql.Append("      )  AS \"備考\"");
            sql.Append("    , '' AS \"加入申込区分\"");
            //以下は帳票出力用項目、設定は帳票作成クラスで実施
            sql.Append("    , '' AS \"一筆半損特約の有無_表示\"");
            sql.Append("    , '' AS \"全相殺方式等の収穫量の確認方法_表示\"");
            sql.Append("FROM ");
        }
        #endregion

        #region 検索テーブル取得(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス)
        /// <summary>
        /// 検索テーブルを取得する(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス)
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetReportSub1SuiIndexDataTableCondition(StringBuilder sql)
        {
            sql.Append("SUI_KEY ");
            sql.Append("LEFT JOIN SUI_ITEMS ");
            sql.Append("    ON  SUI_KEY.組合等コード   = SUI_ITEMS.組合等コード ");
            sql.Append("    AND SUI_KEY.組合員等コード = SUI_ITEMS.組合員等コード ");
            sql.Append("    AND SUI_KEY.類区分         = SUI_ITEMS.類区分 ");
            sql.Append("INNER JOIN m_00020_類名称 ");
            sql.Append("    ON  SUI_KEY.共済目的コード = m_00020_類名称.共済目的コード ");
            sql.Append("    AND SUI_KEY.類区分         = m_00020_類名称.類区分 ");
            sql.Append("INNER JOIN m_10080_引受方式名称 ");
            sql.Append("    ON  SUI_KEY.引受方式       = m_10080_引受方式名称.引受方式 ");
            sql.Append("INNER JOIN m_20030_補償割合名称 ");
            sql.Append("    ON  SUI_KEY.補償割合コード = m_20030_補償割合名称.補償割合コード ");
        }
        #endregion

        #region 検索条件を取得する(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス)
        /// <summary>
        /// 検索条件を取得する。(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス)
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="reportJoukens">検索条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSub1SuiIndexQueryCondition_Where(StringBuilder sql, List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd, List<NpgsqlParameter> parameters)
        {
            //共通条件設定
            GetCommonQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters, "SUI_KEY");
        }
        #endregion

        #region クエリ式にGROUP設定(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス)
        /// <summary>
        /// GROUP設定処理(加入承諾書兼共済掛金等払込通知書Sub1 水稲インデックス)
        /// </summary>
        /// <param name="sql">sql</param>
        private void GetSub1SuiOtherGroupby(StringBuilder sql)
        {
            sql.Append("GROUP BY ");
            sql.Append("    SUI_KEY.組合等コード ");
            sql.Append("    , SUI_KEY.年産 ");
            sql.Append("    , SUI_KEY.共済目的コード ");
            sql.Append("    , SUI_KEY.支所コード ");
            sql.Append("    , SUI_KEY.引受回 ");
            sql.Append("    , SUI_KEY.類区分 ");
            sql.Append("    , SUI_KEY.統計単位地域コード ");
            sql.Append("    , SUI_KEY.組合員等コード ");
            sql.Append("    , m_00020_類名称.類短縮名称 ");
            sql.Append("    , m_10080_引受方式名称.引受方式名称 ");
            sql.Append("    , SUI_KEY.補償割合コード ");
            sql.Append("    , m_20030_補償割合名称.補償割合短縮名称 ");
            sql.Append("    , SUI_KEY.特約区分 ");
            sql.Append("    , SUI_KEY.共済金額選択順位 ");
        }
        #endregion

        #region 検索項目取得（加入承諾書兼共済掛金等払込通知書Sub1 麦）
        /// <summary>
        /// 検索項目を取得する（加入承諾書兼共済掛金等払込通知書Sub1 麦）
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetSub1MugiCondition(StringBuilder sql)
        {
            sql.Append("UNION ALL ");
            sql.Append("SELECT ");
            sql.Append("    MUGI_KEY.組合等コード  AS \"組合等コード\" ");
            sql.Append("    , MUGI_KEY.年産 AS \"年産\" ");
            sql.Append("    , MUGI_KEY.共済目的コード AS \"共済目的コード\" ");
            sql.Append("    , MUGI_KEY.支所コード AS \"支所コード\" ");
            sql.Append("    , MUGI_KEY.引受回 AS \"引受回\" ");
            sql.Append("    , MUGI_KEY.類区分 AS \"類区分\"");
            sql.Append("    , MUGI_KEY.統計単位地域コード AS \"統計単位地域コード\"");
            sql.Append("    , MUGI_KEY.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , m_00020_類名称.類短縮名称 AS \"類短縮名称\"");
            sql.Append("    , m_10080_引受方式名称.引受方式名称 AS \"引受方式名称\"");
            sql.Append("    , MUGI_KEY.補償割合コード AS \"補償割合コード\"");
            sql.Append("    , m_20030_補償割合名称.補償割合短縮名称 AS \"補償割合短縮名称\"");
            sql.Append("    , MUGI_KEY.特約区分 AS \"特約区分\"");
            sql.Append("    , MUGI_KEY.共済金額選択順位 AS \"共済金額選択順位\"");
            sql.Append("    , STRING_AGG(");
            sql.Append("         CASE ");
            sql.Append("             WHEN MUGI_ITEMS.用途名称 IS NOT NULL ");
            sql.Append("                THEN MUGI_ITEMS.用途名称 || 'は第' || MUGI_ITEMS.共済金額選択順位 || '位'");
            sql.Append("             ELSE ''");
            sql.Append("         END");
            sql.Append("         , ', ' ");
            sql.Append("         ORDER BY MUGI_ITEMS.作付時期, MUGI_ITEMS.種類区分, MUGI_ITEMS.用途区分 ");
            sql.Append("      )  AS \"備考\"");
            sql.Append("    , m_00070_収穫量確認方法名称.加入申込区分 AS \"加入申込区分\"");
            //以下は帳票出力用項目、設定は帳票作成クラスで実施
            sql.Append("    , '' AS \"一筆半損特約の有無_表示\"");
            sql.Append("    , '' AS \"全相殺方式等の収穫量の確認方法_表示\"");
            sql.Append("FROM ");
        }
        #endregion

        #region 検索テーブル取得(加入承諾書兼共済掛金等払込通知書Sub1 麦)
        /// <summary>
        /// 検索テーブルを取得する(加入承諾書兼共済掛金等払込通知書Sub1 麦)
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetReportSub1MugiDataTableCondition(StringBuilder sql)
        {
            sql.Append("MUGI_KEY ");
            sql.Append("LEFT JOIN MUGI_ITEMS ");
            sql.Append("    ON  MUGI_KEY.組合等コード   = MUGI_ITEMS.組合等コード ");
            sql.Append("    AND MUGI_KEY.組合員等コード = MUGI_ITEMS.組合員等コード ");
            sql.Append("    AND MUGI_KEY.類区分         = MUGI_ITEMS.類区分 ");
            sql.Append("INNER JOIN m_00020_類名称 ");
            sql.Append("    ON  MUGI_KEY.共済目的コード = m_00020_類名称.共済目的コード ");
            sql.Append("    AND MUGI_KEY.類区分         = m_00020_類名称.類区分 ");
            sql.Append("INNER JOIN m_10080_引受方式名称 ");
            sql.Append("    ON  MUGI_KEY.引受方式       = m_10080_引受方式名称.引受方式 ");
            sql.Append("INNER JOIN m_20030_補償割合名称 ");
            sql.Append("    ON  MUGI_KEY.補償割合コード = m_20030_補償割合名称.補償割合コード ");
            sql.Append("LEFT JOIN m_00070_収穫量確認方法名称 ");
            sql.Append("    ON  MUGI_KEY.収穫量確認方法 = m_00070_収穫量確認方法名称.収穫量確認方法 ");
        }
        #endregion

        #region 検索条件を取得する(加入承諾書兼共済掛金等払込通知書Sub1 麦)
        /// <summary>
        /// 検索条件を取得する。(加入承諾書兼共済掛金等払込通知書Sub1 麦)
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="reportJoukens">検索条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSub1MugiQueryCondition_Where(StringBuilder sql, List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd, List<NpgsqlParameter> parameters)
        {
            //共通条件設定
            GetCommonQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters, "MUGI_KEY");
        }
        #endregion

        #region クエリ式にGROUP設定(加入承諾書兼共済掛金等払込通知書Sub1 麦)
        /// <summary>
        /// GROUP設定処理(加入承諾書兼共済掛金等払込通知書Sub1 麦)
        /// </summary>
        /// <param name="sql">sql</param>
        private void GetSub1MugiGroupby(StringBuilder sql)
        {
            sql.Append("GROUP BY ");
            sql.Append("    MUGI_KEY.組合等コード ");
            sql.Append("    , MUGI_KEY.年産 ");
            sql.Append("    , MUGI_KEY.共済目的コード ");
            sql.Append("    , MUGI_KEY.支所コード ");
            sql.Append("    , MUGI_KEY.引受回 ");
            sql.Append("    , MUGI_KEY.類区分 ");
            sql.Append("    , MUGI_KEY.統計単位地域コード ");
            sql.Append("    , MUGI_KEY.組合員等コード ");
            sql.Append("    , m_00020_類名称.類短縮名称 ");
            sql.Append("    , m_10080_引受方式名称.引受方式名称 ");
            sql.Append("    , MUGI_KEY.補償割合コード ");
            sql.Append("    , m_20030_補償割合名称.補償割合短縮名称 ");
            sql.Append("    , MUGI_KEY.特約区分 ");
            sql.Append("    , MUGI_KEY.共済金額選択順位 ");
            sql.Append("    , m_00070_収穫量確認方法名称.加入申込区分 ");
        }
        #endregion

        #endregion

        #region 帳票データ取得メソッド群（加入承諾書兼共済掛金等払込通知書Sub2）

        #region 帳票データ取得メソッド（加入承諾書兼共済掛金等払込通知書Sub2）
        /// <summary>
        /// 作成対象とする帳票データを取得する（加入承諾書兼共済掛金等払込通知書Sub2）
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>帳票データ</returns>
        private List<NSK_P107010Sub2Model> GetReportSub2DataList(List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            //WITH句を取得する
            GetWithHJ(sql);
            // 検索項目を取得する
            GetSub2Condition(sql);
            // 検索テーブルを取得する
            GetReportSub2DataTableCondition(sql);
            // 検索条件を取得する
            GetSub2QueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters);
            // ソート順を追加する
            GetSub2Orderby(sql);

            logger.Info("作成対象とする帳票データを取得する。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<NSK_P107010Sub2Model>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region 検索項目取得（加入承諾書兼共済掛金等払込通知書Sub2）
        /// <summary>
        /// 検索項目を取得する（加入承諾書兼共済掛金等払込通知書Sub2）
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetSub2Condition(StringBuilder sql)
        {
            sql.Append("SELECT ");
            sql.Append("    HJ.組合等コード  AS \"組合等コード\" ");
            sql.Append("    , HJ.年産 AS \"年産\" ");
            sql.Append("    , HJ.共済目的コード AS \"共済目的コード\" ");
            sql.Append("    , HJ.引受回 AS \"引受回\" ");
            sql.Append("    , HJ.支所コード AS \"支所コード\" ");
            sql.Append("    , HJ.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , HJ.類区分 AS \"類区分\" ");
            sql.Append("    , HJ.統計単位地域コード AS \"統計単位地域コード\" ");
            sql.Append("    , v_shichoson_nm.shichoson_nm AS \"市町村名\" ");
            sql.Append("    , HJ.引受筆数 AS \"引受筆数\" ");
            sql.Append("    , HJ.引受面積計 AS \"引受面積計\" ");
            sql.Append("    , HJ.基準収穫量計 AS \"基準収穫量計\" ");
            sql.Append("    , HJ.引受収量 AS \"引受収量\" ");
            sql.Append("    , HJ.共済金額単価 AS \"共済金額単価\" ");
            sql.Append("    , HJ.共済金額 AS \"共済金額\" ");
            sql.Append("    , HJ.危険段階地域区分 AS \"危険段階地域区分\" ");
            sql.Append("    , HJ.危険段階区分 AS \"危険段階区分\" ");
            sql.Append("    , HJ.基準共済掛金率 AS \"基準共済掛金率\" ");
            sql.Append("    , HJ.共済掛金 AS \"共済掛金\" ");
            sql.Append("    , HJ.交付対象負担金 AS \"交付対象負担金\" ");
            sql.Append("    , HJ.組合員等負担共済掛金 AS \"組合員等負担共済掛金\" ");
            sql.Append("    , HJ.賦課金計 AS \"賦課金計\" ");
            // 以下は帳票出力用項目、設定は帳票作成クラスで実施
            sql.Append("    , '' AS  \"危険段階区分_表示\" ");
        }
        #endregion

        #region 検索テーブル取得(加入承諾書兼共済掛金等払込通知書Sub2)
        /// <summary>
        /// 検索テーブルを取得する(加入承諾書兼共済掛金等払込通知書Sub2)
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetReportSub2DataTableCondition(StringBuilder sql)
        {
            sql.Append("FROM ");
            sql.Append("    HJ ");
            sql.Append("LEFT JOIN v_nogyosha ");
            sql.Append("    ON HJ.組合等コード = v_nogyosha.kumiaito_cd ");
            sql.Append("    AND HJ.組合員等コード = v_nogyosha.kumiaiinto_cd ");
            sql.Append("LEFT JOIN v_shichoson_nm ");
            sql.Append("    ON v_nogyosha.kumiaito_cd = v_shichoson_nm.kumiaito_cd ");
            sql.Append("    AND v_nogyosha.shichoson_cd = v_shichoson_nm.shichoson_cd ");
        }
        #endregion

        #region 検索条件を取得する(加入承諾書兼共済掛金等払込通知書Sub2)
        /// <summary>
        /// 検索条件を取得する。(加入承諾書兼共済掛金等払込通知書Sub2)
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="reportJoukens">検索条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSub2QueryCondition_Where(StringBuilder sql, List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd, List<NpgsqlParameter> parameters)
        {
            //共通条件設定
            GetCommonQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters, "HJ");

            sql.Append("AND  HJ.類区分 <> '0' ");
        }
        #endregion

        #region クエリ式にソート順設定(加入承諾書兼共済掛金等払込通知書Sub2)
        /// <summary>
        /// ソート順設定処理(加入承諾書兼共済掛金等払込通知書Sub2)
        /// </summary>
        /// <param name="sql">sql</param>
        private void GetSub2Orderby(StringBuilder sql)
        {
            sql.Append("ORDER BY ");
            sql.Append("    HJ.支所コード ASC ");
            sql.Append("    , HJ.組合員等コード ASC ");
            sql.Append("    , HJ.共済目的コード ASC ");
        }
        #endregion

        #endregion

    }
}
