using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 大地区のユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/22
    /// 作成者：Rou I
    /// </remarks>
    public class DaichikuUtil
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 大地区ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>大地区選択リスト</returns>
        public static SelectList GetSelectList(ITodofukenDropDownList model)
        {
            return GetSelectList(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd);
        }

        /// <summary>
        /// 大地区ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <returns>大地区選択リスト</returns>
        public static SelectList GetSelectList(string todofukenCd, string kumiaitoCd, string daichikuCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd))
            {
                return new SelectList(new List<SelectListItem>(), "Value", "Text");
            }

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            List<MDaichikuNm> daichikuList = GetDaichikuList(todofukenCd, kumiaitoCd);

            foreach (var item in daichikuList)
            {
                selectListItem.Add(new SelectListItem { Value = item.DaichikuCd, Text = item.DaichikuCd + CoreConst.SEPARATOR + item.DaichikuNm });
            }

            if (string.IsNullOrEmpty(daichikuCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", daichikuCd);
            }
        }

        /// <summary>
        /// 大地区名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>大地区名</returns>
        public static string GetDaichikuNm(ITodofukenDropDownList model)
        {
            return GetDaichikuNm(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd);
        }

        /// <summary>
        /// 大地区コード名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>大地区コード名</returns>
        public static string GetDaichikuCdNm(ITodofukenDropDownList model)
        {
            return GetDaichikuCdNm(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd);
        }

        /// <summary>
        /// 大地区名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <returns>大地区名</returns>
        public static string GetDaichikuNm(string todofukenCd, string kumiaitoCd, string daichikuCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(daichikuCd))
            {
                return string.Empty;
            }

            MDaichikuNm mNicDaichiku = GetDaichiku(todofukenCd, kumiaitoCd, daichikuCd);

            if (mNicDaichiku == null)
            {
                return string.Empty;
            }
            else
            {
                return mNicDaichiku.DaichikuNm;
            }

        }

        /// <summary>
        /// 大地区コード名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <returns>大地区コード名</returns>
        public static string GetDaichikuCdNm(string todofukenCd, string kumiaitoCd, string daichikuCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(daichikuCd))
            {
                return string.Empty;
            }

            MDaichikuNm mNicDaichiku = GetDaichiku(todofukenCd, kumiaitoCd, daichikuCd);

            if (mNicDaichiku == null)
            {
                return string.Empty;
            }
            else
            {
                return daichikuCd + CoreConst.SEPARATOR + mNicDaichiku.DaichikuNm;
            }

        }

        /// <summary>
        /// 大地区マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>大地区マスタリスト</returns>
        public static List<MDaichikuNm> GetDaichikuList(string todofukenCd, string kumiaitoCd)
        {
            List<MDaichikuNm> result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                logger.Info("名称M大地区マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd + " ）");
                result = db.MDaichikuNms
                            .OrderBy(a => a.DaichikuCd)
                            .Where(a => a.TodofukenCd == todofukenCd &&
                                        a.KumiaitoCd == kumiaitoCd)
                            .AsEnumerable().Select(m => new MDaichikuNm
                            {
                                DaichikuCd = m.DaichikuCd,
                                DaichikuNm = m.DaichikuNm
                            }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 大地区マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <returns>大地区マスタ</returns>
        public static MDaichikuNm GetDaichiku(string todofukenCd, string kumiaitoCd, string daichikuCd)
        {
            MDaichikuNm result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                logger.Info("名称M大地区マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd +
                             "、大地区コード：" + daichikuCd + " ）");
                result = db.MDaichikuNms
                            .OrderBy(a => a.DaichikuCd)
                            .Where(a => a.TodofukenCd == todofukenCd &&
                                        a.KumiaitoCd == kumiaitoCd &&
                                        a.DaichikuCd == daichikuCd)
                            .FirstOrDefault();
            }
            return result;
        }
    }
}