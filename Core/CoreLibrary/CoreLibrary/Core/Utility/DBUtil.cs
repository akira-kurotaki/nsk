using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// DBユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/15
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public static class DBUtil
    {
        /// <summary>
        /// シーケンスの現在値を取得する。
        /// </summary>
        /// <returns>SQL戻り値</returns>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="sequence">シーケンス名</param>
        /// <returns>SQL戻り値</returns>
        public static long CurrSeqVal(DbContext db, string sequence)
        {
            return db.Database.SqlQueryRaw<long>(string.Format(
                        "select last_value AS \"Value\" from {0}", sequence)).First();
        }

        /// <summary>
        /// シーケンスを増分し、増分後の値を取得する。
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="sequence">シーケンス名</param>
        /// <returns>SQL戻り値</returns>
        public static long NextSeqVal(DbContext db, string sequence)
        {
            return db.Database.SqlQueryRaw<long>(string.Format(
                "select nextval('{0}') AS \"Value\"", sequence)).First();
        }

        /// <summary>
        /// システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
        /// </summary>
        /// <param name="systemKbn">システム区分</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>DB接続先情報</returns>
        public static DbConnectionInfo GetDbConnectionInfo(string systemKbn, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            using (var nohoDb = new SystemCommonContext())
            {
                MSystemConnection dbConn = null;
                // 都道府県で絞り込み
                var list = nohoDb.MSystemConnections.Where(
                                s => s.SystemKbn == systemKbn &&
                                s.TodofukenCd == todofukenCd).ToList();
                if (list.Count == 1)
                {
                    dbConn = list[0];
                }
                else if (list.Count > 0)
                {
                    // 都道府県のみで1件に絞り込めない場合は、都道府県、組合等で絞り込み
                    list = nohoDb.MSystemConnections.Where(
                                    s => s.SystemKbn == systemKbn &&
                                    s.TodofukenCd == todofukenCd &&
                                    s.KumiaitoCd == kumiaitoCd).ToList();
                    if (list.Count == 1)
                    {
                        dbConn = list[0];
                    }
                    else if (list.Count > 0)
                    {
                        // 都道府県、組合等で1件に絞り込めない場合は、都道府県、組合等、支所で絞り込み
                        list = nohoDb.MSystemConnections.Where(
                                        s => s.SystemKbn == systemKbn &&
                                        s.TodofukenCd == todofukenCd &&
                                        s.KumiaitoCd == kumiaitoCd &&
                                        s.ShishoCd == shishoCd).ToList();
                        if (list.Count != 1)
                        {
                            // TODO：都道府県、組合等、支所で1件に絞り込めない場合のエラー処理
                            throw new AppException("MF00001", SystemMessageUtil.Get("MF00001"), CoreConst.HEADER_PATTERN_ID_2);
                        }
                        dbConn = list[0];
                    }
                }

                DbConnectionInfo dbConInfo = null;
                if (dbConn != null)
                {
                    dbConInfo = new DbConnectionInfo
                    {
                        TodofukenCd = dbConn.TodofukenCd,
                        KumiaitoCd = dbConn.KumiaitoCd,
                        ShishoCd = dbConn.ShishoCd,
                        SystemKbn = dbConn.SystemKbn,
                        ConnectionString = dbConn.ConnectionString,
                        DefaultSchema = dbConn.DefaultSchema
                    };
                }

                return dbConInfo;
            }
        }
    }
}