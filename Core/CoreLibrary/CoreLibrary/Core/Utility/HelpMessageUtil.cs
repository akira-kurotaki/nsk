using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// ヘルプメッセージマスタのユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2020/12/15
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public static class HelpMessageUtil
    {
        /// <summary>
        /// ヘルプメッセージを取得するメソッド
        /// </summary>
        /// <param name="tekiyoStartYmd">適用開始年月日</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemNo">項目No.</param>
        /// <returns>ヘルプメッセージ</returns>
        public static string Get(DateTime tekiyoStartYmd, string screenId, short itemNo)
        {
            var helpMessage = GetMHelpMessage(tekiyoStartYmd, screenId, itemNo);
            return helpMessage.Message;
        }

        /// <summary>
        /// 表示フラグを取得するメソッド
        /// </summary>
        /// <param name="tekiyoStartYmd">適用開始年月日</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemNo">項目No.</param>
        /// <returns>表示フラグ</returns>
        public static bool IsDisplay(DateTime tekiyoStartYmd, string screenId, short itemNo)
        {
            var helpMessage = GetMHelpMessage_NonError(tekiyoStartYmd, screenId, itemNo);
            return "1".Equals(helpMessage?.DisplayFlg);
        }

        /// <summary>
        /// メッセージマスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MHelpMessageCache mHelpMessageCache = new MHelpMessageCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MHelpMessage>>(CacheManager.GetInstance(), CoreConst.M_HELP_MESSAGE_CACHE, () => (IEnumerable<MHelpMessage>)mHelpMessageCache.GetList());
        }

        /// <summary>
        /// ヘルプメッセージマスタを取得するメソッド
        /// </summary>
        /// <param name="tekiyoStartYmd">適用開始年月日</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemNo">項目No.</param>
        /// <returns>ヘルプメッセージマスタ</returns>
        private static MHelpMessage GetMHelpMessage(DateTime tekiyoStartYmd, string screenId, short itemNo)
        {
            MHelpMessageCache mHelpMessageCache = new MHelpMessageCache(CacheManager.GetInstance());
            MHelpMessage helpMessage = CacheUtil.Get<IEnumerable<MHelpMessage>>(CacheManager.GetInstance()
                                        , CoreConst.M_HELP_MESSAGE_CACHE, () => (IEnumerable<MHelpMessage>)mHelpMessageCache.GetList())
                                        .SingleOrDefault(a =>
                                            (a.TekiyoStartYmd <= tekiyoStartYmd &&
                                                (a.TekiyoEndYmd == null ||
                                                (a.TekiyoEndYmd != null && tekiyoStartYmd <= a.TekiyoEndYmd))) &&
                                             a.ScreenId == screenId &&
                                             a.ItemNo == itemNo
                                        );
            // 例外処理
            if (helpMessage == null)
            {
                throw new ArgumentException(string.Format(
                    "ヘルプメッセージデータが見つかりません。(適用開始年月日 = \"{0}\"、画面ID = \"{1}\"、項目No. = \"{2}\")。システム管理者に連絡してください。(MF00010)",
                    tekiyoStartYmd.ToString(), screenId, itemNo.ToString()));
            }

            return helpMessage;
        }

        /// <summary>
        /// ヘルプメッセージマスタを取得するメソッド
        /// </summary>
        /// <param name="tekiyoStartYmd">適用開始年月日</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemNo">項目No.</param>
        /// <returns>ヘルプメッセージマスタ</returns>
        private static MHelpMessage GetMHelpMessage_NonError(DateTime tekiyoStartYmd, string screenId, short itemNo)
        {
            MHelpMessageCache mHelpMessageCache = new MHelpMessageCache(CacheManager.GetInstance());
            MHelpMessage helpMessage = CacheUtil.Get<IEnumerable<MHelpMessage>>(CacheManager.GetInstance()
                                        , CoreConst.M_HELP_MESSAGE_CACHE, () => (IEnumerable<MHelpMessage>)mHelpMessageCache.GetList())
                                        .SingleOrDefault(a =>
                                            (a.TekiyoStartYmd <= tekiyoStartYmd &&
                                                (a.TekiyoEndYmd == null ||
                                                (a.TekiyoEndYmd != null && tekiyoStartYmd <= a.TekiyoEndYmd))) &&
                                             a.ScreenId == screenId &&
                                             a.ItemNo == itemNo
                                        );
            return helpMessage;
        }
    }
}