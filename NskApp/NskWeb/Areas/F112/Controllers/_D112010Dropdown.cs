using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Pager;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;

namespace NskWeb.Areas.F104._D112010Dropdown
{
    /// <summary>
    /// NSK_102010D_危険段階データ取込（危険段階別料率）（テキスト）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/01/08
    /// 作成者：
    /// </remarks>
    public static class _D112010Dropdown
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ドロップダウン作成メソッド(区分種別)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent UkeireTaisyoNameDropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string todofukenCd,
            string kumiaitoCd,
            string sishoCd,
            object htmlAttributes)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            // モデルの現在値を取得
            var selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model)?.ToString();

            // 支所情報リストの取得
            List<M00190大量データ対象データ> reasonList = GetTaisyoDataist(todofukenCd, kumiaitoCd, sishoCd);

            // 選択リストを生成
            var selectList = reasonList.Select(reason => new SelectListItem
            {
                Value = reason.対象データ区分 +","+ reason.受入対象データ名称,
                Text = reason.受入対象データ名称,
                Selected = reason.受入対象データ名称 == selectedValue
            });

            // ドロップダウンリストを生成
            return htmlHelper.DropDownListFor(expression, selectList, "test", htmlAttributes);
        }

        /// <summary>
        /// ドロップダウン作成メソッド(区分種別)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent UkeireTaisyoCdDropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string todofukenCd,
            string kumiaitoCd,
            string sishoCd,
            object htmlAttributes)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            // モデルの現在値を取得
            var selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model)?.ToString();

            // 支所情報リストの取得
            List<M00190大量データ対象データ> reasonList = GetTaisyoDataist(todofukenCd, kumiaitoCd, sishoCd);

            // 選択リストを生成
            var selectList = reasonList.Select(reason => new SelectListItem
            {
                Value = reason.対象データ区分,
                Text = reason.受入対象データ名称,
                Selected = reason.対象データ区分 == selectedValue
            });

            // ドロップダウンリストを生成
            return htmlHelper.DropDownListFor(expression, selectList, "test", htmlAttributes);
        }

        /// <summary>
        /// ドロップダウン作成メソッド(区分種別)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent SishoCdDropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string todofukenCd,
            string kumiaitoCd,
            string sishoCd,
            object htmlAttributes)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            // モデルの現在値を取得
            var selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model)?.ToString();

            // 支所情報リストの取得
            List<VShishoNm> reasonList = GetSishoCdList(todofukenCd, kumiaitoCd, sishoCd);

            // 選択リストを生成
            var selectList = reasonList.Select(reason => new SelectListItem
            {
                Value = reason.ShishoCd,
                Text = reason.ShishoCd + CoreConst.SEPARATOR + reason.ShishoNm,
                Selected = reason.ShishoCd == selectedValue
            });

            // ドロップダウンリストを生成
            return htmlHelper.DropDownListFor(expression, selectList, "test", htmlAttributes);
        }

        /// <summary>
        /// ドロップダウン作成メソッド(区分種別)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent UkeireUserIdInfoDropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string todofukenCd,
            string kumiaitoCd,
            string sishoCd,
            string userId,
            object htmlAttributes)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            // モデルの現在値を取得
            var selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model)?.ToString();
            // 支所情報リストの取得
            List<T01060大量データ受入履歴> reasonList = GetUkeireRirekiList(todofukenCd, kumiaitoCd, sishoCd, userId);

            // 選択リストを生成
            var selectList = reasonList.Select(reason => new SelectListItem
            {
                Value = reason.登録ユーザid,
                Text = reason.登録ユーザid ,
                Selected = reason.登録ユーザid == selectedValue
            });
            logger.Debug("reasonList.Count");
            logger.Debug(reasonList.Count);

            // ドロップダウンリストを生成
            return htmlHelper.DropDownListFor(expression, selectList, "", htmlAttributes);
        }

        /// <summary>
        /// ドロップダウン作成メソッド(区分種別)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent TorikomiUserIdInfoDropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string todofukenCd,
            string kumiaitoCd,
            string sishoCd,
            string userId,
            object htmlAttributes)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            // モデルの現在値を取得
            var selectedValue = userId;

            // 支所情報リストの取得
            List<T01070大量データ取込履歴> reasonList = GetTorikomiList(todofukenCd, kumiaitoCd, sishoCd, userId);

            // 選択リストを生成
            var selectList = reasonList.Select(reason => new SelectListItem
            {
                Value = reason.登録ユーザid,
                Text = reason.登録ユーザid,
                Selected = reason.登録ユーザid == selectedValue
            });

            // ドロップダウンリストを生成
            return htmlHelper.DropDownListFor(expression, selectList, "test", htmlAttributes);
        }



        /// <summary>
        /// 支所マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>支所マスタリスト</returns>
        public static List<M00190大量データ対象データ> GetTaisyoDataist(string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            List<M00190大量データ対象データ> result;
            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , todofukenCd
                , kumiaitoCd
                , shishoCd);

            using (NskAppContext db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                result = db.M00190大量データ対象データs
                        .Where(t => t.業務区分 == "1"         // 「1：引受」
                                 && t.受入バッチid != null
                                 && t.取込バッチid != null)
                        .OrderBy(t => t.対象データ区分)         // 対象データ区分で昇順ソート
                        .Select(t => new M00190大量データ対象データ
                        {
                            受入対象データ名称 = t.受入対象データ名称,
                            対象データ区分 = t.対象データ区分
                        })
                        .ToList();
            }

            return result;
        }

        /// <summary>
        /// 支所マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>支所マスタリスト</returns>
        public static List<VShishoNm> GetSishoCdList(string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            List<VShishoNm> result;

            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , todofukenCd
                , kumiaitoCd
                , shishoCd);
            
            using (NskAppContext db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                logger.Info("名称M支所マスタデータを取得する。（都道府県コード：" + todofukenCd + "、組合等コード：" + kumiaitoCd + " ）");
                result = db.VShishoNms
                        .Where(t => t.KumiaitoCd == kumiaitoCd)
                        .OrderBy(t => t.ShishoCd)
                        .Select(t => new VShishoNm
                        {
                            ShishoCd = t.ShishoCd,
                            ShishoNm = t.ShishoNm
                        })
                        .ToList();
            }

            return result;
        }

        /// <summary>
        /// 支所マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>支所マスタリスト</returns>
        public static List<T01060大量データ受入履歴> GetUkeireRirekiList(string todofukenCd, string kumiaitoCd, string shishoCd, string userId)
        {
            List<T01060大量データ受入履歴> result;

            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , todofukenCd
                , kumiaitoCd
                , shishoCd);

            using (NskAppContext db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                result = db.T01060大量データ受入履歴s
                        .Where(t => t.組合等コード == kumiaitoCd)
                        .Select(t => new T01060大量データ受入履歴
                        {
                            登録ユーザid = t.登録ユーザid
                        })
                        .Distinct()
                        .OrderBy(t => t.登録ユーザid)
                        .ToList();
            }
            logger.Debug("@登録ユーザid@@@@@@@@@@@@@@@@@@@");
            logger.Debug(userId);
            logger.Debug("@登録ユーザid@@@@@@@@@@@@@@@@@@@");
            if (!result.Any(r => r.登録ユーザid == userId))
            {
                result.Add(new T01060大量データ受入履歴 { 登録ユーザid = userId });
                result = result.OrderBy(r => r.登録ユーザid).ToList();
            }
            return result;
        }

        /// <summary>
        /// 支所マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>支所マスタリスト</returns>
        public static List<T01070大量データ取込履歴> GetTorikomiList(string todofukenCd, string kumiaitoCd, string shishoCd, string userId)
        {
            List<T01070大量データ取込履歴> result;

            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , todofukenCd
                , kumiaitoCd
                , shishoCd);

            using (NskAppContext db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                result = db.T01070大量データ取込履歴s
                        .Where(t => t.組合等コード == kumiaitoCd)
                        .Select(t => new T01070大量データ取込履歴
                        {
                            登録ユーザid = t.登録ユーザid
                        })
                        .Distinct()
                        .OrderBy(id => id)
                        .ToList();
            }
            if (!result.Any(r => r.登録ユーザid == userId))
            {
                result.Add(new T01070大量データ取込履歴 { 登録ユーザid = userId });
                result = result.OrderBy(r => r.登録ユーザid).ToList();
            }
            return result;
        }
    }
}