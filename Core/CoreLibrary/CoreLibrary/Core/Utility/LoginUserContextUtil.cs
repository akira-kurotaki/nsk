using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using ModelLibrary.Context;

namespace CoreLibrary.Core.Utility
{
    public static class LoginUserContextUtil
    {
        /// <summary>
        /// セッションからログインユーザのDB接続先を取得し、DBコンテキストを作成する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static JigyoContext getLoginUserContext(HttpContext context)
        {
            // セッションに格納されたログインユーザのDB接続先からDBコンテキストを作成する
            var info = SessionUtil.Get<DbConnectionInfo>(CoreConst.SESS_DB_CONN, context);
            return new JigyoContext(info.ConnectionString, info.DefaultSchema, ConfigUtil.GetInt("CommandTimeout"));
        }
    }
}
