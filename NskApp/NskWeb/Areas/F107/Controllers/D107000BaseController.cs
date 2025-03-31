using NskAppModelLibrary.Context;
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
using NskWeb.Areas.F000.Models.D000000;
using System.Collections.Generic;
using NskAppModelLibrary.Models;
using StackExchange.Redis;
using static CoreLibrary.Core.Utility.BatchUtil;
using Microsoft.AspNetCore.Mvc.Rendering;
using NskWeb.Areas.F107.Models;
using NskWeb.Areas.F106.Models;

namespace NskWeb.Areas.F107.Controllers
{
    /// <summary>
    /// 消込み処理共通
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [Area("F107")]
    public class D107000BaseController : CoreController
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D107000BaseController(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
        {
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

        #region 本所・支所ドロップダウンリスト取得
        /// <summary>
        /// 本所配下の支所一覧を取得する(本所含む)
        /// </summary>
        /// /// <returns>List<D107000HonshoshishoList></returns>
        protected List<D107000HonshoshishoList> getHonshoInShishoList(NSKPortalInfoModel pmodel)
        {
            Syokuin syokuin = Syokuin;

            // 名称M支所から本所を取得
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT ");
            sql.Append("    hk.支所コード  AS 本所・支所コード ");
            sql.Append("    , nm.shisho_cd || ' ' || nm.shisho_nm AS 本所・支所名称 ");
            sql.Append("FROM ");
            sql.Append("    ( SELECT ");
            sql.Append("        DISTINCT 支所コード ");  // 引受回分取得してしまうため
            sql.Append("    FROM ");
            sql.Append("        t_00010_引受回 ");
            sql.Append("    WHERE 組合等コード   = @KumiaitoCd ");
            sql.Append("    AND   共済目的コード = @KyosaimokutekiCd ");
            sql.Append("    AND   年産           = @Nensan ");
            sql.Append("    ) hk ");
            //sql.Append("LEFT JOIN v_shisho_nm nm ");
            sql.Append("INNER JOIN v_shisho_nm nm ");           // 支所マスタに存在しない引受回データが万が一あった場合
            sql.Append("ON  nm.todofuken_cd = @TodofukenCd ");
            sql.Append("AND nm.kumiaito_cd  = @KumiaitoCd ");
            sql.Append("AND nm.shisho_cd    = hk.支所コード ");

            sql.Append("WHERE '1' = '1'");

            // [セッション：利用可能な支所コード]
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                //本所以外の支所を表示する
                sql.Append("AND hk.支所コード = ANY (@ShishoList) ");
                parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).ToList()
                });
            }
            else if (!string.IsNullOrEmpty(syokuin.ShishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append(" AND hk.支所コード = @ShishoCd_1 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_1", syokuin.ShishoCd));
            }

            sql.Append("ORDER BY hk.支所コード ");

            // パラメータの設定
            // [セッション：都道府県コード]
            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));
            // [セッション：組合等]
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));
            // [セッション：共済目的]
            parameters.Add(new NpgsqlParameter("@KyosaimokutekiCd", pmodel.SKyosaiMokutekiCd));
            // [セッション：年産]
            parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(pmodel.SNensanHikiuke)));

            logger.Info("本所支所一覧取得処理を実行します。");
            logger.Info(sql);

            List<D107000HonshoshishoList> list = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D107000HonshoshishoList>(sql.ToString(), parameters.ToArray()).ToList();

            return list;
        }

        /// <summary>
        /// 本所配下の支所一覧を取得する
        /// </summary>
        /// /// <returns>List<D107000HonshoshishoList></returns>
        protected List<D107000HonshoshishoList> getHonshoShishoList(NSKPortalInfoModel pmodel)
        {
            Syokuin syokuin = Syokuin;

            // 名称M支所から本所を取得
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT ");
            sql.Append("    hk.支所コード  AS 本所・支所コード ");
            sql.Append("    , nm.shisho_cd || ' ' || nm.shisho_nm AS 本所・支所名称 ");
            sql.Append("FROM ");
            sql.Append("    ( SELECT ");
            sql.Append("        DISTINCT 支所コード ");
            sql.Append("    FROM ");
            sql.Append("        t_00010_引受回 ");
            sql.Append("    WHERE 組合等コード   = @KumiaitoCd ");
            sql.Append("    AND   共済目的コード = @KyosaimokutekiCd ");
            sql.Append("    AND   年産           = @Nensan ");
            sql.Append("    ) hk ");
            //sql.Append("LEFT JOIN v_shisho_nm nm ");
            sql.Append("INNER JOIN v_shisho_nm nm ");
            sql.Append("ON  nm.todofuken_cd = @TodofukenCd ");
            sql.Append("AND nm.kumiaito_cd  = @KumiaitoCd ");
            sql.Append("AND nm.shisho_cd    = hk.支所コード ");

            sql.Append("WHERE '1' = '1'");

            // [セッション：利用可能な支所コード]
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                //本所以外の支所を表示する
                sql.Append("AND hk.支所コード = ANY (@ShishoList) ");
                parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).Where(cd => cd != "00").ToList()
                });
            }
            else if (!string.IsNullOrEmpty(syokuin.ShishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append(" AND hk.支所コード = @ShishoCd_1 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_1", syokuin.ShishoCd));
            }

            sql.Append("ORDER BY hk.支所コード ");

            // パラメータの設定
            // [セッション：都道府県コード]
            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));
            // [セッション：組合等]
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));
            // [セッション：共済目的]
            parameters.Add(new NpgsqlParameter("@KyosaimokutekiCd", pmodel.SKyosaiMokutekiCd));
            // [セッション：年産]
            parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(pmodel.SNensanHikiuke)));

            logger.Info("本所支所一覧取得処理を実行します。");
            logger.Info(sql);

            List<D107000HonshoshishoList> list = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D107000HonshoshishoList>(sql.ToString(), parameters.ToArray()).ToList();

            return list;
        }
        #endregion

        #region 対象データの振替日を取得
        /// <summary>
        /// 対象データの振替日初期値を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">ビューモデル</param>
        /// <param name="parameters">パラメーター</param>
        /// <param syokuin="sql">職員情報</param>
        protected DateTime? GetTaisyoFurikaeYmd(Syokuin syokuin, NSKPortalInfoModel m)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT ");
            sql.Append("     MAX(振替日) AS  \"Value\" ");
            sql.Append("FROM ");
            sql.Append("    v_30020_口座振替結果 ");

            sql.Append("WHERE '1' = '1'");

            // [セッション：組合等]
            sql.Append("AND 組合等コード = @KumiaitoCd ");
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));

            if (m != null)
            {
                // セッションがnull以外の時に条件とセッションの値をパラメータにセットする
                // [セッション：共済目的]
                sql.Append("AND 共済目的コード = @KyosaiMokutekiCd ");
                parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", m.SKyosaiMokutekiCd));
                // [セッション：年産]
                sql.Append("AND 年産 = @Nensan ");
                parameters.Add(new NpgsqlParameter("@Nensan", short.Parse(m.SNensanHikiuke)));
            }

            // [固定値：共済事業コード]
            sql.Append("AND 共済事業コード = '01' ");
            // [固定値：金額区分]
            sql.Append("AND 金額区分 = '1' ");

            // 対象振替日を取得
            logger.Info("対象データ振替日取得処理を実行します。");
            logger.Info(sql);

            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<DateTime?>(sql.ToString(), parameters.ToArray()).SingleOrDefault();
        }
        #endregion

        #region バッチ予約状況確認パラメータ生成
        /// <summary>
        /// イベント名：バッチ予約状況確認パラメータを生成する
        /// </summary>
        /// <param name="syokuin">職員情報</param>
        /// <param name="ShishoCd">支所コード</param>
        /// <param name="BatchName">バッチ名称</param>
        /// /// <returns>GetBatchYoyakuListParam</returns>
        protected GetBatchYoyakuListParam BatchYoyakuJokyoParam(Syokuin syokuin, string ShishoCd, string BatchName)
        {
            GetBatchYoyakuListParam param = new()
            {
                // システム区分
                SystemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                // 都道府県コード
                TodofukenCd = syokuin.TodofukenCd,
                // 組合等コード
                KumiaitoCd = syokuin.KumiaitoCd,
                // 支所コード
                ShishoCd = ShishoCd,
                // バッチ名
                BatchNm = BatchName,
            };

            return param;
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

        #region バッチ条件(表示用)生成
        /// <summary>
        /// イベント名：バッチ条件(表示用)を生成する
        /// </summary>
        /// <param name="Nensan">引受年産</param>
        /// <param name="KyosaiMokutekiCd">共済目的コード</param>
        /// <param name="ShishoCd">支所コード</param>
        /// <param name="HikiukeCnt">引受回数</param>
        /// <param name="FurikaeDate">対象データ振替日</param>
        /// /// <returns>バッチ条件（表示用）</returns>
        protected StringBuilder CreateBatchJokenDsp(string Nensan, string KyosaiMokutekiCd, string ShishoCd, string HikiukeCnt, string FurikaeDate, string Userid)
        {
            var batchJokenDispSb = new StringBuilder();
            batchJokenDispSb.Append("[年産：" + Nensan + "]");
            batchJokenDispSb.Append("[共済目的コード：" + KyosaiMokutekiCd + "]");
            batchJokenDispSb.Append("[支所コード：" + ShishoCd + "]");
            batchJokenDispSb.Append("[引受回：" + HikiukeCnt + "]");
            batchJokenDispSb.Append("[対象データ振替日：" + FurikaeDate + "]");
            batchJokenDispSb.Append("[ユーザID：" + Userid + "]");

            return batchJokenDispSb;
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
                条件名称 = "JOUKEN_NENSAN",
                // 表示用条件値
                表示用条件値 = "JOUKEN_NENSAN",
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
                条件名称 = "JOUKEN_KYOSAI_MOKUTEKI_CD",
                // 表示用条件値
                表示用条件値 = "JOUKEN_KYOSAI_MOKUTEKI_CD",
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
                条件名称 = "JOUKEN_SHISHO",
                // 表示用条件値
                表示用条件値 = "JOUKEN_SHISHO",
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

        #region バッチ条件登録(引受回)
        /// <summary>
        /// イベント名：バッチ条件登録(引受回)
        /// </summary>
        /// <param name="Cnt">画面のカレント引受回</param>
        /// <param name="syokuin">職員情報</param>
        /// <param name="systemDate">システム日時</param>
        /// <param name="i">連番</param>
        /// /// <returns>T01050バッチ条件引受回</returns>
        protected T01050バッチ条件 CreatBatchJokenHikiukeCnt(int? Cnt, Syokuin syokuin, string BatchJokenId, DateTime systemDate, int i)
        {
            T01050バッチ条件 t01050BatchJoken = new()
            {
                // バッチ条件id
                バッチ条件id = BatchJokenId,
                // 連番
                連番 = i,
                // 条件名称
                条件名称 = "JOUKEN_HIKIUKE_KAI",
                // 表示用条件値
                表示用条件値 = "JOUKEN_HIKIUKE_KAI",
                // 条件値
                条件値 = Cnt.ToString(),
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

        #region バッチ条件登録(対象データ振替日)
        /// <summary>
        /// イベント名：バッチ条件登録(対象データ振替日)
        /// </summary>
        /// <param name="TaisyoFurikaeDate">画面の対象データ振替日</param>
        /// <param name="syokuin">職員情報</param>
        /// <param name="systemDate">システム日時</param>
        /// <param name="i">連番</param>
        /// /// <returns>T01050バッチ条件引受回</returns>
        protected T01050バッチ条件 CreatBatchJokenTaisyoFurikaeDate(DateTime? TaisyoFurikaeDate, Syokuin syokuin, string BatchJokenId, DateTime systemDate, int i)
        {
            T01050バッチ条件 t01050BatchJoken = new()
            {
                // バッチ条件id
                バッチ条件id = BatchJokenId,
                // 連番
                連番 = i,
                // 条件名称
                条件名称 = "JOUKEN_TAISHO_FURIKAE_DATE",
                // 表示用条件値
                表示用条件値 = "JOUKEN_TAISHO_FURIKAE_DATE",
                // 条件値
                条件値 = TaisyoFurikaeDate.ToString(),
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

        #region バッチ条件登録(ユーザーID)
        /// <summary>
        /// イベント名：バッチ条件登録(ユーザーID)
        /// </summary>
        /// <param name="TaisyoFurikaeDate">画面の対象データ振替日</param>
        /// <param name="syokuin">職員情報</param>
        /// <param name="systemDate">システム日時</param>
        /// <param name="i">連番</param>
        /// /// <returns>T01050バッチ条件引受回</returns>
        protected T01050バッチ条件 CreatBatchJokenUseiId(Syokuin syokuin, string BatchJokenId, DateTime systemDate, int i)
        {
            T01050バッチ条件 t01050BatchJoken = new()
            {
                // バッチ条件id
                バッチ条件id = BatchJokenId,
                // 連番
                連番 = i,
                // 条件名称
                条件名称 = "JOUKEN_USER_ID",
                // 表示用条件値
                表示用条件値 = "JOUKEN_USER_ID",
                // 条件値
                条件値 = syokuin.UserId,
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
