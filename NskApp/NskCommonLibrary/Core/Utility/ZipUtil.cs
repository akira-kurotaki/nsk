using CoreLibrary.Core.Utility;
using System.Text;

namespace NskCommonLibrary.Core.Utility
{
    /// <summary>
    /// ZIPのユーティリティクラス
    /// 作成日：2025/02/14
    /// </summary>
    public static class ZipUtil
    {
        /// <summary>
        /// ZIPフォルダを作成
        /// </summary>
        /// <param name="bid">バッチID</param>
        /// <returns>作成されたZIPフォルダパス</returns>
        public static string CreateZipFolder(long bid, bool reportflg = false)
        {
            // ZIPファイル格納先パスを作成して変数に設定する
            // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
            string zipFolderPath = Path.Combine(
                ConfigUtil.Get(reportflg ? NskCommonLibrary.Core.Consts.CoreConst.REPORT_OUTPUT_FOLDER : NskCommonLibrary.Core.Consts.CoreConst.CSV_OUTPUT_FOLDER)
                , bid.ToString() + CoreLibrary.Core.Consts.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
            Directory.CreateDirectory(zipFolderPath);
            zipFolderPath += "\\";
            return zipFolderPath;
        }

        /// <summary>
        /// 一時フォルダ→暗号化ZIP→バッチダウンロードファイル登録→一時フォルダ削除
        /// </summary>
        /// <param name="bid">バッチID</param>
        /// <param name="tempFolderPath">一時フォルダ</param>
        /// <param name="zipFolderPath">ZIPフォルダ</param>
        /// <param name="userId">ユーザID</param>
        public static void SaveZip(long bid, string tempFolderPath,string zipFolderPath, string userId) {
            // Zip暗号化を行う。
            // データ一時出力フォルダ内のファイルを共通部品「ZipUtil.CreateZip」でZip化（暗号化）
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Dictionary<string, string> zipFilePath = CoreLibrary.Core.Utility.ZipUtil.CreateZip(tempFolderPath);

            // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
            // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
            // [変数：ZIPファイル格納先パス] とファイル名でパスを登録します。
            FolderUtil.MoveFile(zipFilePath, zipFolderPath, userId, bid);

            // フォルダを削除する。
            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, true);
            }
        }
    }
}
