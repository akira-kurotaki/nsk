using CoreLibrary.Core.Consts;
using NLog;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// Zip関連ユーティリティ
    /// </summary>
    public static class ZipUtil
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 指定したフォルダをZip化する
        /// </summary>
        /// <param name="path">zip化されるフォルダのパス</param>
        /// <param name="destination">zip アーカイブを格納するストリーム</param>
        public static void CreateZip(string path, Stream destination)
        {
            // ZIP書庫を作成する
            ZipFile.CreateFromDirectory(
                path,
                destination,
                CompressionLevel.Fastest,
                false, Encoding.GetEncoding("Shift_JIS"));
        }

        /// <summary>
        /// 指定したフォルダをZipし、暗号化する
        /// </summary>
        /// <param name="tempFolder">一時出力フォルダパス</param>
        /// <returns>key：zipファイルパス、value：zipファイル暗号化前のhash値</returns>
        public static Dictionary<string, string> CreateZip(string tempFolder)
        {
            // 処理時間計測用ストップウォッチ
            var stopwatch = new Stopwatch();

            // ZIP書庫のMAXサイズ
            var zipMaxSize = long.Parse(ConfigUtil.Get(CoreConst.ZIP_MAX_SIZE_TAG_NAME)) * 1024 * 1024;

            // ZIPファイルパス
            var zipFilePath = tempFolder + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmssfff") + ".zip";

            // ■ZIP書庫作成
            logger.Info("ZIP書庫作成開始");
            stopwatch.Start();

            var fileCountSize = 0L;
            DirectoryInfo di = new DirectoryInfo(tempFolder);
            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);
            List<string> archiveFile = new List<string>();
            HashSet<string> archiveDirectory = new HashSet<string>();

            for (var i = 0; i < files.Length; i++)
            {
                archiveFile = new List<string>();

                //読み取りと書き込みができるようにして、ZIP書庫を開く
                using (ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Update, Encoding.GetEncoding("Shift_JIS")))
                {
                    for (var j = i; j < files.Length; j++)
                    {
                        i = j;

                        // ZIPに追加する
                        if (di.FullName.Equals(files[j].Directory.FullName))
                        {
                            zip.CreateEntryFromFile(files[j].FullName, files[j].Name, CompressionLevel.Fastest);
                        }
                        else
                        {
                            zip.CreateEntryFromFile(files[j].FullName, Path.Combine(files[j].Directory.Name, files[j].Name), CompressionLevel.Fastest);
                            archiveDirectory.Add(files[j].DirectoryName);
                        }

                        // ファイルサイズを加算する
                        fileCountSize += files[j].Length;

                        archiveFile.Add(files[j].FullName);

                        // 次のファイルが存在する場合
                        if (j + 1 < files.Length)
                        {
                            // ZIPファイル中のファイルサイズ + 次のファイルのサイズがZIP書庫のMAXサイズにオーバーしたかどうか 
                            if (zipMaxSize < fileCountSize + files[j + 1].Length)
                            {
                                zipFilePath = tempFolder + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmssfff") + ".zip";
                                fileCountSize = 0L;
                                break;
                            }
                        }
                    }
                }

                // ZIPファイル作成済のファイルを削除する
                foreach (var item in archiveFile)
                {
                    File.Delete(item);
                }
            }
            foreach (var item in archiveDirectory)
            {
                Directory.Delete(item);
            }

            // 処理時間
            stopwatch.Stop();
            logger.Info("ZIP書庫作成終了：" + stopwatch.ElapsedMilliseconds.ToString());

            // ■ZIP書庫をリネームする
            logger.Info("ZIP書庫リネーム開始");
            stopwatch.Restart();
            DirectoryInfo zipDi = new DirectoryInfo(tempFolder).Parent;
            FileInfo[] zipFiles = zipDi.GetFiles("*.zip", SearchOption.TopDirectoryOnly);

            if (zipFiles.Length > 1)
            {
                for (var i = 0; i < zipFiles.Length; i++)
                {
                    zipFilePath = string.Format("{0}_{1:0000}.zip", tempFolder, i + 1);
                    File.Move(zipFiles[i].FullName, zipFilePath);
                }
            }
            else
            {
                zipFilePath = tempFolder + ".zip";
                File.Move(zipFiles[0].FullName, zipFilePath);
            }
            // 処理時間
            stopwatch.Stop();
            logger.Info("ZIP書庫リネーム終了：" + stopwatch.ElapsedMilliseconds.ToString());

            // ■ZIP書庫を暗号化する
            logger.Info("ZIP書庫を暗号化する開始");
            stopwatch.Restart();

            Dictionary<string, string> zipFilePaths = new Dictionary<string, string>();
            FileInfo[] encryptZipFiles = zipDi.GetFiles("*.zip", SearchOption.TopDirectoryOnly);

            foreach (var item in encryptZipFiles)
            {
                byte[] fileData = null;
                var hash = string.Empty;

                using (var fileStream = new FileStream(item.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] data = new byte[fileStream.Length];
                    fileStream.Read(data, 0, data.Length);

                    // 暗号化前のファイルのハッシュ値を取得する。
                    hash = CryptoUtil.GetSHA256Hex(data);

                    // ファイル暗号化する。
                    fileData = CryptoUtil.Encrypt(data, Path.GetFileName(item.FullName));
                }

                using (var fileStreamWrite = new FileStream(item.FullName, FileMode.Create, FileAccess.Write))
                {
                    fileStreamWrite.Write(fileData, 0, fileData.Length);
                }

                // キーをzipファイルパス、valueはzipファイル暗号化前のhash値
                zipFilePaths.Add(item.FullName, hash);
            }
            // 処理時間
            stopwatch.Stop();
            logger.Info("ZIP書庫を暗号化する終了：" + stopwatch.ElapsedMilliseconds.ToString());

            return zipFilePaths;
        }
    }
}
