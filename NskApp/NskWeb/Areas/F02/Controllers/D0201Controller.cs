using NskAppModelLibrary.Context;
using NskWeb.Areas.F02.Models.D0201;
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
using Microsoft.Extensions.Primitives;
using ModelLibrary.Models;
using Npgsql;
using NpgsqlTypes;
using ReportService.Core;
using System.Text;
using System.Text.RegularExpressions;
using static NskWeb.Areas.F02.Models.D0201.D0201SearchCondition;

namespace NskWeb.Areas.F02.Controllers
{
    /// <summary>
    /// 一括帳票出力
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [Area("F02")]
    public class D0201Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D0201)
        /// </summary>
        private static readonly string SCREEN_ID_D0201 = "D0201";

        /// <summary>
        /// セッションキー(D0201)
        /// </summary>
        private static readonly string SESS_D0201 = SCREEN_ID_D0201 + "_" + "SCREEN";

        /// <summary>
        /// 部分ビュー名（検索結果）
        /// </summary>
        private static readonly string RESULT_VIEW_NAME = "_D0201SearchResult";

        /// <summary>
        /// 帳票制御ID:H001
        /// </summary>
        private static readonly string REPORT_CONTROL_ID_H001 = "H001";

        /// <summary>
        /// 帳票制御ID:H002
        /// </summary>
        private static readonly string REPORT_CONTROL_ID_H002 = "H002";

        /// <summary>
        /// 予約を実行した処理名(選択作成)
        /// </summary>
        private static readonly string D0201_REPORT_YOYAKU_SHORI_NM_SELECT = "D0201_CreateSelect";

        /// <summary>
        /// 予約を実行した処理名(全件作成)
        /// </summary>
        private static readonly string D0201_REPORT_YOYAKU_SHORI_NM_ALL = "D0201_CreateAll";

        /// <summary>
        /// バッチ名
        /// </summary>
        private static readonly string D0201_BATCH_NM = "一括帳票出力";

        /// <summary>
        /// ページ0
        /// </summary>
        private static readonly string PAGE_0 = "0";

        /// <summary>
        /// 表示順
        /// </summary>
        private static readonly Dictionary<DisplaySortType?, string> allSortDic = new Dictionary<DisplaySortType?, string>
            {
                { DisplaySortType.Nendo, "t_kanyusha.nendo" },
                { DisplaySortType.TodofukenCd, "v_nogyosha.todofuken_cd" },
                { DisplaySortType.KumiaitoCd, "v_nogyosha.kumiaito_cd" },
                { DisplaySortType.ShishoCd, "v_nogyosha.shisho_cd" },
                { DisplaySortType.ShichosonCd, "v_nogyosha.shichoson_cd" },
                { DisplaySortType.DaichikuCd, "v_nogyosha.daichiku_cd" },
                { DisplaySortType.ShochikuCd, "v_nogyosha.shochiku_cd" },
                { DisplaySortType.HojinFullNm, "v_nogyosha.hojin_full_nm" },
                { DisplaySortType.KanyushaCd, "t_kanyusha.kanyusha_cd" },
            };

        /// <summary>
        /// 表示順の倫理名
        /// </summary>
        private static readonly Dictionary<DisplaySortType?, string> allSortDicNm = new Dictionary<DisplaySortType?, string>
            {
                { DisplaySortType.Nendo, "対象年度" },
                { DisplaySortType.TodofukenCd, "都道府県コード" },
                { DisplaySortType.KumiaitoCd, "組合等コード" },
                { DisplaySortType.ShishoCd, "支所コード" },
                { DisplaySortType.ShichosonCd, "市町村コード" },
                { DisplaySortType.DaichikuCd, "大地区コード" },
                { DisplaySortType.ShochikuCd, "小地区コード" },
                { DisplaySortType.HojinFullNm, "氏名又は法人名" },
                { DisplaySortType.KanyushaCd, "加入者管理コード" },
            };
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D0201Controller(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
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
            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D0201, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }

            // セッションに検索条件がある場合
            if (SessionUtil.Get<D0201Model>(SESS_D0201, HttpContext) is D0201Model model)
            {
                return View(SCREEN_ID_D0201, GetPageDataList(model.SearchResult.Pager?.CurrentPage, model));
            }

            // モデル状態ディクショナリからすべての項目を削除します。       
            ModelState.Clear();
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D0201, HttpContext);

            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
            // 画面項目の初期化
            model = new D0201Model(Syokuin, shishoList);

            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D0201, model, HttpContext);

            // パンくずリストを変更する
            // SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>() { "D000000" }, HttpContext);

            // 一括帳票出力画面を表示する
            return View(SCREEN_ID_D0201, model);
        }
        #endregion

        #region 戻るイベント
        /// <summary>
        /// イベント名：戻る 
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D0201, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion

        #region クリアイベント
        /// <summary>
        /// イベント名：クリア
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Clear()
        {
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D0201, HttpContext);

            // 一括帳票出力画面を表示する
            return RedirectToAction("Init");
        }
        #endregion

        #region 検索イベント
        /// <summary>
        /// イベント名：検索
        /// </summary>
        /// <param name="model">一括帳票出力モデル</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind("SearchCondition")] D0201Model model)
        {
            // 属性チェックまたは独自チェックエラーの場合
            if (!ModelState.IsValid || CheckSearchCondition(model))
            {
                // 検索結果は非表示
                model.SearchCondition.IsResultDisplay = false;
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D0201, model, HttpContext);
                return View(SCREEN_ID_D0201, model);
            }

            // 検索して、画面に返す
            return View(SCREEN_ID_D0201, GetPageDataList(1, model));
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

            // セッションから一括帳票出力モデルを取得する
            D0201Model model = SessionUtil.Get<D0201Model>(SESS_D0201, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 検索結果を取得する
            model = GetPageDataList(int.Parse(id), model);
            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D0201, model, HttpContext);
            return PartialViewAsJson(RESULT_VIEW_NAME, model);
        }
        #endregion

        #region ページ分データ取得メソッド
        /// <summary>
        /// メソッド：ページ分データを取得する
        /// </summary>
        /// <param name="pageId">ページID</param>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果モデル</returns>
        private D0201Model GetPageDataList(int? pageId, D0201Model model)
        {
            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();
            // 検索結果をクリアする
            model.SearchResult = new D0201SearchResult();
            // 検索フラグ設定
            model.SearchCondition.IsResultDisplay = true;
            // 検索結果件数を取得する
            var totalCount = GetSearchResultCount(model);
            // 検索件数を画面表示用モデルに設定する
            model.SearchResult.TotalCount = totalCount;
            // 検索結果は0件の場合
            if (totalCount == 0)
            {
                model.MessageArea2 = MessageUtil.Get("MI00011");
                // 画面エラーメッセージエリアにメッセージ設定
                ModelState.AddModelError("MessageArea2", MessageUtil.Get("MI00011"));
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D0201, model, HttpContext);
                return model;
            }
            // 検索結果表示数の取得
            var displayCount = GetDisplayCount(model);
            // ページIDの取得
            var intPageId = GetPageId((pageId ?? 1), totalCount, displayCount);
            // 検索結果ページ分の取得
            model.SearchResult.TableRecords = GetPageData(model, displayCount * (intPageId - 1), displayCount);
            // ページャーの初期化
            model.SearchResult.Pager = new Pagination(intPageId, displayCount, totalCount);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D0201, model, HttpContext);

            return model;
        }
        #endregion

        #region 検索条件チェックメソッド
        /// <summary>
        /// 検索条件チェックメソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        private bool CheckSearchCondition(D0201Model model)
        {
            var checkFlg = false;

            // [画面：保険年度（終了）]<[画面：保険年度（開始）]の場合、エラーと判定する
            if (!string.IsNullOrEmpty(model.SearchCondition.NendoFrom) && !string.IsNullOrEmpty(model.SearchCondition.NendoTo) &&
                int.Parse(model.SearchCondition.NendoTo) < int.Parse(model.SearchCondition.NendoFrom))
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90006"));
                ModelState.AddModelError("SearchCondition.NendoFrom", " ");
                ModelState.AddModelError("SearchCondition.NendoTo", " ");
                checkFlg = true;
            }

            // ソート順選択値重複チェック
            if (model.SearchCondition.DisplaySort1.HasValue &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort2.ToString()) &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順1、表示順2、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", "表示順"));
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
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", "表示順"));
                ModelState.AddModelError("SearchCondition.DisplaySort1", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort2", " ");
                checkFlg = true;
                return checkFlg;
            }

            if (model.SearchCondition.DisplaySort1.HasValue &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順1、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", "表示順"));
                ModelState.AddModelError("SearchCondition.DisplaySort1", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort3", " ");
                checkFlg = true;
                return checkFlg;
            }

            if (model.SearchCondition.DisplaySort2.HasValue &&
                model.SearchCondition.DisplaySort2.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順2、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", "表示順"));
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
        private int GetSearchResultCount(D0201Model model)
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
            GetSearchCondition(cntSql, model, parameters);

            cntSql.Append(") ");

            // sql実行 
            logger.Info("検索結果件数取得処理を実行します。");
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
            sql.Append("SELECT ");
            sql.Append("    false AS SelectCheck ");
            sql.Append("    , v_nogyosha.nogyosha_id AS NogyoshaId ");
            sql.Append("    , t_kanyusha.nendo AS Nendo ");
            sql.Append("    , t_kanyusha.kanyusha_cd AS KanyushaCd ");
            sql.Append("    , v_todofuken.todofuken_nm AS TodofukenNm ");
            sql.Append("    , v_kumiaito.kumiaito_nm AS KumiaitoNm ");
            sql.Append("    , v_shisho_nm.shisho_nm AS ShishoNm ");
            sql.Append("    , v_nogyosha.hojin_full_nm AS HojinFullNm ");
        }
        #endregion

        #region 検索テーブル取得
        /// <summary>
        /// 検索テーブルを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private void GetTableCondition(StringBuilder sql)
        {
            sql.Append("FROM ");
            sql.Append("    v_nogyosha ");
            sql.Append("    LEFT JOIN t_kanyusha ");
            sql.Append("        ON v_nogyosha.nogyosha_id = t_kanyusha.nogyosha_id ");
            sql.Append("    LEFT JOIN v_todofuken ");
            sql.Append("        ON v_nogyosha.todofuken_cd = v_todofuken.todofuken_cd ");
            sql.Append("    LEFT JOIN v_kumiaito ");
            sql.Append("        ON v_nogyosha.todofuken_cd = v_kumiaito.todofuken_cd ");
            sql.Append("        AND v_nogyosha.kumiaito_cd = v_kumiaito.kumiaito_cd ");
            sql.Append("    LEFT JOIN v_shisho_nm ");
            sql.Append("        ON v_nogyosha.todofuken_cd = v_shisho_nm.todofuken_cd ");
            sql.Append("        AND v_nogyosha.kumiaito_cd = v_shisho_nm.kumiaito_cd ");
            sql.Append("        AND v_nogyosha.shisho_cd = v_shisho_nm.shisho_cd ");
        }
        #endregion

        #region 検索条件取得
        /// <summary>
        /// 検索条件を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSearchCondition(StringBuilder sql, D0201Model model, List<NpgsqlParameter> parameters)
        {
            Syokuin syokuin = Syokuin;

            sql.Append("WHERE '1' = '1'");

            // 都道府県
            sql.Append("AND v_nogyosha.todofuken_cd = @TodofukenCd ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));

            // 組合等
            sql.Append("AND v_nogyosha.kumiaito_cd = @KumiaitoCd ");
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));

            // 支所
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                sql.Append("AND v_nogyosha.shisho_cd = ANY (@ShishoList) ");
                parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).ToList()
                });
            }
            else if (!string.IsNullOrEmpty(syokuin.ShishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_1 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_1", syokuin.ShishoCd));
            }

            // [画面：対象年度（開始）]
            if (!string.IsNullOrEmpty(model.SearchCondition.NendoFrom))
            {
                sql.Append("AND t_kanyusha.nendo >= @NendoFrom ");
                parameters.Add(new NpgsqlParameter("@NendoFrom", short.Parse(model.SearchCondition.NendoFrom)));
            }

            // [画面：対象年度（終了）]
            if (!string.IsNullOrEmpty(model.SearchCondition.NendoTo))
            {
                sql.Append("AND t_kanyusha.nendo <= @NendoTo ");
                parameters.Add(new NpgsqlParameter("@NendoTo", short.Parse(model.SearchCondition.NendoTo)));
            }

            // 支所
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShishoCd))
            {
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_2 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_2", model.SearchCondition.TodofukenDropDownList.ShishoCd));
            }

            // 市町村
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShichosonCd))
            {
                sql.Append("AND v_nogyosha.shichoson_cd = @ShichosonCd ");
                parameters.Add(new NpgsqlParameter("@ShichosonCd", model.SearchCondition.TodofukenDropDownList.ShichosonCd));
            }

            // 大地区
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.DaichikuCd))
            {
                sql.Append("AND v_nogyosha.daichiku_cd = @DaichikuCd ");
                parameters.Add(new NpgsqlParameter("@DaichikuCd", model.SearchCondition.TodofukenDropDownList.DaichikuCd));
            }

            // 小地区（開始）
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom))
            {
                sql.Append("AND v_nogyosha.shochiku_cd >= @ShochikuCdFrom ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdFrom", model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom));
            }

            // 小地区（終了）
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                sql.Append("AND v_nogyosha.shochiku_cd <= @ShochikuCdTo ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdTo", model.SearchCondition.TodofukenDropDownList.ShochikuCdTo));
            }

            // 加入者管理コード（開始）
            if (!string.IsNullOrEmpty(model.SearchCondition.KanyushaCdFrom))
            {
                sql.Append("AND t_kanyusha.kanyusha_cd >= @KanyushaCdFrom ");
                parameters.Add(new NpgsqlParameter("@KanyushaCdFrom", model.SearchCondition.KanyushaCdFrom));
            }

            // 加入者管理コード（終了）
            if (!string.IsNullOrEmpty(model.SearchCondition.KanyushaCdTo))
            {
                sql.Append("AND t_kanyusha.kanyusha_cd <= @KanyushaCdTo ");
                parameters.Add(new NpgsqlParameter("@KanyushaCdTo", model.SearchCondition.KanyushaCdTo));
            }

            // 氏名又は法人名
            if (!string.IsNullOrEmpty(model.SearchCondition.HojinFullNm))
            {
                // 全角スペース、半角スペースを除く
                var hojinFullNmInput = model.SearchCondition.HojinFullNm.Replace("　", "").Replace(" ", "");

                sql.Append("AND replace(replace(v_nogyosha.hojin_full_nm,'　',''),' ','') LIKE '%' || @HojinFullNm || '%' ESCAPE '\\' ");
                parameters.Add(new NpgsqlParameter("@HojinFullNm", StringUtil.RemoveEscapeChar(hojinFullNmInput)));
            }
        }
        #endregion

        #region 検索結果表示数取得メソッド
        /// <summary>
        /// 検索結果表示数取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果表示数</returns>
        private int GetDisplayCount(D0201Model model)
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
        /// <returns>検索結果のページ分</returns>
        private List<D0201TableRecord> GetPageData(D0201Model model, int offset, int pageSize)
        {
            // 検索結果件数分データを取得
            var sqlResults = GetResult(model, offset, pageSize);

            return sqlResults;
        }
        #endregion

        #region 検索情報取得メソッド
        /// <summary>
        /// メソッド：検索情報を取得する
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <returns>検索情報</returns>
        private List<D0201TableRecord> GetResult(D0201Model model, int offset = 0, int pageSize = 10)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetTableCondition(sql);
            // 検索条件を取得する
            GetSearchCondition(sql, model, parameters);

            // ソート順を追加する
            GetOrderby(sql, model);

            // ページ分追加
            sql.Append(" LIMIT @PageSize ");
            sql.Append(" OFFSET @Offset ");
            parameters.Add(new NpgsqlParameter("@PageSize", pageSize));
            parameters.Add(new NpgsqlParameter("@Offset", offset));

            logger.Info("ページ分検索結果情報取得処理を実行します。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D0201TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region クエリ式にソート順設定
        /// <summary>
        /// メソッド：クエリ式にソート順を設定する
        /// </summary>
        /// <param name="sb">文字ビューター</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="isSearch">検索処理かどうか（ture:検索処理、false:検索処理ではない）</param>
        /// <returns></returns>
        private void GetOrderby(StringBuilder sb, D0201Model model, bool isSearch = true)
        {
            // 表示順キー１ と　表示順キー２と表示順キー３が空の場合
            if (string.IsNullOrEmpty(model.SearchCondition.DisplaySort1.ToString()) &&
                string.IsNullOrEmpty(model.SearchCondition.DisplaySort2.ToString()) &&
                string.IsNullOrEmpty(model.SearchCondition.DisplaySort3.ToString()))
            {
                if (!isSearch)
                {
                    sb.Append("対象年度　昇順" + Environment.NewLine);
                    sb.Append("都道府県コード　昇順" + Environment.NewLine);
                    sb.Append("組合等コード　昇順" + Environment.NewLine);
                    sb.Append("支所コード　昇順" + Environment.NewLine);
                    sb.Append("市町村コード　昇順" + Environment.NewLine);
                    sb.Append("大地区コード　昇順" + Environment.NewLine);
                    sb.Append("小地区コード　昇順" + Environment.NewLine);
                    sb.Append("加入者管理コード　昇順" + Environment.NewLine);
                    sb.Append("氏名又は法人名　昇順" + Environment.NewLine);
                    sb.Append("農業者ID　昇順");
                }
                else
                {
                    sb.Append("ORDER BY ");
                    sb.Append("t_kanyusha.nendo ASC, ");
                    sb.Append("v_nogyosha.todofuken_cd ASC, ");
                    sb.Append("v_nogyosha.kumiaito_cd ASC, ");
                    sb.Append("v_nogyosha.shisho_cd ASC, ");
                    sb.Append("v_nogyosha.shichoson_cd ASC, ");
                    sb.Append("v_nogyosha.daichiku_cd ASC, ");
                    sb.Append("v_nogyosha.shochiku_cd ASC, ");
                    sb.Append("t_kanyusha.kanyusha_cd ASC, ");
                    sb.Append("v_nogyosha.hojin_full_nm ASC, ");
                    sb.Append("v_nogyosha.nogyosha_id ASC ");
                }
            }
            else
            {
                // ソート順を追加する
                var sorts = new List<string>();
                if (isSearch)
                {
                    sb.Append("ORDER BY ");
                }
                // 表示順1に値があるの場合
                if (!string.IsNullOrEmpty(model.SearchCondition.DisplaySort1.ToString()))
                {
                    SetKeyOrderby(sb, model.SearchCondition.DisplaySort1, model.SearchCondition.DisplaySortOrder1, isSearch);
                    sorts.Add(model.SearchCondition.DisplaySort1.ToString());
                }

                // 表示順2に値があるの場合
                if (!string.IsNullOrEmpty(model.SearchCondition.DisplaySort2.ToString()))
                {
                    SetKeyOrderby(sb, model.SearchCondition.DisplaySort2, model.SearchCondition.DisplaySortOrder2, isSearch);
                    sorts.Add(model.SearchCondition.DisplaySort2.ToString());
                }

                // 表示順3に値があるの場合
                if (!string.IsNullOrEmpty(model.SearchCondition.DisplaySort3.ToString()))
                {
                    SetKeyOrderby(sb, model.SearchCondition.DisplaySort3, model.SearchCondition.DisplaySortOrder3, isSearch);
                    sorts.Add(model.SearchCondition.DisplaySort3.ToString());
                }

                // [画面:表示順１]～[画面:表示順3]で「対象年度」が選択されない場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.Nendo.ToString()))
                {
                    sb.Append(isSearch ? "t_kanyusha.nendo ASC, " : "対象年度　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「都道府県」が選択されない場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.TodofukenCd.ToString()))
                {
                    sb.Append(isSearch ? "v_nogyosha.todofuken_cd ASC, " : "都道府県コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「組合等」が選択されない場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.KumiaitoCd.ToString()))
                {
                    sb.Append(isSearch ? "v_nogyosha.kumiaito_cd ASC, " : "組合等コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「支所」が選択されない場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.ShishoCd.ToString()))
                {
                    sb.Append(isSearch ? "v_nogyosha.shisho_cd ASC, " : "支所コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「市町村」が選択されている場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.ShichosonCd.ToString()))
                {
                    sb.Append(isSearch ? "v_nogyosha.shichoson_cd ASC, " : "市町村コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「大地区」が選択されない場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.DaichikuCd.ToString()))
                {
                    sb.Append(isSearch ? "v_nogyosha.daichiku_cd ASC, " : "大地区コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「小地区」が選択されない場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.ShochikuCd.ToString()))
                {
                    sb.Append(isSearch ? "v_nogyosha.shochiku_cd ASC, " : "小地区コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「加入者管理コード」が選択されている場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.KanyushaCd.ToString()))
                {
                    sb.Append(isSearch ? "t_kanyusha.kanyusha_cd ASC, " : "加入者管理コード　昇順" + Environment.NewLine);
                }

                // [画面:表示順１]～[画面:表示順3]で「[氏名または法人名」が選択されている場合
                if (!sorts.Contains(D0201SearchCondition.DisplaySortType.HojinFullNm.ToString()))
                {
                    sb.Append(isSearch ? "v_nogyosha.hojin_full_nm ASC, " : "氏名または法人名　昇順" + Environment.NewLine);
                }

                // 農業者IDソート順の設定
                sb.Append(isSearch ? "v_nogyosha.nogyosha_id ASC " : "農業者ID　昇順");
            }
        }
        #endregion

        #region 表示順キーによってソート順設定メソッド
        /// <summary>
        /// メソッド：表示順キーによってソート順を設定する
        /// </summary>
        /// <param name="sql">検索SQL</param>
        /// <param name="sortKey">表示順キー</param>
        /// <param name="sortOrder">表示順</param>
        /// <param name="isSearch">検索処理かどうか（ture:検索処理、false:検索処理ではない）</param>
        /// <returns></returns>
        private void SetKeyOrderby(StringBuilder sql, D0201SearchCondition.DisplaySortType? sortKey, CoreConst.SortOrder sortOrder, bool isSearch = true)
        {
            // 表示順キーと表示順によって、表示順をsqlに追加する
            string sortKeyName = isSearch ? allSortDic[sortKey] : allSortDicNm[sortKey];
            sql.Append(sortKeyName);
            if (CoreConst.SortOrder.ASC.Equals(sortOrder))
            {
                if (isSearch)
                {
                    sql.Append(" ASC, ");
                }
                else
                {
                    sql.Append("　昇順" + Environment.NewLine);
                }
            }
            else
            {
                if (isSearch)
                {
                    sql.Append(" DESC, ");
                }
                else
                {
                    sql.Append("　降順" + Environment.NewLine);
                }
            }
        }
        #endregion

        #region 選択作成イベント
        /// <summary>
        /// イベント名：選択作成
        /// </summary>
        /// <param name="selects">選択チェックボックスリスト</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult CreateSelect(List<string> selects)
        {
            Regex regex = new Regex(@"^\d+$");
            // 脆弱性対応のため、パラメータチェック追加
            if (selects.IsNullOrEmpty() || !selects.All(item => regex.IsMatch(item)))
            {
                return BadRequest();
            }
            // セッション情報取得
            var model = SessionUtil.Get<D0201Model>(SESS_D0201, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面選択した明細データのindexをセッションに保存する
            model.SearchResult.SelectCheckBoxes = selects;
            // 選択作成ボタンを押下フラグ
            model.isSelectBtnClick = true;
            SessionUtil.Set(SESS_D0201, model, HttpContext);

            // 帳票処理情報を取得する
            MReportKanri mReportKanri = null;
            logger.Info("帳票処理管理マスタから帳票処理情報を取得する。");
            // [画面：対象書類]で「加入者情報（一覧）」が選択されている場合
            if (TargetReports.KanyushaInfoList.Equals(model.SearchCondition.TargetReport))
            {
                mReportKanri = ReportKanriUtil.GetReportKanri(REPORT_CONTROL_ID_H001, 1);
            }
            else if (TargetReports.KanyushaInfoSingle.Equals(model.SearchCondition.TargetReport))
            {
                // [画面：対象書類]で「加入者情報（単票）」が選択されている場合
                mReportKanri = ReportKanriUtil.GetReportKanri(REPORT_CONTROL_ID_H002, 1);
            }

            // リアルタイム帳票の場合
            if (selects.Count <= (mReportKanri?.BatchShoriKensu ?? 0))
            {
                return Json(new { isRealTime = true, message = MessageUtil.Get("MQ00010", mReportKanri?.FileNm) });
            }
            else
            {
                // バッチ帳票の場合
                return Json(new { isRealTime = false, message = MessageUtil.Get("MQ00011", mReportKanri?.FileNm) });
            }
        }
        #endregion

        #region 全件作成イベント
        /// <summary>
        /// イベント名：全件作成
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult CreateAll()
        {
            // セッション情報取得
            var model = SessionUtil.Get<D0201Model>(SESS_D0201, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 選択作成ボタンを押下フラグ
            model.isSelectBtnClick = false;
            SessionUtil.Set(SESS_D0201, model, HttpContext);

            // 帳票処理情報を取得する
            MReportKanri mReportKanri = null;
            logger.Info("帳票処理管理マスタから帳票処理情報を取得する。");
            // [画面：対象書類]で「加入者情報（一覧）」が選択されている場合
            if (TargetReports.KanyushaInfoList.Equals(model.SearchCondition.TargetReport))
            {
                mReportKanri = ReportKanriUtil.GetReportKanri(REPORT_CONTROL_ID_H001, 1);
            }
            else if (TargetReports.KanyushaInfoSingle.Equals(model.SearchCondition.TargetReport))
            {
                // [画面：対象書類]で「加入者情報（単票）」が選択されている場合
                mReportKanri = ReportKanriUtil.GetReportKanri(REPORT_CONTROL_ID_H002, 1);
            }

            // リアルタイム帳票の場合
            if (model.SearchResult.TotalCount <= (mReportKanri?.BatchShoriKensu ?? 0))
            {
                return Json(new { isRealTime = true, message = MessageUtil.Get("MQ00010", mReportKanri?.FileNm) });
            }
            else
            {
                // バッチ帳票の場合
                return Json(new { isRealTime = false, message = MessageUtil.Get("MQ00011", mReportKanri?.FileNm) });
            }
        }
        #endregion

        #region リアルタイム帳票作成イベント
        /// <summary>
        /// イベント名：リアルタイム帳票作成
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public async Task<ActionResult> CreatRealtimeReport()
        {
            var model = SessionUtil.Get<D0201Model>(SESS_D0201, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 条件IDを取得する
            logger.Info("条件ID取得処理を実行します。");
            int joukenId = (int)DBUtil.NextSeqVal(getJigyoDb<NskAppContext>(), "seq_jouken_id");
            logger.Info("条件ID取得処理が実行完了（条件ID:" + joukenId + "）");

            // 帳票作成条件を登録する
            InsertTReportJouken(model, joukenId, model.isSelectBtnClick);

            // リアル帳票出力サービスを呼び出す
            var result = new ReportResult();
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
            var shishoCdList = new List<string>();
            if (!shishoList.IsNullOrEmpty())
            {
                shishoCdList = shishoList.Select(t => t.ShishoCd).Order().ToList();
            }
            var controld = string.Empty;
            // [画面：対象書類]で「加入者情報（一覧）」が選択されている場合
            if (TargetReports.KanyushaInfoList.Equals(model.SearchCondition.TargetReport))
            {
                controld = REPORT_CONTROL_ID_H001;
                logger.Info("リアルタイム帳票を呼び出す。（帳票制御ID:" + REPORT_CONTROL_ID_H001 + "）");
                result = await _serviceClient.CreateReport(REPORT_CONTROL_ID_H001, Syokuin.UserId, joukenId, Syokuin.TodofukenCd, Syokuin.KumiaitoCd, Syokuin.ShishoCd, shishoCdList);
            }
            else if (TargetReports.KanyushaInfoSingle.Equals(model.SearchCondition.TargetReport))
            {
                // [画面：対象書類]で「加入者情報（単票）」が選択されている場合
                controld = REPORT_CONTROL_ID_H002;
                logger.Info("リアルタイム帳票を呼び出す。（帳票制御ID:" + REPORT_CONTROL_ID_H002 + "）");
                result = await _serviceClient.CreateReport(REPORT_CONTROL_ID_H002, Syokuin.UserId, joukenId, Syokuin.TodofukenCd, Syokuin.KumiaitoCd, Syokuin.ShishoCd, shishoCdList);
            }

            // 帳票出力処理エラーの場合
            if (result == null)
            {
                throw new SystemException("リアル帳票出力処理に予期以外のエラーが発生しました。");
            }

            // 帳票出力処理エラーの場合
            if (result.Result != 0)
            {
                throw new AppException(result.ErrorMessageId, result.ErrorMessage, CoreConst.HEADER_PATTERN_ID_2);
            }

            // 選択作成の場合
            if (model.isSelectBtnClick)
            {
                // （出力メッセージ："選択作成 帳票制御ID："＋「３．２．２．」の引数で指定した帳票ID＋" 条件ID："＋「３．２．１．（１）」で取得した条件ID）
                logger.Info("選択作成 帳票制御ID：" + controld + " 条件ID：" + joukenId);
            }
            else
            {
                // 全件作成の場合
                // （出力メッセージ："全件作成 帳票制御ID："＋「２．２．２．」の引数で指定した帳票ID＋" 条件ID："＋「２．２．１．（１）」で取得した条件ID）x
                logger.Info("全件作成 帳票制御ID：" + controld + " 条件ID：" + joukenId);
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                Inline = true,
            };
            Response.Headers.Append(AppConst.RESPONSE_HEADER, cd.ToString());
            return File(result.ReportData, AppConst.RESPONSE_CONTENT_TYPE_APPLICATION_PDF, cd.FileName);
        }
        #endregion

        #region バッチ帳票作成イベント
        /// <summary>
        /// イベント名：バッチ帳票作成
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult CreatBatchReport()
        {
            var model = SessionUtil.Get<D0201Model>(SESS_D0201, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 条件IDを取得する
            logger.Info("条件ID取得処理を実行します。");
            int joukenId = (int)DBUtil.NextSeqVal(getJigyoDb<NskAppContext>(), "seq_jouken_id");
            logger.Info("条件ID取得処理が実行完了（条件ID:" + joukenId + "）");

            // 帳票作成条件を登録する
            InsertTReportJouken(model, joukenId, model.isSelectBtnClick);

            // バッチ予約登録
            var refMsg = string.Empty;
            long batchId = 0;
            // バッチ条件（表示用）作成
            var displayJouken = SetDisplayJouken(model, joukenId);

            // バッチ予約登録
            int? result = null;
            try
            {
                logger.Info("バッチ予約登録処理を実行する。");
                result = BatchUtil.InsertBatchYoyaku(AppConst.BatchBunrui.BATCH_BUNRUI_01_REPORT,
                ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN), Syokuin.TodofukenCd,
                Syokuin.KumiaitoCd, Syokuin.ShishoCd, DateUtil.GetSysDateTime(), Syokuin.UserId,
                model.isSelectBtnClick ? D0201_REPORT_YOYAKU_SHORI_NM_SELECT : D0201_REPORT_YOYAKU_SHORI_NM_ALL,
                D0201_BATCH_NM, joukenId.ToString(), displayJouken, AppConst.FLG_OFF, AppConst.BatchType.BATCH_TYPE_PATROL,
                null, AppConst.FLG_OFF, ref refMsg, ref batchId);
            }
            catch (Exception e)
            {
                // （出力メッセージ：バッチ予約登録失敗）
                // （出力メッセージ：（メッセージID：ME90008、引数{0}："（"+｢４．２．３．｣で返却されたメッセージ + "）"））
                logger.Error("バッチ予約登録失敗");
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                return Json(new { message = MessageUtil.Get("ME90005") });
            }

            // 処理結果がエラーだった場合
            if (result == 0)
            {
                // （出力メッセージ：バッチ予約登録失敗）
                // （出力メッセージ：（メッセージID：ME90008、引数{0}："（"+｢４．２．３．｣で返却されたメッセージ + "）"））
                logger.Error("バッチ予約登録失敗");
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                return Json(new { message = MessageUtil.Get("ME90005") });
            }

            // （出力メッセージ：バッチ予約登録成功）
            logger.Info("バッチ予約登録成功");
            return Json(new { message = MessageUtil.Get("MI00004", "帳票作成バッチ処理の予約") });
        }
        #endregion

        #region 帳票作成条件登録メッソド
        /// <summary>
        /// メッソド:帳票作成条件登録
        /// </summary>
        /// <param name="model">画面モデル</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="isSelectBtn">選択作成押下フラグ(true:選択作成押下、false:全件作成押下)</param>
        private void InsertTReportJouken(D0201Model model, int joukenId, bool isSelectBtnClick = false)
        {
            // ユーザID
            var userId = Syokuin.UserId;
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();
            // 連番
            int serialNumber = 0;

            MReportKanri mReportKanri = null;
            var targetReport = string.Empty;
            // [画面：対象書類]で「加入者情報（一覧）」が選択されている場合
            if (TargetReports.KanyushaInfoList.Equals(model.SearchCondition.TargetReport))
            {
                mReportKanri = ReportKanriUtil.GetReportKanri(REPORT_CONTROL_ID_H001, 1);
                targetReport = AppConst.FLG_OFF;
            }
            else if (TargetReports.KanyushaInfoSingle.Equals(model.SearchCondition.TargetReport))
            {
                // [画面：対象書類]で「加入者情報（単票）」が選択されている場合
                mReportKanri = ReportKanriUtil.GetReportKanri(REPORT_CONTROL_ID_H002, 1);
                targetReport = AppConst.FLG_ON;
            }

            using (var tx = getJigyoDb<NskAppContext>().Database.BeginTransaction())
            {
                // 予約名
                var jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_YOYAKU_NM,
                    JoukenDisplayValue = mReportKanri?.YoyakuNm,
                    JoukenValue = mReportKanri?.YoyakuNm,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_YOYAKU_NM + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 帳票制御ID
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_REPORT_CONTROL_ID,
                    JoukenDisplayValue = mReportKanri?.ReportControlId,
                    JoukenValue = mReportKanri?.ReportControlId,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_REPORT_CONTROL_ID + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 帳票パス
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_REPORT_PATH,
                    JoukenDisplayValue = GetReportPath(mReportKanri?.FileNm, systemDate),
                    JoukenValue = GetReportPath(mReportKanri?.FileNm, systemDate),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_REPORT_PATH + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 都道府県
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_TODOFUKEN,
                    JoukenDisplayValue = Syokuin.TodofukenCd,
                    JoukenValue = Syokuin.TodofukenCd,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_TODOFUKEN + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 組合等
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_KUMIAITO,
                    JoukenDisplayValue = Syokuin.KumiaitoCd,
                    JoukenValue = Syokuin.KumiaitoCd,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_KUMIAITO + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
                var shishoCdList = new List<string>();
                if (!shishoList.IsNullOrEmpty())
                {
                    shishoCdList = shishoList.Select(t => t.ShishoCd).Order().ToList();
                }
                // 利用可能支所一覧
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_SHISHO_LIST,
                    JoukenDisplayValue = string.Join(",", shishoCdList),
                    JoukenValue = string.Join(",", shishoCdList),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_SHISHO_LIST + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 支所（ログインユーザのセッション
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_SESSION_SHISHO,
                    JoukenDisplayValue = Syokuin.ShishoCd,
                    JoukenValue = Syokuin.ShishoCd,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_SESSION_SHISHO + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 対象書類
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_TARGET_REPORT,
                    JoukenDisplayValue = model.SearchCondition.TargetReport.ToDescription(),
                    JoukenValue = targetReport,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_TARGET_REPORT + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 対象年度（開始）
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_TARGET_NENDO_FROM,
                    JoukenDisplayValue = NendoUtil.GetNendoDisp1(model.SearchCondition.NendoFrom),
                    JoukenValue = model.SearchCondition.NendoFrom,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_TARGET_NENDO_FROM + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 対象年度（終了）
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_TARGET_NENDO_TO,
                    JoukenDisplayValue = NendoUtil.GetNendoDisp1(model.SearchCondition.NendoTo),
                    JoukenValue = model.SearchCondition.NendoTo,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_TARGET_NENDO_TO + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 支所   
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_SHISHO,
                    JoukenDisplayValue = ShishoUtil.GetShishoNm(model.SearchCondition.TodofukenDropDownList.TodofukenCd, model.SearchCondition.TodofukenDropDownList.KumiaitoCd, model.SearchCondition.TodofukenDropDownList.ShishoCd),
                    JoukenValue = model.SearchCondition.TodofukenDropDownList.ShishoCd,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_SHISHO + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 市町村   
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_SHICHOSON,
                    JoukenDisplayValue = ShichosonUtil.GetShichosonNm(model.SearchCondition.TodofukenDropDownList.TodofukenCd, model.SearchCondition.TodofukenDropDownList.KumiaitoCd, model.SearchCondition.TodofukenDropDownList.ShichosonCd),
                    JoukenValue = model.SearchCondition.TodofukenDropDownList.ShichosonCd,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_SHICHOSON + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 大地区   
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_DAICHIKU,
                    JoukenDisplayValue = DaichikuUtil.GetDaichikuNm(model.SearchCondition.TodofukenDropDownList.TodofukenCd, model.SearchCondition.TodofukenDropDownList.KumiaitoCd, model.SearchCondition.TodofukenDropDownList.DaichikuCd),
                    JoukenValue = model.SearchCondition.TodofukenDropDownList.DaichikuCd,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_DAICHIKU + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 小地区（開始）  
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_SHOCHIKU_START,
                    JoukenDisplayValue = ShochikuUtil.GetShochikuNm(model.SearchCondition.TodofukenDropDownList.TodofukenCd, model.SearchCondition.TodofukenDropDownList.KumiaitoCd, model.SearchCondition.TodofukenDropDownList.DaichikuCd, model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom),
                    JoukenValue = model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_SHOCHIKU_START + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 小地区（終了）  
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_SHOCHIKU_END,
                    JoukenDisplayValue = ShochikuUtil.GetShochikuNm(model.SearchCondition.TodofukenDropDownList.TodofukenCd, model.SearchCondition.TodofukenDropDownList.KumiaitoCd, model.SearchCondition.TodofukenDropDownList.DaichikuCd, model.SearchCondition.TodofukenDropDownList.ShochikuCdTo),
                    JoukenValue = model.SearchCondition.TodofukenDropDownList.ShochikuCdTo,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_SHOCHIKU_END + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 加入者管理コード（開始） 
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_KANYUSHA_CD_START,
                    JoukenDisplayValue = model.SearchCondition.KanyushaCdFrom,
                    JoukenValue = model.SearchCondition.KanyushaCdFrom,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_KANYUSHA_CD_START + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 加入者管理コード（終了）
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_KANYUSHA_CD_END,
                    JoukenDisplayValue = model.SearchCondition.KanyushaCdTo,
                    JoukenValue = model.SearchCondition.KanyushaCdTo,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_KANYUSHA_CD_END + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 氏名又は法人名
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_HOJIN_FULL_NM,
                    JoukenDisplayValue = string.IsNullOrEmpty(model.SearchCondition.HojinFullNm) ? string.Empty : "%" + model.SearchCondition.HojinFullNm + "%",
                    JoukenValue = model.SearchCondition.HojinFullNm,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_HOJIN_FULL_NM + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 表示順キー１
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_ORDER_BY_KEY1,
                    JoukenDisplayValue = model.SearchCondition.DisplaySort1.ToDescription(),
                    JoukenValue = model.SearchCondition.DisplaySort1.ToString(),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_ORDER_BY_KEY1 + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 表示順１
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_ORDER_BY1,
                    JoukenDisplayValue = model.SearchCondition.DisplaySortOrder1.ToDescription(),
                    JoukenValue = model.SearchCondition.DisplaySortOrder1.ToString(),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_ORDER_BY1 + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 表示順キー２
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_ORDER_BY_KEY2,
                    JoukenDisplayValue = model.SearchCondition.DisplaySort2.ToDescription(),
                    JoukenValue = model.SearchCondition.DisplaySort2.ToString(),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_ORDER_BY_KEY2 + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 表示順２
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_ORDER_BY2,
                    JoukenDisplayValue = model.SearchCondition.DisplaySortOrder2.ToDescription(),
                    JoukenValue = model.SearchCondition.DisplaySortOrder2.ToString(),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_ORDER_BY2 + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 表示順キー３
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_ORDER_BY_KEY3,
                    JoukenDisplayValue = (model.SearchCondition.DisplaySort3).ToDescription(),
                    JoukenValue = model.SearchCondition.DisplaySort3.ToString(),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_ORDER_BY_KEY3 + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 表示順３
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_ORDER_BY3,
                    JoukenDisplayValue = (model.SearchCondition.DisplaySortOrder3).ToDescription(),
                    JoukenValue = model.SearchCondition.DisplaySortOrder3.ToString(),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_ORDER_BY3 + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 選択作成ボタンを押下する場合
                if (isSelectBtnClick)
                {
                    var nogyoshaNendoList = new List<Tuple<int, short>>();
                    foreach (var index in model.SearchResult.SelectCheckBoxes)
                    {
                        var nogyoushaId = model.SearchResult.TableRecords[int.Parse(index)].NogyoshaId;
                        var nendo = (short)model.SearchResult.TableRecords[int.Parse(index)].Nendo;
                        nogyoshaNendoList.Add(Tuple.Create(nogyoushaId, nendo));

                        // 農業者ID
                        jouken = new TReportJouken
                        {
                            JoukenId = joukenId,
                            SerialNumber = ++serialNumber,
                            JoukenNm = JoukenNameConst.JOUKEN_NOGYOSHA_ID,
                            JoukenDisplayValue = nogyoushaId.ToString(),
                            JoukenValue = nogyoushaId.ToString(),
                            InsertUserId = userId,
                            InsertDate = systemDate,
                            UpdateUserId = userId,
                            UpdateDate = systemDate
                        };
                        getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                        logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_NOGYOSHA_ID + ")");
                        getJigyoDb<NskAppContext>().SaveChanges();

                        // 年度
                        jouken = new TReportJouken
                        {
                            JoukenId = joukenId,
                            SerialNumber = ++serialNumber,
                            JoukenNm = JoukenNameConst.JOUKEN_TARGET_NENDO,
                            JoukenDisplayValue = nendo.ToString(),
                            JoukenValue = nendo.ToString(),
                            InsertUserId = userId,
                            InsertDate = systemDate,
                            UpdateUserId = userId,
                            UpdateDate = systemDate
                        };
                        getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                        logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_TARGET_NENDO + ")");
                        getJigyoDb<NskAppContext>().SaveChanges();
                    }

                    if (!nogyoshaNendoList.IsNullOrEmpty())
                    {
                        string nogyoshaNendoStr = string.Join(", ", nogyoshaNendoList.Select(t => $"({t.Item1}, {t.Item2})"));
                        // 農業者ID、年度
                        jouken = new TReportJouken
                        {
                            JoukenId = joukenId,
                            SerialNumber = ++serialNumber,
                            JoukenNm = JoukenNameConst.JOUKEN_NOGYOSHA_ID_TARGET_NENDO,
                            JoukenDisplayValue = nogyoshaNendoStr,
                            JoukenValue = nogyoshaNendoStr,
                            InsertUserId = userId,
                            InsertDate = systemDate,
                            UpdateUserId = userId,
                            UpdateDate = systemDate
                        };
                        getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                        logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_NOGYOSHA_ID_TARGET_NENDO + ")");
                        getJigyoDb<NskAppContext>().SaveChanges();
                    }
                }

                tx.Commit();
            }
        }
        #endregion

        #region 各種補助メソッド
        /// <summary>
        /// バッチ条件（表示用）設定
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="joukenId">条件ID</param>/// 
        /// <returns>設定したバッチ条件（表示用）</returns>
        private string SetDisplayJouken(D0201Model model, int joukenId)
        {
            var sb = new StringBuilder();
            sb.Append("（対象）" + Environment.NewLine);
            sb.Append(model.SearchCondition.TargetReport.ToDescription() + Environment.NewLine);

            // 条件設定
            sb.Append("（条件）" + Environment.NewLine);

            logger.Info("条件情報を取得する。（バッチ条件（表示用）設定処理用）");
            var constJouken = new string[] { JoukenNameConst.JOUKEN_YOYAKU_NM, JoukenNameConst.JOUKEN_REPORT_CONTROL_ID, JoukenNameConst.JOUKEN_REPORT_PATH };
            var joukenList = getJigyoDb<NskAppContext>().TReportJoukens.Where(t => t.JoukenId == joukenId && !t.JoukenNm.StartsWith("表示順") && !constJouken.Contains(t.JoukenNm)).OrderBy(t => t.SerialNumber);
            foreach (var jouken in joukenList)
            {
                if (jouken.JoukenNm == JoukenNameConst.JOUKEN_HOJIN_FULL_NM)
                {
                    sb.Append(jouken.JoukenNm + "（部分一致）=" + jouken.JoukenValue + Environment.NewLine);
                }
                else
                {
                    sb.Append(jouken.JoukenNm + "=" + jouken.JoukenValue + Environment.NewLine);
                }
            }

            // （並び順）
            sb.Append("（並び順）" + Environment.NewLine);
            this.GetOrderby(sb, model, false);

            return sb.ToString();
        }
        #endregion
    }
}
