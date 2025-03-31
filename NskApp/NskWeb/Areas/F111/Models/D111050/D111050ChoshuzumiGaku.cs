using System.Data;
using System.Text;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;

namespace NskWeb.Areas.F111.Models.D111050
{
    public class D111050ChoshuzumiGaku
    {

        /// <summary>交付金計算</summary>
        public List<D111050ChoshuzumiGakuRecord> DispRecords { get; set; } = new();


        /// <summary>
        /// 徴収済み額を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <returns>検索情報</returns>
        public List<D111050ChoshuzumiGakuRecord> GetResult(NskAppContext dbContext, D111050SessionInfo sessionInfo)
        {

            // ３．１．t_12090_組合員等別徴収情報から [共済目的コード]に沿った徴収金額の合計値を取得する
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"   T1.共済目的コード         As \"{nameof(D111050ChoshuzumiGakuRecord.KyosaiMokutekiCd)}\" ");
            query.Append($"  ,M1.共済目的名称           As \"{nameof(D111050ChoshuzumiGakuRecord.KyosaiMokuteki)}\" ");
            query.Append($"  ,SUM(T1.徴収金額)          As \"{nameof(D111050ChoshuzumiGakuRecord.ChoshuzumiGaku)}\" ");

            query.Append(" FROM ");
            query.Append("   t_12090_組合員等別徴収情報 T1 ");
            query.Append("   INNER JOIN m_00010_共済目的名称 M1 ");
            query.Append("   ON M1.共済目的コード = T1.共済目的コード ");

            query.Append(" WHERE ");
            query.Append("   T1.組合等コード = @組合等コード ");
            query.Append("   AND T1.年産 = @年産 ");
            query.Append("   AND T1.共済目的コード = ANY(STRING_TO_ARRAY(@対象共済目的コード, ',')) ");

            query.Append(" GROUP BY ");
            query.Append("   T1.共済目的コード ");
            query.Append("  ,M1.共済目的名称 ");

            query.Append(" ORDER BY ");
            query.Append("   T1.共済目的コード ");
            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("対象共済目的コード", sessionInfo.TaishouKyosaiMokutekiCd)
            ];

            DispRecords.AddRange(dbContext.Database.SqlQueryRaw<D111050ChoshuzumiGakuRecord>(query.ToString(), queryParams));

