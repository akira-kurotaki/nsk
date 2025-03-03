using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 都道府県マルチドロップダウンリスト用HTMLヘルパー拡張クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/19
    /// 作成者：Rou I
    /// </remarks>
    public static class TodofukenDropDownListHelper
    {

        /// <summary>
        /// マルチドロップダウン作成メソッド(属性名)。
        /// </summary>
        /// <param name="htmlHelper">HTMLヘルパー</param>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="optionLabel">1項目目表示ラベル</param>
        /// <param name="htmlAttributes">HTML属性</param>
        /// <returns>HTML文字列</returns>
        public static IHtmlContent TodofukenDropDownList(
            this IHtmlHelper htmlHelper,
            TodofukenDropDownListUtil.KbnSbt kbnSbt,
            ITodofukenDropDownList model,
            string optionLabel,
            object htmlAttributes)
        {
            string name = TodofukenDropDownListUtil.GetName(kbnSbt, model);
            IDictionary<string, object> attributes = htmlAttributes.ToDictionary();
            switch (kbnSbt)
            {
                // 都道府県
                case TodofukenDropDownListUtil.KbnSbt.Todofuken:
                    if (model.IsTodofuken)
                    {
                        attributes.AddDisabledAttribute();
                    }
                    break;
                // 組合等
                case TodofukenDropDownListUtil.KbnSbt.Kumiaito:
                    if (model.IsKumiaito || string.IsNullOrWhiteSpace(model.TodofukenCd))
                    {
                        attributes.AddDisabledAttribute();
                    }
                    break;
                // 支所
                case TodofukenDropDownListUtil.KbnSbt.Shisho:
                    if (model.IsShisho || string.IsNullOrWhiteSpace(model.KumiaitoCd))
                    {
                        attributes.AddDisabledAttribute();
                    }
                    break;
                // 市町村
                case TodofukenDropDownListUtil.KbnSbt.Shichoson:
                    if (string.IsNullOrWhiteSpace(model.KumiaitoCd))
                    {
                        attributes.AddDisabledAttribute();
                    }
                    break;
                // 大地区
                case TodofukenDropDownListUtil.KbnSbt.Daichiku:
                    if (string.IsNullOrWhiteSpace(model.KumiaitoCd))
                    {
                        attributes.AddDisabledAttribute();
                    }
                    break;
                // 小地区
                case TodofukenDropDownListUtil.KbnSbt.Shochiku:
                    if (string.IsNullOrWhiteSpace(model.DaichikuCd))
                    {
                        attributes.AddDisabledAttribute();
                    }
                    break;
                // 小地区(From)
                case TodofukenDropDownListUtil.KbnSbt.ShochikuFrom:
                    if (string.IsNullOrWhiteSpace(model.DaichikuCd))
                    {
                        attributes.AddDisabledAttribute();
                    }
                    break;
                // 小地区(To)
                case TodofukenDropDownListUtil.KbnSbt.ShochikuTo:
                    if (string.IsNullOrWhiteSpace(model.DaichikuCd))
                    {
                        attributes.AddDisabledAttribute();
                    }
                    break;
            }

            IEnumerable<SelectListItem> selectList = TodofukenDropDownListUtil.GetSelectList(kbnSbt, model);

            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }
            return htmlHelper.DropDownList(name, selectList, optionLabel, attributes);
        }

        /// <summary>
        /// オブジェクト型をDictionary型に変換メソッド。
        /// </summary>
        /// <param name="data">オブジェクト型データ</param>
        /// <returns>変換後Dictionary型データ</returns>
        public static IDictionary<string, object> ToDictionary(this object data)
        {
            BindingFlags publicAttributes = BindingFlags.Public | BindingFlags.Instance;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            foreach (PropertyInfo property in data.GetType().GetProperties(publicAttributes))
            {
                if (property.CanRead)
                {
                    dictionary.Add(property.Name.Replace('_' , '-'), property.GetValue(data, null));
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Disabled属性追加メソッド。
        /// </summary>
        /// <param name="data">Dictionary型データ</param>
        /// <returns>編集後Dictionary型データ</returns>
        public static IDictionary<string, object> AddDisabledAttribute(this IDictionary<string, object> data)
        {
            data.Add("disabled", "disabled");
            return data;
        }
    }
}