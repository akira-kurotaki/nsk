using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 組合等のユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/22
    /// 作成者：Rou I
    /// </remarks>
    public static class KumiaitoUtil
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 組合等ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>組合等選択リスト</returns>
        public static SelectList GetSelectList(ITodofukenDropDownList model)
        {
            return GetSelectList(model.TodofukenCd, model.KumiaitoCd);
        }

        /// <summary>
        /// 組合等ドロップダウンリストの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>組合等選択リスト</returns>
        public static SelectList GetSelectList(string todofukenCd, string kumiaitoCd)
        {
            if (string.IsNullOrEmpty(todofukenCd))
            {
                return new SelectList(new List<SelectListItem>(), "Value", "Text");
            }

            List<SelectListItem> selectListItem = new List<SelectListItem>();

            List<MKumiaito> kumiaitoList = GetKumiaitoList(todofukenCd);

            foreach (var item in kumiaitoList)
            {
                selectListItem.Add(new SelectListItem { Value = item.KumiaitoCd, Text = item.KumiaitoCd + CoreConst.SEPARATOR + item.KumiaitoRnm });
            }

            if (string.IsNullOrEmpty(kumiaitoCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", kumiaitoCd);
            }
        }

        /// <summary>
        /// 組合等正式名称の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>組合等正式名称</returns>
        public static string GetKumiaitoNm(ITodofukenDropDownList model)
        {
            return GetKumiaitoNm(model.TodofukenCd, model.KumiaitoCd);
        }

        /// <summary>
        /// 組合等コード正式名称の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>組合等コード正式名称</returns>
        public static string GetKumiaitoCdNm(ITodofukenDropDownList model)
        {
            return GetKumiaitoCdNm(model.TodofukenCd, model.KumiaitoCd);
        }

        /// <summary>
        /// 組合等略称の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>組合等略称</returns>
        public static string GetKumiaitoRnm(ITodofukenDropDownList model)
        {
            return GetKumiaitoRnm(model.TodofukenCd, model.KumiaitoCd);
        }

        /// <summary>
        /// 組合等コード略称の取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>組合等コード略称</returns>
        public static string GetKumiaitoCdRnm(ITodofukenDropDownList model)
        {
            return GetKumiaitoCdRnm(model.TodofukenCd, model.KumiaitoCd);
        }

        /// <summary>
        /// 組合等正式名称の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>組合等正式名称</returns>
        public static string GetKumiaitoNm(string todofukenCd, string kumiaitoCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd))
            {
                return string.Empty;
            }

            List<MKumiaito> kumiaitoList = GetKumiaitoList(todofukenCd);

            MKumiaito mNicKumiaito = kumiaitoList.Where(a => a.KumiaitoCd == kumiaitoCd).FirstOrDefault();

            if (mNicKumiaito == null)
            {
                return string.Empty;
            }
            else
            {
                return mNicKumiaito.KumiaitoNm;
            }

        }

        /// <summary>
        /// 組合等コード正式名称の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>組合等コード正式名称</returns>
        public static string GetKumiaitoCdNm(string todofukenCd, string kumiaitoCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd))
            {
                return string.Empty;
            }

            List<MKumiaito> kumiaitoList = GetKumiaitoList(todofukenCd);

            MKumiaito mNicKumiaito = kumiaitoList.Where(a => a.KumiaitoCd == kumiaitoCd).FirstOrDefault();

            if (mNicKumiaito == null)
            {
                return string.Empty;
            }
            else
            {
                return kumiaitoCd + CoreConst.SEPARATOR + mNicKumiaito.KumiaitoNm;
            }

        }

        /// <summary>
        /// 組合等略称の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>組合等略称</returns>
        public static string GetKumiaitoRnm(string todofukenCd, string kumiaitoCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd))
            {
                return string.Empty;
            }

            MKumiaito mNicKumiaito = GetKumiaito(todofukenCd, kumiaitoCd);

            if (mNicKumiaito == null)
            {
                return string.Empty;
            }
            else
            {
                return mNicKumiaito.KumiaitoRnm;
            }

        }


        /// <summary>
        /// 組合等コード略称の取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>組合等コード略称</returns>
        public static string GetKumiaitoCdRnm(string todofukenCd, string kumiaitoCd)
        {
            if (string.IsNullOrEmpty(todofukenCd) ||
                string.IsNullOrEmpty(kumiaitoCd))
            {
                return string.Empty;
            }

            MKumiaito mNicKumiaito = GetKumiaito(todofukenCd, kumiaitoCd);

            if (mNicKumiaito == null)
            {
                return string.Empty;
            }
            else
            {
                return kumiaitoCd + CoreConst.SEPARATOR + mNicKumiaito.KumiaitoRnm;
            }

        }

        /// <summary>
        /// 組合等マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <returns>組合等マスタリスト</returns>
        public static List<MKumiaito> GetKumiaitoList(string todofukenCd)
        {
            List<MKumiaito> result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                logger.Info("組合等マスタデータを取得する。（都道府県コード：" + todofukenCd + " ）");
                result = db.MKumiaitos
                    .OrderBy(a => a.TodofukenCd)
                    .OrderBy(a => a.KumiaitoCd)
                    .Where(a => a.TodofukenCd == todofukenCd)
                    .AsEnumerable().Select(m => new MKumiaito
                    {
                        TodofukenCd = m.TodofukenCd,
                        KumiaitoCd = m.KumiaitoCd,
                        KumiaitoNm = m.KumiaitoNm,
                        KumiaitoRnm = m.KumiaitoRnm
                    }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 組合等マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>組合等マスタ</returns>
        public static MKumiaito GetKumiaito(string todofukenCd, string kumiaitoCd)
        {
            MKumiaito result;
            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                logger.Info("組合等マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd + " ）");
                result = db.MKumiaitos
                    .OrderBy(a => a.TodofukenCd)
                    .OrderBy(a => a.KumiaitoCd)
                    .Where(a => a.TodofukenCd == todofukenCd &&
                            a.KumiaitoCd == kumiaitoCd)
                    .FirstOrDefault();
            }
            return result;
        }
    }
}