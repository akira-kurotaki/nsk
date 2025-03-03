using BAS_B1001_TempFolderDelete.Common;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Utility;
using NLog;
using System.Diagnostics;
using System.Text;

namespace BAS_B1001_TempFolderDelete
{
    /// <summary>
    /// 一時フォルダ削除本体プログラム
    /// </summary>
    class Program
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 一時フォルダ削除処理
        /// </summary>
        /// <param name="args">
        /// </param>
        static void Main(string[] args)
        {
            //１．処理開始のログを出力する。
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " 一時フォルダ削除処理"));
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //２．定数：temp_folder_listから一時フォルダのパスを取得する。
            string paths = ConfigUtil.Get(Constants.TEMP_FOLDER_LIST);
            if (!string.IsNullOrEmpty(paths))
            {
                string[] TempFolders = paths.Split(Constants.DELIMITER_SEMICOLON);
                // ３．「２．」で取得したフォルダの配下にあるフォルダについて最終更新日時が1週間以上前のフォルダを削除する。
                foreach (var folder in TempFolders)
                {
                    try
                    {
                        CleanOldFolders(folder);
                    }
                    catch (Exception ex)
                    {
                        //削除失敗した場合は、エラー内容をログに出力して、次の一時フォルダを処理する。
                        logger.Error("一時フォルダ削除処理失敗");
                        logger.Error(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));
                        Console.Error.WriteLine(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    }
                }
            }

            // 処理時間
            stopwatch.Stop();
            var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + CoreConst.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + CoreConst.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
            logger.Info(excutingTime);

            //４．処理終了のログを出力し、処理を終了する。	
            logger.Info(string.Concat(CoreConst.LOG_END_KEYWORD, " 一時フォルダ削除処理"));

            // 処理結果（正常：0、エラー：1）
            Environment.ExitCode = Constants.BATCH_EXECUT_SUCCESS;
        }

        /// <summary>
        /// 最終更新日時が1週間以上前のフォルダを削除する
        /// </summary>
        /// <param name="rootFolder">フォルダ</param>
        private static void CleanOldFolders(string rootFolder)
        {
            if (Directory.Exists(rootFolder))
            {
                var oneWeekAgo = DateTime.Now.AddDays(-7);

                // フォルダの配下にあるフォルダ
                var subFolders = Directory.GetDirectories(rootFolder);

                foreach (var subFolder in subFolders)
                {
                    // 最終更新日時
                    var lastWriteTime = Directory.GetLastWriteTime(subFolder);

                    // 最終更新日時が1週間以上前のフォルダを削除する
                    if (lastWriteTime <= oneWeekAgo)
                    {
                        try
                        {
                            Directory.Delete(subFolder, true);
                            logger.Debug(string.Concat("フォルダ削除: ", subFolder));
                        }
                        catch (Exception ex)
                        {
                            logger.Error("一時フォルダ削除処理失敗");
                            logger.Error(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));
                            Console.Error.WriteLine(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));
                        }
                    }
                }
            }

        }

    }
}