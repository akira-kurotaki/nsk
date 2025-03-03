using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Context;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 種類・品目・用途ドロップダウンリスト用HTMLヘルパー拡張クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/21
    /// 作成者：Rou I
    /// </remarks>
    public static class ShuruiDropDownListHelper
    {
        /// <summary>
        /// 種類onchangeイベントファンクション名
        /// </summary>
        public static readonly string SHURUI_ONCHANGE_FUNC = "changeShurui";

        /// <summary>
        /// 品目onchangeイベントファンクション名
        /// </summary>
        public static readonly string HIMMOKU_ONCHANGE_FUNC = "changeHimmoku";

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
            IShuruiDropDownList model,
            ShuruiParam param,
            int index,
            IEnumerable<SelectListItem> selectList,
            string optionLabel,
            object htmlAttributes)
        {
            string name = ShuruiDropDownListUtil.GetName(param.DetailModelName, 
                                                         index, ShuruiDropDownListUtil.KbnSbt.Shurui);

            IDictionary<string, object> attributes = htmlAttributes.ToDictionary();
            if (string.IsNullOrEmpty(param.ShuruiOnchangeFunc))
            {
                attributes.AddOnChangeAttribute(string.Format(SHURUI_ONCHANGE_FUNC + "({0})", index));
            }
            else
            {
                attributes.AddOnChangeAttribute(string.Format(param.ShuruiOnchangeFunc + "({0})", index));
            }

            IEnumerable<SelectListItem> selectListItems = string.IsNullOrEmpty(model.ShuruiCd) ?
                                                          new SelectList(selectList, "Value", "Text") :
                                                          new SelectList(selectList, "Value", "Text", model.ShuruiCd);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }
            return htmlHelper.DropDownList(name, selectListItems, optionLabel, attributes);
        }

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
            ShuruiDropDownList model,
            ShuruiParam param,
            int index,
            string optionLabel,
            object htmlAttributes)
        {
            return HimmokuDropDownList(htmlHelper,
                                       model,
                                       param,
                                       index,
                                       ShuruiDropDownListUtil.ProgramMode.Default,
                                       optionLabel,
                                       htmlAttributes);
        }

        /// <summary>
        /// 品目ドロップダウン作成メソッド(連動)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="param">パラメータ</param>
        /// <param name="index">インデックス</param>
        /// <param name="selectList">選択リスト</param>
        /// <param name="programMode">プログラムモード</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent HimmokuDropDownList(
            this IHtmlHelper htmlHelper,
            ShuruiDropDownList model,
            ShuruiParam param,
            int index,
            ShuruiDropDownListUtil.ProgramMode programMode,
            string optionLabel,
            object htmlAttributes)
        {
            string name = ShuruiDropDownListUtil.GetName(param.DetailModelName, 
                                                         index, ShuruiDropDownListUtil.KbnSbt.Himmoku);
            IDictionary<string, object> attributes = htmlAttributes.ToDictionary();
            if (string.IsNullOrEmpty(param.HimmokuOnchangeFunc))
            {
                attributes.AddOnChangeAttribute(string.Format(HIMMOKU_ONCHANGE_FUNC + "({0})", index));
            }
            else
            {
                attributes.AddOnChangeAttribute(string.Format(param.HimmokuOnchangeFunc + "({0})", index));
            }

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (string.IsNullOrEmpty(model.ShuruiCd))
            {
                attributes.AddDisabledAttribute();
                return htmlHelper.DropDownList(name, new SelectList(new List<SelectListItem>(), "Value", "Text"), optionLabel, attributes);
            }
            else
            {
                model.TodofukenCd = param.TodofukenCd;
                model.KumiaitoCd = param.KumiaitoCd;
                model.ShishoCd = param.ShishoCd;

                IEnumerable<SelectListItem> selectList;
                using (JigyoContext db = LoginUserContextUtil.getLoginUserContext(htmlHelper.ViewContext.HttpContext))
                {
                    selectList = ShuruiDropDownListUtil.GetHimmokuSelectList(model, programMode);
                }

                return htmlHelper.DropDownList(name, selectList, optionLabel, attributes);
            }

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
            ShuruiDropDownList model,
            ShuruiParam param,
            int index,
            string optionLabel,
            object htmlAttributes)
        {
            return YotoDropDownList(htmlHelper,
                                       model,
                                       param,
                                       index,
                                       ShuruiDropDownListUtil.ProgramMode.Default,
                                       optionLabel,
                                       htmlAttributes);
        }

        /// <summary>
        /// 用途ドロップダウン作成メソッド(連動)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="param">パラメータ</param>
        /// <param name="index">インデックス</param>
        /// <param name="programMode">プログラムモード</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent YotoDropDownList(
            this IHtmlHelper htmlHelper,
            ShuruiDropDownList model,
            ShuruiParam param,
            int index,
            ShuruiDropDownListUtil.ProgramMode programMode,
            string optionLabel,
            object htmlAttributes)
        {
            string name = ShuruiDropDownListUtil.GetName(param.DetailModelName, index, ShuruiDropDownListUtil.KbnSbt.Yoto);
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

                IEnumerable<SelectListItem> selectList;
                using (JigyoContext db = LoginUserContextUtil.getLoginUserContext(htmlHelper.ViewContext.HttpContext))
                {
                    selectList = ShuruiDropDownListUtil.GetYotoSelectList(model, programMode);
                }
                return htmlHelper.DropDownList(name, selectList, optionLabel, attributes);
            }

        }

        /// <summary>
        /// onchange属性追加メソッド。
        /// </summary>
        /// <param name="data">Dictionary型データ</param>
        /// <param name="str">追加文字列</param>
        /// <returns>編集後Dictionary型データ</returns>
        public static IDictionary<string, object> AddOnChangeAttribute(this IDictionary<string, object> data, string str)
        {
            data.Add("onchange", str);
            return data;
        }
    }
}