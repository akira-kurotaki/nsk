using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;

namespace NskWeb.Areas.F111.Models.D111050
{
    public class D111050KakekinChoshugaku
    {

        /// <summary>交付金計算</summary>
        public List<D111050KakekinChoshugakuRecord> DispRecords { get; set; } = new();


        /// <summary>
        /// 交付金計算掛金徴収額一覧を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <returns>検索情報</returns>
        public List<D111050KakekinChoshugakuRecord> GetResult(NskAppContext dbContext, D111050SessionInfo sessionInfo)
        {

            // (1) t_00050_交付回、t_13010_組合等引受_合計部、t_15010_交付徴収テーブルより、交付徴収データを取得する。
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"   T1.共済目的コード                         AS \"{nameof(D111050KakekinChoshugakuRecord.KyosaiMokutekiCd)}\" ");
            query.Append($"  ,SQ1.共済目的名称                          AS \"{nameof(D111050KakekinChoshugakuRecord.KyosaiMokuteki)}\" ");
            query.Append($"  ,SQ1.報告回                                AS \"{nameof(D111050KakekinChoshugakuRecord.HikiukeHoukokukai)}\" ");
            query.Append($"  ,COALESCE(SQ1.組合等計交付対象負担金額, 0) AS \"{nameof(D111050KakekinChoshugakuRecord.Futankin)}\" ");
            query.Append($"  ,COALESCE(SQ1.掛金徴収済額, 0)             AS \"{nameof(D111050KakekinChoshugakuRecord.ChoshuzumiGaku)}\" ");
            query.Append($"  ,CASE  ");
            query.Append($"     WHEN SQ1.掛金徴収済額 is null  ");
            query.Append($"         THEN 0 ");
            query.Append($"     WHEN SQ1.組合等計交付対象負担金額 > 0 ");
            query.Append($"         THEN TRUNC(SQ1.掛金徴収済額 / SQ1.組合等計交付対象負担金額 * 100, 2) ");
            query.Append($"     ELSE 0 ");   
            query.Append($"   END                                       AS \"{nameof(D111050KakekinChoshugakuRecord.ChoshuWariai)}\" ");
            query.Append($"  ,SQ1.交付徴収Xmin                          AS \"{nameof(D111050KakekinChoshugakuRecord.KofuChoshuXmin)}\" ");
            query.Append($"  ,cast('' || T1.Xmin as integer)            AS \"{nameof(D111050KakekinChoshugakuRecord.KofukaiXmin)}\" ");
            query.Append(" FROM ");
            query.Append("   t_00050_交付回 T1 ");
            query.Append("   INNER JOIN ");
            query.Append("   ( ");
            query.Append("      SELECT ");
            query.Append("         SQT1.組合等コード ");
            query.Append("        ,SQT1.年産 ");
            query.Append("        ,SQT1.共済目的コード ");
            query.Append("        ,SQM1.共済目的名称 ");
            query.Append("        ,SQT1.報告回 ");
            query.Append("        ,SQT1.組合等計交付対象負担金額 ");
            query.Append("        ,SQT2.掛金徴収済額 ");
            query.Append("        ,cast('' || SQT2.Xmin as integer) AS 交付徴収Xmin ");
            query.Append("        ,ROW_NUMBER() OVER (PARTITION BY SQT1.共済目的コード ORDER BY SQT1.報告回 DESC) AS row_num ");

            query.Append("      FROM t_13010_組合等引受_合計部 SQT1 ");
            query.Append("      INNER JOIN m_00010_共済目的名称 SQM1 ");
            query.Append("        ON SQM1.共済目的コード        =  SQT1.共済目的コード ");
            query.Append("      LEFT OUTER JOIN t_15010_交付徴収 SQT2 ");
            query.Append("        ON SQT2.組合等コード          =  SQT1.組合等コード ");
            query.Append("        AND SQT2.年産 = SQT1.年産 ");
            query.Append("        AND SQT2.共済目的コード       =  SQT1.共済目的コード ");
            query.Append("        AND SQT2.交付回               =  @交付回 ");

            query.Append("      WHERE ");
            query.Append("            SQT1.組合等コード         = @組合等コード ");
            query.Append("        AND SQT1.年産                 = @年産 ");
            query.Append("        AND SQT1.共済目的コード       = ANY(STRING_TO_ARRAY(@対象共済目的コード, ',')) ");

            query.Append("      ORDER BY ");
            query.Append("            SQT1.共済目的コード ASC ");
            query.Append("   ) AS SQ1 ");
            query.Append("   ON  T1.組合等コード   = SQ1.組合等コード ");
            query.Append("   AND T1.年産           = SQ1.年産 ");
            query.Append("   AND T1.共済目的コード = SQ1.共済目的コード ");
            query.Append("   AND SQ1.row_num       = 1 ");

            query.Append(" WHERE ");
            query.Append("   T1.交付回 =  @交付回  ");

            query.Append(" ORDER BY ");
            query.Append("   T1.共済目的コード ASC ");
            NpgsqlParameter[] queryParams =
            [
                new NpgsqlParameter("組合等コード", sessionInfo.KumiaitoCd),
                new NpgsqlParameter("年産", sessionInfo.Nensan),
                new NpgsqlParameter("対象共済目的コード", sessionInfo.TaishouKyosaiMokutekiCd),
                new NpgsqlParameter("交付回", sessionInfo.Koufukai)
            ];

            DispRecords.AddRange(dbContext.Database.SqlQueryRaw<D111050KakekinChoshugakuRecord>(query.ToString(), queryParams));

            return DispRecords;
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src">コピー元</param>
        public void ApplyInput(D111050KakekinChoshugaku src)
        {
            this.DispRecords = src.DispRecords;
        }

    }
}
