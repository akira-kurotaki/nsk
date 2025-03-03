using CoreLibrary.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 品目・種類・用途ドロップダウンリスト用ユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/21
    /// 作成者：Rou I
    /// </remarks>
    public class HimmokuDropDownListUtil
    {
        /// <summary>
        /// モデル名(品目)
        /// </summary>
        public static readonly string HIMMOKU = "HimmokuDropDownList.HimmokuCd";

        /// <summary>
        /// モデル名(種類)
        /// </summary>
        public static readonly string SHURUI = "HimmokuDropDownList.ShuruiCd";

        /// <summary>
        /// モデル名(用途)
        /// </summary>
        public static readonly string YOTO = "HimmokuDropDownList.YotoCd";

        /// <summary>
        /// 「その他」項目値(種類)
        /// </summary>
        public static readonly string SHURUI_OTHER_ITEM_VALUE = "999";

        /// <summary>
        /// 「その他」項目表示ラベル(種類)
        /// </summary>
        public static readonly string SHURUI_OTHER_ITEM_TEXT = "その他";

        /// <summary>
        /// 「その他」項目値
        /// </summary>
        public static readonly string YOTO_OTHER_ITEM_VALUE = "99";

        /// <summary>
        /// 「その他」項目表示ラベル
        /// </summary>
        public static readonly string YOTO_OTHER_ITEM_TEXT = "その他用途";

        /// <summary>
        /// 「その他」項目
        /// </summary>
        public static readonly SelectListItem SHURUI_OTHER_ITEM =
            new SelectListItem
            {
                Value = SHURUI_OTHER_ITEM_VALUE,
                Text = SHURUI_OTHER_ITEM_VALUE + CoreConst.SEPARATOR + SHURUI_OTHER_ITEM_TEXT
            };

        /// <summary>
        /// 「その他」項目
        /// </summary>
        public static readonly SelectListItem YOTO_OTHER_ITEM =
            new SelectListItem
            {
                Value = YOTO_OTHER_ITEM_VALUE,
                Text = YOTO_OTHER_ITEM_VALUE + CoreConst.SEPARATOR + YOTO_OTHER_ITEM_TEXT
            };

        /// <summary>
        /// 品目・種類・用途ドロップダウンリストの表示区分種別
        /// </summary>
        public enum KbnSbt
        {
            Himmoku,       // 品目
            Shurui,        // 種類
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
        /// 品目選択リストの取得メソッド。
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <param name="displayMode">表示モード</param>
        /// <returns>選択リスト</returns>
        public static List<SelectListItem> GetHimmokuSelectList(HimmokuParam param,
                                                                DisplayMode displayMode)
        {
            var selectListItem = new List<SelectListItem>();

            /* ビルドエラーのためコメントアウト 2018/05/16
            using (SynContext syndb = new SynContext())
            {
                var result = (
                                 from a in syndb.MNosambutsutoHimmokus
                                 join b in syndb.MShishobetsuHimmokus on a.HimmokuCd equals b.HimmokuCd
                                 where b.HiddenFlg == CoreConst.HIDDEN_FLG_0
                                    && b.TodofukenCd == param.TodofukenCd
                                    && b.KumiaitoCd == param.KumiaitoCd
                                    && b.ShishoCd == param.ShishoCd
                                 select new HimmokuSearchResult
                                 {
                                     HimmokuCd = a.HimmokuCd,
                                     HimmokuNm = a.HimmokuNm,
                                     NochikusanKbn = a.NochikusanKbn,
                                     ShishoHimmokuCd = b.ShishoHimmokuCd
                                 }
                            );

                // 表示順
                switch (displayMode)
                {
                    case DisplayMode.Nosan:
                        result = result.Where(a => a.NochikusanKbn == "1")
                                       .OrderBy(a => a.ShishoHimmokuCd).ThenBy(a => a.HimmokuCd);
                        break;
                    case DisplayMode.Chikusan:
                        result = result.Where(a => a.NochikusanKbn == "2")
                                       .OrderBy(a => a.ShishoHimmokuCd).ThenBy(a => a.HimmokuCd);
                        break;
                    case DisplayMode.Nochikusan:
                        result = result.Where(a => a.NochikusanKbn == "1" || a.NochikusanKbn == "2")
                                       .OrderBy(a => a.NochikusanKbn).ThenBy(a => a.ShishoHimmokuCd).ThenBy(a => a.HimmokuCd);
                        break;
                }

                foreach (var item in result.ToList())
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = item.HimmokuCd,
                        Text = item.HimmokuCd + CoreConst.SEPARATOR + item.HimmokuNm
                    });
                }
            }
            */ 
            return selectListItem;
        }

        /// <summary>
        /// 種類選択リストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetShuruiSelectList(IHimmokuDropDownList model)
        {
            var selectListItem = new List<SelectListItem>();

            /* ビルドエラーのためコメントアウト 2018/05/16
            using (SynContext syndb = new SynContext())
            {
                var result = (
                                 from a in syndb.MNosambutsutoShuruis
                                 join b in syndb.MShishobetsuShuruis on new { a.HimmokuCd, a.ShuruiCd }
                                                                 equals new { b.HimmokuCd, b.ShuruiCd }
                                 where b.HimmokuCd == model.HimmokuCd
                                    && b.HiddenFlg == CoreConst.HIDDEN_FLG_0
                                    && b.TodofukenCd == model.TodofukenCd
                                    && b.KumiaitoCd == model.KumiaitoCd
                                    && b.ShishoCd == model.ShishoCd
                                 select new HimmokuSearchResult
                                 {
                                     HimmokuCd = a.HimmokuCd,
                                     ShuruiCd = a.ShuruiCd,
                                     ShuruiNm = a.ShuruiNm,
                                     ShishoShuruiCd = b.ShishoShuruiCd
                                 }
                             ).OrderBy(a => a.ShishoShuruiCd).ThenBy(a => a.ShuruiCd);

                foreach (var item in result.ToList())
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = item.ShuruiCd,
                        Text = item.ShuruiCd + CoreConst.SEPARATOR + item.ShuruiNm
                    });
                }
            }

            selectListItem.Add(SHURUI_OTHER_ITEM);
            */
            if (string.IsNullOrEmpty(model.ShuruiCd))
            {
                return new SelectList(selectListItem, "Value", "Text");
            }
            else
            {
                return new SelectList(selectListItem, "Value", "Text", model.ShuruiCd);
            }
        }

        /// <summary>
        /// 用途選択リストの取得メソッド。
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>選択リスト</returns>
        public static SelectList GetYotoSelectList(IHimmokuDropDownList model)
        {
            var selectListItem = new List<SelectListItem>();
            /* ビルドエラーのためコメントアウト 2018/05/16
            using (SynContext syndb = new SynContext())
            {
                var result = (
                                 from a in syndb.MNosambutsutoYotos
                                 join b in syndb.MShishobetsuYotos on new { a.HimmokuCd, a.ShuruiCd, a.YotoCd }
                                                               equals new { b.HimmokuCd, b.ShuruiCd, b.YotoCd }
                                 where b.HimmokuCd == model.HimmokuCd
                                    && b.HiddenFlg == CoreConst.HIDDEN_FLG_0
                                    && b.TodofukenCd == model.TodofukenCd
                                    && b.KumiaitoCd == model.KumiaitoCd
                                    && b.ShishoCd == model.ShishoCd
                                 select new HimmokuSearchResult
                                 {
                                     HimmokuCd = a.HimmokuCd,
                                     ShuruiCd = a.ShuruiCd,
                                     YotoCd = a.YotoCd,
                                     YotoNm = a.YotoNm,
                                     ShishoYotoCd = b.ShishoYotoCd
                                 }
                             );

                if (string.IsNullOrEmpty(model.ShuruiCd) || SHURUI_OTHER_ITEM_VALUE.Equals(model.ShuruiCd))
                {
                    result = result.OrderBy(a => a.ShishoYotoCd).ThenBy(a => a.YotoCd);
                }
                else
                {
                    result = result.Where(a => a.ShuruiCd == model.ShuruiCd).OrderBy(a => a.ShishoYotoCd).ThenBy(a => a.YotoCd);
                }

                foreach (var item in result.ToList())
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = item.YotoCd,
                        Text = item.YotoCd + CoreConst.SEPARATOR + item.YotoNm
                    });
                }
            }

            selectListItem.Add(YOTO_OTHER_ITEM);
            */
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

    /// <summary>
    /// 品目サブ関連クラス
    /// </summary>
    [Serializable]
    public class HimmokuSubSelectList
    {
        /// <summary>
        /// 種類
        /// </summary>
        public SelectList Shurui { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        public SelectList Yoto { get; set; }

    }
}
 