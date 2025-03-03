using NSK_B000000_ManagementService.Common;
using CoreLibrary.Core.Utility;
using System.Diagnostics;

namespace NSK_B000000_ManagementService
{
    /// <summary>
    /// バッチ実行管理プログラム
    /// </summary>
    public class B000000ManagementService : BackgroundService
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger<B000000ManagementService> _logger;

        /// <summary>
        /// バッチ管理プログラムを呼び出すプロセス
        /// </summary>
        private Process process = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger"></param>
        public B000000ManagementService(ILogger<B000000ManagementService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// サービス開始イベント
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(Constants.SERVICE_START_MESSAGE);

            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// サービス終了イベント
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(Constants.SERVICE_STOP_MESSAGE);

                if (process != null)
                {
                    // プロセスを強制終了する
                    process.Kill();

                    // プロセスを破棄する 
                    process.Close();
                    process.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }

            return base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// バッチ管理プログラムの呼び出し
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // プロセス起動情報オブジェクトを作成する
                ProcessStartInfo start_info = new ProcessStartInfo();

                // バッチ管理プログラムの格納先 
                var batchManagementProPath = ConfigUtil.Get(Constants.BATCH_MANAGEMENT_PRO_PATH_TAG_NAME);
                _logger.LogInformation(Constants.BATCH_MANAGEMENT_PRO_LOCATION_MSG + batchManagementProPath);

                // 実行するファイル
                start_info.FileName = batchManagementProPath;

                // コンソールを開かない
                start_info.CreateNoWindow = true;

                // プロセスを起動する
                process = Process.Start(start_info);

                // プロセスが終了したときに Exited イベントを発生させない
                process.EnableRaisingEvents = false;

                // ログ出力する
                _logger.LogInformation(Constants.BATCH_MANAGEMENT_PRO_START_MESSAGE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
