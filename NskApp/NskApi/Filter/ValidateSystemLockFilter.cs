using NskApi.Base;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace NskApi.Filter
{
    /// <summary>
    /// システムロック有無を検証するフィルター（API用）
    /// </summary>
    public class ValidateSystemLockFilter : ActionFilterAttribute
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 業務メソッドが呼ばれる前の処理
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            logger.Debug("ActionArguments : " + context.ActionArguments);

            // ControllerのExcludeSystemLockCheckAttribute属性を取得
            var exclude = (ExcludeSystemLockCheckAttribute[])context.Controller.GetType().GetCustomAttributes(typeof(ExcludeSystemLockCheckAttribute), false);
            logger.Debug("exclude.Length : " + exclude.Length);
            if (exclude.Length == 0)
            {
                if (context.ActionArguments.TryGetValue("request", out object value))
                {
                    if (value is ApiRequestBase)
                    {
                        var param = value as ApiRequestBase;

                        // システム区分
                        string systemKbn = param.SystemKbn;
                        // 都道府県コード
                        var todofukenCd = param.TodofukenCd;

                        // システム日時
                        var systemDate = DateUtil.GetSysDateTime();

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
                            context.Result = new ConflictObjectResult(new ApiResponseBase()
                            {
                                messages = new List<Message>
                                {
                                    new()
                                    {
                                        message = MessageUtil.Get("ME91013", "システム", lockUserId)
                                    }
                                }
                            });
                        }
                        else if (lockStatus == LockUtil.LOCKED_STATE_ERROR)
                        {
                            // エラー
                            logger.Fatal("システムロック取得でエラーが発生しました。");
                            throw new SystemException(MessageUtil.Get("MF00001"));
                        }
                    }
                }
            }
        }
    }
}