            return DispRecords;
        }

        /// <summary>
        /// 交付徴収レコードの存在チェックをする。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="target">掛金徴収額レコードリスト</param>
        public void GetRecordCount(NskAppContext dbContext, D111050SessionInfo sessionInfo, string userId, D111050KakekinChoshugakuRecord target)
        {

            // ３．１．t_12090_組合員等別徴収情報から [共済目的コード]に沿った徴収金額の合計値を取得する
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append("   EXISTS ( ");
            query.Append("     SELECT ");
            query.Append("       *");

            query.Append("     FROM ");
            query.Append("       t_15010_交付徴収 ");

            query.Append("     WHERE ");
            query.Append("       組合等コード = @組合等コード ");
            query.Append("       AND 年産 = @年産 ");
            query.Append("       AND 負担金交付区分 = @負担金交付区分 ");
            query.Append("       AND 共済目的コード = @共済目的コード ");
            query.Append("       AND 交付回 = @交付回 ");
            query.Append($"   ) As \"{nameof(D111050KoufuChoshuExists.RecordExists)}\"  ");

            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd),
                new NpgsqlParameter("共済目的コード", target.KyosaiMokutekiCd),
                new NpgsqlParameter("交付回", sessionInfo.Koufukai),
            ];

            D111050KoufuChoshuExists record = dbContext.Database.SqlQueryRaw<D111050KoufuChoshuExists>(query.ToString(), queryParams).SingleOrDefault();

            // 交付徴収レコードの存在チェック
            if (record.RecordExists == false)
            {
                InsKoufuChoshu(dbContext, sessionInfo, userId, target);
            }
            else
            {
                UpdKoufuChoshu(dbContext, sessionInfo, userId, target);
            }

        }

        /// <summary>
        /// 交付徴収テーブルの登録
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="target">掛金徴収額レコードリスト</param>
        public void InsKoufuChoshu(NskAppContext dbContext, D111050SessionInfo sessionInfo, string userId, D111050KakekinChoshugakuRecord target)
        {
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // ３．１． t_15010_交付徴収の削除
            StringBuilder query = new();
            query.Append(" INSERT ");
            query.Append(" INTO ");
            query.Append("     t_15010_交付徴収");
            query.Append(" VALUES ( ");
            query.Append("     @組合等コード ");
            query.Append("   , @年産 ");
            query.Append("   , @負担金交付区分 ");
            query.Append("   , @共済目的コード ");
            query.Append("   , @交付回 ");
            query.Append("   , @掛金徴収済額 ");
            query.Append("   , @登録日時 ");
            query.Append("   , @登録ユーザid ");
            query.Append("   , @更新日時 ");
            query.Append("   , @更新ユーザid ");
            query.Append(" ) ");

            NpgsqlParameter[] addParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd),
                new NpgsqlParameter("共済目的コード", target.KyosaiMokutekiCd),
                new NpgsqlParameter("交付回", sessionInfo.Koufukai),
                new NpgsqlParameter("掛金徴収済額", target.ChoshuzumiGaku),
                new NpgsqlParameter("登録日時", systemDate),
                new NpgsqlParameter("登録ユーザid", userId),
                new NpgsqlParameter("更新日時", systemDate),
                new NpgsqlParameter("更新ユーザid", userId)
            ];
            int cnt = dbContext.Database.ExecuteSqlRaw(query.ToString(), addParams);
            if (cnt == 0)
            {
                throw new DBConcurrencyException();
            }

        }

        /// <summary>
        /// 交付徴収テーブルの更新
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="target">掛金徴収額レコードリスト</param>
        public void UpdKoufuChoshu(NskAppContext dbContext, D111050SessionInfo sessionInfo, string userId, D111050KakekinChoshugakuRecord target)
        {
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // ３．１． t_15010_交付徴収の削除
            StringBuilder query = new();
            query.Append(" UPDATE ");
            query.Append("     t_15010_交付徴収");
            query.Append(" SET ");
            query.Append("     掛金徴収済額 = @掛金徴収済額 ");
            query.Append("   , 更新日時 = @更新日時 ");
            query.Append("   , 更新ユーザid = @更新ユーザid ");
            query.Append(" WHERE ");
            query.Append("       組合等コード = @組合等コード ");
            query.Append("   AND 年産 = @年産 ");
            query.Append("   AND 負担金交付区分 = @負担金交付区分 ");
            query.Append("   AND 共済目的コード = @共済目的コード ");
            query.Append("   AND 交付回 = @交付回 ");
            query.Append("   AND xmin = @Xmin ");

            NpgsqlParameter[] addParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd),
                new NpgsqlParameter("共済目的コード", target.KyosaiMokutekiCd),
                new NpgsqlParameter("交付回", sessionInfo.Koufukai),
                new NpgsqlParameter("掛金徴収済額", target.ChoshuzumiGaku),
                new NpgsqlParameter("更新日時", systemDate),
                new NpgsqlParameter("更新ユーザid", userId),
                new NpgsqlParameter("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.KofuChoshuXmin }

            ];
            int cnt = dbContext.Database.ExecuteSqlRaw(query.ToString(), addParams);
            if (cnt == 0)
            {
                throw new DBConcurrencyException();
            }
        }

        /// <summary>
        /// 交付回テーブルの更新
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="target">掛金徴収額レコードリスト</param>
        public void UpdKoufukai(NskAppContext dbContext, D111050SessionInfo sessionInfo, string userId, D111050KakekinChoshugakuRecord target)
        {
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // ３．１． t_15010_交付徴収の削除
            StringBuilder query = new();
            query.Append(" UPDATE ");
            query.Append("     t_00050_交付回 ");
            query.Append(" SET ");
            query.Append("       紐づけ報告回   = @引受報告回 ");
            query.Append("    ,  更新日時       = @更新日時 ");
            query.Append("    ,  更新ユーザID   = @更新ユーザ ");
            query.Append(" WHERE ");
            query.Append("       組合等コード   = @組合等コード ");
            query.Append("  AND  年産           = @年産 ");
            query.Append("  AND  負担金交付区分 = @負担金交付区分 ");
            query.Append("  AND  共済目的コード = @共済目的コード ");
            query.Append("  AND  交付回         = @交付回 ");
            query.Append("  AND  xmin = @Xmin ");


            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("負担金交付区分", sessionInfo.FutankinKofuKbnCd),
                new NpgsqlParameter("共済目的コード", target.KyosaiMokutekiCd),
                new NpgsqlParameter("交付回", sessionInfo.Koufukai),
                new NpgsqlParameter("引受報告回", target.HikiukeHoukokukai),
                new NpgsqlParameter("更新日時", systemDate),
                new NpgsqlParameter("更新ユーザ", userId),
                new NpgsqlParameter("xmin", NpgsqlTypes.NpgsqlDbType.Xid) { Value = target.KofukaiXmin }
            ];
            int cnt = dbContext.Database.ExecuteSqlRaw(query.ToString(), queryParams);
            if (cnt == 0)
            {
                throw new DBConcurrencyException();
            }

        }

    }
}
