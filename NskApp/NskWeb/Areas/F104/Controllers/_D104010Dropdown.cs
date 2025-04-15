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

namespace NskWeb.Areas.F104._D104010Dropdown
{
    /// <summary>
    /// NSK_102010D_危険段階データ取込（危険段階別料率）（テキスト）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/01/08
    /// 作成者：
    /// </remarks>
    public static class _D104010Dropdown
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ドロップダウン作成メソッド(解除理由名称)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="sishoCd">支所コード</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent KaijyoReasonDropDownListFor<TModel, TResult>(
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

            // 解除理由名称リストの取得
            List<M10150解除理由名称> reasonList = GetReasonList(todofukenCd, kumiaitoCd, sishoCd);

            // 選択リストを生成
            var selectList = reasonList.Select(reason => new SelectListItem
            {
                Value = reason.解除理由コード,
                Text = reason.解除理由コード + CoreConst.SEPARATOR + reason.解除理由名称,
                Selected = reason.解除理由コード == selectedValue
            });

            // ドロップダウンリストを生成
            return htmlHelper.DropDownListFor(expression, selectList, "", htmlAttributes);
        }

        /// <summary>
        /// 解除理由名称マスタデータの取得メソッド。
        /// </summary>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>解除理由名称マスタリスト</returns>
        public static List<M10150解除理由名称> GetReasonList(string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            List<M10150解除理由名称> result;

            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , todofukenCd
                , kumiaitoCd
                , shishoCd);

            using (NskAppContext db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                logger.Info("解除理由名称マスタデータを取得する。");
                result = db.M10150解除理由名称s
                    .AsEnumerable().Select(m => new M10150解除理由名称
                    {
                        解除理由コード = m.解除理由コード,
                        解除理由名称 = m.解除理由名称
                    }).ToList();
            }
            return result;
        }
    }
}