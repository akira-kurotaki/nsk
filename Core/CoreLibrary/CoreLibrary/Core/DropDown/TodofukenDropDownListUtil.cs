using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 都道府県ドロップダウンリスト用ユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/19
    /// 作成者：Rou I
    /// </remarks>
    public class TodofukenDropDownListUtil
    {

        /// <summary>
        /// モデル名(都道府県)
        /// </summary>
        public const string TODOFUKEN = "TodofukenDropDownList.TodofukenCd";

        /// <summary>
        /// モデル名(組合等)
        /// </summary>
        public const string KUMIAITO = "TodofukenDropDownList.KumiaitoCd";

        /// <summary>
        /// モデル名(支所)
        /// </summary>
        public const string SHISHO = "TodofukenDropDownList.ShishoCd";

        /// <summary>
        /// モデル名(市町村)
        /// </summary>
        public const string SHICHOSON = "TodofukenDropDownList.ShichosonCd";

        /// <summary>
        /// モデル名(大地区)
        /// </summary>
        public const string DAICHIKU = "TodofukenDropDownList.DaichikuCd";

        /// <summary>
        /// モデル名(小地区)
        /// </summary>
        public const string SHOCHIKU = "TodofukenDropDownList.ShochikuCd";

        /// <summary>
        /// モデル名(小地区From)
        /// </summary>
        public const string SHOCHIKUFROM = "TodofukenDropDownList.ShochikuCdFrom";

        /// <summary>
        /// モデル名(小地区To)
        /// </summary>
        public const string SHOCHIKUTO = "TodofukenDropDownList.ShochikuCdTo";

        /// <summary>
        /// モデル名(都道府県所属)
        /// </summary>
        public const string ISTODOFUKEN = "TodofukenDropDownList.IsTodofuken";

        /// <summary>
        /// モデル名(組合等所属)
        /// </summary>
        public const string ISKUMIAITO = "TodofukenDropDownList.IsKumiaito";

        /// <summary>
        /// モデル名(支所所属)
        /// </summary>
        public const string ISSHISHO = "TodofukenDropDownList.IsShisho";

        /// <summary>
        /// モデル名(モデルプロパティ名)
        /// </summary>
        public const string MODELPROPERTYLNAME = "TodofukenDropDownList.ModelPropertyName";

        /// <summary>
        /// 都道府県ドロップダウンリストの表示区分種別
        /// </summary>
        public enum KbnSbt
        {
            Todofuken,       // 都道府県
            Kumiaito,        // 組合等
            Shisho,          // 支所
            Shichoson,       // 市町村
            Daichiku,        // 大地区
            Shochiku,        // 小地区
            ShochikuFrom,    // 小地区(From)
            ShochikuTo,      // 小地区(To)
            IsTodofuken,     // 都道府県所属
            IsKumiaito,      // 組合等所属
            IsShisho,        // 支所所属
            ModelPropertyName // モデルプロパティ名
        }

        /// <summary>
        /// 選択リストの取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="model">ビューモデル</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetSelectList(KbnSbt kbnSbt, ITodofukenDropDownList model)
        {
            switch (kbnSbt)
            {
                // 都道府県
                case KbnSbt.Todofuken:
                    return TodofukenUtil.GetSelectList(model);
                // 組合等
                case KbnSbt.Kumiaito:
                    return KumiaitoUtil.GetSelectList(model);
                // 支所
                case KbnSbt.Shisho:
                    return ShishoUtil.GetSelectList(model);
                // 市町村
                case KbnSbt.Shichoson:
                    return ShichosonUtil.GetSelectList(model);
                // 大地区
                case KbnSbt.Daichiku:
                    return DaichikuUtil.GetSelectList(model);
                // 小地区
                case KbnSbt.Shochiku:
                    return ShochikuUtil.GetSelectList(model);
                // 小地区(From)
                case KbnSbt.ShochikuFrom:
                    return ShochikuUtil.GetSelectListFrom(model);
                // 小地区(To)
                case KbnSbt.ShochikuTo:
                    return ShochikuUtil.GetSelectListTo(model);
            }

            return new SelectList(new List<SelectListItem>(), "Value", "Text");
        }

        /// <summary>
        /// モデル名の取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="model">ビューモデル</param>
        /// <returns>モデル名</returns>
        public static string GetName(KbnSbt kbnSbt, ITodofukenDropDownList model)
        {
            string name = string.Empty;
            switch (kbnSbt)
            {
                // 都道府県
                case KbnSbt.Todofuken:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = TODOFUKEN;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + TODOFUKEN;
                    }
                    break;
                // 組合等
                case KbnSbt.Kumiaito:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = KUMIAITO;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + KUMIAITO;
                    }
                    break;
                // 支所
                case KbnSbt.Shisho:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = SHISHO;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + SHISHO;
                    }
                    break;
                // 市町村
                case KbnSbt.Shichoson:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = SHICHOSON;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + SHICHOSON;
                    }
                    break;
                // 大地区
                case KbnSbt.Daichiku:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = DAICHIKU;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + DAICHIKU;
                    }
                    break;
                // 小地区
                case KbnSbt.Shochiku:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = SHOCHIKU;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + SHOCHIKU;
                    }
                    break;
                // 小地区(From)
                case KbnSbt.ShochikuFrom:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = SHOCHIKUFROM;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + SHOCHIKUFROM;
                    }
                    break;
                // 小地区(To)
                case KbnSbt.ShochikuTo:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = SHOCHIKUTO;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + SHOCHIKUTO;
                    }
                    break;
                // 都道府県所属
                case KbnSbt.IsTodofuken:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = ISTODOFUKEN;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + ISTODOFUKEN;
                    }
                    break;
                // 組合等所属
                case KbnSbt.IsKumiaito:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = ISKUMIAITO;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + ISKUMIAITO;
                    }
                    break;
                // 支所所属
                case KbnSbt.IsShisho:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = ISSHISHO;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + ISSHISHO;
                    }
                    break;
                // モデルプロパティ名
                case KbnSbt.ModelPropertyName:
                    if (string.IsNullOrEmpty(model.ModelPropertyName))
                    {
                        name = MODELPROPERTYLNAME;
                    }
                    else
                    {
                        name = model.ModelPropertyName + "." + MODELPROPERTYLNAME;
                    }
                    break;
            }

            return name;
        }
    }

    /// <summary>
    /// 組合等サブ関連クラス
    /// </summary>
    [Serializable]
    public class KumiaitoSubSelectList
    {
        /// <summary>
        /// 支所
        /// </summary>
        public SelectList Shisho { get; set; }

        /// <summary>
        /// 市町村
        /// </summary>
        public SelectList Shichoson { get; set; }

        /// <summary>
        /// 大地区
        /// </summary>
        public SelectList Daichiku { get; set; }

    }
}