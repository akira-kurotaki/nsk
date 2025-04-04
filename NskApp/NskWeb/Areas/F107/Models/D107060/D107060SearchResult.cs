using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using NuGet.Protocol.Core.Types;
using System.ComponentModel;
using System.Data;
using System.Text;
using NskWeb.Common.Models;
using static NskWeb.Areas.F107.Models.D107060.D107060SearchResult;
using NskWeb.Areas.F105.Models.D105030;

namespace NskWeb.Areas.F107.Models.D107060
{
    /// <summary>
    /// 引受情報検索結果
    /// </summary>
    [Serializable]
    public class D107060SearchResult : BasePager<D107060TableRecord>
    {
        /// <summary>徴収区分一括設定ドロップダウンリスト選択値</summary>
        //public List<SelectListItem> ChoshuKbnList { get; set; } = new();

        /// <summary>徴収理由一括設定ドロップダウンリスト選択値</summary>
        //public List<SelectListItem> ChoshuRiyuList { get; set; } = new();

        public D107060SearchCondition SearchCondition { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D107060SearchResult()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="searchCondition"></param>
        public D107060SearchResult(D107060SearchCondition searchCondition)
        {
            SearchCondition = searchCondition;
            DisplayCount = SearchCondition.DisplayCount ?? CoreConst.PAGE_SIZE;
        }

        /// <summary>
        /// 引受情報を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <returns>検索情報</returns>
        public override List<D107060TableRecord> GetResult(NskAppContext dbContext, BaseSessionInfo session)
        {
            D107060SessionInfo sessionInfo = (D107060SessionInfo)session;
            List<D107060TableRecord> records = new();

            StringBuilder query = new();
            List<NpgsqlParameter> queryParams =
            [
                // セッションからの取得
                //new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("組合等コード", "999"),
                //new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("年産", 2025),
                new NpgsqlParameter("共済目的コード", "11"),

                // 画面からの取得
                //new NpgsqlParameter("引受回", SearchCondition.HikiukeCnt),
                new NpgsqlParameter("引受回", 2),
                //new NpgsqlParameter("表示数", SearchCondition.DisplayCount),
                new NpgsqlParameter("表示数", 10),
                //new NpgsqlParameter("選択ページ数", SearchCondition.),
                new NpgsqlParameter("選択ページ数", 1),
            ];

            query.Append(" SELECT ");
            query.Append("   HJ1.組合員等コード KumiaiintoCd ");
            query.Append("  ,NJ.hojin_full_nm NogyosyaNm ");
            query.Append("  ,HJ1.納入額 ChoteiGaku ");
            query.Append("  ,HJ1.組合員等負担共済掛金 KyosaiKakekin ");
            query.Append("  ,HJ1.賦課金計 Hukakin ");
            query.Append("  ,HJ1.引受解除返還賦課金額 ");
            query.Append("  ,CJ_PRE1.前回迄徴収金額 ChoshuzumiGaku ");
            query.Append("  ,CJ_PRE1.前回迄内農家負担掛金 ");
            query.Append("  ,CJ_PRE1.前回迄内賦課金 ");
            query.Append("  ,CJ_PRE1.前回迄引受解除徴収賦課金額 ");
            query.Append("  ,HJ1.解除フラグ ");
            query.Append("  ,SAGAKU.組合員等負担共済掛金_差額 ");
            query.Append("  ,SAGAKU.賦課金計_差額 ");
            query.Append("  ,SAGAKU.今回引受解除徴収賦課金額 ");
            query.Append("  ,CJ_PRE1.前回徴収年月日 ZenkaiChoshuYmd ");
            query.Append("  ,HJ1.納入額 - (CJ_PRE1.前回迄徴収金額 - (CJ_PRE1.前回迄引受解除徴収賦課金額 + SAGAKU.今回引受解除徴収賦課金額)) SeikyuGaku ");
            query.Append("  ,CJ.徴収金額 ChoshuGakuNow ");
            query.Append("  ,CJ.徴収年月日 ChoshuYmd ");
            query.Append("  ,CAST(CJ.徴収区分コード AS Integer) ChoshuKbn ");
            //query.Append("  ,CK.choshu_kbn_display              AS \"{nameof(D107060TableRecord.Riyu)}\" ");
            query.Append("  ,CAST(CJ.徴収理由コード AS Integer) Riyu ");
            //query.Append("  ,CR.choshu_riyu_nm                  AS \"{nameof(D107060TableRecord.Riyu)}\" ");
            //query.Append("  ,CJ.徴収区分コード || ' ' || CK.choshu_kbn_display ChoshuKbn ");
            //query.Append("  ,CJ.徴収理由コード || ' ' || CR.choshu_riyu_nm Riyu ");
            query.Append("  ,CJ.徴収者 Choshusya ");
            query.Append("  ,CJ.自動振替フラグ JidoFurikaeFlg ");
            query.Append("  ,CAST('' || CJ.xmin AS Integer) ChoshuJohoXmin ");
            query.Append(" FROM t_12040_組合員等別引受情報    HJ1 ");
            query.Append(" LEFT JOIN v_nogyosha         NJ ");
            query.Append("      ON  HJ1.組合等コード   = NJ.kumiaito_cd ");
            query.Append("      AND HJ1.組合員等コード = NJ.kumiaiinto_cd ");
            query.Append(" LEFT JOIN ( ");
            query.Append("      SELECT ");
            query.Append("           HJ.組合等コード ");
            query.Append("          ,HJ.年産 ");
            query.Append("          ,HJ.共済目的コード ");
            query.Append("          ,HJ.組合員等コード ");
            query.Append("          ,(HJ.組合員等負担共済掛金 - CJ_PRE.前回迄内農家負担掛金) AS 組合員等負担共済掛金_差額 ");
            query.Append("          ,(HJ.賦課金計 - (CJ_PRE.前回迄内賦課金 - CJ_PRE.前回迄引受解除徴収賦課金額)) AS 賦課金計_差額 ");
            query.Append("          ,(CASE WHEN ");
            query.Append("                  HJ.解除フラグ = '1' ");
            query.Append("              THEN ");
            query.Append("                  HJ.引受解除返還賦課金額 - (HJ.賦課金計 - (CJ_PRE.前回迄内賦課金 - CJ_PRE.前回迄引受解除徴収賦課金額)) ");
            query.Append("              ELSE ");
            query.Append("                  0 ");
            query.Append("          END) AS 今回引受解除徴収賦課金額 ");
            query.Append("      FROM ");
            query.Append("          t_12040_組合員等別引受情報 HJ ");
            query.Append("          LEFT JOIN ( ");
            query.Append("              SELECT ");
            query.Append("                  組合等コード ");
            query.Append("                  ,年産 ");
            query.Append("                  ,共済目的コード ");
            query.Append("                  ,組合員等コード ");
            query.Append("                  ,SUM(内農家負担掛金)             AS 前回迄内農家負担掛金 ");
            query.Append("                  ,SUM(内賦課金)                   AS 前回迄内賦課金 ");
            query.Append("                  ,SUM(引受解除徴収賦課金額)       AS 前回迄引受解除徴収賦課金額 ");
            query.Append("              FROM ");
            query.Append("                  t_12090_組合員等別徴収情報 ");
            query.Append("              WHERE ");
            query.Append("                  引受回 < @引受回 ");
            query.Append("              GROUP BY ");
            query.Append("                  組合等コード ");
            query.Append("                  ,年産 ");
            query.Append("                  ,共済目的コード ");
            query.Append("                  ,組合員等コード ");
            query.Append("          ) CJ_PRE ");
            query.Append("          ON  HJ.組合等コード   = CJ_PRE.組合等コード ");
            query.Append("          AND HJ.年産           = CJ_PRE.年産 ");
            query.Append("          AND HJ.共済目的コード = CJ_PRE.共済目的コード ");
            query.Append("          AND HJ.組合員等コード = CJ_PRE.組合員等コード ");
            query.Append("      ) SAGAKU ");
            query.Append("      ON  HJ1.組合等コード   = SAGAKU.組合等コード ");
            query.Append("      AND HJ1.年産           = SAGAKU.年産 ");
            query.Append("      AND HJ1.共済目的コード = SAGAKU.共済目的コード ");
            query.Append("      AND HJ1.組合員等コード = SAGAKU.組合員等コード ");
            query.Append(" LEFT JOIN ( ");
            query.Append("      SELECT ");
            query.Append("          組合等コード ");
            query.Append("          ,年産 ");
            query.Append("          ,共済目的コード ");
            query.Append("          ,組合員等コード ");
            query.Append("          ,SUM(徴収金額)             AS 前回迄徴収金額 ");
            query.Append("          ,SUM(内農家負担掛金)       AS 前回迄内農家負担掛金 ");
            query.Append("          ,SUM(内賦課金)             AS 前回迄内賦課金 ");
            query.Append("          ,SUM(引受解除徴収賦課金額) AS 前回迄引受解除徴収賦課金額 ");
            query.Append("          ,MAX(徴収年月日)           AS 前回徴収年月日 ");
            query.Append("      FROM ");
            query.Append("          t_12090_組合員等別徴収情報 ");
            query.Append("      WHERE ");
            query.Append("          引受回 < @引受回 ");
            query.Append("      GROUP BY ");
            query.Append("          組合等コード ");
            query.Append("          ,年産 ");
            query.Append("          ,共済目的コード ");
            query.Append("          ,組合員等コード ");
            query.Append(" ) CJ_PRE1 ");
            query.Append(" ON  HJ1.組合等コード   = CJ_PRE1.組合等コード ");
            query.Append(" AND HJ1.年産           = CJ_PRE1.年産 ");
            query.Append(" AND HJ1.共済目的コード = CJ_PRE1.共済目的コード ");
            query.Append(" AND HJ1.組合員等コード = CJ_PRE1.組合員等コード ");
            query.Append(" LEFT JOIN t_12090_組合員等別徴収情報 CJ ");
            query.Append("      ON  HJ1.組合等コード   = CJ.組合等コード ");
            query.Append("      AND HJ1.年産           = CJ.年産 ");
            query.Append("      AND HJ1.共済目的コード = CJ.共済目的コード ");
            query.Append("      AND HJ1.引受回         = CJ.引受回 ");
            query.Append("      AND HJ1.組合員等コード = CJ.組合員等コード ");
            query.Append(" LEFT JOIN v_choshu_kbn CK ");
            query.Append("      ON  CAST(CJ.徴収区分コード AS Integer) = CK.choshu_kbn_cd ");
            query.Append(" LEFT JOIN v_choshu_riyu CR ");
            query.Append("      ON  CAST(CJ.徴収区分コード AS Integer) = CR.choshu_kbn_cd ");
            query.Append("      AND CAST(CJ.徴収理由コード AS Integer) = CR.choshu_riyu_cd ");
            query.Append(" LEFT JOIN v_nogyosha_kinyukikan NK ");
            query.Append("      ON  NJ.nogyosha_id = NK.nogyosha_id ");
            query.Append(" INNER JOIN ( ");
            query.Append("      SELECT ");
            query.Append("           共済事業コード ");
            query.Append("           ,共済目的コード_fim ");
            query.Append("           ,振込区分 ");
            query.Append("      FROM ");
            query.Append("           m_00220_共済目的対応 KT1 ");
            query.Append("      WHERE ");
            query.Append("           共済目的コード_nsk = @共済目的コード ");
            query.Append("      AND ");
            query.Append("           採用順位 = ( ");
            query.Append("              SELECT ");
            query.Append("                  MIN(採用順位) AS 採用順位 ");
            query.Append("              FROM ");
            query.Append("                  m_00220_共済目的対応 KT2 ");
            query.Append("              WHERE ");
            query.Append("                      KT2.共済事業コード     = KT1.共済事業コード ");
            query.Append("                  AND KT2.共済目的コード_fim = KT1.共済目的コード_fim ");
            query.Append("              ) ");
            query.Append(" ) KT ");
            query.Append("      ON  NK.kyosai_jigyo_cd        = KT.共済事業コード ");
            query.Append("      AND NK.kyosai_mokutekito_cd   = KT.共済目的コード_fim ");
            query.Append("      AND NK.furikomi_hikiotoshi_cd = KT.振込区分 ");
            query.Append(" WHERE ");
            query.Append("          HJ1.組合等コード        =  @組合等コード ");
            query.Append("      AND HJ1.年産                =  @年産 ");
            query.Append("      AND HJ1.共済目的コード      =  @共済目的コード ");

            // [画面:支所]の選択があるかつ本所以外の場合
            //if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShishoCd) && SearchCondition.TodofukenDropDownList.ShishoCd != "00")
            //{
            //    query.Append("      AND HJ1.支所コード          =  @支所コード ");
            //    queryParams.Add(new NpgsqlParameter("支所コード", SearchCondition.TodofukenDropDownList.ShishoCd));
            //}

            query.Append("      AND HJ1.引受回              =  @引受回 ");

            // [画面:大地区コード]の選択がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.DaichikuCd)) 
            {
                query.Append("      AND HJ1.大地区コード        =  @大地区コード ");
                queryParams.Add(new NpgsqlParameter("大地区コード", SearchCondition.TodofukenDropDownList.DaichikuCd));
            }

            // [画面:小地区コード開始]の選択がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdFrom)) 
            {
                query.Append("      AND HJ1.小地区コード       >=  @小地区コード開始 ");
                queryParams.Add(new NpgsqlParameter("小地区コード開始", SearchCondition.TodofukenDropDownList.ShochikuCdFrom));
            }

            // [画面:小地区コード終了]の選択がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                query.Append("      AND HJ1.小地区コード       <=  @小地区コード終了 ");
                queryParams.Add(new NpgsqlParameter("小地区コード終了", SearchCondition.TodofukenDropDownList.ShochikuCdTo));
            }

            // [画面:市町村コード]の選択がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShichosonCd))
            {
                query.Append("      AND HJ1.市町村コード        =  @市町村コード ");
                queryParams.Add(new NpgsqlParameter("市町村コード", SearchCondition.TodofukenDropDownList.ShichosonCd));
            }

            // [画面:組合員等コード開始]の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.KumiaiintoCdFrom))
            {
                query.Append("      AND HJ1.組合員等コード     >=  @組合員等コード開始 ");
                queryParams.Add(new NpgsqlParameter("組合員等コード開始", SearchCondition.KumiaiintoCdFrom));
            }

            // [画面:組合員等コード終了]の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.KumiaiintoCdTo))
            {
                query.Append("      AND HJ1.組合員等コード     <=  @組合員等コード終了 ");
                queryParams.Add(new NpgsqlParameter("組合員等コード終了", SearchCondition.KumiaiintoCdTo));
            }

            // [画面:徴収予定区分]の選択がある場合
            if (!string.IsNullOrEmpty(SearchCondition.ChoshuYoteiKbn))
            {
                query.Append("      AND NK.振込引落区分コード  =  @区分コード ");
                queryParams.Add(new NpgsqlParameter("区分コード", SearchCondition.ChoshuYoteiKbn));
            }

            // [画面:自動振替予定]の選択がある場合
            if (!string.IsNullOrEmpty(SearchCondition.JidoFurikaeYotei))
            {
                query.Append("      AND NK.自動振替処理の有無  =  @自動振替予定 ");
                queryParams.Add(new NpgsqlParameter("自動振替予定", SearchCondition.JidoFurikaeYotei));
            }

            // [画面:未納]のチェックがある場合
            if (SearchCondition.MinosyaOnly)
            {
                //query.Append("      AND HJ1.納入額              <> (CJ.徴収金額 + CJ_PRE1.前回迄徴収金額 -  ");
                //query.Append("                                      (CJ_PRE1.前回迄引受解除徴収賦課金額 + 今回引受解除徴収賦課金額)) ");
            }

            query.Append("      AND HJ1.類区分               = '0' ");
            query.Append("      AND HJ1.統計単位地域コード   = '0' ");
            query.Append("      AND (HJ1.納入額             <> 0 ");
            query.Append("      OR   CJ_PRE1.前回迄徴収金額 <> 0 ) ");
            query.Append(" ORDER BY ");


            //if (!string.IsNullOrEmpty(SearchCondition.DisplaySortOrder1.ToString()))
            //{
            //    query.Append("      @表示順キー1 = @表示順1 ");
            //}

            //if (!string.IsNullOrEmpty(SearchCondition.DisplaySortOrder2.ToString()))
            //{
            //    query.Append("      @表示順キー2 = @表示順2 ");
            //}

            //if (!string.IsNullOrEmpty(SearchCondition.DisplaySortOrder3.ToString()))
            //{
            //    query.Append("      @表示順キー3 = @表示順3 ");
            //}

            //if (string.IsNullOrEmpty(SearchCondition.DisplaySortOrder1.ToString()) && string.IsNullOrEmpty(SearchCondition.DisplaySortOrder2.ToString()) && string.IsNullOrEmpty(SearchCondition.DisplaySortOrder3.ToString()))
            //{
                query.Append("      HJ1.組合員等コード ASC ");
            //}

            query.Append(" LIMIT ");
            query.Append("      @表示数 ");
            query.Append(" OFFSET ");
            query.Append("      (CASE WHEN ");
            query.Append("              @選択ページ数 > 0 ");
            query.Append("          THEN ");
            query.Append("              (@選択ページ数 - 1) * @表示数 ");
            query.Append("          ELSE ");
            query.Append("              0 ");
            query.Append("      END) ");

            records.AddRange(dbContext.Database.SqlQueryRaw<D107060TableRecord>(query.ToString(), queryParams.ToArray()));

            return records;
        }

