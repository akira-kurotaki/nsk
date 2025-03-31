using NskAppModelLibrary.Context;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using ReportService.Core;
using System.Text;
using NskWeb.Areas.F000.Models.D000000;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F106.Models;
using NskCommonLibrary.Core.Consts;

namespace NskWeb.Areas.F106.Controllers
{
    /// <summary>
    /// 引受計算処理共通
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [Area("F106")]
    public class D106000BaseController : CoreController
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D106000BaseController(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
        {
        }
        #endregion

        #region 本所・支所ドロップダウンリスト取得
        /// <summary>
        /// 本所のドロップダウンリストを取得
        /// </summary>
        /// <returns>List<D106000HonshoshishoList></returns>
        protected List<D106000HonshoshishoList> getHonshoList()
        {
            Syokuin syokuin = Syokuin;

            // 名称M支所から本所を取得
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT ");
            sql.Append("    shisho_cd AS 本所・支所コード ");
            sql.Append("    , shisho_cd || ' ' || shisho_nm AS 本所・支所名称 ");
            sql.Append("FROM ");
            sql.Append("    v_shisho_nm ");
            sql.Append("WHERE '1' = '1' ");
            sql.Append("AND todofuken_cd = @TodofukenCd ");
            sql.Append("AND kumiaito_cd  = @KumiaitoCd ");
            sql.Append("AND shisho_cd    = @HonshoCd ");

            // [セッション：都道府県コード]
            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));
            // [セッション：組合等]
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));
            // [本所コード]
            parameters.Add(new NpgsqlParameter("@HonshoCd", AppConst.HONSHO_CD));

            List<D106000HonshoshishoList> list = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D106000HonshoshishoList>(sql.ToString(), parameters.ToArray()).ToList();

            return list;
        }

        /// <summary>
        /// 支所のドロップダウンリストを取得
        /// </summary>
        /// <returns>List<D106000HonshoshishoList></returns>
        protected List<D106000HonshoshishoList> getShishoList()
        {
            Syokuin syokuin = Syokuin;

            // 利用可能な支所リストを取得
            List<Shisho> shishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            // 名称M支所から本所を取得
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT ");
            sql.Append("    shisho_cd AS 本所・支所コード ");
            sql.Append("    , shisho_cd || ' ' || shisho_nm AS 本所・支所名称 ");
            sql.Append("FROM ");
            sql.Append("    v_shisho_nm ");
            sql.Append("WHERE '1' = '1' ");
            sql.Append("AND todofuken_cd = @TodofukenCd ");
            sql.Append("AND kumiaito_cd  = @KumiaitoCd ");

            // 支所コードは利用可能支所リストと職員情報の状態に合わせて条件を設定する(全て空白の場合は全て取得)
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                // 利用可能な支所一覧の引受回を取得する
                sql.Append(" AND shisho_cd = ANY (@ShishoList) ");
                parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).ToList()
                });
            }
            else if (!string.IsNullOrEmpty(syokuin.ShishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append(" AND shisho_cd = @ShishoCd_1 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_1", syokuin.ShishoCd));
            }

            // [セッション：都道府県コード]
            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));
            // [セッション：組合等]
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));

            List<D106000HonshoshishoList> list = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D106000HonshoshishoList>(sql.ToString(), parameters.ToArray()).ToList();

            return list;
        }
        #endregion

        #region 共済目的名称取得メソッド
        /// <summary>
        /// 共済目的名称取得メソッド
        /// </summary>
        /// <param name="KyosaiMokutekiCd">共済目的コード</param>
        /// <returns>共済目的名称取得</returns>
        protected string GetKyosaiMokutekiNM(string KyosaiMokutekiCd)
        {

            logger.Info("共済目的名称を取得する（画面表示用）");

            var strKyosaiMokuteki = getJigyoDb<NskAppContext>().M00010共済目的名称s.Where(t => t.共済目的コード == KyosaiMokutekiCd).SingleOrDefault();


            return strKyosaiMokuteki.共済目的名称.ToString();
        }
        #endregion

        #region 本所の引受回を取得
        /// <summary>
        /// 選択した支所の引受回一覧を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="parameters">検索sqlパラメータ</param>
        /// <param name="model">ビューモデル</param>
        protected void GetHonshoHikiukeList(StringBuilder sql, List<NpgsqlParameter> parameters, NSKPortalInfoModel model)
        {
            Syokuin syokuin = Syokuin;

            sql.Append("SELECT distinct ");
            sql.Append("    MAIN.支所コード ShishoCd ");
            sql.Append("    , MAIN.支所コード || ' ' || MAIN.支所名 ShishoNm ");
            sql.Append("    , MAIN.引受回 HikiukeCnt");
            sql.Append("    , t_00010_引受回.引受計算実施日 HikiukeKeisanDate ");
            sql.Append("    , COALESCE(t_00020_引受確定.確定引受回, 0) KakuteiHikiukeCnt ");
            sql.Append("    , COALESCE(t_00040_報告回.紐づけ引受回, 0) HoukokuCnt ");
            sql.Append("FROM ");
            sql.Append("    ( SELECT ");
            sql.Append("        M1.shisho_cd 支所コード ");
            sql.Append("        , M1.shisho_nm 支所名 ");
            sql.Append("        , M2.引受回 ");
            sql.Append("    FROM ");
            sql.Append("        v_shisho_nm M1 ");
            sql.Append("    INNER JOIN ( ");
            sql.Append("        SELECT ");
            sql.Append("            支所コード ");
            sql.Append("            , 組合等コード ");
            sql.Append("            , MAX(引受回) 引受回 ");
            sql.Append("        FROM ");
            sql.Append("            t_00010_引受回 ");
            sql.Append("        WHERE '1' = '1' ");
            sql.Append("            AND 組合等コード   = @KumiaitoCd ");
            sql.Append("            AND 共済目的コード = @KyosaiMokutekiCd ");
            sql.Append("            AND 年産           = @Nensan ");
            sql.Append("            AND 支所コード     = @HonshoCd ");
            sql.Append("        GROUP BY ");
            sql.Append("        組合等コード ");
            sql.Append("        , 支所コード ");
            sql.Append("    ) M2 ");
            sql.Append("    ON  M2.組合等コード = M1.kumiaito_cd ");
            sql.Append("    AND M2.支所コード   = M1.shisho_cd ");
            sql.Append("    WHERE '1' = '1' ");
            sql.Append("    AND M1.todofuken_cd = @TodofukenCd ");
            sql.Append("    AND M1.kumiaito_cd  = @KumiaitoCd ");
            sql.Append("    AND M1.shisho_cd    = @HonshoCd ");
            sql.Append("    ) MAIN ");
            sql.Append("LEFT JOIN t_00010_引受回 ");
            sql.Append("ON  t_00010_引受回.組合等コード     = @KumiaitoCd ");
            sql.Append("AND t_00010_引受回.共済目的コード   = @KyosaiMokutekiCd ");
            sql.Append("AND t_00010_引受回.年産             = @Nensan ");
            sql.Append("AND t_00010_引受回.支所コード       = MAIN.支所コード ");
            sql.Append("AND t_00010_引受回.引受回           = MAIN.引受回 ");
            sql.Append("LEFT JOIN t_00020_引受確定 ");
            sql.Append("ON  t_00020_引受確定.組合等コード   = @KumiaitoCd ");
            sql.Append("AND t_00020_引受確定.共済目的コード = @KyosaiMokutekiCd ");
            sql.Append("AND t_00020_引受確定.年産           = @Nensan ");
            sql.Append("AND t_00020_引受確定.支所コード     = MAIN.支所コード ");
            sql.Append("AND t_00020_引受確定.確定引受回    >= MAIN.引受回 ");
            sql.Append("LEFT JOIN t_00040_報告回 ");
            sql.Append("ON  t_00040_報告回.組合等コード     = @KumiaitoCd ");
            sql.Append("AND t_00040_報告回.共済目的コード   = @KyosaiMokutekiCd ");
            sql.Append("AND t_00040_報告回.年産             = @Nensan ");
            //sql.Append("AND t_00040_報告回.支所コード       = MAIN.支所コード ");  報告回は00のレコードは作成されないので本所本所の場合は支所コードを条件に入れない
            sql.Append("AND t_00040_報告回.紐づけ引受回     = MAIN.引受回 ");
            sql.Append("AND t_00040_報告回.報告実施日 Is Not Null ");
            sql.Append("ORDER BY ");
            sql.Append("MAIN.支所コード ");

            // [セッション：都道府県コード]
            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));
            // [セッション：組合等]
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));
            // [セッション：共済目的]
            parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", model.SKyosaiMokutekiCd));
            // [セッション：年産]
            parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHikiuke)));
            // [本所コード]
            parameters.Add(new NpgsqlParameter("@HonshoCd", AppConst.HONSHO_CD));
        }
        #endregion

        #region 選択した支所の引受回を取得(支所単体)
        /// <summary>
        /// 選択した支所の引受回一覧を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="parameters">検索sqlパラメータ</param>
        /// <param name="model">ビューモデル</param>
        protected void GetShishoHikiukeList(StringBuilder sql, List<NpgsqlParameter> parameters, NSKPortalInfoModel model, string shishocd)
        {
            Syokuin syokuin = Syokuin;

            sql.Append("SELECT ");
            sql.Append("    MAIN.支所コード ShishoCd ");
            sql.Append("    , MAIN.支所コード || ' ' || MAIN.支所名 ShishoNm ");
            sql.Append("    , MAIN.引受回 HikiukeCnt");
            sql.Append("    , t_00010_引受回.引受計算実施日 HikiukeKeisanDate ");
            sql.Append("    , COALESCE(t_00020_引受確定.確定引受回, 0) KakuteiHikiukeCnt ");
            sql.Append("    , COALESCE(t_00040_報告回.紐づけ引受回, 0) HoukokuCnt ");
            sql.Append("FROM ");
            sql.Append("    ( SELECT ");
            sql.Append("        M1.shisho_cd 支所コード ");
            sql.Append("        , M1.shisho_nm 支所名 ");
            sql.Append("        , M2.引受回 ");
            sql.Append("    FROM ");
            sql.Append("        v_shisho_nm M1 ");
            sql.Append("    INNER JOIN ( ");
            sql.Append("        SELECT ");
            sql.Append("            支所コード ");
            sql.Append("            , 組合等コード ");
            sql.Append("            , MAX(引受回) 引受回 ");
            sql.Append("        FROM ");
            sql.Append("            t_00010_引受回 ");
            sql.Append("        WHERE '1' = '1' ");
            sql.Append("            AND 組合等コード   = @KumiaitoCd ");
            sql.Append("            AND 共済目的コード = @KyosaiMokutekiCd ");
            sql.Append("            AND 年産           = @Nensan ");
            sql.Append("            AND 支所コード     = @ShishoCd ");
            sql.Append("        GROUP BY ");
            sql.Append("        組合等コード ");
            sql.Append("        , 支所コード ");
            sql.Append("    ) M2 ");
            sql.Append("    ON  M2.組合等コード = M1.kumiaito_cd ");
            sql.Append("    AND M2.支所コード   = M1.shisho_cd ");
            sql.Append("    WHERE '1' = '1' ");
            sql.Append("    AND M1.todofuken_cd = @TodofukenCd ");
            sql.Append("    AND M1.kumiaito_cd  = @KumiaitoCd ");
            sql.Append("    AND M1.shisho_cd    = @ShishoCd ");
            sql.Append("    ) MAIN ");
            sql.Append("LEFT JOIN t_00010_引受回 ");
            sql.Append("ON  t_00010_引受回.組合等コード     = @KumiaitoCd ");
            sql.Append("AND t_00010_引受回.共済目的コード   = @KyosaiMokutekiCd ");
            sql.Append("AND t_00010_引受回.年産             = @Nensan ");
            sql.Append("AND t_00010_引受回.支所コード       = MAIN.支所コード ");
            sql.Append("AND t_00010_引受回.引受回           = MAIN.引受回 ");
            sql.Append("LEFT JOIN t_00020_引受確定 ");
            sql.Append("ON  t_00020_引受確定.組合等コード   = @KumiaitoCd ");
            sql.Append("AND t_00020_引受確定.共済目的コード = @KyosaiMokutekiCd ");
            sql.Append("AND t_00020_引受確定.年産           = @Nensan ");
            sql.Append("AND t_00020_引受確定.支所コード     = MAIN.支所コード ");
            sql.Append("AND t_00020_引受確定.確定引受回    >= MAIN.引受回 ");
            sql.Append("LEFT JOIN t_00040_報告回 ");
            sql.Append("ON  t_00040_報告回.組合等コード     = @KumiaitoCd ");
            sql.Append("AND t_00040_報告回.共済目的コード   = @KyosaiMokutekiCd ");
            sql.Append("AND t_00040_報告回.年産             = @Nensan ");
            sql.Append("AND t_00040_報告回.支所コード       = MAIN.支所コード ");
            sql.Append("AND t_00040_報告回.紐づけ引受回     = MAIN.引受回 ");
            sql.Append("AND t_00040_報告回.報告実施日 Is Not Null ");
            sql.Append("ORDER BY ");
            sql.Append("MAIN.支所コード ");

            // [セッション：都道府県コード]
            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));
            // [セッション：組合等]
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));
            // [セッション：共済目的]
            parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", model.SKyosaiMokutekiCd));
            // [セッション：年産]
            parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHikiuke)));
            // [画面：支所ドロップダウンリスト]
            parameters.Add(new NpgsqlParameter("@ShishoCd", shishocd));
        }
        #endregion

        #region 本所配下の引受回一覧を取得
        /// <summary>
        /// 本所配下の支所一覧を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="parameters">検索sqlパラメータ</param>
        /// <param name="syokuin">職員情報</param>
        protected void GetHonshoHaikaHikiukeList(StringBuilder sql, List<NpgsqlParameter> parameters, NSKPortalInfoModel model, string shishocd)
        {

            Syokuin syokuin = Syokuin;
            List<Shisho> shishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            sql.Append("SELECT ");
            sql.Append("    MAIN.支所コード ShishoCd ");
            sql.Append("    , MAIN.支所コード || ' ' || MAIN.支所名 ShishoNm ");
            sql.Append("    , MAIN.引受回 HikiukeCnt");
            sql.Append("    , t_00010_引受回.引受計算実施日 HikiukeKeisanDate ");
            sql.Append("    , COALESCE(t_00020_引受確定.確定引受回, 0) KakuteiHikiukeCnt ");
            sql.Append("    , COALESCE(t_00040_報告回.紐づけ引受回, 0) HoukokuCnt ");
            sql.Append("FROM ");
            sql.Append("    ( SELECT ");
            sql.Append("        M1.shisho_cd 支所コード ");
            sql.Append("        , M1.shisho_nm 支所名 ");
            sql.Append("        , M2.引受回 ");
            sql.Append("    FROM ");
            sql.Append("        v_shisho_nm M1 ");
            sql.Append("    INNER JOIN ( ");
            sql.Append("        SELECT ");
            sql.Append("            支所コード ");
            sql.Append("            , 組合等コード ");
            sql.Append("            , MAX(引受回) 引受回 ");
            sql.Append("        FROM ");
            sql.Append("            t_00010_引受回 ");
            sql.Append("        WHERE '1' = '1' ");
            sql.Append("            AND 組合等コード   = @KumiaitoCd ");
            sql.Append("            AND 共済目的コード = @KyosaiMokutekiCd ");
            sql.Append("            AND 年産           = @Nensan ");
            sql.Append("            AND 支所コード    <> '00' ");
            sql.Append("        GROUP BY ");
            sql.Append("        組合等コード ");
            sql.Append("        , 支所コード ");
            sql.Append("    ) M2 ");
            sql.Append("    ON  M2.組合等コード = M1.kumiaito_cd ");
            sql.Append("    AND M2.支所コード   = M1.shisho_cd ");
            sql.Append("    WHERE '1' = '1' ");
            sql.Append("    AND M1.todofuken_cd = @TodofukenCd ");
            sql.Append("    AND M1.kumiaito_cd  = @KumiaitoCd ");

            // 支所コードはドロップダウンリストの状態に合わせて条件を設定する(全て空白の場合は全て取得)
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                // 利用可能な支所一覧の引受回を取得する
                sql.Append(" AND M1.shisho_cd = ANY (@ShishoList) ");
                parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).ToList()
                });
            }
            else if (!string.IsNullOrEmpty(syokuin.ShishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append(" AND M1.shisho_cd = @ShishoCd_1 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_1", shishocd));
            }

            sql.Append("    ) MAIN ");
            sql.Append("LEFT JOIN t_00010_引受回 ");
            sql.Append("ON  t_00010_引受回.組合等コード     = @KumiaitoCd ");
            sql.Append("AND t_00010_引受回.共済目的コード   = @KyosaiMokutekiCd ");
            sql.Append("AND t_00010_引受回.年産             = @Nensan ");
            sql.Append("AND t_00010_引受回.支所コード       = MAIN.支所コード ");
            sql.Append("AND t_00010_引受回.引受回           = MAIN.引受回 ");
            sql.Append("LEFT JOIN t_00020_引受確定 ");
            sql.Append("ON  t_00020_引受確定.組合等コード   = @KumiaitoCd ");
            sql.Append("AND t_00020_引受確定.共済目的コード = @KyosaiMokutekiCd ");
            sql.Append("AND t_00020_引受確定.年産           = @Nensan ");
            sql.Append("AND t_00020_引受確定.支所コード     = MAIN.支所コード ");
            sql.Append("AND t_00020_引受確定.確定引受回    >= MAIN.引受回 ");
            sql.Append("LEFT JOIN t_00040_報告回 ");
            sql.Append("ON  t_00040_報告回.組合等コード     = @KumiaitoCd ");
            sql.Append("AND t_00040_報告回.共済目的コード   = @KyosaiMokutekiCd ");
            sql.Append("AND t_00040_報告回.年産             = @Nensan ");
            sql.Append("AND t_00040_報告回.支所コード       = MAIN.支所コード ");
            sql.Append("AND t_00040_報告回.紐づけ引受回     = MAIN.引受回 ");
            sql.Append("AND t_00040_報告回.報告実施日 Is Not Null ");
            sql.Append("ORDER BY ");
            sql.Append("MAIN.支所コード ");

            // [セッション：都道府県コード]
            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));
            // [セッション：組合等]
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));
            // [セッション：共済目的]
            parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", model.SKyosaiMokutekiCd));
            // [セッション：年産]
            parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHikiuke)));
        }
        #endregion

        #region 引受回表示件数取得メソッド
        /// <summary>
        /// 引受回表示件数取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>引受回表示件数</returns>
        public int GetHikiukeDispCount(StringBuilder Sql, List<NpgsqlParameter> parameters)
        {
            // sql用定数定義
            var cntSql = new StringBuilder();

            // 件数取得
            cntSql.Append("SELECT COUNT(*) AS \"Value\" FROM ( ");
            // 件数取得用SQLを追加する
            cntSql.Append(Sql);

            cntSql.Append(") ");

            // sql実行 
            logger.Info("検索結果件数取得処理を実行します。");
            logger.Info(cntSql.ToString());
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<int>(cntSql.ToString(), parameters.ToArray()).Single();
        }
        #endregion

        #region バッチ条件ID取得
        /// <summary>
        /// イベント名：バッチ条件IDの取得
        /// </summary>
        /// /// <returns>バッチ条件ID</returns>
        protected string GetBatchJoken()
        {
            var BatchJokenId = System.Guid.NewGuid().ToString("D");

            return BatchJokenId;
        }
        #endregion

        #region バッチ条件テーブル登録イベント
        /// <summary>
        /// イベント名：バッチ条件テーブル登録
        /// </summary>
        /// <param name="t01050BatchJoken">バッチ条件テーブルモデル</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected void CreatBatchJoken(T01050バッチ条件 t01050BatchJoken, NskAppContext jigyodb)
        {
            // insertの条件を設定
            jigyodb.T01050バッチ条件s.Add(t01050BatchJoken);

            // insertの実行
            jigyodb.SaveChanges();
        }
        #endregion

        #region バッチ条件登録(引受年産)
        /// <summary>
        /// イベント名：バッチ条件登録(引受年産)
        /// </summary>
        /// <param name="Nensan">引受年産</param>
        /// <param name="syokuin">職員情報</param>
        /// <param name="systemDate">システム日時</param>
        /// <param name="i">連番</param>
        /// /// <returns>T01050バッチ条件引受年産</returns>
        protected T01050バッチ条件 CreatBatchJokenNensan(string Nensan, Syokuin syokuin, string BatchJokenId, DateTime systemDate, int i)
        {
            T01050バッチ条件 t01050BatchJoken = new()
            {
                // バッチ条件id
                バッチ条件id = BatchJokenId,
                // 連番
                連番 = i,
                // 条件名称
                条件名称 = JoukenNameConst.JOUKEN_NENSAN,
                // 表示用条件値
                表示用条件値 = JoukenNameConst.JOUKEN_NENSAN,
                // 条件値
                条件値 = Nensan,
                // 登録日時
                登録日時 = systemDate,
                // 登録ユーザid
                登録ユーザid = syokuin.UserId,
                // 更新日時
                更新日時 = systemDate,
                // 更新ユーザid
                更新ユーザid = syokuin.UserId,
            };

            return t01050BatchJoken;
        }
        #endregion

        #region バッチ条件登録(共済目的)
        /// <summary>
        /// イベント名：バッチ条件登録(共済目的)
        /// </summary>
        /// <param name="KyosaiMokutekiCd">共済目的コード</param>
        /// <param name="syokuin">職員情報</param>
        /// <param name="systemDate">システム日時</param>
        /// <param name="i">連番</param>
        /// /// <returns>T01050バッチ条件共済目的コード</returns>
        protected T01050バッチ条件 CreatBatchJokenKyosaiMokuteki(string KyosaiMokutekiCd, Syokuin syokuin, string BatchJokenId, DateTime systemDate, int i)
        {
            T01050バッチ条件 t01050BatchJoken = new()
            {
                // バッチ条件id
                バッチ条件id = BatchJokenId,
                // 連番
                連番 = i,
                // 条件名称
                条件名称 = JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,
                // 表示用条件値
                表示用条件値 = JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,
                // 条件値
                条件値 = KyosaiMokutekiCd,
                // 登録日時
                登録日時 = systemDate,
                // 登録ユーザid
                登録ユーザid = syokuin.UserId,
                // 更新日時
                更新日時 = systemDate,
                // 更新ユーザid
                更新ユーザid = syokuin.UserId,
            };

            return t01050BatchJoken;
        }
        #endregion

        #region バッチ条件登録(本所支所コード)
        /// <summary>
        /// イベント名：バッチ条件登録(支所コード)
        /// </summary>
        /// <param name="ShishoCd">画面の支所コード</param>
        /// <param name="syokuin">職員情報</param>
        /// <param name="systemDate">システム日時</param>
        /// <param name="i">連番</param>
        /// /// <returns>T01050バッチ条件本所支所コード</returns>
        protected T01050バッチ条件 CreatBatchJokenShishoCd(string ShishoCd, Syokuin syokuin, string BatchJokenId, DateTime systemDate, int i)
        {
            T01050バッチ条件 t01050BatchJoken = new()
            {
                // バッチ条件id
                バッチ条件id = BatchJokenId,
                // 連番
                連番 = i,
                // 条件名称
                条件名称 = JoukenNameConst.JOUKEN_SHISHO,
                // 表示用条件値
                表示用条件値 = JoukenNameConst.JOUKEN_SHISHO,
                // 条件値
                条件値 = ShishoCd,
                // 登録日時
                登録日時 = systemDate,
                // 登録ユーザid
                登録ユーザid = syokuin.UserId,
                // 更新日時
                更新日時 = systemDate,
                // 更新ユーザid
                更新ユーザid = syokuin.UserId,
            };

            return t01050BatchJoken;
        }
        #endregion

    }
}
