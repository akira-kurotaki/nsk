using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 支所のユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/22
    /// 作成者：Rou I
    /// </remarks>
    public class ShishoUtil
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 支所ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>支所選択リスト</returns>
        public static SelectList GetSelectList(ITodofukenDropDownList model)
        {
            return GetSelectList(model.TodofukenCd, model.KumiaitoCd, model.ShishoCd, model.ShishoList);
        }

        /// <summary>
        /// 支所ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">支所情報リスト</param>
        /// <returns>支所選択リスト</returns>
        public static SelectList GetSelectList(string todofukenCd, string kumiaitoCd, string shishoCd, List<Shisho> shishoList)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd))
            {
                return new SelectList(new List<SelectListItem>(), "Value", "Text");
            }

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            // セッションから取得する利用可能支所一覧が設定されている場合
            if (shishoList != null && shishoList.Count != 0)
            {
                foreach (var item in shishoList)
                {
                    selectListItem.Add(new SelectListItem { Value = item.ShishoCd, Text = item.ShishoCd + CoreConst.SEPARATOR + item.ShishoNm });
                }
            }
            else
            {
                // セッションから取得する利用可能支所一覧が設定されていない場合
                List<MShishoNm> vshishoList = GetShishoList(todofukenCd, kumiaitoCd);

                foreach (var item in vshishoList)
                {
                    selectListItem.Add(new SelectListItem { Value = item.ShishoCd, Text = item.ShishoCd + CoreConst.SEPARATOR + item.ShishoNm });
                }
            }

            if (string.IsNullOrEmpty(shishoCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", shishoCd);
            }
        }

        /// <summary>
        /// 支所名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>支所名</returns>
        public static string GetShishoNm(ITodofukenDropDownList model)
        {
            if (model.ShishoList != null && model.ShishoList.Count != 0)
            {
                return GetShishoNm(model.ShishoCd, model.ShishoList);
            }
            else
            {
            return GetShishoNm(model.TodofukenCd, model.KumiaitoCd, model.ShishoCd);
        }
        }

        /// <summary>
        /// 支所コード名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>支所コード名</returns>
        public static string GetShishoCdNm(ITodofukenDropDownList model)
        {
            if (model.ShishoList != null && model.ShishoList.Count != 0)
            {
                return GetShishoCdNm(model.ShishoCd, model.ShishoList);
            }
            else
            {
            return GetShishoCdNm(model.TodofukenCd, model.KumiaitoCd, model.ShishoCd);
        }
        }

        /// <summary>
        /// 支所名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>支所名</returns>
        public static string GetShishoNm(string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(shishoCd))
            {
                return string.Empty;
            }

            MShishoNm mNicShisho = GetShisho(todofukenCd, kumiaitoCd, shishoCd);

            if (mNicShisho == null)
            {
                return string.Empty;
            }
            else
            {
                return mNicShisho.ShishoNm;
            }
        }

        /// <summary>
        /// 支所名の取得メソッド。
        /// </summary>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">支所情報リスト</param>
        /// <returns>支所名</returns>
        public static string GetShishoNm(string shishoCd, List<Shisho> shishoList)
        {
            if (shishoList == null || shishoList.Count ==0 || string.IsNullOrEmpty(shishoCd))
            {
                return string.Empty;
            }

            var shisho = shishoList.Where(t => t.ShishoCd == shishoCd).FirstOrDefault();

            if (shisho == null)
            {
                return string.Empty;
            }
            else
            {
                return shisho.ShishoNm;
            }
        }

        /// <summary>
        /// 支所コード名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>支所コード名</returns>
        public static string GetShishoCdNm(string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(shishoCd))
            {
                return string.Empty;
            }

            MShishoNm mNicShisho = GetShisho(todofukenCd, kumiaitoCd, shishoCd);

            if (mNicShisho == null)
            {
                return string.Empty;
            }
            else
            {
                return shishoCd + CoreConst.SEPARATOR + mNicShisho.ShishoNm;
            }
        }

        /// <summary>
        /// 支所コード名の取得メソッド。
        /// </summary>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">支所情報リスト</param>
        /// <returns>支所コード名</returns>
        public static string GetShishoCdNm(string shishoCd, List<Shisho> shishoList)
        {
            if (string.IsNullOrEmpty(shishoCd) || shishoList == null || shishoList.Count == 0)
            {
                return string.Empty;
            }

            var shisho = shishoList.Where(t => t.ShishoCd == shishoCd).FirstOrDefault();

            if (shisho == null)
            {
                return string.Empty;
            }
            else
            {
                return shisho.ShishoCd + CoreConst.SEPARATOR + shisho.ShishoNm;
            }
        }

        /// <summary>
        /// 支所マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>支所マスタリスト</returns>
        public static List<MShishoNm> GetShishoList(string todofukenCd, string kumiaitoCd)
        {
            List<MShishoNm> result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                logger.Info("名称M支所マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd + " ）");
                result = db.MShishoNms
                    .OrderBy(a => a.ShishoCd)
                    .Where(a => a.TodofukenCd == todofukenCd &&
                                a.KumiaitoCd == kumiaitoCd)
                    .AsEnumerable().Select(m => new MShishoNm
                    {
                        ShishoCd = m.ShishoCd,
                        ShishoNm = m.ShishoNm
                    }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 支所マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>支所マスタ</returns>
        public static MShishoNm GetShisho(string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            MShishoNm result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                logger.Info("名称M支所マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd +
                    "、支所コード：" + shishoCd + " ）");
                result = db.MShishoNms
                    .OrderBy(a => a.ShishoCd)
                    .Where(a => a.TodofukenCd == todofukenCd &&
                                a.KumiaitoCd == kumiaitoCd &&
                                a.ShishoCd == shishoCd)
                    .FirstOrDefault();
            }
            return result;
        }
    }
}