        /// <summary>
        /// t_12090_組合員等別徴収情報の対象レコードの消込みをする
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="sysDateTime">システム日付</param>
        /// <param name="searchConditions">検索画面ビューモデル</param>
        /// <param name="AddRecords">登録対象のレコード</param>
        /// <returns>登録件数</returns>
        public int AppendKeshikomi(ref NskAppContext dbContext
                                        , D107060SessionInfo sessionInfo
                                        , string userId
                                        , DateTime? sysDateTime
                                        , ref List<D107060TableRecord> AddRecords
                                        , ref D107060Model model)
        {
            // 消込み件数カウンター
            int addCount = 0;

            // (1) t_11090_引受耕地の対象レコードを削除するSQL
            StringBuilder DelSql = new();

            DelSql.Append(" DELETE FROM t_12090_組合員等別徴収情報 ");
            DelSql.Append(" WHERE ");
            DelSql.Append("     組合等コード   = @組合等コード ");
            DelSql.Append(" AND 年産           = @年産 ");
            DelSql.Append(" AND 共済目的コード = @共済目的コード ");
            DelSql.Append(" AND 引受回         = @引受回 ");
            DelSql.Append(" AND 組合員等コード = @組合員等コード ");
            DelSql.Append(" AND xmin           = @xmin ");

            // t_12090_組合員等別徴収情報の対象レコードを登録するリスト
            List<T12090組合員等別徴収情報> addListRecs = new();

            // for用index
            int index = 0;

            foreach (D107060TableRecord target in AddRecords)
            {
                // 賦課金分岐用解除フラグ
                var kaijoFlg = 0;

                if (!target.ChoshuJohoXmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                // リストのデータが非活性の場合対象外
                if (target.ActivityFlg == 1)
                {
                    continue;
                }

                // 徴収年月日に入力が無い場合対象外
                if (target.ChoshuYmd == null) 
                {
                    continue;
                }

                List<NpgsqlParameter> delParams =
                [
                    new("組合等コード", sessionInfo.KumiaitoCd),
                    new("年産", sessionInfo.Nensan),
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("引受回", sessionInfo.HikiukeCnt),
                    new("組合員等コード", target.KumiaiintoCd),
                    //new("xmin", sessionInfo.Xmin),
                    new("xmin", target.ChoshuJohoXmin),
                ];

                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.ChoshuJohoXmin };
                delParams.Add(xminParam);

                //削除を実行
                int cnt = dbContext.Database.ExecuteSqlRaw(DelSql.ToString(), delParams);

                //削除されたレコードが0件の場合はエラー
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }

                // 正常に削除された場合登録予約を続行
                addListRecs.Add(new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    引受回 = (short)sessionInfo.HikiukeCnt,
                    組合員等コード = target.KumiaiintoCd,
                    徴収区分コード = target.ChoshuKbn.ToString(),
                    自動振替フラグ = "0",
                    徴収理由コード = target.Riyu.ToString(),
                    徴収年月日 = target.ChoshuYmd,
                    徴収者 = target.Choshusya,
                    徴収金額 = target.SeikyuGaku,
                    内農家負担掛金 = 0,
                    // 賦課金は解除フラグによって分岐
                    内賦課金 = (kaijoFlg == 1 ? 1 : 0),
                    //引受解除徴収賦課金額 = target.Kbn,
                    登録日時 = sysDateTime,
                    登録ユーザid = userId,
                    更新日時 = sysDateTime,
                    更新ユーザid = userId,
                });

