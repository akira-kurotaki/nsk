using NskAppModelLibrary.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using NskAppModelLibrary.Models;
using NskWeb.Common.Consts;
using NpgsqlTypes;

namespace NskWeb.Areas.F110.Models.D110010
{
    /// <summary>
    /// 引受回
    /// </summary>
    public class D110010HikiukeKai
    {
        /// <summary>表示用検索結果</summary>
        public List<D110010HikiukeKaiRecord> DispRecords { get; set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D110010HikiukeKai()
        {
        }

        /// <summary>
        /// 引受回を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        public List<D110010HikiukeKaiRecord> GetResult(NskAppContext dbContext, D110010SessionInfo sessionInfo, string shishoCd)
        {
            StringBuilder query = new();
            query.Append(" SELECT ");
            query.Append($"    M1.shisho_cd      as \"{nameof(D110010HikiukeKaiRecord.ShishoCd)}\" "); //支所コード
            query.Append($"  , M1.shisho_nm      as \"{nameof(D110010HikiukeKaiRecord.ShishoNm)}\" "); //支所名
            query.Append($"  , M2.引受回         as \"{nameof(D110010HikiukeKaiRecord.HikiukeKai)}\" ");
            query.Append($"  , M2.引受計算実施日 as \"{nameof(D110010HikiukeKaiRecord.HikiukeKeisanJikkobi)}\" ");
            query.Append($"  , M3.報告回         as \"{nameof(D110010HikiukeKaiRecord.HokokuKai)}\" ");
            query.Append($"  , M3.報告実施日     as \"{nameof(D110010HikiukeKaiRecord.HokokuHiduke)}\" ");
            query.Append($"  , M4.確定引受回     as \"{nameof(D110010HikiukeKaiRecord.KakuteiHikiukeKai)}\" ");
            query.Append($"  , M2.xmin           as \"{nameof(D110010HikiukeKaiRecord.Xmin)}\" ");
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

            List<D110010HikiukeKaiRecord> records = new();
            records.AddRange(dbContext.Database.SqlQueryRaw<D110010HikiukeKaiRecord>(query.ToString(), queryParams));

            return records;
        }

        /// <summary>
        /// 引受回を加算したレコードを登録する。
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public int RegistHikiukeKai(ref NskAppContext dbContext, D110010SessionInfo sessionInfo, DateTime sysDateTime, string uid)
        {
            int registCnt = 0;
            // ２．１．セッション「処理対象（引受回情報）」に格納されているレコード分以下の処理を繰り返す
            List<T00010引受回> addRecs = new();
            foreach (D110010HikiukeKaiRecord target in DispRecords)
            {
                addRecs.Add(new()
                {
                    組合等コード = sessionInfo.KumiaitoCd,
                    共済目的コード = sessionInfo.KyosaiMokutekiCd,
                    年産 = (short)sessionInfo.Nensan,
                    支所コード = target.ShishoCd,
                    引受回 = (short)(target.HikiukeKai.GetValueOrDefault(0) + 1),
                    引受計算実施日 = null,
                    登録日時 = sysDateTime,
                    登録ユーザid = uid,
                    更新日時 = sysDateTime,
                    更新ユーザid = uid
                });
            }
            dbContext.T00010引受回s.AddRange(addRecs);
            registCnt = dbContext.SaveChanges();

            return registCnt;
        }

        /// <summary>
        /// 再引受前処理対象取得
        /// </summary>
        /// <returns></returns>
        public List<D110010HikiukeKaiRecord> GetTargets()
        {
            List<D110010HikiukeKaiRecord> targetRecs = new();

            // ２．１．セッション「検索結果（引受回情報）」に格納されているレコード分以下の処理を繰り返す
            foreach (D110010HikiukeKaiRecord dispRec in DispRecords)
            {
                // ２．１．１．セッション「検索結果（引受回情報）」からデータを１件取得

                // ２．１．２．取得した検索結果（引受回情報）の引受計算実行日がNULL以外の場合、
                // セッション「処理対象（引受回情報）」に取得した検索結果（引受回情報）を追加する。
                if (!dispRec.HikiukeKeisanJikkobi.HasValue)
                {
                    targetRecs.Add(dispRec);
                }
            }

            return targetRecs;
        }
    }
}