using CoreLibrary.Core.Consts;
using Microsoft.VisualBasic.FileIO;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// フォルダ関連ユーティリティ
    /// </summary>
    public static class FolderUtil
    {
        /// <summary>
        /// CSVの一時出力フォルダを作成
        /// </summary>
        /// <param name="sysDateTime">システム日時</param>
        /// <param name="zipFileNm">zipファイル名</param>
        /// <returns>作成されたCSV一時出力フォルダパス</returns>
        public static string CreateCsvTempFolder(DateTime sysDateTime, string zipFileNm)
        {
            // 定数（設定ファイル）：CSV一時出力フォルダ\yyyyMMdd\HHmmss\（GUIDを生成したフォルダ名）\zipファイル名（引数）の拡張子以外
            var tempFolder = Path.Combine(
                ConfigUtil.Get(CoreConst.CSV_TEMP_FOLDER),
                sysDateTime.ToString("yyyyMMdd"),
                sysDateTime.ToString("HHmmss"),
                System.Guid.NewGuid().ToString(),
                Path.GetFileNameWithoutExtension(zipFileNm));

            FileSystem.CreateDirectory(tempFolder);

            return tempFolder;
        }

        /// <summary>
        /// 帳票一時出力フォルダを作成
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="filePath">帳票パス</param>
        /// <returns>作成された帳票一時出力フォルダパス</returns>
        public static string CreatePrintTempFolder(string userId, string filePath)
        {
            var tempFolder = Path.Combine(
                ConfigUtil.Get(CoreConst.PRINT_TEMP_FOLDER_TAG_NAME),
                userId + "_" + System.Guid.NewGuid().ToString("N"),
                Path.GetFileNameWithoutExtension(filePath));

            FileSystem.CreateDirectory(tempFolder);

            return tempFolder;
        }

        /// <summary>
        /// ファイル移動
        /// ・ZIPファイルパスリスト内のファイルを移動先フォルダに移動する
        /// ・移動先のファイルパスをバッチダウンロードファイルに登録する
        /// </summary>
        /// <param name="zipFilePath">ZIPファイルパスリスト(キー：ファイルパス、value:暗号化前のzipファイルのハッシュ)</param>
        /// <param name="destDirPath">移動先フォルダパス</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="batchId">バッチID</param>
        public static void MoveFile(Dictionary<string, string> zipFilePath, string destDirPath, string userId, long batchId)
        {
            // 移動先フォルダが存在しない場合、作成する
            if (!Directory.Exists(destDirPath))
            {
                FileSystem.CreateDirectory(destDirPath);
            }

            // ファイル移動
            foreach (var item in zipFilePath)
            {
                FileInfo zipFileInfo = new FileInfo(item.Key);
                var zipFilePathNew = Path.Combine(destDirPath, zipFileInfo.Name);

                // 移動先のファイルがすでに存在する場合、削除する
                if (File.Exists(zipFilePathNew))
                {
                    File.Delete(zipFilePathNew);
                }

                // 新しいパスにファイルを移動する
                zipFileInfo.MoveTo(zipFilePathNew);

                // バッチダウンロードファイル登録
                try
                {
                    var message = string.Empty;
                    var reult = BatchUtil.InsertBatchDownloadFile(batchId, zipFilePathNew, item.Value, zipFileInfo.Name, userId, ref message);
                    if (reult == 0)
                    {
                        if (File.Exists(zipFilePathNew))
                        {
                            File.Delete(zipFilePathNew);
                        }
                        throw new ApplicationException("バッチダウンロードファイルの登録に失敗しました。");
                    }
                }
                catch (Exception)
                {
                    if (File.Exists(zipFilePathNew))
                    {
                        File.Delete(zipFilePathNew);
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// 一時出力フォルダを削除する
        /// </summary>
        /// <param name="tempFolder">一時出力フォルダパス</param>
        public static void DeleteTempFolder(string tempFolder)
        {
            Directory.Delete((new DirectoryInfo(tempFolder)).Parent.FullName, true);
        }
    }
}
