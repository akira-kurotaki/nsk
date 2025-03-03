using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Windows.Forms;

namespace NskReportDesignDriver
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);

            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                        loggingBuilder.AddNLog();
                    });

                    services.AddSingleton<Form1>();
                });

            builder.UseNLog();

            using (var host = builder.Build())
            {
                var serviceProvider = host.Services;
                var mainForm = serviceProvider.GetRequiredService<Form1>();

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.Run(mainForm);
            }

            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/
        }
    }
}
