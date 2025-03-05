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
using NskWeb.Areas.F501.Models.D501010;
using NskWeb.Common.Consts;
using ReportService.Core;
using System.Text;

namespace NskWeb.Areas.F501.Controllers
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
    [Area("F501")]
    public class D501010Controller : CoreController
    {

        #region メンバー定数
        /// <summary>
        /// 画面ID(D501010)
        /// </summary>
        private static readonly string SCREEN_ID_D501010 = "D501010";

        /// <summary>
        /// セッションキー(D501010)
        /// </summary>
        private static readonly string SESS_D501010 = SCREEN_ID_D501010 + "_" + "SCREEN";

        /// <summary>
        /// セッションキー(PAGE_TOKEN）
        /// </summary>
        public static readonly string SESS_D501010_PAGE_TOKEN = SCREEN_ID_D501010 + "_" + "PAGE_TOKEN";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D501010Controller(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
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
            D501010Model model = new D501010Model();

            // セッションから「農業者ID」を取得する。
            model.NogyoshaId = SessionUtil.Get<string>(AppConst.SESS_NOGYOSHA_ID, HttpContext);

            // ポータル情報を取得
            NSKPortalInfoModel m = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if ((m != null) && (!String.IsNullOrEmpty(m.SKyosaiMokutekiCd)) && (!String.IsNullOrEmpty(m.SNensanHikiuke)))
            {
                model.D501010Info.SKyosaiMokutekiCd = m.SKyosaiMokutekiCd;
                model.D501010Info.SNensanHikiuke = m.SNensanHikiuke;
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
                GetKanyuJokyoList(jigyoDb, ref model);
            }

            logger.Debug("[加入状況リスト] {}件", model.tableRecords.Count());

            // 農作物共済加入状況画面を表示する
            return View(SCREEN_ID_D501010, model);
        }
        #endregion

        #region 農業者情報取得
        /// <summary>
        /// 農業者情報取得
        /// </summary>
        private void SetAgriculturalInfo(D501010Model model)
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

        #region 加入状況リスト情報の取得
        /// <summary>
        /// 加入状況リスト情報の取得
        /// </summary>
        /// <returns>加入状況リスト</returns>
        private void GetKanyuJokyoList(NskAppContext dbContext, ref D501010Model model)
        {
            try
            {
                // 実行SQL組み立て
                var sql = new StringBuilder();

                sql.Append("WITH RankedData AS ( ");
                sql.Append("    SELECT ");
                sql.Append("          t_12040.年産");
                sql.Append("        , t_12040.共済目的コード");
                sql.Append("        , m_00010.共済目的名称 AS 共済目的");
                sql.Append("        , t_12040.引受回");
                sql.Append("        , t_00010.引受計算実施日");
                sql.Append("        , t_12040.引受面積計 AS 引受面積");
                sql.Append("        , t_12040.組合員等負担共済掛金 AS 農家負担掛金");
                sql.Append("        , t_12040.基準収穫量計 AS 基準収穫量");
                sql.Append("        , t_12040.賦課金計 AS 賦課金");
                sql.Append("        , t_12040.共済金額");
                sql.Append("        , t_12090.徴収年月日 AS 掛金徴収日");
                sql.Append("        , t_12090.徴収金額 AS 掛金等徴収額");
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
                sql.Append("    LEFT JOIN t_12090_組合員等別徴収情報 t_12090 ");
                sql.Append("        ON t_12090.組合等コード = t_12040.組合等コード ");
                sql.Append("        AND t_12090.年産 = t_12040.年産 ");
                sql.Append("        AND t_12090.共済目的コード = t_12040.共済目的コード ");
                sql.Append("        AND t_12090.引受回 = t_12040.引受回 ");
                sql.Append("        AND t_12090.組合員等コード = t_12040.組合員等コード ");
                sql.Append("    LEFT JOIN m_00010_共済目的名称 m_00010 ");
                sql.Append("        ON m_00010.共済目的コード = t_12040.共済目的コード ");
                sql.Append("        WHERE t_12040.組合等コード  = @組合等コード ");
                sql.Append("        AND t_12040.年産           = @年産 ");
                sql.Append("        AND t_12040.組合員等コード  = @組合員等コード ");
                sql.Append("        AND t_12040.引受対象フラグ  = '1' ");
                sql.Append("        AND t_12040.未加入フラグ    = '0' ");
                sql.Append(") ");
                sql.Append("SELECT ");
                sql.Append("    年産");
                sql.Append("  , 共済目的コード");
                sql.Append("  , 共済目的");
                sql.Append("  , 引受回");
                sql.Append("  , 引受計算実施日");
                sql.Append("  , 引受面積");
                sql.Append("  , 農家負担掛金");
                sql.Append("  , 基準収穫量");
                sql.Append("  , 賦課金");
                sql.Append("  , 共済金額");
                sql.Append("  , 掛金徴収日");
                sql.Append("  , 掛金等徴収額 ");
                sql.Append("FROM RankedData ");
                sql.Append("WHERE rn = 1 ");
                sql.Append("ORDER BY 共済目的コード ");

                var parameters = new List<NpgsqlParameter>
            {
                new("@組合等コード", model.nogyosha.KumiaitoCd),
                new("@年産", int.Parse(model.D501010Info.SNensanHikiuke)),
                new("@組合員等コード", model.nogyosha.KumiaiintoCd),
            };

                // SQLのクエリ結果をListに格納する
                model.tableRecords = dbContext.Database.SqlQueryRaw<D501010TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
            }
            catch(Exception ex)
            {
                // 加入者状況の取得に失敗した場合、業務エラー画面を表示する。
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "加入状況の取得"));
            }
        }
        #endregion

    }
}