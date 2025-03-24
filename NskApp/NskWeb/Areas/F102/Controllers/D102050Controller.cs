using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis;
using Pipelines.Sockets.Unofficial.Arenas;
using NskCommonLibrary.Core.Consts;
using NskWeb.Areas.F102.Models.D102050;
using NskWeb.Areas.F000.Models.D000000;
using ModelLibrary.Models;
using NskAppModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F000.Models.D000999;
using NskWeb.Areas.F102.Consts;
using System.IO.Compression;
using NskWeb.Areas.F102.Models.D102010;

namespace NskWeb.Areas.F102.Controllers
{
    /// <summary>
    /// NSK_102010D_危険段階データ取込（危険段階別料率）（テキスト）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/01/08
    /// 作成者：ジョンジュンヒョ
    /// </remarks>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [Area("F102")]
    public class D102050Controller : CoreController
    {
        public D102050Controller(ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(viewEngine)
        {
        }



        // GET: F00/D2010/Init
        public ActionResult Init()
        {

            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(F102Const.SCREEN_ID_D102050, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }

            //var pagefrom = HttpContext.Request.Query[CoreConst.SCREEN_PAGE_FROM];
            var syokuin = SessionUtil.Get<Syokuin>("_D9000_LOGIN_USER", HttpContext);
            if (syokuin == null)
            {
                ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01033"));
                D000999Model d000999Model = GetInitModel();
                d000999Model.UserId = "";
                return View("D000999_Pre", d000999Model);
            }
            //// モデル初期化
            D102050Model model = new D102050Model
            {
                // 「ログイン情報」を取得する
                VSyokuinRecords = getJigyoDb<NskAppContext>().VSyokuins.Where(t => t.UserId == Syokuin.UserId).Single()
            };
            var updateKengen = ScreenSosaUtil.CanUpdate(F102Const.SCREEN_ID_D102050, HttpContext);
            model.UpdateKengenFlg = updateKengen;

            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (md != null)
            {
                model.D102050Info.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.D102050Info.SNensanHikiuke = md.SNensanHikiuke;
                model.D102050Info.SNensanHyoka = md.SNensanHyoka;

            }

            logger.Debug(md.SKyosaiMokutekiCd);

            try
            {
                var kyosai = getJigyoDb<NskAppContext>().M00010共済目的名称s
                              .Where(t => t.共済目的コード == md.SKyosaiMokutekiCd)
                              .Single();

                model.KyosaiMokutekiMeisho = kyosai.共済目的名称;
            }
            catch (Exception ex)
            {
                logger.Debug(ex.StackTrace);
                throw new AppException("MF80002", MessageUtil.Get("MF80002", "共済目的"));
            }

            // 初期表示情報をセッションに保存する
            SessionUtil.Set(F102Const.SESS_D102050, model, HttpContext);
            return View(F102Const.SCREEN_ID_D102050, model);
        }

        #region バッチ登録イベント
        /// <summary>
        /// イベント名：バッチ登録
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatBatchReport([Bind("TorikomiFilePath")] D102050Model form)
        {
            logger.Debug("START CreatBatchReport");
            var model = SessionUtil.Get<D102050Model>(F102Const.SESS_D102050, HttpContext);
            logger.Debug(model == null);
            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                return Json(new { success = false, message = MessageUtil.Get("ME01645", "セッション情報の取得") });
            }

            // バッチ予約状況取得引数の設定
            BatchUtil.GetBatchYoyakuListParam param = new()
            {
                SystemKbn = ConfigUtil.Get(CoreLibrary.Core.Consts.CoreConst.APP_ENV_SYSTEM_KBN),
                TodofukenCd = Syokuin.TodofukenCd,
                KumiaitoCd = Syokuin.KumiaitoCd,
                ShishoCd = Syokuin.ShishoCd,
                BatchNm = F102Const.BATCH_ID_NSK_102051B
            };
            // 総件数取得フラグ
            bool boolAllCntFlg = false;
            // 件数（出力パラメータ）
            int intAllCnt = 0;
            // エラーメッセージ（出力パラメータ）
            string message = string.Empty;

            // バッチ予約状況取得（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
            List<BatchYoyaku> batchYoyakuList = BatchUtil.GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message);

