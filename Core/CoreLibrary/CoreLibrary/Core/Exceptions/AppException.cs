namespace CoreLibrary.Core.Exceptions
{
    /// <summary>
    /// AppException
    /// エラーコードを持つ
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/07
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    [Serializable]
    public class AppException : ApplicationException
    {
        // エラーコード
        public string ErrorCode { get; set; }

        // ヘッダーパターンID
        public string HeaderPatternId { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <param name="headerPatternId"></param>
        public AppException(string messageId, string message, string headerPatternId = null) : base(message)
        {
            ErrorCode = messageId;
            HeaderPatternId = headerPatternId;
        }
    }
}