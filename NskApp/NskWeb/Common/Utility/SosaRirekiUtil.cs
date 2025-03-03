using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using CoreLibrary.Core.Utility;
using NLog;

namespace NskWeb.Common.Utility
{
    /// <summary>
    /// 操作履歴関連ユーティリティ
    /// </summary>
    public class SosaRirekiUtil
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 操作履歴登録
        /// </summary>
        /// <param name="db">データベース</param>
        /// <param name="nogyoshaId">農業者ID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="systemRiyoKbn">システム利用者区分</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="sosaNaiyo">操作内容</param>
        /// <param name="displayColor">表示色</param>
        /// <returns>True：成功　False：失敗</returns>
        public static bool RegisterHistory(NskAppContext db, int nogyoshaId, string userId, string screenId,
                                        string systemRiyoKbn, string shishoCd, string sosaNaiyo, string displayColor)
        {

            TSosaRireki sosaRireki = new TSosaRireki()
            {
                NogyoshaId = nogyoshaId,
                SosaDate = DateUtil.GetSysDateTime(),
                UserId = userId,
                SystemRiyoKbn = systemRiyoKbn,
                ScreenId = screenId,
                ShishoCd = shishoCd,
                SosaNaiyo = sosaNaiyo,
                DisplayColor = displayColor
            };

            db.TSosaRirekis.Add(sosaRireki);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }

            return true;
        }
    }
}
