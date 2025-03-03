using BaseDmpApi.Base;
using System.Runtime.Serialization;

namespace BaseDmpApi.Models
{
    /// <summary>
    /// ダンプファイル取得APIのレスポンスパラメータ.連携データ部
    /// </summary>
    public class GetDumpFileResponse : ApiResponseBase
    {
        /// <summary>
        /// ダンプファイルのデータ
        /// </summary>
        [DataMember(Name = "dmpfile")]
        public byte[] dmpfile { get; set; }
    }
}
