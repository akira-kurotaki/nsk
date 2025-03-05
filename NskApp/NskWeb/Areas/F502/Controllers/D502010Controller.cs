using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F000.Models.D000000;
using NskWeb.Areas.F502.Models.D502010;
using NskWeb.Common.Consts;
using ReportService.Core;
using System.Text;

namespace NskWeb.Areas.F502.Controllers
{
    /// <summary>
    /// NSK_501010D_農作物共済加入状況
    /// </summary>
    /// <remarks>
    /// 作成日：2025/02/17
    /// 作成者：アンヘジン
    /// </remarks>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [Area("F502")]
    public class D502010Controller : CoreController
    {

        #region メンバー定数
        /// <summary>
        /// 画面ID(D502010)
        /// </summary>
        private static readonly string SCREEN_ID_D502010 = "D502010";

        /// <summary>
        /// セッションキー(D502010)
        /// </summary>
        private static readonly string SESS_D502010 = SCREEN_ID_D502010 + "_" + "SCREEN";

        /// <summary>
        /// セッションキー(PAGE_TOKEN）
        /// </summary>
        public static readonly string SESS_D502010_PAGE_TOKEN = SCREEN_ID_D502010 + "_" + "PAGE_TOKEN";

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D502010Controller(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
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
            // model初期化
            D502010Model model = new D502010Model();

            // セッションから「農業者ID」を取得する。
            model.NogyoshaId = SessionUtil.Get<string>(AppConst.SESS_NOGYOSHA_ID, HttpContext);

            // ポータル情報を取得
            NSKPortalInfoModel m = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if ((m != null) && (!String.IsNullOrEmpty(m.SKyosaiMokutekiCd)) && (!String.IsNullOrEmpty(m.SNensanHikiuke)))
            {
                model.D502010Info.SKyosaiMokutekiCd = m.SKyosaiMokutekiCd;
                model.D502010Info.SNensanHikiuke = m.SNensanHikiuke;
            }

            if (String.IsNullOrEmpty(model.NogyoshaId) || String.IsNullOrEmpty(m.SKyosaiMokutekiCd) || String.IsNullOrEmpty(m.SNensanHikiuke))
            {
                // セッション情報が取得できない場合業務エラーへ遷移
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "セッション情報の取得"));
            }

