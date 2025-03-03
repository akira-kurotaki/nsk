using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace NskWeb.Core.Filter
{
    /// <summary>
    /// ベースアプリケーションのシステムロック有無を検証するフィルター
    /// </summary>
    public class ValidSystemLockNskWebAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 業務のメソッドが呼ばれる前の処理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // ControllerのExcludeSystemLockCheckAttribute属性を取得
            var exclude = (ExcludeSystemLockCheckAttribute[])filterContext.Controller.GetType().GetCustomAttributes(typeof(ExcludeSystemLockCheckAttribute), false);
            if (exclude.Length == 0)
            {
                // システム区分
                string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);

                // システム日時
                var systemDate = DateUtil.GetSysDateTime();

                // 都道府県コード
                var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, filterContext.HttpContext);
                var todofukenCd = syokuin?.TodofukenCd;

                logger.Debug("SystemKbn : " + systemKbn);
                logger.Debug("TodofukenCd : " + todofukenCd);

                // ロック実行ユーザID
                string lockUserId = string.Empty;

                // システムロック取得
                var lockStatus = LockUtil.GetSysLock(systemKbn, todofukenCd, systemDate, ref lockUserId);

                logger.Debug("LockStatus : " + lockStatus);
                logger.Debug("LockUserId : " + lockUserId);

                if (lockStatus == LockUtil.LOCKED_STATE_LOCKED)
                {
                    // ロック有り
                    throw new AppException("MI00020", SystemMessageUtil.Get("MI00020", lockUserId), CoreConst.HEADER_PATTERN_ID_2);
                }
                else if (lockStatus == LockUtil.LOCKED_STATE_ERROR)
                {
                    // エラー
                    logger.Fatal("システムロック取得でエラーが発生しました。");
                    throw new SystemException(SystemMessageUtil.Get("MF00001"));
                }
            }
        }
    }
}
