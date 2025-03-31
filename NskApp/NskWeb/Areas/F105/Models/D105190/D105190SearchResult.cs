using System.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Common.Models;

namespace NskWeb.Areas.F105.Models.D105190
{
    public class D105190SearchResult : BasePager<D105190ResultRecord>
    {
        /// <summary>
        /// メッセージエリア２
        /// </summary>
        public string MessageArea2 { get; set; } = string.Empty;

        public D105190SearchCondition SearchCondition { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105190SearchResult()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        public D105190SearchResult(D105190SearchCondition searchCondition)
        {
            SearchCondition = searchCondition;
        }

        /// <summary>
        /// 検索結果を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="session">セッション情報</param>
        public override List<D105190ResultRecord> GetResult(NskAppContext dbContext, BaseSessionInfo session)
        {
            D105190SessionInfo sessionInfo = (D105190SessionInfo)session;
            List<D105190ResultRecord> records = new();

            StringBuilder query = new();
            List<NpgsqlParameter> queryParams =
            [
                new NpgsqlParameter("都道府県コード", sessionInfo.TodofukenCd),
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new NpgsqlParameter("組合員等コード", sessionInfo.KumiaiintoCd),
                new NpgsqlParameter("組合員等コード", sessionInfo.KumiaiintoCd),
                new NpgsqlParameter("類区分", SearchCondition.RuiKbn)
            ];


            #region 抽出対象
            query.Append(" SELECT ");
            query.Append($"   T1.組合員等コード              as \"{nameof(D105190ResultRecord.KumiaiintoCd)}\" ");
            query.Append($"  ,M1.hojin_full_nm               as \"{nameof(D105190ResultRecord.FullNm)}\" ");
            query.Append($"  ,T1.共済金額                    as \"{nameof(D105190ResultRecord.KyousaiKingaku)}\" ");
            query.Append($"  ,cast('' || T1.Xmin as integer) as \"{nameof(D105190ResultRecord.Xmin)}\" ");
            #endregion

            #region 検索テーブル
            query.Append(" FROM ");
            query.Append("   t_12080_組合員等別共済金額設定 T1 ");
            query.Append("   INNER JOIN v_nogyosha M1 ");
            query.Append("     ON T1.組合等コード = M1.kumiaito_cd ");
            query.Append("     AND T1.組合員等コード = M1.kumiaiinto_cd ");
            #endregion

            #region 検索条件
            query.Append(" WHERE ");
            query.Append("   T1.組合等コード = @組合等コード ");
            query.Append("   AND T1.年産 = @年産 ");
            query.Append("   AND T1.共済目的コード = @共済目的コード ");
            query.Append("   AND T1.類区分 = @類区分 ");

            // ※「画面：支所」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShishoCd))
            {
                query.Append("   AND M1.shisho_cd = @支所 ");
                queryParams.Add(new NpgsqlParameter("支所", SearchCondition.TodofukenDropDownList.ShishoCd));
            }
            // ※「画面：大地区」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.DaichikuCd))
            {
                query.Append("   AND M1.daichiku_cd = @大地区 ");
                queryParams.Add(new NpgsqlParameter("大地区", SearchCondition.TodofukenDropDownList.DaichikuCd));
            }
            // ※「画面：小地区（開始）」のみ入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                query.Append($"  AND   M1.shochiku_cd = @小地区From ");
                queryParams.Add(new NpgsqlParameter("小地区From", SearchCondition.TodofukenDropDownList.ShochikuCdFrom));
            }
            // ※「画面：小地区（終了）」のみ入力がある場合
            if (string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                !string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                query.Append($"  AND   M1.shochiku_cd = @小地区To ");
                queryParams.Add(new NpgsqlParameter("小地区To", SearchCondition.TodofukenDropDownList.ShochikuCdTo));
            }
            // ※「画面：小地区（開始）」および「画面：小地区（終了）」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                !string.IsNullOrEmpty(SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                query.Append($"  AND  (M1.shochiku_cd >= @小地区From ");
                query.Append($"  AND   M1.shochiku_cd <= @小地区To) ");
                queryParams.Add(new NpgsqlParameter("小地区From", SearchCondition.TodofukenDropDownList.ShochikuCdFrom));
                queryParams.Add(new NpgsqlParameter("小地区To", SearchCondition.TodofukenDropDownList.ShochikuCdTo));
            }
            // ※「画面：組合員等コード（開始）」のみ入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.KumiaiinToCdFrom) &&
                string.IsNullOrEmpty(SearchCondition.KumiaiinToCdTo))
            {
                query.Append($"  AND   T1.組合員等コード = @組合員等コードFrom ");
                queryParams.Add(new NpgsqlParameter("組合員等コードFrom", SearchCondition.KumiaiinToCdFrom));
            }
            // ※「画面：組合員等コード（終了）」のみ入力がある場合
            if (string.IsNullOrEmpty(SearchCondition.KumiaiinToCdFrom) &&
                !string.IsNullOrEmpty(SearchCondition.KumiaiinToCdTo))
            {
                query.Append($"  AND   T1.組合員等コード = @組合員等コードTo ");
                queryParams.Add(new NpgsqlParameter("組合員等コードTo", SearchCondition.KumiaiinToCdTo));
            }
            // ※「画面：組合員等コード（開始）」および「画面：組合員等コード（終了）」の入力がある場合
            if (!string.IsNullOrEmpty(SearchCondition.KumiaiinToCdFrom) &&
                !string.IsNullOrEmpty(SearchCondition.KumiaiinToCdTo))
            {
                query.Append($"  AND  (T1.組合員等コード >= @組合員等コードFrom ");
                query.Append($"  AND   T1.組合員等コード <= @組合員等コードTo) ");
                queryParams.Add(new NpgsqlParameter("組合員等コードFrom", SearchCondition.KumiaiinToCdFrom));
                queryParams.Add(new NpgsqlParameter("組合員等コードTo", SearchCondition.KumiaiinToCdTo));
            }
            #endregion

