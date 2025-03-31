using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using CoreLibrary.Core.Consts;
using NskCommon = NskCommonLibrary.Core.Consts;
using ModelLibrary.Context;
using System.Collections.Generic;
using NskCommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;

namespace NskCommonLibrary.Core.Utility
{
    /// <summary>
    /// チェックユーティリティクラス
    /// 
    /// 作成日：2025/02/14
    /// </summary>
    public static class CheckUtil
    {
        #region バッチ予約取得
        /// <summary>
        /// バッチ予約ID取得
        /// </summary>
        /// <param name="bid">バッチID</param>
        /// <returns></returns>
        public static string GetBatchYoyaku(long bid)
        {
            string batchYoyakuId = string.Empty;
            SystemCommonContext db = new();
            ModelLibrary.Models.TBatchYoyaku? batchYoyaku = db.TBatchYoyakus.FirstOrDefault(x =>
                (x.BatchId == bid));

            // バッチ予約が存在する場合
            if (batchYoyaku != null)
            {
                batchYoyakuId = batchYoyaku.BatchYoyakuId;
            }
            else
            {
                ThrowUtil.Throw("ME01645", "パラメータの取得");
            }
            return batchYoyakuId;
        }
        #endregion

        #region 「バッチ条件情報」を取得する。
        /// <summary>
        /// 「バッチ条件情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">変数：バッチ条件ID</param>
        /// <param name="jokenNames">条件名のList</param>
        /// <returns>List<BatchJokenNameValue></returns>
        public static List<BatchJokenNameValue> GetbatchJoken(NskAppContext dbContext, string jid, List<string> jokenNames)
        {
            //List<T01050バッチ条件> batchJokens = dbContext.T01050バッチ条件s
            //    .Where(x => x.バッチ条件id == jid && jokenNames.Contains(x.条件名称))
            //    .ToList();

            List<BatchJokenNameValue> batchJokens =
             (from n in jokenNames
              from j in dbContext.T01050バッチ条件s
             .Where(j => j.バッチ条件id == jid)
             .Where(j => n == j.条件名称).DefaultIfEmpty()
              select new BatchJokenNameValue
              {
                  条件名称 = n,
                  バッチ条件 = j
             }).ToList();

            return batchJokens;
        }
        #endregion

        #region 引数チェック
        /// <summary>
        /// 引数チェック
        /// </summary>
        /// <param name="args">引数</param>
        public static void CheckArguments(string[] args)
        {
            // バッチID
            if (string.IsNullOrEmpty(args[0]))
            {
                ThrowUtil.Throw("ME01054", "バッチID");
            }

            if (!long.TryParse(args[0], out long result))
            {
                ThrowUtil.Throw("ME90012", "バッチID");
            }

            // 都道府県コード
            if (string.IsNullOrEmpty(args[1]))
            {
                ThrowUtil.Throw("ME01054", "都道府県コード");
            }

            // 組合等コード
            if (string.IsNullOrEmpty(args[2]))
            {
                ThrowUtil.Throw("ME01054", "組合等コード");
            }

            // 支所コード
            if (string.IsNullOrEmpty(args[3]))
            {
                ThrowUtil.Throw("ME01054", "支所コード");
            }

            // バッチ条件のキー情報
            if (string.IsNullOrEmpty(args[4]))
            {
                ThrowUtil.Throw("ME01054", "バッチ条件のキー情報");
            }
        }

        /// <summary>
        /// 引数チェック
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="batchId">バッチID</param>

