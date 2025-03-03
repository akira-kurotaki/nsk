using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Pager;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;

namespace CoreLibrary.Core.Extensions
{
    /// <summary>
    /// HTMLヘルパー拡張クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/10
    /// 作成者：Rou I
    /// </remarks>
    public static class HtmlHelpersExtensions
    {
        /// <summary>
        /// ドロップダウン作成メソッド(区分種別、表示モード)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="displayMode">表示モード</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent DropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string kbnSbt,
            CoreConst.DisplayMode displayMode,
            string optionLabel,
            object htmlAttributes)
        {
            object selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            IEnumerable<SelectListItem> selectList = selectedValue == null ?
                HanyokubunUtil.GetSelectList(kbnSbt, displayMode) :
                HanyokubunUtil.GetSelectList(kbnSbt, displayMode, selectedValue);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);

        }

        /// <summary>
        /// ドロップダウン作成メソッド(区分種別、表示モード)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="displayMode">表示モード</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent DropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string kbnSbt,
            CoreConst.DisplayMode displayMode,
            object htmlAttributes)
        {
            object selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            IEnumerable<SelectListItem> selectList = selectedValue == null ?
                HanyokubunUtil.GetSelectList(kbnSbt, displayMode) :
                HanyokubunUtil.GetSelectList(kbnSbt, displayMode, selectedValue);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel: null, htmlAttributes);
        }

        /// <summary>
        /// ドロップダウン作成メソッド(区分種別)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent DropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string kbnSbt,
            string optionLabel,
            object htmlAttributes)
        {
            object selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            // 表示モード：コード値
            IEnumerable<SelectListItem> selectList = selectedValue == null ?
                HanyokubunUtil.GetSelectList(kbnSbt, CoreConst.DisplayMode.KbnNm) :
                HanyokubunUtil.GetSelectList(kbnSbt, CoreConst.DisplayMode.KbnNm, selectedValue);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
        }

        /// <summary>
        /// ドロップダウン作成メソッド(区分種別)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent DropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string kbnSbt,
            object htmlAttributes)
        {
            object selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            // 表示モード：コード値
            IEnumerable<SelectListItem> selectList = selectedValue == null ?
                HanyokubunUtil.GetSelectList(kbnSbt, CoreConst.DisplayMode.KbnNm) :
                HanyokubunUtil.GetSelectList(kbnSbt, CoreConst.DisplayMode.KbnNm, selectedValue);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel: null, htmlAttributes);
        }

        /// <summary>
        /// 年度ドロップダウン作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent NendoDropDownListFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string optionLabel,
            object htmlAttributes)
        {
            object selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            IEnumerable<SelectListItem> selectList = selectedValue == null ?
                NendoUtil.GetSelectList(string.Empty) :
                NendoUtil.GetSelectList(selectedValue.ToString());

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
        }

        /// <summary>
        /// 年度ドロップダウン（表示範囲指定）作成ヘルパー
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <param name="rangeKbn">範囲指定区分値("0": 指定した年度までをリスト化する，"1":指定した年度以降をリスト化する)</param>
        /// <param name="rangeNendo">範囲指定年度</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent NendoDropDownListWithRangeFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            string optionLabel,
            object htmlAttributes,
            string rangeKbn,
            string rangeNendo)
        {
            object selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);
            IEnumerable<SelectListItem> selectList = selectedValue == null ?
                NendoUtil.GetSelectListWithRange(string.Empty, rangeKbn, rangeNendo) :
                NendoUtil.GetSelectListWithRange(selectedValue.ToString(), rangeKbn, rangeNendo);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
        }

        /// <summary>
        /// 品目ドロップダウン作成メソッド(検索条件用)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        //public static IHtmlContent HimmokuDropDownListFor<TModel, TResult>(
        //    this IHtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TResult>> expression,
        //    string optionLabel,
        //    object htmlAttributes)
        //{
        //    object selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);
        //    IEnumerable<SelectListItem> selectList = new SelectList(new List<SelectListItem>(), "Value", "Text");

        //    if (SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, htmlHelper.ViewContext.HttpContext) != null)
        //    {
        //        Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, htmlHelper.ViewContext.HttpContext);

        //        if (((int)CoreConst.SystemRiyoKbn.Shisho).ToString().Equals(syokuin.SystemRiyoKbn))
        //        {
        //            // 支所の場合
        //            var param = new HimmokuParam()
        //            {
        //                TodofukenCd = syokuin.TodofukenCd,
        //                KumiaitoCd = syokuin.KumiaitoCd,
        //                ShishoCd = syokuin.ShishoCd
        //            };

        //            var himmokuSelectList = HimmokuDropDownListUtil.GetHimmokuSelectList(
        //                                                            param,
        //                                                            HimmokuDropDownListUtil.DisplayMode.Nochikusan);

        //            selectList = selectedValue == null ? new SelectList(himmokuSelectList, "Value", "Text") :
        //                                                 new SelectList(himmokuSelectList, "Value", "Text", selectedValue);
        //        }
        //        else
        //        {
        //            // ビルドエラー 2018/05/15
        //            //// 支所以外の場合
        //            //selectList = selectedValue == null ? NosambutsutoHimmokuUtil.GetSelectList(string.Empty) :
        //            //                                     NosambutsutoHimmokuUtil.GetSelectList(selectedValue.ToString());
        //        }
        //    }

        //    if (htmlHelper == null)
        //    {
        //        throw new ArgumentNullException(nameof(htmlHelper));
        //    }

        //    if (expression == null)
        //    {
        //        throw new ArgumentNullException(nameof(expression));
        //    }

        //    return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
        //}

        /// <summary>
        /// 種類ドロップダウン作成メソッド(検索条件用)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        //public static IHtmlContent ShuruiDropDownListFor<TModel, TResult>(
        //    this IHtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TResult>> expression,
        //    string optionLabel,
        //    object htmlAttributes)
        //{
        //    object selectedValue = expression.Compile().Invoke(htmlHelper.ViewData.Model);
        //    IEnumerable<SelectListItem> selectList = new SelectList(new List<SelectListItem>(), "Value", "Text");

        //    if (SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, htmlHelper.ViewContext.HttpContext) != null)
        //    {
        //        Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, htmlHelper.ViewContext.HttpContext);

        //        if (((int)CoreConst.SystemRiyoKbn.Shisho).ToString().Equals(syokuin.SystemRiyoKbn))
        //        {
        //            // 支所の場合
        //            var param = new ShuruiParam()
        //            {
        //                TodofukenCd = syokuin.TodofukenCd,
        //                KumiaitoCd = syokuin.KumiaitoCd,
        //                ShishoCd = syokuin.ShishoCd
        //            };

        //            var shuruiSelectList = ShuruiDropDownListUtil.GetShuruiSelectList(
        //                                                            param,
        //                                                            ShuruiDropDownListUtil.DisplayMode.Nochikusan);

        //            selectList = selectedValue == null ? new SelectList(shuruiSelectList, "Value", "Text") :
        //                                                new SelectList(shuruiSelectList, "Value", "Text", selectedValue);
        //        }
        //        else
        //        {
        //            // 支所以外の場合
        //            var selectListItem = new List<SelectListItem>();

        //            using (JigyoCommonContext db = new JigyoCommonContext())
        //            {
        //                var result = (
        //                                 from a in db.MNcShuruis
        //                                 where a.DeleteFlg == "0"
        //                                 select new
        //                                 {
        //                                     a.NcShuruiCd,
        //                                     a.NcShuruiNm,
        //                                     a.NochikusanKbn
        //                                 }
        //                            );

        //                result = result.Where(a => a.NochikusanKbn == "1"
        //                                        || a.NochikusanKbn == "2"
        //                                        || a.NochikusanKbn == "999")
        //                               .OrderBy(a => a.NochikusanKbn).ThenBy(a => a.NcShuruiCd);

        //                foreach (var item in result.ToList())
        //                {
        //                    selectListItem.Add(new SelectListItem
        //                    {
        //                        Value = item.NcShuruiCd,
        //                        Text = item.NcShuruiCd + CoreConst.SEPARATOR + item.NcShuruiNm
        //                    });
        //                }
        //            }
        //            if (selectedValue == null)
        //            {
        //                selectList = new SelectList(selectListItem, "Value", "Text");
        //            }
        //            else
        //            {
        //                selectList = new SelectList(selectListItem, "Value", "Text", selectedValue.ToString());
        //            }
        //        }
        //    }

        //    if (htmlHelper == null)
        //    {
        //        throw new ArgumentNullException(nameof(htmlHelper));
        //    }

        //    if (expression == null)
        //    {
        //        throw new ArgumentNullException(nameof(expression));
        //    }

        //    return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
        //}

        /// <summary>
        /// ページャー作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="actionName">アクション メソッドの名前</param>
        /// <param name="pagination">ページング用モデル</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Pager(this IHtmlHelper htmlHelper, string actionName, IPagination pagination)
        {
            return Pager(htmlHelper, actionName, null, pagination);
        }

        /// <summary>
        /// ページャー作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="actionName">アクション メソッドの名前</param>
        /// <param name="functionName">クリックファンクション名</param>
        /// <param name="pagination">ページング用モデル</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Pager(this IHtmlHelper htmlHelper, string actionName, string functionName, IPagination pagination)
        {
            //string localPath = htmlHelper.ViewContext.HttpContext.Request.Url.LocalPath;
            string localPath = htmlHelper.ViewContext.HttpContext.Request.PathBase + htmlHelper.ViewContext.HttpContext.Request.Path;
            string[] segments = localPath.Split('/');

            if (segments.Length < 3)
            {
                return new HtmlString(string.Empty);
            }
            else
            {
                // localPath の中から/Fxx/Dxxxx/ActionNameを抜き出す
                // 例）/F00/D1234/Pager → /F00/D1234/Pager
                // 　  /SynWeb/F00/D1234/Pager/2 → /SynWeb/F00/D1234/Pager

                // segmentsの中からFxx/Dxxxx の位置を探す
                StringBuilder path = new StringBuilder();
                for (int i = 1; i < segments.Length - 1; i++)
                {
                    if (segments[i].Length > 0 && "F".Equals(segments[i].Substring(0, 1)) &&
                        segments[i + 1].Length > 0 && "D".Equals(segments[i + 1].Substring(0, 1)))
                    {
                        // segments のi番目にFxx, i+1番目にDxxがある
                        for (int j = 1; j <= i + 1; j++)
                        {
                            path.Append("/").Append(segments[j]);
                        }
                        path.Append("/").Append(actionName);
                        break;
                    }
                }

                if (string.IsNullOrEmpty(path.ToString()))
                {
                    return new HtmlString(string.Empty);
                }
                else
                {
                    Paging paging = new Paging(path.ToString(), pagination);
                    if (string.IsNullOrEmpty(functionName))
                    {
                        return new HtmlString(paging.ToString());
                    }
                    else
                    {
                        return new HtmlString(paging.ToString(functionName));
                    }
                }
            }

        }

        /// <summary>
        /// パンくず作成メソッド
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Breadcrumb(this IHtmlHelper htmlHelper)
        {
            var htmlStr = new StringBuilder();
            //var screenId = Path.GetFileNameWithoutExtension(((BuildManagerCompiledView)htmlHelper.ViewContext.View).ViewPath);
            //var screenId = Path.GetFileNameWithoutExtension(htmlHelper.ViewContext.View.Path);
            var screenId = htmlHelper.ViewContext.RouteData.Values["controller"].ToString();
            var screenNm = ScreenUtil.Get(screenId);
            //var uniqueId = HttpUtility.UrlEncode(htmlHelper.ViewContext.HttpContext.Request.Query[CoreConst.CHILD_WINDOW_ID]) ?? string.Empty;
            var uniqueId = WebUtility.HtmlEncode(htmlHelper.ViewContext.HttpContext.Request.Query[CoreConst.CHILD_WINDOW_ID]) ?? string.Empty;
            if (htmlHelper.ViewData.Model is CoreViewModel && string.Empty.Equals(uniqueId))
            {
                var model = htmlHelper.ViewData.Model as CoreViewModel;
                uniqueId = model.SelfWindowId ?? string.Empty;
            }
            var sessionKey = CoreConst.SESS_BREADCRUMB + uniqueId;
            //var screenIdList = SessionUtil.Get(sessionKey) as List<string> ?? new List<string>();
            var screenIdList = SessionUtil.Get<List<string>>(sessionKey, htmlHelper.ViewContext.HttpContext) ?? new List<string>();

            // DB登録済み画面のみパンくず更新
            if (!string.Empty.Equals(screenNm))
            {
                var isLeftSubMenu = htmlHelper.ViewContext.TempData[CoreConst.IS_LEFT_SUB_MENU] as bool? ?? false;
                var deleteIndex = screenIdList.IndexOf(screenId);
                if (isLeftSubMenu)
                {
                    deleteIndex = 0 == deleteIndex ? 0 : 1;
                }

                if (0 <= deleteIndex && deleteIndex <= screenIdList.Count)
                {
                    screenIdList.RemoveRange(deleteIndex, screenIdList.Count - deleteIndex);
                }
                screenIdList.Add(screenId);
                //SessionUtil.Set(sessionKey, screenIdList);
                SessionUtil.Set(sessionKey, screenIdList, htmlHelper.ViewContext.HttpContext);
            }

            // パンくずリスト作成
            htmlStr.Append("<dt>\n");
            htmlStr.Append("<ul class=\"location_box\">\n");
            for (var i = 0; i < screenIdList.Count; ++i)
            {
                screenNm = ScreenUtil.Get(screenIdList[i]);
                if (!string.Empty.Equals(screenNm))
                {
                    if (0 != i)
                    {
                        htmlStr.Append("<li> &gt; </li>\n");
                    }
                    htmlStr.Append(String.Format("<li>{0}</li>\n", screenNm));
                }
            }
            htmlStr.Append("</ul>\n");
            htmlStr.Append("</dt>\n");

            return new HtmlString(htmlStr.ToString());
        }

        /// <summary>
        /// ドロップダウン作成メソッド(列挙型)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent DropDownListFor<TModel, TEnum>(
                        this IHtmlHelper<TModel> htmlHelper,
                        Expression<Func<TModel, TEnum>> expression,
                        object htmlAttributes)
        {
            //var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            // .NET CORE6 MVC
            // --------------------------------------------
            // ラムダ式からモデルのメタデータと値を取得する
            // --------------------------------------------
            // ModelexpressionProviderをServiceProviderから取得する
            ModelExpressionProvider? modelExpressionProvider
                = (ModelExpressionProvider?)htmlHelper.ViewContext?.HttpContext?
                    .RequestServices?.GetService(typeof(IModelExpressionProvider));
            // ラムダ式からモデルのメタデータを取得する
            var metadata = modelExpressionProvider?.CreateModelExpression(htmlHelper.ViewData, expression);
            if (metadata == null)
            {
                throw new NullReferenceException("metadata do not get.");
            }

            var enumType = GetNonNullableModelType(metadata?.Metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();

            var items =
                from value in values
                select new SelectListItem
                {
                    Text = (value as Enum).ToDescription(),
                    Value = value.ToString(),
                    Selected = value.Equals(metadata.Model)
                };

            if ((bool)(metadata?.Metadata.IsNullableValueType))
            {
                var emptyItem = new List<SelectListItem>
                {
                    new SelectListItem {Text = string.Empty, Value = string.Empty}
                };
                items = emptyItem.Concat(items);
            }

            //return SelectExtensions.DropDownListFor(htmlHelper, expression, items, htmlAttributes);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return htmlHelper.DropDownListFor(expression, items, optionLabel: null, htmlAttributes);
        }

        /// <summary>
        /// Null許容型のデータ取得メソッド。
        /// </summary>
        /// <param name="modelMetadata">モデルタイプ</param>
        /// <returns>タイプ</returns>
        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        /// <summary>
        /// 列挙型のDescription属性取得メソッド。
        /// </summary>
        /// <param name="value">列挙型データ/param>
        /// <returns>変換後の文字列</returns>
        public static string ToDescription(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var attributes = (DescriptionAttribute[])value.GetType().GetField(
                Convert.ToString(value)).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : Convert.ToString(value);
        }

        /// <summary>
        /// メッセージエリアヘルパー。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="validationMessage">指定したフィールドにエラーがある場合に表示するメッセージ</param>
        /// <param name="htmlAttributes">この要素の HTML 属性を格納するオブジェクト</param>
        /// <returns>モデル オブジェクトが有効でクライアント側の検証が無効な場合は null。それ以外の場合は、エラー メッセージが含まれる tag 要素</returns>
        public static IHtmlContent MessageAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, IDictionary<string, object> htmlAttributes, string tag)
        {
            //HtmlString html = ValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage, htmlAttributes);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            IHtmlContent html = htmlHelper.ValidationMessageFor(expression, validationMessage, htmlAttributes, tag);

            return MakeMessage(htmlHelper, expression, html);
        }

        /// <summary>
        /// メッセージエリアヘルパー。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="validationMessage">指定したフィールドにエラーがある場合に表示するメッセージ</param>
        /// <param name="htmlAttributes">この要素の HTML 属性を格納するオブジェクト</param>
        /// <returns>プロパティまたはオブジェクトが有効な場合は、空の文字列。それ以外の場合は、エラー メッセージを含む span 要素</returns>
        public static IHtmlContent MessageAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, IDictionary<string, object> htmlAttributes)
        {
            //HtmlString html = ValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage, htmlAttributes);

            IHtmlContent html = HtmlHelperValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage, htmlAttributes);

            return MakeMessage(htmlHelper, expression, html);
        }

        /// <summary>
        /// メッセージエリアヘルパー。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="validationMessage">指定したフィールドにエラーがある場合に表示するメッセージ</param>
        /// <param name="htmlAttributes">この要素の HTML 属性を格納するオブジェクト</param>
        /// <param name="tag">検証メッセージの HTML 要素のラッピングのために設定するタグ</param>
        /// <returns>モデル オブジェクトが有効でクライアント側の検証が無効な場合は null。それ以外の場合は、エラー メッセージが含まれる tag 要素</returns>
        public static IHtmlContent MessageAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes, string tag)
        {
            //HtmlString html = ValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage, htmlAttributes, tag);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            IHtmlContent html = htmlHelper.ValidationMessageFor(expression, validationMessage, htmlAttributes, tag);

            return MakeMessage(htmlHelper, expression, html);
        }

        /// <summary>
        /// メッセージエリアヘルパー。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="validationMessage">指定したフィールドにエラーがある場合に表示するメッセージ</param>
        /// <param name="tag">検証メッセージの HTML 要素のラッピングのために設定するタグ</param>
        /// <returns>モデル オブジェクトが有効でクライアント側の検証が無効な場合は null。それ以外の場合は、エラー メッセージが含まれる tag 要素</returns>
        public static IHtmlContent ValidatsageFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, string tag)
        {
            //HtmlString html = ValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage, tag);

            IHtmlContent html = HtmlHelperValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage, tag);

            return MakeMessage(htmlHelper, expression, html);
        }

        /// <summary>
        /// メッセージエリアヘルパー。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="validationMessage">指定したフィールドにエラーがある場合に表示するメッセージ</param>
        /// <returns>プロパティまたはオブジェクトが有効な場合は、空の文字列。それ以外の場合は、エラー メッセージを含む span 要素</returns>
        public static IHtmlContent MessageAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage)
        {
            //HtmlString html = ValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage);

            IHtmlContent html = HtmlHelperValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage);

            return MakeMessage(htmlHelper, expression, html);
        }

        /// <summary>
        /// メッセージエリアヘルパー。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <returns>プロパティまたはオブジェクトが有効な場合は、空の文字列。それ以外の場合は、エラー メッセージを含む span 要素</returns>
        public static IHtmlContent MessageAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            //HtmlString html = ValidationExtensions.ValidationMessageFor(htmlHelper, expression);

            IHtmlContent html = HtmlHelperValidationExtensions.ValidationMessageFor(htmlHelper, expression, message: null);

            return MakeMessage(htmlHelper, expression, html);
        }

        /// <summary>
        /// メッセージエリアヘルパー。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="validationMessage">指定したフィールドにエラーがある場合に表示するメッセージ</param>
        /// <param name="htmlAttributes">この要素の HTML 属性を格納するオブジェクト</param>
        /// <returns>プロパティまたはオブジェクトが有効な場合は、空の文字列。それ以外の場合は、エラー メッセージを含む span 要素</returns>
        public static IHtmlContent MessageAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, Object htmlAttributes)
        {
            //HtmlString html = ValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage, htmlAttributes);
            IHtmlContent html = HtmlHelperValidationExtensions.ValidationMessageFor(htmlHelper, expression, validationMessage, htmlAttributes);
            return MakeMessage(htmlHelper, expression, html);
        }

        /// <summary>
        /// ２番目以降のメッセージを追加する。
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        private static IHtmlContent MakeMessage<TModel, TProperty>(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IHtmlContent html)
        {
            string name = GetExpressionName(expression);
            foreach (var key in htmlHelper.ViewData.ModelState.Keys)
            {
                if (key == name)
                {
                    //if (htmlHelper.ViewData.ModelState[key].Errors.Count() > 1)
                    if (htmlHelper.ViewData.ModelState[key].Errors.Count > 1)
                    {
                        // ValidationMessageFor は１番目のメッセージしか設定されないので、２番目以降を追加する。
                        String allMessages = String.Empty;
                        for (int i = 0; i < htmlHelper.ViewData.ModelState[key].Errors.Count; i++)
                        {
                            if (i != 0)
                            {
                                allMessages += "<br />";
                            }
                            allMessages += htmlHelper.ViewData.ModelState[key].Errors[i].ErrorMessage;
                        }
                        using (var writer = new StringWriter())
                        {
                            //String htmlStr = html.ToHtmlString();
                            html.WriteTo(writer, HtmlEncoder.Default);
                            String htmlStr = writer.ToString();
                            var encodeMessage = HtmlEncoder.Default.Encode(htmlHelper.ViewData.ModelState[key].Errors[0].ErrorMessage);
                            htmlStr = htmlStr.Replace(encodeMessage, allMessages);
                            //html = HtmlString.Create(htmlStr);
                            html = new HtmlString(htmlStr);
                        }
                    }
                    break;
                }
            }
            return html;
        }

        /// <summary>
        /// expression.Body.Member.Name を取り出す。
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GetExpressionName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            Type bodyType = expression.Body.GetType();
            PropertyInfo memberPropertyInfo = bodyType.GetProperty("Member");
            object member = memberPropertyInfo.GetValue(expression.Body, null);
            Type memberType = memberPropertyInfo.GetType();
            PropertyInfo namePropertyInfo = memberType.GetProperty("Name");
            return namePropertyInfo.GetValue(member, null).ToString();
        }

        /// <summary>
        /// ダイアログ作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="title">タイトル</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Dialog(this IHtmlHelper htmlHelper, string divId, string title)
        {
            return Dialog(htmlHelper, divId, title, null, "閉じる");
        }

        /// <summary>
        /// ダイアログ作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="title">タイトル</param>
        /// <param name="button">ボタン表示文字</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Dialog(this IHtmlHelper htmlHelper, string divId, string title, string button)
        {
            return Dialog(htmlHelper, divId, title, null, button);
        }

        /// <summary>
        /// ダイアログ作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="button">ボタン表示文字</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Dialog(this IHtmlHelper htmlHelper, string divId, string title, string message, string button)
        {
            StringBuilder builder = new StringBuilder();

            //builder.Append(String.Format("\n<div class=\"modal\" id=\"{0}\" tabindex=\"-1\" data-backdrop=\"false\" aria-hidden=\"true\">\n", divId));
            builder.Append(String.Format("\n<div class=\"modal\" id=\"{0}\" tabindex=\"-1\" data-bs-backdrop=\"false\" aria-hidden=\"true\">\n", divId));
            builder.Append("    <div class=\"modal-dialog\">\n");
            builder.Append("        <div class=\"modal-content\">\n");

            builder.Append("            <div class=\"modal-header\">\n");
            //builder.Append("                <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\"><span>×</span></button>\n");
            builder.Append(String.Format("                <h4 class=\"modal-title\">{0}</h4>\n", title));
            builder.Append("                <button type=\"button\" class=\"btn-close btn-close-white\" data-bs-dismiss=\"modal\" aria-label=\"Close\"></button>\n");
            builder.Append("            </div>\n");

            builder.Append("            <div class=\"modal-body\">\n");
            if (!string.IsNullOrEmpty(message))
            {
                builder.Append(String.Format("                {0}\n", message));
            }
            builder.Append("            </div>\n");

            builder.Append("            <div class=\"modal-footer\">\n");
            //builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\" aria-hidden=\"true\">{0}</button>\n", button));
            builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-bs-dismiss=\"modal\" aria-hidden=\"true\">{0}</button>\n", button));
            builder.Append("            </div>\n");

            builder.Append("        </div>\n");
            builder.Append("    </div>\n");
            builder.Append("</div>\n");

            return new HtmlString(builder.ToString());
        }

        /// <summary>
        /// ダイアログ表示メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString ShowDialog(this IHtmlHelper htmlHelper, string divId)
        {
            return ShowDialog(htmlHelper, divId, null);
        }

        /// <summary>
        /// ダイアログ表示メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="message">メッセージ</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString ShowDialog(this IHtmlHelper htmlHelper, string divId, string message)
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(message))
            {
                builder.Append(String.Format("    $('#{0}').find('.modal-body').html({1});\n", divId, message));
            }

            builder.Append(String.Format("    $('#{0}').modal('show');\n", divId));

            return new HtmlString(builder.ToString());
        }

        /// <summary>
        /// コンファーム作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="clickId">クリックボタンID</param>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="button1Dismiss">button1の「data-bs-dismiss」出力有無</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Confirm(this IHtmlHelper htmlHelper, string divId, string clickId, string title, string message, bool button1Dismiss = true)
        {
            return Confirm(htmlHelper, divId, clickId, null, title, message, "OK", "キャンセル", button1Dismiss);
        }


        /// <summary>
        /// コンファーム作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="clickId">クリックボタンID</param>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="button1">ボタン表示文字1</param>
        /// <param name="button2">ボタン表示文字2</param>
        /// <param name="button1Dismiss">button1の「data-bs-dismiss」出力有無</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Confirm(this IHtmlHelper htmlHelper, string divId, string clickId, string title, string message, string button1, string button2, bool button1Dismiss = true)
        {
            return Confirm(htmlHelper, divId, clickId, null, title, message, button1, button2, button1Dismiss);
        }

        /// <summary>
        /// コンファーム作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="clickId1">クリックボタンID1</param>
        /// <param name="clickId2">クリックボタンID2</param>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="button1Dismiss">button1の「data-bs-dismiss」出力有無</param>
        /// <param name="button2Dismiss">button2の「data-bs-dismiss」出力有無</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Confirm(this IHtmlHelper htmlHelper, string divId, string clickId1, string clickId2, string title, string message, bool button1Dismiss = true, bool button2Dismiss = true)
        {
            return Confirm(htmlHelper, divId, clickId1, clickId2, null, title, message, "OK", "キャンセル", button1Dismiss, button2Dismiss);
        }

        /// <summary>
        /// コンファーム作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="clickId1">クリックボタンID1</param>
        /// <param name="clickId2">クリックボタンID2</param>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="button1">ボタン表示文字1</param>
        /// <param name="button2">ボタン表示文字2</param>
        /// <param name="button1Dismiss">button1の「data-bs-dismiss」出力有無</param>
        /// <param name="button2Dismiss">button2の「data-bs-dismiss」出力有無</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Confirm(this IHtmlHelper htmlHelper, string divId, string clickId1, string clickId2, string title, string message, string button1, string button2, bool button1Dismiss = true, bool button2Dismiss = true)
        {
            return Confirm(htmlHelper, divId, clickId1, clickId2, null, title, message, button1, button2, button1Dismiss, button2Dismiss);
        }

        /// <summary>
        /// コンファーム作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="clickId">クリックボタンID</param>
        /// <param name="paramId">パラメータID</param>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="button1">ボタン表示文字1</param>
        /// <param name="button2">ボタン表示文字2</param>
        /// <param name="button1Dismiss">button1の「data-bs-dismiss」出力有無</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Confirm(this IHtmlHelper htmlHelper, string divId, string clickId, string[] paramId, string title, string message, string button1, string button2, bool button1Dismiss = true)
        {
            StringBuilder builder = new StringBuilder();

            //builder.Append(String.Format("\n<div class=\"modal\" id=\"{0}\" tabindex=\"-1\" data-backdrop=\"false\" aria-hidden=\"true\">\n", divId));
            builder.Append(String.Format("\n<div class=\"modal\" id=\"{0}\" tabindex=\"-1\" data-bs-backdrop=\"false\" aria-labelledby=\"{0}TitleLabel\" aria-hidden=\"true\">\n", divId));
            if (paramId != null && paramId.Length > 0)
            {
                for (int index = 0; index < paramId.Length; index++)
                {
                    builder.Append(String.Format("    <input type=\"hidden\" id=\"{0}\" value=\"\"/>\n", paramId[index]));
                }
            }
            builder.Append("    <div class=\"modal-dialog\">\n");
            builder.Append("        <div class=\"modal-content\">\n");

            builder.Append("            <div class=\"modal-header\">\n");
            //builder.Append("                <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\"><span>×</span></button>\n");
            //builder.Append(String.Format("                <h4 class=\"modal-title\">{0}</h4>\n", title));
            builder.Append(String.Format("                <h4 class=\"modal-title\" id=\"{1}TitleLabel\">{0}</h4>\n", title, divId));
            builder.Append("                <button type=\"button\" class=\"btn-close btn-close-white\" data-bs-dismiss=\"modal\" aria-label=\"Close\"></button>\n");
            builder.Append("            </div>\n");

            builder.Append("            <div class=\"modal-body\">\n");
            if (!string.IsNullOrEmpty(message))
            {
                builder.Append(String.Format("                {0}\n", message));
            }
            builder.Append("            </div>\n");

            builder.Append("            <div class=\"modal-footer\">\n");
            if (button1Dismiss)
            {
                //builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId, button1));
                builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-bs-dismiss=\"modal\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId, button1));
            }
            else
            {
                builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId, button1));
            }
            //builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\" aria-hidden=\"true\">{0}</button>\n", button2));
            builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-bs-dismiss=\"modal\" aria-hidden=\"true\">{0}</button>\n", button2));
            builder.Append("            </div>\n");

            builder.Append("        </div>\n");
            builder.Append("    </div>\n");
            builder.Append("</div>\n");

            return new HtmlString(builder.ToString());
        }

        /// <summary>
        /// コンファーム作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="clickId1">クリックボタンID1</param>
        /// <param name="clickId2">クリックボタンID2</param>
        /// <param name="paramId">パラメータID</param>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="button1">ボタン表示文字1</param>
        /// <param name="button2">ボタン表示文字2</param>
        /// <param name="button1Dismiss">button1の「data-bs-dismiss」出力有無</param>
        /// <param name="button2Dismiss">button2の「data-bs-dismiss」出力有無</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Confirm(this IHtmlHelper htmlHelper, string divId, string clickId1, string clickId2, string[] paramId, string title, string message, string button1, string button2, bool button1Dismiss = true, bool button2Dismiss = true)
        {
            StringBuilder builder = new StringBuilder();

            //builder.Append(String.Format("\n<div class=\"modal\" id=\"{0}\" tabindex=\"-1\" data-backdrop=\"false\" aria-hidden=\"true\">\n", divId));
            builder.Append(String.Format("\n<div class=\"modal\" id=\"{0}\" tabindex=\"-1\" data-bs-backdrop=\"false\" aria-labelledby=\"{0}TitleLabel\" aria-hidden=\"true\">\n", divId));
            if (paramId != null && paramId.Length > 0)
            {
                for (int index = 0; index < paramId.Length; index++)
                {
                    builder.Append(String.Format("    <input type=\"hidden\" id=\"{0}\" value=\"\"/>\n", paramId[index]));
                }
            }
            builder.Append("    <div class=\"modal-dialog\">\n");
            builder.Append("        <div class=\"modal-content\">\n");

            builder.Append("            <div class=\"modal-header\">\n");
            //builder.Append("                <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\"><span>×</span></button>\n");
            //builder.Append(String.Format("                <h4 class=\"modal-title\">{0}</h4>\n", title));
            builder.Append(String.Format("                <h4 class=\"modal-title\" id=\"{1}TitleLabel\">{0}</h4>\n", title, divId));
            builder.Append("                <button type=\"button\" class=\"btn-close btn-close-white\" data-bs-dismiss=\"modal\" aria-label=\"Close\"></button>\n");
            builder.Append("            </div>\n");

            builder.Append("            <div class=\"modal-body\">\n");
            if (!string.IsNullOrEmpty(message))
            {
                builder.Append(String.Format("                {0}\n", message));
            }
            builder.Append("            </div>\n");

            builder.Append("            <div class=\"modal-footer\">\n");
            if (button1Dismiss)
            {
                //builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId1, button1));
                builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-bs-dismiss=\"modal\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId1, button1));
            }
            else
            {
                builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId1, button1));
            }
            if (button2Dismiss)
            {
                //builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId2, button2));
                builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" data-bs-dismiss=\"modal\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId2, button2));
            }
            else
            {
                builder.Append(String.Format("                <button type=\"button\" class=\"btn btn-default\" aria-hidden=\"true\" id=\"{0}\">{1}</button>\n", clickId2, button2));
            }
            builder.Append("            </div>\n");

            builder.Append("        </div>\n");
            builder.Append("    </div>\n");
            builder.Append("</div>\n");

            return new HtmlString(builder.ToString());
        }

        /// <summary>
        /// コンファーム表示メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString ShowConfirm(this IHtmlHelper htmlHelper, string divId)
        {
            return ShowConfirm(htmlHelper, divId, null);
        }

        /// <summary>
        /// コンファーム表示メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="message">メッセージ</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString ShowConfirm(this IHtmlHelper htmlHelper, string divId, string message)
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(message))
            {
                builder.Append(String.Format("    $('#{0}').find('.modal-body').html({1});\n", divId, message));
            }

            builder.Append(String.Format("    $('#{0}').modal('show');\n", divId));

            return new HtmlString(builder.ToString());
        }

        /// <summary>
        /// コンファーム表示メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="divId">ブロックID</param>
        /// <param name="param">パラメータ</param>
        /// <param name="message">メッセージ</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString ShowConfirm(this IHtmlHelper htmlHelper, string divId, NameValueCollection param, string message)
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(message))
            {
                builder.Append(String.Format("    $('#{0}').find('.modal-body').html({1});\n", divId, message));
            }

            builder.Append(String.Format("    $('#{0}').modal('show');\n", divId));
            if (param != null)
            {
                foreach (string key in param.AllKeys)
                {
                    string value = param[key];
                    builder.Append(String.Format("    $('#{0}').find('#{1}').val({2}).text({3});\n", divId, key, value, value));
                }
            }

            return new HtmlString(builder.ToString());
        }

        /// <summary>
        /// Ajaxスクリプト作成メソッド。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="data">データ文字列</param>
        /// <param name="url">URL文字列</param>
        /// <param name="success">successファンクション名</param>
        /// <param name="error">errorファンクション名</param>
        /// <returns>HTML文字列</returns>
        public static HtmlString Ajax(this IHtmlHelper htmlHelper, string data, string url, string success, string error)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("$.ajax({\n");
            builder.Append("    type: 'GET',\n");
            builder.Append("    cache: false,\n");
            builder.Append("    contentType: \"application/json; charset=utf-8\",\n");
            if (string.IsNullOrEmpty(data))
            {
                builder.Append("    data: '',\n");
            }
            else
            {
                builder.Append(String.Format("    data: {0},\n", data));
            }
            builder.Append("    dataType: 'json',\n");
            builder.Append(String.Format("    url: {0},\n", url));
            if (string.IsNullOrEmpty(error))
            {
                builder.Append(String.Format("    success: {0}\n", success));
            }
            else
            {
                builder.Append(String.Format("    success: {0},\n", success));
                builder.Append(String.Format("    error: {0}\n", error));
            }
            builder.Append("})\n");

            return new HtmlString(builder.ToString());
        }

        #region モデルの属性値取得
        /// <summary>
        /// 数字全桁入力チェック属性の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>最大桁数</returns>
        public static int GetFullDigitLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<FullDigitLengthAttribute>(modelExpression.ModelExplorer, "FullLength");
        }

        /// <summary>
        /// 文字全桁入力チェック属性の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>最大桁数</returns>
        public static int GetFullStringLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<FullStringLengthAttribute>(modelExpression.ModelExplorer, "FullLength");
        }

        /// <summary>
        /// 数値(符号無/小数有)チェック属性の最大桁数を取得する
        /// 少数部の桁数指定ありの場合、NumberDec属性の設定値に小数点分 +1とする
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>最大桁数</returns>
        public static int GetNumberDecFullMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            var fullMaxLength = GetPropertyInt<NumberDecAttribute>(modelExpression.ModelExplorer, "FullMaxLength");
            var decMaxLength = GetPropertyInt<NumberDecAttribute>(modelExpression.ModelExplorer, "DecMaxLength");

            // 少数部の桁数指定ありの場合、小数点分 +1とする
            return (decMaxLength > 0) ? fullMaxLength + 1 : fullMaxLength;
        }

        /// <summary>
        /// 数値(符号無/小数有)チェック属性の整数部の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>整数部の最大桁数</returns>
        public static int GetNumberDecIntMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<NumberDecAttribute>(modelExpression.ModelExplorer, "IntMaxLength");
        }

        /// <summary>
        /// 数値(符号無/小数有)チェック属性の小数部の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>小数部の最大桁数</returns>
        public static int GetNumberDecDecMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<NumberDecAttribute>(modelExpression.ModelExplorer, "DecMaxLength");
        }

        /// <summary>
        /// 数値(符号無/小数無)チェック属性の整数の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>整数の最大桁数</returns>
        public static int GetNumberPositiveMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<NumberPositiveAttribute>(modelExpression.ModelExplorer, "MaxLength");
        }

        /// <summary>
        /// 数値(符号有/小数無)チェック属性の整数の最大桁数を取得する
        /// NumberSign属性の設定に符号分 +1とする
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>整数の最大桁数</returns>
        public static int GetNumberSignMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            // 符号分 +1とする
            return GetPropertyInt<NumberSignAttribute>(modelExpression.ModelExplorer, "MaxLength") + 1;
        }

        /// <summary>
        /// 数値(符号有/小数有)チェック属性の最大桁数を取得する
        /// NumberSignDec属性の設定に符号分 +1、少数部の桁数指定ありの場合、小数点分 +1とする
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>最大桁数</returns>
        public static int GetNumberSignDecFullMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            var fullMaxLength = GetPropertyInt<NumberSignDecAttribute>(modelExpression.ModelExplorer, "FullMaxLength");
            var decMaxLength = GetPropertyInt<NumberSignDecAttribute>(modelExpression.ModelExplorer, "DecMaxLength");
            // 符号分 +1、少数部の桁数指定ありの場合、小数点分 +1とする
            return (decMaxLength > 0) ? fullMaxLength + 2 : fullMaxLength +1 ;
        }

        /// <summary>
        /// 数値(符号有/小数有)チェック属性の整数部の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>整数部の最大桁数</returns>
        public static int GetNumberSignDecIntMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<NumberSignDecAttribute>(modelExpression.ModelExplorer, "IntMaxLength");
        }

        /// <summary>
        /// 数値(符号有/小数有)チェック属性の小数部の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>小数部の最大桁数</returns>
        public static int GetNumberSignDecDecMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<NumberSignDecAttribute>(modelExpression.ModelExplorer, "DecMaxLength");
        }

        /// <summary>
        /// 桁数以内入力チェック属性の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>最大桁数</returns>
        public static int GetWithinDigitLengthMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<WithinDigitLengthAttribute>(modelExpression.ModelExplorer, "MaxLength");
        }

        /// <summary>
        /// 文字数以内入力チェック属性の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>最大桁数</returns>
        public static int GetWithinStringLengthMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<WithinStringLengthAttribute>(modelExpression.ModelExplorer, "MaxLength");
        }

        /// <summary>
        /// 文字数検証属性の最大桁数を取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns>最大桁数</returns>
        public static int GetStringLengthMaxLength<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            var modelExpression = GetModelExpression(htmlHelper, expression);
            return GetPropertyInt<StringLengthAttribute>(modelExpression.ModelExplorer, "MaximumLength");
        }

        /// <summary>
        /// ビューモデル項目のModelExpressionを取得する
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private static ModelExpression GetModelExpression<TModel, TResult>(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            // .NET CORE6 MVC
            // --------------------------------------------
            // ラムダ式からモデルのメタデータと値を取得する
            // --------------------------------------------
            // ModelexpressionProviderをServiceProviderから取得する
            ModelExpressionProvider? modelExpressionProvider
                = (ModelExpressionProvider?)htmlHelper.ViewContext?.HttpContext?
                    .RequestServices?.GetService(typeof(IModelExpressionProvider));
            // ラムダ式からモデルのメタデータを取得する
            var metadata = modelExpressionProvider?.CreateModelExpression(htmlHelper.ViewData, expression);
            if (metadata == null)
            {
                throw new NullReferenceException("metadata do not get.");
            }

            return metadata;
        }

        /// <summary>
        /// Validatorの指定したプロパティ名の値を取得する
        /// </summary>
        /// <typeparam name="T">ValidatorMetadata</typeparam>
        /// <param name="modelExplorer">ModelExplorer</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>プロパティ値、取得できない場合は0</returns>
        private static int GetPropertyInt<T>(ModelExplorer modelExplorer, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(modelExplorer);

            var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (property != null)
            {
                IReadOnlyList<object> validatorMetadata = modelExplorer.Metadata.ValidatorMetadata;

                if (validatorMetadata != null)
                {
                    for (var i = 0; i < validatorMetadata.Count; i++)
                    {
                        if (validatorMetadata[i] is T atribute && (int)property.GetValue(atribute) > 0)
                        {
                            return (int)property.GetValue(atribute);
                        }
                    }
                }
            }
            return 0;
        }

        #endregion
    }
}