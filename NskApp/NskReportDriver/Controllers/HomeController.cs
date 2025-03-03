using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Context;
using RealtimeReportDriver.Models;
using ReportService.Core;
using System.Diagnostics;

namespace RealtimeRBaseReportDrivereportDriver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReportServiceClient _serviceClient;

        private JigyoCommonContext db = new JigyoCommonContext();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="serviceClient">リアル帳票出力サービス呼び出しクライアント</param>
        public HomeController(ReportServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public IActionResult Index()
        {
            var indexModel = new IndexModel();
            indexModel.ReportControlIdOptions = GetReportControlIdOptions();

            var capacity = 10;
            indexModel.ShishoList = new List<string>(capacity);
            for (int i = 0; i < capacity; i++) { indexModel.ShishoList.Add(null); }

            return View(indexModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// リアルタイム帳票作成
        /// </summary>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="joukenId">条件ID</param>
        /// <returns></returns>
        public async Task<ActionResult> CreatRealtimeReportAsync([Bind("ReportControlId, UserId, JoukenId, TodofukenCd, KumiaitoCd, ShishoCd, ShishoList")] IndexModel model)
        {
            // 条件IDは正の整数以外の場合、エラーとする
            if (model.JoukenId <= 0)
            {
                throw new ApplicationException("条件IDは正の整数を入力してください！");
            }

            var shishoList = new List<string>();
            if (model.ShishoList != null)
            {
                shishoList = model.ShishoList.Where(s => s != null).ToList();
            }

            // リアル帳票出力サービスの帳票出力処理を呼び出す
            var result = await _serviceClient.CreateReport(model.ReportControlId, model.UserId, (int)model.JoukenId, model.TodofukenCd, model.KumiaitoCd, model.ShishoCd, shishoList);

            // エラーの場合
            if (result.Result != 0)
            {
                throw new ApplicationException(result.ErrorMessage);
            }

            var fileName = db.MReportKanris.Where(t => t.ReportControlId == model.ReportControlId).FirstOrDefault().FileNm;
            var cd = new System.Net.Mime.ContentDisposition
            {
                // ERR_RESPONSE_HEADERS_MULTIPLE_CONTENT_DISPOSITIONを対応するため、cd.FileNameの設定不要
                Inline = true,
            };

            Response.Headers.Append("Content-Disposition", cd.ToString());
            return File(result.ReportData, "application/pdf", cd.FileName);
        }

        /// <summary>
        /// 帳票制御IDの選択肢を取得する
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ReportControlIdOptionModel> GetReportControlIdOptions()
        {
            // 帳票処理管理マスタから帳票制御情報を取得
            var reportControlIdOptions =
                db.MReportKanris.OrderBy(p => p.ReportControlId).Select(p => new ReportControlIdOptionModel()
                {
                    ControlId = p.ReportControlId,
                    ControlName = p.ReportControlNm
                }).ToList();

            return reportControlIdOptions;
        }
    }
}
