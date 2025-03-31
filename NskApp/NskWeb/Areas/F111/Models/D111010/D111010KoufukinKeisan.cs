using System.Text;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;

namespace NskWeb.Areas.F111.Models.D111010
{
    public class D111010KoufukinKeisan
    {

        /// <summary>交付金計算</summary>
        public List<D111010KoufukinKeisanRecord> DispRecords { get; set; } = new();


        /// <summary>
        /// 交付金計算一覧を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <returns>検索情報</returns>
        public List<D111010KoufukinKeisanRecord> GetResult(NskAppContext dbContext, D111010SessionInfo sessionInfo)
        {

            // (1) t_00050_交付回、t_13010_組合等引受_合計部、t_15010_交付徴収テーブルより、交付徴収データを取得する。
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"   T1.交付回                                     As \"{nameof(D111010KoufukinKeisanRecord.Koufukai)}\" ");
            query.Append($"  ,T1.交付計算実施日                             As \"{nameof(D111010KoufukinKeisanRecord.KoufukinKeisanJisshibi)}\" ");
            query.Append($",  COALESCE(SUM(T2.組合等計交付対象負担金額), 0) As \"{nameof(D111010KoufukinKeisanRecord.Futankin)}\" ");
            query.Append($",  COALESCE(SUM(T3.掛金徴収済額), 0)             As \"{nameof(D111010KoufukinKeisanRecord.ChoshuzumiGaku)}\" ");
            query.Append($",  CASE ");
            query.Append($"       WHEN SUM(T2.組合等計交付対象負担金額) > 0 ");
            query.Append($"           THEN TRUNC(COALESCE(SUM(T3.掛金徴収済額), 0) / SUM(T2.組合等計交付対象負担金額) * 100, 2) ");
            query.Append($"       ELSE 0 ");
            query.Append($"   END                                           As \"{nameof(D111010KoufukinKeisanRecord.ChoshuWariai)}\" ");
            query.Append($",  EXISTS ( ");
            query.Append($"       SELECT * ");
            query.Append($"       FROM   t_15010_交付徴収 ");
            query.Append($"       WHERE  組合等コード       = @組合等コード ");
            query.Append($"              AND 年産           = @年産 ");
            query.Append($"              AND 交付回         = T1.交付回 ");
            query.Append($"              AND 負担金交付区分 = @負担金交付区分 ");
            query.Append($"   )                                             As \"{nameof(D111010KoufukinKeisanRecord.ChoshuGakuNyuryokuzumi)}\" ");

            query.Append(" FROM t_00050_交付回 T1 ");
            query.Append(" LEFT OUTER JOIN t_13010_組合等引受_合計部 T2 ");
            query.Append("  ON   T2.組合等コード   = T1.組合等コード ");
            query.Append("  AND  T2.年産           = T1.年産 ");
            query.Append("  AND  T2.共済目的コード = T1.共済目的コード ");
            query.Append("  AND  T2.報告回         = T1.紐づけ報告回 ");
            query.Append(" LEFT OUTER JOIN t_15010_交付徴収 T3 ");
            query.Append("  ON   T3.組合等コード   = T1.組合等コード ");
            query.Append("  AND  T3.年産           = T1.年産 ");
            query.Append("  AND  T3.共済目的コード = T1.共済目的コード ");
            query.Append("  AND  T3.交付回         = T1.交付回 ");
            query.Append("  AND  T3.負担金交付区分 = T1.負担金交付区分 ");

            query.Append(" WHERE ");
            query.Append("       T1.組合等コード   = @組合等コード ");
            query.Append("  AND  T1.年産           = @年産 ");
            query.Append("  AND  T1.負担金交付区分 = @負担金交付区分 ");

            query.Append(" GROUP BY ");
            query.Append("    T1.交付回 ");
            query.Append("  , T1.交付計算実施日 ");

