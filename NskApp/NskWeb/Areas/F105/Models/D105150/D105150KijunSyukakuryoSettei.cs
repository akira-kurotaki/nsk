using NskAppModelLibrary.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using NskAppModelLibrary.Models;
using System.Data;
using NskWeb.Common.Models;
using NskWeb.Areas.F105.Models.D105030;
using NskCommonLibrary.Core.Consts;
using System.Reflection;
using ModelLibrary.Models;
using CoreLibrary.Core.Dto;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using CoreLibrary.Core.Utility;

namespace NskWeb.Areas.F105.Models.D105150
{
    /// <summary>
    /// 基準収穫量設定
    /// </summary>
    public class D105150KijunSyukakuryoSettei : BasePager<D105150KijunSyukakuryoSetteiRecord>
    {
        /// <summary>
        /// メッセージエリア2
        /// </summary>
        public string MessageArea2 { get; set; } = string.Empty;

        /// <summary>類区分リスト</summary>
        public List<SelectListItem> RuiKbnList { get; set; } = new();

        /// <summary>検索条件</summary>
        public D105150SearchCondition SearchCondition { get; set; } = new();

        /// <summary>規格最大値（40）</summary>
        private const int KIKAKU_MAX = 40;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105150KijunSyukakuryoSettei()
        {
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D105150SessionInfo sessionInfo)
        {
            // ２．３．「類区分情報リスト」を取得する。
            RuiKbnList = new();
            RuiKbnList.AddRange(dbContext.M00020類名称s.Where(x =>
                (x.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(x => x.類区分).
                Select(x => new SelectListItem($"{x.類区分} {x.類短縮名称}", $"{x.類区分}"))
            );
        }


        /// <summary>
        /// 統計単位地域基準収穫量設定区分設定を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="session">セッション情報</param>
        /// <returns>検索情報</returns>
        public override List<D105150KijunSyukakuryoSetteiRecord> GetResult(NskAppContext dbContext, BaseSessionInfo session)
        {
            D105150SessionInfo sessionInfo = (D105150SessionInfo)session;

            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"    T1.組合等コード     As \"{nameof(Query6Result.KumiaitoCd)}\" ");
            query.Append($"   ,T1.組合員等コード   As \"{nameof(Query6Result.KumiaiintoCd)}\" ");
            query.Append($"   ,T2.hojin_full_nm    As \"{nameof(Query6Result.HojinFullNm)}\" ");
            query.Append($"   ,T1.類区分           As \"{nameof(Query6Result.RuiKbn)}\" ");
            query.Append($"   ,M1.類名称           As \"{nameof(Query6Result.RuiNm)}\" ");
            query.Append($"   ,T1.産地別銘柄コード As \"{nameof(Query6Result.SanchibetsuMeigaratoCd)}\" ");
            query.Append($"   ,M2.産地別銘柄名称   As \"{nameof(Query6Result.SanchibetsuMeigaratoNm)}\" ");
            query.Append($"   ,T1.営農対象外フラグ As \"{nameof(Query6Result.EinoTaisyogaiFlg)}\" ");
            query.Append($"   ,T1.平均単収         As \"{nameof(Query6Result.HeikinTanshu)}\" ");
            query.Append($"   ,T3.規格             As \"{nameof(Query6Result.Kikaku)}\" ");
            query.Append($"   ,T3.規格別割合       As \"{nameof(Query6Result.KikakubetsuWariai)}\" ");
            query.Append($"   ,cast('' || T1.xmin as integer) As \"{nameof(Query6Result.Xmin)}\" ");
            query.Append(" FROM t_11060_基準収穫量設定 T1 ");
            query.Append(" INNER JOIN v_nogyosha T2 ");
            query.Append(" ON  T1.組合等コード     = T2.kumiaito_cd ");
            query.Append(" AND T1.組合員等コード   = T2.kumiaiinto_cd ");
            query.Append(" INNER JOIN m_00020_類名称 M1 ");
            query.Append(" ON  T1.共済目的コード   = M1.共済目的コード ");
            query.Append(" AND T1.類区分           = M1.類区分 ");
            query.Append(" INNER JOIN m_00130_産地別銘柄名称設定 M2 ");
            query.Append(" ON  T1.組合等コード     = M2.組合等コード ");
            query.Append(" AND T1.年産             = M2.年産 ");
            query.Append(" AND T1.共済目的コード   = M2.共済目的コード ");
            query.Append(" AND T1.産地別銘柄コード = M2.産地別銘柄コード ");
            query.Append(" LEFT OUTER JOIN t_11070_基準収穫量設定_規格別 T3 ");
            query.Append(" ON  T1.組合等コード     = T3.組合等コード ");
            query.Append(" AND T1.年産             = T3.年産 ");
            query.Append(" AND T1.共済目的コード   = T3.共済目的コード ");
            query.Append(" AND T1.組合員等コード   = T3.組合員等コード ");
            query.Append(" AND T1.類区分           = T3.類区分 ");
            query.Append(" AND T1.産地別銘柄コード = T3.産地別銘柄コード ");
            query.Append(" AND T1.営農対象外フラグ = T3.営農対象外フラグ ");
            query.Append(" WHERE 1 = 1");
            query.Append("  AND T1.組合等コード    = @組合等コード");
            query.Append("  AND T1.年産            = @年産");
            query.Append("  AND T1.共済目的コード  = @共済目的コード");

            List<NpgsqlParameter> queryParams =
            [
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
            ];

            // 農業者情報.支所コード＝[画面：支所コード]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShishoCd))
            {
                query.Append("  AND T2.shisho_cd       = @支所コード ");
                queryParams.Add(new("支所コード", SearchCondition.TodofukenDropDownList.ShishoCd));
            }
            // 農業者情報.大地区コード＝[画面：大地区コード]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.DaichikuCd))
            {
                query.Append("  AND T2.daichiku_cd     = @大地区コード ");
                queryParams.Add(new("大地区コード", SearchCondition.TodofukenDropDownList.DaichikuCd));
            }
            // 農業者情報.小地区コード>＝[画面：小地区コード（開始）]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdFrom))
            {
                query.Append("  AND T2.shochiku_cd    >= @小地区コードFrom ");
                queryParams.Add(new("小地区コードFrom", SearchCondition.TodofukenDropDownList.ShochikuCdFrom));
            }
            // 農業者情報.小地区コード<＝[画面：小地区コード（終了）]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                query.Append("  AND T2.shochiku_cd    <= @小地区コードTo ");
                queryParams.Add(new("小地区コードTo", SearchCondition.TodofukenDropDownList.ShochikuCdTo));
            }
            // t_11060_基準収穫量設定.組合員等コード>＝[画面：組合員等コード（開始）]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.KumiaiinToCdFrom))
            {
                query.Append("  AND T1.組合員等コード >= @組合員等コードFrom ");
                queryParams.Add(new("組合員等コードFrom", SearchCondition.KumiaiinToCdFrom));
            }
            // t_11060_基準収穫量設定.組合員等コード<＝[画面：組合員等コード（終了）]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.KumiaiinToCdTo))
            {
                query.Append("  AND T1.組合員等コード <= @組合員等コードTo ");
                queryParams.Add(new("組合員等コードTo", SearchCondition.KumiaiinToCdTo));
            }
            // t_11060_基準収穫量設定.類区分＝[画面：類区分]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.RuiKbn))
            {
                query.Append("  AND T1.類区分         = @類区分 ");
                queryParams.Add(new("類区分", SearchCondition.RuiKbn));
            }
            // t_11060_基準収穫量設定.営農対象外フラグ＝[画面：営農対象外フラグ]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.EinoTaishogaiFlg))
            {
                query.Append("  AND T1.営農対象外フラグ = @営農対象外フラグ ");
                queryParams.Add(new("営農対象外フラグ", SearchCondition.EinoTaishogaiFlg));
            }
            // t_11060_基準収穫量設定.産地別銘柄等コード>＝[画面：産地別銘柄等コード(開始)]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.SanchibetsuMeigaratoCdFrom))
            {
                query.Append("  AND T1.産地別銘柄コード >= @産地別銘柄等コードFrom ");
                queryParams.Add(new("産地別銘柄等コードFrom", SearchCondition.SanchibetsuMeigaratoCdFrom));
            }
            // t_11060_基準収穫量設定.産地別銘柄等コード<＝[画面：産地別銘柄等コード（終了）]※画面で選択されているときのみ実施
            if (!string.IsNullOrEmpty(SearchCondition.SanchibetsuMeigaratoCdTo))
            {
                query.Append("  AND T1.産地別銘柄コード <= @産地別銘柄等コードTo ");
                queryParams.Add(new("産地別銘柄等コードTo", SearchCondition.SanchibetsuMeigaratoCdTo));
            }

            // ※画面で表示順の指定がある場合
            if (SearchCondition.DisplaySort1.HasValue ||
                SearchCondition.DisplaySort2.HasValue ||
                SearchCondition.DisplaySort3.HasValue)
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
                        case D105150SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" T2.shisho_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Shichoson:
                            query.Append($" T2.shichoson_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" T2.daichiku_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" T2.shochiku_cd {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.組合員等コード)} {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.RuiKbn:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.類区分 )} {SearchCondition.DisplaySortOrder1} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.SanchibetsuMeigaratoCd:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.産地別銘柄コード)} {SearchCondition.DisplaySortOrder1} ");
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
                    switch (SearchCondition.DisplaySort2)
                    {
                        case D105150SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" T2.shisho_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Shichoson:
                            query.Append($" T2.shichoson_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" T2.daichiku_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" T2.shochiku_cd {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.組合員等コード)} {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.RuiKbn:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.類区分)} {SearchCondition.DisplaySortOrder2} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.SanchibetsuMeigaratoCd:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.産地別銘柄コード)} {SearchCondition.DisplaySortOrder2} ");
                            break;
                    }
                }
                if (SearchCondition.DisplaySort3.HasValue)
                {
                    if (isPutOrder)
                    {
                        // ソート条件1またはソート条件2が出力されていた場合、カンマを付与する
                        query.Append(", ");
                    }
                    switch (SearchCondition.DisplaySort3)
                    {
                        case D105150SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" T2.shisho_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Shichoson:
                            query.Append($" T2.shichoson_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" T2.daichiku_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" T2.shochiku_cd {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.組合員等コード)} {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.RuiKbn:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.類区分)} {SearchCondition.DisplaySortOrder3} ");
                            break;
                        case D105150SearchCondition.DisplaySortType.SanchibetsuMeigaratoCd:
                            query.Append($" T1.{nameof(T11060基準収穫量設定.産地別銘柄コード)} {SearchCondition.DisplaySortOrder3} ");
                            break;
                    }
                }
            }
            // ※画面で表示順の指定がない場合
            else
            {
                // ORDER BY
                query.Append(" ORDER BY ");
                // 農業者情報.支所コード＝ASC
                query.Append($"  T2.shisho_cd {CoreConst.SortOrder.ASC} ");
                // 農業者情報.大地区コード＝ASC
                query.Append($" ,T2.daichiku_cd {CoreConst.SortOrder.ASC} ");
                // 農業者情報.小地区コード＝ASC
                query.Append($" ,T2.shochiku_cd {CoreConst.SortOrder.ASC} ");
                // t_11060_基準収穫量設定.組合員等コード＝ASC
                query.Append($" ,T1.{nameof(T11060基準収穫量設定.組合員等コード)} {CoreConst.SortOrder.ASC} ");
                // t_11060_基準収穫量設定.類区分＝ASC
                query.Append($" ,T1.{nameof(T11060基準収穫量設定.類区分)} {CoreConst.SortOrder.ASC} ");
                // t_11060_基準収穫量設定.産地別銘柄等コード＝ASC
                query.Append($" ,T1.{nameof(T11060基準収穫量設定.産地別銘柄コード)} {CoreConst.SortOrder.ASC} ");
            }

