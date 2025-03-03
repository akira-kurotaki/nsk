using System.Runtime.Serialization;

namespace NskApi.Base
{
    /// <summary>
    /// データ連携REST APIサービスのレスポンスの基底クラス
    /// </summary>
    [DataContract]
    public class ApiResponseBase : IApiResponse
    {
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        [DataMember(Name = "messages")]
        //public List<Message> Messages { get; set; }
        public List<Message> messages { get; set; }
    }
}