            // 農業者情報を取得する
            using (NskAppContext jigyoDb = getJigyoDb<NskAppContext>())
            {
                model.nogyosha = jigyoDb.VNogyoshas.SingleOrDefault(x => x.KumiaiintoCd == model.NogyoshaId);

                // 農業者情報取得チェック
                if (model.nogyosha == null)
                {
                    // マスタ情報が取得できない場合、業務エラーへ遷移
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "農業者情報の取得"));
                }

                // 農業者関連の情報を一度に取得
                SetAgriculturalInfo(model);

                // 加入状況を取得する
                GetSetteiJokyoList(jigyoDb, ref model);
            }

            logger.Debug("[加入状況リスト] {}件", model.tableRecords.Count());

            // 農作物共済加入状況画面を表示する
            return View(SCREEN_ID_D502010, model);
        }
        #endregion

        #region 農業者情報取得
        /// <summary>
        /// 農業者情報取得
        /// </summary>
        private void SetAgriculturalInfo(D502010Model model)
        {
            model.ShishoNm = ShishoUtil.GetShishoNm(model.nogyosha.TodofukenCd, model.nogyosha.KumiaitoCd, model.nogyosha.ShishoCd);
            model.DaichikuNm = DaichikuUtil.GetDaichikuNm(model.nogyosha.TodofukenCd, model.nogyosha.KumiaitoCd, model.nogyosha.DaichikuCd);
            model.ShochikuNm = ShochikuUtil.GetShochikuNm(model.nogyosha.TodofukenCd, model.nogyosha.KumiaitoCd, model.nogyosha.DaichikuCd, model.nogyosha.ShochikuCd);
            model.ShichosonNm = ShichosonUtil.GetShichosonNm(model.nogyosha.TodofukenCd, model.nogyosha.KumiaitoCd, model.nogyosha.ShichosonCd);

            // すべてのマスタ情報が取得できなかった場合、業務エラーへ遷移
            if (string.IsNullOrEmpty(model.ShishoNm) || string.IsNullOrEmpty(model.DaichikuNm)
                || string.IsNullOrEmpty(model.ShochikuNm) || string.IsNullOrEmpty(model.ShichosonNm))
            {
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "農業者情報の取得"));
            }
        }
        #endregion

        #region 設定状況リスト情報の取得
        /// <summary>
        /// 設定状況リスト情報の取得
        /// </summary>
        /// <returns>設定状況リスト</returns>
        private void GetSetteiJokyoList(NskAppContext dbContext, ref D502010Model model)
        {
            var sql = new StringBuilder();
            sql.Append("WITH RankedData AS ( ");
            sql.Append("    SELECT ");
            sql.Append("          t_12040.共済目的コード");
            sql.Append("        , m_00010.共済目的名称 AS 共済目的");
            sql.Append("        , t_12040.年産");
            sql.Append("        , t_12040.引受回");
            sql.Append("        , t_12040.耕地筆数 AS 入力済み耕地筆数");
            // テーブルからレコードが取得できた場合は「実施済」、取得できない場合は「未実施」の文字列を設定
            sql.Append("        , CASE WHEN t_11010.組合員等コード IS NOT NULL THEN '実施済' ELSE '未実施' END AS 組合員等毎設定");
            // テーブルからレコードが取得できた場合は「実施済」、取得できない場合は「未実施」の文字列を設定
            sql.Append("        , CASE WHEN t_11030.組合員等コード IS NOT NULL THEN '実施済' ELSE '未実施' END AS 類区分毎設定");
            // テーブルからレコードが取得できた場合は「実施済」、取得できない場合は「未実施」の文字列を設定
            sql.Append("        , CASE WHEN t_11040.組合員等コード IS NOT NULL THEN '実施済' ELSE '未実施' END AS 料率設定");
            sql.Append("        , t_00010.引受計算実施日");
            sql.Append("        , ROW_NUMBER() OVER ( ");
            sql.Append("            PARTITION BY m_00010.共済目的名称");
            sql.Append("            ORDER BY t_12040.引受回 DESC");
            sql.Append("        ) AS rn ");
            sql.Append("    FROM t_12040_組合員等別引受情報 t_12040 ");
            sql.Append("    LEFT JOIN t_00010_引受回 t_00010 ");
            sql.Append("        ON t_00010.組合等コード = t_12040.組合等コード ");
            sql.Append("        AND t_00010.共済目的コード = t_12040.共済目的コード ");
            sql.Append("        AND t_00010.年産 = t_12040.年産 ");
            sql.Append("        AND t_00010.支所コード = t_12040.支所コード ");
            sql.Append("        AND t_00010.引受回 = t_12040.引受回 ");
            sql.Append("    LEFT JOIN t_11010_個人設定 t_11010 ");
            sql.Append("        ON t_11010.組合等コード = t_12040.組合等コード ");
            sql.Append("        AND t_11010.年産 = t_12040.年産 ");
            sql.Append("        AND t_11010.共済目的コード = t_12040.共済目的コード ");
            sql.Append("        AND t_11010.組合員等コード = t_12040.組合員等コード ");
            sql.Append("    LEFT JOIN t_11030_個人設定類 t_11030 ");
            sql.Append("        ON t_11030.組合等コード = t_12040.組合等コード ");
            sql.Append("        AND t_11030.年産 = t_12040.年産 ");
            sql.Append("        AND t_11030.共済目的コード = t_12040.共済目的コード ");
            sql.Append("        AND t_11030.組合員等コード = t_12040.組合員等コード ");
            sql.Append("        AND t_11030.類区分 = t_12040.類区分 ");
            sql.Append("    LEFT JOIN t_11040_個人料率 t_11040 ");
            sql.Append("        ON t_11040.共済目的コード = t_12040.共済目的コード ");
            sql.Append("        AND t_11040.年産 = t_12040.年産 ");
            sql.Append("        AND t_11040.共済目的コード = t_12040.共済目的コード ");
            sql.Append("        AND t_11040.組合員等コード = t_12040.組合員等コード ");
            sql.Append("        AND t_11040.類区分 = t_12040.類区分 ");
            sql.Append("        AND t_11040.統計単位地域コード = t_12040.統計単位地域コード ");
            sql.Append("    LEFT JOIN m_00010_共済目的名称 m_00010 ");
            sql.Append("        ON m_00010.共済目的コード = t_12040.共済目的コード ");
            sql.Append("        WHERE t_12040.組合等コード  = @組合等コード ");
            sql.Append("        AND t_12040.年産           = @年産 ");
            sql.Append("        AND t_12040.組合員等コード  = @組合員等コード ");
            sql.Append("        AND t_12040.引受対象フラグ  = '1' ");
            sql.Append("        AND t_12040.未加入フラグ    = '0' ");
            sql.Append(") ");
            sql.Append("SELECT ");
            sql.Append("    共済目的コード");
            sql.Append("  , 共済目的");
            sql.Append("  , 年産");
            sql.Append("  , 引受回");
            sql.Append("  , 入力済み耕地筆数");
            sql.Append("  , 組合員等毎設定");
            sql.Append("  , 類区分毎設定");
            sql.Append("  , 料率設定");
            sql.Append("  , 引受計算実施日 ");
            sql.Append("FROM RankedData ");
            // rn= 1（引受回の最大値）を指定
            sql.Append("WHERE rn = 1 ");
            sql.Append("ORDER BY 共済目的コード;");

            var parameters = new List<NpgsqlParameter>
            {
                new("@組合等コード", model.nogyosha.KumiaitoCd),
                new("@年産", int.Parse(model.D502010Info.SNensanHikiuke)),
                new("@組合員等コード", model.nogyosha.KumiaiintoCd),
            };

            // SQLのクエリ結果をListに格納する
            model.tableRecords = dbContext.Database.SqlQueryRaw<D502010TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

    }
}