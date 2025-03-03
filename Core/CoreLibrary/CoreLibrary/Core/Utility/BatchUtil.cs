using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using Npgsql;
using NpgsqlTypes;
using System.ComponentModel;
using System.Text;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// バッチ関連ユーティリティ
    /// </summary>
    public static class BatchUtil
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 処理成否（失敗）
        /// </summary>
        public const int RET_FAIL = 0;

        /// <summary>
        /// 処理成否（成功）
        /// </summary>
        public const int RET_SUCCESS = 1;

        /// <summary>
        /// バッチ名（CSV出力）
        /// </summary>
        public const string BATCH_NM_CSV = "CSV出力";

        /// <summary>
        /// バッチステータス（処理待ち）
        /// </summary>
        public const string BATCH_STATUS_WAITING = "01";

        /// <summary>
        /// バッチステータス（処理中）
        /// </summary>
        public const string BATCH_STATUS_RUNNING = "02";

        /// <summary>
        /// バッチステータス（成功）
        /// </summary>
        public const string BATCH_STATUS_SUCCESS = "03";

        /// <summary>
        /// バッチステータス（キャンセル）
        /// </summary>
        public const string BATCH_STATUS_CANCEL = "11";

        /// <summary>
        /// バッチステータス（削除）
        /// </summary>
        public const string BATCH_STATUS_DELETE = "98";

        /// <summary>
        /// バッチステータス（エラー）
        /// </summary>
        public const string BATCH_STATUS_ERROR = "99";

        /// <summary>
        /// バッチ種類（巡回バッチ）
        /// </summary>
        public const string BATCH_TYPE_PATROL = "1";

        /// <summary>
        /// バッチ種類（定時バッチ）
        /// </summary>
        public const string BATCH_TYPE_SCHEDULED = "2";

        /// <summary>
        /// 削除フラグ（未削除）
        /// </summary>
        public const string DELETE_FLG_NOT_DELETED = "0";

        /// <summary>
        /// 削除フラグ（削除済み）
        /// </summary>
        public const string DELETE_FLG_DELETED = "1";

        #region バッチ予約登録
        /// <summary>
        /// バッチ予約登録
        /// </summary>
        /// <param name="strBatchBunrui">バッチ分類</param>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="strKumiaitoCd">組合等コード</param>
        /// <param name="strShishoCd">支所コード</param>
        /// <param name="dateYoyakuDate">予約日時</param>
        /// <param name="strYoyakuUserId">予約ユーザID</param>
        /// <param name="strYoyakuShoriNm">予約を実行した処理名</param>
        /// <param name="strBatchNm">バッチ名</param>
        /// <param name="strBatchJoken">バッチ条件</param>
        /// <param name="strBatchJokenDisp">バッチ条件(表示用）</param>
        /// <param name="strMultiRunNgFlg">複数実行禁止フラグ</param>
        /// <param name="strBatchType">バッチ種類（巡回、定時）</param>
        /// <param name="dateBatchScheDatetime">定時実行バッチ予約日時</param>
        /// <param name="strLockTargetFlg">ロック対象フラグ</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <param name="batchId">バッチID(出力パラメータ)</param>
        /// <param name="strMultiRunNgId">複数実行禁止ID</param>
        /// <returns>処理成否：0（失敗）、1（成功）</returns>
        public static int InsertBatchYoyaku(string strBatchBunrui,
                                            string strSystemKbn,
                                            string strTodofukenCd,
                                            string strKumiaitoCd,
                                            string strShishoCd,
                                            DateTime? dateYoyakuDate,
                                            string strYoyakuUserId,
                                            string strYoyakuShoriNm,
                                            string strBatchNm,
                                            string strBatchJoken,
                                            string strBatchJokenDisp,
                                            string strMultiRunNgFlg,
                                            string strBatchType,
                                            DateTime? dateBatchScheDatetime,
                                            string strLockTargetFlg,
                                            ref string message,
                                            ref long batchId,
                                            string strMultiRunNgId = null)
        {
            message = null;
            batchId = 0L;

            var param = new InsertBatchYoyakuParam
            {
                BatchBunrui = strBatchBunrui,
                SystemKbn = strSystemKbn,
                TodofukenCd = strTodofukenCd,
                KumiaitoCd = strKumiaitoCd,
                ShishoCd = strShishoCd,
                YoyakuDate = dateYoyakuDate,
                YoyakuUserId = strYoyakuUserId,
                YoyakuShoriNm = strYoyakuShoriNm,
                BatchNm = strBatchNm,
                BatchJoken = strBatchJoken,
                BatchJokenDisp = strBatchJokenDisp,
                MultiRunNgFlg = strMultiRunNgFlg,
                MultiRunNgId = strMultiRunNgId,
                BatchType = strBatchType,
                BatchScheDatetime = dateBatchScheDatetime,
                LockTargetFlg = strLockTargetFlg
            };

            // 必須チェック
            if (!CheckInsertBatchYoyakuParamRequired(param, ref message))
            {
                return RET_FAIL;
            }

            // 桁数チェック
            if (!CheckInsertBatchYoyakuParamMaxLength(param, ref message))
            {
                return RET_FAIL;
            }

            try
            {
                // マスタ整合性チェック
                if (!CheckInsertBatchYoyakuParamMaster(param, ref message))
                {
                    return RET_FAIL;
                }

                // バッチ予約登録
                InsertTBatchYoyaku(param, ref batchId);
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                message = SystemMessageUtil.Get("MF80001");
                return RET_FAIL;
            }
            return RET_SUCCESS;
        }
        #endregion

        #region バッチ予約状況取得
        /// <summary>
        /// バッチ予約状況取得
        /// </summary>
        /// <param name="param">バッチ予約状況取得条件</param>
        /// <param name="boolAllCntFlg">総件数取得フラグ</param>
        /// <param name="intAllCnt">件数（出力パラメータ）</param>
        /// <param name="message">エラーメッセージ（出力パラメータ）</param>
        /// <returns>バッチ予約状況</returns>
        public static List<BatchYoyaku> GetBatchYoyakuList(GetBatchYoyakuListParam param,
                                                            bool boolAllCntFlg,
                                                            ref int intAllCnt,
                                                            ref string message)
        {
            intAllCnt = 0;
            message = null;
            List<BatchYoyaku> batchYoyakuList = null;

            try
            {
                using (var db = new SystemCommonContext())
                {
                    // 総件数取得の場合
                    if (boolAllCntFlg) {
                        logger.Info("バッチ予約件数を取得します。");
                        int totalCount = GetBatchYoyakuListCount(db, param);
                        // バッチ管理データ総件数を設定する
                        intAllCnt = totalCount;
                        logger.Debug("バッチ予約件数：{0}", totalCount);
                    }
                    else
                    {
                        logger.Info("バッチ予約状況を取得します。");
                        batchYoyakuList = GetBatchYoyakuListPageData(db, param);
                        // 取得結果件数を設定する
                        intAllCnt = batchYoyakuList.Count;
                    }
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                message = SystemMessageUtil.Get("MF80002");
            }

            return batchYoyakuList;
        }
        #endregion

        #region バッチ実行件数取得（サーバ）
        /// <summary>
        /// バッチ実行件数取得（サーバ）
        /// </summary>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>バッチ実行件数（サーバ）</returns>
        public static List<BatchRunCntSrv> GetBatchRunCntSrv(ref string message)
        {
            message = null;
            List<BatchRunCntSrv> batchRunCntList = null;

            try
            {
                logger.Info("バッチ実行件数（サーバ）を取得します。");
                using (var db = new SystemCommonContext())
                {
                    batchRunCntList = db.TBatchYoyakus
                                        .Where(a => a.BatchStatus == BATCH_STATUS_RUNNING && a.DeleteFlg == DELETE_FLG_NOT_DELETED)
                                        .GroupBy(a => a.BatchRunServer)
                                        .Select(a => new BatchRunCntSrv { BatchRunServer = a.Key, Count = a.Count() })
                                        .ToList();
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                message = SystemMessageUtil.Get("MF80002");
            }

            return batchRunCntList;
        }
        #endregion

        #region バッチ実行件数取得（都道府県）
        /// <summary>
        /// バッチ実行件数取得（都道府県）
        /// </summary>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>バッチ実行件数（都道府県）</returns>
        public static List<BatchRunCntTdofuken> GetBatchRunCntTdofuken(ref string message)
        {
            message = null;

            var sql =
                "SELECT " +
                "  m_todofuken.todofuken_cd AS TodofukenCd, " +
                "  m_todofuken.todofuken_nm AS TodofukenNm, " +
                "  SUM ( CASE WHEN batch_status = '02' THEN 1 ELSE 0 END ) AS RunningCount, " +
                "  SUM ( CASE WHEN batch_status = '01' THEN 1 ELSE 0 END ) AS WaitingCount " +
                "FROM " +
                "  m_todofuken " +
                "  LEFT JOIN t_batch_yoyaku " +
                "    ON t_batch_yoyaku.todofuken_cd = m_todofuken.todofuken_cd " +
                "WHERE " +
                "  batch_status IN ('01', '02') " +
                "  AND delete_flg = '0' " +
                "GROUP BY " +
                "  m_todofuken.todofuken_cd, " +
                "  m_todofuken.todofuken_nm " +
                "ORDER BY " +
                "  m_todofuken.todofuken_cd ";

            List <BatchRunCntTdofuken> batchRunCntList = null;

            try
            {
                logger.Info("バッチ実行件数（都道府県）を取得します。");
                using (var db = new SystemCommonContext())
                {
                    batchRunCntList = db.Database.SqlQueryRaw<BatchRunCntTdofuken>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                message = SystemMessageUtil.Get("MF80002");
            }

            return batchRunCntList;
        }
        #endregion

        #region 実行可能バッチ予約取得
        /// <summary>
        /// 実行可能バッチ予約取得
        /// </summary>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strBatchNm">バッチ名</param>
        /// <param name="strBatchRunServer">実行サーバ</param>
        /// <param name="strUpdateUserId">更新ユーザID</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>バッチ予約</returns>
        public static BatchYoyaku GetRunBatchYoyaku(string strSystemKbn,
                                                    string strBatchNm,
                                                    string strBatchRunServer,
                                                    string strUpdateUserId,
                                                    ref string message)
        {
            message = null;

            var param = new GetRunBatchYoyakuParam
            {
                SystemKbn = strSystemKbn,
                BatchNm = strBatchNm,
                BatchRunServer = strBatchRunServer,
                UpdateUserId = strUpdateUserId
            };

            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // 必須チェック
            if (!CheckGetRunBatchYoyakuParamRequired(param, ref message))
            {
                return null;
            }

            // 桁数チェック
            if (!CheckGetRunBatchYoyakuParamMaxLength(param, ref message))
            {
                return null;
            }

            BatchYoyaku batchYoyaku = null;

            try
            {
                // マスタ整合性チェック
                if (!CheckGetRunBatchYoyakuParamMaster(param, ref message))
                {
                    return null;
                }

                // 独自チェック
                if (!CheckGetRunBatchYoyakuParamCustom(param, ref message))
                {
                    return null;
                }

                using (var db = new SystemCommonContext())
                {

                    // 実行可能時間帯の確認
                    var executableBatchTypeList = GetExecutableBatchType(db, param, systemDate);
                    if (executableBatchTypeList.IsNullOrEmpty())
                    {
                        // データが取得できなかった場合
                        message = SystemMessageUtil.Get("MI80001");
                        return null;
                    }

                    using (var tx = db.Database.BeginTransaction())
                    {
                        // システム共通スキーマのバッチ予約テーブルをテーブルロックする。（ログ出力あり）
                        logger.Info("バッチ予約テーブルをテーブルロックします。");
                        db.Database.ExecuteSqlRaw("LOCK TABLE t_batch_yoyaku IN EXCLUSIVE MODE");

                        // 実行中のバッチの件数を取得
                        var runningCount = GetRunningBatchCount(db, param);

                        // バッチ実行上限数マスタのバッチ実行上限数を取得
                        var batchRunMax = GetBatchRunMax(db);

                        // 実行中バッチ件数＞バッチ実行上限数の場合
                        if (runningCount > batchRunMax)
                        {
                            message = SystemMessageUtil.Get("MI80002");
                            return null;
                        }

                        // 実行可能な最も古い予約データを取得
                        var tBatchYoyaku = GetOldestBatchYoyaku(db, param, executableBatchTypeList, systemDate);
                        if (tBatchYoyaku == null)
                        {
                            message = SystemMessageUtil.Get("MI80003");
                            return null;
                        }

                        // バッチステータス（処理中）
                        tBatchYoyaku.BatchStatus = BATCH_STATUS_RUNNING;
                        // バッチ開始日時
                        tBatchYoyaku.BatchStartDate = systemDate;
                        // 実行サーバ
                        tBatchYoyaku.BatchRunServer = param.BatchRunServer;
                        // 更新ユーザID
                        tBatchYoyaku.UpdateUserId = param.UpdateUserId;
                        // 更新日時
                        tBatchYoyaku.UpdateDate = systemDate;

                        db.Entry(tBatchYoyaku).State = EntityState.Modified;
                        try
                        {
                            logger.Info("バッチ予約データを更新します。");
                            db.SaveChanges();
                            tx.Commit();
                            logger.Info("バッチ予約テーブルをテーブルロックを解除しました。");
                        }
                        catch (DbUpdateException e)
                        {
                            // 失敗の場合
                            logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                            message = SystemMessageUtil.Get("MF80002");

                            tx.Rollback();
                            return null;
                        }

                        batchYoyaku = new BatchYoyaku
                        {
                            BatchId = tBatchYoyaku.BatchId,
                            BatchBunrui = tBatchYoyaku.BatchBunrui,
                            SystemKbn = tBatchYoyaku.SystemKbn,
                            TodofukenCd = tBatchYoyaku.TodofukenCd,
                            KumiaitoCd = tBatchYoyaku.KumiaitoCd,
                            ShishoCd = tBatchYoyaku.ShishoCd,
                            BatchYoyakuDate = tBatchYoyaku.BatchYoyakuDate,
                            BatchYoyakuId = tBatchYoyaku.BatchYoyakuId,
                            BatchYoyakuShori = tBatchYoyaku.BatchYoyakuShori,
                            BatchNm = tBatchYoyaku.BatchNm,
                            BatchJoken = tBatchYoyaku.BatchJoken,
                            BatchJokenDisp = tBatchYoyaku.BatchJokenDisp,
                            MultiRunNgFlg = tBatchYoyaku.MultiRunNgFlg,
                            MultiRunNgId = tBatchYoyaku.MultiRunNgId,
                            BatchType = tBatchYoyaku.BatchType,
                            BatchScheDatetime = tBatchYoyaku.BatchScheDatetime,
                            LockTargetFlg = tBatchYoyaku.LockTargetFlg,
                            BatchStatus = tBatchYoyaku.BatchStatus,
                            BatchStartDate = tBatchYoyaku.BatchStartDate,
                            BatchEndDate = tBatchYoyaku.BatchEndDate,
                            BatchRunServer = tBatchYoyaku.BatchRunServer,
                            ErrorInfo = tBatchYoyaku.ErrorInfo,
                            InsertUserId = tBatchYoyaku.InsertUserId,
                            InsertDate = tBatchYoyaku.InsertDate,
                            UpdateUserId = tBatchYoyaku.UpdateUserId,
                            UpdateDate = tBatchYoyaku.UpdateDate
                        };
                    }
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                message = SystemMessageUtil.Get("MF80002");
            }
            return batchYoyaku;
        }
        #endregion

        #region バッチ実行状況更新
        /// <summary>
        /// バッチ実行状況更新
        /// </summary>
        /// <param name="lngBatchId">バッチID</param>
        /// <param name="strStatus">ステータス</param>
        /// <param name="strErrInfo">エラー情報</param>
        /// <param name="strUpdateUserId">更新ユーザID</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>処理成否：0（失敗）、1（成功）</returns>
        public static int UpdateBatchYoyakuSts(long lngBatchId,
                                                string strStatus,
                                                string strErrInfo,
                                                string strUpdateUserId,
                                                ref string message)
        {
            message = null;

            var param = new UpdateBatchYoyakuStsParam
            {
                BatchId = lngBatchId,
                Status = strStatus,
                ErrInfo = strErrInfo,
                UpdateUserId = strUpdateUserId
            };

            // 必須チェック
            if (!CheckUpdateBatchYoyakuStsParamRequired(param, ref message))
            {
                return RET_FAIL;
            }

            try
            {
                // マスタ整合性チェック
                if (!CheckUpdateBatchYoyakuStsParamMaster(param, ref message))
                {
                    return RET_FAIL;
                }

                // 独自チェック
                if (!CheckUpdateBatchYoyakuStsParamCustom(param, ref message))
                {
                    return RET_FAIL;
                }

                using (var db = new SystemCommonContext())
                {
                    // システム日時
                    var systemDate = DateUtil.GetSysDateTime();

                    using (var tx = db.Database.BeginTransaction())
                    {
                        // 更新対象のデータを取得しつつデータロックする
                        var tBatchYoyaku = GetBatchYoyaku(db, param);

                        // 取得できなかった場合
                        if (tBatchYoyaku == null)
                        {
                            tx.Rollback();
                            message = SystemMessageUtil.Get("ME80009", "バッチID", param.BatchId.ToString());
                            return RET_FAIL;
                        }

                        // [引数：ステータス]が"03"（成功）の場合
                        if (BATCH_STATUS_SUCCESS.Equals(param.Status))
                        {
                            // ステータスが"02"（処理中）の場合
                            if (BATCH_STATUS_RUNNING.Equals(tBatchYoyaku.BatchStatus))
                            {
                                // バッチステータス
                                tBatchYoyaku.BatchStatus = param.Status;
                                // バッチ完了日時
                                tBatchYoyaku.BatchEndDate = systemDate;
                                // エラー情報
                                tBatchYoyaku.ErrorInfo = null;
                                // 更新ユーザID
                                tBatchYoyaku.UpdateUserId = param.UpdateUserId;
                                // 更新日時
                                tBatchYoyaku.UpdateDate = systemDate;
                            }
                            else
                            {
                                // ステータスが"02"（処理中）以外の場合
                                tx.Rollback();
                                message = SystemMessageUtil.Get("ME80007", "ステータス", tBatchYoyaku.BatchStatus, param.Status);
                                return RET_FAIL;
                            }
                        }
                        else if (BATCH_STATUS_CANCEL.Equals(param.Status))
                        {
                            // [引数：ステータス]が"11"（キャンセル）の場合

                            // ステータスが"01"（処理待ち）の場合
                            if (BATCH_STATUS_WAITING.Equals(tBatchYoyaku.BatchStatus))
                            {
                                // バッチステータス
                                tBatchYoyaku.BatchStatus = param.Status;
                                // 更新ユーザID
                                tBatchYoyaku.UpdateUserId = param.UpdateUserId;
                                // 更新日時
                                tBatchYoyaku.UpdateDate = systemDate;
                            }
                            else
                            {
                                // ステータスが"01"（処理待ち）以外の場合
                                tx.Rollback();
                                message = SystemMessageUtil.Get("ME80007", "ステータス", tBatchYoyaku.BatchStatus, param.Status);
                                return RET_FAIL;
                            }

                        }
                        else if (BATCH_STATUS_DELETE.Equals(param.Status))
                        {
                            // [引数：ステータス]が"98"（削除）の場合

                            // ステータスが"01"（処理待ち）、"02"（処理中）の場合
                            if (BATCH_STATUS_WAITING.Equals(tBatchYoyaku.BatchStatus) || BATCH_STATUS_RUNNING.Equals(tBatchYoyaku.BatchStatus))
                            {
                                tx.Rollback();
                                message = SystemMessageUtil.Get("ME80008", "ステータス", tBatchYoyaku.BatchStatus);
                                return RET_FAIL;
                            }
                            else
                            {
                                // ステータスが"01"（処理待ち）、"02"（処理中）のいずれでもない場合

                                // 削除フラグ
                                tBatchYoyaku.DeleteFlg = DELETE_FLG_DELETED;
                                // 更新ユーザID
                                tBatchYoyaku.UpdateUserId = param.UpdateUserId;
                                // 更新日時
                                tBatchYoyaku.UpdateDate = systemDate;
                            }
                        }
                        else if (BATCH_STATUS_ERROR.Equals(param.Status))
                        {
                            // [引数：ステータス]が"99"（エラー）の場合

                            // バッチステータス
                            tBatchYoyaku.BatchStatus = param.Status;
                            // バッチ完了日時
                            tBatchYoyaku.BatchEndDate = systemDate;
                            // エラー情報
                            tBatchYoyaku.ErrorInfo = param.ErrInfo;
                            // 更新ユーザID
                            tBatchYoyaku.UpdateUserId = param.UpdateUserId;
                            // 更新日時
                            tBatchYoyaku.UpdateDate = systemDate;
                        }

                        db.Entry(tBatchYoyaku).State = EntityState.Modified;

                        try
                        {
                            logger.Info("バッチ実行状況を更新します。");
                            db.SaveChanges();
                            tx.Commit();
                        }
                        catch (DbUpdateException e)
                        {
                            // 失敗の場合
                            logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                            message = SystemMessageUtil.Get("MF80001");
                            tx.Rollback();
                            return RET_FAIL;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                message = SystemMessageUtil.Get("MF80001");
                return RET_FAIL;
            }

            return RET_SUCCESS;
        }
        #endregion

        #region バッチダウンロードファイル登録
        /// <summary>
        /// バッチダウンロードファイル登録
        /// </summary>
        /// <param name="lngBatchId">バッチID</param>
        /// <param name="strFilePath">ファイルパス</param>
        /// <param name="strHash">ハッシュ</param>
        /// <param name="strFileNm">ファイル名</param>
        /// <param name="strInsertUserId">登録ユーザID</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>処理成否</returns>
        public static int InsertBatchDownloadFile(long lngBatchId,
                                                    string strFilePath,
                                                    string strHash,
                                                    string strFileNm,
                                                    string strInsertUserId,
                                                    ref string message)
        {
            message = null;

            var param = new InsertBatchDownloadFileParam
            {
                BatchId = lngBatchId,
                FilePath = strFilePath,
                Hash = strHash,
                FileNm = strFileNm,
                InsertUserId = strInsertUserId,
            };

            // 必須チェック
            if (!CheckInsertBatchDownloadFileParamRequired(param, ref message))
            {
                return RET_FAIL;
            }

            // 桁数チェック
            if (!CheckInsertBatchDownloadFileParamMaxLength(param, ref message))
            {
                return RET_FAIL;
            }

            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // sql用定数定義
            var sql =
                "INSERT INTO t_batch_download_file (" +
                "  batch_id, " +
                "  renban, " +
                "  file_path, " +
                "  hash, " +
                "  file_nm, " +
                "  insert_user_id, " +
                "  insert_date, " +
                "  update_user_id, " +
                "  update_date " +
                ") VALUES (" +
                "  @BatchId_1, " +
                "  ( " +
                "    SELECT coalesce(MAX(renban), 0) + 1 " +
                "    FROM t_batch_download_file " +
                "    WHERE batch_id = @BatchId_2 " +
                "  ), " +
                "  @FilePath, " +
                "  @Hash, " +
                "  @FileNm, " +
                "  @InsertUserId, " +
                "  @InsertDate, " +
                "  @UpdateUserId, " +
                "  @UpdateDate " +
                ") ";

            var parameters = new List<NpgsqlParameter>
            {
                new("@BatchId_1", param.BatchId),
                new("@BatchId_2", param.BatchId),
                new("@FilePath", param.FilePath),
                new("@Hash", param.Hash),
                new("@FileNm", param.FileNm),
                new("@InsertUserId", param.InsertUserId),
                new("@InsertDate", systemDate),
                new("@UpdateUserId", param.InsertUserId),
                new("@UpdateDate", systemDate),
            };

            using var db = new SystemCommonContext();
            using var tx = db.Database.BeginTransaction();

            try
            {
                logger.Info("バッチダウンロードファイルを登録します。");
                db.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());

                tx.Commit();
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                message = SystemMessageUtil.Get("MF80001");
                tx.Rollback();
                return RET_FAIL;
            }

            return RET_SUCCESS;
        }
        #endregion

        #region 引数チェック（バッチ予約登録）
        /// <summary>
        /// 必須チェック（バッチ予約登録）
        /// </summary>
        /// <param name="param">バッチ予約登録引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertBatchYoyakuParamRequired(InsertBatchYoyakuParam param, ref string message)
        {
            // バッチ分類
            if (!CommonFuncUtil.ValidateRequired(param.BatchBunrui, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchBunrui)), ref message))
            {
                return false;
            }

            // システム区分
            if (!CommonFuncUtil.ValidateRequired(param.SystemKbn, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)), ref message))
            {
                return false;
            }

            // 都道府県コード
            if (!CommonFuncUtil.ValidateRequired(param.TodofukenCd, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.TodofukenCd)), ref message))
            {
                return false;
            }

            // 予約日時
            if (param.YoyakuDate == null)
            {
                message = SystemMessageUtil.Get("ME80004", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.YoyakuDate)));
                return false;
            }

            // 予約ユーザID
            if (!CommonFuncUtil.ValidateRequired(param.YoyakuUserId, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.YoyakuUserId)), ref message))
            {
                return false;
            }

            // 予約を実行した処理名
            if (!CommonFuncUtil.ValidateRequired(param.YoyakuShoriNm, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.YoyakuShoriNm)), ref message))
            {
                return false;
            }

            // バッチ名
            if (!CommonFuncUtil.ValidateRequired(param.BatchNm, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchNm)), ref message))
            {
                return false;
            }

            // 複数実行禁止フラグ
            if (!CommonFuncUtil.ValidateRequired(param.MultiRunNgFlg, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.MultiRunNgFlg)), ref message))
            {
                return false;
            }

            // バッチ種類（巡回、定時）
            if (!CommonFuncUtil.ValidateRequired(param.BatchType, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchType)), ref message))
            {
                return false;
            }

            // ロック対象
            if (!CommonFuncUtil.ValidateRequired(param.LockTargetFlg, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.LockTargetFlg)), ref message))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 桁数チェック（バッチ予約登録）
        /// </summary>
        /// <param name="param">バッチ予約登録引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertBatchYoyakuParamMaxLength(InsertBatchYoyakuParam param, ref string message)
        {
            var obj = new TBatchYoyaku();

            // バッチ分類
            if (!CommonFuncUtil.ValidateMaxLength(param.BatchBunrui, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.BatchBunrui)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchBunrui)), ref message))
            {
                return false;
            }

            // システム区分
            if (!CommonFuncUtil.ValidateMaxLength(param.SystemKbn, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.SystemKbn)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)), ref message))
            {
                return false;
            }

            // 都道府県コード
            if (!CommonFuncUtil.ValidateMaxLength(param.TodofukenCd, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.TodofukenCd)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.TodofukenCd)), ref message))
            {
                return false;
            }

            // 組合等コード
            if (!CommonFuncUtil.ValidateMaxLength(param.KumiaitoCd, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.KumiaitoCd)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.KumiaitoCd)), ref message))
            {
                return false;
            }

            // 支所コード
            if (!CommonFuncUtil.ValidateMaxLength(param.ShishoCd, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.ShishoCd)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.ShishoCd)), ref message))
            {
                return false;
            }

            // 予約ユーザID
            if (!CommonFuncUtil.ValidateMaxLength(param.YoyakuUserId, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.BatchYoyakuId)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.YoyakuUserId)), ref message))
            {
                return false;
            }

            // 予約を実行した処理名
            if (!CommonFuncUtil.ValidateMaxLength(param.YoyakuShoriNm, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.BatchYoyakuShori)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.YoyakuShoriNm)), ref message))
            {
                return false;
            }

            // バッチ名
            if (!CommonFuncUtil.ValidateMaxLength(param.BatchNm, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.BatchNm)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchNm)), ref message))
            {
                return false;
            }

            // バッチ条件
            if (!CommonFuncUtil.ValidateMaxLength(param.BatchJoken, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.BatchJoken)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchJoken)), ref message))
            {
                return false;
            }

            // 複数実行禁止フラグ
            if (!CommonFuncUtil.ValidateMaxLength(param.MultiRunNgFlg, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.MultiRunNgFlg)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.MultiRunNgFlg)), ref message))
            {
                return false;
            }

            // 複数実行禁止ID
            if (!CommonFuncUtil.ValidateMaxLength(param.MultiRunNgId, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.MultiRunNgId)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.MultiRunNgId)), ref message))
            {
                return false;
            }

            // バッチ種類
            if (!CommonFuncUtil.ValidateMaxLength(param.BatchType, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.BatchType)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchType)), ref message))
            {
                return false;
            }

            // ロック対象
            if (!CommonFuncUtil.ValidateMaxLength(param.LockTargetFlg, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.LockTargetFlg)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.LockTargetFlg)), ref message))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// マスタ整合性チェック（バッチ予約登録）
        /// </summary>
        /// <param name="param">バッチ予約登録引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertBatchYoyakuParamMaster(InsertBatchYoyakuParam param, ref string message)
        {
            using (var db = new SystemCommonContext())
            {
                // バッチ分類：汎用区分マスタ「batch_bunrui」
                var mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == "batch_bunrui" && m.KbnCd == param.BatchBunrui).SingleOrDefault();
                if (mHanyokubun == null)
                {
                    message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchBunrui)), "マスタ設定に含まれるコード");
                    return false;
                }

                // システム区分：汎用区分マスタ「system_kbn」
                mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == "system_kbn" && m.KbnCd == param.SystemKbn).SingleOrDefault();
                if (mHanyokubun == null)
                {
                    message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)), "マスタ設定に含まれるコード");
                    return false;
                }

                // 都道府県コード：都道府県マスタ
                var mTodofuken = db.MTodofukens.Where(m => m.TodofukenCd == param.TodofukenCd).SingleOrDefault();
                if (mTodofuken == null)
                {
                    message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.TodofukenCd)), "マスタ設定に含まれるコード");
                    return false;
                }

                // 組合等コード：組合等マスタ
                if (!string.IsNullOrEmpty(param.KumiaitoCd))
                {
                    var mKumiaito = db.MKumiaitos.Where(m => m.TodofukenCd == param.TodofukenCd && m.KumiaitoCd == param.KumiaitoCd).SingleOrDefault();
                    if (mKumiaito == null)
                    {
                        message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.TodofukenCd)), "マスタ設定に含まれるコード");
                        return false;
                    }
                }

                // 支所コード：名称M支所
                if (!string.IsNullOrEmpty(param.KumiaitoCd) && !string.IsNullOrEmpty(param.ShishoCd))
                {
                    var mShishoNm = db.MShishoNms.Where(m => m.TodofukenCd == param.TodofukenCd &&
                                                            m.KumiaitoCd == param.KumiaitoCd &&
                                                            m.ShishoCd == param.ShishoCd).SingleOrDefault();
                    if (mShishoNm == null)
                    {
                        message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.ShishoCd)), "マスタ設定に含まれるコード");
                        return false;
                    }
                }

                // 複数実行禁止フラグ：汎用区分マスタ「multi_run_ng_flg」	
                mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == "multi_run_ng_flg" && m.KbnCd == param.MultiRunNgFlg).SingleOrDefault();
                if (mHanyokubun == null)
                {
                    message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.MultiRunNgFlg)), "マスタ設定に含まれるコード");
                    return false;
                }

                // バッチ種類：汎用区分マスタ「batch_type」	
                mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == "batch_type" && m.KbnCd == param.BatchType).SingleOrDefault();
                if (mHanyokubun == null)
                {
                    message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchType)), "マスタ設定に含まれるコード");
                    return false;
                }

                // ロック対象フラグ：汎用区分マスタ「lock_target_flg」	
                mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == "lock_target_flg" && m.KbnCd == param.LockTargetFlg).SingleOrDefault();
                if (mHanyokubun == null)
                {
                    message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.LockTargetFlg)), "マスタ設定に含まれるコード");
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 引数チェック（実行可能バッチ予約取得）
        /// <summary>
        /// 必須チェック（実行可能バッチ予約取得）
        /// </summary>
        /// <param name="param">実行可能バッチ予約取得引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckGetRunBatchYoyakuParamRequired(GetRunBatchYoyakuParam param, ref string message)
        {
            // 実行サーバ
            if (!CommonFuncUtil.ValidateRequired(param.BatchRunServer, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchRunServer)), ref message))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 桁数チェック（実行可能バッチ予約取得）
        /// </summary>
        /// <param name="param">実行可能バッチ予約取得引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckGetRunBatchYoyakuParamMaxLength(GetRunBatchYoyakuParam param, ref string message)
        {
            var obj = new TBatchYoyaku();

            // システム区分
            if (!CommonFuncUtil.ValidateMaxLength(param.SystemKbn, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.SystemKbn)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)), ref message))
            {
                return false;
            }

            // バッチ名
            if (!CommonFuncUtil.ValidateMaxLength(param.BatchNm, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.BatchNm)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchNm)), ref message))
            {
                return false;
            }

            // 実行サーバ
            if (!CommonFuncUtil.ValidateMaxLength(param.BatchRunServer, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.BatchRunServer)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.BatchRunServer)), ref message))
            {
                return false;
            }

            // 更新ユーザID
            if (!CommonFuncUtil.ValidateMaxLength(param.UpdateUserId, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.UpdateUserId)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.UpdateUserId)), ref message))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// マスタ整合性チェック（実行可能バッチ予約取得）
        /// </summary>
        /// <param name="param">実行可能バッチ予約取得引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckGetRunBatchYoyakuParamMaster(GetRunBatchYoyakuParam param, ref string message)
        {
            if (!string.IsNullOrEmpty(param.SystemKbn))
            {
                using (var db = new SystemCommonContext())
                {
                    // システム区分：汎用区分マスタ「system_kbn」
                    var mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == "system_kbn" && m.KbnCd == param.SystemKbn).SingleOrDefault();
                    if (mHanyokubun == null)
                    {
                        message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)), "マスタ設定に含まれるコード");
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 独自チェック（実行可能バッチ予約取得）
        /// </summary>
        /// <param name="param">実行可能バッチ予約取得引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckGetRunBatchYoyakuParamCustom(GetRunBatchYoyakuParam param, ref string message)
        {
            // [引数：システム区分]の値について、設定なし（nullまたは空白）かつ、バッチ名が「CSV出力」ではない場合
            if (string.IsNullOrEmpty(param.SystemKbn) && !BATCH_NM_CSV.Equals(param.BatchNm))
            {
                message = SystemMessageUtil.Get("ME80004", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)));
                return false;
            }

            return true;
        }
        #endregion

        #region 引数チェック（バッチ実行状況更新）
        /// <summary>
        /// 必須チェック（バッチ実行状況更新）
        /// </summary>
        /// <param name="param">バッチ実行状況更新引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckUpdateBatchYoyakuStsParamRequired(UpdateBatchYoyakuStsParam param, ref string message)
        {
            // ステータス
            if (!CommonFuncUtil.ValidateRequired(param.Status, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.Status)), ref message))
            {
                return false;
            }

            // 更新ユーザID
            if (!CommonFuncUtil.ValidateRequired(param.UpdateUserId, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.UpdateUserId)), ref message))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// マスタ整合性チェック（バッチ実行状況更新）
        /// </summary>
        /// <param name="param">バッチ実行状況更新引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>

        private static bool CheckUpdateBatchYoyakuStsParamMaster(UpdateBatchYoyakuStsParam param, ref string message)
        {
            using (var db = new SystemCommonContext())
            {
                // バッチ分類：汎用区分マスタ「batch_status」
                var mHanyokubun = db.MHanyokubuns.Where(m => m.KbnSbt == "batch_status" && m.KbnCd == param.Status).SingleOrDefault();
                if (mHanyokubun == null)
                {
                    message = SystemMessageUtil.Get("ME80006", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.Status)), "マスタ設定に含まれるコード");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 独自チェック（バッチ実行状況更新）
        /// </summary>
        /// <param name="param">バッチ実行状況更新引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckUpdateBatchYoyakuStsParamCustom(UpdateBatchYoyakuStsParam param, ref string message)
        {
            // [引数：ステータス]が99（エラー）かつ、[引数：エラー情報]が設定なしの場合
            if (BATCH_STATUS_ERROR.Equals(param.Status) && string.IsNullOrEmpty(param.ErrInfo))
            {
                message = SystemMessageUtil.Get("ME80004", CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.ErrInfo)));
                return false;
            }

            return true;
        }

        #endregion

        #region 引数チェック（バッチダウンロードファイル登録）
        /// <summary>
        /// 必須チェック（バッチダウンロードファイル登録）
        /// </summary>
        /// <param name="param">バッチ実行状況更新引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertBatchDownloadFileParamRequired(InsertBatchDownloadFileParam param, ref string message)
        {
            // ファイルパス
            if (!CommonFuncUtil.ValidateRequired(param.FilePath, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.FilePath)), ref message))
            {
                return false;
            }

            // ハッシュ
            if (!CommonFuncUtil.ValidateRequired(param.Hash, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.Hash)), ref message))
            {
                return false;
            }

            // ファイル名
            if (!CommonFuncUtil.ValidateRequired(param.FileNm, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.FileNm)), ref message))
            {
                return false;
            }

            // 登録ユーザID
            if (!CommonFuncUtil.ValidateRequired(param.InsertUserId, CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.InsertUserId)), ref message))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 桁数チェック（バッチダウンロードファイル登録）
        /// </summary>
        /// <param name="param">バッチダウンロードファイル登録</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertBatchDownloadFileParamMaxLength(InsertBatchDownloadFileParam param, ref string message)
        {
            var obj = new TBatchDownloadFile();

            // ファイルパス
            if (!CommonFuncUtil.ValidateMaxLength(param.FilePath, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.FilePath)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.FilePath)), ref message))
            {
                return false;
            }

            // ハッシュ
            if (!CommonFuncUtil.ValidateMaxLength(param.Hash, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.Hash)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.Hash)), ref message))
            {
                return false;
            }

            // ファイル名
            if (!CommonFuncUtil.ValidateMaxLength(param.FileNm, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.FileNm)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.FileNm)), ref message))
            {
                return false;
            }

            // 登録ユーザID
            if (!CommonFuncUtil.ValidateMaxLength(param.InsertUserId, CommonFuncUtil.GetMaximumLength(obj.GetType(), nameof(obj.InsertUserId)),
                                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.InsertUserId)), ref message))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region バッチ予約登録
        /// <summary>
        /// バッチ予約登録
        /// </summary>
        /// <param name="param">バッチ予約登録引数</param>
        /// <param name="batchId">バッチID(出力パラメータ)</param>
        private static void InsertTBatchYoyaku(InsertBatchYoyakuParam param, ref long batchId)
        {
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            using (var db = new SystemCommonContext())
            {
                using (var tx = db.Database.BeginTransaction())
                {

                    // バッチID取得
                    var seqBatchId = DBUtil.NextSeqVal(db, "seq_batch_id");

                    TBatchYoyaku yoyaku = new()
                    {
                        // バッチID
                        BatchId = seqBatchId,
                        // バッチ分類
                        BatchBunrui = param.BatchBunrui,
                        // システム区分
                        SystemKbn = param.SystemKbn,
                        // 都道府県コード
                        TodofukenCd = param.TodofukenCd,
                        // 組合等コード
                        KumiaitoCd = param.KumiaitoCd,
                        // 支所コード
                        ShishoCd = param.ShishoCd,
                        // 予約日時
                        BatchYoyakuDate = param.YoyakuDate,
                        // 予約ユーザID
                        BatchYoyakuId = param.YoyakuUserId,
                        // 予約を実行した処理名
                        BatchYoyakuShori = param.YoyakuShoriNm,
                        // バッチ名
                        BatchNm = param.BatchNm,
                        // バッチ条件
                        BatchJoken = param.BatchJoken,
                        // バッチ条件（画面表示用）
                        BatchJokenDisp = param.BatchJokenDisp,
                        // 複数実行禁止フラグ
                        MultiRunNgFlg = param.MultiRunNgFlg,
                        // 複数実行禁止ID
                        MultiRunNgId = param.MultiRunNgId,
                        // バッチ種類
                        BatchType = param.BatchType,
                        // 定時実行バッチ予約日時
                        BatchScheDatetime = param.BatchScheDatetime,
                        // ロック対象フラグ
                        LockTargetFlg = param.LockTargetFlg,
                        // バッチステータス（処理待ち）
                        BatchStatus = BATCH_STATUS_WAITING,
                        // バッチ開始日時
                        BatchStartDate = null,
                        // バッチ完了日時
                        BatchEndDate = null,
                        // 実行サーバ
                        BatchRunServer = null,
                        // エラー情報
                        ErrorInfo = null,
                        // 削除フラグ
                        DeleteFlg = DELETE_FLG_NOT_DELETED,
                        // 登録ユーザID
                        InsertUserId = param.YoyakuUserId,
                        // 登録日時
                        InsertDate = systemDate,
                        // 更新ユーザID
                        UpdateUserId = param.YoyakuUserId,
                        // 更新日時
                        UpdateDate = systemDate
                    };

                    db.TBatchYoyakus.Add(yoyaku);

                    batchId = seqBatchId;

                    try
                    {
                        logger.Info(string.Format("バッチID：{0}を登録します。", seqBatchId));
                        db.SaveChanges();
                        tx.Commit();
                    }
                    catch (DbUpdateException e)
                    {
                        // 失敗の場合
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region バッチ予約状況取得
        /// <summary>
        /// バッチ予約件数取得
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="param">バッチ予約状況取得引数</param>
        /// <returns>バッチ予約件数</returns>
        private static int GetBatchYoyakuListCount(SystemCommonContext db, GetBatchYoyakuListParam param)
        {
            // sql用定数定義
            var cntSql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 件数取得
            cntSql.Append(
                "SELECT COUNT(1) AS \"Value\" " +
                "FROM t_batch_yoyaku ");
            // 検索条件を取得する
            GetSearchCondition(cntSql, param, parameters);

            // sql実行 
            return db.Database.SqlQueryRaw<int>(cntSql.ToString(), parameters.ToArray()).Single();
        }

        /// <summary>
        /// バッチ予約状況取得
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="param">バッチ予約状況取得引数</param>
        /// <returns>バッチ予約状況一覧</returns>
        private static List<BatchYoyaku> GetBatchYoyakuListPageData(SystemCommonContext db, GetBatchYoyakuListParam param)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetTableCondition(sql);
            // 検索条件を取得する
            GetSearchCondition(sql, param, parameters);
            // 検索結果を取得する
            GetResult(sql);

            // 検索条件
            sql.Append("WHERE '1' = '1' ");

            // 引数：取得件数開始、取得件数終了のいずれかが0の場合は設定しない
            if (!(param.CntStart == null || param.CntEnd == null || param.CntStart == 0 || param.CntEnd == 0))
            {
                // 取得件数開始
                sql.Append("AND No >= @CntStart ");
                parameters.Add(new NpgsqlParameter("@CntStart", param.CntStart));

                // 取得件数終了
                sql.Append("AND No <= @CntEnd ");
                parameters.Add(new NpgsqlParameter("@CntEnd", param.CntEnd));
            }

            // ソート順を追加する
            sql.Append(
                "ORDER BY " +
                "a.batch_id DESC, " +
                "a.batch_yoyaku_date DESC, " +
                "a.renban ASC ");

            return db.Database.SqlQueryRaw<BatchYoyaku>(sql.ToString(), parameters.ToArray()).ToList();
        }

        /// <summary>
        /// 検索項目を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private static void GetCondition(StringBuilder sql)
        {
            sql.Append(
                "WITH a AS (  " +
                "SELECT " +
                "  dense_rank() OVER (ORDER BY t_batch_yoyaku.batch_id DESC, t_batch_yoyaku.batch_yoyaku_date DESC) As No, " +
                "  t_batch_yoyaku.batch_id, " +
                "  t_batch_yoyaku.batch_bunrui, " +
                "  t_batch_yoyaku.system_kbn, " +
                "  m_hanyokubun.kbn_nm, " +
                "  t_batch_yoyaku.todofuken_cd, " +
                "  m_todofuken.todofuken_nm, " +
                "  t_batch_yoyaku.kumiaito_cd, " +
                "  m_kumiaito.kumiaito_nm, " +
                "  t_batch_yoyaku.shisho_cd, " +
                "  m_shisho_nm.shisho_nm, " +
                "  t_batch_yoyaku.batch_yoyaku_date, " +
                "  t_batch_yoyaku.batch_yoyaku_id, " +
                "  t_batch_yoyaku.batch_yoyaku_shori, " +
                "  t_batch_yoyaku.batch_nm, " +
                "  t_batch_yoyaku.batch_joken, " +
                "  t_batch_yoyaku.batch_joken_disp, " +
                "  t_batch_yoyaku.multi_run_ng_flg, " +
                "  t_batch_yoyaku.multi_run_ng_id, " +
                "  t_batch_yoyaku.batch_type, " +
                "  t_batch_yoyaku.batch_sche_datetime, " +
                "  t_batch_yoyaku.lock_target_flg, " +
                "  t_batch_yoyaku.batch_status, " +
                "  t_batch_yoyaku.batch_start_date, " +
                "  t_batch_yoyaku.batch_end_date, " +
                "  t_batch_yoyaku.batch_run_server, " +
                "  t_batch_yoyaku.error_info, " +
                "  t_batch_download_file.renban, " +
                "  t_batch_download_file.file_path, " +
                "  t_batch_download_file.hash, " +
                "  t_batch_download_file.file_nm, " +
                "  t_batch_yoyaku.insert_user_id, " +
                "  t_batch_yoyaku.insert_date, " +
                "  t_batch_yoyaku.update_user_id, " +
                "  t_batch_yoyaku.update_date ");
        }

        /// <summary>
        /// 検索テーブルを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private static void GetTableCondition(StringBuilder sql)
        {
            sql.Append(
                "FROM " +
                "  t_batch_yoyaku " +
                "LEFT JOIN t_batch_download_file " +
                "  ON t_batch_yoyaku.batch_id = t_batch_download_file.batch_id " +
                "LEFT JOIN m_hanyokubun " +
                "  ON m_hanyokubun.kbn_sbt = 'system_kbn' " +
                "  AND t_batch_yoyaku.system_kbn = m_hanyokubun.kbn_cd " +
                "INNER JOIN m_todofuken " +
                "  ON t_batch_yoyaku.todofuken_cd= m_todofuken.todofuken_cd " +
                "INNER JOIN m_kumiaito " +
                "  ON t_batch_yoyaku.todofuken_cd= m_kumiaito.todofuken_cd " +
                "  AND t_batch_yoyaku.kumiaito_cd= m_kumiaito.kumiaito_cd " +
                "LEFT JOIN m_shisho_nm " +
                "  ON t_batch_yoyaku.todofuken_cd = m_shisho_nm.todofuken_cd " +
                "  AND t_batch_yoyaku.kumiaito_cd = m_shisho_nm.kumiaito_cd " +
                "  AND t_batch_yoyaku.shisho_cd = m_shisho_nm.shisho_cd ");
        }

        /// <summary>
        /// 検索結果を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private static void GetResult(StringBuilder sql)
        {
            sql.Append(
                ") " +
                "SELECT " +
                "  a.batch_id AS BatchId, " +
                "  a.batch_bunrui AS BatchBunrui, " +
                "  a.system_kbn AS SystemKbn, " +
                "  a.kbn_nm AS SystemKbnNm, " +
                "  a.todofuken_cd AS TodofukenCd, " +
                "  a.todofuken_nm AS TodofukenNm, " +
                "  a.kumiaito_cd AS KumiaitoCd, " +
                "  a.kumiaito_nm AS KumiaitoNm, " +
                "  a.shisho_cd AS ShishoCd, " +
                "  a.shisho_nm AS ShishoNm, " +
                "  a.batch_yoyaku_date AS BatchYoyakuDate, " +
                "  a.batch_yoyaku_id AS BatchYoyakuId, " +
                "  a.batch_yoyaku_shori AS BatchYoyakuShori, " +
                "  a.batch_nm AS BatchNm, " +
                "  a.batch_joken AS BatchJoken, " +
                "  a.batch_joken_disp AS BatchJokenDisp, " +
                "  a.multi_run_ng_flg AS MultiRunNgFlg, " +
                "  a.multi_run_ng_id AS MultiRunNgId, " +
                "  a.batch_type AS BatchType, " +
                "  a.batch_sche_datetime AS BatchScheDatetime, " +
                "  a.lock_target_flg AS LockTargetFlg, " +
                "  a.batch_status AS BatchStatus, " +
                "  a.batch_start_date AS BatchStartDate, " +
                "  a.batch_end_date AS BatchEndDate, " +
                "  a.batch_run_server AS BatchRunServer, " +
                "  a.error_info AS ErrorInfo, " +
                "  a.renban AS Renban, " +
                "  a.file_path AS FilePath, " +
                "  a.hash AS Hash, " +
                "  a.file_nm AS FileNm, " +
                "  a.insert_user_id AS InsertUserId, " +
                "  a.insert_date AS InsertDate, " +
                "  a.update_user_id AS UpdateUserId, " +
                "  a.update_date AS UpdateDate " +
                "FROM " +
                "  a ");
        }
        /// <summary>
        /// 検索条件を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="param">バッチ予約状況取得引数</param>
        /// <param name="parameters">パラメータ</param>
        private static void GetSearchCondition(StringBuilder sql, GetBatchYoyakuListParam param, List<NpgsqlParameter> parameters)
        {
            sql.Append("WHERE '1' = '1'");

            // バッチID
            if (param.BatchId != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_id = @BatchId ");
                parameters.Add(new NpgsqlParameter("@BatchId", param.BatchId));
            }

            // バッチ分類
            if (!string.IsNullOrEmpty(param.BatchBunrui))
            {
                sql.Append("AND t_batch_yoyaku.batch_bunrui = @BatchBunrui ");
                parameters.Add(new NpgsqlParameter("@BatchBunrui", param.BatchBunrui));
            }

            // システム区分
            if (!string.IsNullOrEmpty(param.SystemKbn))
            {
                sql.Append("AND t_batch_yoyaku.system_kbn = @SystemKbn ");
                parameters.Add(new NpgsqlParameter("@SystemKbn", param.SystemKbn));
            }

            // 都道府県コード
            if (!string.IsNullOrEmpty(param.TodofukenCd))
            {
                sql.Append("AND t_batch_yoyaku.todofuken_cd = @TodofukenCd ");
                parameters.Add(new NpgsqlParameter("@TodofukenCd", param.TodofukenCd));
            }

            // 組合等コード
            if (!string.IsNullOrEmpty(param.KumiaitoCd))
            {
                sql.Append("AND t_batch_yoyaku.kumiaito_cd = @KumiaitoCd ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd", param.KumiaitoCd));
            }

            // 支所コード
            if (!string.IsNullOrEmpty(param.ShishoCd))
            {
                sql.Append("AND t_batch_yoyaku.shisho_cd = ANY (@ShishoList) ");
                parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = new List<string>(param.ShishoCd.Split(",")),
                });
            }

            // 予約日時（開始）
            if (param.YoyakuDateFrom != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_yoyaku_date >= @YoyakuDateFrom ");
                parameters.Add(new NpgsqlParameter("@YoyakuDateFrom", param.YoyakuDateFrom));
            }

            // 予約日時（終了）
            if (param.YoyakuDateTo != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_yoyaku_date <= @YoyakuDateTo ");
                parameters.Add(new NpgsqlParameter("@YoyakuDateTo", param.YoyakuDateTo));
            }

            // 予約ユーザID
            if (!string.IsNullOrEmpty(param.YoyakuUserId))
            {
                sql.Append("AND t_batch_yoyaku.batch_yoyaku_id = @YoyakuUserId ");
                parameters.Add(new NpgsqlParameter("@YoyakuUserId", param.YoyakuUserId));
            }

            // 予約を実行した処理名
            if (!string.IsNullOrEmpty(param.YoyakuShoriNm))
            {
                sql.Append("AND t_batch_yoyaku.batch_yoyaku_shori = @YoyakuShoriNm ");
                parameters.Add(new NpgsqlParameter("@YoyakuShoriNm", param.YoyakuShoriNm));
            }

            // バッチ名
            if (!string.IsNullOrEmpty(param.BatchNm))
            {
                sql.Append("AND t_batch_yoyaku.batch_nm = @BatchNm ");
                parameters.Add(new NpgsqlParameter("@BatchNm", param.BatchNm));
            }

            // バッチ条件
            if (!string.IsNullOrEmpty(param.BatchJoken))
            {
                sql.Append("AND t_batch_yoyaku.batch_joken = @BatchJoken ");
                parameters.Add(new NpgsqlParameter("@BatchJoken", param.BatchJoken));
            }

            // バッチ条件（表示用）
            if (!string.IsNullOrEmpty(param.BatchJokenDisp))
            {
                sql.Append("AND t_batch_yoyaku.batch_joken_disp = @BatchJokenDisp ");
                parameters.Add(new NpgsqlParameter("@BatchJokenDisp", param.BatchJokenDisp));
            }

            // 複数実行禁止フラグ
            if (!string.IsNullOrEmpty(param.MultiRunNgFlg))
            {
                sql.Append("AND t_batch_yoyaku.multi_run_ng_flg = @MultiRunNgFlg ");
                parameters.Add(new NpgsqlParameter("@MultiRunNgFlg", param.MultiRunNgFlg));
            }

            // 複数実行禁止ID
            if (!string.IsNullOrEmpty(param.MultiRunNgId))
            {
                sql.Append("AND t_batch_yoyaku.multi_run_ng_id = @MultiRunNgId ");
                parameters.Add(new NpgsqlParameter("@MultiRunNgId", param.MultiRunNgId));
            }

            // バッチ種類
            if (!string.IsNullOrEmpty(param.BatchType))
            {
                sql.Append("AND t_batch_yoyaku.batch_type = @BatchType ");
                parameters.Add(new NpgsqlParameter("@BatchType", param.BatchType));
            }

            // 定時実行バッチ予約日時（開始）
            if (param.BatchScheDatetimeFrom != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_sche_datetime >= @BatchScheDatetimeFrom ");
                parameters.Add(new NpgsqlParameter("@BatchScheDatetimeFrom", param.BatchScheDatetimeFrom));
            }

            // 定時実行バッチ予約日時（終了）
            if (param.BatchScheDatetimeTo != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_sche_datetime <= @BatchScheDatetimeTo ");
                parameters.Add(new NpgsqlParameter("@BatchScheDatetimeTo", param.BatchScheDatetimeTo));
            }

            // ロック対象フラグ
            if (!string.IsNullOrEmpty(param.LockTargetFlg))
            {
                sql.Append("AND t_batch_yoyaku.lock_target_flg = @LockTargetFlg ");
                parameters.Add(new NpgsqlParameter("@LockTargetFlg", param.LockTargetFlg));
            }

            // バッチ状態
            if (!string.IsNullOrEmpty(param.BatchStatus))
            {
                sql.Append("AND t_batch_yoyaku.batch_status = ANY (@BatchStatus) ");
                parameters.Add(new NpgsqlParameter("@BatchStatus", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = param.BatchStatus.Split(',').ToList()
                });
            }

            // 開始日時（開始）
            if (param.BatchStartDateFrom != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_start_date >= @BatchStartDateFrom ");
                parameters.Add(new NpgsqlParameter("@BatchStartDateFrom", param.BatchStartDateFrom));
            }

            // 開始日時（終了）
            if (param.BatchStartDateTo != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_start_date <= @BatchStartDateTo ");
                parameters.Add(new NpgsqlParameter("@BatchStartDateTo", param.BatchStartDateTo));
            }

            // 完了日時（開始）
            if (param.BatchEndDateFrom != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_end_date >= @BatchEndDateFrom ");
                parameters.Add(new NpgsqlParameter("@BatchEndDateFrom", param.BatchEndDateFrom));
            }

            // 完了日時（終了）
            if (param.BatchEndDateTo != null)
            {
                sql.Append("AND t_batch_yoyaku.batch_end_date <= @BatchEndDateTo ");
                parameters.Add(new NpgsqlParameter("@BatchEndDateTo", param.BatchEndDateTo));
            }

            sql.Append("AND t_batch_yoyaku.delete_flg = '0' ");
        }
        #endregion

        /// <summary>
        /// 現在時刻で実行可能なバッチ種類を取得する
        /// </summary>
        /// <param name="db">システム共通DBコンテキスト</param>
        /// <param name="param">実行可能バッチ予約取得引数</param>
        /// <param name="systemDate">システム日時</param>
        /// <returns></returns>
        private static List<string> GetExecutableBatchType(SystemCommonContext db, GetRunBatchYoyakuParam param, DateTime systemDate)
        {
            // システム日時
            var systemDateStr = systemDate.ToString("HH:mm:ss");

            logger.Info("実行可能時間帯を確認します。");
            return db.MBatchRunTimes.Where(a => string.Compare(a.BatchRunStartTime, systemDateStr) <= 0 &&
                                                string.Compare(a.BatchRunEndTime, systemDateStr) >= 0).Select(a => a.BatchType).Distinct().ToList();
        }

        /// <summary>
        /// 本処理が実行されているサーバ名にて、システム共通スキーマのバッチ予約テーブルの実行中のバッチの件数を取得する。
        /// </summary>
        /// <param name="db">システム共通DBコンテキスト</param>
        /// <param name="param">実行可能バッチ予約取得引数</param>
        /// <returns>実行中のバッチの件数</returns>
        private static int GetRunningBatchCount(SystemCommonContext db, GetRunBatchYoyakuParam param)
        {
            logger.Info("実行中のバッチの件数を取得します。");
            return db.TBatchYoyakus.Where(a => a.BatchRunServer == param.BatchRunServer &&
                                                a.BatchStatus == BATCH_STATUS_RUNNING &&
                                                a.DeleteFlg == DELETE_FLG_NOT_DELETED).Count();
        }

        /// <summary>
        /// バッチ実行上限数を取得する。
        /// </summary>
        /// <param name="db">システム共通DBコンテキスト</param>
        /// <returns>バッチ実行上限数</returns>
        private static int GetBatchRunMax(SystemCommonContext db)
        {
            logger.Info("実行中のバッチの件数を取得します。");
            return db.MBatchRunMaxs.Select(a => a.BatchMaxRun).SingleOrDefault();
        }

        /// <summary>
        /// 実行可能な最も古い予約データを取得する。
        /// </summary>
        /// <param name="db">システム共通DBコンテキスト</param>
        /// <param name="param">実行可能バッチ予約取得引数</param>
        /// <param name="executableBatchTypeList">現在時刻で実行可能なバッチ種類一覧</param>
        /// <param name="systemDate">システム日時</param>
        /// <returns>実行可能な最も古い予約データ</returns>
        private static TBatchYoyaku GetOldestBatchYoyaku(SystemCommonContext db,
                                                            GetRunBatchYoyakuParam param,
                                                            List<string> executableBatchTypeList,
                                                            DateTime systemDate)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append(
                "SELECT " +
                "  main.batch_id, " +
                "  batch_bunrui, " +
                "  system_kbn, " +
                "  todofuken_cd, " +
                "  kumiaito_cd, " +
                "  shisho_cd, " +
                "  batch_yoyaku_date, " +
                "  batch_yoyaku_id, " +
                "  batch_yoyaku_shori, " +
                "  batch_nm, " +
                "  batch_joken, " +
                "  batch_joken_disp, " +
                "  multi_run_ng_flg, " +
                "  multi_run_ng_id, " +
                "  batch_type, " +
                "  batch_sche_datetime, " +
                "  lock_target_flg, " +
                "  batch_status, " +
                "  batch_start_date, " +
                "  batch_end_date, " +
                "  batch_run_server, " +
                "  error_info, " +
                "  delete_flg, " +
                "  insert_user_id, " +
                "  insert_date, " +
                "  update_user_id, " +
                "  update_date, " +
                "  xmin " +
                "FROM " +
                "  t_batch_yoyaku AS main " +
                "WHERE '1' = '1' ");

            // システム区分
            if (!string.IsNullOrEmpty(param.SystemKbn))
            {
                sql.Append("  AND system_kbn = @SystemKbn ");
                parameters.Add(new NpgsqlParameter("@SystemKbn", param.SystemKbn));
            }

            // バッチ名
            if (!string.IsNullOrEmpty(param.BatchNm))
            {
                sql.Append("  AND batch_nm = @BatchNm ");
                parameters.Add(new NpgsqlParameter("@BatchNm", param.BatchNm));
            }

            sql.Append(
                "  AND batch_status = '01' " +
                "  AND delete_flg = '0' " +
                "  AND todofuken_cd IN ( " +
                "    SELECT " +
                "      m_todofuken_batch_run_max.todofuken_cd " +
                "    FROM " +
                "      m_todofuken_batch_run_max  " +
                "      LEFT JOIN (" +
                "        SELECT " +
                "          todofuken_cd, " +
                "          COUNT(1) As batch_run_cnt " +
                "        FROM " +
                "          t_batch_yoyaku " +
                "        WHERE " +
                "          batch_run_server = @BatchRunServer " +
                "          AND batch_status = '02' " +
                "          AND delete_flg = '0' " +
                "        GROUP BY  " +
                "          todofuken_cd  " +
                "      ) sub1 " +
                "        ON m_todofuken_batch_run_max.todofuken_cd = sub1.todofuken_cd " +
                "    WHERE " +
                "      m_todofuken_batch_run_max.batch_max_run > COALESCE(sub1.batch_run_cnt, 0) " +
                "  ) " +
                "  AND ( ");

            // 現在時刻で実行可能なバッチ種類の中に1（巡回）が含まれている場合
            if (executableBatchTypeList.Contains(BATCH_TYPE_PATROL))
            {
                sql.Append("  batch_type = '1' ");
            }

            // 現在時刻で実行可能なバッチ種類の中に1（巡回）と2（定時）が含まれている場合
            if (executableBatchTypeList.Contains(BATCH_TYPE_PATROL) &&
                executableBatchTypeList.Contains(BATCH_TYPE_SCHEDULED))
            {
                sql.Append("  OR ");
            }

            // 現在時刻で実行可能なバッチ種類の中に2（定時）が含まれている場合
            if (executableBatchTypeList.Contains(BATCH_TYPE_SCHEDULED))
            {
                sql.Append("  ( batch_type = '2' AND batch_sche_datetime <= @SystemDate_1 ) ");
                parameters.Add(new NpgsqlParameter("@SystemDate_1", systemDate));
            }

            sql.Append(
                "  ) " +
                "  AND ( " +
                "    multi_run_ng_flg = '0' " +
                "    OR ( " +
                "      multi_run_ng_flg = '1' " +
                "      AND NOT EXISTS ( " +
                "        SELECT 1 " +
                "        FROM t_batch_yoyaku " +
                "        WHERE " +
                "          system_kbn = main.system_kbn " +
                "          AND multi_run_ng_id = main.multi_run_ng_id " +
                "          AND batch_status = '02' " +
                "          AND delete_flg = '0' " +
                "      ) " +
                "    ) " +
                "  ) " +
                "  AND (  " +
                "      lock_target_flg = '0' " +
                "      OR (  " +
                "          ( " +
                "            lock_target_flg = '1' " +
                "            AND NOT EXISTS ( " +
                "              SELECT 1 FROM t_sys_lock " +
                "              WHERE " +
                "                lock_date <=  @SystemDate_2 " +
                "                AND lock_end_date >= @SystemDate_3 " +
                "            ) " +
                "          ) " +
                "          AND ( " +
                "            lock_target_flg = '1' " +
                "            AND NOT EXISTS ( " +
                "              SELECT 1 FROM t_data_lock " +
                "              WHERE " +
                "                ( " +
                "                  ( todofuken_cd = main.todofuken_cd AND kumiaito_cd = main.kumiaito_cd AND shisho_cd = main.shisho_cd ) " +
                "                  OR " +
                "                  ( todofuken_cd = main.todofuken_cd AND kumiaito_cd = main.kumiaito_cd AND COALESCE(main.shisho_cd, '') = '' ) " +
                "                  OR " +
                "                  ( todofuken_cd = main.todofuken_cd AND COALESCE(main.kumiaito_cd, '') = '' AND COALESCE(main.shisho_cd, '') = '' ) " +
                "                  OR " +
                "                  ( todofuken_cd = main.todofuken_cd AND kumiaito_cd = main.kumiaito_cd AND COALESCE(shisho_cd, '') = '' ) " +
                "                  OR " +
                "                  ( todofuken_cd = main.todofuken_cd AND COALESCE(kumiaito_cd, '') = '' AND COALESCE(main.shisho_cd, '') = '' ) " +
                "                ) " +
                "                AND lock_date <=  @SystemDate_4 " +
                "                AND lock_end_date >= @SystemDate_5 " +
                "            ) " +
                "          ) " +
                "      ) " +
                "  ) " +
                "ORDER BY " +
                "  batch_yoyaku_date ASC, " +
                "  batch_id ASC " +
                "LIMIT  " +
                "  1");

            parameters.Add(new NpgsqlParameter("@BatchRunServer", param.BatchRunServer));
            parameters.Add(new NpgsqlParameter("@SystemDate_2", systemDate));
            parameters.Add(new NpgsqlParameter("@SystemDate_3", systemDate));
            parameters.Add(new NpgsqlParameter("@SystemDate_4", systemDate));
            parameters.Add(new NpgsqlParameter("@SystemDate_5", systemDate));

            logger.Info("実行可能な最も古い予約データを取得します。");
            return db.Database.SqlQueryRaw<TBatchYoyaku>(sql.ToString(), parameters.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// バッチ予約データを取得しつつデータロックする。
        /// </summary>
        /// <param name="db">システム共通DBコンテキスト</param>
        /// <param name="param">実行可能バッチ予約取得引数</param>
        /// <returns></returns>
        private static TBatchYoyaku GetBatchYoyaku(SystemCommonContext db, UpdateBatchYoyakuStsParam param)
        {
            return db.TBatchYoyakus.Where(a => a.BatchId == param.BatchId && a.DeleteFlg == DELETE_FLG_NOT_DELETED).FirstOrDefault();
        }

        #region 引数格納用クラス
        /// <summary>
        /// バッチ予約登録引数
        /// </summary>
        private class InsertBatchYoyakuParam
        {
            /// <summary>
            /// バッチ分類
            /// </summary>
            [DisplayName("バッチ分類")]
            public string BatchBunrui { get; set; }

            /// <summary>
            /// システム区分
            /// </summary>
            [DisplayName("システム区分")]
            public string SystemKbn { get; set; }

            /// <summary>
            /// 都道府県コード
            /// </summary>
            [DisplayName("都道府県コード")]
            public string TodofukenCd { get; set; }

            /// <summary>
            /// 組合等コード
            /// </summary>
            [DisplayName("組合等コード")]
            public string KumiaitoCd { get; set; }

            /// <summary>
            /// 支所コード
            /// </summary>
            [DisplayName("支所コード")]
            public string ShishoCd { get; set; }

            /// <summary>
            /// 予約日時
            /// </summary>
            [DisplayName("予約日時")]
            public DateTime? YoyakuDate { get; set; }

            /// <summary>
            /// 予約ユーザID
            /// </summary>
            [DisplayName("予約ユーザID")]
            public string YoyakuUserId { get; set; }

            /// <summary>
            /// 予約を実行した処理名
            /// </summary>
            [DisplayName("予約を実行した処理名")]
            public string YoyakuShoriNm { get; set; }

            /// <summary>
            /// バッチ名
            /// </summary>
            [DisplayName("バッチ名")]
            public string BatchNm { get; set; }

            /// <summary>
            /// バッチ条件
            /// </summary>
            [DisplayName("バッチ条件")]
            public string BatchJoken { get; set; }

            /// <summary>
            /// バッチ条件（表示用）
            /// </summary>
            [DisplayName("バッチ条件（表示用）")]
            public string BatchJokenDisp { get; set; }

            /// <summary>
            /// 複数実行禁止フラグ
            /// </summary>
            [DisplayName("複数実行禁止フラグ")]
            public string MultiRunNgFlg { get; set; }

            /// <summary>
            /// 複数実行禁止ID
            /// </summary>
            [DisplayName("複数実行禁止ID")]
            public string MultiRunNgId { get; set; }

            /// <summary>
            /// バッチ種類
            /// </summary>
            [DisplayName("バッチ種類")]
            public string BatchType { get; set; }

            /// <summary>
            /// 定時実行バッチ予約日時
            /// </summary>
            [DisplayName("定時実行バッチ予約日時")]
            public DateTime? BatchScheDatetime { get; set; }

            /// <summary>
            /// ロック対象
            /// </summary>
            [DisplayName("ロック対象")]
            public string LockTargetFlg { get; set; }
        }

        /// <summary>
        /// バッチ予約状況取得条件
        /// </summary>
        public class GetBatchYoyakuListParam
        {
            /// <summary>
            /// バッチID
            /// </summary>
            [DisplayName("バッチID")]
            public long? BatchId { get; set; }

            /// <summary>
            /// バッチ分類
            /// </summary>
            [DisplayName("バッチ分類")]
            public string BatchBunrui { get; set; }

            /// <summary>
            /// システム区分
            /// </summary>
            [DisplayName("システム区分")]
            public string SystemKbn { get; set; }

            /// <summary>
            /// 都道府県コード
            /// </summary>
            [DisplayName("都道府県コード")]
            public string TodofukenCd { get; set; }

            /// <summary>
            /// 組合等コード
            /// </summary>
            [DisplayName("組合等コード")]
            public string KumiaitoCd { get; set; }

            /// <summary>
            /// 支所コード
            /// </summary>
            [DisplayName("支所コード")]
            public string ShishoCd { get; set; }

            /// <summary>
            /// 予約日時（開始）
            /// </summary>
            [DisplayName("予約日時（開始）")]
            public DateTime? YoyakuDateFrom { get; set; }

            /// <summary>
            /// 予約日時（終了）
            /// </summary>
            [DisplayName("予約日時（終了）")]
            public DateTime? YoyakuDateTo { get; set; }

            /// <summary>
            /// 予約ユーザID
            /// </summary>
            [DisplayName("予約ユーザID")]
            public string YoyakuUserId { get; set; }

            /// <summary>
            /// 予約を実行した処理名
            /// </summary>
            [DisplayName("予約を実行した処理名")]
            public string YoyakuShoriNm { get; set; }

            /// <summary>
            /// バッチ名
            /// </summary>
            [DisplayName("バッチ名")]
            public string BatchNm { get; set; }

            /// <summary>
            /// バッチ条件
            /// </summary>
            [DisplayName("バッチ条件")]
            public string BatchJoken { get; set; }

            /// <summary>
            /// バッチ条件（表示用）
            /// </summary>
            [DisplayName("バッチ条件（表示用）")]
            public string BatchJokenDisp { get; set; }

            /// <summary>
            /// 複数実行禁止フラグ
            /// </summary>
            [DisplayName("複数実行禁止フラグ")]
            public string MultiRunNgFlg { get; set; }

            /// <summary>
            /// 複数実行禁止ID
            /// </summary>
            [DisplayName("複数実行禁止ID")]
            public string MultiRunNgId { get; set; }

            /// <summary>
            /// バッチ種類
            /// </summary>
            [DisplayName("バッチ種類")]
            public string BatchType { get; set; }

            /// <summary>
            /// 定時実行バッチ予約日時（開始）
            /// </summary>
            [DisplayName("定時実行バッチ予約日時（開始）")]
            public DateTime? BatchScheDatetimeFrom { get; set; }

            /// <summary>
            /// 定時実行バッチ予約日時（終了）
            /// </summary>
            [DisplayName("定時実行バッチ予約日時（終了）")]
            public DateTime? BatchScheDatetimeTo { get; set; }

            /// <summary>
            /// ロック対象
            /// </summary>
            [DisplayName("ロック対象")]
            public string LockTargetFlg { get; set; }

            /// <summary>
            /// バッチ状態
            /// </summary>
            [DisplayName("バッチ状態")]
            public string BatchStatus { get; set; }

            /// <summary>
            /// 開始日時（開始）
            /// </summary>
            [DisplayName("開始日時（開始）")]
            public DateTime? BatchStartDateFrom { get; set; }

            /// <summary>
            /// 開始日時（終了）
            /// </summary>
            [DisplayName("開始日時（終了）")]
            public DateTime? BatchStartDateTo { get; set; }

            /// <summary>
            /// 完了日時（開始）
            /// </summary>
            [DisplayName("完了日時（開始）")]
            public DateTime? BatchEndDateFrom { get; set; }

            /// <summary>
            /// 完了日時（終了）
            /// </summary>
            [DisplayName("完了日時（終了）")]
            public DateTime? BatchEndDateTo { get; set; }

            /// <summary>
            /// 取得件数開始
            /// </summary>
            [DisplayName("取得件数開始")]
            public int? CntStart { get; set; }

            /// <summary>
            /// 取得件数終了
            /// </summary>
            [DisplayName("取得件数終了")]
            public int? CntEnd { get; set; }
        }

        /// <summary>
        /// 実行可能バッチ予約取得引数
        /// </summary>
        private class GetRunBatchYoyakuParam
        {
            /// <summary>
            /// システム区分
            /// </summary>
            [DisplayName("システム区分")]
            public string SystemKbn { get; set; }

            /// <summary>
            /// バッチ名
            /// </summary>
            [DisplayName("バッチ名")]
            public string BatchNm { get; set; }

            /// <summary>
            /// 実行サーバ
            /// </summary>
            [DisplayName("実行サーバ")]
            public string BatchRunServer { get; set; }

            /// <summary>
            /// 更新ユーザID
            /// </summary>
            [DisplayName("更新ユーザID")]
            public string UpdateUserId { get; set; }
        }

        /// <summary>
        /// バッチ実行状況更新引数
        /// </summary>
        private class UpdateBatchYoyakuStsParam
        {
            /// <summary>
            /// バッチID
            /// </summary>
            [DisplayName("バッチID")]
            public long BatchId { get; set; }

            /// <summary>
            /// ステータス
            /// </summary>
            [DisplayName("ステータス")]
            public string Status { get; set; }

            /// <summary>
            /// エラー情報
            /// </summary>
            [DisplayName("エラー情報")]
            public string ErrInfo { get; set; }

            /// <summary>
            /// 更新ユーザID
            /// </summary>
            [DisplayName("更新ユーザID")]
            public string UpdateUserId { get; set; }
        }

        /// <summary>
        /// バッチダウンロードファイル登録引数
        /// </summary>
        private class InsertBatchDownloadFileParam
        {
            /// <summary>
            /// バッチID
            /// </summary>
            [DisplayName("バッチID")]
            public long BatchId { get; set; }

            /// <summary>
            /// ファイルパス
            /// </summary>
            [DisplayName("ファイルパス")]
            public string FilePath { get; set; }

            /// <summary>
            /// ハッシュ
            /// </summary>
            [DisplayName("ハッシュ")]
            public string Hash { get; set; }

            /// <summary>
            /// ファイル名
            /// </summary>
            [DisplayName("ファイル名")]
            public string FileNm { get; set; }

            /// <summary>
            /// 登録ユーザID
            /// </summary>
            [DisplayName("登録ユーザID")]
            public string InsertUserId { get; set; }
        }
        #endregion
    }
}
