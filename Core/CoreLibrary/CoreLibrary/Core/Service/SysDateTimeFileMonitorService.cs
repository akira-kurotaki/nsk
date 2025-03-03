using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Utility;
using NLog;

namespace CoreLibrary.Core.Service
{
    /// <summary>
    /// システム時間設定ファイル監視機能
    /// </summary>
    public class SysDateTimeFileMonitorService : BackgroundService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private FileSystemWatcher _fileSystemWatcher;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SysDateTimeFileMonitorService()
        {
        }

        /// <summary>
        /// アプリケーション ホストでサービスを開始する準備ができたときに呼び出される
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var filePath = ConfigUtil.Get(CoreConst.SYS_DATE_TIME_PATH);

            if (Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                _fileSystemWatcher = new FileSystemWatcher
                {
                    Filter = Path.GetFileName(filePath),
                    Path = Path.GetDirectoryName(filePath),
                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = false
                };
                _fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 対象ファイル変更後イベント
        /// </summary>
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                using (var file = new StreamReader(e.FullPath))
                {
                    DateUtil.SystemDate = DateTime.Parse(file.ReadLine());
                }
            }
            catch (IOException ioe)
            {
                // 下記のエラーを握りつぶす
                // 別のプロセスで使用されているため、プロセスはファイル 'xxx' にアクセスできません。
                if (ioe.Message.Contains("別のプロセスで使用されているため"))
                {
                    ;
                }
                else
                {
                    logger.Error(ioe);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
