using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;
using NskWeb.Common.Models;
using System.Text;

namespace NskWeb.Areas.F105.Models.D105036
{
    public class D105036SearchResult : BasePager<D105036ResultRecord>
    {
        /// <summary>
        /// メッセージエリア２
        /// </summary>
        public string MessageArea2 { get; set; } = string.Empty;

        public D105036SearchCondition SearchCondition { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105036SearchResult()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        public D105036SearchResult(D105036SearchCondition searchCondition)
        {
            SearchCondition = searchCondition;
        }

        /// <summary>
        /// 検索結果を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="session">セッション情報</param>
        public override List<D105036ResultRecord> GetResult(NskAppContext dbContext, BaseSessionInfo session)
        {
            D105036SessionInfo sessionInfo = (D105036SessionInfo)session;
            List<D105036ResultRecord> records = new();

            StringBuilder query = new();
            List<NpgsqlParameter> queryParams =
            [
                new NpgsqlParameter("都道府県コード", sessionInfo.TodofukenCd),
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new NpgsqlParameter("組合員等コード", sessionInfo.KumiaiintoCd),
            ];

            query.Append(" SELECT ");
            query.Append($"   T1.kumiaiinto_cd                                as \"{nameof(D105036ResultRecord.KumiaiintoCd)}\" ");
            query.Append($"  ,T1.hojin_full_nm                                as \"{nameof(D105036ResultRecord.FullNm)}\" ");
            query.Append($"  ,T1.shisho_cd                                    as \"{nameof(D105036ResultRecord.ShishoCd)}\" ");
            query.Append($"  ,NM1.shisho_nm                                   as \"{nameof(D105036ResultRecord.ShishoNm)}\" ");
            query.Append($"  ,T1.daichiku_cd                                  as \"{nameof(D105036ResultRecord.DaichikuCd)}\" ");
            query.Append($"  ,NM2.daichiku_nm                                 as \"{nameof(D105036ResultRecord.DaichikuNm)}\" ");
            query.Append($"  ,T1.shochiku_cd                                  as \"{nameof(D105036ResultRecord.ShochikuCd)}\" ");
            query.Append($"  ,NM3.shochiku_nm                                 as \"{nameof(D105036ResultRecord.ShochikuNm)}\" ");
            query.Append($"  ,T1.shichoson_cd                                 as \"{nameof(D105036ResultRecord.ShichosonCd)}\" ");
            query.Append($"  ,NM4.shichoson_nm                                as \"{nameof(D105036ResultRecord.ShichosonNm)}\" ");
            query.Append("  ,CASE WHEN T2.未加入フラグ IS NULL THEN ''         ");
            query.Append($"   ELSE '未加入' END                               as \"{nameof(D105036ResultRecord.HikiukeTeishi)}\" ");
            query.Append("  ,CASE WHEN T3.解除フラグ IS NULL THEN ''          ");
            query.Append($"   ELSE '解除' END                                 as \"{nameof(D105036ResultRecord.Kaijo)}\" ");
            query.Append("  ,CASE WHEN T4.組合員等コード IS NULL THEN '新規' ");
            query.Append($"   ELSE '有' END                                   as \"{nameof(D105036ResultRecord.KouchiUmu)}\" ");
            query.Append($"  ,NM5.合併時識別コード                            as \"{nameof(D105036ResultRecord.GappeijiShikibetsuCd)}\" ");
            query.Append(" FROM v_nogyosha T1 ");// 農業者情報 T1 ");

            query.Append($" LEFT OUTER JOIN ({SubQuery1()}) T2 ");
            query.Append(" ON    T1.kumiaito_cd       = T2.組合等コード ");
            query.Append("  AND  T1.kumiaiinto_cd     = T2.組合員等コード ");
            query.Append("  AND  T2.共済目的コード     = @共済目的コード ");
            query.Append("  AND  T2.年産               = @年産 ");

            query.Append($" LEFT OUTER JOIN ({SubQuery2()}) T3 ");
            query.Append(" ON    T1.kumiaito_cd       = T3.組合等コード ");
            query.Append("  AND  T1.kumiaiinto_cd     = T3.組合員等コード ");
            query.Append("  AND  T3.共済目的コード     = @共済目的コード ");
            query.Append("  AND  T3.年産               = @年産 ");

            query.Append($" LEFT OUTER JOIN ({SubQuery3()}) T4 ");
            query.Append(" ON    T1.kumiaito_cd       = T4.組合等コード ");
            query.Append("  AND  T1.kumiaiinto_cd     = T4.組合員等コード ");
            query.Append("  AND  T4.共済目的コード     = @共済目的コード ");
            query.Append("  AND  T4.年産               = @年産 ");

            query.Append(" LEFT OUTER JOIN v_shisho_nm NM1 ");// 名称M支所 NM1 ");
            query.Append(" ON    T1.todofuken_cd      = NM1.todofuken_cd ");
            query.Append(" AND   T1.kumiaito_cd       = NM1.kumiaito_cd ");
            query.Append("  AND  T1.shisho_cd         = NM1.shisho_cd ");

            query.Append(" LEFT OUTER JOIN v_daichiku_nm NM2 ");// 名称M大地区 NM2 ");
            query.Append(" ON    T1.todofuken_cd      = NM2.todofuken_cd ");
            query.Append("  AND  T1.kumiaito_cd       = NM2.kumiaito_cd ");
            query.Append("  AND  T1.daichiku_cd       = NM2.daichiku_cd ");

            query.Append(" LEFT OUTER JOIN v_shochiku_nm NM3 ");// 名称M小地区 NM3 ");
            query.Append(" ON    T1.todofuken_cd      = NM3.todofuken_cd ");
            query.Append("  AND  T1.kumiaito_cd       = NM3.kumiaito_cd ");
            query.Append("  AND  T1.daichiku_cd       = NM3.daichiku_cd ");
            query.Append("  AND  T1.shochiku_cd       = NM3.shochiku_cd ");

            query.Append(" LEFT OUTER JOIN v_shichoson_nm NM4 ");// 名称M市町村 NM4 ");
            query.Append(" ON    T1.todofuken_cd      = NM4.todofuken_cd ");
            query.Append("  AND  T1.kumiaito_cd       = NM4.kumiaito_cd ");
            query.Append("  AND  T1.shichoson_cd      = NM4.shichoson_cd ");

            query.Append(" LEFT OUTER JOIN m_10220_地区別設定 NM5 ");
            query.Append(" ON    T1.kumiaito_cd       = NM5.組合等コード ");
            query.Append("  AND  NM5.年産             = @年産 ");
            query.Append("  AND  NM5.共済目的コード   = @共済目的コード ");
            query.Append("  AND  T1.daichiku_cd       = NM5.大地区コード ");
            query.Append("  AND  T1.shochiku_cd       = NM5.小地区コード ");

            query.Append($" WHERE  T1.todofuken_cd     = @都道府県コード ");
            query.Append($"  AND   T1.kumiaito_cd      = @組合等コード ");

            // ※「画面：支所」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShishoCd))
            {
                query.Append($"  AND   T1.shisho_cd = @支所 ");
                queryParams.Add(new NpgsqlParameter("支所", SearchCondition.TodofukenDropDownList.ShishoCd));
            }
            // ※「画面：市町村」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShichosonCd))
            {
                query.Append($"  AND   T1.shichoson_cd = @市町村 ");
                queryParams.Add(new NpgsqlParameter("市町村", SearchCondition.TodofukenDropDownList.ShichosonCd));
            }
            // ※「画面：大地区」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.DaichikuCd))
            {
                query.Append($"  AND   T1.daichiku_cd = @大地区 ");
                queryParams.Add(new NpgsqlParameter("大地区", SearchCondition.TodofukenDropDownList.DaichikuCd));
            }
            // ※「画面：小地区（開始）」のみ入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                query.Append($"  AND   T1.shochiku_cd = @小地区From ");
                queryParams.Add(new NpgsqlParameter("小地区From", SearchCondition.TodofukenDropDownList.ShochikuCdFrom));
            }
            // ※「画面：小地区（終了）」のみ入力がある場合
            if (string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                !string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                query.Append($"  AND   T1.shochiku_cd = @小地区To ");
                queryParams.Add(new NpgsqlParameter("小地区To", SearchCondition.TodofukenDropDownList.ShochikuCdTo));
            }
            // ※「画面：小地区（開始）」および「画面：小地区（終了）」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                !string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                query.Append($"  AND  (T1.shochiku_cd >= @小地区From ");
                query.Append($"  AND   T1.shochiku_cd <= @小地区To) ");
                queryParams.Add(new NpgsqlParameter("小地区From", SearchCondition.TodofukenDropDownList.ShochikuCdFrom));
                queryParams.Add(new NpgsqlParameter("小地区To", SearchCondition.TodofukenDropDownList.ShochikuCdTo));
            }
            // ※「画面：組合員等コード（開始）」のみ入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.KumiaiinToCdFrom) &&
                string.IsNullOrEmpty(SearchCondition.KumiaiinToCdTo))
            {
                query.Append($"  AND   T1.kumiaiinto_cd = @組合員等コードFrom ");
                queryParams.Add(new NpgsqlParameter("組合員等コードFrom", SearchCondition.KumiaiinToCdFrom));
            }
            // ※「画面：組合員等コード（終了）」のみ入力がある場合
            if (string.IsNullOrEmpty(SearchCondition.KumiaiinToCdFrom) &&
                !string.IsNullOrEmpty(SearchCondition.KumiaiinToCdTo))
            {
                query.Append($"  AND   T1.kumiaiinto_cd = @組合員等コードTo ");
                queryParams.Add(new NpgsqlParameter("組合員等コードTo", SearchCondition.KumiaiinToCdTo));
            }
            // ※「画面：組合員等コード（開始）」および「画面：組合員等コード（終了）」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.KumiaiinToCdFrom) &&
                !string.IsNullOrEmpty(SearchCondition.KumiaiinToCdTo))
            {
                query.Append($"  AND  (T1.kumiaiinto_cd >= @組合員等コードFrom ");
                query.Append($"  AND   T1.kumiaiinto_cd <= @組合員等コードTo) ");
                queryParams.Add(new NpgsqlParameter("組合員等コードFrom", SearchCondition.KumiaiinToCdFrom));
                queryParams.Add(new NpgsqlParameter("組合員等コードTo", SearchCondition.KumiaiinToCdTo));
            }
            // ※「画面：未加入」が「加入」選択の場合
            if (SearchCondition.KanyuState == D105036SearchCondition.KanyuJokyo.KANYU)
            {
                query.Append($"  AND   T2.未加入フラグ IS NOT NULL ");
            }
            // ※「画面：未加入」が「未加入」選択の場合
            if (SearchCondition.KanyuState == D105036SearchCondition.KanyuJokyo.MIKANYU)
            {
                query.Append("  AND   T2.未加入フラグ IS NULL ");
            }
            // ※「画面：解除」が「解除」選択の場合
            if (SearchCondition.KaijoState == D105036SearchCondition.KaijoJokyo.KAIJO)
            {
                query.Append("  AND   T3.解除フラグ IS NOT NULL ");
            }
            // ※「画面：解除」が「解除以外」選択の場合
            if (SearchCondition.KaijoState == D105036SearchCondition.KaijoJokyo.OTHER)
            {
                query.Append("  AND   T3.解除フラグ IS NULL ");
            }
            // ※「画面：新規（耕地情報有無）」が「新規」選択の場合
            if (SearchCondition.KouchiUmu == D105036SearchCondition.KouchiJokyo.NEW)
            {
                query.Append("  AND   T4.組合員等コード IS NULL ");
            }
            // ※「画面：新規（耕地情報有無）」が「新規以外」選択の場合
            if (SearchCondition.KouchiUmu == D105036SearchCondition.KouchiJokyo.OTHER)
            {
                query.Append("  AND   T4.組合員等コード IS NOT NULL ");
            }

            // ※「画面：表示順キー１」「画面：表示順キー２」「画面：表示順キー３」のいずれかが選択されている場合
            if (SearchCondition.DisplaySort1.HasValue || SearchCondition.DisplaySort2.HasValue || SearchCondition.DisplaySort3.HasValue)
            {
                // ORDER BY
                query.Append(" ORDER BY ");

                bool isPutOrder = false;
                //  画面指定ソート順
                if (SearchCondition.DisplaySort1.HasValue)
                {
                    isPutOrder = true;
                    switch (SearchCondition.DisplaySort1)
                    {
                        case D105036SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" T1.shisho_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Shichoson:
                            query.Append($" T1.shichoson_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" T1.daichiku_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" T1.shochiku_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.kumiaiinto_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                    }
                }
                if (SearchCondition.DisplaySort2.HasValue)
                {
                    if (isPutOrder)
                    {
                        // ソート条件1が出力されていた場合、カンマを付与する
                        query.Append(", ");
                    }
                    isPutOrder = true;
                    switch (SearchCondition.DisplaySort2)
                    {
                        case D105036SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" T1.shisho_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Shichoson:
                            query.Append($" T1.shichoson_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" T1.daichiku_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" T1.shochiku_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.kumiaiinto_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                    }
                }
                if (SearchCondition.DisplaySort3.HasValue)
                {
                    if (isPutOrder)
                    {
                        // ソート条件1or2が出力されていた場合、カンマを付与する
                        query.Append(", ");
                    }
                    switch (SearchCondition.DisplaySort3)
                    {
                        case D105036SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" T1.shisho_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Shichoson:
                            query.Append($" T1.shichoson_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" T1.daichiku_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" T1.shochiku_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105036SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.kumiaiinto_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                    }
                }
            }

            records.AddRange(dbContext.Database.SqlQueryRaw<D105036ResultRecord>(query.ToString(), queryParams.ToArray()));

            return records;
        }

        /// <summary>
        /// サブクエリ１
        /// </summary>
        /// <returns></returns>
        private string SubQuery1()
        {
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append("   組合等コード ");
            query.Append("  ,組合員等コード ");
            query.Append("  ,共済目的コード ");
            query.Append("  ,年産 ");
            query.Append("  ,未加入フラグ ");
            query.Append(" FROM t_11010_個人設定 "); 
            query.Append(" GROUP BY ");  
            query.Append("   組合等コード ");
            query.Append("  ,組合員等コード ");
            query.Append("  ,共済目的コード ");
            query.Append("  ,年産 ");
            query.Append("  ,未加入フラグ ");

            return query.ToString();
        }

        /// <summary>
        /// サブクエリ２
        /// </summary>
        /// <returns></returns>
        private string SubQuery2()
        {
            StringBuilder query = new();
            query.Append(" SELECT ");  
            query.Append("   組合等コード ");
            query.Append("  ,組合員等コード ");
            query.Append("  ,共済目的コード ");
            query.Append("  ,年産 ");
            query.Append("  ,'1' As 解除フラグ ");
            query.Append(" FROM t_11020_個人設定解除 "); 
            query.Append(" GROUP BY ");  
            query.Append("   組合等コード ");
            query.Append("  ,組合員等コード ");
            query.Append("  ,共済目的コード ");
            query.Append("  ,年産 ");
            query.Append("  ,解除フラグ ");

            return query.ToString();
        }

        /// <summary>
        /// サブクエリ３
        /// </summary>
        /// <returns></returns>
        private string SubQuery3()
        {
            StringBuilder query = new();
            query.Append(" SELECT ");  
            query.Append("   組合等コード ");
            query.Append("  ,組合員等コード ");
            query.Append("  ,共済目的コード ");
            query.Append("  ,年産 ");
            query.Append(" FROM t_11090_引受耕地 ");
            query.Append(" GROUP BY ");  
            query.Append("   組合等コード ");
            query.Append("  ,組合員等コード ");
            query.Append("  ,共済目的コード ");
            query.Append("  ,年産 ");
            return query.ToString();
        }

        /// <summary>
        /// 更新対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public override List<D105036ResultRecord> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo sessionInfo)
        {
            return [];
        }
    }
}