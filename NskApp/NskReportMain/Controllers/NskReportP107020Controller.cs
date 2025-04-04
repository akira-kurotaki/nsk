using NskAppModelLibrary.Context;
using NskReportMain.Common;
using NskReportMain.Models.NSK_P107020;
using NskReportMain.ReportCreators.NSK_P107020;
using NskReportMain.Reports;
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
using CoreLibrary.Core.Consts;
using NskAppModelLibrary.Models;

namespace NskReportMain.Controllers
{
    public class NskReportP107020Controller : ReportController
    {

        #region メンバー定数
        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "P107020_耕地等情報（６号）_制御処理";
        #endregion

        public NskReportP107020Controller(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        #region P107020_耕地等情報（６号）_制御処理_制御処理
        /// <summary>
        /// P107020_耕地等情報（６号）_制御処理_制御処理
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
                    JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,            // 共済目的コード
                    JoukenNameConst.JOUKEN_DAICHIKU,                      // 大地区コード
                    JoukenNameConst.JOUKEN_SHOCHIKU_START,                // 小地区コード（開始）
                    JoukenNameConst.JOUKEN_SHOCHIKU_END,                  // 小地区コード（終了）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,           // 組合員等コード（開始）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,             // 組合員等コード（終了）
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
                var pModelList = GetReportDataList(reportJoukens, kumiaitoCd, shishoCd);
                if (pModelList.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME10076", "0");
                }

                // 帳票PDF一時出力フォルダ作成
                // 定数取得(（定数ID：PrintTempFolder）)\バッチID_システム日時(yyyyMMddHHmmss)
                var printTempFolder = NskCommonUtil.FolderUtil.CreateTempFolder(sysDate, batchId.ToString());

                // 帳票オブジェクト
                var report = new BaseSectionReport();
                // 帳票出力処理を呼び出す。
                result = CreateP107020(reportJoukens, pModelList, joukenId, ref report);
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

        #region 「P107020_耕地等情報（６号）」を作成するメソッド
        /// <summary>
        /// 「P107020_耕地等情報（６号）」を作成するメソッド
        /// </summary>
        /// <param name="reportJoukens">T01050バッチ条件リスト</param>
        /// <param name="model">帳票モデルリスト</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="report">帳票オブジェクト</param>
        /// <returns>実行結果</returns>
        private ControllerResult CreateP107020(
            List<T01050バッチ条件> reportJoukens,
            List<NSK_P107020Model> model,
            string joukenId,
            ref BaseSectionReport report)
        {
            // 実行結果
            var result = new ControllerResult();
            NSK_P107020Creator pCreator = new NSK_P107020Creator();
            CreatorResult creatorResult = pCreator.CreateReport(joukenId, reportJoukens, model);
            // 失敗した場合
            if (creatorResult.Result == ReportConst.RESULT_FAILED)
            {
                return result.ControllerResultError(creatorResult.ErrorMessage, creatorResult.ErrorMessageId);
            }

            // 出力レポートにP107020_耕地等情報（６号）を入れる。
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

        #region 帳票データ取得メソッド
        /// <summary>
        /// 作成対象とする帳票データを取得する。
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>帳票データ</returns>
        private List<NSK_P107020Model> GetReportDataList(List<T01050バッチ条件> reportJoukens, string kumiaitoCd, string shishoCd)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetReportDataTableCondition(sql);
            // 検索条件を取得する
            GetQueryCondition_Where(sql, reportJoukens, kumiaitoCd, shishoCd, parameters);
            // ソート順を追加する
            GetOrderby(sql);

            logger.Info("作成対象とする帳票データを取得する。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<NSK_P107020Model>(sql.ToString(), parameters.ToArray()).ToList();
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
            sql.Append("    v_nogyosha.hojin_full_kana AS \"氏名又は法人名フリガナ\" ");
            sql.Append("    , v_nogyosha.hojin_full_nm AS \"氏名又は法人名\" ");
            sql.Append("    , CASE WHEN v_nogyosha.postal_cd <> '' AND v_nogyosha.postal_cd IS NOT NULL THEN '(〒' || LEFT(v_nogyosha.postal_cd, 3) || '-' || RIGHT(v_nogyosha.postal_cd, 4) || ')' ELSE v_nogyosha.postal_cd END AS \"郵便番号\" ");
            sql.Append("    , v_nogyosha.address AS \"住所\" ");
            sql.Append("    , v_nogyosha.tel AS \"電話番号\" ");
            sql.Append("    , t_11010_個人設定.加入形態 AS \"加入形態\" ");
            sql.Append("    , t_12010_引受結果.耕地番号 AS \"耕地番号\" ");
            sql.Append("    , t_12010_引受結果.分筆番号 AS \"分筆番号\" ");
            sql.Append("    , v_shichoson_nm.shichoson_nm AS \"市町村名\" ");
            sql.Append("    , t_12010_引受結果.地名地番 AS \"地名地番\" ");
            sql.Append("    , t_12010_引受結果.耕地面積 AS \"耕地面積\" ");
            sql.Append("    , t_12010_引受結果.引受面積 AS \"引受面積\" ");
            sql.Append("    , t_12010_引受結果.転作等面積 AS \"転作等面積\" ");
            sql.Append("    , CASE WHEN @KyosaiMokutekiCd <> '" + ((int)NskCoreConst.KyosaiMokutekiCdNumber.Rikutou).ToString() + "' THEN t_12010_引受結果.類区分 ELSE '' END AS \"類区分\" ");          //@はGetQueryCondition_Whereで埋め込み
            sql.Append("    , CASE WHEN @KyosaiMokutekiCd <> '" + ((int)NskCoreConst.KyosaiMokutekiCdNumber.Rikutou).ToString() + "' THEN m_00020_類名称.類短縮名称 ELSE '' END AS \"類短縮名称\" ");    //@はGetQueryCondition_Whereで埋め込み
            sql.Append("    , t_12010_引受結果.品種コード AS \"品種コード\" ");
            sql.Append("    , m_00110_品種係数.品種名等 AS \"品種名等\" ");
            sql.Append("    , m_00040_田畑名称.田畑名称 AS \"田畑名称\" ");
            sql.Append("    , t_12010_引受結果.受委託者コード AS \"受委託者コード\" ");
            sql.Append("    , t_12010_引受結果.収量等級コード AS \"収量等級コード\" ");
            sql.Append("    , m_10040_参酌係数.参酌係数 AS \"参酌係数\" ");
            sql.Append("    , t_12040_組合員等別引受情報.支所コード AS \"支所コード\" ");
            sql.Append("    , t_12040_組合員等別引受情報.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , t_12040_組合員等別引受情報.大地区コード AS \"大地区コード\" ");
            sql.Append("    , t_12040_組合員等別引受情報.小地区コード AS \"小地区コード\" ");
            // 出力制御用
            sql.Append("    , @KyosaiMokutekiCd AS \"共済目的コード\" ");
            // 以下は帳票出力用項目、設定は帳票作成クラスで実施
            sql.Append("    , FALSE AS \"個人\" ");
            sql.Append("    , FALSE AS \"法人\" ");
            sql.Append("    , FALSE AS \"農作物資格団体\" ");
            sql.Append("    , '' AS  \"類区分_表示\" ");
            sql.Append("    , '' AS  \"品種又は転作作物名等\" ");
            sql.Append("    , '' AS  \"栽培上の特殊事情\" ");
            sql.Append("    , '' AS  \"ページ右\" ");
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
            sql.Append("    t_12040_組合員等別引受情報 ");
            sql.Append("INNER JOIN  ( ");
            sql.Append("    SELECT ");
            sql.Append("        t_00010_引受回.組合等コード");
            sql.Append("        , t_00010_引受回.年産");
            sql.Append("        , t_00010_引受回.共済目的コード");
            sql.Append("        , t_00010_引受回.支所コード");
            sql.Append("        , MAX(t_00010_引受回.引受回) AS \"引受回\" ");
            sql.Append("    FROM");
            sql.Append("        t_00010_引受回");
            sql.Append("    GROUP BY ");
            sql.Append("        t_00010_引受回.組合等コード");
            sql.Append("        , t_00010_引受回.年産");
            sql.Append("        , t_00010_引受回.共済目的コード");
            sql.Append("        , t_00010_引受回.支所コード");
            sql.Append(") HK ");
            sql.Append("    ON  t_12040_組合員等別引受情報.組合等コード   = HK.組合等コード ");
            sql.Append("    AND t_12040_組合員等別引受情報.年産           = HK.年産 ");
            sql.Append("    AND t_12040_組合員等別引受情報.共済目的コード = HK.共済目的コード ");
            sql.Append("    AND t_12040_組合員等別引受情報.支所コード     = HK.支所コード ");
            sql.Append("    AND t_12040_組合員等別引受情報.引受回         = HK.引受回 ");
            sql.Append("LEFT JOIN v_nogyosha ");
            sql.Append("    ON  t_12040_組合員等別引受情報.組合等コード   = v_nogyosha.kumiaito_cd ");
            sql.Append("    AND t_12040_組合員等別引受情報.組合員等コード = v_nogyosha.kumiaiinto_cd ");
            sql.Append("LEFT JOIN t_11010_個人設定 ");
            sql.Append("    ON  t_12040_組合員等別引受情報.組合等コード   = t_11010_個人設定.組合等コード ");
            sql.Append("    AND t_12040_組合員等別引受情報.年産           = t_11010_個人設定.年産 ");
            sql.Append("    AND t_12040_組合員等別引受情報.共済目的コード = t_11010_個人設定.共済目的コード ");
            sql.Append("    AND t_12040_組合員等別引受情報.組合員等コード = t_11010_個人設定.組合員等コード ");
            sql.Append("INNER JOIN m_00010_共済目的名称 ");
            sql.Append("    ON t_12040_組合員等別引受情報.共済目的コード  = m_00010_共済目的名称.共済目的コード ");
            sql.Append("INNER JOIN t_12010_引受結果 ");
            sql.Append("    ON  t_12040_組合員等別引受情報.組合等コード   = t_12010_引受結果.組合等コード ");
            sql.Append("    AND t_12040_組合員等別引受情報.年産           = t_12010_引受結果.年産 ");
            sql.Append("    AND t_12040_組合員等別引受情報.共済目的コード = t_12010_引受結果.共済目的コード ");
            sql.Append("    AND t_12040_組合員等別引受情報.組合員等コード = t_12010_引受結果.組合員等コード ");
            sql.Append("LEFT JOIN v_shichoson_nm ");
            sql.Append("    ON  v_nogyosha.kumiaito_cd  = v_shichoson_nm.kumiaito_cd ");
            sql.Append("    AND v_nogyosha.shichoson_cd = v_shichoson_nm.shichoson_cd ");
            sql.Append("LEFT JOIN m_00020_類名称 ");
            sql.Append("    ON t_12010_引受結果.共済目的コード = m_00020_類名称.共済目的コード ");
            sql.Append("    AND t_12010_引受結果.類区分        = m_00020_類名称.類区分 ");
            sql.Append("LEFT JOIN m_00110_品種係数 ");
            sql.Append("    ON t_12010_引受結果.組合等コード    = m_00110_品種係数.組合等コード ");
            sql.Append("    AND t_12010_引受結果.年産           = m_00110_品種係数.年産 ");
            sql.Append("    AND t_12010_引受結果.共済目的コード = m_00110_品種係数.共済目的コード ");
            sql.Append("    AND t_12010_引受結果.品種コード     = m_00110_品種係数.品種コード ");
            sql.Append("LEFT JOIN m_00040_田畑名称 ");
            sql.Append("    ON t_12010_引受結果.田畑区分 = m_00040_田畑名称.田畑区分 ");
            sql.Append("LEFT JOIN m_10040_参酌係数 ");
            sql.Append("    ON t_12010_引受結果.組合等コード    = m_10040_参酌係数.組合等コード ");
            sql.Append("    AND t_12010_引受結果.年産           = m_10040_参酌係数.年産 ");
            sql.Append("    AND t_12010_引受結果.共済目的コード = m_10040_参酌係数.共済目的コード ");
            sql.Append("    AND t_12010_引受結果.参酌コード     = m_10040_参酌係数.参酌コード ");
            sql.Append("INNER JOIN m_00030_区分名称 ");
            sql.Append("    ON t_12010_引受結果.組合等コード    = m_00030_区分名称.組合等コード ");
            sql.Append("    AND t_12010_引受結果.年産           = m_00030_区分名称.年産 ");
            sql.Append("    AND t_12010_引受結果.共済目的コード = m_00030_区分名称.共済目的コード ");
            sql.Append("    AND t_12010_引受結果.区分コード     = m_00030_区分名称.区分コード ");
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
                sql.Append("AND t_12040_組合員等別引受情報.組合等コード = @KumiaitoCd ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd", kumiaitoCd));
            }

            // 年産
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_NENSAN).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND t_12040_組合員等別引受情報.年産 = @Nensan ");
                parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(jouken.条件値)));
            }

            // 共済目的コード
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND t_12040_組合員等別引受情報.共済目的コード = @KyosaiMokutekiCd ");
                parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", jouken.条件値));
            }

            // 支所
            if (!string.IsNullOrEmpty(shishoCd))
            {
                sql.Append("AND t_12040_組合員等別引受情報.支所コード = @ShishoCd ");
                parameters.Add(new NpgsqlParameter("@ShishoCd", shishoCd));
            }

            // 大地区
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_DAICHIKU).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND t_12040_組合員等別引受情報.大地区コード = @DaichikuCd ");
                parameters.Add(new NpgsqlParameter("@DaichikuCd", jouken.条件値));
            }

            // 小地区（開始）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_SHOCHIKU_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND t_12040_組合員等別引受情報.小地区コード >= @ShochikuCdFrom ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdFrom", jouken.条件値));
            }

            // 小地区（終了）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_SHOCHIKU_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND t_12040_組合員等別引受情報.小地区コード <= @ShochikuCdTo ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdTo", jouken.条件値));
            }

            // 組合員等コード（開始）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND t_12040_組合員等別引受情報.組合員等コード >= @KumiaiintoCdFrom ");
                parameters.Add(new NpgsqlParameter("@KumiaiintoCdFrom", jouken.条件値));
            }

            // 組合員等コード（終了）
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                sql.Append("AND t_12040_組合員等別引受情報.組合員等コード <= @KumiaiintoCdTo ");
                parameters.Add(new NpgsqlParameter("@KumiaiintoCdTo", jouken.条件値));
            }

            sql.Append("AND t_12040_組合員等別引受情報.類区分 = '0' ");
            sql.Append("AND t_12040_組合員等別引受情報.統計単位地域コード = '0' ");
            sql.Append("AND t_12040_組合員等別引受情報.引受対象フラグ = '" + ReportConst.FLG_ON + "' ");
            sql.Append("AND m_00030_区分名称.引受フラグ = '" + ReportConst.FLG_ON + "' ");
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
            sql.Append("    t_12040_組合員等別引受情報.支所コード ASC ");
            sql.Append("    , t_12040_組合員等別引受情報.大地区コード ASC ");
            sql.Append("    , t_12040_組合員等別引受情報.小地区コード ASC ");
            sql.Append("    , t_12040_組合員等別引受情報.組合員等コード ASC ");
            sql.Append("    , t_12010_引受結果.耕地番号 ASC ");
            sql.Append("    , t_12010_引受結果.分筆番号 ASC ");
        }
        #endregion
    }
}
