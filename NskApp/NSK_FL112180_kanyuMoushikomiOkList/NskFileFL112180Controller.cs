using NSK_FL112180_KikiDankaiDataToriKokuji.Common;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using NLog;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskReportMain.Common;
using NskReportLibrary.Core.Consts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using static CoreLibrary.Core.Utility.CsvUtil;

namespace NSK_FL112180_KikiDankaiDataToriKokuji
{

    public class NskFileFL112180Controller()
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "Pl112180_加入申込書大量大量データ受入OKリスト制御処理";

        #region Pl112180_加入申込書大量大量データ受入OKリスト制御処理
        /// <summary>
        /// Pl112180_加入申込書大量大量データ受入OKリスト
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="batchId">バッチID</param>/// 
        /// <returns>実行結果</returns>
        public ControllerResult ManageFile(string userId, string joukenId, string todofukenCd, string kumiaitoCd, string shishoCd, string filePath, long? batchId)
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

            // TODO: ファイル出力の戻り値 ControllerResultクラスは帳票と同じものか？（2025/3/10 現在ほかに見当たらない）
            var result = new ControllerResult();

            // 引数チェック
            result = CheckParams(result, userId, joukenId, todofukenCd, kumiaitoCd, shishoCd, filePath, batchId);
            if (Constants.RESULT_FAILED.Equals(result.Result))
            {
                return result;
            }
            try
            {

                // バッチのDB接続先取得処理
                DbConnectionInfo dbConnectionInfo =
                    DBUtil.GetDbConnectionInfo(ConfigUtil.Get(Constants.SYSTEM_KBN_COMMON), todofukenCd, kumiaitoCd, shishoCd);

                using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
                {
                    // バッチ条件情報取得
                    List<T01050バッチ条件> joukenList = GetBatchJoukenList(db, joukenId, Constants.JOUKEN_UKEIRE_RIREKI_ID, Constants.JOUKEN_FILE_NAME, Constants.JOUKEN_MOJI_CD);

                    result = CheckJouken(result, joukenList);
                    if (Constants.RESULT_FAILED.Equals(result.Result))
                    {
                        return result;
                    }

                    string fileName       = joukenList.FirstOrDefault(c => c.条件名称 == Constants.JOUKEN_FILE_NAME)!.条件値 + CoreConst.FILE_EXTENSION_CSV;
                    string ukeireRirekiId = joukenList.FirstOrDefault(c => c.条件名称 == Constants.JOUKEN_UKEIRE_RIREKI_ID)!.条件値;
                    string sysDateStr = DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss");

                    // CSV一時出力フォルダ\[引数：バッチID]\yyyyMMddHHmmss\
                    var tempFolder = Path.Combine(ConfigUtil.Get(Constants.FILE_TEMP_FOLDER_PATH), batchId?.ToString() ?? "", sysDateStr);

                    // 検索対象テーブル名
                    string targetEntity = typeof(T19070大量データ受入加入申込書ok)
                            .GetCustomAttribute<TableAttribute>()?.Name ?? typeof(T19070大量データ受入加入申込書ok).Name;

                    // 検索条件（SQL）
                    string strSqlConf = Constants.JOUKEN_UKEIRE_RIREKI_ID + ukeireRirekiId;

                    // 検索条件（パラメータ）
                    List<SqlParam> sqlParams =
                        [
                            new SqlParam() { name = "@" + Constants.JOUKEN_UKEIRE_RIREKI_ID, value = ukeireRirekiId, type = (int)SqlParamType.String }
                        ];

                    // 並び順
                    string sortColumnName = typeof(T19070大量データ受入加入申込書ok)
                        .GetProperty(nameof(T19070大量データ受入加入申込書ok.行番号))
                        ?.GetCustomAttribute<ColumnAttribute>()?.Name
                        ?? nameof(T19070大量データ受入加入申込書ok.行番号);

                    // エラーメッセージ
                    var csvMessage = string.Empty;

                    // CSVファイルを出力
                    CsvUtil.CsvFileOutput(tempFolder,
                                            targetEntity,
                                            strSqlConf,
                                            sqlParams,
                                            sortColumnName,
                                            joukenList.FirstOrDefault(c => c.条件名称 == Constants.JOUKEN_MOJI_CD)!.条件値,
                                            fileName,
                                            true,
                                            Constants.PARAM_SEPARATOR,
                                            true,
                                            db,
                                            ref csvMessage);

                    if (!string.IsNullOrEmpty(csvMessage))
                    {
                        logger.Error(csvMessage);
                        // CSV出力に失敗した場合
                        return result.ControllerResultError(string.Empty, "ME01645", "ファイル出力");
                    }

                    // CSVファイルの出力パス　[FILE_TEMP_FOLDER_PATH]\バッチID\yyyyMMddHHmmss\ファイル名
                    string outPutCsvPath = System.IO.Path.Combine(tempFolder, fileName);
                    string outPutZipPath = System.IO.Path.Combine(Constants.CSV_OUTPUT_FOLDER, batchId?.ToString() ?? "", sysDateStr);

                    // Zip暗号化を行う
                    var CreatedZipFilePath = ZipUtil.CreateZip(outPutCsvPath);

                    // Zipファイルを　[CSV_OUTPUT_FOLDER]]\バッチID\yyyyMMddHHmmss　に移動する。
                    FolderUtil.MoveFile(CreatedZipFilePath, outPutZipPath, userId, batchId!.Value);

                    result.Result = ReportConst.RESULT_SUCCESS;

                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.Error("FL112180_Error: " + ex);
                return result.ControllerResultError(string.Empty, "ME10033");
            }
            finally
            {
                // 一時保存フォルダの削除
                string deleteTmpFolderPath = Path.Combine(ConfigUtil.Get(Constants.FILE_TEMP_FOLDER_PATH), batchId?.ToString() ?? "");
                if (Directory.Exists(deleteTmpFolderPath))
                {
                    // フォルダの削除
                    FolderUtil.DeleteTempFolder(deleteTmpFolderPath);
                }

                // ログ出力(終了)
                logger.Info(string.Format(
                    ReportConst.METHOD_END_LOG,
                    ReportConst.CLASS_NM_CONTROLLER,
                    joukenId,
                    LOGGER_INFO_STR));
            }
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
        /// <param name="batchId">バッチID</param>
        /// <returns>実行結果</returns>
        private ControllerResult CheckParams(ControllerResult result, string userId, string joukenId, string todofukenCd,
                                             string kumiaitoCd, string shishoCd, string filePath, long? batchId)
        {
            // ■１．引数チェックする
            // ユーザIDがない場合、エラーメッセージを返す
            if (string.IsNullOrEmpty(userId))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_USER_ID);
            }

