using NskAppModelLibrary.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using NskWeb.Common.Consts;
using NpgsqlTypes;

namespace NskWeb.Areas.F110.Models.D110020
{
    /// <summary>
    /// 引受回
    /// </summary>
    public class D110020HikiukeKai
    {
        /// <summary>表示用検索結果</summary>
        public List<D110020HikiukeKaiRecord> DispRecords { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D110020HikiukeKai()
        {
        }

        /// <summary>
        /// 引受回を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        public List<D110020HikiukeKaiRecord> GetResult(NskAppContext dbContext, D110020SessionInfo sessionInfo, string shishoCd)
        {
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"    M1.shisho_cd      as \"{nameof(D110020HikiukeKaiRecord.ShishoCd)}\" "); //支所コード
            query.Append($"  , M1.shisho_nm      as \"{nameof(D110020HikiukeKaiRecord.ShishoNm)}\" "); //支所名
            query.Append($"  , M2.引受回         as \"{nameof(D110020HikiukeKaiRecord.HikiukeKai)}\" ");
            query.Append($"  , M2.引受計算実施日 as \"{nameof(D110020HikiukeKaiRecord.HikiukeKeisanJikkobi)}\" ");
            query.Append($"  , M3.報告回         as \"{nameof(D110020HikiukeKaiRecord.HokokuKai)}\" ");
            query.Append($"  , M3.報告実施日     as \"{nameof(D110020HikiukeKaiRecord.HokokuHiduke)}\" ");
            query.Append($"  , M4.年産           as \"{nameof(D110020HikiukeKaiRecord.HikiukeKakuteiNensan)}\" ");
            query.Append($"  , M2.xmin           as \"{nameof(D110020HikiukeKaiRecord.Xmin)}\" ");
            query.Append(" FROM ");
            query.Append("   v_shisho_nm    M1 "); //名称M支所
            query.Append("   INNER JOIN ( ");
            query.Append("          SELECT ");
            query.Append("             組合等コード ");
            query.Append("           , 共済目的コード ");
            query.Append("           , 年産 ");
            query.Append("           , 支所コード ");
            query.Append("           , 引受回 ");
            query.Append("           , 引受計算実施日 ");
            query.Append("           , xmin ");
            query.Append("          FROM ");
            query.Append("            ( ");
            query.Append("              SELECT ");
            query.Append("                  * ");
            query.Append("                , RANK() OVER(PARTITION BY 組合等コード, 共済目的コード, 年産, 支所コード ORDER BY 引受回 DESC) 内部ソート ");
            query.Append("                , cast('' || xmin as integer) xmin ");
            query.Append("             FROM t_00010_引受回 ");
            query.Append("            ) a ");
            query.Append("          WHERE ");
            query.Append("            内部ソート = 1 ");
            query.Append("       ) M2 ");
            query.Append("    ON   M2.組合等コード   = M1.kumiaito_cd "); //組合等コード
            query.Append("    AND  M2.支所コード     = M1.shisho_cd ");   //支所コード
            query.Append("   LEFT OUTER JOIN t_00040_報告回     M3 ");
            query.Append("    ON   M3.組合等コード   = M2.組合等コード ");
            query.Append("    AND  M3.共済目的コード = M2.共済目的コード ");
            query.Append("    AND  M3.年産           = M2.年産 ");
            query.Append("    AND  M3.支所コード     = M2.支所コード ");
            query.Append("    AND  M3.紐づけ引受回   = M2.引受回 ");
            query.Append("   LEFT OUTER JOIN t_00020_引受確定   M4 ");
            query.Append("    ON   M4.組合等コード   = M2.組合等コード ");
            query.Append("    AND  M4.共済目的コード = M2.共済目的コード ");
            query.Append("    AND  M4.年産           = M2.年産 ");
            query.Append("    AND  M4.支所コード     = M2.支所コード ");
            query.Append("    AND  M4.確定引受回     = M2.引受回 ");
            query.Append(" WHERE ");
            query.Append("       M2.年産             = @年産 ");
            query.Append("  AND  M2.共済目的コード   = @共済目的コード ");
            query.Append("  AND  M1.todofuken_cd     = @都道府県コード ");  //都道府県コード
            query.Append("  AND  M1.kumiaito_cd      = @組合等コード ");    //組合等コード

            List<string> shishoCds = new();
            if (shishoCd == AppConst.HONSHO_CD)
            {
                if (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "1")
                {
                    // 本所のみ
                    query.Append($"  AND  M1.shisho_cd      = '{AppConst.HONSHO_CD}' ");    //支所コード
                }
                else if ((sessionInfo.HikiukeJikkoTanniKbnHikiuke == "2") ||
                         (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "3"))
                {
                    // 本所配下
                    query.Append("  AND  M1.shisho_cd      IN ( "); //支所コード
                    query.Append("           SELECT ");
                    query.Append("             shisho_cd "); //支所コード
                    query.Append("           FROM ");
                    query.Append("             v_shisho_nm ");
                    query.Append("           WHERE ");
                    query.Append("                 todofuken_cd   = @都道府県コード "); //都道府県コード
                    query.Append("            AND  kumiaito_cd    = @組合等コード ");   //組合等コード
                    query.Append($"            AND  shisho_cd     <> '{AppConst.HONSHO_CD}' ");            //支所コード
                    query.Append("          ) ");
                }
                //else if (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "3")
                //{
                //    // 利用可能支所
                //    shishoCds.AddRange(sessionInfo.RiyokanoShishoList.Select(x => x.ShishoCd));
                //    query.Append("  AND  M1.shisho_cd        = ANY ( @支所コードリスト )"); //利用可能支所コードリスト
                //}
            }
            else
            {
                query.Append("  AND  M1.shisho_cd        = @支所コード ");
            }

            NpgsqlParameter[] queryParams =
            [
                new ("都道府県コード", sessionInfo.TodofukenCd),
                new ("組合等コード", sessionInfo.KumiaitoCd),
                new ("年産", sessionInfo.Nensan),
                new ("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new ("支所コード", shishoCd),
                new ("支所コードリスト", NpgsqlDbType.Array | NpgsqlDbType.Varchar) { Value = shishoCds },
            ];

            List<D110020HikiukeKaiRecord> records = new();
            records.AddRange(dbContext.Database.SqlQueryRaw<D110020HikiukeKaiRecord>(query.ToString(), queryParams));

            return records;
        }
    }
}