using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NSK_P103040.Models.P103040;
using NSK_P103040.ReportCreators.P103040;
using NskAppModelLibrary.Context;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using System.Globalization;
using System.Text;
using Core = CoreLibrary.Core.Consts;
using NskCommon = NskCommonLibrary.Core.Consts;

namespace NSK_P103040.Controllers
{
    /// <summary>
    /// 基準統計単収一覧帳票制御
    /// </summary>
    public class NSK_103040PController : ReportController
    {
        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "P103040_帳票出力（基準統計単収一覧）";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbInfo">DB接続情報</param>
        public NSK_103040PController(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        /// <summary>
        /// 基準統計単収一覧制御処理
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="args">
        /// 引数配列要素1：ユーザーID
        /// 引数配列要素2：バッチID
        /// 引数配列要素3：都道府県コード
        /// 引数配列要素4：組合等コード
        /// 引数配列要素5：支所コード
        /// 引数配列要素6：バッチ条件のキー情報
        /// </param>
        public ControllerResult ManageReports(NskAppContext dbContext, string[] args)
        {
            ControllerResult controllerResult = new();

            // 引数
            // ユーザーID
            string userId = string.Empty;
            // バッチID
            string bid = string.Empty;
            // 都道府県コード
            string todofukenCd = string.Empty;
            // 組合等コード
            string kumiaitoCd = string.Empty;
            // 支所コード
            string shishoCd = string.Empty;
            // バッチ条件のキー情報
            string jid = string.Empty;
            // 一時保存フォルダパス
            string fileTempFolderPath = string.Empty;

            try
            {
                // args から 各変数へ展開する
                // ユーザーID
                if (args.Length > 0)
                {
                    userId = args[0];
                }
                // バッチID
                if (args.Length > 1)
                {
                    bid = args[1];
                }
                // 都道府県コード
                if (args.Length > 2)
                {
                    todofukenCd = args[2];
                }
                // 組合等コード
                if (args.Length > 3)
                {
                    kumiaitoCd = args[3];
                }
                // 支所コード
                if (args.Length > 4)
                {
                    shishoCd = args[4];
                }
                // バッチ条件のキー情報
                if (args.Length > 5)
                {
                    jid = args[5];
                }

                // ログ出力(開始)
                logger.Info(string.Format(
                    ReportConst.METHOD_BEGIN_LOG,
                    ReportConst.CLASS_NM_CONTROLLER,
                    jid,
                    LOGGER_INFO_STR,
                    string.Join(ReportConst.PARAM_SEPARATOR, new string[]{
                        ReportConst.PARAM_NAME_USER_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + userId,
                        ReportConst.PARAM_NAME_JOUKEN_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + jid,
                        ReportConst.PARAM_NAME_TODOFUKEN_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + todofukenCd,
                        ReportConst.PARAM_NAME_KUMIAITO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + kumiaitoCd,
                        ReportConst.PARAM_NAME_SHISHO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + shishoCd})));

                // 引数のチェック
                IsRequired(userId, bid, todofukenCd, kumiaitoCd, shishoCd, jid, out int nBid);

                // バッチ条件を取得する。
                BatchJouken batchJouken = new();
                // バッチ条件情報の取得
                batchJouken.GetTyouhyouSakuseiJouken(dbContext, jid);

                // 必須入力チェック
                batchJouken.IsRequired();

                // 帳票ヘッダーを取得する。
                var header = GetReportHeaderData(dbContext, batchJouken);

                // 帳票データを取得する。
                var detail = GetReportDataList(dbContext, batchJouken, kumiaitoCd);

                // ヘッダーまたは明細情報が0件の場合は、エラーを返却
                if ((header == null) || (detail.IsNullOrEmpty()))
                {
                    return controllerResult.ControllerResultError(string.Empty, "ME10076", "０");
                }

                // 帳票PDF一時出力フォルダ作成
                // 帳票PDF一時出力フォルダを作成する。
                // [FileTempFolderPath]/[変数：バッチID]_yyyyMMddHHmmss/
                fileTempFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH),
                    bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                Directory.CreateDirectory(fileTempFolderPath);

                // 帳票出力フォルダを作成する。
                // [ReportOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss/
                string reportOutputFolder = Path.Combine(ConfigUtil.Get(Core.CoreConst.REPORT_OUTPUT_FOLDER),
                    bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                Directory.CreateDirectory(reportOutputFolder);

                // 一覧帳票作成
                // 帳票「基準統計単収一覧」を作成する。
                NSK_103040PModel tyouhyouDatas = new()
                {
                    headerModel = header,
                    tableRecords = detail
                };
                CreatorResult creatorResult = NSK_103040PCreator.CreateReport(tyouhyouDatas, batchJouken);


                // 作成された帳票をPDFファイルとして、一時保存フォルダに出力する。
                string tempFileName = batchJouken.joukenReportName + ReportConst.REPORT_EXTENSION;
                string tempPath = Path.Combine(fileTempFolderPath, tempFileName);
                pdfExport.Export(creatorResult.SectionReport.Document, tempPath);

                // 終了処理
                // 帳票作成処理が成功した場合
                if (creatorResult.Result == ReportConst.RESULT_SUCCESS)
                {
                    // 一時保存フォルダ内の帳票をZipUtil.CreateZipを使用して分割（所定のサイズ以上の場合）、およびZip化（暗号化）
                    Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(fileTempFolderPath);
                    // ZipファイルをFolderUtil.MoveFileを使用して「reportOutputFolder」に移動する。
                    // ※FolderUtil.MoveFile内で、ファイルパス/ファイル名がシステム共通スキーマのバッチダウンロードファイルテーブルに登録される。
                    NskCommonLibrary.Core.Utility.FolderUtil.MoveFile(zipFilePath, reportOutputFolder, userId, nBid);

                    // 処理結果：0（成功）を返す。
                    controllerResult.Result = ReportConst.RESULT_SUCCESS;
                }
                else
                {
                    controllerResult.Result = ReportConst.RESULT_FAILED;
                    // [変数：エラーメッセージ] に以下のメッセージを設定
                    // （"ME01645" 引数{0}：基準統計単収一覧の出力)
                    controllerResult.ErrorMessageId = "ME01645";
                    controllerResult.ErrorMessage = MessageUtil.Get("ME01645", "基準統計単収一覧の出力");
                }
            }
            catch (Exception ex)
            {
                // 上記までの処理で例外が発生した場合
                // 例外の内容をログに出力する。
                logger.Error(ex);

                // 処理結果：1（失敗）を返す
                controllerResult.Result = ReportConst.RESULT_FAILED;

                // 設定されたエラーメッセージを返す。
                if (ex is AppException aEx)
                {
                    controllerResult.ErrorMessageId = aEx.ErrorCode;
                }
                controllerResult.ErrorMessage = ex.Message;
            }
            finally
            {
                // 一時保存フォルダを削除する。
                if (Directory.Exists(fileTempFolderPath))
                {
                    Directory.Delete(fileTempFolderPath, true);
                }
            }

            // ログ出力(終了)
            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CONTROLLER,
                jid,
                LOGGER_INFO_STR));

