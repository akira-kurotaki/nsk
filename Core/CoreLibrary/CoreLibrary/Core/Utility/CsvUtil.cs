using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using Npgsql;
using NpgsqlTypes;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// Csvデータユーティリティ
    /// </summary>
    /// <remarks>
    /// 作成日：2018/03/01
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public static class CsvUtil
    {
        /// <summary>
        /// 行データのカンマ区切りした結果を取得する。
        /// </summary>
        /// <param name="line">行データ</param>
        /// <returns>カンマ区切りした行データ</returns>
        public static string[] GetItems(string line)
        {
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(line)))
            {
                var parser = new TextFieldParser(stream);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                return parser.ReadFields();
            }
        }

        /// <summary>
        /// 文字列の配列から行データを取得する。
        /// </summary>
        /// <param name="items">行データ用配列</param>
        /// <returns>行データ</returns>
        public static string GetLine(string[] items)
        {
            var retLine = "";

            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(retLine))
                {
                    // 列区切り文字　→　「,」
                    retLine += ",";
                }
                if (string.IsNullOrEmpty(item))
                {
                    // データの囲み　→　空欄の場合は""とする。
                    retLine += "\"\"";
                }
                else
                {
                    // データ中にダブルクォーテーションがある場合は、直前にダブルクォーテーションをひとつ付加する。
                    // データの囲み　→　データはすべて「""」で囲む。
                    retLine += string.Format("\"{0}\"", item.Replace("\"", "\"\""));
                }
            }
            // 行区切り文字　→　改行コードはCRLFとする。単独のCR、LFではない。
            retLine += "\r\n";

            return retLine;
        }

        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 処理結果：01（即時実行完了）
        /// </summary>
        public const string RET_SUCCESS_IMMEDIATE_EXECUTION = "01";

        /// <summary>
        /// 処理結果：02（バッチ予約完了）
        /// </summary>
        public const string RET_SUCCESS_BATCH_RESERVE = "02";

        /// <summary>
        /// 処理結果：11（バッチ予約警告）
        /// </summary>
        public const string RET_WARN_BATCH_RESERVE = "11";

        /// <summary>
        /// 処理結果：12（件数０件エラー）
        /// </summary>
        public const string RET_ERROR_ZERO_RECORDS = "12";

        /// <summary>
        /// 処理結果：21（引数エラー）
        /// </summary>
        public const string RET_ERROR_ARGUMENT = "21";

        /// <summary>
        /// 処理結果：22（即時実行エラー）
        /// </summary>
        public const string RET_ERROR_IMMEDIATE_EXECUTION = "22";

        /// <summary>
        /// 処理結果：23（バッチ予約エラー）
        /// </summary>
        public const string RET_ERROR_BATCH_RESERVE = "23";

        /// <summary>
        /// バッチ分類：CSV出力
        /// </summary>
        public const string BATCH_BUNRUI_CSV = "02";

        /// <summary>
        /// バッチ名（CSV出力）
        /// </summary>
        public const string BATCH_NM_CSV = "CSV出力";

        /// <summary>
        /// バッチ条件（モードSQL）
        /// </summary>
        public const string BATCH_JOKEN_MODE_SQL = "SQL";

        /// <summary>
        /// バッチ条件（モードTBL）
        /// </summary>
        public const string BATCH_JOKEN_MODE_TBL = "TBL";

        /// <summary>
        /// バッチ条件（フォーマット）
        /// </summary>
        public const string BATCH_JOKEN_FORMAT = "mode={0},filenm={1}";

        /// <summary>
        /// バッチ種類（巡回バッチ）
        /// </summary>
        public const string BATCH_TYPE_PATROL = "1";

        /// <summary>
        /// ロック対象外（対象外）
        /// </summary>
        public const string LOCK_TARGET_FLG_NOT = "0";

        /// <summary>
        /// 処理成否（失敗）
        /// </summary>
        public const int RET_FAIL = 0;

        /// <summary>
        /// 処理成否（成功）
        /// </summary>
        public const int RET_SUCCESS = 1;

        /// <summary>
        /// ファイル書き出し件数
        /// </summary>
        public const int FLASH_COUNT = 1000;

        /// <summary>
        /// データ型
        /// </summary>
        public enum SqlParamType
        {
            [Description("整数")]
            Int = 1,
            [Description("小数含む数値")]
            Decimal = 2,
            [Description("文字列")]
            String = 3,
            [Description("日時")]
            DateTime = 4,
            [Description("リスト（NpgsqlDbType.Array | NpgsqlDbType.Varchar）")]
            ArrayVarchar = 5,
        }

        /// <summary>
        /// バッチ即時強制指定
        /// </summary>
        public static class BatchNowSet
        {
            [Description("バッチ・即時指定なし")]
            public const string NotSpecified = "0";
            [Description("バッチ")]
            public const string Batch = "1";
            [Description("即時")]
            public const string Now = "2";
        }

        /// <summary>
        /// 複数実行禁止フラグ
        /// </summary>
        public static class MultiRunNgFlg
        {
            [Description("複数実行可能")]
            public const string Possible = "0";
            [Description("複数実行禁止")]
            public const string Impossible = "1";
        }

        /// <summary>
        /// CSV出力共通部品
        /// </summary>
        /// <param name="strUserId">ユーザID</param>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="strKumiaitoCd">組合等コード</param>
        /// <param name="strShishoCd">支所コード</param>
        /// <param name="strSyoriNm">処理名</param>
        /// <param name="strBatchNowSet">バッチ即時強制指定</param>
        /// <param name="boolBatchAlert">バッチ処理前警告</param>
        /// <param name="strZipFileNm">zipファイル名</param>
        /// <param name="strBatchJokenDisp">バッチ条件（表示用）</param>
        /// <param name="listCsvOutputJoken">CSV出力条件クラスList</param>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <param name="csvzipfile">出力されたzipファイル（出力パラメータ）</param>
        /// <returns>処理成否</returns>
        public static string CsvOutput(string strUserId,
                                        string strSystemKbn,
                                        string strTodofukenCd,
                                        string strKumiaitoCd,
                                        string strShishoCd,
                                        string strSyoriNm,
                                        string strBatchNowSet,
                                        bool boolBatchAlert,
                                        string strZipFileNm,
                                        string strBatchJokenDisp,
                                        List<CsvOutputJoken> listCsvOutputJoken,
                                        DbContext db,
                                        ref string message,
                                        ref byte[] csvzipfile)
        {
            var param = new CsvOutputParam
            {
                UserId = strUserId,
                SystemKbn = strSystemKbn,
                TodofukenCd = strTodofukenCd,
                KumiaitoCd = strKumiaitoCd,
                ShishoCd = strShishoCd,
                SyoriNm = strSyoriNm,
                BatchNowSet = strBatchNowSet,
                BatchAlert = boolBatchAlert,
                ZipFileNm = strZipFileNm,
                BatchJokenDisp = strBatchJokenDisp,
                CsvOutputJokenList = listCsvOutputJoken.IsNullOrEmpty() ? null :
                                        listCsvOutputJoken.Select(x => new CsvOutputJokenInternal(x)).ToList()
            };

            return CsvOutputCommon(param, db, false, ref message, ref csvzipfile);

        }

        /// <summary>
        /// CSVファイル取得共通部品
        /// </summary>
        /// <param name="strOutputPath">出力先</param>
        /// <param name="strSerchTarget">検索対象</param>
        /// <param name="strSqlConf">検索条件（SQL）</param>
        /// <param name="listSqlParam">検索条件（パラメータ）</param>
        /// <param name="strSqlOrder">並び順</param>
        /// <param name="strCharacterCd">文字コード</param>
        /// <param name="strCsvNm">CSVファイル名</param>
        /// <param name="boolHeaderOnOff">ヘッダ有無</param>
        /// <param name="strSeparatorFont">セパレーター</param>
        /// <param name="boolBomOnOff">BOMコード有無</param>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>処理成否</returns>
        public static int CsvFileOutput(string strOutputPath,
                                        string strSerchTarget,
                                        string strSqlConf,
                                        List<SqlParam> listSqlParam,
                                        string strSqlOrder,
                                        string strCharacterCd,
                                        string strCsvNm,
                                        bool boolHeaderOnOff,
                                        string strSeparatorFont,
                                        bool boolBomOnOff,
                                        DbContext db,
                                        ref string message)
        {
            try
            {
                using var connection = db.Database.GetDbConnection() as NpgsqlConnection;
                connection.Open();

                return CsvFileOutputComon(strOutputPath,
                                            strSerchTarget,
                                            strSqlConf,
                                            listSqlParam,
                                            strSqlOrder,
                                            strCharacterCd,
                                            strCsvNm,
                                            boolHeaderOnOff,
                                            null,
                                            strSeparatorFont,
                                            boolBomOnOff,
                                            db,
                                            connection,
                                            false,
                                            ref message);
            }
            catch (Exception e)
            {
                // 失敗の場合
                message = SystemMessageUtil.Get("ME80014");
                logger.Error(message);
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
            }
            return RET_FAIL;
        }

        /// <summary>
        /// CSV出力共通部品（SQL設定）
        /// </summary>
        /// <param name="strUserId">ユーザID</param>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="strKumiaitoCd">組合等コード</param>
        /// <param name="strShishoCd">支所コード</param>
        /// <param name="strSyoriNm">処理名</param>
        /// <param name="strBatchNowSet">バッチ即時強制指定</param>
        /// <param name="boolBatchAlert">バッチ処理前警告</param>
        /// <param name="strZipFileNm">zipファイル名</param>
        /// <param name="strBatchJokenDisp">バッチ条件（表示用）</param>
        /// <param name="listCsvOutputSql">CSV出力SQLクラスList</param>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <param name="csvzipfile">出力されたzipファイル（出力パラメータ）</param>
        /// <returns>処理成否</returns>
        public static string CsvOutputSql(string strUserId,
                                            string strSystemKbn,
                                            string strTodofukenCd,
                                            string strKumiaitoCd,
                                            string strShishoCd,
                                            string strSyoriNm,
                                            string strBatchNowSet,
                                            bool boolBatchAlert,
                                            string strZipFileNm,
                                            string strBatchJokenDisp,
                                            List<CsvOutputSql> listCsvOutputSql,
                                            DbContext db,
                                            ref string message,
                                            ref byte[] csvzipfile)
        {
            var param = new CsvOutputParam
            {
                UserId = strUserId,
                SystemKbn = strSystemKbn,
                TodofukenCd = strTodofukenCd,
                KumiaitoCd = strKumiaitoCd,
                ShishoCd = strShishoCd,
                SyoriNm = strSyoriNm,
                BatchNowSet = strBatchNowSet,
                BatchAlert = boolBatchAlert,
                ZipFileNm = strZipFileNm,
                BatchJokenDisp = strBatchJokenDisp,
                CsvOutputJokenList = listCsvOutputSql.IsNullOrEmpty() ? null :
                                        listCsvOutputSql.Select(x => new CsvOutputJokenInternal(x)).ToList()
            };

            return CsvOutputCommon(param, db, true, ref message, ref csvzipfile);
        }

        /// <summary>
        /// CSVファイル取得共通部品（SQL設定）
        /// </summary>
        /// <param name="strOutputPath">出力先</param>
        /// <param name="strSql">SQL本体</param>
        /// <param name="listSqlParam">検索条件（パラメータ）</param>
        /// <param name="strCharacterCd">文字コード</param>
        /// <param name="strCsvNm">CSVファイル名</param>
        /// <param name="boolHeaderOnOff">ヘッダ有無</param>
        /// <param name="listHeader">ヘッダ文字列リスト</param>
        /// <param name="strSeparatorFont">セパレーター</param>
        /// <param name="boolBomOnOff">BOMコード有無</param>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>処理成否</returns>
        public static int CsvFileOutputSql(string strOutputPath,
                                            string strSql,
                                            List<SqlParam> listSqlParam,
                                            string strCharacterCd,
                                            string strCsvNm,
                                            bool boolHeaderOnOff,
                                            List<string> listHeader,
                                            string strSeparatorFont,
                                            bool boolBomOnOff,
                                            DbContext db,
                                            ref string message)
        {
            try
            {
                using var connection = db.Database.GetDbConnection() as NpgsqlConnection;
                connection.Open();

                return CsvFileOutputComon(strOutputPath,
                                            null,
                                            strSql,
                                            listSqlParam,
                                            null,
                                            strCharacterCd,
                                            strCsvNm,
                                            boolHeaderOnOff,
                                            listHeader,
                                            strSeparatorFont,
                                            boolBomOnOff,
                                            db,
                                            connection,
                                            true,
                                            ref message);
            }
            catch (Exception e)
            {
                // 失敗の場合
                message = SystemMessageUtil.Get("ME80014");
                logger.Error(message);
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
            }
            return RET_FAIL;
        }

        /// <summary>
        /// CSVファイル取得共通部品
        /// </summary>
        /// <param name="strOutputPath">出力先</param>
        /// <param name="strSerchTarget">検索対象</param>
        /// <param name="strSqlConf">検索条件（SQL）</param>
        /// <param name="listSqlParam">検索条件（パラメータ）</param>
        /// <param name="strSqlOrder">並び順</param>
        /// <param name="strCharacterCd">文字コード</param>
        /// <param name="strCsvNm">CSVファイル名</param>
        /// <param name="boolHeaderOnOff">ヘッダ有無</param>
        /// <param name="strSeparatorFont">セパレーター</param>
        /// <param name="boolBomOnOff">BOMコード有無</param>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="isExecSql">SQL本体実行かどうか（true：SQL本体実行の場合）</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>処理成否</returns>
        private static int CsvFileOutputComon(string strOutputPath,
                                                string strSerchTarget,
                                                string strSqlConf,
                                                List<SqlParam> listSqlParam,
                                                string strSqlOrder,
                                                string strCharacterCd,
                                                string strCsvNm,
                                                bool boolHeaderOnOff,
                                                List<string> listHeader,
                                                string strSeparatorFont,
                                                bool boolBomOnOff,
                                                DbContext db,
                                                NpgsqlConnection connection,
                                                bool isExecSql,
                                                ref string message)
        {
            try
            {
                // sql用定数定義
                var sql = new StringBuilder();

                if (isExecSql)
                {
                    sql.Append(strSqlConf);
                }
                else
                {
                    sql.Append("SELECT * FROM ");
                    sql.Append(strSerchTarget);

                    // 検索対象
                    if (!string.IsNullOrEmpty(strSqlConf))
                    {
                        sql.Append(" WHERE ");
                        sql.Append(strSqlConf);
                    }

                    // 検索条件（SQL）
                    if (!string.IsNullOrEmpty(strSqlOrder))
                    {
                        sql.Append(" ORDER BY ");
                        sql.Append(strSqlOrder);
                    }
                }

                // パラメータ
                List<NpgsqlParameter> parameters = null;
                // 検索条件（パラメータ）をSQL実行用に詰め直す
                if (!listSqlParam.IsNullOrEmpty())
                {
                    parameters = GetNpgsqlParameters(listSqlParam);
                }

                // ヘッダー部データ保存用変数
                var headerStr = new StringBuilder();

                // セパレータ
                var separator = CommonFuncUtil.GetHanyokubunKbnNm("separator", strSeparatorFont);

                if (boolHeaderOnOff)
                {
                    List<string> headerList = null;
                    if (isExecSql)
                    {
                        headerList = listHeader;
                    }
                    else
                    {
                        // 引数：ヘッダ有無がTrue（ヘッダあり）の場合、列論理名、物理名を取得する
                        var headerInfos = GetHeaderInfo(db, db.Model.GetDefaultSchema(), strSerchTarget);
                        headerList = headerInfos.Select(x => x.LogicalName).ToList();
                    }

                    headerList.ForEach(x => AddItem(headerStr, separator, x));
                }

                // CSVファイル名
                string fileName = strCsvNm;

                if (!isExecSql)
                {
                    // 引数：CSVファイル名が設定なしの場合
                    if (string.IsNullOrEmpty(strCsvNm))
                    {
                        // テーブル名称論理名、物理名を取得
                        var tableInfo = GetTableInfo(db, db.Model.GetDefaultSchema(), strSerchTarget);
                        fileName = tableInfo.TableName + CoreConst.FILE_EXTENSION_CSV;
                    }
                }

                // ファイルパス
                var filePath = Path.Combine(strOutputPath, fileName);

                // 文字コード
                Encoding encoding = null;
                if (CoreConst.CharacterCode.UTF8.ToString("d").Equals(strCharacterCd))
                {
                    // UTF-8
                    encoding = new UTF8Encoding(boolBomOnOff);
                }
                else
                {
                    // SJIS
                    encoding = Encoding.GetEncoding(CommonFuncUtil.GetHanyokubunKbnNm("character cd", strCharacterCd));
                }

                using var sw = new StreamWriter(filePath, true, encoding);
                //using var connection = db.Database.GetDbConnection() as NpgsqlConnection;
                using var command = new NpgsqlCommand
                {
                    Connection = connection,
                    CommandText = sql.ToString(),
                };

                //connection.Open();

                if (!parameters.IsNullOrEmpty())
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                if (!string.IsNullOrEmpty(ConfigUtil.Get(CoreConst.COMMAND_TIMEOUT)))
                {
                    command.CommandTimeout = ConfigUtil.GetInt(CoreConst.COMMAND_TIMEOUT);
                }

                // SQLをログに出力する
                logger.Info("CSVファイル取得共通部品 SQL実行開始");
                logger.Info(command.CommandText);
                if (!parameters.IsNullOrEmpty())
                {
                    foreach (var param in parameters)
                    {
                        logger.Info("-- " + param.ParameterName + ": " + "'" + param.NpgsqlValue + "'" + " (Type = " + param.NpgsqlDbType + ", IsNullable = " + param.IsNullable.ToString() + ")");
                    }
                }

                // SQL実行
                NpgsqlDataReader reader = command.ExecuteReader();
                logger.Info("CSVファイル取得共通部品 SQL実行終了");

                // 行データを保存するStringBuilder
                var dataStr = new StringBuilder();

                // ループカウンタ
                Int64 lCnt = 1;

                bool writeflag = true;

                if (boolHeaderOnOff)
                {
                    // ヘッダー部出力
                    sw.WriteLine(headerStr);
                }

                // データが0件の場合
                if (!reader.HasRows)
                {
                    // StreamWriterを閉じる
                    sw.Flush();
                    sw.Close();
                    // DB接続CLOSE
                    //connection.Dispose();
                    //connection.Close();

                    return RET_SUCCESS;
                }

                while (reader.Read())
                {
                    writeflag = true;

                    NpgsqlDataReader record = reader;
                    for (int i = 0; i <= record.FieldCount - 1; i++)
                    {
                        if (record.IsDBNull(i) || string.IsNullOrEmpty(record[i].ToString()))
                        {
                            dataStr.Append("\"\"");
                        }
                        else
                        {
                            // データ中にダブルクォーテーションがある場合は、直前にダブルクォーテーションをひとつ付加する。
                            // データの囲み　→　データはすべて「""」で囲む。
                            dataStr.Append(string.Format("\"{0}\"", record[i].ToString().Replace("\"", "\"\"")));
                        }

                        if (i == record.FieldCount - 1)
                        {
                            dataStr.AppendLine();
                            if (lCnt % FLASH_COUNT == 0)
                            {
                                // 行データ書き込む
                                sw.Write(dataStr);
                                dataStr.Clear();
                                writeflag = false;
                            }
                        }
                        else
                        {
                            // 区切り文字
                            dataStr.Append(separator);
                        }
                    }

                    lCnt++;
                }

                // 行データ書き込む
                if (writeflag)
                {
                    sw.Write(dataStr);
                    dataStr.Clear();
                }

                // StreamWriterを閉じる
                sw.Flush();
                sw.Close();

                // Call Close when done reading.
                reader.Close();

                // DB接続CLOSE
                //connection.Dispose();
                //connection.Close();

                return RET_SUCCESS;
            }
            catch (Exception e)
            {
                // 失敗の場合
                message = SystemMessageUtil.Get("ME80014");
                logger.Error(message);
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
            }
            return RET_FAIL;
        }

        /// <summary>
        /// CSV出力共通部品
        /// 共通処理
        /// </summary>
        /// <param name="param">CSV出力共通部品引数</param>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="isExecSql">SQL本体実行かどうか（true：SQL本体実行の場合）</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <param name="csvzipfile">出力されたzipファイル</param>
        /// <returns>処理結果</returns>
        private static string CsvOutputCommon(CsvOutputParam param,
                                                DbContext db,
                                                bool isExecSql,
                                                ref string message,
                                                ref byte[] csvzipfile)
        {
            // バッチ即時強制指定
            if (string.IsNullOrEmpty(param.BatchNowSet))
            {
                param.BatchNowSet = BatchNowSet.NotSpecified;
            }

            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            string result = string.Empty;
            try
            {
                // 必須チェック
                if (!CheckCsvOutputParamRequired(param, isExecSql, ref message))
                {
                    return RET_ERROR_ARGUMENT;
                }

                // 桁数チェック
                if (!CheckCsvOutputParamMaxLength(param, isExecSql, ref message))
                {
                    return RET_ERROR_ARGUMENT;
                }

                // マスタ整合性チェック
                if (!CheckCsvOutputParamMaster(param, isExecSql, ref message))
                {
                    return RET_ERROR_ARGUMENT;
                }

                // 引数：バッチ即時強制指定が0（バッチ・即時指定なし）、または、2（即時）の場合、かつ、引数：DBコンテキストが指定なしの場合
                if ((BatchNowSet.NotSpecified.Equals(param.BatchNowSet) || BatchNowSet.Now.Equals(param.BatchNowSet)) && db == null)
                {
                    message = SystemMessageUtil.Get("ME80004", "DBコンテキスト");
                    return RET_ERROR_ARGUMENT;
                }

                // 引数：バッチ即時強制指定
                if (BatchNowSet.Batch.Equals(param.BatchNowSet))
                {
                    // 引数：バッチ即時強制指定が1（バッチ）の場合
                    if (InsertBatchYoyaku(param, systemDate, isExecSql, ref message))
                    {
                        // バッチ予約完了
                        result = RET_SUCCESS_BATCH_RESERVE;
                    }
                    else
                    {
                        // バッチ予約エラー
                        result = RET_ERROR_BATCH_RESERVE;
                    }

                }
                else if (BatchNowSet.Now.Equals(param.BatchNowSet))
                {
                    // 引数：バッチ即時強制指定が2（即時）の場合
                    if (OutputCSV(param, systemDate, db, isExecSql, ref csvzipfile, ref message))
                    {
                        // 即時実行完了
                        result = RET_SUCCESS_IMMEDIATE_EXECUTION;
                    }
                    else
                    {
                        // 即時実行エラー
                        result = RET_ERROR_IMMEDIATE_EXECUTION;
                    }
                }
                else
                {
                    // 引数：バッチ即時強制指定が0（バッチ・即時指定なし）の場合

                    if (param.CsvOutputJokenList.Count == 1)
                    {
                        // 引数：CSV出力条件クラスListの件数が１件の場合
                        var recordCount = GetRcordCount(param.CsvOutputJokenList[0], db, isExecSql);
                        if (recordCount == 0)
                        {
                            // 0件の場合
                            result = RET_ERROR_ZERO_RECORDS;
                        }
                        else
                        {
                            // 件数が1件以上の場合

                            var immedCsvMaxCount = 0;
                            if (!isExecSql)
                            {
                                // 即時CSV出力上限件数マスタから上限件数を取得する。
                                immedCsvMaxCount = GetImmedCsvMaxCount(param.SystemKbn, param.CsvOutputJokenList[0].SearchTaget);
                            }

                            if (immedCsvMaxCount == 0)
                            {
                                // 取得できなかった場合[定数（設定ファイル）：MaxCntDefault]を取得
                                immedCsvMaxCount = ConfigUtil.GetInt(CoreConst.MAX_CNT_DEFAULT);
                            }

                            if (recordCount > immedCsvMaxCount)
                            {
                                // 引数：バッチ処理前警告がTrueの場合
                                if (param.BatchAlert)
                                {
                                    // バッチ予約警告
                                    result = RET_WARN_BATCH_RESERVE;
                                }
                                else
                                {
                                    // バッチ予約登録を実行する。
                                    if (InsertBatchYoyaku(param, systemDate, isExecSql, ref message))
                                    {
                                        // バッチ予約完了
                                        result = RET_SUCCESS_BATCH_RESERVE;
                                    }
                                    else
                                    {
                                        // バッチ予約エラー
                                        result = RET_ERROR_BATCH_RESERVE;
                                    }
                                }
                            }
                            else
                            {
                                // 「即時CSV出力」を実行する。
                                if (OutputCSV(param, systemDate, db, isExecSql, ref csvzipfile, ref message))
                                {
                                    // 即時実行完了
                                    result = RET_SUCCESS_IMMEDIATE_EXECUTION;
                                }
                                else
                                {
                                    // 即時実行エラー
                                    result = RET_ERROR_IMMEDIATE_EXECUTION;
                                }
                            }
                        }
                    }
                    else
                    {
                        // 引数：CSV出力条件クラスListの件数が複数の場合

                        // 引数：バッチ処理前警告がTrueの場合
                        if (param.BatchAlert)
                        {
                            // バッチ予約警告
                            result = RET_WARN_BATCH_RESERVE;
                        }
                        else
                        {
                            // バッチ予約登録を実行する。
                            if (InsertBatchYoyaku(param, systemDate, isExecSql, ref message))
                            {
                                // バッチ予約完了
                                result = RET_SUCCESS_BATCH_RESERVE;
                            }
                            else
                            {
                                // バッチ予約エラー
                                result = RET_ERROR_BATCH_RESERVE;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                message = SystemMessageUtil.Get("MF00001");
                return null;
            }

            return result;
        }

        /// <summary>
        /// パラメータ詰め直し処理
        /// </summary>
        /// <param name="listSqlParam">検索条件（パラメータ）</param>
        /// <returns>SqlParamに保持するパラメータをNpgsqlParameterに詰めなおしたリスト</returns>
        private static List<NpgsqlParameter> GetNpgsqlParameters(List<SqlParam> listSqlParam)
        {
            var parameters = new List<NpgsqlParameter>();

            foreach (var param in listSqlParam)
            {
                SqlParamType sqlParamType = (SqlParamType)Enum.ToObject(typeof(SqlParamType), param.type);

                switch (sqlParamType)
                {
                    case SqlParamType.Int:
                        // SqlParam.type = 1（整数）の場合
                        // valueをint型に変換した上で、NpgsqlParameterに詰め直す
                        parameters.Add(new NpgsqlParameter(param.name, int.Parse(param.value)));
                        break;

                    case SqlParamType.Decimal:
                        // SqlParam.type = 2（小数含む数値）の場合
                        // valueをdecimal型に変換した上で、NpgsqlParameterに詰め直す
                        parameters.Add(new NpgsqlParameter(param.name, decimal.Parse(param.value)));
                        break;

                    case SqlParamType.String:
                        // SqlParam.type = 3（文字列）の場合
                        // 特に値を変換せず、NpgsqlParameterに詰め直す
                        parameters.Add(new NpgsqlParameter(param.name, param.value));
                        break;

                    case SqlParamType.DateTime:
                        // SqlParam.type = 4（日時）の場合
                        // 値を日時型に変換した上で、NpgsqlParameterに詰め直す
                        parameters.Add(new NpgsqlParameter(param.name, DateTime.Parse(param.value)));
                        break;

                    case SqlParamType.ArrayVarchar:
                        // SqlParam.type = 5（リスト（NpgsqlDbType.Array | NpgsqlDbType.Varchar））の場合
                        // 値をリスト（NpgsqlDbType.Array | NpgsqlDbType.Varchar）型に変換した上で、NpgsqlParameterに詰め直す
                        parameters.Add(new NpgsqlParameter(param.name, NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                        {
                            Value = new List<string>(param.value.Split(CoreConst.SYMBOL_COMMA))
                        });
                        break;
                }
            }

            return parameters;
        }

        /// <summary>
        /// 列論理名、物理名を取得する
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="schemaName">DBの接続先スキーマ名</param>
        /// <param name="serchTarget">検索対象</param>
        /// <returns>列論理名、物理名のリスト</returns>
        private static List<HeaderInfo> GetHeaderInfo(DbContext db, string schemaName, string serchTarget)
        {
            StringBuilder sql = new StringBuilder();

            // sql定義
            sql.Append("SELECT ");
            sql.Append("    pa.attname AS PhysicalName, ");
            sql.Append("    COALESCE(pd.description, pa.attname) AS LogicalName ");
            sql.Append("FROM ");
            sql.Append("    pg_attribute pa ");
            sql.Append("    LEFT JOIN pg_description pd ");
            sql.Append("        ON pd.objoid = pa.attrelid ");
            sql.Append("        AND pd.objsubid = pa.attnum ");
            sql.Append("    JOIN pg_class pc ");
            sql.Append("        ON pc.oid = pa.attrelid ");
            sql.Append("    JOIN pg_namespace pn ");
            sql.Append("        ON pn.oid = pc.relnamespace ");
            sql.Append("WHERE ");
            sql.Append("    pn.nspname = @SchemaName ");
            sql.Append("    AND pc.relname = @SerchTarget ");
            sql.Append("    AND pa.attnum > 0 ");
            sql.Append("ORDER BY ");
            sql.Append("    pa.attnum; ");

            var parameters = new List<NpgsqlParameter>
            {
                new("@SchemaName", schemaName),
                new("@SerchTarget", serchTarget)
            };

            logger.Info("列論理名、物理名を取得する");
            return db.Database.SqlQueryRaw<HeaderInfo>(sql.ToString(), parameters.ToArray()).ToList();
        }

        /// <summary>
        /// テーブルの論理名、物理名を取得する
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="schemaName">DBの接続先スキーマ名</param>
        /// <param name="serchTarget">検索対象</param>
        /// <returns>テーブルの論理名、物理名</returns>
        private static TableInfo GetTableInfo(DbContext db, string schemaName, string serchTarget)
        {
            // sql用定数定義
            var sql =
                "SELECT " +
                "    psat.relname AS \"TableName\" " +
                "    , tn.TABLE_COMMENT AS \"TableComment\" " +
                "FROM " +
                "    pg_stat_all_tables psat " +
                "    , (  " +
                "        SELECT " +
                "            psut.relname AS TABLE_NAME " +
                "            , pd.description AS TABLE_COMMENT " +
                "        FROM " +
                "            pg_stat_user_tables psut " +
                "            , pg_description pd  " +
                "        WHERE " +
                "            psut.schemaname = @SchemaName_1 " +
                "            AND psut.relname = @SerchTarget_1 " +
                "            AND psut.relid = pd.objoid  " +
                "            AND pd.objsubid = 0 " +
                "    ) tn  " +
                "WHERE " +
                "    psat.schemaname = @SchemaName_2 " +
                "    AND psat.relname = tn.TABLE_NAME " +
                "    AND psat.relname = @SerchTarget_2 ";

            var parameters = new List<NpgsqlParameter>
            {
                new("@SchemaName_1", schemaName),
                new("@SerchTarget_1", serchTarget),
                new("@SchemaName_2", schemaName),
                new("@SerchTarget_2", serchTarget)
            };

            logger.Info("テーブルの論理名、物理名を取得する");
            return db.Database.SqlQueryRaw<TableInfo>(sql, parameters.ToArray()).SingleOrDefault();
        }

        /// <summary>
        /// 即時CSV出力上限件数を取得する
        /// </summary>
        /// <param name="systemKbn">システム区分</param>
        /// <param name="searchTaget">テーブル名</param>
        /// <returns>即時CSV出力上限件数</returns>
        private static int GetImmedCsvMaxCount(string systemKbn, string searchTaget)
        {
            // sql用定数定義
            var sql =
                "SELECT " +
                "    immed_csv_max_count AS \"Value\" " +
                "FROM " +
                "    m_immed_csv_max_count " +
                "WHERE " +
                "    system_kbn = @SystemKbn " +
                "    AND table_nm = @TableNm ";

            var parameters = new List<NpgsqlParameter>
            {
                new("@SystemKbn", systemKbn),
                new("@TableNm", searchTaget),
            };

            using (var db = new SystemCommonContext())
            {
                logger.Info("即時CSV出力上限件数を取得する");
                return db.Database.SqlQueryRaw<int?>(sql.ToString(), parameters.ToArray()).SingleOrDefault() ?? 0;
            }
        }

        /// <summary>
        /// ダブルクォーテーションを付与し、区切り文字を追加する
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="separator">区切り文字</param>
        /// <param name="item">値</param>
        private static void AddItem(StringBuilder sb, string separator, string item)
        {
            if (sb.Length != 0)
            {
                // 区切り文字
                sb.Append(separator);
            }

            if (string.IsNullOrEmpty(item))
            {
                // データの囲み　→　空欄の場合は""とする。
                sb.Append("\"\"");
            }
            else
            {
                // データ中にダブルクォーテーションがある場合は、直前にダブルクォーテーションをひとつ付加する。
                // データの囲み　→　データはすべて「""」で囲む。
                sb.Append(string.Format("\"{0}\"", item.Replace("\"", "\"\"")));
            }
        }

        /// <summary>
        /// CSV出力件数を取得する
        /// </summary>
        /// <param name="joken">CSV出力条件</param>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="isExecSql">SQL本体実行かどうか（true：SQL本体実行の場合）</param>
        /// <returns>CSV出力件数</returns>
        private static int GetRcordCount(CsvOutputJokenInternal joken, DbContext db, bool isExecSql)
        {
            // sql用定数定義
            var sql = new StringBuilder();

            // パラメータ
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT COUNT(1) AS \"Value\" FROM ");

            if (isExecSql)
            {
                sql.Append("( ");
                sql.Append(joken.SqlConf);
                sql.Append(") ");
            }
            else
            {
                // 検索対象
                sql.Append(joken.SearchTaget);

                // 検索条件（SQL）
                if (!string.IsNullOrEmpty(joken.SqlConf))
                {
                    sql.Append(" WHERE ");
                    sql.Append(joken.SqlConf);
                }
            }

            // 検索条件（パラメータ）をSQL実行用に詰め直す
            if (!joken.SqlParams.IsNullOrEmpty())
            {
                parameters = GetNpgsqlParameters(joken.SqlParams);
            }

            logger.Info("CSV出力件数を取得する");
            return db.Database.SqlQueryRaw<int>(sql.ToString(), parameters.ToArray()).Single();
        }

        #region 引数チェック（CSV出力共通部品）
        /// <summary>
        /// 必須チェック（CSV出力共通部品）
        /// </summary>
        /// <param name="param">CSV出力共通部品引数</param>
        /// <param name="isExecSql">true：SQL設定の場合</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckCsvOutputParamRequired(CsvOutputParam param, bool isExecSql, ref string message)
        {
            // ユーザID
            if (!CommonFuncUtil.ValidateRequired(
                    param.UserId,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.UserId)),
                    ref message))
            {
                return false;
            }

            // システム区分
            if (!CommonFuncUtil.ValidateRequired(
                    param.SystemKbn,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)),
                    ref message))
            {
                return false;
            }

            // 都道府県コード
            if (!CommonFuncUtil.ValidateRequired(
                    param.TodofukenCd,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.TodofukenCd)),
                    ref message))
            {
                return false;
            }

            // 処理名
            if (!CommonFuncUtil.ValidateRequired(
                    param.SyoriNm,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SyoriNm)),
                    ref message))
            {
                return false;
            }

            // CSV出力条件クラスList
            if (param.CsvOutputJokenList.IsNullOrEmpty())
            {
                message = SystemMessageUtil.Get("ME80004", isExecSql ? "CSV出力SQLクラスList" : "CSV出力条件クラスList");
                return false;
            }

            foreach (var joken in param.CsvOutputJokenList)
            {
                if (isExecSql)
                {
                    // CSV出力SQLクラスList[SQL本体]
                    if (string.IsNullOrEmpty(joken.SqlConf))
                    {
                        message = SystemMessageUtil.Get("ME80004", "CSV出力SQLクラスList[SQL本体]");
                        return false;
                    }

                    // CSV出力SQLクラスList[CSVファイル名]
                    if (string.IsNullOrEmpty(joken.CsvNm))
                    {
                        message = SystemMessageUtil.Get("ME80004", "CSV出力SQLクラスList[CSVファイル名]");
                        return false;
                    }

                    // CSV出力SQLクラスList[ヘッダ文字列リスト]
                    if (joken.HeaderOnOff && joken.Headers.IsNullOrEmpty())
                    {
                        message = SystemMessageUtil.Get("ME80004", "CSV出力SQLクラスList[ヘッダ文字列リスト]");
                        return false;
                    }
                }
                else
                {
                    // CSV出力条件クラスList[検索対象]
                    if (string.IsNullOrEmpty(joken.SearchTaget))
                    {
                        message = SystemMessageUtil.Get("ME80004", "CSV出力条件クラスList[検索対象]");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 桁数チェック（CSV出力共通部品）
        /// </summary>
        /// <param name="param">CSV出力共通部品引数</param>
        /// <param name="isExecSql">true：SQL設定の場合</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckCsvOutputParamMaxLength(CsvOutputParam param, bool isExecSql, ref string message)
        {
            var tBatchYoyaku = new TBatchYoyaku();
            var tBatchCsvQuery = new TBatchCsvQuery();

            // ユーザID
            if (!CommonFuncUtil.ValidateMaxLength(
                    param.UserId,
                    CommonFuncUtil.GetMaximumLength(tBatchYoyaku.GetType(), nameof(tBatchYoyaku.BatchYoyakuId)),
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.UserId)),
                    ref message))
            {
                return false;
            }

            // システム区分
            if (!CommonFuncUtil.ValidateMaxLength(
                    param.SystemKbn,
                    CommonFuncUtil.GetMaximumLength(tBatchYoyaku.GetType(), nameof(tBatchYoyaku.SystemKbn)),
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)),
                    ref message))
            {
                return false;
            }

            // 都道府県コード
            if (!CommonFuncUtil.ValidateMaxLength(
                    param.TodofukenCd,
                    CommonFuncUtil.GetMaximumLength(tBatchYoyaku.GetType(), nameof(tBatchYoyaku.TodofukenCd)),
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.TodofukenCd)),
                    ref message))
            {
                return false;
            }

            // 処理名
            if (!CommonFuncUtil.ValidateMaxLength(
                    param.SyoriNm,
                    CommonFuncUtil.GetMaximumLength(tBatchYoyaku.GetType(), nameof(tBatchYoyaku.BatchYoyakuShori)),
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SyoriNm)),
                    ref message))
            {
                return false;
            }

            // CSV出力条件クラスList
            foreach (var joken in param.CsvOutputJokenList)
            {
                if (!isExecSql)
                {
                    // CSV出力条件クラスList[検索対象]
                    if (!CommonFuncUtil.ValidateMaxLength(
                            joken.SearchTaget,
                            CommonFuncUtil.GetMaximumLength(tBatchCsvQuery.GetType(), nameof(tBatchCsvQuery.TargetEntity)),
                            "CSV出力条件クラスList[検索対象]",
                            ref message))
                    {
                        return false;
                    }
                }

                // CSVファイル名
                if (!CommonFuncUtil.ValidateMaxLength(
                        joken.CsvNm,
                        CommonFuncUtil.GetMaximumLength(tBatchCsvQuery.GetType(), nameof(tBatchCsvQuery.CsvFileName)),
                        isExecSql ? "CSV出力SQLクラスList[CSVファイル名]" : "CSV出力条件クラスList[CSVファイル名]",
                        ref message))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// マスタ整合性チェック（CSV出力共通部品）
        /// </summary>
        /// <param name="param">CSV出力共通部品引数</param>
        /// <param name="isExecSql">true：SQL設定の場合</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckCsvOutputParamMaster(CsvOutputParam param, bool isExecSql, ref string message)
        {
            using (var db = new SystemCommonContext())
            {

                // システム区分：汎用区分マスタ「system_kbn」
                if (!CommonFuncUtil.ValidateHanyokubun(db, "system_kbn", param.SystemKbn,
                        CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)), ref message))
                {
                    return false;
                }

                // 都道府県コード：都道府県マスタ
                if (!CommonFuncUtil.ValidateTodofuken(db, param.TodofukenCd, ref message))
                {
                    return false;
                }

                // 組合等コード：組合等マスタ
                if (!CommonFuncUtil.ValidateKumiaito(db, param.TodofukenCd, param.KumiaitoCd, ref message))
                {
                    return false;
                }

                // 支所コード：名称M支所
                if (!string.IsNullOrEmpty(param.ShishoCd) && !CommonFuncUtil.ValidateShisho(db, param.TodofukenCd, param.KumiaitoCd, param.ShishoCd, ref message))
                {
                    return false;
                }

                // バッチ即時強制指定：汎用区分マスタ「batch_now_set」
                if (!CommonFuncUtil.ValidateHanyokubun(db, "batch_now_set", param.BatchNowSet,
                        CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchNowSet)), ref message))
                {
                    return false;
                }

                // CSV出力条件クラスList
                foreach (var joken in param.CsvOutputJokenList)
                {
                    // 文字コード：汎用区分マスタ「character cd」
                    if (!CommonFuncUtil.ValidateHanyokubun(db, "character cd", joken.CharacterCd, "文字コード", ref message))
                    {
                        return false;
                    }

                    // セパレータ：汎用区分マスタ「separator」
                    if (!CommonFuncUtil.ValidateHanyokubun(db, "separator", joken.SeparatorFont, "セパレーター", ref message))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// パッチ予約登録
        /// </summary>
        /// <param name="systemDate">システム日時</param>
        /// <param name="param">CSV出力共通部品引数</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>処理結果（true：成功、false：失敗）</returns>
        private static bool InsertBatchYoyaku(CsvOutputParam param, DateTime systemDate, bool isExecSql, ref string message)
        {
            string errMessage = string.Empty;
            long batchId = 0L;

            // バッチ予約登録インターフェースに引数を渡す。
            if (BatchUtil.InsertBatchYoyaku(BATCH_BUNRUI_CSV,
                                            param.SystemKbn,
                                            param.TodofukenCd,
                                            param.KumiaitoCd,
                                            param.ShishoCd,
                                            systemDate,
                                            param.UserId,
                                            param.SyoriNm,
                                            BATCH_NM_CSV,
                                            string.Format(BATCH_JOKEN_FORMAT, isExecSql ? BATCH_JOKEN_MODE_SQL : BATCH_JOKEN_MODE_TBL, param.ZipFileNm),
                                            param.BatchJokenDisp,
                                            MultiRunNgFlg.Possible,
                                            BATCH_TYPE_PATROL,
                                            null,
                                            LOCK_TARGET_FLG_NOT,
                                            ref errMessage,
                                            ref batchId) == RET_SUCCESS)
            {
                // バッチ予約成功時

                using var db = new SystemCommonContext();
                using var tx = db.Database.BeginTransaction();
                int serialNumber = 1;

                foreach (var joken in param.CsvOutputJokenList)
                {
                    TBatchCsvQuery query = new()
                    {
                        // バッチID
                        BatchId = batchId,
                        // 連番
                        SerialNumber = serialNumber++,
                        // 検索対象
                        TargetEntity = CommonFuncUtil.GetString(joken.SearchTaget),
                        // 検索条件（SQL）
                        QueryCondition = CommonFuncUtil.GetString(joken.SqlConf),
                        // 検索条件（パラメータ）
                        QueryParam = GetQueryParamJson(joken.SqlParams),
                        // 並び順
                        Sort = CommonFuncUtil.GetString(joken.SqlOrder),
                        // 文字コード
                        CharacterCd = CommonFuncUtil.GetString(joken.CharacterCd),
                        // CSVファイル名
                        CsvFileName = CommonFuncUtil.GetString(joken.CsvNm),
                        // ヘッダ有無
                        HeaderUmu = CommonFuncUtil.GetFlagStr(joken.HeaderOnOff),
                        // ヘッダ文字列リスト
                        HeaderList = isExecSql ? GetHeaderListJson(joken.Headers) : string.Empty,
                        // セパレータ
                        Separator = string.IsNullOrEmpty(joken.SeparatorFont) ? CoreConst.Separator.COMMA.ToString("d") : joken.SeparatorFont,
                        // BOMコード有無
                        BomCdUmu = CommonFuncUtil.GetFlagStr(joken.BomOnOff),
                        // 登録ユーザID
                        InsertUserId = param.UserId,
                        // 登録日時
                        InsertDate = systemDate,
                        // 更新ユーザID
                        UpdateUserId = param.UserId,
                        // 更新日時
                        UpdateDate = systemDate,
                    };

                    db.TBatchCsvQuerys.Add(query);
                }

                try
                {
                    logger.Info(string.Format("バッチCSV取得条件にバッチID：{0}を登録します。", batchId));
                    db.SaveChanges();
                    tx.Commit();

                    // バッチCSV取得条件登録成功
                    return true;
                }
                catch (DbUpdateException e)
                {
                    // 失敗の場合
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    tx.Rollback();
                    //throw;

                    // 『バッチ実行状況更新』インターフェースに引数を渡す。
                    // バッチCSV取得条件の登録に失敗しました。
                    message = SystemMessageUtil.Get("ME80016");

                    string msg = null;
                    if (BatchUtil.UpdateBatchYoyakuSts(batchId, BatchUtil.BATCH_STATUS_ERROR,
                                                        message, param.UserId, ref msg) == 0)
                    {
                        // 失敗の場合
                        logger.Error(msg);
                    }

                    // バッチCSV取得条件登録失敗
                    return false;
                }
            }

            // バッチ予約失敗時
            // CSV出力バッチの予約登録に失敗しました。
            message = SystemMessageUtil.Get("ME80017");

            if (!string.IsNullOrEmpty(errMessage))
            {
                logger.Error(errMessage);
            }

            return false;
        }

        /// <summary>
        /// 検索条件（パラメータ）をJSONシリアライズした値を取得する
        /// </summary>
        /// <param name="param">検索条件（パラメータ）</param>
        /// <returns>JSON文字列、検索条件（パラメータ）が指定されていない場合はnull</returns>
        private static string GetQueryParamJson(List<SqlParam> sqlParams)
        {
            string json = null;
            if (!sqlParams.IsNullOrEmpty())
            {
                json = JsonSerializer.Serialize(sqlParams);
            }
            return json;
        }

        /// <summary>
        /// ヘッダ文字列リストをJSONシリアライズした値を取得する
        /// </summary>
        /// <param name="headers">ヘッダ文字列リスト</param>
        /// <returns>JSON文字列、ヘッダ文字列リストが指定されていない場合はnull</returns>
        private static string GetHeaderListJson(List<string> headers)
        {
            string json = null;
            if (!headers.IsNullOrEmpty())
            {
                json = JsonSerializer.Serialize(headers);
            }
            return json;
        }

        /// <summary>
        /// 即時CSV出力
        /// </summary>
        /// <param name="param">CSV出力共通部品引数</param>
        /// <param name="systemDate">システム日時</param>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="isExecSql">SQL本体実行かどうか（true：SQL本体実行の場合）</param>
        /// <param name="csvzipfile">出力されたzipファイル（出力パラメータ）</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>処理結果（true：成功、false：失敗）</returns>
        private static bool OutputCSV(CsvOutputParam param, DateTime systemDate, DbContext db, bool isExecSql, ref byte[] csvzipfile, ref string message)
        {
            // CSVの一時出力フォルダを作成する。
            var csvTempFolder = FolderUtil.CreateCsvTempFolder(systemDate, param.ZipFileNm);
            try
            {
                using var connection = db.Database.GetDbConnection() as NpgsqlConnection;
                connection.Open();

                foreach (var joken in param.CsvOutputJokenList)
                {
                    if (CsvFileOutputComon(csvTempFolder,
                                            joken.SearchTaget,
                                            joken.SqlConf,
                                            joken.SqlParams,
                                            joken.SqlOrder,
                                            joken.CharacterCd,
                                            joken.CsvNm,
                                            joken.HeaderOnOff,
                                            joken.Headers,
                                            joken.SeparatorFont,
                                            joken.BomOnOff,
                                            db,
                                            connection,
                                            isExecSql,
                                            ref message) == RET_FAIL)
                    {
                        // CSV出力失敗
                        message = SystemMessageUtil.Get("ME80015");
                        logger.Error(message);
                        return false;
                    }
                }

                // CSV出力成功

                // CSVの一時出力フォルダをzip化する。
                using (var outStream = new MemoryStream())
                {
                    ZipUtil.CreateZip(csvTempFolder, outStream);
                    csvzipfile = outStream.ToArray();
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                message = SystemMessageUtil.Get("ME80015");
                logger.Error(message);
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                return false;
            }
            finally
            {
                // CSV一時出力フォルダの削除
                // CSV一時出力フォルダが存在する場合、削除する
                if (Directory.Exists(csvTempFolder))
                {
                    Directory.Delete(csvTempFolder, true);
                }
            }

            return true;
        }

        /// <summary>
        /// 文字コードが設定されていない場合、1（UTF-8）を返す。
        /// 上記以外の場合は、引数で指定された文字列を返す。
        /// </summary>
        /// <param name="characterCd">検査対象文字列</param>
        /// <returns></returns>
        private static string GetCharacterCd(string characterCd)
        {
            if (string.IsNullOrEmpty(characterCd))
            {
                return CoreConst.CharacterCode.UTF8.ToString("d");
            }
            return characterCd;
        }

        /// <summary>
        /// セパレータが設定されていない場合、1（カンマ）を返す。
        /// 上記以外の場合は、引数で指定された文字列を返す。
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        private static string GetSeparator(string separator)
        {
            if (string.IsNullOrEmpty(separator))
            {
                return CoreConst.Separator.COMMA.ToString("d");
            }
            return separator;
        }

        #region 引数格納用クラス
        /// <summary>
        /// CSV出力共通部品引数
        /// </summary>
        private class CsvOutputParam
        {
            /// <summary>
            /// ユーザID
            /// </summary>
            [DisplayName("ユーザID")]
            public string UserId { get; set; }

            /// <summary>
            /// システム区分
            /// </summary>
            [DisplayName("システム区分")]
            public string SystemKbn { get; set; }

            /// <summary>
            /// 都道府県コード
            /// </summary>
            [DisplayName("都道府県コード")]
            public string TodofukenCd { get; set; }

            /// <summary>
            /// 組合等コード
            /// </summary>
            [DisplayName("組合等コード")]
            public string KumiaitoCd { get; set; }

            /// <summary>
            /// 支所コード
            /// </summary>
            [DisplayName("支所コード")]
            public string ShishoCd { get; set; }

            /// <summary>
            /// 処理名
            /// </summary>
            [DisplayName("処理名")]
            public string SyoriNm { get; set; }

            /// <summary>
            /// バッチ即時強制指定
            /// </summary>
            [DisplayName("バッチ即時強制指定")]
            public string BatchNowSet { get; set; }

            /// <summary>
            /// バッチ処理前警告
            /// </summary>
            [DisplayName("バッチ処理前警告")]
            public bool BatchAlert { get; set; }

            /// <summary>
            /// zipファイル名
            /// </summary>
            [DisplayName("zipファイル名")]
            public string ZipFileNm { get; set; }

            /// <summary>
            /// バッチ条件（表示用）
            /// </summary>
            [DisplayName("バッチ条件（表示用）")]
            public string BatchJokenDisp { get; set; }

            /// <summary>
            /// CSV出力条件クラスList
            /// </summary>
            [DisplayName("CSV出力条件クラスList")]
            public List<CsvOutputJokenInternal> CsvOutputJokenList { get; set; }
        }
        #endregion

        /// <summary>
        /// CSV出力条件クラス内部用
        /// </summary>
        private class CsvOutputJokenInternal
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="joken">CSV出力条件クラス</param>
            public CsvOutputJokenInternal(CsvOutputJoken joken)
            {
                SearchTaget = joken.SearchTaget;
                SqlConf = joken.SqlConf;
                SqlParams = joken.SqlParams;
                SqlOrder = joken.SqlOrder;
                CharacterCd = GetCharacterCd(joken.CharacterCd);
                CsvNm = joken.CsvNm;
                HeaderOnOff = joken.HeaderOnOff;
                SeparatorFont = GetSeparator(joken.SeparatorFont);
                BomOnOff = joken.BomOnOff;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="joken">CSV出力SQLクラス</param>
            public CsvOutputJokenInternal(CsvOutputSql joken)
            {
                SqlConf = joken.Sql;
                SqlParams = joken.SqlParams;
                SqlOrder = joken.SqlOrder;
                CharacterCd = GetCharacterCd(joken.CharacterCd);
                CsvNm = joken.CsvNm;
                HeaderOnOff = joken.HeaderOnOff;
                Headers = joken.Headers;
                SeparatorFont = GetSeparator(joken.SeparatorFont);
                BomOnOff = joken.BomOnOff;
            }

            /// <summary>
            /// 検索対象
            /// </summary>
            public string SearchTaget { get; set; }

            /// <summary>
            /// 検索条件（SQL）
            /// </summary>
            public string SqlConf { get; set; }

            /// <summary>
            /// 検索条件（パラメータ）
            /// </summary>
            public List<SqlParam> SqlParams { get; set; }

            /// <summary>
            /// 並び順
            /// </summary>
            public string SqlOrder { get; set; }

            /// <summary>
            /// 文字コード
            /// </summary>
            public string CharacterCd { get; set; }

            /// <summary>
            /// CSVファイル名
            /// </summary>
            public string CsvNm { get; set; }

            /// <summary>
            /// ヘッダ有無
            /// </summary>
            public bool HeaderOnOff { get; set; }

            /// <summary>
            /// ヘッダ文字列リスト
            /// </summary>
            public List<string> Headers { get; set; }

            /// <summary>
            /// セパレーター
            /// </summary>
            public string SeparatorFont { get; set; }

            /// <summary>
            /// BOMコード有無
            /// </summary>
            public bool BomOnOff { get; set; }
        }

        /// <summary>
        /// 列論理名、物理名を保持するクラス
        /// </summary>
        private class HeaderInfo
        {
            /// <summary>
            /// 物理名
            /// </summary>
            public string PhysicalName { get; set; }

            /// <summary>
            /// 論理名
            /// </summary>
            public string LogicalName { get; set; }
        }

        /// <summary>
        /// テーブルの論理名、物理名を保持するクラス
        /// </summary>
        private class TableInfo
        {
            /// <summary>
            /// テーブルの論理名
            /// </summary>
            public string TableName { get; set; }

            /// <summary>
            /// テーブルの論理名
            /// </summary>
            public string TableComment { get; set; }
        }
    }
}