using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NLog;
using System.Text.RegularExpressions;

namespace ModelLibrary.Context
{
    public abstract class ContextBase : DbContext
    {
        private const string SQL_COMMAND_INSERT = "INSERT ";
        private const string SQL_COMMAND_UPDATE = "UPDATE ";
        private const string SQL_COMMAND_DELETE = "DELETE ";

        /// <summary>
        /// 接続先文字列
        /// </summary>
        public string connectionString;

        /// <summary>
        /// デフォルトスキーマ
        /// </summary>
        public string defaultSchema;

        /// <summary>
        /// ユーザーID
        /// </summary>
        protected string userId = null;

        /// <summary>
        /// IPアドレス
        /// </summary>
        protected string ipAddress = null;

        /// <summary>
        /// ロガー
        /// </summary>
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Npgsql 6.0での「timestamp with time zone」非互換対応
        /// Npgsql 6.0より前の動作に戻す
        /// https://www.npgsql.org/doc/types/datetime.html#timestamps-and-timezones
        /// </summary>
        static ContextBase()
        {
            // Npgsql 6.0での「timestamp with time zone」非互換対応
            // Npgsql 6.0より前の動作に戻す
            // https://www.npgsql.org/doc/types/datetime.html#timestamps-and-timezones
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="defaultSchema">デフォルトスキーマ</param>
        /// <param name="commandTimeout">コンテキスト操作のタイムアウト値(秒)
        /// 値が未指定、または、0未満の値場合、設定ファイルからタイムアウト値を取得する</param>
        public ContextBase(string connectionString, string defaultSchema, int commandTimeout = 0) :
            this(connectionString, defaultSchema, null, null, commandTimeout)
        {
            ;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <param name="defaultSchema">デフォルトスキーマ</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="ipAddress">IPアドレス</param>
        /// <param name="commandTimeout">コンテキスト操作のタイムアウト値(秒)
        /// 値が未指定、または、0未満の値場合、設定ファイルからタイムアウト値を取得する</param>
        public ContextBase(string connectionString, string defaultSchema, string userId, string ipAddress, int commandTimeout = 0)
        {
            this.connectionString = connectionString;
            this.defaultSchema = defaultSchema;
            this.userId = userId;
            this.ipAddress = ipAddress;
            Database.SetCommandTimeout(0 < commandTimeout ? commandTimeout : ConfigUtil.GetInt(CoreConst.COMMAND_TIMEOUT));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // デフォルトスキーマを設定する
            modelBuilder.HasDefaultSchema(defaultSchema);
        }

        #region OnConfiguring
        /// <summary>
        /// ログ出力設定、接続先設定
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .LogTo(message =>
                {
                    // Executed DbCommandをMatchする。
                    var commandTextMatch = Regex.Match(message, @"((Executed|Failed executing) DbCommand \([^\]]*\) \[Parameters=\[.*?\], CommandType='Text', CommandTimeout='\d+'\])([\s\S]+)", RegexOptions.Singleline);
                    if (commandTextMatch.Success)
                    {
                        // Executed DbCommand　[Parameters=～CommandTimeout=]部分を取得する。
                        var originalCommand = commandTextMatch.Groups[1].Value;
                        // SQL本体を取得する。
                        var commandText = commandTextMatch.Groups[3].Value.Trim();

                        // INSERTとUPDATE以外の場合
                        if (!(commandText.StartsWith(SQL_COMMAND_INSERT) || commandText.StartsWith(SQL_COMMAND_INSERT.ToLower()) ||
                        commandText.StartsWith(SQL_COMMAND_UPDATE) || commandText.StartsWith(SQL_COMMAND_UPDATE.ToLower())))
                        {
                            // SQLのパラメータ出力
                            logger.Info(originalCommand);
                        }

                        // SQLの本体出力
                        logger.Debug(commandText + Environment.NewLine);
                    }
                    else
                    {
                        // もしくは、Match失敗する場合、sql情報を全てログに出力する
                        logger.Debug(message);
                    }

                }, new[] { DbLoggerCategory.Database.Name }, Microsoft.Extensions.Logging.LogLevel.Information, DbContextLoggerOptions.None)
                .UseNpgsql(connectionString)
                .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
        }
        #endregion
    }
}