            List<Query6Result> query6Results = new();
            query6Results.AddRange(dbContext.Database.SqlQueryRaw<Query6Result>(query.ToString(), queryParams.ToArray()));

            List<D105150KijunSyukakuryoSetteiRecord> records = new();
            PropertyInfo[] props = typeof(D105150KijunSyukakuryoSetteiRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (Query6Result query6Result in query6Results)
            {
                D105150KijunSyukakuryoSetteiRecord? record = records.FirstOrDefault(x =>
                    (x.KumiaiintoCd == query6Result.KumiaiintoCd) &&
                    (x.RuiKbn == query6Result.RuiKbn)
                );
                if (record is null)
                {
                    record = new()
                    {
                        KumiaiintoCd = query6Result.KumiaiintoCd,
                        Shimei = query6Result.HojinFullNm,
                        RuiKbn = query6Result.RuiKbn,
                        SanchibetsuMeigaratoCd = query6Result.SanchibetsuMeigaratoCd,
                        SanchibetsuMeigaratoNm = query6Result.SanchibetsuMeigaratoNm,
                        EinoTaisyogaiFlg = query6Result.EinoTaisyogaiFlg == CoreConst.FLG_ON,
                        HeikinTanshu = query6Result.HeikinTanshu,
                        Xmin = query6Result.Xmin
                    };
                    records.Add(record);
                }

                // KikakubetsuWariai1 ～ KikakubetsuWariai40
                string propName = $"KikakubetsuWariai{query6Result.Kikaku}";
                props.SingleOrDefault(x => x.Name == propName)?.SetValue(record, query6Result.KikakubetsuWariai);

            }

            return records;
        }

