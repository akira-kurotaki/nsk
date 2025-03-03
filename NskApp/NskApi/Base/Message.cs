using System.Runtime.Serialization;

namespace NskApi.Base
{
    /// <summary>
    /// レスポンス：メッセージリスト
    /// </summary>
    /// <remarks>
    /// 作成日：2022/08/24
    /// 作成者：You En
    /// <remarks>
    [Serializable]
    [DataContract]
    public class Message
    {
        /// <summary>
        /// メッセージ内容
        /// </summary>
        [DataMember(Name = "message")]
        //public string MessageNaiyo { get; set; }
        public string message { get; set; }
    }
}
