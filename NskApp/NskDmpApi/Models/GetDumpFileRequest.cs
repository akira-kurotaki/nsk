using BaseDmpApi.Base;
using System.Runtime.Serialization;

namespace BaseDmpApi.Models
{
    /// <summary>
    /// ダンプファイル取得APIのリクエストパラメータ.連携データ部
    /// </summary>
    public class GetDumpFileRequest : ApiRequestBase
    {
        /// <summary>
        /// トークン
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
