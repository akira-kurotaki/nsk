using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using NLog;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 権限ユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/09
    /// 作成者：Rou I
    /// </remarks>
    public static class AuthorityUtil
    {
        /// <summary>
        /// ユーザ管理権限「1:要」
        /// </summary>
        public const string USER_KANRI_KENGEN_YOU = "1";

        /// <summary>
        /// ユーザ管理権限「0:なし」
        /// </summary>
        public const string USER_KANRI_KENGEN_NASHI = "0";

        /// <summary>
        /// アクセス権限なし
        /// </summary>
        public const string ACCESS_AUTHORITY_NASHI = "0";

        /// <summary>
        /// アクセス権限あり
        /// </summary>
        public const string ACCESS_AUTHORITY_ARI = "1";

        /// <summary>
        /// アクセス権限なし（左メニューのみ）
        /// </summary>
        public const string ACCESS_AUTHORITY_MENU_NASHI = "2";

        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 画面遷移権限チェックメソッド
        /// </summary>
        /// <param name="screenId">遷移先画面ID</param>
        /// <param name="context">HttpContext</param>
        /// <returns>権限チェック結果(true / false)</returns>
        public static bool HasTransitionAuthority(string screenId, HttpContext context)
        {
            if (SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context) == null)
            {
                logger.Info("screenId:" + screenId, ", ログインユーザ不明のため、権限なし");
                return false;
            }

            Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context);

            // 画面マスタの取得
            var mScreen = ScreenUtil.GetScreen(screenId);

            if (mScreen == null)
            {
                return false;
            }

            // 画面マスタ.[ユーザ管理権限]が「1:要」かつ 引数.ユーザ管理権限が「0:なし」の場合
            if (USER_KANRI_KENGEN_YOU.Equals(mScreen.UserKanriKengen) &&
                USER_KANRI_KENGEN_NASHI.Equals(syokuin.UserKanriKengen))
            {
                return false;
            }

            // 画面マスタ.[表示区分]にログインユーザ.[システム利用者区分]が含まれない場合
            if (!mScreen.HyojiKbn.Contains(syokuin.SystemRiyoKbn))
            {
                return false;
            }

            // 画面機能権限チェック
            return ScreenSosaUtil.CanReference(screenId, context);
        }
    }
}