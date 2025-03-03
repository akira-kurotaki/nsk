using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// メッセージマスタのユーティリティクラス
    /// システム共通向け
    /// </summary>
    public static class SystemMessageUtil
    {
        /// <summary>
        /// メッセージを取得するメソッド
        /// </summary>
        /// <param name="messageId">メッセージID</param>
        /// <returns>メッセージ</returns>
        public static string Get(string messageId)
        {
            MSystemMessageCache mMessageCache = new MSystemMessageCache(CacheManager.GetInstance());
            MMessage message = CacheUtil.Get<IEnumerable<MMessage>>(CacheManager.GetInstance()
                                        , CoreConst.M_SYSTEM_MESSAGE_CACHE, () => (IEnumerable<MMessage>)mMessageCache.GetList())
                                        .SingleOrDefault(a => a.MessageId == messageId);
            // 例外処理
            if (message == null)
            {
                // MF00006：メッセージが見つかりません。(メッセージID = "{0}")。システム管理者に連絡してください。
                // throw new ArgumentException(Get("MF00006", (messageId == null) ? "null" : messageId));
                throw new ArgumentException(string.Format(
                    "メッセージが見つかりません。(メッセージID = \"{0}\")。システム管理者に連絡してください。(MF00006)", messageId));
            }

            string msg = message.Message;
            if (messageId.StartsWith("ME") || messageId.StartsWith("MI") || messageId.StartsWith("MW") || messageId.StartsWith("MF"))
            {
                msg += "(" + messageId + ")";
            }
            return msg;

        }

        /// <summary>
        /// メッセージを取得し、書式指定項目を文字列で置換するメソッド
        /// </summary>
        /// <param name="messageId">メッセージID</param>
        /// <param name="arg">置換用文字列の配列(可変長)</param>
        /// <returns>置換済みメッセージ</returns>
        public static string Get(string messageId, params string[] arg)
        {
            // argの数が{0},{1}...より少ないと、例外が発生する。
            return String.Format(Get(messageId), arg);
        }

        /// <summary>
        /// メッセージを取得するメソッド（メッセージIDを出力しない）
        /// </summary>
        /// <param name="messageId">メッセージID</param>
        /// <returns>メッセージ</returns>
        public static string GetNoAfterID(string messageId)
        {
            MSystemMessageCache mMessageCache = new MSystemMessageCache(CacheManager.GetInstance());
            MMessage message = CacheUtil.Get<IEnumerable<MMessage>>(CacheManager.GetInstance()
                                        , CoreConst.M_SYSTEM_MESSAGE_CACHE, () => (IEnumerable<MMessage>)mMessageCache.GetList())
                                        .SingleOrDefault(a => a.MessageId == messageId);
            // 例外処理
            if (message == null)
            {
                throw new ArgumentException(string.Format(
                    "メッセージが見つかりません。(メッセージID = \"{0}\")。システム管理者に連絡してください。(MF00006)", messageId));
            }

            return message.Message;
        }

        /// <summary>
        /// メッセージを取得し、書式指定項目を文字列で置換するメソッド（メッセージIDを出力しない）
        /// </summary>
        /// <param name="messageId">メッセージID</param>
        /// <param name="arg">置換用文字列の配列(可変長)</param>
        /// <returns>置換済みメッセージ</returns>
        public static string GetNoAfterID(string messageId, params string[] arg)
        {
            // argの数が{0},{1}...より少ないと、例外が発生する。
            return String.Format(GetNoAfterID(messageId), arg);
        }

        /// <summary>
        /// メッセージマスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MSystemMessageCache mMessageCache = new MSystemMessageCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MMessage>>(CacheManager.GetInstance(), CoreConst.M_SYSTEM_MESSAGE_CACHE, () => (IEnumerable<MMessage>)mMessageCache.GetList());
        }

        /// <summary>
        /// エラー用メッセージを取得する
        /// </summary>
        /// <param name="exception">例外オブジェクト</param>
        /// <param name="maxInnerCount">内部例外の取得最大深度(0:内部例外の取得数0)</param>
        /// <returns>エラー用メッセージ</returns>
        public static string GetErrorMessage(Exception exception, int maxInnerCount)
        {
            string retMessage = string.Format("{0}({1}){2}", exception.GetType().Name, exception.Message, Environment.NewLine + exception.StackTrace);
            if (null == exception.InnerException || 0 >= maxInnerCount)
            {
                return retMessage;
            }
            return retMessage + Environment.NewLine + GetErrorMessage(exception.InnerException, maxInnerCount - 1);
        }
    }
}