            // 条件IDがない、または36桁ではない場合、エラーメッセージを返す
            if (String.IsNullOrEmpty(joukenId) && joukenId.Length != 36)
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_JOUKEN_ID);
            }

            // 都道府県コードがない場合、エラーメッセージを返す。
            if (string.IsNullOrEmpty(todofukenCd))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_TODOFUKEN_CD);
            }

            // 組合等コードがない場合、エラーメッセージを返す。
            if (string.IsNullOrEmpty(kumiaitoCd))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_KUMIAITO_CD);
            }

            // 支所コードがない場合、エラーメッセージを返す。
            if (string.IsNullOrEmpty(shishoCd))
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_SHISHO_CD);
            }

            // バッチIDがない場合、エラーメッセージを返す。
            if (!batchId.HasValue)
            {
                return result.ControllerResultError(string.Empty, "ME01054", ReportConst.PARAM_NAME_BATCH_ID);
            }

            return result;
        }
        #endregion

        #region 条件取得
        /// <summary>
        /// バッチ条件テーブルからデータ取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="joukenId">条件id</param>
        /// <param name="ukeirerirekiId">受入履歴id</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="mojiCd">文字コード</param>
        /// <returns>バッチ条件リスト</returns>
        private List<T01050バッチ条件> GetBatchJoukenList(
                NskAppContext db,
                string joukenId,
                string ukeirerirekiIdStr,
                string fileNameStr,
                string mojiCdStr)
        {
            logger.Info("バッチ条件テーブルから、指定されたバッチ条件idおよび条件名称に一致するデータを取得します。");

            List<T01050バッチ条件> results = db.Set<T01050バッチ条件>()
                            .AsNoTracking()
                            .Where(b => b.バッチ条件id == joukenId &&
                                        (b.条件名称 == ukeirerirekiIdStr ||
                                         b.条件名称 == fileNameStr ||
                                         b.条件名称 == mojiCdStr))
                            .ToList();
            return results;
        }
        #endregion

        #region 条件チェック
        /// <summary>
        /// 出力条件チェック
        /// </summary>
        /// <param name="result">実行結果</param>
        /// <param name="joukenList">バッチ条件リスト</param>
        /// <returns>実行結果</returns>
        private ControllerResult CheckJouken(ControllerResult result, List<T01050バッチ条件> joukenList)
        {
            // 受入履歴IDがない場合、エラーメッセージを返す。
            if (joukenList.FirstOrDefault(c => c.条件名称 == Constants.JOUKEN_UKEIRE_RIREKI_ID) == null)
            {
                return result.ControllerResultError(string.Empty, "ME01644", Constants.JOUKEN_UKEIRE_RIREKI_ID);
            }

            // ファイル名がない場合、エラーメッセージを返す。
            if (joukenList.FirstOrDefault(c => c.条件名称 == Constants.JOUKEN_FILE_NAME) == null)
            {
                return result.ControllerResultError(string.Empty, "ME01644", Constants.JOUKEN_FILE_NAME);
            }

            // 文字コードがない場合、エラーメッセージを返す。
            if (joukenList.FirstOrDefault(c => c.条件名称 == Constants.JOUKEN_MOJI_CD) == null)
            {
                return result.ControllerResultError(string.Empty, "ME01644", Constants.JOUKEN_MOJI_CD);
            }
            return result;
        }
        #endregion
    }
}