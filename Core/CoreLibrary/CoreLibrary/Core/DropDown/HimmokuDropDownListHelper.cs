using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 品目・種類・用途ドロップダウンリスト用HTMLヘルパー拡張クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/21
    /// 作成者：Rou I
    /// </remarks>
    public static class HimmokuDropDownListHelper
    {
        /// <summary>
        /// 品目onchangeイベントファンクション名
        /// </summary>
        public static readonly string HIMMOKU_ONCHANGE_FUNC = "changeHimmoku";

        /// <summary>
        /// 種類onchangeイベントファンクション名
        /// </summary>
        public static readonly string SHURUI_ONCHANGE_FUNC = "changeShurui";

        /// <summary>
        /// 品目ドロップダウン作成メソッド(連動)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="param">パラメータ</param>
        /// <param name="index">インデックス</param>
        /// <param name="selectList">選択リスト</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent HimmokuDropDownList(
            this IHtmlHelper htmlHelper,
            IHimmokuDropDownList model,
            HimmokuParam param,
            int index,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            object htmlAttributes)
        {
            string name = HimmokuDropDownListUtil.GetName(param.DetailModelName, index, HimmokuDropDownListUtil.KbnSbt.Himmoku);

            IDictionary<string, object> attributes = htmlAttributes.ToDictionary();
            if (string.IsNullOrEmpty(param.HimmokuOnchangeFunc))
            {
                attributes.AddOnChangeAttribute(string.Format(HIMMOKU_ONCHANGE_FUNC + "({0})", index));
            }
            else
            {
                attributes.AddOnChangeAttribute(string.Format(param.HimmokuOnchangeFunc + "({0})", index));
            }

            IEnumerable<SelectListItem> selectListItems = string.IsNullOrEmpty(model.HimmokuCd) ?
                                                          new SelectList(selectList, "Value", "Text") :
                                                          new SelectList(selectList, "Value", "Text", model.HimmokuCd);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            return htmlHelper.DropDownList(name, selectListItems, optionLabel, attributes);
        }

        /// <summary>
        /// 種類ドロップダウン作成メソッド(連動)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="param">パラメータ</param>
        /// <param name="index">インデックス</param>
        /// <param name="selectList">選択リスト</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent ShuruiDropDownList(
            this IHtmlHelper htmlHelper,
            HimmokuDropDownList model,
            HimmokuParam param,
            int index,
            string optionLabel,
            object htmlAttributes)
        {
            string name = HimmokuDropDownListUtil.GetName(param.DetailModelName, index, HimmokuDropDownListUtil.KbnSbt.Shurui);
            IDictionary<string, object> attributes = htmlAttributes.ToDictionary();
            if (string.IsNullOrEmpty(param.ShuruiOnchangeFunc))
            {
                attributes.AddOnChangeAttribute(string.Format(SHURUI_ONCHANGE_FUNC + "({0})", index));
            }
            else
            {
                attributes.AddOnChangeAttribute(string.Format(param.ShuruiOnchangeFunc + "({0})", index));
            }

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (string.IsNullOrEmpty(model.HimmokuCd))
            {
                attributes.AddDisabledAttribute();
                return htmlHelper.DropDownList(name, new SelectList(new List<SelectListItem>(), "Value", "Text"), optionLabel, attributes);
            }
            else
            {
                model.TodofukenCd = param.TodofukenCd;
                model.KumiaitoCd = param.KumiaitoCd;
                model.ShishoCd = param.ShishoCd;

                IEnumerable<SelectListItem> selectList = HimmokuDropDownListUtil.GetShuruiSelectList(model);
                return htmlHelper.DropDownList(name, selectList, optionLabel, attributes);
            }

        }

        /// <summary>
        /// 種類入力項目作成メソッド(連動)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="expression">ビューモデル項目</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="templateName">テンプレート名</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent ShuruiEditFor<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression,
            IHimmokuDropDownList model,
            string templateName,
            object htmlAttributes)
        {
            IDictionary<string, object> attributes = htmlAttributes.ToDictionary();

            if (!HimmokuDropDownListUtil.SHURUI_OTHER_ITEM_VALUE.Equals(model.ShuruiCd))
            {
                attributes.AddDisabledAttribute();
            }

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            return htmlHelper.EditorFor(expression, templateName, htmlFieldName:null, new { htmlAttributes = attributes });
        }

        /// <summary>
        /// 用途ドロップダウン作成メソッド(連動)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="param">パラメータ</param>
        /// <param name="index">インデックス</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent YotoDropDownList(
            this IHtmlHelper htmlHelper,
            HimmokuDropDownList model,
            HimmokuParam param,
            int index,
            string optionLabel,
            object htmlAttributes)
        {
            string name = HimmokuDropDownListUtil.GetName(param.DetailModelName, index, HimmokuDropDownListUtil.KbnSbt.Yoto);
            IDictionary<string, object> attributes = htmlAttributes.ToDictionary();

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (string.IsNullOrEmpty(model.HimmokuCd))
            {
                attributes.AddDisabledAttribute();
                return htmlHelper.DropDownList(name, new SelectList(new List<SelectListItem>(), "Value", "Text"), optionLabel, attributes);
            }
            else
            {
                model.TodofukenCd = param.TodofukenCd;
                model.KumiaitoCd = param.KumiaitoCd;
                model.ShishoCd = param.ShishoCd;

                IEnumerable<SelectListItem> selectList = HimmokuDropDownListUtil.GetYotoSelectList(model);
                return htmlHelper.DropDownList(name, selectList, optionLabel, attributes);
            }

        }
    }
}