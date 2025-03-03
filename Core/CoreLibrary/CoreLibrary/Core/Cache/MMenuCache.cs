using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// メニューマスタテーブルのキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/13
    /// 作成者：Rou I
    /// </remarks>
    public class MMenuCache : CacheBase
    {
        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MMenuCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_MENU_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            return db.MMenus
                .AsEnumerable()
                .Select(m => new MMenu
                {
                    FirstMenuGroup = m.FirstMenuGroup,
                    FirstMenuDisplayFlg = m.FirstMenuDisplayFlg,
                    FirstMenuDisplayOrder = m.FirstMenuDisplayOrder,
                    FirstMenuDisplayKinoNm = m.FirstMenuDisplayKinoNm,
                    HyojiKbn = m.HyojiKbn,
                    UserKanriKengen = m.UserKanriKengen,
                }).ToList();
        }
    }
}