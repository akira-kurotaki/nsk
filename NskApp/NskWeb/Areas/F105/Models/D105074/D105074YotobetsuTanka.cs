using NskAppModelLibrary.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using NskAppModelLibrary.Models;
using System.Data;
using static NskWeb.Areas.F105.Consts.F105Const;
using NskWeb.Areas.F105.Consts;
using NskWeb.Common.Models;

namespace NskWeb.Areas.F105.Models.D105074
{
    /// <summary>
    /// 危険段階
    /// </summary>
    public class D105074YotobetsuTanka : BasePager<D105074YotobetsuTankaRecord>
    {

        /// <summary>
        /// 作付時期リスト
        /// </summary>
        public List<SelectListItem> SakutukeJikiLists { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105074YotobetsuTanka()
        {
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D105074SessionInfo sessionInfo)
        {

            // ２．３．[作付時期] ドロップダウンリスト項目を取得する。		
            // (1) m_10120_用途区分選択、m_00140_名称テーブルより、作付時期、正式名称を取得する。
            // (2) 取得した結果をドロップダウンリストの項目として設定する。
            StringBuilder sakutukejikiQuery = new();
            sakutukejikiQuery.Append("SELECT ");
            sakutukejikiQuery.Append($"  T1.作付時期 As \"{nameof(SakutukeJikiRec.SakutukeJiki)}\"");
            sakutukejikiQuery.Append($" ,T2.正式名称 As \"{nameof(SakutukeJikiRec.SeishikiNm)}\"");
            sakutukejikiQuery.Append("FROM m_10120_用途区分選択 T1 ");
            sakutukejikiQuery.Append("LEFT OUTER JOIN m_00140_名称 T2 ");
            sakutukejikiQuery.Append(" ON  T1.作付時期           = T2.名称コード ");
            sakutukejikiQuery.Append(" AND T2.名称グループコード = @名称グループコード ");
            sakutukejikiQuery.Append("WHERE ");
            sakutukejikiQuery.Append("     T1.共済目的コード     = @共済目的コード ");
            sakutukejikiQuery.Append(" AND T1.類区分             = @類区分 ");
            sakutukejikiQuery.Append("GROUP BY ");
            sakutukejikiQuery.Append("  T1.作付時期 ");
            sakutukejikiQuery.Append(" ,T2.正式名称 ");
            List<NpgsqlParameter> sakutukejikiParams =
            [
                new("名称グループコード", F105Const.MEISHO_GRP_SAKUTUKEJIKI_MUGI),
                new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new("類区分", sessionInfo.RuiKbn),
            ];
            List<SakutukeJikiRec> sakutukejikiRecords = new();
            sakutukejikiRecords.AddRange(dbContext.Database.SqlQueryRaw<SakutukeJikiRec>(sakutukejikiQuery.ToString(), sakutukejikiParams.ToArray()));
            SakutukeJikiLists = new();
            SakutukeJikiLists.AddRange(sakutukejikiRecords.Select(x => new SelectListItem($"{x.SakutukeJiki} {x.SeishikiNm}", x.SakutukeJiki)));


            // ２．４．[用途区分] ドロップダウンリスト項目を取得する。		
            // (1) m_10120_用途区分選択、m_00140_名称テーブルより、用途区分、正式名称を取得する。
            // (2) 取得した結果をドロップダウンリストの項目として設定する。
            UpdateYotoKbn(dbContext, sessionInfo);

            // ２．５．[適用単価] ドロップダウンリスト項目を取得する。		
            // (1) m_10210_単当共済金額用途テーブルより、単当共済金額順位を取得する。
            // (2) 取得した結果をドロップダウンリストの項目として設定する。
            UpdateTekiyoTanka(dbContext, sessionInfo);

        }

        /// <summary>
        /// [用途区分] ドロップダウンリスト項目を取得する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        public void UpdateYotoKbn(NskAppContext dbContext, D105074SessionInfo sessionInfo)
        {
            StringBuilder yotoKbnQuery = new();
            yotoKbnQuery.Append("SELECT ");
            yotoKbnQuery.Append($"  T1.用途区分 As \"{nameof(YotoKbnRec.YotoKbn)}\"");
            yotoKbnQuery.Append($" ,T2.用途名称 As \"{nameof(YotoKbnRec.YotoNm)}\"");
            yotoKbnQuery.Append("FROM m_10120_用途区分選択 T1 ");
            yotoKbnQuery.Append("LEFT OUTER JOIN m_10110_用途区分名称 T2 ");
            yotoKbnQuery.Append(" ON  T1.用途区分           = T2.用途区分 ");
            yotoKbnQuery.Append(" AND T2.共済目的コード     = @共済目的コード ");
            yotoKbnQuery.Append("WHERE ");
            yotoKbnQuery.Append("     T1.共済目的コード     = @共済目的コード ");
            yotoKbnQuery.Append(" AND T1.類区分             = @類区分 ");
            yotoKbnQuery.Append(" AND T1.作付時期           = @作付時期 ");
            yotoKbnQuery.Append("GROUP BY ");
            yotoKbnQuery.Append("  T1.用途区分 ");
            yotoKbnQuery.Append(" ,T2.用途名称 ");

            // [２．２．]で取得した件数分、以下を繰り返す。
            foreach (D105074YotobetsuTankaRecord rec in DispRecords)
            {
                // ２．４．[用途区分] ドロップダウンリスト項目を取得する。		
                // (1) m_10120_用途区分選択、m_00140_名称テーブルより、用途区分、正式名称を取得する。
                // (2) 取得した結果をドロップダウンリストの項目として設定する。
                List<NpgsqlParameter> yotoKbnParams =
                [
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("類区分", sessionInfo.RuiKbn),
                    new("作付時期", rec.SakutsukeJiki ?? string.Empty),
                ];
                List<YotoKbnRec> yotoKbnRecords = new();
                yotoKbnRecords.AddRange(dbContext.Database.SqlQueryRaw<YotoKbnRec>(yotoKbnQuery.ToString(), yotoKbnParams.ToArray()));
                rec.YotoKbnLists = new();
                rec.YotoKbnLists.AddRange(yotoKbnRecords.Select(x => new SelectListItem($"{x.YotoKbn} {x.YotoNm}", x.YotoKbn)));
            }
        }

        /// <summary>
        /// [適用単価] ドロップダウンリスト項目を取得する。	
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        public void UpdateTekiyoTanka(NskAppContext dbContext, D105074SessionInfo sessionInfo)
        {
            StringBuilder tekiyoTankaQuery = new();
            tekiyoTankaQuery.Append("SELECT ");
            tekiyoTankaQuery.Append($"  T1.単当共済金額順位 As \"{nameof(TekiyoTankaRec.TekiyoTanka)}\"");
            tekiyoTankaQuery.Append("FROM m_10210_単当共済金額用途  T1 ");
            tekiyoTankaQuery.Append("LEFT JOIN m_00020_類名称       T2 ");
            tekiyoTankaQuery.Append(" ON   T2.共済目的コード =  @共済目的コード ");
            tekiyoTankaQuery.Append(" AND  T2.類区分         =  @類区分 ");
            tekiyoTankaQuery.Append(" AND  T1.種類区分       =  T2.種類区分 ");
            tekiyoTankaQuery.Append("WHERE ");
            tekiyoTankaQuery.Append("      T1.組合等コード   =  @組合等コード ");
            tekiyoTankaQuery.Append(" AND  T1.年産           =  @年産 ");
            tekiyoTankaQuery.Append(" AND  T1.共済目的コード =  @共済目的コード ");
            tekiyoTankaQuery.Append(" AND  T1.作付時期       =  @作付時期 ");
            tekiyoTankaQuery.Append(" AND  T1.用途区分       =  @用途区分 ");
            if (sessionInfo.NinaiteKbn == $"{(int)NinaiteNoukaKbn.Kazei}")
            {
                // ①[セッション：担手農家区分] = １：課税 の場合
                tekiyoTankaQuery.Append(" AND (T1.課税単価区分 = 0 OR  T1.課税単価区分 = 1) ");
            }
            else if (sessionInfo.NinaiteKbn == $"{(int)NinaiteNoukaKbn.Menzei}")
            {
                // ②[セッション：担手農家区分] = ２：免税 の場合
                tekiyoTankaQuery.Append(" AND (T1.課税単価区分 = 0 OR  T1.課税単価区分 = 2) ");
            }
            else if (sessionInfo.NinaiteKbn == $"{(int)NinaiteNoukaKbn.Other}")
            {
                // ③[セッション：担手農家区分] = ３：以外 の場合
                tekiyoTankaQuery.Append(" AND (T1.課税単価区分 = 0 OR (T1.課税単価区分 = 1 AND T1.含数量払フラグ = 0)) ");
            }

            // [２．２．]で取得した件数分、以下を繰り返す。
            foreach (D105074YotobetsuTankaRecord rec in DispRecords)
            {
                // ２．５．[適用単価] ドロップダウンリスト項目を取得する。		
                // (1) m_10210_単当共済金額用途テーブルより、単当共済金額順位を取得する。
                // (2) 取得した結果をドロップダウンリストの項目として設定する。
                List<NpgsqlParameter> tekiyoTankaParams =
                [
                    new("組合等コード", sessionInfo.KumiaitoCd),
                    new("年産", sessionInfo.Nensan),
                    new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new("類区分", sessionInfo.RuiKbn),
                    new("作付時期", rec.SakutsukeJiki ?? string.Empty),
                    new("用途区分", rec.YotoKbn ?? string.Empty),
                ];
                List<TekiyoTankaRec> tekiyoTankaRecords = new();
                tekiyoTankaRecords.AddRange(dbContext.Database.SqlQueryRaw<TekiyoTankaRec>(tekiyoTankaQuery.ToString(), tekiyoTankaParams.ToArray()));
                rec.TekiyoTankaLists = new();
                rec.TekiyoTankaLists.AddRange(tekiyoTankaRecords.Select(x => new SelectListItem($"{x.TekiyoTanka}", $"{x.TekiyoTanka}")));
            }
        }

        /// <summary>
        /// [作付時期] ドロップダウンリスト項目
        /// </summary>
        private class SakutukeJikiRec
        {
            /// <summary>作付時期</summary>
            public string SakutukeJiki { get; set; } = string.Empty;
            /// <summary>正式名称</summary>
            public string SeishikiNm { get; set; } = string.Empty;
        }

        /// <summary>
        /// [用途区分] ドロップダウンリスト項目
        /// </summary>
        private class YotoKbnRec
        {
            public string YotoKbn { get; set; } = string.Empty;
            public string YotoNm { get; set; } = string.Empty;
        }

        /// <summary>
        /// [適用単価] ドロップダウンリスト項目
        /// </summary>
        private class TekiyoTankaRec
        {
            public decimal? TekiyoTanka { get; set; }
        }

        /// <summary>
        /// 用途別単価設定を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="session">セッション情報</param>
        /// <returns>検索情報</returns>
        public override List<D105074YotobetsuTankaRecord> GetResult(NskAppContext dbContext, BaseSessionInfo session)
        {
            D105074SessionInfo sessionInfo = (D105074SessionInfo)session;

            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"   作付時期                    As \"{nameof(D105074YotobetsuTankaRecord.SakutsukeJiki)}\" ");
            query.Append($"  ,用途区分                    As \"{nameof(D105074YotobetsuTankaRecord.YotoKbn)}\" ");
            query.Append($"  ,共済金額選択順位            As \"{nameof(D105074YotobetsuTankaRecord.TekiyoTanka)}\" ");
            query.Append($"  ,cast('' || xmin as integer) As \"{nameof(D105074YotobetsuTankaRecord.Xmin)}\" ");
            query.Append($" FROM t_11050_個人用途別 ");
            query.Append(" WHERE ");
            query.Append("       組合等コード   = @組合等コード ");
            query.Append("  AND  年産           = @年産 ");
            query.Append("  AND  共済目的コード = @共済目的コード ");
            query.Append("  AND  組合員等コード = @組合員等コード ");
            query.Append("  AND  類区分         = @類区分 ");

            NpgsqlParameter[] queryParams =
            [
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new ("組合員等コード", sessionInfo.KumiaiintoCd),
                new ("類区分", sessionInfo.RuiKbn),
            ];

            List<D105074YotobetsuTankaRecord> records = new();
            records.AddRange(dbContext.Database.SqlQueryRaw<D105074YotobetsuTankaRecord>(query.ToString(), queryParams));

            return records;
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src">コピー元</param>
        public void ApplyInput(D105074YotobetsuTanka src)
        {
            this.DispRecords = src.DispRecords;
        }

        /// <summary>
        /// t_11050_個人用途別の対象レコードを削除する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="delRecords"></param>
        /// <returns></returns>
        public int DeleteYotobetsuTanka(ref NskAppContext dbContext, D105074SessionInfo sessionInfo, ref List<D105074YotobetsuTankaRecord> delRecords)
        {
            int delCount = 0;

            // t_11050_個人用途別の対象レコードを削除する。
            StringBuilder delKojinYotobetsu = new();
            delKojinYotobetsu.Append("DELETE FROM t_11050_個人用途別 ");
            delKojinYotobetsu.Append("WHERE ");
            delKojinYotobetsu.Append("     組合等コード   = @組合等コード ");
            delKojinYotobetsu.Append(" AND 年産           = @年産 ");
            delKojinYotobetsu.Append(" AND 共済目的コード = @共済目的コード ");
            delKojinYotobetsu.Append(" AND 組合員等コード = @組合員等コード ");
            delKojinYotobetsu.Append(" AND 作付時期       = @作付時期 ");
            delKojinYotobetsu.Append(" AND 用途区分       = @用途区分 ");
            delKojinYotobetsu.Append(" AND xmin           = @xmin ");

            foreach (D105074YotobetsuTankaRecord target in delRecords)
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
                    new ("組合員等コード", sessionInfo.KumiaiintoCd),
                    new ("作付時期", target.SakutsukeJiki),
                    new ("用途区分", target.YotoKbn),
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                delParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(delKojinYotobetsu.ToString(), delParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                delCount += cnt;
            }

            return delCount;
        }

        /// <summary>
        ///t_11050_個人用途別の対象レコードを更新する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="updRecords"></param>
        /// <returns></returns>
        public int UpdateYotobetsuTanka(ref NskAppContext dbContext, D105074SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105074YotobetsuTankaRecord> updRecords)
        {
            int updCount = 0;

            // t_11050_個人用途別の対象レコードを更新する。
            StringBuilder updKojinYotobetsu = new();
            updKojinYotobetsu.Append("UPDATE t_11050_個人用途別 SET ");
            updKojinYotobetsu.Append("  共済金額選択順位  = @共済金額選択順位 ");
            updKojinYotobetsu.Append("WHERE ");
            updKojinYotobetsu.Append("     組合等コード   = @組合等コード ");
            updKojinYotobetsu.Append(" AND 年産           = @年産 ");
            updKojinYotobetsu.Append(" AND 共済目的コード = @共済目的コード ");
            updKojinYotobetsu.Append(" AND 組合員等コード = @組合員等コード ");
            updKojinYotobetsu.Append(" AND 作付時期       = @作付時期 ");
            updKojinYotobetsu.Append(" AND 用途区分       = @用途区分 ");
            updKojinYotobetsu.Append(" AND xmin           = @xmin ");

            foreach (D105074YotobetsuTankaRecord target in updRecords)
            {
                if (!target.Xmin.HasValue)
                {
                    // xmin nullは処理対象外
                    continue;
                }

                List<NpgsqlParameter> updParams =
                [
                    new ("共済金額選択順位", target.TekiyoTanka),
                    new ("組合等コード", sessionInfo.KumiaitoCd),
                    new ("年産", sessionInfo.Nensan),
                    new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                    new ("組合員等コード", sessionInfo.KumiaiintoCd),
                    new ("作付時期", target.SakutsukeJiki),
                    new ("用途区分", target.YotoKbn)
                ];
                NpgsqlParameter xminParam = new("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.Xmin };
                updParams.Add(xminParam);
                int cnt = dbContext.Database.ExecuteSqlRaw(updKojinYotobetsu.ToString(), updParams);
                if (cnt == 0)
                {
                    throw new DBConcurrencyException();
                }
                updCount += cnt;
            }

            return updCount;
        }

        /// <summary>
        /// t_11050_個人用途別の対象レコードを登録する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <param name="userId"></param>
        /// <param name="sysDateTime"></param>
        /// <param name="YotobetsuTankaAddRecords"></param>
        /// <returns></returns>
        public int AppendYotobetsuTanka(ref NskAppContext dbContext, D105074SessionInfo sessionInfo, string userId, DateTime sysDateTime, ref List<D105074YotobetsuTankaRecord> YotobetsuTankaAddRecords)
        {
            List<T11050個人用途別> addYotobetsuTankaRecs = new();

            foreach (D105074YotobetsuTankaRecord target in YotobetsuTankaAddRecords)
            {
                M00020類名称? ruiNm = dbContext.M00020類名称s.SingleOrDefault(x =>
                    (x.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                    (x.類区分 == sessionInfo.RuiKbn));

                addYotobetsuTankaRecs.Add(new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    年産 = (short)sessionInfo.Nensan,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    組合員等コード = sessionInfo.KumiaiintoCd,
                    類区分 = sessionInfo.RuiKbn,
                    種類区分 = ruiNm?.種類区分,
                    作付時期 = target.SakutsukeJiki,
                    用途区分 = target.YotoKbn,
                    共済金額選択順位 = target.TekiyoTanka,
                    登録日時 = sysDateTime,
                    登録ユーザid = userId,
                    更新日時 = sysDateTime,
                    更新ユーザid = userId
                });
            }
            dbContext.T11050個人用途別s.AddRange(addYotobetsuTankaRecs);

            return dbContext.SaveChanges();
        }

        /// <summary>
        /// 更新対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public override List<D105074YotobetsuTankaRecord> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo session)
        {
            D105074SessionInfo sessionInfo = (D105074SessionInfo)session;

            List<D105074YotobetsuTankaRecord> updRecs = new();

            // 検索結果取得
            List<D105074YotobetsuTankaRecord> dbResults = GetResult(dbContext, sessionInfo);

            // 検索結果と画面入力値を比較
            foreach (D105074YotobetsuTankaRecord dispRec in DispRecords)
            {
                // 追加行、削除行以外を対象とする
                if (dispRec is BasePagerRecord pagerRec && !pagerRec.IsNewRec && !pagerRec.IsDelRec)
                {
                    D105074YotobetsuTankaRecord dbRec = dbResults.SingleOrDefault(x =>
                        (x.SakutsukeJiki == dispRec.SakutsukeJiki) &&
                        (x.YotoKbn == dispRec.YotoKbn)
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