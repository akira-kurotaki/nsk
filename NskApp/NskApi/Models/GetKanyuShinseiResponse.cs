using NskApi.Base;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;

namespace NskApi.Models
{
    /// <summary>
    /// 加入者情報取得レスポンス
    /// </summary>
    public class GetKanyuShinseiResponse: ApiSelectResponseBase
    {
        /// <summary>
        /// 連携データ
        /// </summary>
        [DataMember(Name = "records")]
        public List<GetKanyuShinseiRecord> records { get; set; }

    }
}
