using CoreLibrary.Core.Utility;
using System.Net.Http.Json;

namespace ReportService.Core
{
    /// <summary>
    /// リアルタイム帳票呼び出しクライアント
    /// </summary>
    public class ReportServiceClient
    {
        private readonly HttpClient _httpClient;

        private readonly string baseUrl;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="httpClient"></param>
        public ReportServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // タイムアウト（デフォルトは100秒）
            _httpClient.Timeout = TimeSpan.FromSeconds(ConfigUtil.GetInt("ReportServiceSendTimeout"));

            // レスポンスのバッファーの最大バイト数（デフォルトは2 GB）
            _httpClient.MaxResponseContentBufferSize = ConfigUtil.GetLong("ReportServiceMaxResponseContentBufferSize");

            baseUrl = ConfigUtil.Get("ReportServiceUrl");
            if (!baseUrl.EndsWith("/")) { baseUrl += "/"; }
        }

        /// <summary>
        /// リアルタイム帳票呼び出し
        /// </summary>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">利用可能支所一覧</param>
        /// <returns></returns>
        public async Task<ReportResult> CreateReport(string reportControlId, string userId, int joukenId, string todofukenCd, string kumiaitoCd, string shishoCd, List<string> shishoList)
        {
            ReportResult result = null;

            // POST メソッドで JSON の Body のリクエストを投げる
            var response = await _httpClient.PostAsJsonAsync(
                baseUrl + reportControlId,
                new ReportRequest
                {
                    reportControlId = reportControlId,
                    userId = userId,
                    joukenId = joukenId,
                    todofukenCd = todofukenCd,
                    kumiaitoCd = kumiaitoCd,
                    shishoCd = shishoCd,
                    shishoList = shishoList
                });

            if (response.IsSuccessStatusCode)   // StatusCodeが200～299 の範囲の場合は、true
            {
                result = await response.Content.ReadFromJsonAsync<ReportResult>();
            }

            return result;
        }
    }
}