        /// <summary>
        /// DB検索仕様書 No.6 検索結果
        /// </summary>
        private class Query6Result
        {
            /// <summary>組合等コード</summary>
            public string KumiaitoCd { get; set; } = string.Empty;
            /// <summary>組合員等コード</summary>
            public string KumiaiintoCd { get; set; } = string.Empty;
            /// <summary>氏名又は法人名</summary>
            public string HojinFullNm { get; set; } = string.Empty;
            /// <summary>類区分</summary>
            public string RuiKbn { get; set; } = string.Empty;
            /// <summary>類名称</summary>
            public string RuiNm { get; set; } = string.Empty;
            /// <summary>産地別銘柄コード</summary>
            public string SanchibetsuMeigaratoCd { get; set; } = string.Empty;
            /// <summary>産地別銘柄名称</summary>
            public string SanchibetsuMeigaratoNm { get; set; } = string.Empty;
            /// <summary>営農対象外フラグ</summary>
            public string EinoTaisyogaiFlg { get; set; } = string.Empty;
            /// <summary>平均単収</summary>
            public decimal? HeikinTanshu { get; set; }
            /// <summary>規格</summary>
            public string Kikaku { get; set; } = string.Empty;
            /// <summary>規格別割合</summary>
            public decimal? KikakubetsuWariai { get; set; }
            /// <summary>xmin</summary>
            public uint? Xmin { get; set; }
        }


        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src">コピー元</param>
        public void ApplyInput(D105150KijunSyukakuryoSettei src)
        {
            this.DispRecords = src.DispRecords;
        }