            #region ソート条件
            query.Append(" ORDER BY ");

            bool ruikbnSort = false;
            bool shisyoSort = false;
            bool daichikuSort = false;
            bool shochikuSort = false;
            bool kumiaiintoSort = false;
            bool isPutOrder = false;

            // ※「画面：表示順キー１」「画面：表示順キー２」「画面：表示順キー３」のいずれかが選択されている場合
            if (SearchCondition.DisplaySort1.HasValue || SearchCondition.DisplaySort2.HasValue || SearchCondition.DisplaySort3.HasValue)
            {

                //  画面指定ソート順
                if (SearchCondition.DisplaySort1.HasValue)
                {
                    isPutOrder = true;
                    switch (SearchCondition.DisplaySort1)
                    {
                        case D105190SearchCondition.DisplaySortType.Ruikbn:
                            query.Append($" T1.類区分 {SearchCondition.DisplaySortOrder1} ");
                            ruikbnSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" M1.shisho_cd {SearchCondition.DisplaySortOrder1} ");
                            shisyoSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" M1.daichiku_cd {SearchCondition.DisplaySortOrder1} ");
                            daichikuSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" M1.shochiku_cd {SearchCondition.DisplaySortOrder1} ");
                            shochikuSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.組合員等コード {SearchCondition.DisplaySortOrder1} ");
                            kumiaiintoSort = true;
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
                        case D105190SearchCondition.DisplaySortType.Ruikbn:
                            query.Append($" T1.類区分 {SearchCondition.DisplaySortOrder2} ");
                            ruikbnSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" M1.shisho_cd {SearchCondition.DisplaySortOrder2} ");
                            shisyoSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" M1.daichiku_cd {SearchCondition.DisplaySortOrder2} ");
                            daichikuSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" M1.shochiku_cd {SearchCondition.DisplaySortOrder2} ");
                            shochikuSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.組合員等コード {SearchCondition.DisplaySortOrder2} ");
                            kumiaiintoSort = true;
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
                    isPutOrder = true;
                    switch (SearchCondition.DisplaySort3)
                    {
                        case D105190SearchCondition.DisplaySortType.Ruikbn:
                            query.Append($" T1.類区分 {SearchCondition.DisplaySortOrder3} ");
                            ruikbnSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Shisyo:
                            query.Append($" M1.shisho_cd {SearchCondition.DisplaySortOrder3} ");
                            shisyoSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Daichiku:
                            query.Append($" M1.daichiku_cd {SearchCondition.DisplaySortOrder3} ");
                            daichikuSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.Shochiku:
                            query.Append($" M1.shochiku_cd {SearchCondition.DisplaySortOrder3} ");
                            shochikuSort = true;
                            break;
                        case D105190SearchCondition.DisplaySortType.KumiaiintoCd:
                            query.Append($" T1.組合員等コード {SearchCondition.DisplaySortOrder3} ");
                            kumiaiintoSort = true;
                            break;
                    }
                }
            }

            // 「画面：表示順キー」で類区分の指定がない場合
            if (!ruikbnSort)
            {
                if (isPutOrder)
                {
                    // ソート条件1or2or3が出力されていた場合、カンマを付与する
                    query.Append(", ");
                }
                query.Append(" T1.類区分 Asc ");
            }

            // 「画面：表示順キー」で支所の指定がない場合
            if (!shisyoSort)
            {
                query.Append(", ");
                query.Append(" M1.shisho_cd Asc ");
            }

            // 「画面：表示順キー」で大地区の指定がない場合
            if (!daichikuSort)
            {
                query.Append(", ");
                query.Append(" M1.daichiku_cd Asc ");
            }

            // 「画面：表示順キー」で小地区の指定がない場合
            if (!shochikuSort)
            {
                query.Append(", ");
                query.Append(" M1.shochiku_cd Asc ");
            }

            // 「画面：表示順キー」で組合員等コードの指定がない場合
            if (!kumiaiintoSort)
            {
                query.Append(", ");
                query.Append(" T1.組合員等コード Asc ");
            }
            #endregion

            records.AddRange(dbContext.Database.SqlQueryRaw<D105190ResultRecord>(query.ToString(), queryParams.ToArray()));

            return records;
        }


        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D105190SearchResult src)
        {
            this.DisplayCount = src.DisplayCount;
            this.AllRecCount = src.AllRecCount;
            this.DispRecords = src.DispRecords;
        }

        /// <summary>
        /// t_12080_組合員等別共済金額設定の対象レコードを削除する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="delRecords"></param>
        /// <returns></returns>
        public int DeleteKyousaiKingaku(ref NskAppContext dbContext, D105190SessionInfo sessionInfo, ref List<D105190ResultRecord> delRecords)
        {
            int delCount = 0;

            // t_12080_組合員等別共済金額設定の対象レコードを削除する。
            StringBuilder delKojinRyoritsu = new();
            delKojinRyoritsu.Append("DELETE FROM t_12080_組合員等別共済金額設定 ");
            delKojinRyoritsu.Append("WHERE ");
            delKojinRyoritsu.Append("     組合等コード       = @組合等コード ");
            delKojinRyoritsu.Append(" AND 年産               = @年産 ");
            delKojinRyoritsu.Append(" AND 共済目的コード     = @共済目的コード ");
            delKojinRyoritsu.Append(" AND 組合員等コード     = @組合員等コード ");
            delKojinRyoritsu.Append(" AND 類区分             = @類区分 ");
            delKojinRyoritsu.Append(" AND xmin               = @xmin ");

            foreach (D105190ResultRecord target in delRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                List<NpgsqlParameter> delParams =
                [
                    new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                    new NpgsqlParameter("年産", sessionInfo.Nensan),
                    new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new NpgsqlParameter("組合員等コード", target.KumiaiintoCd),
                    new NpgsqlParameter("類区分", SearchCondition.RuiKbn)
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                delParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(delKojinRyoritsu.ToString(), delParams);
                if (cnt == 0)
                {
                    string message = "ME10083";
                    throw new DBConcurrencyException(message);
                }
                delCount += cnt;
            }

            return delCount;
        }

        /// <summary>
        /// t_12080_組合員等別共済金額設定の対象レコードを更新する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="updRecords"></param>
        /// <returns></returns>
        public int UpdateKyousaiKingaku(ref NskAppContext dbContext, D105190SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105190ResultRecord> updRecords)
        {
            int updCount = 0;

            // t_12080_組合員等別共済金額設定の対象レコードを更新する。
            StringBuilder updKojinRyoritsu = new();
            updKojinRyoritsu.Append("UPDATE t_12080_組合員等別共済金額設定 SET ");
            updKojinRyoritsu.Append("  共済金額     = @共済金額 ");
            updKojinRyoritsu.Append(" ,更新日時     = @更新日時 ");
            updKojinRyoritsu.Append(" ,更新ユーザid = @更新ユーザid ");
            updKojinRyoritsu.Append("WHERE ");
            updKojinRyoritsu.Append("     組合等コード       = @組合等コード ");
            updKojinRyoritsu.Append(" AND 年産               = @年産 ");
            updKojinRyoritsu.Append(" AND 共済目的コード     = @共済目的コード ");
            updKojinRyoritsu.Append(" AND 組合員等コード     = @組合員等コード ");
            updKojinRyoritsu.Append(" AND 類区分             = @類区分 ");
            updKojinRyoritsu.Append(" AND xmin               = @xmin ");

            foreach (D105190ResultRecord target in updRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                List<NpgsqlParameter> updParams =
                [
                    new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                    new NpgsqlParameter("年産", sessionInfo.Nensan),
                    new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new NpgsqlParameter("組合員等コード", target.KumiaiintoCd),
                    new NpgsqlParameter("類区分", SearchCondition.RuiKbn),
                    new NpgsqlParameter("共済金額", target.KyousaiKingaku),
                    new NpgsqlParameter("更新日時", sysDateTime),
                    new NpgsqlParameter("更新ユーザid", userId)
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                updParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(updKojinRyoritsu.ToString(), updParams);
                if (cnt == 0)
                {
                    string message = "ME10082";
                    throw new DBConcurrencyException(message);
                }
                updCount += cnt;
            }

            return updCount;
        }

        /// <summary>
        /// t_12080_組合員等別共済金額設定の対象レコードを登録する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="KyousaiKingakuAddRecords"></param>
        /// <returns></returns>
        public int AppendKyousaiKingaku(ref NskAppContext dbContext, D105190SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105190ResultRecord> KyousaiKingakuAddRecords)
        {
            List<T12080組合員等別共済金額設定> addKojinRyoritsuRecs = new();
            foreach (D105190ResultRecord target in KyousaiKingakuAddRecords)
            {
                addKojinRyoritsuRecs.Add(new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    組合員等コード = target.KumiaiintoCd,
                    類区分 = SearchCondition.RuiKbn,
                    共済金額 = target.KyousaiKingaku,
                    登録日時 = sysDateTime,
                    登録ユーザid = userId,
                    更新日時 = sysDateTime,
                    更新ユーザid = userId
                });
            }
            dbContext.T12080組合員等別共済金額設定s.AddRange(addKojinRyoritsuRecs);

            return dbContext.SaveChanges();
        }

        /// <summary>
        /// 更新対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public override List<D105190ResultRecord> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo session)
        {
            D105190SessionInfo sessionInfo = (D105190SessionInfo)session;

            List<D105190ResultRecord> updRecs = new();

            // 検索結果取得
            List<D105190ResultRecord> dbResults = GetResult(dbContext, sessionInfo);

            // 検索結果と画面入力値を比較
            foreach (D105190ResultRecord dispRec in DispRecords)
            {
                // 追加行、削除行以外を対象とする
                if (dispRec is BasePagerRecord pagerRec && !pagerRec.IsNewRec && !pagerRec.IsDelRec)
                {
                    D105190ResultRecord dbRec = dbResults.SingleOrDefault(x =>
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