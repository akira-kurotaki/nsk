using NskAppModelLibrary.Context;
using NskReportMain.Common;
using NskReportMain.Models.P103040;
using NskReportMain.ReportCreators.P103040;
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
    public class NskReportP103040Controller : ReportController
    {

        #region メンバー定数
        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "P103040_基準統計単収一覧制御処理";
        #endregion

        public NskReportP103040Controller(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        #region P103040_基準統計単収一覧制御処理
        /// <summary>
        /// P103040_基準統計単収一覧制御処理
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="filePath">帳票パス</param>
        /// <param name="batchId">バッチID</param>/// 
        /// <returns>実行結果</returns>
        public ControllerResult ManageReports(string userId, string joukenId, string todofukenCd, string kumiaitoCd, string shishoCd, string filePath, long? batchId)
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

                var model = new P103040Model();

                // 帳票出力条件を取得する。
                // 条件IDをキーに帳票条件テーブルより帳票出力条件を取得する。
                model.P103040BatchJouken = GetBatchJouken(getJigyoDb<NskAppContext>(), joukenId);

                // 条件チェック
                // 年産または共済目的コードが取得できない場合
                // または、共済目的 <> 陸稲 の時、類区分が取得できない場合はエラー
                if (model.P103040BatchJouken.年産.IsNullOrEmpty() || model.P103040BatchJouken.共済目的コード.IsNullOrEmpty() ||
                    ((model.P103040BatchJouken.共済目的コード != "20") && model.P103040BatchJouken.類区分.IsNullOrEmpty()))
                {
                    return result.ControllerResultError(string.Empty, "ME90010");
                }

                // 帳票オブジェクト
                var report = new BaseSectionReport();

                // 帳票ヘッダーを取得する。
                model.P103040HeaderModel = GetReportHeaderData(getJigyoDb<NskAppContext>(), model.P103040BatchJouken);
                // 帳票データを取得する。
                model.P103040TableRecords = GetReportDataList(getJigyoDb<NskAppContext>(), model.P103040BatchJouken, kumiaitoCd);

                // 帳票出力処理を呼び出す。
                result = CreateP103040(model, joukenId, ref report);
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

                // ファイル名は「11_基準統計単収一覧.PDF」とする。
                pdfExport.Export(report.Document, Path.Combine(printTempFolder, userId + "_" + model.dateTimeToString.ToString("yyyyMMddHHmmss") + "_" + "基準統計単収一覧" + ReportConst.REPORT_EXTENSION));

                // PDF出力用一時フォルダをzip化（暗号化）する。
                var zipFilePath = ZipUtil.CreateZip(printTempFolder);

                // Zipファイルを引数：帳票パスに移動する
                FolderUtil.MoveFile(zipFilePath, filePath, userId, batchId.Value);

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

        #region 「P103040_基準統計単収一覧」を作成するメソッド
        /// <summary>
        /// 「P103040_基準統計単収一覧」を作成するメソッド
        /// </summary>
        /// <param name="model">帳票モデルリスト</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="report">帳票オブジェクト</param>
        /// <returns>実行結果</returns>
        private ControllerResult CreateP103040(
            P103040Model model,
            string joukenId,
            ref BaseSectionReport report)
        {
            // 実行結果
            var result = new ControllerResult();
            P103040Creator p103040Creator = new P103040Creator();
            CreatorResult creatorResult = p103040Creator.CreateReport(joukenId, model);
            // 失敗した場合
            if (creatorResult.Result == ReportConst.RESULT_FAILED)
            {
                return result.ControllerResultError(creatorResult.ErrorMessage, creatorResult.ErrorMessageId);
            }

            // ページ番号を描画する
            creatorResult.SectionReport = ReportPagerUtil.DrawReportPageNumber(creatorResult.SectionReport, ReportConst.REPORT_BOTTOM_MARGIN_STANDARD);
            // 出力レポートにP103040_基準統計単収一覧を入れる。
            report.Document.Pages.AddRange(creatorResult.SectionReport.Document.Pages);

            return result;
        }
        #endregion

        #region 引数チェック
        /// <summary>
        /// P103040_基準統計単収一覧引数チェック
        /// </summary>
        /// <param name="result">実行結果</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="filePath">帳票パス</param>
        /// <param name="filePath">バッチID</param>
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
        private static P103040JoukenModel GetBatchJouken(NskAppContext db, string joukenId)
        {
            var validJoukenNames = new[]
            {
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_NENSAN,
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,  // 条件定数「共済目的コード」を要追加
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_RUIKUBUN
            };

            // T01050バッチ条件のデータを取得し、P103040JoukenModelに変換して返す
            var batchJouken = db.Set<T01050バッチ条件>()
                                .AsNoTracking()
                                .Where(b => b.バッチ条件id == joukenId && validJoukenNames.Contains(b.条件名称))
                                .Select(b => new P103040JoukenModel
                                {
                                    年産 = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_NENSAN ? b.条件値 : null,
                                    共済目的コード = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD ? b.条件値 : null,
                                    類区分 = b.条件名称 == NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_RUIKUBUN ? b.条件値 : null
                                })
                                .FirstOrDefault();

            return batchJouken;
        }
        #endregion

        #region 帳票ヘッダー取得
        /// <summary>
        /// 帳票ヘッダー情報を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jouken">データ取得条件</param>
        /// <returns>ヘッダー情報</returns>
        private P103040HeaderModel GetReportHeaderData(NskAppContext dbContext, P103040JoukenModel jouken)
        {
            var headerInfo = new P103040HeaderModel();
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("  m_00010.共済目的名称, ");
            sql.AppendLine("  CASE WHEN @共済目的コード = '20' THEN '' ELSE @類区分 END AS 類区分, ");
            sql.AppendLine("  CASE WHEN @共済目的コード = '20' THEN '' ELSE m_00020.類短縮名称 END AS 類名称 ");
            sql.AppendLine("FROM m_00010_共済目的名称 m_00010 ");
            sql.AppendLine("INNER JOIN m_00020_類名称 m_00020 ");
            sql.AppendLine("  ON m_00020.共済目的コード = m_00010.共済目的コード ");
            sql.AppendLine("WHERE m_00010.共済目的コード = @共済目的コード ");
            // 共済目的が '20' 以外の場合、類区分も条件に含める
            sql.AppendLine("  AND (@共済目的コード != '20' AND m_00020.類区分 = @類区分 OR @共済目的コード = '20') ");
            sql.AppendLine(";");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@共済目的コード", jouken.共済目的コード),
                new NpgsqlParameter("@類区分", jouken.類区分)
            };

            logger.Info("作成対象とする帳票ヘッダー情報を取得する。");

            try
            {
                headerInfo = dbContext.Database.SqlQueryRaw<P103040HeaderModel>(sql.ToString(), parameters.ToArray()).FirstOrDefault();
                if (headerInfo != null)
                {
                    headerInfo.年産 = DateUtil.GetReportJapaneseYear(int.Parse(jouken.年産));
                }
            }
            catch (Exception ex)
            {
                logger.Error("ヘッダー情報取得エラー", ex);
                headerInfo.年産 = "";
            }

            return headerInfo;
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
        private List<P103040TableRecord> GetReportDataList(NskAppContext dbContext, P103040JoukenModel jouken, string kumiaitoCd)
        {
            // sql用定数定義
            var sql = new StringBuilder();

            sql.Append("SELECT ");
            sql.Append("   m_10070.統計単位地域コード AS 統計単収地域コード ");
            sql.Append(" , m_00170.統計単位地域名称 AS 統計単収地域名 ");
            sql.Append(" , m_10070.統計単収 ");
            sql.Append("FROM ");
            sql.Append("   m_10070_統計単収引受 m_10070 ");
            sql.Append("   INNER JOIN ");
            sql.Append("      m_00170_統計単位地域 m_00170 ");
            sql.Append("   ON  m_10070.組合等コード      = m_00170.組合等コード ");
            sql.Append("   AND m_10070.年産              = m_00170.年産 ");
            sql.Append("   AND m_10070.共済目的コード     = m_00170.共済目的コード ");
            sql.Append("   AND m_10070.統計単位地域コード = m_00170.統計単位地域コード ");
            sql.Append("WHERE ");
            sql.Append("       m_10070.組合等コード   = @組合等コード ");
            sql.Append("   AND m_10070.年産          = @年産 ");
            sql.Append("   AND m_10070.共済目的コード = @共済目的コード ");
            // 共済目的 <> 陸稲　の場合にのみ、類区分を抽出条件に指定する。
            sql.Append("   AND (@共済目的コード != '20' AND m_10070.類区分 = @類区分 OR @共済目的コード = '20') ");
            sql.Append(";");

            var parameters = new List<NpgsqlParameter>
            {
                new("@組合等コード", kumiaitoCd),
                new("@年産", int.Parse(jouken.年産)),
                new("@共済目的コード", jouken.共済目的コード),
                new("@類区分", jouken.類区分),
            };

            logger.Info("作成対象とする帳票データを取得する。");

            return dbContext.Database.SqlQueryRaw<P103040TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion
    }
}
