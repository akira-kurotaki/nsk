using CoreLibrary.Core.Consts;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using Npgsql;
using System.ComponentModel;
using System.Text;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// ロック関連ユーティリティ
    /// </summary>
    public static class LockUtil
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
        /// 処理成否（既にロック済）
        /// </summary>
        public const int RET_ALREADY_LOCKED = 2;

        /// <summary>
        /// 処理成否（既に終了済）
        /// </summary>
        public const int RET_ALREADY_FINISHED = 2;

        /// <summary>
        /// 処理成否（エラー）
        /// </summary>
        public const int RET_ERROR = 99;

        /// <summary>
        /// ロック状態（ロック無）
        /// </summary>
        public const int LOCKED_STATE_NO_LOCK = 0;

        /// <summary>
        /// ロック状態（ロック有）
        /// </summary>
        public const int LOCKED_STATE_LOCKED = 1;

        /// <summary>
        /// ロック状態（エラー）
        /// </summary>
        public const int LOCKED_STATE_ERROR = 99;


        /// <summary>
        /// システムロック情報登録
        /// </summary>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="dateLockStart">ロック開始日時</param>
        /// <param name="dateLockEnd">ロック終了日時</param>
        /// <param name="strLockShori">ロック実行処理</param>
        /// <param name="strUserId">ロック実行ユーザID</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns>処理成否（0：失敗、1：成功、2：既にロック済）</returns>
        public static int InsertSysLock(string strSystemKbn,
                                        string strTodofukenCd,
                                        DateTime? dateLockStart,
                                        DateTime? dateLockEnd,
                                        string strLockShori,
                                        string strUserId,
                                        ref string message)
        {
            var param = new InsertSysLockParam
            {
                SystemKbn = strSystemKbn,
                TodofukenCd = strTodofukenCd,
                LockStart = dateLockStart,
                LockEnd = dateLockEnd,
                LockShori = strLockShori,
                UserId = strUserId,
            };

            try
            {
                // 必須チェック
                if (!CheckInsertSysLockParamRequired(param, ref message))
                {
                    return RET_FAIL;
                }

                // 桁数チェック
                if (!CheckParamMaxLength(param.LockShori, param.UserId, ref message))
                {
                    return RET_FAIL;
                }

                // マスタ整合性チェック
                if (!CheckInsertSysLockParamMaster(param, ref message))
                {
                    return RET_FAIL;
                }

                // ロック有無チェック

                // ロック実行ユーザID
                string lockStartUserId = string.Empty;

                // システムロック取得
                var status = GetSysLock(param.SystemKbn, param.TodofukenCd, (DateTime)param.LockStart, ref lockStartUserId);

                if (status == LOCKED_STATE_LOCKED)
                {
                    // ロック状態が1：ロック有の場合、処理成否に2を設定して、呼び出し元に処理を戻す。
                    return RET_ALREADY_LOCKED;
                }
                else if (status == LOCKED_STATE_ERROR)
                {
                    // ロック状態が99：エラーの場合、処理成否に0を設定して、呼び出し元に処理を戻す。
                    message = SystemMessageUtil.Get("MF80002");
                    return RET_FAIL;
                }
                else
                {
                    // ロック情報登録
                    using (var db = new SystemCommonContext())
                    {
                        using (var tx = db.Database.BeginTransaction())
                        {
                            logger.Info("ロック情報を登録します。");
                            TSysLock tSysLock = new()
                            {
                                // システム区分
                                SystemKbn = param.SystemKbn,
                                // 都道府県コード
                                TodofukenCd = param.TodofukenCd,
                                // ロック開始日時
                                LockDate = (DateTime)param.LockStart,
                                // ロック終了日時
                                LockEndDate = (DateTime)param.LockEnd,
                                // ロック実行ユーザ
                                LockUserId = param.UserId,
                                // ロック実行処理
                                LockShori = param.LockShori
                            };

                            db.TSysLocks.Add(tSysLock);

                            try
                            {
                                db.SaveChanges();
                                tx.Commit();
                            }
                            catch (DbUpdateException)
                            {
                                // 失敗の場合
                                tx.Rollback();
                                throw;
                            }
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

        /// <summary>
        /// システムロック取得
        /// </summary>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="dateSysDateTime">システム日時</param>
        /// <param name="strUserId">ロック実行ユーザID</param>
        /// <returns>ロック状態（0：ロック無、1：ロック有、99：エラー）</returns>
        public static int GetSysLock(string strSystemKbn,
                                        string strTodofukenCd,
                                        DateTime dateSysDateTime,
                                        ref string strUserId)
        {
            try
            {
                // ロック情報取得
                using (var db = new SystemCommonContext())
                {
                    logger.Info("システムロック情報を取得します。");
                    var lockInfo = db.TSysLocks.Where(x => x.SystemKbn == strSystemKbn &&
                                                            x.TodofukenCd == strTodofukenCd &&
                                                            x.LockDate <= dateSysDateTime &&
                                                            x.LockEndDate >= dateSysDateTime).FirstOrDefault();

                    if (lockInfo != null)
                    {
                        strUserId = lockInfo.LockUserId;
                        return LOCKED_STATE_LOCKED;
                    }
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                return LOCKED_STATE_ERROR;
            }

            // 「ロック情報」が取得できなかった場合
            return LOCKED_STATE_NO_LOCK;
        }

        /// <summary>
        /// システムロック終了日時更新
        /// </summary>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="dateLockStart">ロック開始日時</param>
        /// <param name="dateLockEnd">ロック終了日時</param>
        /// <param name="strUserId">ロック終了ユーザID</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns>処理成否（0：失敗、1：成功、2：既に終了済）</returns>
        public static int UpdateSysLockEndDate(string strSystemKbn,
                                                string strTodofukenCd,
                                                DateTime? dateLockStart,
                                                DateTime? dateLockEnd,
                                                string strUserId,
                                                ref string message)
        {
            var param = new UpdateSysLockEndDateParam
            {
                SystemKbn = strSystemKbn,
                TodofukenCd = strTodofukenCd,
                LockStart = dateLockStart,
                LockEnd = dateLockEnd,
                UserId = strUserId,
            };

            try
            {
                // 必須チェック
                if (!CheckUpdateSysLockEndDateParamRequired(param, ref message))
                {
                    return RET_FAIL;
                }

                // 桁数チェック
                if (!CheckParamMaxLength(param.UserId, ref message))
                {
                    return RET_FAIL;
                }

                // ロック終了確認
                using (var db = new SystemCommonContext())
                {
                    using (var tx = db.Database.BeginTransaction())
                    {
                        logger.Info("ロック情報を取得します。");
                        var lockInfo = db.TSysLocks.Where(x => x.SystemKbn == param.SystemKbn &&
                                                                x.TodofukenCd == param.TodofukenCd &&
                                                                x.LockDate == param.LockStart).FirstOrDefault();

                        // 「ロック終了日時（DB）」が取得できない場合
                        if (lockInfo == null)
                        {
                            message = SystemMessageUtil.Get("ME80018");
                            return RET_FAIL;
                        }

                        // 「ロック終了日時（DB）」<=「引数：ロック終了日時」の場合
                        if (lockInfo.LockEndDate <= param.LockEnd)
                        {
                            return RET_ALREADY_FINISHED;
                        }

                        // ロック終了日時
                        lockInfo.LockEndDate = param.LockEnd;
                        // ロック実行ユーザ
                        lockInfo.LockUserId = param.UserId;

                        try
                        {
                            logger.Info("システムロック終了日時を更新します。");
                            db.SaveChanges();
                            tx.Commit();
                        }
                        catch (DbUpdateException)
                        {
                            // 失敗の場合
                            tx.Rollback();
                            throw;
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

        /// <summary>
        /// データロック情報登録
        /// </summary>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="strKumiaitoCd">組合等コード</param>
        /// <param name="strShishoCd">支所コード</param>
        /// <param name="dateLockStart">ロック開始日時</param>
        /// <param name="dateLockEnd">ロック終了日時</param>
        /// <param name="strLockShori">ロック実行処理</param>
        /// <param name="strUserId">ロック実行ユーザID</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns>処理成否（0：失敗、1：成功、2：既にロック済）</returns>
        public static int InsertDataLock(string strSystemKbn,
                                            string strTodofukenCd,
                                            string strKumiaitoCd,
                                            string strShishoCd,
                                            DateTime? dateLockStart,
                                            DateTime? dateLockEnd,
                                            string strLockShori,
                                            string strUserId,
                                            ref string message)
        {
            var param = new InsertDataLockParam
            {
                SystemKbn = strSystemKbn,
                TodofukenCd = strTodofukenCd,
                KumiaitoCd = strKumiaitoCd,
                ShishoCd = strShishoCd,
                LockStart = dateLockStart,
                LockEnd = dateLockEnd,
                LockShori = strLockShori,
                UserId = strUserId,
            };

            try
            {
                // 必須チェック
                if (!CheckInsertDataLockParamRequired(param, ref message))
                {
                    return RET_FAIL;
                }

                // 桁数チェック
                if (!CheckParamMaxLength(param.LockShori, param.UserId, ref message))
                {
                    return RET_FAIL;
                }

                // マスタ整合性チェック
                if (!CheckInsertDataLockParamMaster(param, ref message))
                {
                    return RET_FAIL;
                }

                // ロック有無チェック

                // ロック実行ユーザID
                string lockStartUserId = string.Empty;

                // データロック取得
                var status = GetDataLock(param.SystemKbn, param.TodofukenCd, param.KumiaitoCd,
                                            param.ShishoCd, (DateTime)param.LockStart, ref lockStartUserId);

                if (status == LOCKED_STATE_LOCKED)
                {
                    // ロック状態が1：ロック有の場合、処理成否に2を設定して、呼び出し元に処理を戻す。
                    return RET_ALREADY_LOCKED;
                }
                else if (status == LOCKED_STATE_ERROR)
                {
                    // ロック状態が99：エラーの場合、処理成否に0を設定して、呼び出し元に処理を戻す。
                    message = SystemMessageUtil.Get("MF80002");
                    return RET_FAIL;
                }
                else
                {
                    // ロック情報登録
                    using (var db = new SystemCommonContext())
                    {
                        using (var tx = db.Database.BeginTransaction())
                        {
                            logger.Info("ロック情報を登録します。");
                            TDataLock tDataLock = new()
                            {
                                // システム区分
                                SystemKbn = param.SystemKbn,
                                // 都道府県コード
                                TodofukenCd = param.TodofukenCd,
                                // 組合等コード
                                KumiaitoCd = CommonFuncUtil.GetString(param.KumiaitoCd),
                                // 支所コード
                                ShishoCd = CommonFuncUtil.GetString(param.ShishoCd),
                                // ロック開始日時
                                LockDate = (DateTime)param.LockStart,
                                // ロック終了日時
                                LockEndDate = (DateTime)param.LockEnd,
                                // ロック実行ユーザ
                                LockUserId = param.UserId,
                                // ロック実行処理
                                LockShori = param.LockShori
                            };

                            db.TDataLocks.Add(tDataLock);

                            try
                            {
                                db.SaveChanges();
                                tx.Commit();
                            }
                            catch (DbUpdateException)
                            {
                                // 失敗の場合
                                tx.Rollback();
                                throw;
                            }
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

        /// <summary>
        /// データロック取得
        /// </summary>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="strKumiaitoCd">組合等コード</param>
        /// <param name="strShishoCd">支所コード</param>
        /// <param name="dateSysDateTime">システム日時</param>
        /// <param name="strUserId">ロック実行ユーザID</param>
        /// <returns>ロック状態（0：ロック無、1：ロック有、99：エラー）</returns>
        public static int GetDataLock(string strSystemKbn,
                                        string strTodofukenCd,
                                        string strKumiaitoCd,
                                        string strShishoCd,
                                        DateTime dateSysDateTime,
                                        ref string strUserId)
        {
            try
            {
                // sql用定数定義
                var sql = new StringBuilder();

                sql.Append("SELECT ");
                sql.Append("    system_kbn ");
                sql.Append("    , todofuken_cd ");
                sql.Append("    , kumiaito_cd ");
                sql.Append("    , shisho_cd ");
                sql.Append("    , lock_date ");
                sql.Append("    , lock_end_date ");
                sql.Append("    , lock_user_id ");
                sql.Append("    , lock_shori ");
                sql.Append("    , xmin ");
                sql.Append("FROM ");
                sql.Append("    t_data_lock ");
                sql.Append("WHERE ");
                sql.Append("    system_kbn = @SystemKbn ");
                sql.Append("    AND lock_date <= @SysDateTime ");
                sql.Append("    AND lock_end_date >= @SysDateTime ");
                sql.Append("    AND ( ");
                sql.Append("        ( ");
                sql.Append("            todofuken_cd = @TodofukenCd ");
                sql.Append("            AND kumiaito_cd = @KumiaitoCd ");
                sql.Append("            AND shisho_cd = @ShishoCd ");
                sql.Append("        ) ");
                sql.Append("        OR ( ");
                sql.Append("            todofuken_cd = @TodofukenCd ");
                sql.Append("            AND kumiaito_cd = @KumiaitoCd ");
                sql.Append("            AND shisho_cd = '' ");
                sql.Append("        ) ");
                sql.Append("        OR ( ");
                sql.Append("            todofuken_cd = @TodofukenCd ");
                sql.Append("            AND kumiaito_cd = '' ");
                sql.Append("            AND shisho_cd = '' ");
                sql.Append("        ) ");

                // 引数：支所コードが空欄の場合
                if (string.IsNullOrEmpty(strShishoCd))
                {
                    sql.Append("        OR ( ");
                    sql.Append("            todofuken_cd = @TodofukenCd ");
                    sql.Append("            AND kumiaito_cd = '' ");
                    sql.Append("        ) ");
                }

                // 引数：組合等コードが空欄の場合
                if (string.IsNullOrEmpty(strKumiaitoCd))
                {
                    sql.Append("        OR ( ");
                    sql.Append("            todofuken_cd = @TodofukenCd ");
                    sql.Append("           ) ");
                }

                sql.Append("    ) ");

                var parameters = new List<NpgsqlParameter>
                {
                    new("@SystemKbn", strSystemKbn),
                    new("@SysDateTime", dateSysDateTime),
                    new("@TodofukenCd", strTodofukenCd),
                    new("@KumiaitoCd", CommonFuncUtil.GetString(strKumiaitoCd)),
                    new("@ShishoCd", CommonFuncUtil.GetString(strShishoCd))
                };

                // ロック情報取得
                using (var db = new SystemCommonContext())
                {
                    logger.Info("データロック情報を取得します。");
                    var dataLock = db.Database.SqlQueryRaw<TDataLock>(sql.ToString(), parameters.ToArray()).SingleOrDefault();
                    if (dataLock != null)
                    {
                        strUserId = dataLock.LockUserId;
                        return LOCKED_STATE_LOCKED;
                    }
                }
            }
            catch (Exception e)
            {
                // 失敗の場合
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                return LOCKED_STATE_ERROR;
            }

            return LOCKED_STATE_NO_LOCK;
        }

        /// <summary>
        /// データロック終了日時更新
        /// </summary>
        /// <param name="strSystemKbn">システム区分</param>
        /// <param name="strTodofukenCd">都道府県コード</param>
        /// <param name="strKumiaitoCd">組合等コード</param>
        /// <param name="strShishoCd">支所コード</param>
        /// <param name="dateLockStart">ロック開始日時</param>
        /// <param name="dateLockEnd">ロック終了日時</param>
        /// <param name="strUserId">ロック終了ユーザID</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns>処理成否（0：失敗、1：成功、2：既に終了済）</returns>
        public static int UpdateDataLockEndDate(string strSystemKbn,
                                                string strTodofukenCd,
                                                string strKumiaitoCd,
                                                string strShishoCd,
                                                DateTime? dateLockStart,
                                                DateTime? dateLockEnd,
                                                string strUserId,
                                                ref string message)
        {
            var param = new UpdateDataLockEndDateParam
            {
                SystemKbn = strSystemKbn,
                TodofukenCd = strTodofukenCd,
                KumiaitoCd = strKumiaitoCd,
                ShishoCd = strShishoCd,
                LockStart = dateLockStart,
                LockEnd = dateLockEnd,
                UserId = strUserId,
            };

            try
            {
                // 必須チェック
                if (!CheckUpdateDataLockEndDateParamRequired(param, ref message))
                {
                    return RET_FAIL;
                }

                // 桁数チェック
                if(!CheckParamMaxLength(param.UserId, ref message))
                {
                    return RET_FAIL;
                }

                // ロック終了確認

                using (var db = new SystemCommonContext())
                {
                    // データロック終了日時更新
                    using (var tx = db.Database.BeginTransaction())
                    {
                        logger.Info("データロック情報を取得します。");
                        var lockInfo = db.TDataLocks.Where(x => x.SystemKbn == param.SystemKbn &&
                                                                x.TodofukenCd == param.TodofukenCd &&
                                                                x.KumiaitoCd == CommonFuncUtil.GetString(param.KumiaitoCd) &&
                                                                x.ShishoCd == CommonFuncUtil.GetString(param.ShishoCd) &&
                                                                x.LockDate == param.LockStart).FirstOrDefault();

                        // 「ロック終了日時（DB）」が取得できない場合
                        if (lockInfo == null)
                        {
                            message = SystemMessageUtil.Get("ME80018");
                            return RET_FAIL;
                        }

                        // 「ロック終了日時（DB）」<=「引数：ロック終了日時」の場合
                        if (lockInfo.LockEndDate <= param.LockEnd)
                        {
                            return RET_ALREADY_FINISHED;
                        }

                        // ロック終了日時
                        lockInfo.LockEndDate = (DateTime)param.LockEnd;
                        // ロック実行ユーザ
                        lockInfo.LockUserId = param.UserId;

                        try
                        {
                            logger.Info("データロック終了日時を更新します。");
                            db.SaveChanges();
                            tx.Commit();
                        }
                        catch (DbUpdateException)
                        {
                            // 失敗の場合
                            tx.Rollback();
                            throw;
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

        #region 必須チェック（引数共通）
        /// <summary>
        /// 必須チェック（引数共通）
        /// </summary>
        /// <param name="param">引数共通</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckParamBaseRequired(ParamBase param, ref string message)
        {
            // システム区分
            if (!CommonFuncUtil.ValidateRequired(
                    param.SystemKbn,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)),
                    ref message))
            {
                return false;
            }

            // 都道府県コード
            if (!CommonFuncUtil.ValidateRequired(
                    param.TodofukenCd,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.TodofukenCd)),
                    ref message))
            {
                return false;
            }

            // ロック開始日時
            if (!CommonFuncUtil.ValidateRequired(
                    param.LockStart,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.LockStart)),
                    ref message))
            {
                return false;
            }

            // ロック終了日時
            if (!CommonFuncUtil.ValidateRequired(
                    param.LockEnd,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.LockEnd)),
                    ref message))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 必須チェック（システムロック情報登録）
        /// <summary>
        /// 必須チェック（システムロック情報登録）
        /// </summary>
        /// <param name="param">システムロック情報登録引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertSysLockParamRequired(InsertSysLockParam param, ref string message)
        {
            if (!CheckParamBaseRequired(param, ref message))
            {
                return false;
            }

            // ロック実行ユーザID
            if (!CommonFuncUtil.ValidateRequired(
                    param.UserId,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.UserId)),
                    ref message))
            {
                return false;
            }

            // ロック実行処理
            if (!CommonFuncUtil.ValidateRequired(
                    param.LockShori,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.LockShori)),
                    ref message))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region マスタ整合性チェック（システムロック情報登録）
        /// <summary>
        /// マスタ整合性チェック（システムロック情報登録）
        /// </summary>
        /// <param name="param">システムロック情報登録引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertSysLockParamMaster(InsertSysLockParam param, ref string message)
        {
            using (var db = new SystemCommonContext())
            {
                // システム区分：汎用区分マスタ「system_kbn」
                if (!CommonFuncUtil.ValidateHanyokubun(db, "system_kbn", param.SystemKbn,
                        CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)), ref message))
                {
                    return false;
                }

                // 都道府県コード：都道府県マスタ
                if (!CommonFuncUtil.ValidateTodofuken(db, param.TodofukenCd, ref message))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 必須チェック（システムロック終了日時更新）
        /// <summary>
        /// 必須チェック（システムロック終了日時更新）
        /// </summary>
        /// <param name="param">システムロック終了日時更新引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckUpdateSysLockEndDateParamRequired(UpdateSysLockEndDateParam param, ref string message)
        {
            if (!CheckParamBaseRequired(param, ref message))
            {
                return false;
            }

            // ロック終了ユーザID
            if (!CommonFuncUtil.ValidateRequired(
                    param.UserId,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.UserId)),
                    ref message))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 必須チェック（データロック情報登録）
        /// <summary>
        /// 必須チェック（データロック情報登録）
        /// </summary>
        /// <param name="param">データロック情報登録引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertDataLockParamRequired(InsertDataLockParam param, ref string message)
        {
            if (!CheckParamBaseRequired(param, ref message))
            {
                return false;
            }

            // ロック実行ユーザID
            if (!CommonFuncUtil.ValidateRequired(
                    param.UserId,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.UserId)),
                    ref message))
            {
                return false;
            }

            // ロック実行処理
            if (!CommonFuncUtil.ValidateRequired(
                    param.LockShori,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.LockShori)),
                    ref message))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 桁数チェック
        /// <summary>
        /// 桁数チェック（ロック実行処理、ロック実行ユーザID）
        /// </summary>
        /// <param name="lockShori">ロック実行処理</param>
        /// <param name="userId">ロック実行ユーザID</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckParamMaxLength(string lockShori, string userId, ref string message)
        {
            var tSysLock = new TSysLock();

            // ロック実行処理
            if (!CommonFuncUtil.ValidateMaxLength(
                    lockShori,
                    CommonFuncUtil.GetMaximumLength(tSysLock.GetType(), nameof(tSysLock.LockShori)),
                    "ロック実行処理",
                    ref message))
            {
                return false;
            }

            // ロック実行ユーザID
            if (!CommonFuncUtil.ValidateMaxLength(
                    userId,
                    CommonFuncUtil.GetMaximumLength(tSysLock.GetType(), nameof(tSysLock.LockUserId)),
                    "ロック実行ユーザID",
                    ref message))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 桁数チェック（ロック終了ユーザID）
        /// </summary>
        /// <param name="userId">ロック終了ユーザID</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckParamMaxLength(string userId, ref string message)
        {
            var tSysLock = new TSysLock();

            // ロック終了ユーザID
            if (!CommonFuncUtil.ValidateMaxLength(
                    userId,
                    CommonFuncUtil.GetMaximumLength(tSysLock.GetType(), nameof(tSysLock.LockUserId)),
                    "ロック終了ユーザID",
                    ref message))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region マスタ整合性チェック（データロック情報登録）
        /// <summary>
        /// マスタ整合性チェック（データロック情報登録）
        /// </summary>
        /// <param name="param">データロック情報登録引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckInsertDataLockParamMaster(InsertDataLockParam param, ref string message)
        {
            using (var db = new SystemCommonContext())
            {
                // システム区分：汎用区分マスタ「system_kbn」
                if (!CommonFuncUtil.ValidateHanyokubun(db, "system_kbn", param.SystemKbn,
                        CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.SystemKbn)), ref message))
                {
                    return false;
                }

                // 都道府県コード：都道府県マスタ
                if (!CommonFuncUtil.ValidateTodofuken(db, param.TodofukenCd, ref message))
                {
                    return false;
                }

                // 組合等コード：組合等マスタ
                if (!string.IsNullOrEmpty(param.KumiaitoCd) && 
                    !CommonFuncUtil.ValidateKumiaito(db, param.TodofukenCd, param.KumiaitoCd, ref message))
                {
                    return false;
                }

                // 支所コード：名称M支所
                if (!string.IsNullOrEmpty(param.ShishoCd) &&
                    !CommonFuncUtil.ValidateShisho(db, param.TodofukenCd, param.KumiaitoCd, param.ShishoCd, ref message))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 必須チェック（データロック終了日時更新）
        /// <summary>
        /// 必須チェック（データロック終了日時更新）
        /// </summary>
        /// <param name="param">データロック終了日時更新引数</param>
        /// <param name="message">エラーメッセージ(出力パラメータ)</param>
        /// <returns>true：エラーなし、false：エラーあり</returns>
        private static bool CheckUpdateDataLockEndDateParamRequired(UpdateDataLockEndDateParam param, ref string message)
        {
            if (!CheckParamBaseRequired(param, ref message))
            {
                return false;
            }

            // ロック終了ユーザID
            if (!CommonFuncUtil.ValidateRequired(
                    param.UserId,
                    CommonFuncUtil.GetDisplayName(param.GetType(), nameof(param.UserId)),
                    ref message))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 引数格納用クラス
        /// <summary>
        /// 引数ベースクラス
        /// </summary>
        private abstract class ParamBase
        {
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
            /// ロック開始日時
            /// </summary>
            [DisplayName("ロック開始日時")]
            public DateTime? LockStart { get; set; }

            /// <summary>
            /// ロック終了日時
            /// </summary>
            [DisplayName("ロック終了日時")]
            public DateTime? LockEnd { get; set; }
        }

        /// <summary>
        /// システムロック情報登録引数
        /// </summary>
        private class InsertSysLockParam : ParamBase
        {
            /// <summary>
            /// ロック実行処理
            /// </summary>
            [DisplayName("ロック実行処理")]
            public string LockShori { get; set; }

            /// <summary>
            /// ロック実行ユーザID
            /// </summary>
            [DisplayName("ロック実行ユーザID")]
            public string UserId { get; set; }
        }

        /// <summary>
        /// システムロック終了日時更新引数
        /// </summary>
        private class UpdateSysLockEndDateParam : ParamBase
        {
            /// <summary>
            /// ロック終了ユーザID
            /// </summary>
            [DisplayName("ロック終了ユーザID")]
            public string UserId { get; set; }
        }

        /// <summary>
        /// データロック情報登録引数
        /// </summary>
        private class InsertDataLockParam : ParamBase
        {
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
            /// ロック実行処理
            /// </summary>
            [DisplayName("ロック実行処理")]
            public string LockShori { get; set; }

            /// <summary>
            /// ロック実行ユーザID
            /// </summary>
            [DisplayName("ロック実行ユーザID")]
            public string UserId { get; set; }
        }

        /// <summary>
        /// データロック終了日時更新引数
        /// </summary>
        private class UpdateDataLockEndDateParam : ParamBase
        {
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
            /// ロック終了ユーザID
            /// </summary>
            [DisplayName("ロック終了ユーザID")]
            public string UserId { get; set; }
        }
        #endregion
    }
}
