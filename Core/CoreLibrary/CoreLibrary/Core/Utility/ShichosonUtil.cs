using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 市町村のユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/22
    /// 作成者：Rou I
    /// </remarks>
    public class ShichosonUtil
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 市町村ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>市町村選択リスト</returns>
        public static SelectList GetSelectList(ITodofukenDropDownList model)
        {
            return GetSelectList(model.TodofukenCd, model.KumiaitoCd, model.ShichosonCd);
        }

        /// <summary>
        /// 市町村ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shichosonCd">市町村コード</param>
        /// <returns>市町村選択リスト</returns>
        public static SelectList GetSelectList(string todofukenCd, string kumiaitoCd, string shichosonCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd))
            {
                return new SelectList(new List<SelectListItem>(), "Value", "Text");
            }

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            List<MShichosonNm> shichosonList = GetShichosonList(todofukenCd, kumiaitoCd);

            foreach (var item in shichosonList)
            {
                selectListItem.Add(new SelectListItem { Value = item.ShichosonCd, Text = item.ShichosonCd + CoreConst.SEPARATOR + item.ShichosonNm });
            }

            if (string.IsNullOrEmpty(shichosonCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", shichosonCd);
            }
        }

        /// <summary>
        /// 市町村名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>市町村名</returns>
        public static string GetShichosonNm(ITodofukenDropDownList model)
        {
            return GetShichosonNm(model.TodofukenCd, model.KumiaitoCd, model.ShichosonCd);
        }

        /// <summary>
        /// 市町村コード名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>市町村コード名</returns>
        public static string GetShichosonCdNm(ITodofukenDropDownList model)
        {
            return GetShichosonCdNm(model.TodofukenCd, model.KumiaitoCd, model.ShichosonCd);
        }

        /// <summary>
        /// 市町村名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shichosonCd">市町村コード</param>
        /// <returns>市町村名</returns>
        public static string GetShichosonNm(string todofukenCd, string kumiaitoCd, string shichosonCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(shichosonCd))
            {
                return string.Empty;
            }

            MShichosonNm mNicShichoson = GetShichoson(todofukenCd, kumiaitoCd, shichosonCd);

            if (mNicShichoson == null)
            {
                return string.Empty;
            }
            else
            {
                return mNicShichoson.ShichosonNm;
            }

        }

        /// <summary>
        /// 市町村コード名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shichosonCd">市町村コード</param>
        /// <returns>市町村コード名</returns>
        public static string GetShichosonCdNm(string todofukenCd, string kumiaitoCd, string shichosonCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(shichosonCd))
            {
                return string.Empty;
            }

            MShichosonNm mNicShichoson = GetShichoson(todofukenCd, kumiaitoCd, shichosonCd);

            if (mNicShichoson == null)
            {
                return string.Empty;
            }
            else
            {
                return shichosonCd + CoreConst.SEPARATOR + mNicShichoson.ShichosonNm;
            }

        }

        /// <summary>
        /// 市町村マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>市町村マスタリスト</returns>
        public static List<MShichosonNm> GetShichosonList(string todofukenCd, string kumiaitoCd)
        {
            List<MShichosonNm> result;
            using (JigyoCommonContext db = new JigyoCommonContext()) 
            {
                logger.Info("名称M市町村マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd + " ）");
                result = db.MShichosonNms
                    .OrderBy(a => a.ShichosonCd)
                    .Where(a => a.TodofukenCd == todofukenCd &&
                                a.KumiaitoCd == kumiaitoCd)
                    .AsEnumerable().Select(m => new MShichosonNm
                    {
                        ShichosonCd = m.ShichosonCd,
                        ShichosonNm = m.ShichosonNm
                    }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 市町村マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shichosonCd">市町村コード</param>
        /// <returns>市町村マスタ</returns>
        public static MShichosonNm GetShichoson(string todofukenCd, string kumiaitoCd, string shichosonCd)
        {
            logger.Info("名称M市町村マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd + "、市町村コード：" + shichosonCd + " ）");
            MShichosonNm result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                result = db.MShichosonNms
                    .OrderBy(a => a.ShichosonCd)
                    .Where(a => a.TodofukenCd == todofukenCd &&
                                a.KumiaitoCd == kumiaitoCd &&
                                a.ShichosonCd == shichosonCd)
                    .FirstOrDefault();
            }
            return result;
        }
    }
}