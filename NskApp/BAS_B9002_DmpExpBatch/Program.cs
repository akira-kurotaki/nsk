using BAS_B9002_DmpExpBatch.Common;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using Npgsql;
using System.Diagnostics;
using System.Text;

namespace BAS_B9002_DmpExpBatch
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "ダンプファイル出力";

        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static Program()
        {
            // Npgsql 6.0での「timestamp with time zone」非互換対応
            // Npgsql 6.0より前の動作に戻す
            // https://www.npgsql.org/doc/types/datetime.html#timestamps-and-timezones
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // SJIS(Shift_JIS)を使用可能にする
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// メイン処理
        /// </summary>
        /// <param name="args">パラメータ配列</param>
        static void Main(string[] args)
        {
            // 処理開始のログを出力する。
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " ダンプファイル出力バッチ"));
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var sysDate = DateUtil.GetSysDateTime();
            using (var db = new SystemCommonContext(ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                try
                {
                    // システム共通スキーマの定時バッチ起動管理テーブルを排他ロックする
                    var scheBatchRunManages = GetScheBatchRunManageInfo(db);
                    if (scheBatchRunManages.IsNullOrEmpty())
                    {
                        // 結果が空の場合、新規登録する
                        InsertTScheBatchRunManage(db, sysDate);
                    }
                    else
                    {
                        // システム共通スキーマの定時バッチ起動管理テーブルを更新する
                        var count = UpdateTScheBatchRunManage(db, sysDate);
                        if (count == 0)
                        {
                            logger.Error("既に他サーバのバッチが実行中のため、処理をキャンセルしました。");
                            Console.Error.WriteLine("既に他サーバのバッチが実行中のため、処理をキャンセルしました。");
                            // 処理結果（正常：0、エラー：1）
                            Environment.ExitCode = Constants.BATCH_EXECUT_FAILED;
                            return;
                        }
                    }

                    List<MBackupInfo> backupInfos = null;
                    try
                    {
                        // システム共通スキーマの「バックアップ対象情報」を取得する。
                        logger.Info("システム共通スキーマのバックアップ対象情報テーブルの情報を取得する。");
                        backupInfos = db.MBackupInfos.ToList();
                        // 「バックアップ対象情報」が取得できない（0件、エラー）場合
                        if (backupInfos.IsNullOrEmpty())
                        {
                            logger.Error("バックアップ対象情報が登録されていません。管理者に連絡してください。");
                            Console.Error.WriteLine("バックアップ対象情報が登録されていません。管理者に連絡してください。");
                            // 処理結果（正常：0、エラー：1）
                            Environment.ExitCode = Constants.BATCH_EXECUT_FAILED;
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Error("バックアップ対象情報が登録されていません。管理者に連絡してください。");
                        logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                        Console.Error.WriteLine("バックアップ対象情報が登録されていません。管理者に連絡してください。");
                        // 処理結果（正常：0、エラー：1）
                        Environment.ExitCode = Constants.BATCH_EXECUT_FAILED;
                        return;
                    }

                    // バックアップの実行
                    // ユーザー名
                    var username = ConfigUtil.Get(Constants.CONFIG_BACK_UP_USER);
                    // パスワード
                    var password = ConfigUtil.Get(Constants.CONFIG_BACK_UP_PASS);
                    // ダンプファイルを出力する一次フォルダ
                    var outputFolderTemp = ConfigUtil.Get(Constants.CONFIG_TEMP_BACK_UP_FOLDER);
                    // pg_dump.exeのパス
                    var pgDumpExePath = ConfigUtil.Get(Constants.CONFIG_PG_DUMP_PATH);

                    // 「バックアップ対象情報」の取得件数分以下の処理を行う。
                    foreach (var backupInfo in backupInfos)
                    {
                        // ダンプファイルパス
                        string tempDumpFile = Path.Combine(outputFolderTemp, backupInfo.TodofukenCd, $"{backupInfo.TodofukenCd}{Constants.FILE_EXTENSION_DUMP}");
                        var tempFoler = Path.GetDirectoryName(tempDumpFile);
                        // ダンプファイルを出力する一次フォルダがない場合、作成する
                        if (!Directory.Exists(tempFoler))
                        {
                            Directory.CreateDirectory(tempFoler);
                        }

                        try
                        {
                            // バックアップの実行準備
                            var processStartInfo = new ProcessStartInfo(pgDumpExePath)
                            {
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                Environment = { ["PGPASSWORD"] = password },
                                // pg_dump 「バックアップ対象情報」.バックアップ対象情報 -U 定数：BackUpUser -Fc > 定数：TempBackUpFolder\「バックアップ対象情報」.都道府県コード\「バックアップ対象情報」.都道府県コード.dump
                                Arguments = $"{Constants.HALF_WIDTH_SPACE}{backupInfo.BackupInfo}" +
                                            $"{Constants.HALF_WIDTH_SPACE}-U{Constants.HALF_WIDTH_SPACE}{username}" +
                                            $"{Constants.HALF_WIDTH_SPACE}-F{Constants.HALF_WIDTH_SPACE}c" +
                                            $"{Constants.HALF_WIDTH_SPACE}-f{Constants.HALF_WIDTH_SPACE}\"{tempDumpFile}\""
                            };

                            // 命令実行
                            using (var process = new Process { StartInfo = processStartInfo })
                            {
                                process.Start();
                                // 処理流れの記録を取得する
                                string stdout = ReadStream(process.StandardOutput);
                                // 処理エラーの記録を取得する
                                string stderr = ReadStream(process.StandardError);

                                // 処理を完了まで待ち
                                process.WaitForExit();

                                // コンソールに処理流れを記入する。
                                Console.WriteLine(stdout);
                                // エラーがある場合
                                if (!string.IsNullOrEmpty(stderr))
                                {
                                    Console.WriteLine(stderr);
                                    logger.Error(stderr);
                                }

                                // 処理失敗の場合
                                var exitCode = process.ExitCode;
                                if (exitCode != 0)
                                {
                                    // エラーログに、「ダンプファイル出力が失敗しました。バックアップ対象情報を確認してください。」と出力して、次のループを開始する。
                                    var error = "ダンプファイル出力が失敗しました。バックアップ対象情報を確認してください。（都道府県コード：" + backupInfo.TodofukenCd + "）";
                                    Console.Error.WriteLine(error);
                                    logger.Error(error);
                                    // （１）以下のフォルダを中身のファイルごと削除する。	
                                    // 定数：TempBackUpFolder\「バックアップ対象情報」.都道府県コード
                                    DeleteFolderFiles((new DirectoryInfo(tempDumpFile)).Parent.FullName);
                                    continue;
                                }

                                var success = "ダンプファイル出力が成功しました。（都道府県コード：" + backupInfo.TodofukenCd + "）";
                                Console.WriteLine(success);
                                logger.Info(success);
                            }
                        }
                        catch (Exception e)
                        {
                            // エラーログに、「ダンプファイル出力が失敗しました。バックアップ対象情報を確認してください。」と出力して、次のループを開始する。
                            var error = "ダンプファイル出力が失敗しました。バックアップ対象情報を確認してください。（都道府県コード：" + backupInfo.TodofukenCd + "）";
                            Console.WriteLine(error);
                            logger.Error(error);
                            logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                            // （１）以下のフォルダを中身のファイルごと削除する。	
                            // 定数：TempBackUpFolder\「バックアップ対象情報」.都道府県コード
                            DeleteFolderFiles((new DirectoryInfo(tempDumpFile)).Parent.FullName);
                            continue;
                        }

                        // ファイル分割、暗号化
                        SplitAndEncryptFile(tempDumpFile, backupInfo.DmpOutputPath);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    // 処理結果（正常：0、エラー：1）
                    Environment.ExitCode = Constants.BATCH_EXECUT_FAILED;
                    return;
                }
            }

            // 処理時間
            stopwatch.Stop();
            var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
            logger.Info(excutingTime);

            // 処理終了のログを出力し、処理を終了する。	
            logger.Info(string.Concat(CoreConst.LOG_END_KEYWORD, " ダンプファイル出力バッチ"));
            Console.WriteLine("ダンプファイル出力バッチ実行完了しました。");
            // 処理結果（正常：0、エラー：1）
            Environment.ExitCode = Constants.BATCH_EXECUT_SUCCESS;
        }

        #region 定時実行予約登録のデータを排他ロックする。
        /// <summary>
        /// 定時実行予約登録のデータを排他ロックする。
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <returns>定時バッチ起動管理情報</returns>
        private static List<TScheBatchRunManage> GetScheBatchRunManageInfo(SystemCommonContext db)
        {
            // sql用定数定義
            StringBuilder sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT");
            sql.Append("    batch_nm ");
            sql.Append("    , batch_run_date ");
            sql.Append("    , system_kbn ");
            sql.Append("    , xmin ");
            sql.Append("FROM");
            sql.Append("    t_sche_batch_run_manage ");
            sql.Append("WHERE");
            sql.Append("    batch_nm = @BatchNm FOR UPDATE");

            parameters.Add(new NpgsqlParameter("@BatchNm", BATCH_NAME));

            logger.Info("システム共通スキーマの定時バッチ起動管理テーブルの定時実行予約登録のデータを排他ロックする。");
            var list = db.Database.SqlQueryRaw<TScheBatchRunManage>(sql.ToString(), parameters.ToArray()).ToList();
            return list;
        }
        #endregion

        #region 定時バッチ起動管理新規登録処理
        /// <summary>
        /// 定時バッチ起動管理新規登録処理
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="sysDate">システム日時</param>
        /// <returns>新規件数</returns>
        private static void InsertTScheBatchRunManage(SystemCommonContext db, DateTime sysDate)
        {
            var model = new TScheBatchRunManage()
            {
                SystemKbn = Constants.SYSTEM_KBN_COMMON,
                BatchNm = BATCH_NAME,
                BatchRunDate = sysDate.Date
            };

            logger.Info("「定時バッチ起動管理新規登録」の処理を実行します。");
            db.TScheBatchRunManages.Add(model);
            db.SaveChanges();
        }
        #endregion

        #region 定時バッチ起動管理更新登録処理
        /// <summary>
        /// 定時バッチ起動管理更新登録処理
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="sysDate">システム日時</param>
        /// <returns>更新件数</returns>
        private static int UpdateTScheBatchRunManage(SystemCommonContext db, DateTime sysDate)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("UPDATE t_sche_batch_run_manage ");
            sql.Append("SET");
            sql.Append("    batch_run_date = @BatchRunDate ");
            sql.Append("WHERE");
            sql.Append("    system_kbn = @SystemKbn ");
            sql.Append("    AND batch_nm = @BatchNm ");
            sql.Append("    AND batch_run_date <> @BatchRunDate");

            parameters.Add(new NpgsqlParameter("@BatchRunDate", sysDate.Date));
            parameters.Add(new NpgsqlParameter("@SystemKbn", Constants.SYSTEM_KBN_COMMON));
            parameters.Add(new NpgsqlParameter("@BatchNm", BATCH_NAME));

            logger.Info("「定時バッチ起動管理更新登録」の処理を実行します。");
            return db.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());
        }
        #endregion

        #region ダンプファイル分割、暗号化、移動
        /// <summary>
        /// ダンプファイル分割、暗号化、移動
        /// </summary>
        /// <param name="dumpFilePath">ダンプファイルパス</param>
        /// <param name="outputFolderPath">移動先のダンプファイルフォルダ</param>
        private static void SplitAndEncryptFile(string dumpFilePath, string outputFolderPath)
        {
            // dumpファイル出力成功した場合
            // ダンプファイル分割
            var partSizeBytes = ConfigUtil.GetLong(Constants.CONFIG_DUMP_FILE_DIV_MB) * 1024 * 1024;
            var partFilePaths = new List<string>();

            FileInfo dumpfileInfo = new FileInfo(dumpFilePath);
            // ダンプファイルのサイズは分割サイズを越えない場合
            if (dumpfileInfo.Length <= partSizeBytes)
            {
                byte[] fileBytes;
                using (FileStream fileStream = File.OpenRead(dumpFilePath))
                {
                    fileBytes = new byte[fileStream.Length];
                    fileStream.Read(fileBytes, 0, fileBytes.Length);
                }
                // 暗号化する
                var encryptedData = CryptoUtil.Encrypt(fileBytes, Path.GetFileName(dumpFilePath));
                // 暗号化したファイルデータを保存する
                using (FileStream partStream = File.Create(dumpFilePath))
                {
                    partStream.Write(encryptedData, 0, encryptedData.Length);
                }

                partFilePaths.Add(dumpFilePath);
            }
            else
            {
                // ダンプファイルを分割する必要な場合
                using (FileStream fileStream = File.OpenRead(dumpFilePath))
                {
                    byte[] buffer = new byte[partSizeBytes];
                    int bytesRead;
                    int partNumber = 1;

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // ダンプファイルから分割サイズのByteを読む
                        byte[] actualBuffer = buffer[..bytesRead];
                        // 分割ファイル名を設定する。
                        var partFileName = GetPartFileName(dumpFilePath, partNumber);

                        // ダンプファイル暗号化
                        var fileEncryptData = CryptoUtil.Encrypt(actualBuffer, Path.GetFileName(partFileName));
                        // 暗号化したファイルデータを保存する
                        using (FileStream partStream = File.Create(partFileName))
                        {
                            partStream.Write(fileEncryptData, 0, fileEncryptData.Length);
                        }

                        partFilePaths.Add(partFileName);
                        Console.WriteLine($"分割ファイル {partNumber}: {partFileName}");

                        // 最後まで
                        if (bytesRead < buffer.Length) break;

                        partNumber++;
                    }
                }
            }

            // 「バックアップ対象情報」.ダンプ出力先パスに移動する。
            // 移動先フォルダが存在しない場合、作成する
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            // ファイル移動
            foreach (var item in partFilePaths)
            {
                FileInfo fileInfo = new FileInfo(item);
                var filePathNew = Path.Combine(outputFolderPath, Path.GetFileName(item));
                // 移動先にファイルが既に存在する場合、削除する。
                if (File.Exists(filePathNew))
                {
                    File.Delete(filePathNew);
                }
                // ファイル移動する
                fileInfo.MoveTo(filePathNew);
            }

            // （１）以下のフォルダを中身のファイルごと削除する。	
            // 定数：TempBackUpFolder\「バックアップ対象情報」.都道府県コード
            DeleteFolderFiles((new DirectoryInfo(dumpFilePath)).Parent.FullName);
        }
        #endregion

        #region フォルダ内のファイルを削除する処理
        /// <summary>
        /// フォルダ内のファイルを削除する。
        /// </summary>
        /// <param name="folderPath">削除したいファイルのフォルダ</param>
        /// <returns></returns>
        private static void DeleteFolderFiles(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                // フォルダにあるファイルを削除する。
                var files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string filePath in files)
                {
                    File.Delete(filePath);
                }

                // フォルダにあるフォルダを削除する。
                var folders = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
                foreach (string dirPath in folders)
                {
                    Directory.Delete(dirPath, true);
                }
            }
        }
        #endregion

        #region ストリームからテキストを読み取
        /// <summary>
        /// ストリームからテキストを読み取ります。
        /// </summary>
        /// <param name="streamReader">読み取り対象の StreamReader オブジェクト。</param>
        /// <returns>読み取ったテキストを含む文字列。</returns>
        private static string ReadStream(StreamReader streamReader)
        {
            using (streamReader)
            {
                // ストリーム全体を文字列として読み取ります。
                return streamReader.ReadToEnd();
            }
        }
        #endregion

        #region ダンプファイル分割ファイル名作成
        /// <summary>
        /// ダンプファイル分割後のファイル名を作成する
        /// </summary>
        /// <param name="originalDumpFilePath">ダンプファイルパス</param>
        /// <param name="partNumber">分割No</param>
        /// <returns>ファイル名</returns>
        static string GetPartFileName(string originalDumpFilePath, int partNumber)
        {
            string directoryName = Path.GetDirectoryName(originalDumpFilePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalDumpFilePath);
            string fileExtension = Path.GetExtension(originalDumpFilePath);

            string partNumberStr = partNumber.ToString("D3");

            // 分割したファイルは末尾に.001、.002・・・の連番を付与する。
            // 例え、02.dmp.001、02.dmp.002
            return Path.Combine(directoryName, $"{fileNameWithoutExtension}{fileExtension}.{partNumberStr}");
        }
        #endregion
    }
}
