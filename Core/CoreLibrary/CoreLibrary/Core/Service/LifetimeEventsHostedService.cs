using NLog;

namespace CoreLibrary.Core.Service
{
    public class LifetimeEventsHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        protected Logger logger = LogManager.GetCurrentClassLogger();

        public LifetimeEventsHostedService(
            IHostApplicationLifetime hostApplicationLifetime)
            => _hostApplicationLifetime = hostApplicationLifetime;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            _hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            _hostApplicationLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        /// <summary>
        /// ホストが完全に起動されたときに実行する処理を記述する
        /// </summary>
        private void OnStarted()
        {
            logger.Info("--- Application Start ---");
        }

        /// <summary>
        /// ホストがシャットダウンを行っているときに実行する処理を記述する
        /// </summary>
        private void OnStopping()
        {
            logger.Info("--- Application OnStopping ---");
        }

        /// <summary>
        /// ホストがシャットダウンを完了したときに実行する処理を記述する
        /// </summary>
        private void OnStopped()
        {
            logger.Info("--- Application End --- ");
        }
    }
}