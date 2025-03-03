using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 都道府県マスタのユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/18
    /// 作成者：Rou I
    /// </remarks>
    public static class TodofukenUtil
    {

        /// <summary>
        /// 都道府県ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>都道府県選択リスト</returns>
        public static SelectList GetSelectList(ITodofukenDropDownList model)
        {
            return GetSelectList(model.TodofukenCd);
        }

        /// <summary>
        /// 都道府県ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>都道府県選択リスト</returns>
        public static SelectList GetSelectList(string todofukenCd)
        {

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            IEnumerable<MTodofuken> todofukenList = GetTodofukenList();

            foreach (var item in todofukenList)
            {
                selectListItem.Add(new SelectListItem { Value = item.TodofukenCd, Text = item.TodofukenCd + CoreConst.SEPARATOR + item.TodofukenNm });
            }

            if (string.IsNullOrEmpty(todofukenCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", todofukenCd);
            }
        }

        /// <summary>
        /// 都道府県名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>都道府県名</returns>
        public static string GetTodofukenNm(ITodofukenDropDownList model)
        {
            return GetTodofukenNm(model.TodofukenCd);
        }

        /// <summary>
        /// 都道府県コード名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>都道府県名</returns>
        public static string GetTodofukenCdNm(ITodofukenDropDownList model)
        {
            return GetTodofukenCdNm(model.TodofukenCd);
        }

        /// <summary>
        /// 都道府県名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <returns>都道府県名</returns>
        public static string GetTodofukenNm(string todofukenCd)
        {
            if (string.IsNullOrEmpty(todofukenCd))
            {
                return string.Empty;
            }

            IEnumerable<MTodofuken> todofukenList = GetTodofukenList();

            MTodofuken mTodofuken = todofukenList.Where(a => a.TodofukenCd == todofukenCd).FirstOrDefault();

            if (mTodofuken == null)
            {
                return string.Empty;
            }
            else
            {
                return mTodofuken.TodofukenNm;
            }

        }

        /// <summary>
        /// 都道府県コード名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <returns>都道府県コード名</returns>
        public static string GetTodofukenCdNm(string todofukenCd)
        {
            if (string.IsNullOrEmpty(todofukenCd))
            {
                return string.Empty;
            }

            IEnumerable<MTodofuken> todofukenList = GetTodofukenList();

            MTodofuken mTodofuken = todofukenList.Where(a => a.TodofukenCd == todofukenCd).FirstOrDefault();

            if (mTodofuken == null)
            {
                return string.Empty;
            }
            else
            {
                return todofukenCd + CoreConst.SEPARATOR + mTodofuken.TodofukenNm;
            }

        }

        /// <summary>
        /// 都道府県マスタの取得メソッド。
        /// </summary>
        /// <returns>都道府県リスト</returns>
        public static IEnumerable<MTodofuken> GetTodofukenList()
        {
            MTodofukenCache mTodofukenCache = new MTodofukenCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<MTodofuken>>(CacheManager.GetInstance(), CoreConst.M_TODOFUKEN_CACHE, () => (IEnumerable<MTodofuken>)mTodofukenCache.GetList());
        }

        /// <summary>
        /// 都道府県マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MTodofukenCache mTodofukenCache = new MTodofukenCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MTodofuken>>(CacheManager.GetInstance(), CoreConst.M_TODOFUKEN_CACHE, () => (IEnumerable<MTodofuken>)mTodofukenCache.GetList());
        }
    }
}