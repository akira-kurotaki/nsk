using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;

namespace NskCommonLibrary.Core.Utility
{
    /// <summary>
    /// スローユーティリティクラス
    /// 
    /// 作成日：2025/02/14
    /// </summary>
    public static class ThrowUtil 
    {
        /// <summary>
        /// AppExceptionをスローする
        /// </summary>
        /// <param name="messageId">メッセージID</param>
        /// <param name="arg">置換用文字列の配列(可変長)</param>
        /// <exception cref="AppException">AppException</exception>
        public static void Throw(string messageId, params string[] arg) =>
            throw new AppException(messageId, MessageUtil.Get(messageId, arg));

        /// <summary>
        /// AppExceptionをスローする
        /// </summary>
        /// <param name="messageId">メッセージ</param>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        public static void ThrowMessage(string message) =>
            throw new ArgumentException(message);

    }
}