        /// <summary>
        /// t_11060_基準収穫量設定およびt_11070_基準収穫量設定_規格別の対象レコードを削除する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="delRecords"></param>
        /// <returns></returns>
        public int Delete(ref NskAppContext dbContext, D105150SessionInfo sessionInfo, ref List<D105150KijunSyukakuryoSetteiRecord> delRecords, ref string errMessage)
        {
            int delCount = 0;

            // t_11060_基準収穫量設定の対象レコードを削除する。
            StringBuilder delKijunSyukakuryoSettei = new();
            delKijunSyukakuryoSettei.Append("DELETE FROM t_11060_基準収穫量設定 ");
            delKijunSyukakuryoSettei.Append("WHERE ");
            delKijunSyukakuryoSettei.Append("     組合等コード       = @組合等コード ");
            delKijunSyukakuryoSettei.Append(" AND 年産               = @年産 ");
            delKijunSyukakuryoSettei.Append(" AND 共済目的コード     = @共済目的コード ");
            delKijunSyukakuryoSettei.Append(" AND 組合員等コード     = @組合員等コード ");
            delKijunSyukakuryoSettei.Append(" AND 類区分             = @類区分 ");
            delKijunSyukakuryoSettei.Append(" AND 産地別銘柄コード   = @産地別銘柄コード ");
            delKijunSyukakuryoSettei.Append(" AND 営農対象外フラグ   = @営農対象外フラグ ");
            delKijunSyukakuryoSettei.Append(" AND xmin               = @xmin ");

            foreach (D105150KijunSyukakuryoSetteiRecord target in delRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                List<NpgsqlParameter> delParams =
                [
                    new ("組合等コード", sessionInfo.KumiaitoCd),
                    new ("年産", sessionInfo.Nensan),
                    new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new ("組合員等コード", target.KumiaiintoCd),
                    new ("類区分", target.RuiKbn),
                    new ("産地別銘柄コード", target.SanchibetsuMeigaratoCd),
                    new ("営農対象外フラグ", target.EinoTaisyogaiFlg ? CoreConst.FLG_ON : CoreConst.FLG_OFF),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                delParams.Add(xminParam);

                int cnt = dbContext.Database.ExecuteSqlRaw(delKijunSyukakuryoSettei.ToString(), delParams);
                if (cnt == 0)
                {
                    errMessage = MessageUtil.Get("MW00002", "削除");
                    throw new DBConcurrencyException();
                }
                delCount += cnt;
            }

            // t_11070_基準収穫量設定_規格別の対象レコードを削除する。
            StringBuilder delKijunSyukakuryoSetteiKikakubetsu = new();
            delKijunSyukakuryoSetteiKikakubetsu.Append("DELETE FROM t_11070_基準収穫量設定_規格別 ");
            delKijunSyukakuryoSetteiKikakubetsu.Append("WHERE ");
            delKijunSyukakuryoSetteiKikakubetsu.Append("     組合等コード       = @組合等コード ");
            delKijunSyukakuryoSetteiKikakubetsu.Append(" AND 年産               = @年産 ");
            delKijunSyukakuryoSetteiKikakubetsu.Append(" AND 共済目的コード     = @共済目的コード ");
            delKijunSyukakuryoSetteiKikakubetsu.Append(" AND 組合員等コード     = @組合員等コード ");
            delKijunSyukakuryoSetteiKikakubetsu.Append(" AND 類区分             = @類区分 ");
            delKijunSyukakuryoSetteiKikakubetsu.Append(" AND 産地別銘柄コード   = @産地別銘柄コード ");
            delKijunSyukakuryoSetteiKikakubetsu.Append(" AND 営農対象外フラグ   = @営農対象外フラグ ");
            delKijunSyukakuryoSetteiKikakubetsu.Append(" AND 規格               = @規格 ");
            foreach (D105150KijunSyukakuryoSetteiRecord target in delRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                List<NpgsqlParameter> delParams =
                [
                    new ("組合等コード", sessionInfo.KumiaitoCd),
                    new ("年産", sessionInfo.Nensan),
                    new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new ("組合員等コード", target.KumiaiintoCd),
                    new ("類区分", target.RuiKbn),
                    new ("産地別銘柄コード", target.SanchibetsuMeigaratoCd),
                    new ("営農対象外フラグ", target.EinoTaisyogaiFlg ? CoreConst.FLG_ON : CoreConst.FLG_OFF),
                    new ("規格", DBNull.Value),
                ];

                for (int kikaku = 1; kikaku <= KIKAKU_MAX; kikaku++)
                {
                    delParams.Single(x => x.ParameterName == "規格").Value = $"{kikaku}";

                    int cnt = dbContext.Database.ExecuteSqlRaw(delKijunSyukakuryoSetteiKikakubetsu.ToString(), delParams);
                    delCount += cnt;
                }
            }

            return delCount;
        }

        /// <summary>
        /// t_11060_基準収穫量設定およびt_11070_基準収穫量設定_規格別の対象レコードを更新する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="updRecords"></param>
        /// <returns></returns>
        public int Update(ref NskAppContext dbContext, D105150SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105150KijunSyukakuryoSetteiRecord> updRecords, ref string errMessage)
        {
            int updCount = 0;

            // t_11060_基準収穫量設定の対象レコードを更新する。
            StringBuilder updKijunSyukakuryoSettei = new();
            updKijunSyukakuryoSettei.Append("UPDATE t_11060_基準収穫量設定 SET ");
            updKijunSyukakuryoSettei.Append("  平均単収              = @平均単収 ");
            updKijunSyukakuryoSettei.Append("WHERE ");
            updKijunSyukakuryoSettei.Append("     組合等コード       = @組合等コード ");
            updKijunSyukakuryoSettei.Append(" AND 年産               = @年産 ");
            updKijunSyukakuryoSettei.Append(" AND 共済目的コード     = @共済目的コード ");
            updKijunSyukakuryoSettei.Append(" AND 組合員等コード     = @組合員等コード ");
            updKijunSyukakuryoSettei.Append(" AND 類区分             = @類区分 ");
            updKijunSyukakuryoSettei.Append(" AND 産地別銘柄コード   = @産地別銘柄コード ");
            updKijunSyukakuryoSettei.Append(" AND 営農対象外フラグ   = @営農対象外フラグ ");
            updKijunSyukakuryoSettei.Append(" AND xmin               = @xmin ");

            foreach (D105150KijunSyukakuryoSetteiRecord target in updRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                List<NpgsqlParameter> updParams =
                [
                    new ("組合等コード", sessionInfo.KumiaitoCd),
                    new ("年産", sessionInfo.Nensan),
                    new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new ("組合員等コード", target.KumiaiintoCd),
                    new ("類区分", target.RuiKbn),
                    new ("産地別銘柄コード", target.SanchibetsuMeigaratoCd),
                    new ("営農対象外フラグ", target.EinoTaisyogaiFlg ? CoreConst.FLG_ON : CoreConst.FLG_OFF),
                    new ("平均単収", target.HeikinTanshu.HasValue ? target.HeikinTanshu : DBNull.Value),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                updParams.Add(xminParam);

                int cnt = dbContext.Database.ExecuteSqlRaw(updKijunSyukakuryoSettei.ToString(), updParams);
                if (cnt == 0)
                {
                    errMessage = MessageUtil.Get("MW00002", "更新"); 
                    throw new DBConcurrencyException();
                }
                updCount += cnt;
            }

            // t_11070_基準収穫量設定_規格別の対象レコードを更新する。
            PropertyInfo[] props = typeof(D105150KijunSyukakuryoSetteiRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (D105150KijunSyukakuryoSetteiRecord target in updRecords)
            {
                for (int kikaku = 1; kikaku <= KIKAKU_MAX; kikaku++)
                {
                    decimal? kikakubetsuWariai = null;
                    // KikakubetsuWariai1 ～ KikakubetsuWariai40
                    string propName = $"KikakubetsuWariai{kikaku}";
                    kikakubetsuWariai = (decimal?)props.SingleOrDefault(x => x.Name == propName)?.GetValue(target);

                    T11070基準収穫量設定規格別? updRec = dbContext.T11070基準収穫量設定規格別s.SingleOrDefault(x =>
                        (x.組合等コード == sessionInfo.KumiaitoCd) &&
                        (x.年産 == sessionInfo.Nensan) &&
                        (x.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                        (x.組合員等コード == target.KumiaiintoCd) &&
                        (x.類区分 == target.RuiKbn) &&
                        (x.産地別銘柄コード == target.SanchibetsuMeigaratoCd) &&
                        (x.営農対象外フラグ == (target.EinoTaisyogaiFlg ? CoreConst.FLG_ON : CoreConst.FLG_OFF)) &&
                        (x.規格 == $"{kikaku}")
                    );
                    if (updRec is null)
                    {
                        updRec = new()
                        {
                            // [セッション：組合等コード]
                            組合等コード = sessionInfo.KumiaitoCd,
                            // [セッション：年産]
                            年産 = (short)sessionInfo.Nensan,
                            // [セッション：共済目的コード]
                            共済目的コード = sessionInfo.KyosaiMokutekiCd,
                            // [画面：明細：組合員等コード] 
                            組合員等コード = target.KumiaiintoCd,
                            // [画面：明細：類区分] 
                            類区分 = target.RuiKbn,
                            // [画面：明細：産地別銘柄等コード] 
                            産地別銘柄コード = target.SanchibetsuMeigaratoCd,
                            // [画面：明細：営農対象外フラグ]
                            営農対象外フラグ = target.EinoTaisyogaiFlg ? CoreConst.FLG_ON : CoreConst.FLG_OFF,
                            // [画面：明細：規格]
                            規格 = $"{kikaku}",
                            // [画面：明細：規格別割合]
                            規格別割合 = kikakubetsuWariai,
                            // [共通部品：システム日時]
                            登録日時 = sysDateTime,
                            // [セッション：ユーザID]
                            登録ユーザid = userId,
                            // [共通部品：システム日時]
                            更新日時 = sysDateTime,
                            // [セッション：ユーザID]
                            更新ユーザid = userId
                        };
                        dbContext.T11070基準収穫量設定規格別s.Add(updRec);
                    }
                    else
                    {
                        updRec.規格別割合 = kikakubetsuWariai;
                        updRec.更新日時 = sysDateTime;
                        updRec.更新ユーザid = userId;
                    }
                    updCount += dbContext.SaveChanges();
                }
            }

            return updCount;
        }

        /// <summary>
        /// t_11060_基準収穫量設定およびt_11070_基準収穫量設定_規格別の対象レコードを登録する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="kikenDankaiKbnAddRecords"></param>
        /// <returns></returns>
        public int Insert(ref NskAppContext dbContext, D105150SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105150KijunSyukakuryoSetteiRecord> addRecs)
        {
            // t_11060_基準収穫量設定の対象レコードを登録する。
            List<T11060基準収穫量設定> addKijunSyukakuryoSetteiRecs = new();
            foreach (D105150KijunSyukakuryoSetteiRecord target in addRecs)
            {
                T11060基準収穫量設定 addRec = new()
                {
                    // [セッション：組合等コード]
                    組合等コード = sessionInfo.KumiaitoCd,
                    // [セッション：年産]
                    年産 = (short)sessionInfo.Nensan,
                    // [セッション：共済目的コード]
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    // [画面：明細：組合員等コード] 
                    組合員等コード = target.KumiaiintoCd,
                    // [画面：明細：類区分] 
                    類区分 = target.RuiKbn,
                    // [画面：明細：産地別銘柄等コード] 
                    産地別銘柄コード = target.SanchibetsuMeigaratoCd,
                    // [画面：明細：営農対象外フラグ]
                    営農対象外フラグ = target.EinoTaisyogaiFlg ? CoreConst.FLG_ON : CoreConst.FLG_OFF,
                    // [画面：明細：平均単収]
                    平均単収 = target.HeikinTanshu,
                    // [共通部品：システム日時]
                    登録日時 = sysDateTime,
                    // [セッション：ユーザID]
                    登録ユーザid = userId,
                    // [共通部品：システム日時]
                    更新日時 = sysDateTime,
                    // [セッション：ユーザID]
                    更新ユーザid = userId
                };
                addKijunSyukakuryoSetteiRecs.Add(addRec);
            }
            dbContext.T11060基準収穫量設定s.AddRange(addKijunSyukakuryoSetteiRecs);

            // t_11070_基準収穫量設定_規格別の対象レコードを登録する。
            PropertyInfo[] props = typeof(D105150KijunSyukakuryoSetteiRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<T11070基準収穫量設定規格別> addKijunSyukakuryoSetteiKikakubetsuRecs = new();
            foreach (D105150KijunSyukakuryoSetteiRecord target in addRecs)
            {
                for (int kikaku = 1; kikaku <= KIKAKU_MAX; kikaku++)
                {
                    decimal? kikakubetsuWariai = null;
                    // KikakubetsuWariai1 ～ KikakubetsuWariai40
                    string propName = $"KikakubetsuWariai{kikaku}";
                    kikakubetsuWariai = (decimal?)props.SingleOrDefault(x => x.Name == propName)?.GetValue(target);

                    T11070基準収穫量設定規格別 addRec = new()
                    {
                        // [セッション：組合等コード]
                        組合等コード = sessionInfo.KumiaitoCd,
                        // [セッション：年産]
                        年産 = (short)sessionInfo.Nensan,
                        // [セッション：共済目的コード]
                        共済目的コード = sessionInfo.KyosaiMokutekiCd,
                        // [画面：明細：組合員等コード] 
                        組合員等コード = target.KumiaiintoCd,
                        // [画面：明細：類区分] 
                        類区分 = target.RuiKbn,
                        // [画面：明細：産地別銘柄等コード] 
                        産地別銘柄コード = target.SanchibetsuMeigaratoCd,
                        // [画面：明細：営農対象外フラグ]
                        営農対象外フラグ = target.EinoTaisyogaiFlg ? CoreConst.FLG_ON : CoreConst.FLG_OFF,
                        // [画面：明細：規格]
                        規格 = $"{kikaku}",
                        // [画面：明細：規格別割合]
                        規格別割合 = kikakubetsuWariai,
                        // [共通部品：システム日時]
                        登録日時 = sysDateTime,
                        // [セッション：ユーザID]
                        登録ユーザid = userId,
                        // [共通部品：システム日時]
                        更新日時 = sysDateTime,
                        // [セッション：ユーザID]
                        更新ユーザid = userId
                    };
                    addKijunSyukakuryoSetteiKikakubetsuRecs.Add(addRec);
                }
            }
            dbContext.T11070基準収穫量設定規格別s.AddRange(addKijunSyukakuryoSetteiKikakubetsuRecs);

            return dbContext.SaveChanges();
        }

        /// <summary>
        /// 更新対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public override List<D105150KijunSyukakuryoSetteiRecord> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo session)
        {
            D105150SessionInfo sessionInfo = (D105150SessionInfo)session;

            List<D105150KijunSyukakuryoSetteiRecord> updRecs = new();

            // 検索結果取得
            List<D105150KijunSyukakuryoSetteiRecord> dbResults = GetResult(dbContext, sessionInfo);

            // 検索結果と画面入力値を比較
            foreach (D105150KijunSyukakuryoSetteiRecord dispRec in DispRecords)
            {
                // 追加行、削除行以外を対象とする
                if (dispRec is BasePagerRecord pagerRec && !pagerRec.IsNewRec && !pagerRec.IsDelRec)
                {
                    D105150KijunSyukakuryoSetteiRecord dbRec = dbResults.SingleOrDefault(x =>
                        (x.KumiaiintoCd == dispRec.KumiaiintoCd) &&
                        (x.RuiKbn == dispRec.RuiKbn) &&
                        (x.SanchibetsuMeigaratoCd == dispRec.SanchibetsuMeigaratoCd) &&
                        (x.EinoTaisyogaiFlg == dispRec.EinoTaisyogaiFlg)
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