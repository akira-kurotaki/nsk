using NskAppModelLibrary.Context;
using NskWeb.Areas.F01.Consts;
using NskWeb.Areas.F01.Models.D0101;
using NskWeb.Areas.F01.Models.D0102;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Pager;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.Text;
using System.Text.RegularExpressions;
using static CoreLibrary.Core.Consts.CoreConst;
using static CoreLibrary.Core.Utility.CsvUtil;
using NskWeb.Areas.F000.Models.D000000;

namespace NskWeb.Areas.F01.Controllers
{
    /// <summary>
    /// 加入者一覧
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [ExcludeSystemLockCheck]
    [Area("F01")]
    public class D0101Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D0101)
        /// </summary>
        private static readonly string SCREEN_ID_D0101 = "D0101";

        /// <summary>
        /// 画面ID(D0102)
        /// </summary>
        private static readonly string SCREEN_ID_D0102 = "D0102";

        /// <summary>
        /// 画面機能コード(D0101_001)
        /// </summary>
        private static readonly string SCREEN_KINO_ID_D0101_001 = "D0101_001";

        /// <summary>
        /// セッションキー(D0101)
        /// </summary>
        private static readonly string SESS_D0101 = SCREEN_ID_D0101 + "_" + "SCREEN";

        /// <summary>
        /// セッションキー(抽出条件)
        /// </summary>
        private static readonly string SESS_SQL_INFO_FOR_CSV = SCREEN_ID_D0101 + "_" + "SQL_INFO_FOR_CSV";

        /// <summary>
        /// 部分ビュー名
        /// </summary>
        private static readonly string PARTIAL_VIEW_NAME = "_D0101SearchResult";

        /// <summary>
        /// ページ0
        /// </summary>
        private static readonly string PAGE_0 = "0";

        /// <summary>
        /// CSV処理名
        /// </summary>
        private static readonly string D0101_CSVOUTPUT = "D0101_CsvOutput";

        /// <summary>
        /// CSV出力ファイル名（加入者一覧）
        /// </summary>
        private static readonly string D0101_CSVOUTPUT_FILE_NAME = "加入者一覧";

        /// <summary>
        /// 表示順
        /// </summary>
        private static readonly Dictionary<D0101SearchCondition.DisplaySortType?, string> allSortDic = new Dictionary<D0101SearchCondition.DisplaySortType?, string>
            {
                { D0101SearchCondition.DisplaySortType.Nendo, "a.nendo" },
                { D0101SearchCondition.DisplaySortType.TodofukenCd, "a.todofuken_cd" },
                { D0101SearchCondition.DisplaySortType.KumiaitoCd, "a.kumiaito_cd" },
                { D0101SearchCondition.DisplaySortType.ShishoCd, "a.shisho_cd" },
                { D0101SearchCondition.DisplaySortType.ShichosonCd, "a.shichoson_cd" },
                { D0101SearchCondition.DisplaySortType.DaichikuCd, "a.daichiku_cd" },
                { D0101SearchCondition.DisplaySortType.ShochikuCd, "a.shochiku_cd" },
                { D0101SearchCondition.DisplaySortType.HojinFullNm, "a.hojin_full_nm" },
            };

        /// <summary>
        /// 表示順の倫理名
        /// </summary>
        private static readonly Dictionary<D0101SearchCondition.DisplaySortType?, string> allSortDicNm = new Dictionary<D0101SearchCondition.DisplaySortType?, string>
            {
                { D0101SearchCondition.DisplaySortType.Nendo, "対象年度" },
                { D0101SearchCondition.DisplaySortType.TodofukenCd, "都道府県コード" },
                { D0101SearchCondition.DisplaySortType.KumiaitoCd, "組合等コード" },
                { D0101SearchCondition.DisplaySortType.ShishoCd, "支所コード" },
                { D0101SearchCondition.DisplaySortType.ShichosonCd, "市町村コード" },
                { D0101SearchCondition.DisplaySortType.DaichikuCd, "大地区コード" },
                { D0101SearchCondition.DisplaySortType.ShochikuCd, "小地区コード" },
                { D0101SearchCondition.DisplaySortType.HojinFullNm, "氏名又は法人名" },
            };
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D0101Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期化
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {
            // $$$$$$$$$$$$$$
            NSKPortalInfoModel model_portal = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (model_portal == null)
            {
                throw new AppException("ME00068", MessageUtil.Get("ME00068"));
            }
            //$$$$$$$$$$$$$$

            // セッションに検索条件がある場合
            if (SessionUtil.Get<D0101Model>(SESS_D0101, HttpContext) is D0101Model model)
            {
                if (model.SearchResult.Pager == null)
                {
                    // ページャーは空の場合
                    return View(SCREEN_ID_D0101, GetPageDataList(1, model));
                }

                return View(SCREEN_ID_D0101, GetPageDataList(model.SearchResult.Pager.CurrentPage, model));
            }

            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D0101, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }

            // 利用可能な支所一覧
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);

            model = new D0101Model(Syokuin, shishoList);

            // 画面初期化
            InitializeModel(model);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D0101, model, HttpContext);

            // パンくずリストを変更する
            SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>() { "D000000" }, HttpContext);

            // 加入者一覧画面を表示する
            return View(SCREEN_ID_D0101, model);
        }
        #endregion

        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="model"></param>
        private void InitializeModel(D0101Model model)
        {
            // 利用可能な支所一覧
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);
            model.SearchCondition.TodofukenDropDownList.ShishoList = shishoList;

            // 加入者情報リンクに対するユーザの参照可否を取得する。
            // 取得結果がFlase（参照不可）である場合、「加入者情報参照可否フラグ」＝"1"（参照不可）を設定する。
            model.KanyushaReferenceFlag = GetFlagStr(!ScreenSosaUtil.CanReference(SCREEN_ID_D0102, HttpContext));

            // 一覧表作成ボタンに対するユーザの参照可否を取得する。
            // 取得結果がFlase（参照不可）である場合、「一覧表作成ボタン参照可否フラグ」＝"1"（参照不可）を設定する。
            model.CreateCSVReferenceFlag = GetFlagStr(!ScreenSosaUtil.CanReference(SCREEN_KINO_ID_D0101_001, HttpContext));
        }

        #region 戻るイベント
        /// <summary>
        /// イベント名：戻る 
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            SessionClear();
            return Json(new { result = "success" });
        }
        #endregion

        /// <summary>
        /// セッションをクリアする
        /// </summary>
        private void SessionClear()
        {
            // セッションをクリアする
            SessionUtil.Remove(SESS_D0101, HttpContext);
            SessionUtil.Remove(SESS_SQL_INFO_FOR_CSV, HttpContext);
            SessionUtil.Remove(AppConst.SESS_NOGYOSHA_ID, HttpContext);
            SessionUtil.Remove(AppConst.SESS_NENDO, HttpContext);
        }

        #region クリアイベント
        /// <summary>
        /// イベント名：クリア
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Clear()
        {
            SessionClear();

            // 加入者一覧画面を表示する
            return RedirectToAction("Init");
        }
        #endregion

        #region 検索イベント
        /// <summary>
        /// イベント名：検索
        /// </summary>
        /// <param name="model">加入者一覧モデル</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind("SearchCondition")] D0101Model model)
        {
            // 属性チェックまたは独自チェックエラーの場合
            if (!ModelState.IsValid || CheckSearchCondition(model))
            {
                // 検索結果は非表示
                model.SearchCondition.IsDisplayFlag = false;
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D0101, model, HttpContext);
                return View(SCREEN_ID_D0101, model);
            }

            // 検索して、画面に返す
            return View(SCREEN_ID_D0101, GetPageDataList(1, model));
        }
        #endregion

        #region ページ分データ取得メソッド
        /// <summary>
        /// メソッド：ページ分データを取得する
        /// </summary>
        /// <param name="pageId">ページID</param>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果モデル</returns>
        private D0101Model GetPageDataList(int pageId, D0101Model model)
        {
            // 検索結果をクリアする
            model.SearchResult = new D0101SearchResult();
            //  加入者一覧(検索SQL)CSV出力用
            var sqlInfoForCsv = new D0101SqlInfoForCsv();
            // 検索フラグ設定
            model.SearchCondition.IsDisplayFlag = true;
            // 検索結果件数を取得する
            int totalCount = GetSearchResultCount(model, ref sqlInfoForCsv);
            // 検索件数を画面表示用モデルに設定する
            model.SearchResult.TotalCount = totalCount;
            // 検索結果は0件の場合
            if (totalCount == 0)
            {
                model.MessageArea2 = MessageUtil.Get("MI00011");
                // 画面エラーメッセージエリアにメッセージ設定
                ModelState.AddModelError("MessageArea2", MessageUtil.Get("MI00011"));
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D0101, model, HttpContext);
                return model;
            }
            // メッセージをクリアする
            model.MessageArea1 = string.Empty;
            model.MessageArea2 = string.Empty;
            ModelState.Remove("MessageArea1");
            ModelState.Remove("MessageArea2");
            // 検索結果表示数の取得
            int displayCount = GetDisplayCount(model);
            // ページIDの取得
            int intPageId = GetPageId(pageId, totalCount, displayCount);
            // 検索結果ページ分の取得
            model.SearchResult.TableRecords = GetPageData(model, displayCount * (intPageId - 1), displayCount, ref sqlInfoForCsv);
            // ページャーの初期化
            model.SearchResult.Pager = new Pagination(intPageId, displayCount, totalCount);
            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 抽出条件（ＤＢ検索仕様書No.2）を作成し、セッションに設定する。
            // 出力順（ＤＢ検索仕様書No.3）を作成し、セッションに設定する。
            SessionUtil.Set(SESS_SQL_INFO_FOR_CSV, sqlInfoForCsv, HttpContext);

            // 画面初期化
            InitializeModel(model);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D0101, model, HttpContext);

            return model;
        }
        #endregion

        #region ページャーイベント
        /// <summary>
        /// イベント名：ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Pager(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || PAGE_0 == id)
            {
                return BadRequest();
            }

            // セッションから加入者一覧モデルを取得する
            D0101Model model = SessionUtil.Get<D0101Model>(SESS_D0101, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 検索結果を取得する
            model = GetPageDataList(int.Parse(id), model);
            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D0101, model, HttpContext);
            return PartialViewAsJson(PARTIAL_VIEW_NAME, model);
        }
        #endregion

        #region 詳細イベント
        /// <summary>
        /// イベント名：詳細
        /// </summary>
        /// <param name="id">検索結果一覧のインデックス</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Detail(string id)
        {
            // 異常の場合
            if (!Regex.IsMatch(id, @"^[0-9]+$"))
            {
                return BadRequest();
            }

            // セッションから加入者一覧モデルを取得する
            D0101Model model = SessionUtil.Get<D0101Model>(SESS_D0101, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 明細データの取得
            D0101TableRecord record = model.SearchResult.TableRecords[int.Parse(id)];

            // 農業者IDをセッション（キー：D0101_NOGYOSHA_ID）にセットする。
            SessionUtil.Set(AppConst.SESS_NOGYOSHA_ID, record.NogyoshaId, HttpContext);

            // 対象年度をセッション（キー：D0101_NENDO）にセットする。
            SessionUtil.Set(AppConst.SESS_NENDO, record.Nendo, HttpContext);

            // 加入者情報の遷移元を設定する
            SessionUtil.Set(F01Const.D0102_SCREEN_FROM, SCREEN_ID_D0101, HttpContext);

            // 加入者情報画面へ遷移する
            return RedirectToAction("Init", "D0102");
        }
        #endregion

        #region 検索条件チェックメソッド
        /// <summary>
        /// 検索条件チェックメソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        private bool CheckSearchCondition(D0101Model model)
        {
            var checkFlg = false;

            if (model.SearchCondition.DisplaySort1.HasValue &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort2.ToString()) &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順1、表示順2、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", new string[] { "表示順" }));
                ModelState.AddModelError("SearchCondition.DisplaySort1", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort2", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort3", " ");
                checkFlg = true;
                return checkFlg;
            }

            if (model.SearchCondition.DisplaySort1.HasValue &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort2.ToString()))
            {
                // 表示順1、表示順2の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", new string[] { "表示順" }));
                ModelState.AddModelError("SearchCondition.DisplaySort1", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort2", " ");
                checkFlg = true;
                return checkFlg;
            }

            if (model.SearchCondition.DisplaySort1.HasValue &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順1、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", new string[] { "表示順" }));
                ModelState.AddModelError("SearchCondition.DisplaySort1", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort3", " ");
                checkFlg = true;
                return checkFlg;
            }

            if (model.SearchCondition.DisplaySort2.HasValue &&
                model.SearchCondition.DisplaySort2.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順2、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", new string[] { "表示順" }));
                ModelState.AddModelError("SearchCondition.DisplaySort2", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort3", " ");
                checkFlg = true;
                return checkFlg;
            }
            return checkFlg;
        }
        #endregion

        #region 検索結果件数取得メソッド
        /// <summary>
        /// 検索結果件数取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果件数</returns>
        private int GetSearchResultCount(D0101Model model, ref D0101SqlInfoForCsv sqlInfoForCsv)
        {
            // sql用定数定義
            var cntSql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 件数取得
            cntSql.Append("SELECT COUNT(1) AS \"Value\" FROM ( ");
            // 検索項目を取得する
            GetCondition(cntSql);
            // 検索テーブルを取得する
            GetTableCondition(cntSql);
            // 検索条件を取得する
            GetSearchCondition(cntSql, model, parameters, ref sqlInfoForCsv);
            // 検索結果を取得する
            GetResult(cntSql);

            cntSql.Append(")c ");

            // sql実行 
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<int>(cntSql.ToString(), parameters.ToArray()).Single();
        }
        #endregion

        #region 検索項目取得
        /// <summary>
        /// 検索項目を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetCondition(StringBuilder sql)
        {
            sql.Append(
                "WITH a AS (  " +
                "SELECT " +
                "  v_nogyosha.nogyosha_id, " +
                "  t_kanyusha.nendo, " +
                "  v_todofuken.todofuken_nm, " +
                "  v_kumiaito.kumiaito_nm, " +
                "  v_shisho_nm.shisho_nm, " +
                "  v_nogyosha.hojin_full_nm, " +
                "  v_nogyosha.todofuken_cd, " +
                "  v_nogyosha.kumiaito_cd, " +
                "  v_nogyosha.shisho_cd, " +
                "  v_nogyosha.daichiku_cd, " +
                "  v_nogyosha.shochiku_cd, " +
                "  v_nogyosha.shichoson_cd ");
        }
        #endregion

        #region 検索テーブル取得
        /// <summary>
        /// 検索テーブルを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private void GetTableCondition(StringBuilder sqlAll)
        {
            var table = new StringBuilder();
            table.Append(
                "FROM " +
                "  v_nogyosha " +
                "LEFT JOIN t_kanyusha  " +
                "  ON v_nogyosha.nogyosha_id = t_kanyusha.nogyosha_id " +
                "LEFT JOIN v_todofuken  " +
                "  ON v_nogyosha.todofuken_cd = v_todofuken.todofuken_cd " +
                "LEFT JOIN v_kumiaito " +
                "  ON v_nogyosha.todofuken_cd = v_kumiaito.todofuken_cd " +
                "  AND v_nogyosha.kumiaito_cd = v_kumiaito.kumiaito_cd " +
                "LEFT JOIN v_shisho_nm " +
                "  ON v_nogyosha.todofuken_cd = v_shisho_nm.todofuken_cd " +
                "  AND v_nogyosha.kumiaito_cd = v_shisho_nm.kumiaito_cd " +
                "  AND v_nogyosha.shisho_cd = v_shisho_nm.shisho_cd ");

            sqlAll.Append(table);
        }
        #endregion

        #region 検索条件取得
        /// <summary>
        /// 検索条件を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="sqlInfoForCsv"> 加入者一覧(検索SQL)CSV出力用</param>
        private void GetSearchCondition(StringBuilder sqlAll, D0101Model model, List<NpgsqlParameter> parameters, ref D0101SqlInfoForCsv sqlInfoForCsv)
        {
            Syokuin syokuin = Syokuin;

            var sql = new StringBuilder();
            var cdNm = new StringBuilder();
            var sqlParams = new List<SqlParam>();

            sql.Append("WHERE '1' = '1'");

            // 都道府県
            sql.Append("AND v_nogyosha.todofuken_cd = @TodofukenCd_1 ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd_1", syokuin.TodofukenCd));
            cdNm.Append("都道府県コード=" + syokuin.TodofukenCd + Environment.NewLine);
            sqlParams.Add(new SqlParam() { name = "@TodofukenCd_1", value = syokuin.TodofukenCd, type = (int)SqlParamType.String });

            // 組合等
            sql.Append("AND v_nogyosha.kumiaito_cd = @KumiaitoCd_1 ");
            parameters.Add(new NpgsqlParameter("@KumiaitoCd_1", syokuin.KumiaitoCd));
            cdNm.Append("組合等コード=" + syokuin.KumiaitoCd + Environment.NewLine);
            sqlParams.Add(new SqlParam() { name = "@KumiaitoCd_1", value = syokuin.KumiaitoCd, type = (int)SqlParamType.String });

            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            // 支所
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                sql.Append("AND v_nogyosha.shisho_cd = ANY (@ShishoList_1) ");
                parameters.Add(new NpgsqlParameter("@ShishoList_1", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).ToList()
                });
                cdNm.Append("利用可能支所一覧=" + string.Join(",", shishoList.Select(i => i.ShishoCd).ToList()) + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@ShishoList_1", value = string.Join(",", shishoList.Select(i => i.ShishoCd).ToList()), type = (int)SqlParamType.ArrayVarchar });
            }
            else if (!string.IsNullOrEmpty(syokuin.ShishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_1 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_1", syokuin.ShishoCd));
                cdNm.Append("支所（ログインユーザのセッション）=" + syokuin.ShishoCd + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@ShishoCd_1", value = syokuin.ShishoCd, type = (int)SqlParamType.String });
            }

            // 対象年度
            if (!string.IsNullOrEmpty(model.SearchCondition.Nendo))
            {
                sql.Append("AND t_kanyusha.nendo = @Nendo ");
                parameters.Add(new NpgsqlParameter("@Nendo", int.Parse(model.SearchCondition.Nendo)));
                cdNm.Append("対象年度=" + model.SearchCondition.Nendo + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@Nendo", value = model.SearchCondition.Nendo, type = (int)SqlParamType.Int });
            }

            // 支所
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShishoCd))
            {
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_2 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_2", model.SearchCondition.TodofukenDropDownList.ShishoCd));
                cdNm.Append("支所コード=" + model.SearchCondition.TodofukenDropDownList.ShishoCd + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@ShishoCd_2", value = model.SearchCondition.TodofukenDropDownList.ShishoCd, type = (int)SqlParamType.String });
            }

            // 市町村
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShichosonCd))
            {
                sql.Append("AND v_nogyosha.shichoson_cd = @ShichosonCd ");
                parameters.Add(new NpgsqlParameter("@ShichosonCd", model.SearchCondition.TodofukenDropDownList.ShichosonCd));
                cdNm.Append("市町村コード=" + model.SearchCondition.TodofukenDropDownList.ShichosonCd + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@ShichosonCd", value = model.SearchCondition.TodofukenDropDownList.ShichosonCd, type = (int)SqlParamType.String });
            }

            // 大地区
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.DaichikuCd))
            {
                sql.Append("AND v_nogyosha.daichiku_cd = @DaichikuCd ");
                parameters.Add(new NpgsqlParameter("@DaichikuCd", model.SearchCondition.TodofukenDropDownList.DaichikuCd));
                cdNm.Append("大地区コード=" + model.SearchCondition.TodofukenDropDownList.DaichikuCd + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@DaichikuCd", value = model.SearchCondition.TodofukenDropDownList.DaichikuCd, type = (int)SqlParamType.String });
            }

            // 小地区（開始）
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom))
            {
                sql.Append("AND v_nogyosha.shochiku_cd >= @ShochikuCdFrom ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdFrom", model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom));
                cdNm.Append("小地区（開始）コード=" + model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@ShochikuCdFrom", value = model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom, type = (int)SqlParamType.String });
            }

            // 小地区（終了）
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                sql.Append("AND v_nogyosha.shochiku_cd <= @ShochikuCdTo ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdTo", model.SearchCondition.TodofukenDropDownList.ShochikuCdTo));
                cdNm.Append("小地区（終了）コード=" + model.SearchCondition.TodofukenDropDownList.ShochikuCdTo + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@ShochikuCdTo", value = model.SearchCondition.TodofukenDropDownList.ShochikuCdTo, type = (int)SqlParamType.String });
            }

            // 氏名又は法人名
            if (!string.IsNullOrEmpty(model.SearchCondition.HojinFullNm))
            {
                // 全角スペース、半角スペースを除く
                var hojinFullNmInput = model.SearchCondition.HojinFullNm.Replace("　", "").Replace(" ", "");

                sql.Append("AND replace(replace(v_nogyosha.hojin_full_nm,'　',''),' ','') LIKE '%' || @HojinFullNm || '%' ESCAPE '\\' ");
                parameters.Add(new NpgsqlParameter("@HojinFullNm", StringUtil.RemoveEscapeChar(hojinFullNmInput)));
                cdNm.Append("氏名又は法人名（部分一致）=" + model.SearchCondition.HojinFullNm + Environment.NewLine);
                sqlParams.Add(new SqlParam() { name = "@HojinFullNm", value = StringUtil.RemoveEscapeChar(hojinFullNmInput), type = (int)SqlParamType.String });
            }

            // 「抽出条件」を設定する（加入者一覧ビューに対応するWhere句）
            sqlInfoForCsv.ConditionSqlStr = sql.ToString().Replace("WHERE", "").Replace("v_nogyosha.", "").Replace("t_kanyusha.", "");
            sqlInfoForCsv.WhereName = cdNm.ToString();
            sqlInfoForCsv.SqlParams = sqlParams;
            sqlAll.Append(sql);
        }
        #endregion

        #region 検索結果取得
        /// <summary>
        /// 検索結果を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetResult(StringBuilder sql)
        {
            sql.Append(
                ") " +
                "SELECT " +
                "  a.nogyosha_id AS NogyoshaId, " +
                "  a.nendo AS Nendo, " +
                "  a.todofuken_nm AS TodofukenNm, " +
                "  a.kumiaito_nm AS KumiaitoNm, " +
                "  a.shisho_nm AS ShishoNm, " +
                "  a.hojin_full_nm AS HojinFullNm " +
                "FROM " +
                "  a ");
        }
        #endregion

        #region 検索結果表示数取得メソッド
        /// <summary>
        /// 検索結果表示数取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果表示数</returns>
        private int GetDisplayCount(D0101Model model)
        {
            return model.SearchCondition.DisplayCount ?? CoreConst.PAGE_SIZE;
        }
        #endregion

        #region 検索結果のページ分取得メソッド
        /// <summary>
        /// 検索結果のページ分取得メソッド
        /// <summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <param name="sqlInfoForCsv"> 加入者一覧(検索SQL)CSV出力用</param>
        /// <param name="sqlInfoForCsv"> 加入者一覧(ソート順)CSV出力用</param>
        /// <returns>検索結果のページ分</returns>
        private List<D0101TableRecord> GetPageData(D0101Model model, int offset, int pageSize, ref D0101SqlInfoForCsv sqlInfoForCsv)
        {
            // 検索結果件数分データを取得
            var sqlResults = GetResult(model, ref sqlInfoForCsv, offset, pageSize);

            return sqlResults;
        }
        #endregion

        #region 検索情報取得メソッド
        /// <summary>
        /// メソッド：検索情報を取得する
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="sqlInfoForCsv"> 加入者一覧(検索SQL)CSV出力用</param>
        /// <param name="sqlInfoForCsv"> 加入者一覧(ソート順)CSV出力用</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <returns>検索情報</returns>
        private List<D0101TableRecord> GetResult(D0101Model model, ref D0101SqlInfoForCsv sqlInfoForCsv, int offset = 0, int pageSize = 10)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetTableCondition(sql);
            // 検索条件を取得する
            GetSearchCondition(sql, model, parameters, ref sqlInfoForCsv);
            // 検索結果を取得する
            GetResult(sql);

            // ソート順を追加する
            GetOrderby(sql, model, ref sqlInfoForCsv);

            // ソード順の末尾にあるカマを削除する
            sql = sql.Remove(sql.ToString().Length - 2, 1);

            // ページ分追加
            sql.Append(" LIMIT @PageSize ");
            sql.Append(" OFFSET @Offset ");
            parameters.Add(new NpgsqlParameter("@PageSize", pageSize));
            parameters.Add(new NpgsqlParameter("@Offset", offset));

            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D0101TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region クエリ式にソート順設定
        /// <summary>
        /// メソッド：クエリ式にソート順を設定する
        /// </summary>
        /// <param name="sql">検索SQL</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="sqlInfoForCsv"> 加入者一覧(ソート順)CSV出力用</param>
        /// <returns></returns>
        private void GetOrderby(StringBuilder sqlAll, D0101Model model, ref D0101SqlInfoForCsv sqlInfoForCsv)
        {
            var sql = new StringBuilder();
            var sort = new StringBuilder();
            // ソート順を追加する
            var sorts = new List<string>();
            // 表示順キー１ と　表示順キー２と表示順キー３が空の場合
            if (string.IsNullOrEmpty(model.SearchCondition.DisplaySort1.ToString()) &&
                string.IsNullOrEmpty(model.SearchCondition.DisplaySort2.ToString()) &&
                string.IsNullOrEmpty(model.SearchCondition.DisplaySort3.ToString()))
            {
                sql.Append(
                    "ORDER BY " +
                    "  a.nendo ASC, " +
                    "  a.todofuken_cd ASC, " +
                    "  a.kumiaito_cd ASC, " +
                    "  a.shisho_cd ASC, " +
                    "  a.shichoson_cd ASC, " +
                    "  a.daichiku_cd ASC, " +
                    "  a.shochiku_cd ASC, " +
                    "  a.hojin_full_nm ASC, " +
                    "  a.nogyosha_id ASC, ");

                sort.Append("対象年度　昇順" + Environment.NewLine);
                sort.Append("都道府県コード　昇順" + Environment.NewLine);
                sort.Append("組合等コード　昇順" + Environment.NewLine);
                sort.Append("支所コード　昇順" + Environment.NewLine);
                sort.Append("市町村コード　昇順" + Environment.NewLine);
                sort.Append("大地区コード　昇順" + Environment.NewLine);
                sort.Append("小地区コード　昇順" + Environment.NewLine);
                sort.Append("氏名又は法人名　昇順" + Environment.NewLine);
                sort.Append("農業者ID　昇順");
            }
            else
            {
                sql.Append("ORDER BY ");
                // 表示順1に値があるの場合
                if (!string.IsNullOrEmpty(model.SearchCondition.DisplaySort1.ToString()))
                {
                    SetKeyOrderby(sql, model.SearchCondition.DisplaySort1, model.SearchCondition.DisplaySortOrder1, ref sort);
                    sorts.Add(model.SearchCondition.DisplaySort1.ToString());
                }

                // 表示順2に値があるの場合
                if (!string.IsNullOrEmpty(model.SearchCondition.DisplaySort2.ToString()))
                {
                    SetKeyOrderby(sql, model.SearchCondition.DisplaySort2, model.SearchCondition.DisplaySortOrder2, ref sort);
                    sorts.Add(model.SearchCondition.DisplaySort2.ToString());
                }

                // 表示順3に値があるの場合
                if (!string.IsNullOrEmpty(model.SearchCondition.DisplaySort3.ToString()))
                {
                    SetKeyOrderby(sql, model.SearchCondition.DisplaySort3, model.SearchCondition.DisplaySortOrder3, ref sort);
                    sorts.Add(model.SearchCondition.DisplaySort3.ToString());
                }

                // [画面:表示順１]～[画面:表示順3]で「対象年度」が選択されない場合
                if (!sorts.Contains(D0101SearchCondition.DisplaySortType.Nendo.ToString()))
                {
                    sql.Append("a.nendo ASC, ");
                    sort.Append("対象年度　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「都道府県」が選択されない場合
                if (!sorts.Contains(D0101SearchCondition.DisplaySortType.TodofukenCd.ToString()))
                {
                    sql.Append("a.todofuken_cd ASC, ");
                    sort.Append("都道府県コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「組合等」が選択されない場合
                if (!sorts.Contains(D0101SearchCondition.DisplaySortType.KumiaitoCd.ToString()))
                {
                    sql.Append("a.kumiaito_cd ASC, ");
                    sort.Append("組合等コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「支所」が選択されない場合
                if (!sorts.Contains(D0101SearchCondition.DisplaySortType.ShishoCd.ToString()))
                {
                    sql.Append("a.shisho_cd ASC, ");
                    sort.Append("支所コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「市町村」が選択されない場合
                if (!sorts.Contains(D0101SearchCondition.DisplaySortType.ShichosonCd.ToString()))
                {
                    sql.Append("a.shichoson_cd ASC, ");
                    sort.Append("市町村コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「大地区」が選択されない場合
                if (!sorts.Contains(D0101SearchCondition.DisplaySortType.DaichikuCd.ToString()))
                {
                    sql.Append("a.daichiku_cd ASC, ");
                    sort.Append("大地区コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「小地区」が選択されない場合
                if (!sorts.Contains(D0101SearchCondition.DisplaySortType.ShochikuCd.ToString()))
                {
                    sql.Append("a.shochiku_cd ASC, ");
                    sort.Append("小地区コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で[氏名または法人名]が選択されない場合
                if (!sorts.Contains(D0101SearchCondition.DisplaySortType.HojinFullNm.ToString()))
                {
                    sql.Append("a.hojin_full_nm ASC, ");
                    sort.Append("氏名または法人名　昇順" + Environment.NewLine);
                }

                // 農業者IDソート順の設定
                sql.Append("a.nogyosha_id ASC, ");
                sort.Append("農業者ID　昇順");
            }

            // 「出力順」を設定する（加入者一覧ビューに対応するOrder By句）
            sqlInfoForCsv.OrderBySqlStr = sql.ToString().Replace("ORDER BY", "").Replace("a.", "").TrimEnd(',', ' ');
            sqlInfoForCsv.OrderByName = sort.ToString();

            sqlAll.Append(sql);
        }
        #endregion

        #region 表示順キーによってソート順設定メソッド
        /// <summary>
        /// メソッド：表示順キーによってソート順を設定する
        /// </summary>
        /// <param name="sql">検索SQL</param>
        /// <param name="sortKey">表示順キー</param>
        /// <param name="sortOrder">表示順</param>
        /// <param name="sort">CSV出力用ソート順</param>
        /// <returns></returns>
        private void SetKeyOrderby(StringBuilder sql, D0101SearchCondition.DisplaySortType? sortKey, CoreConst.SortOrder sortOrder, ref StringBuilder sort)
        {
            // 表示順キーと表示順によって、表示順をsqlに追加する
            string sortKeyName = allSortDic[sortKey];
            sql.Append(sortKeyName);
            if (CoreConst.SortOrder.ASC.Equals(sortOrder))
            {
                sql.Append(" ASC, ");
                sort.Append(allSortDicNm[sortKey] + " 昇順" + Environment.NewLine);
            }
            else
            {
                sql.Append(" DESC, ");
                sort.Append(allSortDicNm[sortKey] + " 降順" + Environment.NewLine);
            }
        }
        #endregion

        #region 帳票作成「一覧表作成」イベント
        /// <summary>
        /// イベント名：一覧表作成（CSV）
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult CreateListCsv(string CharacterCode)
        {
            // 異常の場合
            if (string.IsNullOrEmpty(CharacterCode) ||
                !new string[] { CoreConst.CharacterCode.SJIS.ToString(), CoreConst.CharacterCode.UTF8.ToString() }.Contains(CharacterCode))
        {
                return BadRequest();
            }

            // セッション情報取得
            var model = SessionUtil.Get<D0101Model>(SESS_D0101, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 押下されたボタンが非表示（「一覧表作成ボタン参照可否」＝"1"（参照不可））の場合、業務エラー画面を表示する。
            if (AppConst.FLG_ON.Equals(model.CreateCSVReferenceFlag))
            {
                throw new SystemException(MessageUtil.Get("MF00008"));
            }

            var csvMessage = string.Empty;
            byte[] csvzipfile = null;
            // CSV作成インターフェースに引数を渡す
            var result = CsvUtil.CsvOutput(Syokuin.UserId,
                                        ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                                        Syokuin.TodofukenCd,
                                        Syokuin.KumiaitoCd,
                                        Syokuin.ShishoCd,
                                        D0101_CSVOUTPUT,
                                        CoreConst.FLG_OFF,
                                        true,
                                        D0101_CSVOUTPUT_FILE_NAME + CoreConst.FILE_EXTENSION_ZIP,
                                        SetDisplayJouken(),
                                        SetListCsvOutputJoken(CharacterCode),
                                        getJigyoDb<NskAppContext>(),
                                        ref csvMessage,
                                        ref csvzipfile);

            // 共通機能の戻り値.エラーメッセージが空ではない場合
            if (!string.IsNullOrEmpty(csvMessage))
            {
                // 完了メッセージダイアログを表示する。
                // ダイアログのメッセージ：共通機能の戻り値.エラーメッセージ
                return Json(new { message = csvMessage });
            }

            // 共通機能の返却値（処理結果）＝11（バッチ予約警告）の場合
            if (CsvUtil.RET_WARN_BATCH_RESERVE.Equals(result))
            {
                // 確認ダイアログを表示する。
                // （メッセージID：MQ00011）
                return Json(new { alertMessage = MessageUtil.Get("MQ00011", "加入者一覧") });
            }

            // ZIPファイルが返却された場合
            if (csvzipfile != null)
            {
                var fileName = D0101_CSVOUTPUT_FILE_NAME + CoreConst.FILE_EXTENSION_ZIP;
                return File(csvzipfile, GetMimeTypeForFileExtension(fileName), fileName, true);
            }

            return Json(new { message = MessageUtil.Get("MF00001") });
        }
        #endregion

        #region バッチ帳票作成「一覧表作成」イベント
        /// <summary>
        /// イベント名：一覧表作成（CSV）
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult CreateBatchYoyaku(string CharacterCode)
        {
            // 異常の場合
            if (string.IsNullOrEmpty(CharacterCode) ||
                !new string[] { CoreConst.CharacterCode.SJIS.ToString(), CoreConst.CharacterCode.UTF8.ToString() }.Contains(CharacterCode))
        {
                return BadRequest();
            }

            // セッション情報取得
            var model = SessionUtil.Get<D0101Model>(SESS_D0101, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 押下されたボタンが非表示（「一覧表作成ボタン参照可否」＝"1"（参照不可））の場合、業務エラー画面を表示する。
            if (AppConst.FLG_ON.Equals(model.CreateCSVReferenceFlag))
            {
                throw new SystemException(MessageUtil.Get("MF00008"));
            }

            var csvMessage = string.Empty;
            byte[] csvzipfile = null;
            // CSV作成インターフェースに引数を渡す
            var result = CsvUtil.CsvOutput(Syokuin.UserId,
                                        ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                                        Syokuin.TodofukenCd,
                                        Syokuin.KumiaitoCd,
                                        Syokuin.ShishoCd,
                                        D0101_CSVOUTPUT,
                                        CoreConst.FLG_OFF,
                                        false,
                                        D0101_CSVOUTPUT_FILE_NAME + CoreConst.FILE_EXTENSION_ZIP,
                                        SetDisplayJouken(),
                                        SetListCsvOutputJoken(CharacterCode),
                                        getJigyoDb<NskAppContext>(),
                                        ref csvMessage,
                                        ref csvzipfile);

            // 共通機能の戻り値.エラーメッセージが空ではない場合
            if (!string.IsNullOrEmpty(csvMessage))
            {
                // 完了メッセージダイアログを表示する。
                // （メッセージID：ME90005)
                return Json(new { message = MessageUtil.Get("ME90005") });
            }

            // CSV作成インターフェースの戻り値が成功の場合
            // （メッセージID：MI00004、引数{ 0} ： "CSV作成バッチ処理の予約")
            return Json(new { message = MessageUtil.Get("MI00004", "CSV作成バッチ処理の予約") });
        }
        #endregion

        #region 各種補助メソッド
        /// <summary>
        /// フラグの文字列表現を取得する。
        /// </summary>
        /// <param name="flag">フラグ</param>
        /// <returns>true："1"、false："0"</returns>
        private string GetFlagStr(bool flag)
        {
            return flag ? AppConst.FLG_ON : AppConst.FLG_OFF;
        }

        /// <summary>
        /// バッチ条件（表示用）設定
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="joukenId">条件ID</param>/// 
        /// <returns>設定したバッチ条件（表示用）</returns>
        private string SetDisplayJouken()
        {
            // 抽出条件
            var sqlInfoForCsv = SessionUtil.Get<D0101SqlInfoForCsv>(SESS_SQL_INFO_FOR_CSV, HttpContext);
            // 抽出条件、 出力順取得できない場合
            if (sqlInfoForCsv == null || sqlInfoForCsv == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "抽出条件、出力順を取得できませんでした"));
            }

            var sb = new StringBuilder();
            sb.Append("（対象）" + Environment.NewLine);
            sb.Append(D0101_CSVOUTPUT_FILE_NAME + Environment.NewLine);

            // 条件設定
            sb.Append("（条件）" + Environment.NewLine);
            sb.Append(sqlInfoForCsv.WhereName);

            // （並び順）
            sb.Append("（並び順）" + Environment.NewLine);
            sb.Append(sqlInfoForCsv.OrderByName + Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// CSV出力条件クラスList設定
        /// </summary>
        /// <param name="CharacterCode">[画面：文字コード]</param>
        /// <returns>CSV出力条件クラスList</returns>
        /// <exception cref="SystemException">出条件、 出力順取得できない</exception>
        private List<CsvOutputJoken> SetListCsvOutputJoken(string CharacterCode)
        {
            // 抽出条件
            var sqlInfoForCsv = SessionUtil.Get<D0101SqlInfoForCsv>(SESS_SQL_INFO_FOR_CSV, HttpContext);
            // 抽出条件、 出力順取得できない場合
            if (sqlInfoForCsv == null || sqlInfoForCsv == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "抽出条件、出力順を取得できませんでした"));
            }

            var csvOutputJokenList = new List<CsvOutputJoken>();
            var csvOutputJoken = new CsvOutputJoken()
            {
                SearchTaget = "v_kanyusha_ichiran",
                SqlConf = sqlInfoForCsv.ConditionSqlStr,
                SqlParams = sqlInfoForCsv.SqlParams,
                SqlOrder = sqlInfoForCsv.OrderBySqlStr,
                CharacterCd = ((CharacterCode)Enum.Parse(typeof(CharacterCode), CharacterCode)).ToString("d"),
                CsvNm = D0101_CSVOUTPUT_FILE_NAME + FILE_EXTENSION_CSV,
                HeaderOnOff = true,
                SeparatorFont = Separator.COMMA.ToString("d"),
                BomOnOff = CoreConst.CharacterCode.SJIS.ToString().Equals(CharacterCode) ? false : true,
            };

            csvOutputJokenList.Add(csvOutputJoken);
            return csvOutputJokenList;
        }
        #endregion
    }
}
