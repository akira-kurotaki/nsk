using System.Runtime.Serialization;

namespace BaseDmpApi.Base
{
    /// <summary>
    /// レスポンス：メッセージリスト
    /// </summary>
    [Serializable]
    [DataContract]
    public class Message
    {
        /// <summary>
        /// メッセージ内容
        /// </summary>
        [DataMember(Name = "message")]
        public string message { get; set; }
    }
}
