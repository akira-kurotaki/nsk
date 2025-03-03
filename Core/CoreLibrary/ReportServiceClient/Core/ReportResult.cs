using System.Runtime.Serialization;

namespace ReportService.Core
{
    /// <summary>
    /// リアル帳票出力サービスの実行結果
    /// </summary>
    [DataContract]
    public class ReportResult
    {
        /// <summary>
        ///  処理結果（正常：0、エラー：1）
        /// </summary>
        private int result = 0;

        /// <summary>
        /// エラーメッセージId
        /// </summary>
        public string errorMessageId = string.Empty;

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        private string errorMessage = string.Empty;

        /// <summary>
        /// 帳票データ
        /// </summary>
        private byte[] reportData = { };

        [DataMember]
        public int Result
        {
            get { return result; }
            set { result = value; }
        }

        [DataMember]
        public string ErrorMessageId
        {
            get { return errorMessageId; }
            set { errorMessageId = value; }
        }

        [DataMember]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        [DataMember]
        public byte[] ReportData
        {
            get { return reportData; }
            set { reportData = value; }
        }
    }
}