            query.Append(" ORDER BY ");
            query.Append("       T1.交付回 ASC ");
            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd)
            ];

            DispRecords.AddRange(dbContext.Database.SqlQueryRaw<D111010KoufukinKeisanRecord>(query.ToString(), queryParams));

            return DispRecords;
        }

        /// <summary>
        /// 組合等交付テーブルの削除
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="koufukai">交付回</param>
        /// <returns></returns>
        public int DelKumiaitoKoufu(NskAppContext dbContext, D111010SessionInfo sessionInfo, int koufukai)
        {
            int delCount = 0;

            // ３．１． t_15020_組合等交付の削除
            StringBuilder query = new();
            query.Append(" DELETE ");
            query.Append(" FROM t_15020_組合等交付 ");
            query.Append(" WHERE ");
            query.Append("       組合等コード   = @組合等コード ");
            query.Append("  AND  年産           = @年産 ");
            query.Append("  AND  負担金交付区分 = @負担金交付区分 ");
            query.Append("  AND  交付回         = @交付回 ");

            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd),
                new NpgsqlParameter("交付回", koufukai)
            ];

            delCount += dbContext.Database.ExecuteSqlRaw(query.ToString(), queryParams); ;

            return delCount;
        }

        /// <summary>
        /// 交付徴収テーブルの削除
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="koufukai">交付回</param>
        /// <returns></returns>
        public int DelKoufuChoshu(NskAppContext dbContext, D111010SessionInfo sessionInfo, int koufukai)
        {
            int delCount = 0;

            // ３．１． t_15010_交付徴収の削除
            StringBuilder query = new();
            query.Append(" DELETE ");
            query.Append(" FROM t_15010_交付徴収 ");
            query.Append(" WHERE ");
            query.Append("       組合等コード   = @組合等コード ");
            query.Append("  AND  年産           = @年産 ");
            query.Append("  AND  負担金交付区分 = @負担金交付区分 ");
            query.Append("  AND  交付回         = @交付回 ");

            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd),
                new NpgsqlParameter("交付回", koufukai)
            ];

            delCount += dbContext.Database.ExecuteSqlRaw(query.ToString(), queryParams); ;

            return delCount;
        }

        /// <summary>
        /// 交付回テーブルの削除
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="koufukai">交付回</param>
        /// <returns></returns>
        public int DelKoufukai(NskAppContext dbContext, D111010SessionInfo sessionInfo, int koufukai)
        {
            int delCount = 0;

            // ３．１． t_15010_交付徴収の削除
            StringBuilder query = new();
            query.Append(" DELETE ");
            query.Append(" FROM t_00050_交付回 ");
            query.Append(" WHERE ");
            query.Append("       組合等コード   = @組合等コード ");
            query.Append("  AND  年産           = @年産 ");
            query.Append("  AND  負担金交付区分 = @負担金交付区分 ");
            query.Append("  AND  交付回         = @交付回 ");

            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd),
                new NpgsqlParameter("交付回", koufukai)
            ];

            delCount += dbContext.Database.ExecuteSqlRaw(query.ToString(), queryParams); ;

            return delCount;
        }

        /// <summary>
        /// 交付回テーブルの更新（交付金計算情報削除）
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="koufukai">交付回</param>
        /// <returns></returns>
        public int UpdKoufukaiKoufukinKeisanDelete(NskAppContext dbContext, D111010SessionInfo sessionInfo, int koufukai, string userId)
        {
            int updCount = 0;

            // ３．１． t_15010_交付徴収の削除
            StringBuilder query = new();
            query.Append(" UPDATE ");
            query.Append("     t_00050_交付回 ");
            query.Append(" SET ");
            query.Append("       紐づけ報告回   = null ");
            query.Append("    ,  交付計算実施日 = null ");
            query.Append("    ,  更新日時       = @更新日時 ");
            query.Append("    ,  更新ユーザID   = @更新ユーザ ");
            query.Append(" WHERE ");
            query.Append("       組合等コード   = @組合等コード ");
            query.Append("  AND  年産           = @年産 ");
            query.Append("  AND  負担金交付区分 = @負担金交付区分 ");
            query.Append("  AND  交付回         = @交付回 ");

            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd),
                new NpgsqlParameter("交付回", koufukai),
                new NpgsqlParameter("更新日時", DateUtil.GetSysDateTime()),
                new NpgsqlParameter("更新ユーザ", userId)
            ];

            updCount += dbContext.Database.ExecuteSqlRaw(query.ToString(), queryParams); ;

            return updCount;
        }

    }
}
