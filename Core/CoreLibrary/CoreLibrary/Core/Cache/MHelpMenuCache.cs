using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using ModelLibrary.Models;
using System.Text;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// ヘルプメニューマスタテーブルのキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/04/13
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public class MHelpMenuCache : CacheBase
    {
        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MHelpMenuCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_HELP_MENU_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            return db.MHelpMenus
                .AsEnumerable()
                .Select(m => new HelpMenuDto
                {
                    HelpMenuDisplayOrder = m.HelpMenuDisplayOrder,
                    HelpMenuDisplayFlg = m.HelpMenuDisplayFlg,
                    HelpMenuDisplayKinoNm = m.HelpMenuDisplayKinoNm,
                    HelpFilePath = m.HelpFilePath,
                    HyojiKbn = m.HyojiKbn,
                    UserKanriKengen = m.UserKanriKengen,
                    Hash = CryptoUtil.GetMD5Hex(Encoding.UTF8.GetBytes(m.HelpMenuDisplayOrder.ToString()))
                }).ToList();
        }
    }
}