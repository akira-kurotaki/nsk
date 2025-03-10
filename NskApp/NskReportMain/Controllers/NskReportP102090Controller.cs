using NskAppModelLibrary.Context;
using NskReportMain.Common;
using NskReportMain.Models.P102090;
using NskReportMain.ReportCreators.P102090;
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
using NskAppModelLibrary.Models;

namespace NskReportMain.Controllers
{
    public class NskReportP102090Controller : ReportController
    {

        #region メンバー定数
        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "P102090_危険段階地域区分一覧表制御処理";
        #endregion

        public NskReportP102090Controller(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        #region P102090_危険段階地域区分一覧表制御処理
        /// <summary>
        /// P102090_危険段階地域区分一覧表制御処理
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="filePath">帳票パス</param>
        /// <param name="batchId">バッチID</param>/// 
        /// <returns>実行結果</returns>
        public ControllerResult ManageReports(string userId, string joukenId, string todofukenCd, string kumiaitoCd, string shishoCd, string filePath, long batchId)
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
                        ReportConst.PARAM_NAME_TODOFUKEN_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + todofukenCd,
                        ReportConst.PARAM_NAME_KUMIAITO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + kumiaitoCd,
                        ReportConst.PARAM_NAME_SHISHO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + shishoCd,
                        ReportConst.PARAM_NAME_FILE_PATH + ReportConst.PARAM_NAME_VALUE_SEPARATOR + filePath,
                        ReportConst.PARAM_NAME_BATCH_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + (batchId.ToString())})));

            // 実行結果
            var result = new ControllerResult();
            try
            {
                // 引数チェック
                result = CheckParams(result, userId, joukenId, todofukenCd, kumiaitoCd, shishoCd, filePath, batchId);
                if (ReportConst.RESULT_FAILED.Equals(result.Result))
                {
                    return result;
                }

                var model = new P102090Model();

                // 帳票出力条件を取得する。
                // 条件IDをキーに帳票条件テーブルより帳票出力条件を取得する。
                model.P102090BatchJouken = GetBatchJouken(getJigyoDb<NskAppContext>(), joukenId);

                // 条件チェック
                // 年産または共済目的コードまたは類区分が取得できない場合はエラー
                if (model.P102090BatchJouken.nensan.IsNullOrEmpty() || model.P102090BatchJouken.kyosaiMokutekiCd.IsNullOrEmpty() ||
                    model.P102090BatchJouken.ruikbn.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME90010");
                }

                // 帳票オブジェクト
                var report = new BaseSectionReport();

                // 帳票データを取得する。
                model.P102090TableRecords = GetReportDataList(getJigyoDb<NskAppContext>(), model.P102090BatchJouken, kumiaitoCd);

                // 帳票出力処理を呼び出す。
                result = CreateP102090(model, joukenId, ref report);
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

                // 帳票PDF一時出力フォルダ作成
                // 引数：バッチフラグが1（バッチ）の場合
                // 以下の帳票PDF一時出力フォルダを作成する。
                // 定数取得(（定数ID：PrintTempFolder）)\ユーザID_グローバル一意識別子(GUID)\（引数：帳票パスのファイル名（拡張子を除く））
                // （例）引数：帳票パスがC:\aaa\bbb\ccc\tyouhyou.zipの場合
                // 出力フォルダは、定数取得(（定数ID：PrintTempFolder）)\ユーザID_91b84368272946718288a66acc9e078f\tyouhyou
                var printTempFolder = FolderUtil.CreatePrintTempFolder(userId, filePath);

                // ファイル名は「11_危険段階地域区分一覧表.PDF」とする。
                pdfExport.Export(report.Document, Path.Combine(printTempFolder, userId + "_" + model.dateTimeToString.ToString("yyyyMMddHHmmss") + "_" + "危険段階地域区分一覧表" + ReportConst.REPORT_EXTENSION));

                // PDF出力用一時フォルダをzip化（暗号化）する。
                var zipFilePath = ZipUtil.CreateZip(printTempFolder);

                // Zipファイルを引数：帳票パスに移動する
                FolderUtil.MoveFile(zipFilePath, filePath, userId, batchId);

                // PDF出力用一時フォルダを削除する
                FolderUtil.DeleteTempFolder(printTempFolder);
                

                result.Result = ReportConst.RESULT_SUCCESS;
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

        #region 「P102090_危険段階地域区分一覧表」を作成するメソッド
        /// <summary>
        /// 「P102090_危険段階地域区分一覧表」を作成するメソッド
        /// </summary>
        /// <param name="model">帳票モデルリスト</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="report">帳票オブジェクト</param>
        /// <returns>実行結果</returns>
        private ControllerResult CreateP102090(
            P102090Model model,
            string joukenId,
            ref BaseSectionReport report)
        {
            // 実行結果
            var result = new ControllerResult();
            P102090Creator p102090Creator = new P102090Creator();
            CreatorResult creatorResult = p102090Creator.CreateReport(joukenId, model);
            // 失敗した場合
            if (creatorResult.Result == ReportConst.RESULT_FAILED)
            {
                return result.ControllerResultError(creatorResult.ErrorMessage, creatorResult.ErrorMessageId);
            }

            // ページ番号を描画する
            creatorResult.SectionReport = ReportPagerUtil.DrawReportPageNumber(creatorResult.SectionReport, ReportConst.REPORT_BOTTOM_MARGIN_STANDARD);
            // 出力レポートにP102090_危険段階地域区分一覧表を入れる。
            report.Document.Pages.AddRange(creatorResult.SectionReport.Document.Pages);

            return result;
        }
        #endregion

        #region 引数チェック
        /// <summary>
        /// P102090_危険段階地域区分一覧表引数チェック
        /// </summary>
        /// <param name="result">実行結果</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="filePath">帳票パス</param>
        /// <returns>実行結果</returns>
        private ControllerResult CheckParams(ControllerResult result, string userId, string joukenId, string todofukenCd,
                                             string kumiaitoCd, string shishoCd, string filePath, long? batchId)
        {
            // ■１．引数チェックする
            // ユーザIDがない場合、エラーとし、エラーメッセージを返す
            if (string.IsNullOrEmpty(userId))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_USER_ID);
            }

            // 条件IDがない、または36桁ではない場合、エラーとし、エラーメッセージを返す
            if (String.IsNullOrEmpty(joukenId) && joukenId.Length != 36)
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

            // 帳票パスがない場合、エラーとし、エラーメッセージを返す。
            if (string.IsNullOrEmpty(filePath))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_FILE_PATH);
            }

            // バッチIDがない場合、エラーとし、エラーメッセージを返す。
            if (!batchId.HasValue)
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_BATCH_ID);
            }

            // コードの整合性チェックをする
            // 「都道府県コード存在情報」を取得する。
            // （１）データが取得できない場合（該当データがマスタデータに登録されていない場合）、エラーとし、エラーメッセージを返す。
            // （"{0}に、{1}に存在しないコードが存在します。"　引数{0} ：都道府県コード、引数{1} ：都道府県マスタ)
            logger.Info("「都道府県コード存在情報」を取得する。");
            var todofukenCnt = getJigyoDb<NskAppContext>().VTodofukens.Where(t => t.TodofukenCd == todofukenCd).Count();
            if (todofukenCnt == 0)
            {
                return result.ControllerResultError(string.Empty, "ME91003", ReportConst.PARAM_NAME_TODOFUKEN_CD, "都道府県マスタ");
            }

            // 「組合等コード存在情報」を取得する。
            // （１）データが取得できない場合（該当データがマスタデータに登録されていない場合）、エラーとし、エラーメッセージを返す。
            // （"{0}に、{1}に存在しないコードが存在します。"　引数{0} ：組合等コード、引数{1} ：組合等マスタ)
            logger.Info("「組合等コード存在情報」を取得する。");
            var kumiaitoCnt = getJigyoDb<NskAppContext>().VKumiaitos.Where(t => t.TodofukenCd == todofukenCd && t.KumiaitoCd == kumiaitoCd).Count();
            if (kumiaitoCnt == 0)
            {
                return result.ControllerResultError(string.Empty, "ME91003", ReportConst.PARAM_NAME_KUMIAITO_CD, "組合等マスタ");
            }

            // 引数：支所コードが入力されている場合、「支所コード存在情報」を取得する。
            // （１）データが取得できない場合（該当データがマスタデータに登録されていない場合）、エラーとし、エラーメッセージを返す。
            //（"{0}に、{1}に存在しないコードが存在します。"　引数{0} ：支所コード、引数{1} ：名称M支所)
            if (!string.IsNullOrEmpty(shishoCd))
            {
                logger.Info("「支所コード存在情報」を取得する。");
                var shishoCnt = getJigyoDb<NskAppContext>().VShishoNms.Where(t => t.TodofukenCd == todofukenCd && t.KumiaitoCd == kumiaitoCd && t.ShishoCd == shishoCd).Count();
                if (shishoCnt == 0)
                {
                    return result.ControllerResultError(string.Empty, "ME91003", ReportConst.PARAM_NAME_SHISHO_CD, "名称M支所");
                }
            }

            return result;
        }
        #endregion

        // TODO: 親クラスReportControllerの条件取得メソッドは参照テーブルが異なる
        #region バッチ条件取得
        /// <summary>
        /// バッチ条件取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <returns>バッチ条件</returns>
        private static P102090JoukenModel GetBatchJouken(NskAppContext db, string joukenId)
        {
            var validJoukenNames = new[]
            {
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_NENSAN,
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,  // 条件定数「共済目的コード」を要追加
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_RUIKUBUN,
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_DAICHIKU,
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_SHOCHIKU_START,
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_SHOCHIKU_END
            };

            // T01050バッチ条件のデータを取得し、P102090JoukenModelに変換して返す
            var batchJouken = db.Set<T01050バッチ条件>()
                                .AsNoTracking()
                                .Where(b => b.バッチ条件id == joukenId && validJoukenNames.Contains(b.条件名称))
                                .Select(b => new P102090JoukenModel
                                {
                                    nensan = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_NENSAN ? b.条件値 : null,
                                    kyosaiMokutekiCd = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD ? b.条件値 : null,
                                    ruikbn = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_RUIKUBUN ? b.条件値 : null,
                                    daichiku = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_DAICHIKU ? b.条件値 : null,
                                    shochikuStart = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_SHOCHIKU_START ? b.条件値 : null,
                                    shochikuEnd = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_SHOCHIKU_END ? b.条件値 : null
                                })
                                .FirstOrDefault();

            return batchJouken;
        }
        #endregion

        #region 帳票データ取得
        /// <summary>
        /// 帳票データを取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jouken">データ取得条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>帳票データ</returns>
        private List<P102090TableRecord> GetReportDataList(NskAppContext dbContext, P102090JoukenModel jouken, string kumiaitoCd)
        {
            // sql用定数定義
            var sql = new StringBuilder();

            sql.AppendLine("WITH AggData AS (");
            sql.AppendLine("    SELECT");
            sql.AppendLine("         m_10220.共済目的コード,");
            sql.AppendLine("         m_00010.共済目的名称,");
            sql.AppendLine("         COALESCE(m_00020.類区分, '') AS 類区分,");
            sql.AppendLine("         COALESCE(m_00020.類名称, '') AS 類名称,");
            sql.AppendLine("         m_10220.大地区コード,");
            sql.AppendLine("         m_10220.小地区コード,");
            sql.AppendLine("         STRING_AGG(");
            sql.AppendLine("             TO_CHAR(m_10240.危険段階地域区分::INTEGER, 'FM000'), ','");
            sql.AppendLine("             ORDER BY m_10240.引受方式, m_10240.特約区分, m_10240.補償割合コード DESC");
            sql.AppendLine("         ) AS 危険段階地域区分リスト");
            sql.AppendLine("    FROM");
            sql.AppendLine("         m_10220_地区別設定 m_10220");
            sql.AppendLine("         INNER JOIN m_10240_危険段階地域別設定 m_10240");
            sql.AppendLine("             ON m_10220.組合等コード = m_10240.組合等コード");
            sql.AppendLine("             AND m_10220.年産 = m_10240.年産");
            sql.AppendLine("             AND m_10220.共済目的コード = m_10240.共済目的コード");
            sql.AppendLine("             AND m_10220.大地区コード = m_10240.大地区コード");
            sql.AppendLine("             AND m_10220.小地区コード = m_10240.小地区コード");
            sql.AppendLine("         INNER JOIN m_00010_共済目的名称 m_00010");
            sql.AppendLine("             ON m_00010.共済目的コード = m_10240.共済目的コード");
            sql.AppendLine("         INNER JOIN m_00020_類名称 m_00020");
            sql.AppendLine("             ON m_00020.共済目的コード = m_10240.共済目的コード");
            sql.AppendLine("             AND m_00020.類区分 = m_10240.類区分");
            sql.AppendLine("    WHERE");
            sql.AppendLine("         m_10220.組合等コード   = @組合等コード");
            sql.AppendLine("     AND m_10220.年産          = @年産");
            sql.AppendLine("     AND m_10220.共済目的コード = @共済目的コード");
            sql.AppendLine("     AND m_00020.類区分        = @類区分");
            sql.AppendLine("     AND m_00020.加入区分      = '1'");
            sql.AppendLine("     AND (@大地区コード IS NULL OR @大地区コード = '' OR m_10220.大地区コード = @大地区コード)");
            sql.AppendLine("     AND (@小地区コード（開始） IS NULL OR @小地区コード（開始） = '' OR m_10220.小地区コード >= @小地区コード（開始）)");
            sql.AppendLine("     AND (@小地区コード（終了） IS NULL OR @小地区コード（終了） = '' OR m_10220.小地区コード <= @小地区コード（終了）)");
            sql.AppendLine("    GROUP BY");
            sql.AppendLine("         m_10220.共済目的コード,");
            sql.AppendLine("         m_00010.共済目的名称,");
            sql.AppendLine("         m_00020.類区分,");
            sql.AppendLine("         m_00020.類名称,");
            sql.AppendLine("         m_10220.大地区コード,");
            sql.AppendLine("         m_10220.小地区コード");
            sql.AppendLine(")");
            sql.AppendLine("SELECT");
            sql.AppendLine("    共済目的コード,");
            sql.AppendLine("    共済目的名称,");
            sql.AppendLine("    類区分,");
            sql.AppendLine("    類名称,");
            sql.AppendLine("    大地区コード,");
            sql.AppendLine("    小地区コード,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 1) AS ippitsu7wariVal,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 2) AS ippitsu6wariVal,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 3) AS ippitsu5wariVal,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 4) AS hanIppan8wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 5) AS hanIppan7wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 6) AS hanIppan6wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 7) AS hanHanToku8wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 8) AS hanHanToku7wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 9) AS hanHanToku6wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 10) AS zenIppan9wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 11) AS zenIppan8wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 12) AS zenIppan7wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 13) AS zenHanToku9wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 14) AS zenHanToku8wari,");
            sql.AppendLine("    SPLIT_PART(危険段階地域区分リスト, ',', 15) AS zenHanToku7wari");
            sql.AppendLine("FROM AggData");
            sql.AppendLine("ORDER BY 共済目的コード, 大地区コード, 小地区コード;");

            var parameters = new List<NpgsqlParameter>
            {
                new("@組合等コード", kumiaitoCd),
                new("@年産", int.Parse(jouken.nensan)),
                new("@共済目的コード", jouken.kyosaiMokutekiCd),
                new("@類区分", jouken.ruikbn),
                new("@大地区コード", jouken.daichiku),
                new("@小地区コード（開始）", jouken.shochikuStart),
                new("@小地区コード（終了）", jouken.shochikuEnd),
            };

            logger.Info("作成対象とする帳票データを取得する。");

            return dbContext.Database.SqlQueryRaw<P102090TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion
    }
}
