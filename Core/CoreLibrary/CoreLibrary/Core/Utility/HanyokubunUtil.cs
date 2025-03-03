using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 汎用区分マスタのユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/30
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public static class HanyokubunUtil
    {

        /// <summary>
        /// 汎用区分コードリストの取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <returns>コードリスト</returns>
        public static IEnumerable<MHanyokubun> GetHanyokubunList(string kbnSbt)
        {
            if (string.IsNullOrEmpty(kbnSbt))
            {
                return null;
            }
            MHanyokubunCache mHanyokubunCache = new MHanyokubunCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<MHanyokubun>>(CacheManager.GetInstance(),
                    CoreConst.M_HANYOKUBUN_CACHE, () => (IEnumerable<MHanyokubun>)mHanyokubunCache.GetList())
                    .Where(a => a.KbnSbt == kbnSbt);
        }

        /// <summary>
        /// 区分名称の取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="kbnCd">区分コード</param>
        /// <returns>区分名称</returns>
        public static string GetKbnNm(string kbnSbt, string kbnCd)
        {
            if (string.IsNullOrEmpty(kbnSbt))
            {
                return null;
            }
            if (string.IsNullOrEmpty(kbnCd))
            {
                return string.Empty;
            }
            MHanyokubunCache mHanyokubunCache = new MHanyokubunCache(CacheManager.GetInstance());
            MHanyokubun mHanyokubun = CacheUtil.Get<IEnumerable<MHanyokubun>>(CacheManager.GetInstance(),
                    CoreConst.M_HANYOKUBUN_CACHE, () => (IEnumerable<MHanyokubun>)mHanyokubunCache.GetList())
                    .Where(a => a.KbnSbt == kbnSbt)
                    .SingleOrDefault(a => a.KbnCd == kbnCd);

            return mHanyokubun == null ? string.Empty : mHanyokubun.KbnNm;
        }

        /// <summary>
        /// 区分名称の取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="kbnCd">区分コード</param>
        /// <returns>区分名称</returns>
        public static string GetKbnNm(string kbnSbt, object kbnCd)
        {
            if (string.IsNullOrEmpty(kbnSbt))
            {
                return null;
            }
            if (kbnCd == null)
            {
                return string.Empty;
            }

            return GetKbnNm(kbnSbt, kbnCd.ToString());
        }

        /// <summary>
        /// 区分略称の取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="kbnCd">区分コード</param>
        /// <returns>区分略称</returns>
        public static string GetKbnRnm(string kbnSbt, string kbnCd)
        {
            if (string.IsNullOrEmpty(kbnSbt))
            {
                return null;
            }
            if (string.IsNullOrEmpty(kbnCd))
            {
                return string.Empty;
            }
            MHanyokubunCache mHanyokubunCache = new MHanyokubunCache(CacheManager.GetInstance());
            MHanyokubun mHanyokubun = CacheUtil.Get<IEnumerable<MHanyokubun>>(CacheManager.GetInstance(),
                    CoreConst.M_HANYOKUBUN_CACHE, () => (IEnumerable<MHanyokubun>)mHanyokubunCache.GetList())
                    .Where(a => a.KbnSbt == kbnSbt)
                    .SingleOrDefault(a => a.KbnCd == kbnCd);

            return mHanyokubun == null ? string.Empty : mHanyokubun.KbnRnm;
        }

        /// <summary>
        /// 区分略称の取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="kbnCd">区分コード</param>
        /// <returns>区分略称</returns>
        public static string GetKbnRnm(string kbnSbt, object kbnCd)
        {
            if (string.IsNullOrEmpty(kbnSbt))
            {
                return null;
            }
            if (kbnCd == null)
            {
                return string.Empty;
            }

            return GetKbnRnm(kbnSbt, kbnCd.ToString());
        }

        /// <summary>
        /// 選択リストの取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetSelectList(string kbnSbt)
        {
            // デフォルト　表示モード:「区分名称」形式
            return GetSelectList(kbnSbt, CoreConst.DisplayMode.KbnNm);
        }

        /// <summary>
        /// 選択リストの取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="displayMode">表示モード</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetSelectList(string kbnSbt, CoreConst.DisplayMode displayMode)
        {
            if (string.IsNullOrEmpty(kbnSbt))
            {
                return new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
            var hanyokubnList = GetHanyokubunList(kbnSbt);

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            foreach (var item in hanyokubnList)
            {
                switch (displayMode)
                {
                    case CoreConst.DisplayMode.KbnCd:
                        selectListItem.Add(new SelectListItem { Value = item.KbnCd, Text = item.KbnCd });
                        break;
                    case CoreConst.DisplayMode.KbnNm:
                        selectListItem.Add(new SelectListItem { Value = item.KbnCd, Text = item.KbnNm });
                        break;
                    case CoreConst.DisplayMode.KbnCdNm:
                        selectListItem.Add(new SelectListItem { Value = item.KbnCd, Text = item.KbnCd + CoreConst.SEPARATOR + item.KbnNm });
                        break;
                }

            }

            return new SelectList(selectListItem, "Value", "Text");
        }


        /// <summary>
        /// 選択リストの取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="displayMode">表示モード</param>
        /// <param name="selectedValue">選択値</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetSelectList(string kbnSbt, CoreConst.DisplayMode displayMode, object selectedValue)
        {
            if (string.IsNullOrEmpty(kbnSbt))
            {
                return new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
            var hanyokubnList = GetHanyokubunList(kbnSbt);

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            foreach (var item in hanyokubnList)
            {
                switch (displayMode)
                {
                    case CoreConst.DisplayMode.KbnCd:
                        selectListItem.Add(new SelectListItem { Value = item.KbnCd, Text = item.KbnCd });
                        break;
                    case CoreConst.DisplayMode.KbnNm:
                        selectListItem.Add(new SelectListItem { Value = item.KbnCd, Text = item.KbnNm });
                        break;
                    case CoreConst.DisplayMode.KbnCdNm:
                        selectListItem.Add(new SelectListItem { Value = item.KbnCd, Text = item.KbnCd + CoreConst.SEPARATOR + item.KbnNm });
                        break;
                }

            }

            return new SelectList(selectListItem, "Value", "Text", selectedValue);
        }

        /// <summary>
        /// 汎用区分マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MHanyokubunCache mHanyokubunCache = new MHanyokubunCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MHanyokubun>>(CacheManager.GetInstance(), CoreConst.M_HANYOKUBUN_CACHE, () => (IEnumerable<MHanyokubun>)mHanyokubunCache.GetList());
        }
    }
}