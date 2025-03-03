using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Npgsql;
using SynMain.Areas.F900.Models.D900002;
using SynMain.Areas.F900.Models.D900003;
using System.Dynamic;

namespace SynMain.Areas.F900.Controllers
{
    /// <summary>
    /// 業務エラー/システムエラー
    /// 
    /// このControllerでは、1Controller で2画面へ遷移する処理があるが、特例として許可する。
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/15
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [ExcludeSystemLockCheck]
    [Area("F900")]
    public class D900002Controller : CoreController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D900002Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F900/D900002/Init
        public ActionResult Init()
        {
            Exception ex = TempData["_ERROR_EXCEPTION"] as Exception;
            dynamic document = null;

            if (ex == null)
            {
                // Exceptionのシリアライズはできないため、System.Dynamic.ExpandoObjectとして取得する。
                document = SessionUtil.Get<ExpandoObject>(CoreConst.SESS_COMMON_EXCEPTION, HttpContext);
                //if (document != null)
                //{
                //    SessionUtil.Remove(CoreConst.SESS_COMMON_EXCEPTION, HttpContext);
                //}
            }

            if (ex is AppException || document?.ClassName?.GetString().Contains("AppException") ?? false)
            {
                string Message = string.Empty;
                string HeaderPatternId = string.Empty;
                string ErrorCode = string.Empty;

                if (ex != null)
                {
                    AppException ae = ex as AppException;
                    Message = ae.Message;
                    HeaderPatternId = ae.HeaderPatternId;
                    ErrorCode = ae.ErrorCode;
                }
                else if (document != null)
                {
                    Message = document.Message?.GetString();
                    HeaderPatternId = document.HeaderPatternId?.GetString();
                    ErrorCode = document.ErrorCode?.GetString();
                }

                // 業務エラー
                D900002Model d9002Model = new D900002Model();

                if (TempData["_ERROR_TIME"] != null)
                {
                    d9002Model.ErrorTime = ((DateTime)TempData["_ERROR_TIME"]).ToString("yyyy/MM/dd HH:mm:ss");
                }
                else if (SessionUtil.Get<DateTime>(CoreConst.SESS_COMMON_ERROR_TIME, HttpContext) != null)
                {
                    d9002Model.ErrorTime = SessionUtil.Get<DateTime>(CoreConst.SESS_COMMON_ERROR_TIME, HttpContext).ToString("yyyy/MM/dd HH:mm:ss");
                    //SessionUtil.Remove(CoreConst.SESS_COMMON_ERROR_TIME, HttpContext);
                }
                else
                {
                    d9002Model.ErrorTime = DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss");
                }

                d9002Model.ErrorReason = Message;
                d9002Model.HeaderPatternId = HeaderPatternId;

                if ("MI00010".Equals(ErrorCode))
                {
                    d9002Model.HeaderPatternId = CoreConst.HEADER_PATTERN_ID_2;
                }

                return View("D900002", d9002Model);
            }

            // システムエラー
            D900003Model d9003Model = new D900003Model();
            if (TempData["_ERROR_TIME"] != null)
            {
                d9003Model.ErrorTime = ((DateTime)TempData["_ERROR_TIME"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                d9003Model.ErrorTime = DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss");
            }
            d9003Model.ErrorCode = "MF00001";

            d9003Model.HeaderPatternId = CoreConst.HEADER_PATTERN_ID_2;

            if (ex is NpgsqlException || (document?.ClassName?.GetString().Contains("Npgsql") ?? false))
            {
                // DBに繋がらない場合、メッセージを取得できないでの、メッセージを使用しないエラー画面を出力する
                d9003Model.NoDB = true;
            }

            return View("D900003", d9003Model);
        }
    }
}