            return controllerResult;
        }

        /// <summary>
        /// 引数のチェック
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="bid">バッチID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="jid">バッチ条件のキー情報</param>
        /// <param name="nBid">数値型バッチID</param>
        /// <exception cref="AppException"></exception>
        private static void IsRequired(string userId, string bid, string todofukenCd, string kumiaitoCd, string shishoCd, string jid, out int nBid)
        {
            // １．１．ユーザIDが未入力の場合、エラーとし、エラーメッセージを返す。
            // [変数：ユーザID] が未入力の場合
            if (string.IsNullOrEmpty(userId))
            {
                // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                // （"ME01054" 引数{0} ：ユーザID）
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "ユーザID"));
            }

            // [変数：バッチID] が未入力の場合
            if (string.IsNullOrEmpty(bid))
            {
                // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                // （"ME01054" 引数{0} ：パラメータの取得）
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチID"));
            }

            // [変数：バッチID]が数値変換不可の場合
            // 数値化したバッチID
            if (!int.TryParse(bid, out nBid))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、処理を終了する。
                // （"ME90012"　引数{0} ：バッチID)
                throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
            }

            // [変数：都道府県コード]が未入力の場合
            if (string.IsNullOrEmpty(todofukenCd))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、処理を終了する。
                // （"ME01054"　引数{0} ：都道府県コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "都道府県コード"));
            }

            // [変数：組合等コード] が未入力の場合
            if (string.IsNullOrEmpty(kumiaitoCd))
            {
                //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                //（"ME01054" 引数{0} ：組合等コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
            }

            // [変数：支所コード] が未入力の場合
            if (string.IsNullOrEmpty(shishoCd))
            {
                //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                //（"ME01054" 引数{0} ：支所コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "支所コード"));
            }

            // [変数：バッチ条件のキー情報] が未入力の場合
            if (string.IsNullOrEmpty(jid))
            {
                //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
            }
        }

        /// <summary>
        /// ヘッダー情報を取得する。
        /// </summary>
        /// <param name="nskContext">DBコンテキスト</param>
        /// <param name="jouken">バッチ条件</param>
        /// <returns>帳票条件リスト</returns>
        protected HeaderModel GetReportHeaderData(NskAppContext nskContext, BatchJouken jouken)
        {
            logger.Info("ヘッダー情報を取得する。");
            var headerInfo = new HeaderModel();
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

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@共済目的コード", jouken.JoukenKyosaiMokutekiCd),
                new NpgsqlParameter("@類区分", jouken.joukenRuikbn)
            };

            logger.Info("作成対象とする帳票ヘッダー情報を取得する。");

            try
            {
                headerInfo = nskContext.Database.SqlQueryRaw<HeaderModel>(sql.ToString(), parameters.ToArray()).FirstOrDefault();
                
                if (headerInfo == null)
                {
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "ヘッダー情報"));
                }
            }
            catch (Exception ex)
            {
                logger.Error("ヘッダー情報取得エラー");
                logger.Error(ex.Message);
                throw;
            }

            return headerInfo;
        }

        /// <summary>
        /// 帳票データを取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jouken">データ取得条件</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>帳票データ</returns>
        private List<TableRecord> GetReportDataList(NskAppContext dbContext, BatchJouken jouken, string kumiaitoCd)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var result = new List<TableRecord>();
            sql.Append("SELECT ");
            sql.Append("   m_10070.統計単位地域コード AS 統計単収地域コード ");
            sql.Append(" , m_00170.統計単位地域名称 AS 統計単収地域名 ");
            sql.Append(" , m_10070.統計単収 ");
            sql.Append("FROM ");
            sql.Append("   m_10070_統計単収引受 m_10070 ");
            sql.Append("   INNER JOIN m_00170_統計単位地域 m_00170 ");
            sql.Append("     ON  m_10070.組合等コード       = m_00170.組合等コード ");
            sql.Append("     AND m_10070.年産              = m_00170.年産 ");
            sql.Append("     AND m_10070.共済目的コード     = m_00170.共済目的コード ");
            sql.Append("     AND m_10070.統計単位地域コード = m_00170.統計単位地域コード ");
            sql.Append("WHERE ");
            sql.Append("       m_10070.組合等コード   = @組合等コード ");
            sql.Append("   AND m_10070.年産          = @年産 ");
            sql.Append("   AND m_10070.共済目的コード = @共済目的コード ");
            // 共済目的 <> 陸稲　の場合にのみ、類区分を抽出条件に指定する。
            sql.Append("   AND (@共済目的コード != '20' AND m_10070.類区分 = @類区分 OR @共済目的コード = '20') ");

            var parameters = new List<NpgsqlParameter>
            {
                new("@組合等コード", kumiaitoCd),
                new("@年産", int.Parse(jouken.joukenNensan)),
                new("@共済目的コード", jouken.JoukenKyosaiMokutekiCd),
                new("@類区分", jouken.joukenRuikbn),
            };

            logger.Info("作成対象とする帳票データを取得する。");

            try
            {
                result = dbContext.Database.SqlQueryRaw<TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
                if (result == null)
                {
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "基準統計単収一覧データ情報"));
                }
            }
            catch (Exception ex)
            {
                logger.Error("基準統計単収一覧データ情報取得エラー");
                logger.Error(ex.Message);
                throw;
            }


            return result;
        }

        /// <summary>
        /// 日付を和暦に変換する
        /// </summary>
        /// <param name="date">Date型日付</param>
        /// <returns>和暦文字列</returns>
        private string ConvertDateTime(DateTime date)
        {
            var japaneseCalendar = new JapaneseCalendar();
            var ci = new CultureInfo("ja-JP");
            var dtfi = (DateTimeFormatInfo)ci.DateTimeFormat.Clone();

            dtfi.Calendar = japaneseCalendar;
            // 令和yy年mm月dd日
            string wareki = date.ToString("ggyy年MM月dd日", dtfi);
            return wareki; 
        }
    }
}
