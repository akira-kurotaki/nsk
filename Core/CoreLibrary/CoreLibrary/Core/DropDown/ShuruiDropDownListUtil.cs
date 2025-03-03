using CoreLibrary.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Context;
using System.ComponentModel;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 種類・品目・用途ドロップダウンリスト用ユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/21
    /// 作成者：Rou I
    /// </remarks>
    public class ShuruiDropDownListUtil
    {

        /// <summary>
        /// モデル名(種類)
        /// </summary>
        public static readonly string SHURUI = "ShuruiDropDownList.ShuruiCd";

        /// <summary>
        /// モデル名(品目)
        /// </summary>
        public static readonly string HIMMOKU = "ShuruiDropDownList.HimmokuCd";

        /// <summary>
        /// モデル名(用途)
        /// </summary>
        public static readonly string YOTO = "ShuruiDropDownList.YotoCd";

        /// <summary>
        /// 品目・種類・用途ドロップダウンリストの表示区分種別
        /// </summary>
        public enum KbnSbt
        {
            Shurui,        // 種類
            Himmoku,       // 品目
            Yoto           // 用途
        }

        /// <summary>
        /// ドロップダウンリストの表示モード
        /// </summary>
        public enum DisplayMode
        {
            Nosan,         // 農産物
            Chikusan,      // 畜産物
            Nochikusan     // 農産物+畜産物
        }

        /// <summary>
        /// ドロップダウンリストのプログラムモード
        /// </summary>
        public enum ProgramMode
        {
            [Description("Default")]
            Default,       // デフォルト
            [Description("D0204_1")]
            D0204_1,       // 過去申告書
            [Description("D0204_2")]
            D0204_2,       // 補助フォーム
            [Description("D0206")]
            D0206,         // 営農計画
            [Description("D0207")]
            D0207,         // 収入試算
            [Description("D0223")]
            D0223,         // 農業経営の目標
        }

        /// <summary>
        /// モデル名の取得メソッド。
        /// </summary>
        /// <param name="detailModelName">明細モデル変数名</param>
        /// <param name="index">インデックス</param>
        /// <param name="kbnSbt">区分種別</param>
        /// <returns>モデル名</returns>
        public static string GetName(string detailModelName, int index, KbnSbt kbnSbt)
        {
            string name = string.Empty;
            switch (kbnSbt)
            {
                // 種類
                case KbnSbt.Shurui:
                    if (string.IsNullOrEmpty(detailModelName))
                    {
                        name = SHURUI;
                    }
                    else
                    {
                        name = string.Format("{0}[{1}].{2}", detailModelName, index, SHURUI);
                    }
                    break;
                // 品目
                case KbnSbt.Himmoku:
                    if (string.IsNullOrEmpty(detailModelName))
                    {
                        name = HIMMOKU;
                    }
                    else
                    {
                        name = string.Format("{0}[{1}].{2}", detailModelName, index, HIMMOKU);
                    }
                    break;
                // 用途
                case KbnSbt.Yoto:
                    if (string.IsNullOrEmpty(detailModelName))
                    {
                        name = YOTO;
                    }
                    else
                    {
                        name = string.Format("{0}[{1}].{2}", detailModelName, index, YOTO);
                    }
                    break;
            }

            return name;
        }

        /// <summary>
        /// 種類選択リストの取得メソッド。
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <param name="displayMode">表示モード</param>
        /// <returns>選択リスト</returns>
        public static List<SelectListItem> GetShuruiSelectList(ShuruiParam param, DisplayMode displayMode)
        {
            var selectListItem = new List<SelectListItem>();

            using (JigyoCommonContext db = new JigyoCommonContext())
            {
                var result = (
                             from a in db.MNcShuruis
                             join b in db.MShishoNcShuruis on a.NcShuruiCd equals b.NcShuruiCd
                             where a.DeleteFlg == "0"
                                && b.HiddenFlg == CoreConst.HIDDEN_FLG_0
                                && b.TodofukenCd == param.TodofukenCd
                                && b.KumiaitoCd == param.KumiaitoCd
                                && b.ShishoCd == param.ShishoCd
                             select new
                             {
                                 a.NcShuruiCd,
                                 a.NcShuruiNm,
                                 a.NochikusanKbn,
                                 b.ShishoNcShuruiCd
                             }
                        );

                // 表示順
                switch (displayMode)
                {
                    case DisplayMode.Nosan:
                        result = result.Where(a => a.NochikusanKbn == "1" || a.NochikusanKbn == "999")
                                       .OrderBy(a => a.ShishoNcShuruiCd).ThenBy(a => a.NcShuruiCd);
                        break;
                    case DisplayMode.Chikusan:
                        result = result.Where(a => a.NochikusanKbn == "2" || a.NochikusanKbn == "999")
                                       .OrderBy(a => a.ShishoNcShuruiCd).ThenBy(a => a.NcShuruiCd);
                        break;
                    case DisplayMode.Nochikusan:
                        result = result.Where(a => a.NochikusanKbn == "1" || a.NochikusanKbn == "2" || a.NochikusanKbn == "999")
                                       .OrderBy(a => a.NochikusanKbn).ThenBy(a => a.ShishoNcShuruiCd).ThenBy(a => a.NcShuruiCd);
                        break;
                }

                foreach (var item in result.ToList())
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = item.NcShuruiCd,
                        Text = item.NcShuruiCd + CoreConst.SEPARATOR + item.NcShuruiNm
                    });
                }
            }
            return selectListItem;
        }

        /// <summary>
        /// 品目選択リストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetHimmokuSelectList(IShuruiDropDownList model)
        {
            return GetHimmokuSelectList(model, ProgramMode.Default);
        }

        /// <summary>
        /// 品目選択リストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="programMode">プログラムモード</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetHimmokuSelectList(IShuruiDropDownList model,
                                                      ProgramMode? programMode)
        {
            if (programMode == null)
            {
                programMode = ProgramMode.Default;
            }

            var selectListItem = new List<SelectListItem>();

            if (string.IsNullOrEmpty(model.ShuruiCd) ||
                string.IsNullOrEmpty(model.TodofukenCd) ||
                string.IsNullOrEmpty(model.KumiaitoCd) ||
                string.IsNullOrEmpty(model.ShishoCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }

            using (JigyoCommonContext db = new JigyoCommonContext())
            {

                var result = (
                             from a in db.MNcHimmokus
                             join b in db.MShishoNcHimmokus on new { a.NcShuruiCd, a.NcHimmokuCd }
                                                           equals new { b.NcShuruiCd, b.NcHimmokuCd }
                             where a.DeleteFlg == "0"
                                && b.HiddenFlg == CoreConst.HIDDEN_FLG_0
                                && b.NcShuruiCd == model.ShuruiCd
                                && b.TodofukenCd == model.TodofukenCd
                                && b.KumiaitoCd == model.KumiaitoCd
                                && b.ShishoCd == model.ShishoCd
                             select new
                             {
                                 a.NcShuruiCd,
                                 a.NcHimmokuCd,
                                 a.NcHimmokuNm,
                                 b.ShishoNcHimmokuCd,
                                 a.KakoshinkokuYoshikiFlg,
                                 a.HojoformYoshikiFlg,
                                 a.EinokeikakuYoshikiFlg,
                                 a.ShunyushisanYoshikiFlg,
                                 a.KeieimokuhyoYoshikiFlg
                             }
                         );
                switch (programMode)
                {
                    // D0204_1
                    case ProgramMode.D0204_1:
                        result = result.Where(a => a.KakoshinkokuYoshikiFlg == "1");
                        break;
                    // D0204_2
                    case ProgramMode.D0204_2:
                        result = result.Where(a => a.HojoformYoshikiFlg == "1");
                        break;
                    // D0206
                    case ProgramMode.D0206:
                        result = result.Where(a => a.EinokeikakuYoshikiFlg == "1");
                        break;
                    // D0207
                    case ProgramMode.D0207:
                        result = result.Where(a => a.ShunyushisanYoshikiFlg == "1");
                        break;
                    // D0223
                    case ProgramMode.D0223:
                        result = result.Where(a => a.KeieimokuhyoYoshikiFlg == "1");
                        break;

                }

                result = result.OrderBy(a => a.ShishoNcHimmokuCd).ThenBy(a => a.NcHimmokuCd);

                foreach (var item in result.ToList())
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = item.NcHimmokuCd,
                        Text = item.NcHimmokuCd + CoreConst.SEPARATOR + item.NcHimmokuNm
                    });
                }
            }

            if (string.IsNullOrEmpty(model.HimmokuCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", model.HimmokuCd);
            }
        }

        /// <summary>
        /// 用途選択リストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetYotoSelectList(IShuruiDropDownList model)
        {
            return GetYotoSelectList(model, ProgramMode.Default);
        }

        /// <summary>
        /// 用途選択リストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="programMode">プログラムモード</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetYotoSelectList(IShuruiDropDownList model,
                                                   ProgramMode? programMode)
        {
            if (programMode == null)
            {
                programMode = ProgramMode.Default;
            }

            var selectListItem = new List<SelectListItem>();

            if (string.IsNullOrEmpty(model.ShuruiCd) ||
                string.IsNullOrEmpty(model.HimmokuCd) ||
                string.IsNullOrEmpty(model.TodofukenCd) ||
                string.IsNullOrEmpty(model.KumiaitoCd) ||
                string.IsNullOrEmpty(model.ShishoCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }

            using (JigyoCommonContext db = new JigyoCommonContext())
            {

                var result = (
                             from a in db.MNcYotos
                             join b in db.MShishoNcYotos on new { a.NcShuruiCd, a.NcHimmokuCd, a.NcYotoCd }
                                                        equals new { b.NcShuruiCd, b.NcHimmokuCd, b.NcYotoCd }
                             where a.DeleteFlg == "0"
                                && b.HiddenFlg == CoreConst.HIDDEN_FLG_0
                                && b.NcShuruiCd == model.ShuruiCd
                                && b.NcHimmokuCd == model.HimmokuCd
                                && b.TodofukenCd == model.TodofukenCd
                                && b.KumiaitoCd == model.KumiaitoCd
                                && b.ShishoCd == model.ShishoCd
                             select new
                             {
                                 a.NcShuruiCd,
                                 a.NcHimmokuCd,
                                 a.NcYotoCd,
                                 a.NcYotoNm,
                                 b.ShishoNcYotoCd,
                                 a.KakoshinkokuYoshikiFlg,
                                 a.HojoformYoshikiFlg,
                                 a.EinokeikakuYoshikiFlg,
                                 a.ShunyushisanYoshikiFlg,
                                 a.KeieimokuhyoYoshikiFlg
                             }
                         );

                switch (programMode)
                {
                    // D0204_1
                    case ProgramMode.D0204_1:
                        result = result.Where(a => a.KakoshinkokuYoshikiFlg == "1");
                        break;
                    // D0204_2
                    case ProgramMode.D0204_2:
                        result = result.Where(a => a.HojoformYoshikiFlg == "1");
                        break;
                    // D0206
                    case ProgramMode.D0206:
                        result = result.Where(a => a.EinokeikakuYoshikiFlg == "1");
                        break;
                    // D0207
                    case ProgramMode.D0207:
                        result = result.Where(a => a.ShunyushisanYoshikiFlg == "1");
                        break;
                    // D0223
                    case ProgramMode.D0223:
                        result = result.Where(a => a.KeieimokuhyoYoshikiFlg == "1");
                        break;

                }

                result = result.OrderBy(a => a.ShishoNcYotoCd).ThenBy(a => a.NcYotoCd);

                foreach (var item in result.ToList())
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = item.NcYotoCd,
                        Text = item.NcYotoCd + CoreConst.SEPARATOR + item.NcYotoNm
                    });
                }
            }

            if (string.IsNullOrEmpty(model.YotoCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", model.YotoCd);
            }

        }
    }
}
