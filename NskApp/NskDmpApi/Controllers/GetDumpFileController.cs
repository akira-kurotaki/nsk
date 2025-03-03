using BaseDmpApi.Base;
using BaseDmpApi.Const;
using BaseDmpApi.Models;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using ModelLibrary.Models;

namespace BaseDmpApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GetDumpFileController : ApiControllerBase
    {
        /// <summary>
        /// ダンプファイル取得
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <returns>ダンプファイル情報</returns>
        [HttpPost]
        public ActionResult<GetDumpFileResponse> GetDumpFile(GetDumpFileRequest request)
        {
            var messages = new List<Message>();

            //リクエストの形式が不正な場合、以下のHTTPステータスを設定し、呼び出し元に返却する。
            // （HTTPステータス：400（BAD REQUEST））
            if (!ModelState.IsValid)
            {
                logger.Error(MessageUtil.Get("ME01646", "リクエスト", ""));
                messages = AddErrorMsgList(messages, "ME01646", "リクエスト", "");
                return CreateBadRequestResponse<ApiErrorResponseBase>(messages);
            }

            // 共通部分のチェック処理
            messages = CheckCommonRequest(request);
            if (0 < messages.Count)
            {
                // [変数：エラー内容]に値が設定されている場合
                // エラー用のレスポンスに[変数：エラー内容]と以下のHTTPステータスを設定し、呼び出し元に返却する。
                // （HTTPステータス：400（BAD REQUEST））
                return CreateBadRequestResponse<ApiErrorResponseBase>(messages);
            }

            // APIパスフレーズチェック
            if (!ApiPassCheck(request))
            {
                messages = AddErrorMsgList(messages, "ME01646", "パスフレーズ", "");
                logger.Error(MessageUtil.Get("ME01646", "パスフレーズ", ""));
                return CreateUnauthorizedResponse<ApiErrorResponseBase>(messages);
            }

            // [変数：ユーザID]を設定する
            var userId = CreateUserId(request);
            // INFOログ出力
            logger.Info("都道府県・組合等・支所: {UserId}", userId);

            // INFOログ出力
            // （"ダンプファイル取得の対象都道府県：[連携都道府県コード]]）
            logger.Info("ダンプファイル取得の対象都道府県: {TodofukenCd}", request.TodofukenCd);

            // システム日時
            var sysDateTime = DateUtil.GetSysDateTime();

            try
            {
                // 「変数：トークン」
                var token = request.Token;
                // 返却データ
                var getDumpFileResponse = new GetDumpFileResponse() { token = token };

                using (var db = new SystemCommonContext(ConfigUtil.GetInt(BaseDmpApiConst.CONFIG_COMMAND_TIMEOUT)))
                {
                    // もしくは、一時ダンプファイル更新失敗した場合、一時ダンプファイル登録処理はロールバックする必要と想定、トランザクションをします。
                    using (var tx = db.Database.BeginTransaction())
                    {
                        // バックアップ対象情報取得
                        // リクエストパラメータ[トークン]が空の場合
                        if (string.IsNullOrEmpty(token))
                        {
                            MBackupInfo backupInfo = null;

                            // リクエストパラメータ[都道府県コード]を検索条件にシステム共通スキーマの「バックアップ対象情報」を取得する
                            logger.Info("リクエストパラメータ[都道府県コード]を検索条件にシステム共通スキーマの「バックアップ対象情報」を取得する。");
                            backupInfo = db.MBackupInfos.Where(t => t.TodofukenCd == request.TodofukenCd).SingleOrDefault();

                            //  取得結果が0件の場合
                            if (backupInfo == null)
                            {
                                // 呼び出し元に以下を返却する。
                                // ・ メッセージ　（メッセージID：ME01644 、引数１："該当するダンプファイル" ）
                                // ・ HTTPステータス　500（INTERNAL SERVER ERROR）
                                logger.Error(MessageUtil.Get("ME01644", "該当するダンプファイル", ""));
                                messages = AddErrorMsgList(messages, "ME01644", "該当するダンプファイル", "");
                                return CreateErrorResponse<ApiErrorResponseBase>("ME01644", "該当するダンプファイル", "");
                            }

                            // 「バックアップ対象情報」.[ダンプ出力先パス]のパスを参照し、存在するダンプファイルの一覧を取得する。
                            var dmpFilePath = backupInfo.DmpOutputPath;
                            var files = new DirectoryInfo(dmpFilePath).EnumerateFiles();
                            var dumpFiles = files.Where(file => 0 <= file.Name.IndexOf($"{backupInfo.TodofukenCd}{BaseDmpApiConst.FILE_EXTENSION_DUMP}", StringComparison.OrdinalIgnoreCase)).ToList();
                            // ファイルが存在しない場合
                            if (dumpFiles.IsNullOrEmpty())
                            {
                                // 呼び出し元に以下を返却する。
                                // ・ メッセージ　（メッセージID：ME01644 、引数１："該当するダンプファイル" ）
                                // ・ HTTPステータス　500（INTERNAL SERVER ERROR）
                                logger.Error(MessageUtil.Get("ME01644", "該当するダンプファイル", ""));
                                messages = AddErrorMsgList(messages, "ME01644", "該当するダンプファイル", "");
                                return CreateErrorResponse<ApiErrorResponseBase>("ME01644", "該当するダンプファイル", "");
                            }

                            // ウ） 処理キーを取得し、「変数：トークン」、返却データ[トークン]に設定する。
                            var shoriKey = CreateShoriKey(userId, sysDateTime);
                            token = shoriKey;
                            getDumpFileResponse.token = shoriKey;

                            // 「イ）」で取得したファイル数分のデータを システム共通スキーマの「一時ダンプファイル取得」に登録する。（ログ出力：あり）
                            InsertWDumpFileDownload(db, dumpFiles, token, userId, sysDateTime);
                        }

                        // 一時ダンプファイル取得（ログ出力：あり）
                        logger.Info("一時ダンプファイルを取得する");
                        var dumpFile = db.WDumpFileDownloads.Where(t => t.Token == token && CoreConst.FLG_OFF.Equals(t.DownloadFlg)).OrderBy(t => t.Token).ThenBy(t => t.Renban).FirstOrDefault();
                        // 取得結果が0件の場合
                        // ファイルが存在しない場合
                        if (dumpFile == null || !System.IO.File.Exists(dumpFile.DmpfilePath))
                        {
                            // １） 呼び出し元に以下を返却する。
                            // ・ メッセージ　（メッセージID：ME01644 、引数１："未ダウンロードのダンプファイル" ）	
                            // ※エラー発生時は、[ダンプファイル]、[残件数]は設定なしとする。
                            // ・ HTTPステータス　500（INTERNAL SERVER ERROR）
                            logger.Error(MessageUtil.Get("ME01644", "未ダウンロードのダンプファイル", ""));
                            messages = AddErrorMsgList(messages, "ME01644", "未ダウンロードのダンプファイル", "");
                            return CreateErrorResponse<ApiErrorResponseBase>("ME01644", "未ダウンロードのダンプファイル", "");
                        }

                        // 取得したデータの「ダンプファイルパス」のファイルを取得して、復号化し、返却データ[ダンプファイル]に設定する。
                        byte[] fileData = null;
                        // ファイル復号化
                        using (var fileStream = new FileStream(dumpFile.DmpfilePath, FileMode.Open, FileAccess.Read))
                        {
                            // ファイル復号化
                            fileData = CryptoUtil.Decrypt(fileStream, Path.GetFileName(dumpFile.DmpfilePath));
                        }
                        getDumpFileResponse.dmpfile = fileData;

                        // 一時ダンプファイル更新（ログ出力：あり）
                        UpddateWDumpFileDownload(db, dumpFile, userId, sysDateTime);

                        // コミット
                        tx.Commit();
                    }

                    // 未ダウンロード件数取得（ログ出力：あり）
                    var fileNotDownLoadCnt = db.WDumpFileDownloads.Where(t => t.Token == token && CoreConst.FLG_OFF.Equals(t.DownloadFlg)).Count();
                    getDumpFileResponse.kensu = fileNotDownLoadCnt;
                }

                //返却データ
                return Ok(getDumpFileResponse);
            }
            catch (Exception ex)
            {
                // 「６．」でレスポンスデータの取得に失敗した場合
                // 例外として呼び出し元に以下を返却する。
                // ア） メッセージ　（メッセージID：ME01645 、引数１："ダンプファイルの取得"、 引数２："" ）
                // ※エラー発生時は、[ダンプファイル]、[残件数]は設定なしとする。
                // イ） HTTPステータス　500（INTERNAL SERVER ERROR）
                logger.Error(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));
                logger.Error(MessageUtil.Get("ME01645", "ダンプファイルの取得", ""));
                return CreateErrorResponse<ApiErrorResponseBase>("ME01645", "ダンプファイルの取得", "");
            }
        }

        #region 一時ダンプファイル登録
        /// <summary>
        /// 一時ダンプファイル登録
        /// </summary>
        /// <param name="sysDb">DB情報</param>
        /// <param name="dumpFiles">ダンプファイル情報</param>
        /// <param name="token">トークン</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="sysDateTime">システム時間</param>
        private void InsertWDumpFileDownload(SystemCommonContext sysDb, List<FileInfo> dumpFiles, string token, string userId, DateTime sysDateTime)
        {
            var dumpList = new List<WDumpFileDownload>();
            short renban = 0;
            foreach (var file in dumpFiles)
            {
                var dmpInfo = new WDumpFileDownload()
                {
                    Token = token,
                    Renban = ++renban,
                    DmpfilePath = file.FullName,
                    DownloadFlg = CoreConst.FLG_OFF,
                    InsertUserId = userId,
                    InsertDate = sysDateTime,
                    UpdateUserId = userId,
                    UpdateDate = sysDateTime,
                };

                dumpList.Add(dmpInfo);
            }

            // 登録するダンプファイルが空ではない場合
            if (!dumpList.IsNullOrEmpty())
            {
                sysDb.WDumpFileDownloads.AddRange(dumpList);
                // 一時ダンプファイルに登録する
                logger.Info("取得したダンプファイル数分のデータを システム共通スキーマの「一時ダンプファイル取得」に登録する。");
                sysDb.SaveChanges();
            }
        }
        #endregion

        #region 一時ダンプファイル更新
        /// <summary>
        /// 一時ダンプファイル更新
        /// </summary>
        /// <param name="sysDb">DB情報</param>
        /// <param name="dumpFile">一時ダンプファイル情報</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="sysDateTime">システム時間</param>
        private void UpddateWDumpFileDownload(SystemCommonContext sysDb, WDumpFileDownload dumpFile, string userId, DateTime sysDateTime)
        {
            dumpFile.DownloadFlg = CoreConst.FLG_ON;
            dumpFile.UpdateDate = sysDateTime;
            dumpFile.UpdateUserId = userId;

            // 一時ダンプファイル更新
            logger.Info("一時ダンプファイルを更新する。");
            sysDb.WDumpFileDownloads.Entry(dumpFile).State = EntityState.Modified;
            sysDb.SaveChanges();
        }
        #endregion
    }
}
