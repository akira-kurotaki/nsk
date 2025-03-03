using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Extensions;
using NLog;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// セッション管理ユーティリティ
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/20
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public static class SessionUtil
    {
        /// <summary>
        /// セッションオブジェクト許容サイズ(単位：Byte)
        /// </summary>
        private static readonly long sessionObjectMaxSize = Convert.ToInt64(ConfigUtil.Get("SessionObjectMaxSize"));

        /// <summary>
        /// セッションにオブジェクトを設定する。
        /// </summary>
        /// <param name="key">セッションキー(画面ID_キー名)</param>
        /// <param name="value">オブジェクト</param>
        public static void Set(string key, object value, HttpContext context)
        {
            string idRegex = @"^_?[D][0-9]{1,8}_.*$";

            if (Regex.IsMatch(key, idRegex))
            {
                long objectSize = GetObjectSize(value);
                if (objectSize > sessionObjectMaxSize)
                {
                    // メッセージ：セッションに最大値を超えるサイズ（{0}(byte)）の
                    // データが追加されました。（キー:{1} ,サイズ:{2}(byte)）
                    LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Warn, SystemMessageUtil.Get(
                        "MW00005", sessionObjectMaxSize.ToString(), key, objectSize.ToString()));
                }
            }
            else
            {
                // セッションキーが画面IDで始まらない場合は例外をスローする。
                // メッセージ：セッションキーの命名規約が不正です。
                throw new ArgumentException(SystemMessageUtil.Get("MF00003", key));
            }
            SetObject(key, value, context);

            // セッションキーはHashSetで管理し、セッションに積んでおく。
            // セッションに大きなオブジェクトが積まれた時、Session.Keysのようにセッション全体にアクセスする
            // 操作を行うと時間がかかるため。
            if (!(Get<HashSet<string>>(CoreConst.SESS_KEYS, context) is HashSet<string> sessionKeys))
            {
                sessionKeys = new HashSet<string>();
            }
            sessionKeys.Add(key);

            SetObject(CoreConst.SESS_KEYS, sessionKeys, context);
        }

        /// <summary>
        /// セッションからオブジェクトを取得する。
        /// Exceptionのシリアライズはできないため、System.Dynamic.ExpandoObjectとして取得する。暫定対応★
        /// </summary>
        /// <param name="key">セッションキー(画面ID_キー名)</param>
        /// <returns></returns>
        public static T Get<T>(string key, HttpContext context)
        {
            var value = context.Session.Get(key);
            if (value == null)
            {
                return default;
            }

            var readOnlySpan = new ReadOnlySpan<byte>(value);
            return JsonSerializer.Deserialize<T>(readOnlySpan);
        }

        /// <summary>
        /// セッションからオブジェクトを削除する。
        /// </summary>
        /// <param name="key">セッションキー(画面ID_キー名)</param>
        public static void Remove(string key, HttpContext context)
        {
            context.Session.Remove(key);

            // セッションキーを管理するHashSetからも削除する。
            if (Get<HashSet<string>>(CoreConst.SESS_KEYS, context) is HashSet<string> sessionKeys)
            {
                sessionKeys.Remove(key);
                SetObject(CoreConst.SESS_KEYS, sessionKeys, context);
            }
        }

        /// <summary>
        /// キー名が"_"で始まる値以外を削除する。
        /// </summary>
        public static void RemoveAll(HttpContext context)
        {
            // セッションキーはHashSetで管理されている。
            if (Get<HashSet<string>>(CoreConst.SESS_KEYS, context) is HashSet<string> sessionKeys)
            {
                List<string> keyLists = sessionKeys.ToList();
                foreach (string key in keyLists)
                {
                    if (!key.StartsWith("_"))
                    {
                        context.Session.Remove(key);
                        sessionKeys.Remove(key);
                    }
                }
                SetObject(CoreConst.SESS_KEYS, sessionKeys, context);
            }
        }

        /// <summary>
        /// 対象データのサイズを取得する。
        /// </summary>
        /// <param name="objGraph">オブジェクト</param>
        /// <returns>オブジェクトサイズ</returns>
        private static long GetObjectSize(object objGraph)
        {
            long objSize = 0;
            if (objGraph is Exception)
            {
                // Exceptionのシリアライズはできないため、プロパティを抜き出しJSON化し、シリアライズする。
                JsonSerializerOptions options = new(JsonSerializerOptions.Default);
                options.Converters.Add(new ExceptionJsonConverter());
                var json = JsonSerializer.Serialize((Exception)objGraph, options);
                objSize = Encoding.UTF8.GetBytes(json).Length;
            }
            else
            {
                var json = JsonSerializer.SerializeToUtf8Bytes(objGraph);
                objSize = json.Length;
            }

            return objSize;
        }

        /// <summary>
        /// セッションにオブジェクトを設定する。
        /// Exceptionのシリアライズはできないため、プロパティを抜き出しJSON化し、シリアライズする。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="context"></param>
        public static void SetObject(string key, object obj, HttpContext context)
        {
            if (obj is Exception)
            {
                // Exceptionのシリアライズはできないため、プロパティを抜き出しJSON化し、シリアライズする。
                JsonSerializerOptions options = new(JsonSerializerOptions.Default);
                options.Converters.Add(new ExceptionJsonConverter());
                context.Session.SetString(key, JsonSerializer.Serialize((Exception)obj, options));
            }
            else
            {
                context.Session.Set(key, JsonSerializer.SerializeToUtf8Bytes(obj));
            }
        }
    }
}