            // バッチ予約が存在し、かつ未実行のバッチが含まれている場合
            if (intAllCnt >= 1 && batchYoyakuList.Exists(b => b.BatchStatus == BatchUtil.BATCH_STATUS_WAITING))
            {
                // メッセージ表示して処理を中止する
                return Json(new { success = false, message = MessageUtil.Get("ME10019", "危険段階データ取込（危険段階地域区分）") });
            }

            // 独自チェック
            var errMessage = IsPrivateCheckError(form);
            // 独自チェックエラーの場合
            if (!string.IsNullOrEmpty(errMessage))
            {
                return Json(new { success = false, message = errMessage });
            }

            // アップロードファイル
            var uploadFile = Request.Form.Files[0];

            var importFilePath = Path.GetFullPath(uploadFile.FileName);
            var importFileName = Path.GetFileName(uploadFile.FileName);

            var extension = Path.GetExtension(uploadFile.FileName);
            var fileNameWithExtension = ChangeMacDakuten(Path.GetFileName(uploadFile.FileName));
            var fileNameWithPath = ChangeMacDakuten(uploadFile.FileName);
            var point = fileNameWithExtension.LastIndexOf(extension);
            var fileName = fileNameWithExtension.Substring(0, point);
            var modelVar = new D102050ModelVar
            {
                // ファイル名
                FileNameWithPath = fileNameWithPath,
                FileName = fileName,
                // ファイルサイズ
                //FileSize = uploadFile.ContentLength,
                FileSize = (int)uploadFile.Length,
                // ファイル識別名
                FileId = Guid.NewGuid().ToString()
            };

            // ユーザIDの取得
            var userId = Syokuin.UserId;
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();
            // 一時フォルダルート
            var tempRoot = "C:\\SYN\\tmp"; // TODO: aasettings.json に追加されたら置き換える →　 ConfigUtil.Get(InfraConst.FILE_TEMP_FOLDER_PATH);

            // 一時フォルダー　\yyyyMMdd\hhmmss\[セッション：ユーザ情報.ユーザID]
            var yyyyMMddHHmmssStr = systemDate.ToString("yyyyMMddHHmmss");
            // 削除対象パス(一時フォルダ)
            var deleteTargetTmpPath = Path.Combine(tempRoot, yyyyMMddHHmmssStr);
            var tempFolder = Path.Combine(deleteTargetTmpPath, userId);

            var uniqueFileName = modelVar.FileId + extension;

            // 一時ファイルパス
            var tempFilePath = Path.Combine(tempFolder, uniqueFileName);

            modelVar.TempFolder = tempFolder;
            modelVar.TempFilePath = tempFilePath;

            // アップロードファイルを一時フォルダに保存し、ウィルスチェック
            Directory.CreateDirectory(tempFolder);
            using (Stream fileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                uploadFile.CopyTo(fileStream);
            }

            // 削除対象パス(アップロードフォルダ)
            var deleteTargetUpPath = "";
            try
            {
                // ウィルスチェックのため、ウィルスチェック待ち時間分だけ待つ
                var uploadWaitTime = int.Parse("1") * 1000;
                Thread.Sleep(uploadWaitTime);

                // ファイルがなければ、ファイルが駆除されたのでエラーとする
                if (!System.IO.File.Exists(tempFilePath))
                {
                    return Json(new { success = false, message = MessageUtil.Get("ME90017", "") });
                }



                // ファイルデータをメモリに読み込む
                byte[] fileData = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    uploadFile.CopyTo(ms);
                    fileData = ms.ToArray();
                }

                // 暗号化処理
                var encryptData = CryptoUtil.Encrypt(fileData, fileNameWithExtension);

                var upRoot = "C:\\SYN\\up"; // TODO: aasettings.json にUPパスが追加されたら置き換える
                deleteTargetUpPath = upRoot + @"\" + yyyyMMddHHmmssStr;
                // アップロードフォルダの作成
                modelVar.UploadFolder = deleteTargetUpPath + @"\" + userId + @"\" + modelVar.FileName;

                // ZIPファイルパスの生成
                var zipFileName = Path.GetFileNameWithoutExtension(uniqueFileName) + ".zip";
                var zipFilePath = Path.Combine(modelVar.UploadFolder, zipFileName);


                if (!Directory.Exists(modelVar.UploadFolder))
                {
                    Directory.CreateDirectory(modelVar.UploadFolder);
                }

