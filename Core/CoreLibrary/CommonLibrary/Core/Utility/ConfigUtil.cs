using Microsoft.Extensions.Configuration;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 設定ファイルユーティリティ
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/18
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public static class ConfigUtil
    {
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        private const string FILE_APPSETTINGS = "appsettings.json";

        /// <summary>
        /// セクション名（appsettings）
        /// </summary>
        private const string SECTION_APPSETTINGS = "appsettings";

        /// <summary>
        /// 設定ファイル
        /// </summary>
        private static IConfiguration _configuration;

        /// <summary>
        /// appsettings.jsonのappSettingsから値を取得する。
        /// </summary>
        /// <param name="key">検索キー</param>
        /// <returns>value設定値</returns>
        public static string Get(string key)
        {
            return string.IsNullOrEmpty(key) ? null : GetConfig().GetSection(SECTION_APPSETTINGS)[key];
        }

        /// <summary>
        /// appsettings.jsonのappSettingsから値(int)を取得する。
        /// </summary>
        /// <param name="key">検索キー</param>
        /// <returns>value設定値</returns>
        public static int GetInt(string key)
        {
            return int.Parse(Get(key));
        }

        /// <summary>
        /// appsettings.jsonのappSettingsから値(long)を取得する。
        /// </summary>
        /// <param name="key">検索キー</param>
        /// <returns>value設定値</returns>
        public static long GetLong(string key)
        {
            return long.Parse(Get(key));
        }

        /// <summary>
        /// appsettings.jsonのappSettingsから値(bool)を取得する。
        /// </summary>
        /// <param name="key">検索キー</param>
        /// <returns>value設定値</returns>
        public static bool GetBool(string key)
        {
            return bool.Parse(Get(key));
        }

        /// <summary>
        /// appsettings.jsonのconnectionStringsから値を取得する。
        /// </summary>
        /// <param name="key">検索キー</param>
        /// <returns>value設定値</returns>
        public static string GetConnectionString(string key)
        {
            return GetConfig().GetConnectionString(key);
        }

        /// <summary>
        /// 設定ファイルを取得する
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetConfig()
        {
            if (_configuration == null)
            {
                var builder = new ConfigurationBuilder()
                                .AddJsonFile(FILE_APPSETTINGS, optional: false, reloadOnChange: true)
                                .AddEnvironmentVariables();
                _configuration = builder.Build();
            }
            return _configuration;
        }
    }
}