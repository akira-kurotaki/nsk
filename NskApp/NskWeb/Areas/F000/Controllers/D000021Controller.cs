using NskAppModelLibrary.Context;
using NskWeb.Areas.F000.Models.D000021;
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
using static NskWeb.Areas.F000.Models.D000021.D000021SearchCondition;
using NskWeb.Areas.F000.Models.D000000;
using System.Collections.Generic;
using NskAppModelLibrary.Models;
using StackExchange.Redis;
using System.Xml;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NskWeb.Areas.F000.Controllers
{
    /// <summary>
    /// 品種コード検索
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [Area("F000")]
    public class D000021Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D000021)
        /// </summary>
        private static readonly string SCREEN_ID_D000021 = "D000021";

        /// <summary>
        /// セッションキー(D000021)
        /// </summary>
        private static readonly string SESS_D000021 = SCREEN_ID_D000021 + "_" + "SCREEN";

        /// <summary>
        /// 表部品初期化(検索結果)
        /// </summary>
        private static readonly string D000021_INITIALIZE_VALUE = "";

        /// <summary>
        /// 表示件数0件(検索結果)
        /// </summary>
        private static readonly int D000021_SEARCH_NO_COUNT = 0;

        /// <summary>
        /// 最大表示件数(検索結果)
        /// </summary>
        private static readonly int D000021_SEARCH_LIMITED_COUNT = 50;

        /// <summary>
        /// 表部品1
        /// </summary>
        private static readonly string D000021_TABLE_PART1 = "1～";

        /// <summary>
        /// 表部品2
        /// </summary>
        private static readonly string D000021_TABLE_PART2 = "件/";

        /// <summary>
        /// 表部品3
        /// </summary>
        private static readonly string D000021_TABLE_PART3 = "件中";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D000021Controller(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
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
            //NSKPortalInfoModel model_portal = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            //if (model_portal == null)
            //{
            //    throw new AppException("ME00068", MessageUtil.Get("ME00068"));
            //}
            //$$$$$$$$$$$$$$

            //モデルを取得
            D000021Model model = new D000021Model();

            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D000021, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }

            // 画面項目の初期化
            model = new D000021Model();

            // 画面初期化
            InitializeModel(model, D000021_INITIALIZE_VALUE);

            // モデル状態ディクショナリからすべての項目を削除します。       
            ModelState.Clear();

            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D000021, HttpContext);

            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D000021, model, HttpContext);

            // パンくずリストを変更する
            SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>() { "D000021" }, HttpContext);

            // 共済目的、年産をセッションから取得
            NSKPortalInfoModel m = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            // セッション情報変数定義
            String KyosaiMokutekiCd;
            String NensanHikiuke;
            if (m != null)
            {
                //セッション情報を設定
                KyosaiMokutekiCd = m.SKyosaiMokutekiCd;
                NensanHikiuke = m.SNensanHikiuke;
            }
            else
            {
                // 空白を設定
                KyosaiMokutekiCd = "";
                NensanHikiuke = "";
            }

            // 組合等コードを職員情報から取得
            String KumiaitoCd = Syokuin.KumiaitoCd;
            // 1,エラー処理
            // 共済目的、年産、組合等コードいずれも取得出来ない場合
            if (string.IsNullOrEmpty(KyosaiMokutekiCd) || string.IsNullOrEmpty(NensanHikiuke) || string.IsNullOrEmpty(KumiaitoCd))
            {
                var massageArea1 = "";
                if (string.IsNullOrEmpty(KyosaiMokutekiCd))
                {
                    // 共済目的が取得出来ない場合
                    massageArea1 += MessageUtil.Get("ME01644", "共済目的");
                }
                if (string.IsNullOrEmpty(NensanHikiuke))
                {
                    // 年産が取得出来ない場合
                    massageArea1 += MessageUtil.Get("ME01644", "年産");
                }
                if (string.IsNullOrEmpty(KumiaitoCd))
                {
                    // 組合等コードが取得出来ない場合
                    massageArea1 += MessageUtil.Get("ME01644", "組合等コード");
                }
                // メッセージエリア１に表示する（メッセージID：ME01644) 
                ModelState.AddModelError("MessageArea1", massageArea1);
            }
            // 2,正常処理
            // 共済目的または年産が取得出来た場合
            if (!string.IsNullOrEmpty(KyosaiMokutekiCd) || !string.IsNullOrEmpty(NensanHikiuke))
            {
                // 画面項目に表示する
                model.SearchCondition.Nensan = NensanHikiuke;
                model.SearchCondition.KyosaiMokutekiCd = KyosaiMokutekiCd;
                // 共済目的が空白以外の時
                if (!string.IsNullOrEmpty(KyosaiMokutekiCd))
                {
                    // 共済目的名称を取得する
                    model.SearchCondition.KyosaiMokutekiNm = GetKyosaiMokutekiNM(model, KyosaiMokutekiCd);
                }
            }

            // 品種コード検索(子画面)画面を表示する
            return View(SCREEN_ID_D000021, model);
        }
        #endregion

        #region 画面初期化
        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="model"></param>
        private void InitializeModel(D000021Model model, string v)
        {
            // 検索結果が存在する場合のみ一覧表を表示する為初期化する
            // 検索結果件数
            model.SearchResult.TotalCount = v;
            // 表部品1
            model.SearchResult.TablePart1 = v;
            // 表部品2
            model.SearchResult.TablePart2 = v;
            // 表部品3
            model.SearchResult.TablePart3 = v;
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
            SessionUtil.Remove(SESS_D000021, HttpContext);

            // 品種コード検索(子画面)画面を表示する
            return RedirectToAction("Init");
        }
        #endregion

        #region 検索イベント
        /// <summary>
        /// イベント名：検索
        /// </summary>
        /// <param name="model">品種コード検索(子画面)モデル</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind("SearchCondition")] D000021Model model)
        {
            // 属性チェックまたは独自チェックエラーの場合
            if (!ModelState.IsValid || CheckSearchCondition(model))
            {
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D000021, model, HttpContext);
                return View(SCREEN_ID_D000021, model);
            }

            // 検索して、画面に返す
            return View(SCREEN_ID_D000021, GetPageDataList(model));
        }
        #endregion

        #region 閉じるイベント
        /// <summary>
        /// イベント名：閉じる
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Close()
        {
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D000021, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion

        #region 共済目的名称取得メソッド
        /// <summary>
        /// 共済目的名称取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="KyosaiMokutekiCd">共済目的コード</param>
        /// <returns>共済目的名称取得</returns>
        private string GetKyosaiMokutekiNM(D000021Model model, string KyosaiMokutekiCd)
        {
            logger.Info("共済目的名称を取得する（画面表示用）");
            var strKyosaiMokuteki = getJigyoDb<NskAppContext>().M00010共済目的名称s.Where(t => t.共済目的コード == KyosaiMokutekiCd).SingleOrDefault();
            return strKyosaiMokuteki.共済目的名称.ToString();
        }
        #endregion

        #region 検索条件チェックメソッド
        /// <summary>
        /// 検索条件チェックメソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        private bool CheckSearchCondition(D000021Model model)
        {
            // チェックフラグ
            var checkFlg = false;
            // [画面：品種コード(開始)]>[画面：品種コード(終了)]の場合、エラーと判定する
            if (!string.IsNullOrEmpty(model.SearchCondition.HinshuCdFrom) && !string.IsNullOrEmpty(model.SearchCondition.HinshuCdTo) &&
                int.Parse(model.SearchCondition.HinshuCdFrom) > int.Parse(model.SearchCondition.HinshuCdTo))
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME10020", "品種コード"));
                ModelState.AddModelError("SearchCondition.HinshuCdFrom", " ");
                ModelState.AddModelError("SearchCondition.HinshuCdTo", " ");
                checkFlg = true;
            }

            return checkFlg;
        }
        #endregion

        #region 品種データ取得メソッド(一覧取得)
        /// <summary>
        /// メソッド：品種データ取得
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>品種コード検索結果モデル</returns>
        private D000021Model GetPageDataList(D000021Model model)
        {
            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();
            // 品種コード検索結果をクリアする
            model.SearchResult = new D000021SearchResult();
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 品種データ取得用SQLをセットする
            var tableRecords = GetCondition(sql, model, parameters);
            // 取得用SQL実行結果件数を取得する
            var TotalCount = tableRecords.ToList().Count;
            // 0件の場合
            if (TotalCount == D000021_SEARCH_NO_COUNT)
            {
                // メッセージエリア１に表示する（メッセージID：MI00011) 
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("MI00011"));
            }
            // 50件以上の場合
            else if(TotalCount >= D000021_SEARCH_LIMITED_COUNT)
            {
                // メッセージエリア１に表示する（メッセージID：ME10097) 
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME10097"));
            }
            else
            {
                // 品種データをセットする
                model.SearchResult.TableRecords = tableRecords;
                // 件数表示項目をセットする
                model.SearchResult.TotalCount = TotalCount.ToString();
                model.SearchResult.TablePart1 = D000021_TABLE_PART1;
                model.SearchResult.TablePart2 = D000021_TABLE_PART2;
                model.SearchResult.TablePart3 = D000021_TABLE_PART3;
            }

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D000021, model, HttpContext);

            return model;
        }
        #endregion

        #region 品種データ取得
        /// <summary>
        /// 検索項目を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private List<D000021TableRecord> GetCondition(StringBuilder sql, D000021Model model, List<NpgsqlParameter> parameters)
        {
            Syokuin syokuin = Syokuin;

            sql.Append("SELECT ");
            sql.Append("    品種コード AS HinshuCd ");
            sql.Append("    , 品種名等 AS HinshuNm ");
            sql.Append("FROM ");
            sql.Append("    m_00110_品種係数 ");
            sql.Append("WHERE ");
            sql.Append("    組合等コード = @KumiaitoCd ");
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));
            sql.Append("    AND 年産 = @Nensan ");
            parameters.Add(new NpgsqlParameter("@Nensan", short.Parse(model.SearchCondition.Nensan)));
            sql.Append("    AND 共済目的コード = @KyosaiMokutekiCd ");
            parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", model.SearchCondition.KyosaiMokutekiCd));
            // 検索条件SQLのセット
            if (!string.IsNullOrEmpty(model.SearchCondition.HinshuNm))
            {
                // [画面：品種名称]が入力されている場合
                GetSearchHinshuNm(sql, model, parameters);
            }
            else if (!string.IsNullOrEmpty(model.SearchCondition.HinshuCdFrom) || !string.IsNullOrEmpty(model.SearchCondition.HinshuCdTo))
            {
                // [画面：品種コード(開始)]または[画面：品種コード(終了)]が入力されている場合
                GetSearchHinshuCd(sql, model, parameters);
            }
            sql.Append("ORDER BY ");
            sql.Append("    品種コード ASC ");

            // sql実行 
            logger.Info("検索結果件数取得処理を実行します。");
            logger.Info(sql.ToString());
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D000021TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region 品種データ一覧取得検索条件_品種名称
        /// <summary>
        /// [画面：品種名称]が入力されている場合の取得条件
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSearchHinshuNm(StringBuilder sql, D000021Model model, List<NpgsqlParameter> parameters)
        {
            Syokuin syokuin = Syokuin;
            // 半角全角スペースにて分割し配列に格納
            string[] HinshuNms = model.SearchCondition.HinshuNm.Split(new char[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries);
            // 文字列前後に"%"を付ける
            string[] HinshuNmsWithPercent = HinshuNms.Select(word => "%" + word + "%").ToArray();
            foreach (var (item, index) in HinshuNmsWithPercent.Select((item, index) => (item, index)))
            {
                // 配列を検索条件として部分一致検索を行う
                sql.Append($"    AND 品種名等 ILIKE @HinshuNms{index} ");
                parameters.Add(new NpgsqlParameter($"@HinshuNms{index}", item));
            }
        }
        #endregion

        #region 品種データ一覧取得検索条件_品種コード
        /// <summary>
        /// [画面：品種コード]が入力されている場合の取得条件
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSearchHinshuCd(StringBuilder sql, D000021Model model, List<NpgsqlParameter> parameters)
        {
            // [画面：品種コード(開始)]が入力されている場合
            if (!string.IsNullOrEmpty(model.SearchCondition.HinshuCdFrom))
            {
                sql.Append("    AND CAST(品種コード AS INTEGER) >= @HinshuCdFrom ");
                parameters.Add(new NpgsqlParameter("@HinshuCdFrom", int.Parse(model.SearchCondition.HinshuCdFrom)));
            }
            // [画面：品種コード(終了)]が入力されている場合
            if (!string.IsNullOrEmpty(model.SearchCondition.HinshuCdTo))
            {
                sql.Append("    AND CAST(品種コード AS INTEGER) <= @HinshuCdTo ");
                parameters.Add(new NpgsqlParameter("@HinshuCdTo", int.Parse(model.SearchCondition.HinshuCdTo)));
            }
        }
        #endregion
    }
}
