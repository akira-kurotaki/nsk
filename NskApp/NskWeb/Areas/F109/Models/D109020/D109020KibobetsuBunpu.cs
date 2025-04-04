using NskAppModelLibrary.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using NskAppModelLibrary.Models;
using System.Data;
using CoreLibrary.Core.Utility;

namespace NskWeb.Areas.F109.Models.D109020
{
    /// <summary>
    /// 規模別分布状況データ
    /// </summary>
    public class D109020KibobetsuBunpu
    {
        /// <summary>
        /// メッセージエリア2
        /// </summary>
        public string MessageArea2 { get; set; } = string.Empty;

        /// <summary>表示用検索結果</summary>
        public List<D109020KibobetsuBunpuRecord> DispRecords { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D109020KibobetsuBunpu()
        {
        }


        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="searchCondition"></param>
        public void Search(NskAppContext dbContext, D109020SessionInfo sessionInfo, D109020SearchCondition searchCondition)
        {
            // ◆選択した支所の規模別面積区分情報を取得する。
            StringBuilder query = new();
            query.Append("SELECT ");
            query.Append($"  規模別面積区分 As \"{nameof(D109020KibobetsuBunpuRecord.MensekiKbn)}\" ");
            query.Append($" ,対象面積上限 As \"{nameof(D109020KibobetsuBunpuRecord.MensekiJogen)}\" ");
            query.Append($" ,cast('' || xmin as integer) As \"{nameof(D109020KibobetsuBunpuRecord.Xmin)}\" ");
            query.Append("FROM ");
            query.Append(" t_14070_規模別面積区分情報 ");
            query.Append("WHERE ");
            query.Append("     組合等コード   = @組合等コード ");
            query.Append(" AND 年産           = @年産 ");
            query.Append(" AND 共済目的コード = @共済目的コード");
            query.Append(" AND 支所コード     = @支所コード ");
            query.Append("ORDER BY ");
            query.Append("     CAST(規模別面積区分 AS integer) ");
            NpgsqlParameter[] queryParams =
            [
                new("組合等コード", sessionInfo.KumiaitoCd),
                new("年産", sessionInfo.Nensan),
                new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new("支所コード", searchCondition.ShishoCd),
            ];

            DispRecords.AddRange(dbContext.Database.SqlQueryRaw<D109020KibobetsuBunpuRecord>(query.ToString(), queryParams));
        }

        /// <summary>
        /// 登録済みの対象データを削除する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="shishoCd"></param>
        public void Delete(ref NskAppContext dbContext, D109020SessionInfo sessionInfo, string shishoCd, ref string errMessage)
        {
            // (1) 登録済みの対象データを削除する。
            StringBuilder delQuery = new();
            delQuery.Append("DELETE FROM t_14070_規模別面積区分情報 ");
            delQuery.Append("WHERE ");
            delQuery.Append("     組合等コード   = @組合等コード ");
            delQuery.Append(" AND 年産           = @年産 ");
            delQuery.Append(" AND 共済目的コード = @共済目的コード ");

            List<NpgsqlParameter> delParams =
            [
                new("組合等コード", sessionInfo.KumiaitoCd),
                new("年産", sessionInfo.Nensan),
                new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
            ];
            int cnt = dbContext.Database.ExecuteSqlRaw(delQuery.ToString(), delParams);
            if (cnt == 0)
            {
                errMessage = MessageUtil.Get("ME10083");
                throw new DBConcurrencyException();
            }
        }

        /// <summary>
        /// 登録処理s
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="shishoCd"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="errMessage"></param>
        public void Insert(ref NskAppContext dbContext, D109020SessionInfo sessionInfo, string shishoCd, DateTime sysDateTime, ref string errMessage)
        {
            List<T14070規模別面積区分情報> addRecs = new();
            for (int i = 0; i < 21; i++)
            {
                T14070規模別面積区分情報 addRec = new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    支所コード = shishoCd,
                    登録日時 = sysDateTime,
                    登録ユーザid = sessionInfo.UserId,
                    更新日時 = sysDateTime,
                    更新ユーザid = sessionInfo.UserId,
                    規模別面積区分 = $"{(i + 1)}",
                    対象面積上限 = string.Empty
                };

                D109020KibobetsuBunpuRecord? dispRec = DispRecords.SingleOrDefault(x => x.MensekiKbn == $"{(i + 1)}");
                if (dispRec != null)
                {
                    // 規模別面積区分が一致する画面入力がある場合、
                    // 画面入力の対象面積上限をセットする
                    addRec.対象面積上限 = dispRec.MensekiJogen;
                }

                addRecs.Add(addRec);
            }

            dbContext.T14070規模別面積区分情報s.AddRange(addRecs);
            dbContext.SaveChanges();
        }
    }
}