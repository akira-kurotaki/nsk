using System.Runtime.Serialization;

namespace BaseDmpApi.Base
{
    /// <summary>
    /// データ取得機能共通用レスポンス
    /// </summary>
    [DataContract]
    public class ApiResponseBase : IApiResponse
    {
        /// <summary>
        /// 残件数
        /// </summary>
        [DataMember(Name = "kensu")]
        public long kensu { get; set; }

        /// <summary>
        /// トークン
        /// </summary>
        [DataMember(Name = "token")]
        public string token { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        [DataMember(Name = "messages")]
        public List<Message> messages { get; set; }
    }
}
