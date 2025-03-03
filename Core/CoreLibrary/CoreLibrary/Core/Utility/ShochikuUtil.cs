using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 小地区のユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/22
    /// 作成者：Rou I
    /// </remarks>
    public class ShochikuUtil
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 小地区ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>小地区選択リスト</returns>
        public static SelectList GetSelectList(ITodofukenDropDownList model)
        {
            return GetSelectList(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCd);
        }

        /// <summary>
        /// 小地区(From)ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetSelectListFrom(ITodofukenDropDownList model)
        {
            return GetSelectList(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCdFrom);
        }

        /// <summary>
        /// 小地区(To)ドロップダウンリストの取得メソッド(初期値設定あり)。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetSelectListTo(ITodofukenDropDownList model)
        {
            return GetSelectList(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCdTo);
        }

        /// <summary>
        /// 小地区ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <param name="shochikuCd">小地区コード</param>
        /// <returns>小地区選択リスト</returns>
        public static SelectList GetSelectList(string todofukenCd, string kumiaitoCd, string daichikuCd, string shochikuCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(daichikuCd))
            {
                return new SelectList(new List<SelectListItem>(), "Value", "Text");
            }

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            List<MShochikuNm> shochikuList = GetShochikuList(todofukenCd, kumiaitoCd, daichikuCd);

            foreach (var item in shochikuList)
            {
                selectListItem.Add(new SelectListItem { Value = item.ShochikuCd, Text = item.ShochikuCd + CoreConst.SEPARATOR + item.ShochikuNm });
            }

            if (string.IsNullOrEmpty(shochikuCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", shochikuCd);
            }

        }

        /// <summary>
        /// 小地区名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>小地区名</returns>
        public static string GetShochikuNm(ITodofukenDropDownList model)
        {
            return GetShochikuNm(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCd);
        }

        /// <summary>
        /// 小地区名の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>小地区コード名</returns>
        public static string GetShochikuCdNm(ITodofukenDropDownList model)
        {
            return GetShochikuCdNm(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCd);
        }

        /// <summary>
        /// 小地区名(From)の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>小地区名(From)</returns>
        public static string GetShochikuFromNm(JigyoContext db, ITodofukenDropDownList model)
        {
            return GetShochikuNm(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCdFrom);
        }

        /// <summary>
        /// 小地区コード名(From)の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>小地区コード名(From)</returns>
        public static string GetShochikuFromCdNm(ITodofukenDropDownList model)
        {
            return GetShochikuCdNm(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCdFrom);
        }

        /// <summary>
        /// 小地区名(To)の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>小地区名(To)</returns>
        public static string GetShochikuToNm(ITodofukenDropDownList model)
        {
            return GetShochikuNm(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCdTo);
        }

        /// <summary>
        /// 小地区コード名(To)の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>小地区コード名(To)</returns>
        public static string GetShochikuToCdNm(ITodofukenDropDownList model)
        {
            return GetShochikuCdNm(model.TodofukenCd, model.KumiaitoCd, model.DaichikuCd, model.ShochikuCdTo);
        }

        /// <summary>
        /// 小地区名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <param name="shochikuCd">小地区コード</param>
        /// <returns>小地区名</returns>
        public static string GetShochikuNm(string todofukenCd, string kumiaitoCd, string daichikuCd, string shochikuCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(daichikuCd) ||
                string.IsNullOrEmpty(shochikuCd))
            {
                return string.Empty;
            }

            MShochikuNm mNicShochiku = GetShochiku(todofukenCd, kumiaitoCd, daichikuCd, shochikuCd);

            if (mNicShochiku == null)
            {
                return string.Empty;
            }
            else
            {
                return mNicShochiku.ShochikuNm;
            }

        }

        /// <summary>
        /// 小地区コード名の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <param name="shochikuCd">小地区コード</param>
        /// <returns>小地区コード名</returns>
        public static string GetShochikuCdNm(string todofukenCd, string kumiaitoCd, string daichikuCd, string shochikuCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd) ||
                string.IsNullOrEmpty(daichikuCd) ||
                string.IsNullOrEmpty(shochikuCd))
            {
                return string.Empty;
            }

            MShochikuNm mNicShochiku = GetShochiku(todofukenCd, kumiaitoCd, daichikuCd, shochikuCd);

            if (mNicShochiku == null)
            {
                return string.Empty;
            }
            else
            {
                return shochikuCd + CoreConst.SEPARATOR + mNicShochiku.ShochikuNm;
            }

        }

        /// <summary>
        /// 小地区マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <returns>小地区マスタデータリスト</returns>
        public static List<MShochikuNm> GetShochikuList(string todofukenCd, string kumiaitoCd, string daichikuCd)
        {
            List<MShochikuNm> result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                logger.Info("名称M小地区マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd +
                    "、大地区コード：" + daichikuCd + " ）");
                result = db.MShochikuNms
                    .OrderBy(a => a.ShochikuCd)
                    .Where(a => a.TodofukenCd == todofukenCd &&
                                a.KumiaitoCd == kumiaitoCd &&
                                a.DaichikuCd == daichikuCd)
                    .AsEnumerable().Select(m => new MShochikuNm
                    {
                        ShochikuCd = m.ShochikuCd,
                        ShochikuNm = m.ShochikuNm
                    }).ToList();
            }

            return result;
        }

        /// <summary>
        /// 小地区マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <param name="shochikuCd">小地区コード</param>
        /// <returns>小地区マスタデータ</returns>
        public static MShochikuNm GetShochiku(string todofukenCd, string kumiaitoCd, string daichikuCd, string shochikuCd)
        {
            MShochikuNm result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                logger.Info("名称M小地区マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード："+ kumiaitoCd + 
                    "、大地区コード：" + daichikuCd + "、小地区コード：" + shochikuCd + " ）");
                result = db.MShochikuNms
                    .OrderBy(a => a.ShochikuCd)
                    .Where(a => a.TodofukenCd == todofukenCd &&
                                a.KumiaitoCd == kumiaitoCd &&
                                a.DaichikuCd == daichikuCd &&
                                a.ShochikuCd == shochikuCd)
                    .FirstOrDefault();
            }
            return result;
        }
    }
}