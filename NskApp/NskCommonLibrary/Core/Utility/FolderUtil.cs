using CoreLibrary.Core.Utility;
using NskCommonLibrary.Core.Consts;
using Microsoft.VisualBasic.FileIO;

namespace NskCommonLibrary.Core.Utility
{
    /// <summary>
    /// フォルダ関連ユーティリティ
    /// </summary>
    public static class FolderUtil
    {

        /// <summary>
        /// 一時出力フォルダを作成
        /// </summary>
        /// <param name="sysDateTime">システム日時</param>
        /// <param name="batchID">バッチID</param>
        /// <returns>作成された一時出力フォルダパス</returns>
        public static string CreateTempFolder(DateTime sysDateTime, string batchID)
        {
            // 定数（設定ファイル）：一時出力フォルダ\バッチID_yyyyMMddHHmmss
            var tempFolder = Path.Combine(ConfigUtil.Get(CoreConst.FILE_TEMP_FOLDER_PATH), batchID + CoreConst.SYMBOL_UNDERSCORE + sysDateTime.ToString("yyyyMMddHHmmss"));

            FileSystem.CreateDirectory(tempFolder);

            return tempFolder;
        }

        /// <summary>
        /// ファイル移動
        /// </summary>
        /// <param name="zipFilePath">ZIPファイルパスリスト(キー：ファイルパス、value:暗号化前のzipファイルのハッシュ)</param>
        /// <param name="filePath">移動先ファイルパス</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="batchId">バッチID</param>
        public static void MoveFile(Dictionary<string, string> zipFilePath, string filePath, string userId, long batchId)
        {
            // 移動先フォルダが存在しない場合、作成する
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            // ファイル移動
            foreach (var item in zipFilePath)
            {
                FileInfo zipFileInfo = new FileInfo(item.Key);
                var zipFilePathNew = Path.Combine(filePath, zipFileInfo.Name);

                if (File.Exists(zipFilePathNew))
                {
                    File.Delete(zipFilePathNew);
                }

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
            Directory.Delete(tempFolder, true);
        }
    }
}
