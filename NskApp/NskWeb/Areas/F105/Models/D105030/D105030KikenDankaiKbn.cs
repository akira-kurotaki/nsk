﻿using NskAppModelLibrary.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using NskAppModelLibrary.Models;
using System.Data;
using NskWeb.Common.Models;
using NskCommonLibrary.Core.Consts;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// 危険段階
    /// </summary>
    public class D105030KikenDankaiKbn : BasePager<D105030KikenDankaiKbnRecord>
    {
        /// <summary>危険段階区分ドロップダウンリスト選択値</summary>
        public List<SelectListItem> KikenDankaiKbnLists { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105030KikenDankaiKbn()
        {
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D105030SessionInfo sessionInfo)
        {
            // ２．１７．[危険段階区分] ドロップダウンリスト項目を取得する。		
            // (1) m_10230_危険段階テーブルより、危険段階区分を取得する。
            // (2) 取得した結果をドロップダウンリストの項目として設定する。
            KikenDankaiKbnLists = new();
            KikenDankaiKbnLists.AddRange(dbContext.M10230危険段階s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.合併時識別コード == (sessionInfo.GappeiShikibetsuCd ?? string.Empty)))?.
                OrderBy(m => m.危険段階区分).
                Select(m => new SelectListItem($"{m.危険段階区分}", $"{m.危険段階区分}")));
        }


        /// <summary>
        /// 危険段階区分を取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="session">セッション情報</param>
        /// <returns>検索情報</returns>
        public override List<D105030KikenDankaiKbnRecord> GetResult(NskAppContext dbContext, BaseSessionInfo session)
        {
            D105030SessionInfo sessionInfo = (D105030SessionInfo)session;

            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"   T1.統計単位地域コード   As \"{nameof(D105030KikenDankaiKbnRecord.TokeiTaniChiikiCd)}\" ");
            query.Append($"  ,T2.統計単位地域名称     As \"{nameof(D105030KikenDankaiKbnRecord.TokeiTaniChiikiNm)}\" ");
            query.Append($"  ,T1.個人危険段階区分     As \"{nameof(D105030KikenDankaiKbnRecord.KikenDankaiKbn)}\" ");
            query.Append($"  ,cast('' || T1.xmin as integer) As \"{nameof(D105030KikenDankaiKbnRecord.Xmin)}\" ");
            query.Append($" FROM t_11040_個人料率 T1 ");
            query.Append(" LEFT OUTER JOIN m_00170_統計単位地域 T2 ");
            query.Append(" ON    T1.組合等コード       = T2.組合等コード ");
            query.Append("  AND  T1.年産               = T2.年産 ");
            query.Append("  AND  T1.共済目的コード     = T2.共済目的コード ");
            query.Append("  AND  T1.統計単位地域コード = T2.統計単位地域コード ");
            query.Append(" WHERE ");
            query.Append("       T1.組合等コード   = @組合等コード ");
            query.Append("  AND  T1.年産           = @年産 ");
            query.Append("  AND  T1.共済目的コード = @共済目的コード ");
            query.Append("  AND  T1.組合員等コード = @組合員等コード ");
            query.Append("  AND  T1.類区分         = @類区分 ");

            string ruiKbn =
                sessionInfo.KyosaiMokutekiCd == $"{(int)CoreConst.KyosaiMokutekiCdNumber.Suitou}" ? "7" :
                sessionInfo.KyosaiMokutekiCd == $"{(int)CoreConst.KyosaiMokutekiCdNumber.Rikutou}" ? "1" :
                string.Empty;

            NpgsqlParameter[] queryParams =
            [
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new ("組合員等コード", sessionInfo.KumiaiintoCd),
                new ("類区分", ruiKbn),
            ];

            List<D105030KikenDankaiKbnRecord> records = new();
            records.AddRange(dbContext.Database.SqlQueryRaw<D105030KikenDankaiKbnRecord>(query.ToString(), queryParams));

            return records;
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src">コピー元</param>
        public void ApplyInput(D105030KikenDankaiKbn src)
        {
            this.DispRecords = src.DispRecords;
        }

        /// <summary>
        /// t_11040_個人料率の対象レコードを削除する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="delRecords"></param>
        /// <returns></returns>
        public int DeleteKikenDankaiKbn(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, ref List<D105030KikenDankaiKbnRecord> delRecords)
        {
            int delCount = 0;

            // t_11040_個人料率の対象レコードを削除する。
            StringBuilder delKojinRyoritsu = new();
            delKojinRyoritsu.Append("DELETE FROM t_11040_個人料率 ");
            delKojinRyoritsu.Append("WHERE ");
            delKojinRyoritsu.Append("     組合等コード       = @組合等コード ");
            delKojinRyoritsu.Append(" AND 年産               = @年産 ");
            delKojinRyoritsu.Append(" AND 共済目的コード     = @共済目的コード ");
            delKojinRyoritsu.Append(" AND 組合員等コード     = @組合員等コード ");
            delKojinRyoritsu.Append(" AND 類区分             = @類区分 ");
            delKojinRyoritsu.Append(" AND 統計単位地域コード = @統計単位地域コード ");
            delKojinRyoritsu.Append(" AND xmin               = @xmin ");

            foreach (D105030KikenDankaiKbnRecord target in delRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }
                // 類区分
                string ruiKbn = GetRuiKbn(sessionInfo.KyosaiMokutekiCd);

                List<NpgsqlParameter> delParams =
                [
                    new ("組合等コード", sessionInfo.KumiaitoCd),
                    new ("年産", sessionInfo.Nensan),
                    new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new ("組合員等コード", sessionInfo.KumiaiintoCd),
                    new ("類区分", ruiKbn),
                    new ("統計単位地域コード", target.TokeiTaniChiikiCd),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                delParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(delKojinRyoritsu.ToString(), delParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                delCount += cnt;
            }

            return delCount;
        }

        /// <summary>
        /// t_11040_個人料率の対象レコードを更新する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="updRecords"></param>
        /// <returns></returns>
        public int UpdateKikenDankaiKbn(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105030KikenDankaiKbnRecord> updRecords)
        {
            int updCount = 0;

            // t_11040_個人料率の対象レコードを更新する。
            StringBuilder updKojinRyoritsu = new();
            updKojinRyoritsu.Append("UPDATE t_11040_個人料率 SET ");
            updKojinRyoritsu.Append("  個人危険段階区分    = @個人危険段階区分 ");
            updKojinRyoritsu.Append("WHERE ");
            updKojinRyoritsu.Append("     組合等コード       = @組合等コード ");
            updKojinRyoritsu.Append(" AND 年産               = @年産 ");
            updKojinRyoritsu.Append(" AND 共済目的コード     = @共済目的コード ");
            updKojinRyoritsu.Append(" AND 組合員等コード     = @組合員等コード ");
            updKojinRyoritsu.Append(" AND 類区分             = @類区分 ");
            updKojinRyoritsu.Append(" AND 統計単位地域コード = @統計単位地域コード ");
            updKojinRyoritsu.Append(" AND xmin               = @xmin ");

            foreach (D105030KikenDankaiKbnRecord target in updRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }
                // 類区分
                string ruiKbn = GetRuiKbn(sessionInfo.KyosaiMokutekiCd);

                List<NpgsqlParameter> updParams =
                [
                    new ("組合等コード", sessionInfo.KumiaitoCd),
                    new ("年産", sessionInfo.Nensan),
                    new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new ("組合員等コード", sessionInfo.KumiaiintoCd),
                    new ("類区分", ruiKbn),
                    new ("統計単位地域コード", target.TokeiTaniChiikiCd),
                    new ("個人危険段階区分", target.KikenDankaiKbn)
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                updParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(updKojinRyoritsu.ToString(), updParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                updCount += cnt;
            }

            return updCount;
        }

        /// <summary>
        /// t_11040_個人料率の対象レコードを登録する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="kikenDankaiKbnAddRecords"></param>
        /// <returns></returns>
        public int AppendKikenDankaiKbn(ref NskAppContext dbContext, D105030SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105030KikenDankaiKbnRecord> kikenDankaiKbnAddRecords)
        {
            List<T11040個人料率> addKojinRyoritsuRecs = new();
            foreach (D105030KikenDankaiKbnRecord target in kikenDankaiKbnAddRecords)
            {
                // 類区分
                string ruiKbn = GetRuiKbn(sessionInfo.KyosaiMokutekiCd);

                addKojinRyoritsuRecs.Add(new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    組合員等コード = sessionInfo.KumiaiintoCd,
                    類区分 = ruiKbn,
                    統計単位地域コード = target.TokeiTaniChiikiCd,
                    個人危険段階区分 = target.KikenDankaiKbn,
                    登録日時 = sysDateTime,
                    登録ユーザid = userId,
                    更新日時 = sysDateTime,
                    更新ユーザid = userId
                });
            }
            dbContext.T11040個人料率s.AddRange(addKojinRyoritsuRecs);

            return dbContext.SaveChanges();
        }

        /// <summary>
        /// 共済目的コードに応じた類区分を取得する。
        /// </summary>
        /// <param name="kyosaiMokutekiCd">共済目的コード</param>
        /// <returns></returns>
        private string GetRuiKbn(string kyosaiMokutekiCd)
        {
            string ruiKbn =
                kyosaiMokutekiCd == $"{(int)CoreConst.KyosaiMokutekiCdNumber.Suitou}" ? "7" :
                kyosaiMokutekiCd == $"{(int)CoreConst.KyosaiMokutekiCdNumber.Rikutou}" ? "1" :
                string.Empty;
            return ruiKbn;

        }

        /// <summary>
        /// 更新対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public override List<D105030KikenDankaiKbnRecord> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo sessionInfo)
        {
            List<D105030KikenDankaiKbnRecord> updRecs = new();

            // 検索結果取得
            List<D105030KikenDankaiKbnRecord> dbResults = GetResult(dbContext, sessionInfo);

            // 検索結果と画面入力値を比較
            foreach (D105030KikenDankaiKbnRecord dispRec in DispRecords)
            {
                // 追加行、削除行以外を対象とする
                if (dispRec is BasePagerRecord pagerRec && !pagerRec.IsNewRec && !pagerRec.IsDelRec)
                {
                    D105030KikenDankaiKbnRecord dbRec = dbResults.SingleOrDefault(x =>
                        (x.TokeiTaniChiikiCd == dispRec.TokeiTaniChiikiCd)
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