                // ZIP化して保存
                using (var zipStream = new FileStream(zipFilePath, FileMode.Create))
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    var zipEntry = archive.CreateEntry(uniqueFileName); // ZIP内のファイル名を設定
                    using (var entryStream = zipEntry.Open())
                    {
                        entryStream.Write(encryptData, 0, encryptData.Length); // 暗号化データを書き込む
                    }
                }

                modelVar.FileSavePath = zipFilePath;

                // 元ファイルのハッシュを取得
                modelVar.FileHash = CryptoUtil.GetSHA256Hex(fileData);


                // 一時フォルダを削除する
                Directory.Delete(tempFolder, true);
            }
            catch (Exception e)
            {
                logger.Error(e);
                // アップロードフォルダの削除
                deleteFolder(deleteTargetUpPath);

                return Json(new { success = false, message = MessageUtil.Get("ME10045") });
            }
            finally
            {
                // 一時保存フォルダの削除
                deleteFolder(deleteTargetTmpPath);
            }

            // 条件IDを取得する
            // Guid.NewGuid().ToString();
            String strJoukenId = Guid.NewGuid().ToString();
            string strJoukenErrorMsg = string.Empty;

            // 条件を登録する
            //InsertTJouken(model, strJoukenId, form.TorikomiFilePath, modelVar.FileHash);
            strJoukenErrorMsg = InsertTJouken(model, strJoukenId, modelVar.UploadFolder, modelVar.FileHash);
            if (!string.IsNullOrEmpty(strJoukenErrorMsg))
            {
                return Json(new { success = false, message = strJoukenErrorMsg });
            }

            // バッチ予約登録
            var refMsg = string.Empty;
            long batchId = 0;
            // バッチ条件（表示用）作成
            var displayJouken = form.TorikomiFilePath;
            //ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
            // バッチ予約登録
            int? result = null;
            try
            {
                logger.Info("バッチ予約登録処理を実行する。");
                result = BatchUtil.InsertBatchYoyaku(AppConst.BatchBunrui.BATCH_BUNRUI_04_FILE_IMPORT,
                "02",
                Syokuin.TodofukenCd,
                Syokuin.KumiaitoCd,
                Syokuin.ShishoCd,
                DateUtil.GetSysDateTime(),
                Syokuin.UserId,
                F102Const.SCREEN_ID_NSK_D102050,
                F102Const.BATCH_ID_NSK_102051B,
                strJoukenId,
                displayJouken,
                AppConst.FLG_OFF,
                AppConst.BatchType.BATCH_TYPE_PATROL,
                null,
                AppConst.FLG_OFF,
                ref refMsg,
                ref batchId
                );
            }
            catch (Exception e)
            {
                // アップロードフォルダの削除
                deleteFolder(deleteTargetUpPath);
                // （出力メッセージ：バッチ予約登録失敗）
                // （出力メッセージ：（メッセージID：ME90008、引数{0}："（"+｢４．２．３．｣で返却されたメッセージ + "）"））
                logger.Error("バッチ予約登録失敗1");
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                //logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                logger.Error(MessageUtil.GetErrorMessage(e, 10));
                return Json(new { success = false, message = MessageUtil.Get("ME90005") });
            }

            // 処理結果がエラーだった場合
            if (result == 0)
            {
                // アップロードフォルダの削除
                deleteFolder(deleteTargetUpPath);
                // （出力メッセージ：バッチ予約登録失敗）
                // （出力メッセージ：（メッセージID：ME90008、引数{0}："（"+｢４．２．３．｣で返却されたメッセージ + "）"））
                logger.Error("バッチ予約登録失敗2");
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                return Json(new { success = false, message = MessageUtil.Get("ME90005") });
            }


            // （出力メッセージ：バッチ予約登録成功）
            logger.Info("バッチ予約登録成功");
            return Json(new { success = true, message = MessageUtil.Get("MI00004", "危険段階データ取込バッチの予約") });
        }
        #endregion

        #region フォルダ削除
        /// <summary>
        /// イベント：フォルダ削除
        /// </summary>
        /// <param name="targetPath">削除対象パス</param>
        private void deleteFolder(string targetPath)
        {
            if (Directory.Exists(targetPath))
            {
                // フォルダの削除
                FolderUtil.DeleteTempFolder(targetPath);
            }
        }
        #endregion

        #region 独自チェック
        /// <summary>
        /// イベント：アップロード
        /// </summary>
        /// <param name="form">画面の入力値</param>
        /// <param name="request">Web 要求中にクライアントから送信された HTTP 値を ASP.NET で読み取れるようにするクラスの基本クラス</param>
        /// <returns>エラーメッセージ</returns>
        private string IsPrivateCheckError(D102050Model form)
        {
            logger.Debug("START IsPrivateCheckError");
            var errMessage = string.Empty;
            // ファイルサイズの合計値
            var fileTotalSize = 0;
            // ファイル対象リスト
            var files = Request.Form.Files;

            try
            {
                // [アップロードするファイル]にファイルが存在しない場合
                if (Request.Form.Files.Count == 0)
                {
                    errMessage = MessageUtil.Get("ME01645", "取込ファイルの取得");
                    ModelState.AddModelError("TorikomiFilePath", " ");

                    return errMessage;
                }

                var uploadFile = Request.Form.Files[0];
                var fileName = uploadFile.FileName;
                var fileSize = uploadFile.Length;
                var fileExtension = Path.GetExtension(fileName).Replace(".", "");
                var fileNameWithoutPath = Path.GetFileName(fileName);

                // 【追加】拡張子とMIMEタイプのチェック
                if (fileExtension != F102Const.allowedExtension || uploadFile.ContentType != F102Const.allowedMimeType)
                {
                    errMessage = MessageUtil.Get("ME10050");
                    ModelState.AddModelError("TorikomiFilePath", "");
                    return errMessage;
                }

                // ファイル名文字数をチェック
                if (fileName.Length > F102Const.fileNameMaxLength)
                {
                    errMessage = MessageUtil.Get("ME91008", $"{F102Const.fileNameMaxLength}");
                    ModelState.AddModelError("TorikomiFilePath", "");
                    return errMessage;
                }

                // アップロードするファイルサイズがuploadFileMaxSizeを超えている場合
                if (fileTotalSize > F102Const.uploadFileMaxSize)
                {
                    errMessage = MessageUtil.Get("ME10049");
                    ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01421", F102Const.uploadFileMaxDispSize));
                    ModelState.AddModelError("TorikomiFilePath", "");
                    return errMessage;
                }
                // 外字チェック
                // [アップロードするファイル]
                if (fileName != null && !"".Equals(fileName))
                {
                    // MS932以外の文字や外字を取得。
                    var exceptedString = StringUtil.CheckMS932ExceptGaiji(ChangeMacDakuten(fileName));
                    if (exceptedString != null && exceptedString.Length != 0)
                    {
                        errMessage = MessageUtil.Get("ME91011", "exceptedString", "");
                        ModelState.AddModelError("TorikomiFilePath", "");
                    }
                }
                return errMessage;
            }
            catch
            {
                errMessage = MessageUtil.Get("ME10045");
                return errMessage;
            }
        }
        #endregion

        #region 画面設定値保持
        /// <summary>
        /// 画面設定値保持
        /// </summary>
        /// <param name="form">画面フォーム</param>
        /// <param name="model">ビューモデル</param>
        private void SetFormToModel(D102050Model form, D102050Model model)
        {
            // アップロードするファイル
            model.TorikomiFilePath = form.TorikomiFilePath;
        }
        #endregion

        #region Macの濁点や半濁点を変換する
        /// <summary>
        /// Macの濁点や半濁点を変換する
        /// </summary>
        /// <param name="str">変更対象</param>
        /// <returns>返還結果</returns>
        private string ChangeMacDakuten(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            string result = str;
            // NFCかどうかを判定
            if (!result.IsNormalized())
            {
                result = result.Normalize();
            }
            return result;
        }
        #endregion


        #region 条件登録メッソド
        /// <summary>
        /// メッソド:条件登録
        /// </summary>
        /// <param name="model">画面モデル</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>登録結果</returns>
        private string InsertTJouken(D102050Model model, string joukenId, string filePath, string fileHash)
        {
            // ユーザID
            var userId = Syokuin.UserId;
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // NSKポータル情報の取得
            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);

            // 連番を手動で初期化
            int serialNumber = 0;

            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , Syokuin.TodofukenCd
                , Syokuin.KumiaitoCd
                , Syokuin.ShishoCd);

            logger.Debug("filePath : " + filePath.Length);
            logger.Debug("fileHash : " + fileHash.Length);
            logger.Debug("md.SNensanHikiuke : " + md.SNensanHikiuke);


            // DbContext を一度だけ使用する
            using (var db1 = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                // トランザクションを開始
                var transaction = db1.Database.BeginTransaction();

                try
                {
                    // 条件情報を追加する共通処理
                    void AddCondition(string conditionName, string value)
                    {
                        foreach (var segment in SplitByLength(value, 30))
                        {
                            var condition = new T01050バッチ条件
                            {
                                バッチ条件id = joukenId,
                                連番 = ++serialNumber,
                                条件名称 = conditionName,
                                表示用条件値 = segment,
                                条件値 = segment,
                                登録日時 = systemDate,
                                登録ユーザid = userId,
                                更新日時 = systemDate,
                                更新ユーザid = userId
                            };
                            db1.T01050バッチ条件s.Add(condition);
                        }
                    }

                    // filePath と fileHash の条件を処理
                    AddCondition("ファイルパス", filePath);
                    AddCondition("ファイルハッシュ", fileHash);

                    // fileNensanの条件をそのままINSERT
                    var fileNensan = new T01050バッチ条件
                    {
                        バッチ条件id = joukenId,
                        連番 = ++serialNumber,
                        条件名称 = JoukenNameConst.JOUKEN_NENSAN,
                        表示用条件値 = md.SNensanHikiuke,
                        条件値 = md.SNensanHikiuke,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01050バッチ条件s.Add(fileNensan);

                    // すべてのエンティティを一括保存
                    db1.SaveChanges();

                    // トランザクションコミット
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Debug($"Exception: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        logger.Debug($"Inner Exception: {ex.InnerException.Message}");
                        if (ex.InnerException.InnerException != null)
                        {
                            logger.Debug($"Inner Exception (Level 2): {ex.InnerException.InnerException.Message}");
                        }
                    }
                    logger.Debug($"StackTrace: {ex.StackTrace}");
                    logger.Error("バッチ条件登録失敗");
                    transaction.Rollback();
                    return MessageUtil.Get("ME90005");
                }
            }
            return string.Empty;
        }

        // 30文字ずつに分割するヘルパーメソッド
        IEnumerable<string> SplitByLength(string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                yield break;
            for (int i = 0; i < value.Length; i += length)
            {
                yield return value.Substring(i, Math.Min(length, value.Length - i));
            }
        }

        #endregion

        /// <summary>
        /// 初期モデルの取得メソッド。
        /// </summary>
        /// <returns>初期モデル</returns>
        private D000999Model GetInitModel()
        {
            D000999Model model = new D000999Model();

            List<MTodofuken> todofukenList = TodofukenUtil.GetTodofukenList().ToList();
            if (todofukenList.Count() > 0)
            {
                model.TodofukenCd = todofukenList[0].TodofukenCd;
                model.TodofukenNm = todofukenList[0].TodofukenNm;
                List<MKumiaito> kumiaitoList = KumiaitoUtil.GetKumiaitoList(model.TodofukenCd);
                if (kumiaitoList.Count() > 0)
                {
                    model.KumiaitoCd = kumiaitoList[0].KumiaitoCd;
                    model.KumiaitoNm = kumiaitoList[0].KumiaitoNm;
                    List<MShishoNm> shishoList = ShishoUtil.GetShishoList(model.TodofukenCd, model.KumiaitoCd);
                    if (shishoList.Count() > 0)
                    {
                        model.ShishoCd = shishoList[0].ShishoCd;
                        model.ShishoNm = shishoList[0].ShishoNm;
                    }
                }
            }

            model.ScreenMode = "1";

            return model;
        }

        /// <summary>
        /// サーバー側でのファイル存在確認
        /// </summary>
        /// <returns>チェック結果</returns>
        public JsonResult CheckFileExistence(string fileName)
        {
            try
            {
                string rootDirectory = "C:\\Backup";
                string searchPattern = fileName;　// 完全一致ファイル名

                // 指定されたファイル名に一致するファイルをすべてのサブディレクトリで検索
                var files = Directory.GetFiles(rootDirectory, searchPattern, SearchOption.AllDirectories);

                // ファイルが存在するか確認
                bool fileExists = files.Length > 0;

                return Json(new { exists = fileExists });
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.Debug($"StackTrace: {ex.StackTrace}");
                // アクセス権限エラーが発生した場合の処理
                return Json(new { exists = false, error = "ME10003" });
            }
            catch (Exception ex)
            {
                // その他のエラー
                return Json(new { exists = false, error = ex.Message });
            }
        }
    }
}