                // 今回消込フラグを1にする
                model.SearchResult.DispRecords[index].KeshikomiJissiFlg = 1;
                
                index++;
            }

            // 登録処理を実行
            dbContext.T12090組合員等別徴収情報s.AddRange(addListRecs);
            dbContext.SaveChanges();

            // 登録されたレコード数を取得
            addCount += addListRecs.Count;

            return addCount;
        }

        /// <summary>
        /// t_12090_組合員等別徴収情報の対象レコードの消込み解除をする
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="searchConditions">検索画面ビューモデル</param>
        /// <param name="ListDelRecords">削除対象のレコード</param>
        /// <returns>消込解除件数</returns>
        public int DeleteKeshikomi(ref NskAppContext dbContext
                                            , D107060SessionInfo sessionInfo
                                            , ref List<D107060TableRecord> ListDelRecords
                                            , ref D107060Model model)
        {
            // 消込み解除件数カウンター
            int delCount = 0;

            // (1) t_11090_引受耕地の対象レコードを削除するSQL
            StringBuilder DelSql = new();

            DelSql.Append(" DELETE FROM t_12090_組合員等別徴収情報 ");
            DelSql.Append(" WHERE ");
            DelSql.Append("     組合等コード   = @組合等コード ");
            DelSql.Append(" AND 年産           = @年産 ");
            DelSql.Append(" AND 共済目的コード = @共済目的コード ");
            DelSql.Append(" AND 引受回         = @引受回 ");
            DelSql.Append(" AND 組合員等コード = @組合員等コード ");
            DelSql.Append(" AND xmin           = @xmin ");

            // for用index
            int index = 0;

            foreach (D107060TableRecord target in ListDelRecords)
            {
                if (!target.ChoshuJohoXmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                // リストのデータが非活性の場合対象外
                if (target.ActivityFlg == 1)
                {
                    continue;
                }

                // 徴収年月日に入力がある場合対象外
                if (target.ChoshuYmd != null)
                {
                    continue;
                }

                List<NpgsqlParameter> delParams =
                [
                    new("組合等コード", sessionInfo.KumiaitoCd),
                    new("年産", sessionInfo.Nensan),
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("引受回", sessionInfo.HikiukeCnt),
                    new("組合員等コード", target.KumiaiintoCd),
                    //new("xmin", sessionInfo.Xmin),
                    new("xmin", target.ChoshuJohoXmin),
                ];

                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.ChoshuJohoXmin };
                delParams.Add(xminParam);

                //削除を実行
                int cnt = dbContext.Database.ExecuteSqlRaw(DelSql.ToString(), delParams);

                //削除されたレコードが0件の場合はエラー
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }

                // 今回消込フラグを0にする
                model.SearchResult.DispRecords[index].KeshikomiJissiFlg = 0;

                delCount += cnt;
                index++;
            }

            return delCount;
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D107060SearchResult src)
        {
            this.DispRecords = src.DispRecords;
            this.AllRecCount = src.AllRecCount;
        }

        /// <summary>
        /// 検索結果表示対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public override List<D107060TableRecord> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo session)
        {
            D107060SessionInfo sessionInfo = (D107060SessionInfo)session;
            List<D107060TableRecord> updRecs = new();

            // 検索結果取得
            List<D107060TableRecord> dbResults = GetResult(dbContext, sessionInfo);

            // 検索結果と画面入力値を比較
            foreach (D107060TableRecord dispRec in DispRecords)
            {
                // 追加行、削除行以外を対象とする
                if (dispRec is BasePagerRecord pagerRec && !pagerRec.IsNewRec && !pagerRec.IsDelRec)
                {
                    D107060TableRecord dbRec = dbResults.SingleOrDefault(x =>
                        (x.KumiaiintoCd == dispRec.KumiaiintoCd)
                    );

                    // DB検索結果と比較し差分ありの場合、更新対象とする
                    if ((dbRec is not null) && !dispRec.Compare(dbRec))
                    {
                        // 画面入力を更新対象として追加
                        updRecs.Add(dispRec);
                    }
                }
            }

            return updRecs;
        }
    }
}