        public static void CheckArguments(string userId, string joukenId, string todofukenCd, string kumiaitoCd, string shishoCd, long batchId)
        {
            // ユーザID
            if (string.IsNullOrEmpty(userId))
            {
                ThrowUtil.Throw("ME01054", "ユーザID");
            }
            // 条件ID
            if (string.IsNullOrEmpty(joukenId))
            {
                ThrowUtil.Throw("ME01054", "条件ID");
            }
            // 都道府県コード
            if (string.IsNullOrEmpty(todofukenCd))
            {
                ThrowUtil.Throw("ME01054", "都道府県コード");
            }
            // 組合等コード
            if (string.IsNullOrEmpty(kumiaitoCd))
            {
                ThrowUtil.Throw("ME01054", "組合等コード");
            }
            // 支所コード
            if (string.IsNullOrEmpty(shishoCd))
            {
                ThrowUtil.Throw("ME01054", "支所コード");
            }
            // バッチID
            if (batchId == 0)
            {
                ThrowUtil.Throw("ME01054", "バッチID");
            }
        }
        #endregion

        #region 整合性チェック
        /// <summary>
        /// 整合性チェック
        /// </summary>
        /// <param name="dbContext">DBContext</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <exception cref="AppException"></exception>

        public static void CheckSeigosei(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd) 
        {
            // 都道府県コード存在情報を取得する
            if (!dbContext.VTodofukens.Where(t => t.TodofukenCd == todofukenCd).Any())
            {
                ThrowUtil.Throw("ME10005", "都道府県コード");
            }
            // 組合等コード存在情報を取得する
            if (!dbContext.VKumiaitos.Where(t => t.TodofukenCd == todofukenCd && t.KumiaitoCd == kumiaitoCd).Any())
            {
                ThrowUtil.Throw("ME10005", "組合等コード");
            }
            // 支所コード存在情報を取得する
            if (!dbContext.VShishoNms.Where(t => t.TodofukenCd == todofukenCd && t.KumiaitoCd == kumiaitoCd && t.ShishoCd == shishoCd).Any())
            {
                ThrowUtil.Throw("ME10005", "支所コード");
            }
        }
        #endregion

        #region システム日付取得
        /// <summary>
        /// システム日付取得
        /// </summary>
        /// <returns>システム日付</returns>
        /// <exception cref="AppException"></exception>
        public static DateTime GetSysDateTime()
        {
            //１．設定ファイルから、以下の内容を取得し、グローバル変数へ保存する。
            // システム時間フラグ    （検索キー：SysDateTimeFlag）
            // appsetting.jsonの設定にあるシステム時間フラグ
            bool sysDateTimeFlag = false;
            if (bool.TryParse(ConfigUtil.Get(CoreLibrary.Core.Consts.CoreConst.SYS_DATE_TIME_FLAG), out sysDateTimeFlag))
            {
                // システム時間ファイルパス （検索キー：SysDateTimePath）※本項目は「システム時間フラグ」が"true"で取得できた場合のみ対象
                // appsetting.jsonの設定にあるシステム時間ファイルパス
                string sysDateTimePath = string.Empty;
                if (sysDateTimeFlag)
                {
                    // システム時間ファイルパス
                    sysDateTimePath = ConfigUtil.Get(CoreLibrary.Core.Consts.CoreConst.SYS_DATE_TIME_PATH);

                    if (string.IsNullOrEmpty(sysDateTimePath))
                    {
                        // エラーメッセージをログに出力し、処理を中断する。
                        // （"ME90015"、{0}：システム時間ファイルパス）
                        throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間ファイルパス"));
                    }
                }
            }
            else
            {
                // エラーメッセージをログに出力し、処理を中断する。
                // （"ME90015"、{0}：システム時間フラグ）
                throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間フラグ"));
            }

            // ２．システム日付の設定を行う。
            // ２．１．システム日付の設定
            // （ア）[変数：システム日付] に以下の条件に従って設定を行う。
            // （１）[グローバル変数：システム時間フラグ] がtrueの場合、[グローバル変数：システム時間ファイルパス] の年月日＋マシン時間を設定する。
            // （２）上記以外、マシン時間を設定する。
            // [変数：システム日付]
            DateTime systemDate = DateUtil.GetSysDateTime(); // DateUtil.GetSysDateTime()内で全てやっている

            return systemDate;
        }
        #endregion
    }
}