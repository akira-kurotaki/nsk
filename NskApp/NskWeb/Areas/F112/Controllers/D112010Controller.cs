using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis;
using Pipelines.Sockets.Unofficial.Arenas;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F112.Models.D112010;
using NskWeb.Areas.F000.Models.D000000;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Pager;
using NskWeb.Areas.F000.Models.D000999;
using ModelLibrary.Models;
using NskWeb.Areas.F102.Models.D102010;
using System.IO.Compression;
using NskAppModelLibrary.Models;
using System.Text.RegularExpressions;

namespace NskWeb.Areas.F112.Controllers
{
    /// <summary>
    /// NSK_102010D_危険段階データ取込（危険段階別料率）（テキスト）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/01/08
    /// 作成者：
    /// </remarks>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [Area("F112")]
    public class D112010Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D2010)
        /// </summary>
        private static readonly string SCREEN_ID_D112010 = "D112010";

        /// <summary>
        /// セッションキー(D102010)
        /// </summary>
        private static readonly string SESS_D112010 = "D112010_SCREEN";

        /// <summary>
        /// ページ0
        /// </summary>
        private static readonly string PAGE_0 = "0";

        /// <summary>
        /// 部分ビュー名（検索結果）
        /// </summary>
        private static readonly string RESULT_VIEW_NAME = "_D112010SearchResult";

        /// <summary>
        /// 予約を実行した処理名
        /// </summary>
        private static readonly string D112010_REPORT_YOYAKU_SHORI_NM = "NSK_112010D";
        /// <summary>
        /// バッチ名
        /// </summary>
        private static readonly string D112010_BATCH_NM = "危険段階データ取込（危険段階別料率）（テキスト）";

        public D112010Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }
        #endregion

        

        // GET: F00/D2010/Init
        public ActionResult Init()
        {
            SessionUtil.Remove(SESS_D112010, HttpContext);
            ModelState.Clear();
            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D112010, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }
            //var pagefrom = HttpContext.Request.Query[CoreConst.SCREEN_PAGE_FROM];
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);
            if (syokuin == null)
            {
                ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01033"));
                D000999Model d000999Model = GetInitModel();
                d000999Model.UserId = "";
                return View("D000999_Pre", d000999Model);
            }

            var m00190Result = getJigyoDb<NskAppContext>()
                .M00190大量データ対象データs 
                .Where(t => t.業務区分 == "1"
                         && t.受入バッチid != null
                         && t.取込バッチid != null)
                .OrderBy(t => t.対象データ区分)
                .Select(t => new {
                    t.受入対象データ名称,
                    t.対象データ区分
                })
                .ToList();

            var vShishoResult = getJigyoDb<NskAppContext>()
                .VShishoNms  
                .Where(t => t.KumiaitoCd == syokuin.KumiaitoCd)
                .OrderBy(t => t.ShishoCd)
                .Select(t => new {
                    t.ShishoCd,
                    t.ShishoNm
                })
                .ToList();
            var t01060Result = getJigyoDb<NskAppContext>()
                .T01060大量データ受入履歴s   
                .Where(t => t.組合等コード == syokuin.KumiaitoCd)
                .Select(t => t.登録ユーザid)
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            if (!t01060Result.Contains(syokuin.UserId))
            {
                t01060Result.Add(syokuin.UserId);
                // 追加後にリストを再ソートする（必要に応じて）
                t01060Result = t01060Result.OrderBy(id => id).ToList();
            }

            var t01070Result = getJigyoDb<NskAppContext>()
                .T01070大量データ取込履歴s
                .Where(t => t.組合等コード == syokuin.KumiaitoCd)
                .Select(t => t.登録ユーザid)
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            if (!t01070Result.Contains(syokuin.UserId))
            {
                t01070Result.Add(syokuin.UserId);
                // 追加後にリストを再ソートする（必要に応じて）
                t01070Result = t01070Result.OrderBy(id => id).ToList();
            }

            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            //// モデル初期化
            D112010Model model = new D112010Model(Syokuin, shishoList)
            {
                // 「ログイン情報」を取得する
                VSyokuinRecords = getJigyoDb<NskAppContext>().VSyokuins.Where(t => t.UserId == Syokuin.UserId).Single()
            };

            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (md != null)
            {
                model.D112010Info.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.D112010Info.SNensanHikiuke = md.SNensanHikiuke;
                model.D112010Info.SNensanHyoka = md.SNensanHyoka;
            }
            model.SearchCondition.IncludeOtherUserHistoryFlg = false;
            model.SearchCondition.UkeireUserId = Syokuin.UserId;
            model.SearchConditionTorikomi.TorikomiUserId = Syokuin.UserId;

            // 初期表示情報をセッションに保存する
            SessionUtil.Set(SESS_D112010, model, HttpContext);
            return View(SCREEN_ID_D112010, model);
        }

        #region バッチ登録イベント
        /// <summary>
        /// イベント名：バッチ登録
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatBatchReport(D112010Model form)
        {
            logger.Debug("START CreatBatchReport");
            var model = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);
            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            logger.Debug("form@@@@@@@@@@@@@@@@@@");
            logger.Debug("form.UkeireFilePath : " + form.UkeireFilePath);
            logger.Debug("form.UkeireTaisho : " + form.UkeireTaisho);
            logger.Debug("form.Comment : " + form.Comment);
            logger.Debug("form@@@@@@@@@@@@@@@@@@");

            

            // 独自チェックエラーの場合
            if (IsPrivateCheckError(form))
            {
                ModelState.AddModelError("MessageArea", MessageUtil.Get("ME91009"));

                // 画面情報をセッションに保存する
                SessionUtil.Set(SESS_D112010, model, HttpContext);

                return View(SCREEN_ID_D112010, model);
            }

            // アップロードファイル
            var uploadFile = Request.Form.Files[0];

            var importFilePath = Path.GetFullPath(uploadFile.FileName);
            var importFileName = Path.GetFileName(uploadFile.FileName);
            logger.Debug("importFileName:" + importFileName);
            logger.Debug("importFilePath:" + importFilePath);

            var extension = Path.GetExtension(uploadFile.FileName);
            var fileNameWithExtension = ChangeMacDakuten(Path.GetFileName(uploadFile.FileName));
            logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            logger.Debug("fileNameWithExtension ; " + fileNameWithExtension);
            logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            var fileNameWithPath = ChangeMacDakuten(uploadFile.FileName);
            var point = fileNameWithExtension.LastIndexOf(extension);
            var fileName = fileNameWithExtension.Substring(0, point);
            var modelVar = new D102010ModelVar
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
            /*
            * TODO:ファイル格納先と処理のロジックが決まってないため、
            * 必要に応じて修正
            */
            var testTetmp = "C:\\SYN\\2011";

            // 一時フォルダー(定数ID：D0401UploadTempFolder)\yyyyMMdd\hhmmss\[セッション：ユーザ情報.ユーザID]
            var yyyyMMddHHmmssStr = systemDate.ToString("yyyyMMddHHmmss");
            //var tempFolder = Path.Combine(ConfigUtil.Get(InfraConst.D102010_UPLOAD_TEMP_FOLDER), yyyyMMddHHmmssStr, userId);
            var tempFolder = Path.Combine(testTetmp, yyyyMMddHHmmssStr, userId);

            var uniqueFileName = modelVar.FileId + extension;
            logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            logger.Debug("uniqueFileName ; " + uniqueFileName);
            logger.Debug("modelVar.FileName ; " + modelVar.FileName);
            logger.Debug("extension ; " + extension);
            logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

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

            try
            {
                // ウィルスチェックのため、ウィルスチェック待ち時間分だけ待つ
                var uploadWaitTime = int.Parse("1") * 1000;
                Thread.Sleep(uploadWaitTime);

                // ファイルがなければ、ファイルが駆除されたのでエラーとする
                if (!System.IO.File.Exists(tempFilePath))
                {
                    if (Directory.Exists(tempFolder))
                    {
                        Directory.Delete(tempFolder, true);
                    }
                    ModelState.AddModelError("MessageArea", MessageUtil.Get("ME90017", fileNameWithExtension));

                    // 画面入力値を保持する
                    SetFormToModel(form, model);

                    // 画面情報をセッションに保存する
                    SessionUtil.Set(SESS_D112010, model, HttpContext);

                    return View(SCREEN_ID_D112010, model);
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

                var testTetmpReal = "C:\\SYN\\112011Real";
                // アップロードフォルダの作成
                //modelVar.UploadFolder = ConfigUtil.Get(InfraConst.D102010_UPLOAD_FOLDER) + @"\" + yyyyMMddHHmmssStr + @"\" + userId + @"\" + modelVar.FileName;
                modelVar.UploadFolder = testTetmpReal + @"\" + yyyyMMddHHmmssStr + @"\" + userId + @"\" + modelVar.FileName;

                // ZIPファイルパスの生成
                var zipFileName = Path.GetFileNameWithoutExtension(modelVar.FileName) + ".zip";
                var zipFilePath = Path.Combine(modelVar.UploadFolder, zipFileName);

                logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                logger.Debug("uniqueFileName ; " + uniqueFileName);
                logger.Debug("zipFileName ; " + zipFileName);
                logger.Debug("zipFilePath ; " + zipFilePath);
                logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

                if (!Directory.Exists(modelVar.UploadFolder))
                {
                    Directory.CreateDirectory(modelVar.UploadFolder);
                }

                // ZIP化して保存
                using (var zipStream = new FileStream(zipFilePath, FileMode.Create))
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    var zipEntry = archive.CreateEntry(modelVar.FileName + extension); // ZIP内のファイル名を設定
                    using (var entryStream = zipEntry.Open())
                    {
                        entryStream.Write(encryptData, 0, encryptData.Length); // 暗号化データを書き込む
                    }
                }

                modelVar.FileSavePath = zipFilePath;

                // 元ファイルのハッシュを取得
                modelVar.FileHash = CryptoUtil.GetSHA256Hex(fileData);
                // 一時フォルダを削除する
                var parentFolder = Directory.GetParent(tempFolder).FullName; // これで "C:\SYN\2011\20250313193648" を取得
                Directory.Delete(parentFolder, true);
            }
            catch (Exception e)
            {
                logger.Error(e);
                if (Directory.Exists(tempFolder))
                {
                    // 一時フォルダを削除する
                    var parentFolder = Directory.GetParent(tempFolder).FullName; // これで "C:\SYN\2011\20250313193648" を取得
                    Directory.Delete(parentFolder, true);
                }
                return Json(new { message = MessageUtil.Get("ME90005") });
            }

            // 条件IDを取得する
            // Guid.NewGuid().ToString();
            String strJoukenId = Guid.NewGuid().ToString("D");
            string strJoukenErrorMsg = string.Empty;

            // カンマで分割して配列に格納
            var parts = form.UkeireTaisho.Split(',');

            // 分割した結果をそれぞれの変数に格納
            var targetDataKbn = parts[0];
            var targetDataName = parts[1];

            logger.Debug("対象データ区分:"+ targetDataKbn);
            logger.Debug("受入対象データ名称:"+ targetDataName);

            int newId = getJigyoDb<NskAppContext>().T01060大量データ受入履歴s
                    .AsEnumerable() // ここでデータをクライアント側に取得
                    .Select(x => string.IsNullOrEmpty(x.受入履歴id) ? 0 : int.Parse(x.受入履歴id))
                    .DefaultIfEmpty(0)
                    .Max() + 1;

            // 条件を登録する
            //InsertTJouken(model, strJoukenId, form.TorikomiFilePath, modelVar.FileHash);
            strJoukenErrorMsg = InsertTJouken(
                form
                , strJoukenId
                , modelVar.FileHash
                , modelVar.FileName
                , modelVar.TempFolder
                , targetDataKbn
                , newId.ToString()
                , modelVar.FileName);
            if (!string.IsNullOrEmpty(strJoukenErrorMsg))
            {
                return Json(new { message = strJoukenErrorMsg });
            }
            
            var 複数実行禁止ID = string.Empty;
            var 予約バッチ名 = string.Empty;
            if ("基準収穫量".Equals(targetDataName))
            {
                複数実行禁止ID = "NSK_112041B";
                予約バッチ名 = "基準収穫量大量データ取込";
            }
            else if ("組合等類別設定".Equals(targetDataName))
            {
                複数実行禁止ID = "NSK_112081B";
                予約バッチ名 = "組合員等類別設定大量データ取込";
            }
            else if ("組合等類別平均単収".Equals(targetDataName))
            {
                複数実行禁止ID = "NSK_112121B";
                予約バッチ名 = "組合等類別平均単収大量データ取込";
            }
            else if ("加入申込書（細目・カンマ）".Equals(targetDataName)
                  || "加入申込書（細目・GISカンマ）".Equals(targetDataName)
                  || "加入申込書（細目・固定長）".Equals(targetDataName))
            {
                複数実行禁止ID = "NSK_112190B";
                予約バッチ名 = "加入申込書大量受入データ取込";
            }

            // バッチ予約登録
            var refMsg = string.Empty;
            long batchId = 0;
            // バッチ条件（表示用）作成
            var displayJouken = targetDataName;
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
                D112010_REPORT_YOYAKU_SHORI_NM,
                予約バッチ名,
                strJoukenId,
                displayJouken,
                AppConst.FLG_ON,
                AppConst.BatchType.BATCH_TYPE_PATROL,
                null,
                AppConst.FLG_OFF,
                ref refMsg,
                ref batchId,
                複数実行禁止ID+Syokuin.TodofukenCd
                );
            }
            catch (Exception e)
            {
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                //logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                logger.Error(MessageUtil.GetErrorMessage(e, 10));
                return Json(new { message = MessageUtil.Get("ME90005") });
            }

            // 処理結果がエラーだった場合
            if (result == 0)
            {
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , Syokuin.TodofukenCd
                , Syokuin.KumiaitoCd
                , Syokuin.ShishoCd);
                using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
                {
                    // トランザクションを開始
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        // t_01070_大量データ取込履歴 テーブルの削除
                        var recordsToDelete1 = db.T01060大量データ受入履歴s
                            .Where(r => r.受入履歴id == newId.ToString());

                        foreach (var record in recordsToDelete1)
                        {
                            db.T01060大量データ受入履歴s.Remove(record);
                        }

                        // t_01050_バッチ条件 テーブルの削除
                        var recordsToDelete2 = db.T01050バッチ条件s
                            .Where(r => r.バッチ条件id == strJoukenId);

                        foreach (var record in recordsToDelete2)
                        {
                            db.T01050バッチ条件s.Remove(record);
                        }

                        // すべてのエンティティの削除を一括保存
                        db.SaveChanges();

                        // トランザクションコミット
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
                
                return Json(new { message = MessageUtil.Get("ME90005") });
            }

            // （出力メッセージ：バッチ予約登録成功）
            logger.Info("バッチ予約登録成功");
            return Json(new { message = MessageUtil.Get("MI90002", "引受大量データ受入処理", "受入対象") });
        }
        #endregion

        #region 独自チェック
        /// <summary>
        /// イベント：アップロード
        /// </summary>
        /// <param name="form">画面の入力値</param>
        /// <param name="request">Web 要求中にクライアントから送信された HTTP 値を ASP.NET で読み取れるようにするクラスの基本クラス</param>
        /// <returns>エラーが存在するかどうか</returns>
        private bool IsPrivateCheckError(D112010Model form)
        {
            logger.Debug("START IsPrivateCheckError");
            var checkErrorFlg = false;
            // 各ファイルサイズの合計値
            var fileTotalSize = 0;
            // ファイル対象リスト
            var files = Request.Form.Files;
            // アップロード可能な拡張子
            var allowedExtension = ".csv";
            var extensions = allowedExtension.Split(',');

            // [アップロードするファイル]にファイルが存在しない場合
            if (Request.Form.Files.Count == 0)
            {
                ModelState.AddModelError("MessageArea", MessageUtil.Get("ME91005", "アップロードするファイル"));
                ModelState.AddModelError("TorikomiFilePath", " ");

                checkErrorFlg = true;
                return checkErrorFlg;
            }

            var uploadFile = Request.Form.Files[0];
            var fileName = uploadFile.FileName;
            var fileSize = uploadFile.Length;
            var fileExtension = Path.GetExtension(fileName).Replace(".", "");
            var fileNameWithoutPath = Path.GetFileName(fileName);

            // アップロードするファイルサイズが「(定数ID：D102010UploadFileMaxSize)の値」を超えている場合
            //var d102010UploadFileMaxSize = int.Parse(ConfigUtil.Get(InfraConst.D102010_UPLOAD_FILE_MAX_SIZE));
            var d102010UploadFileMaxSize = 10000000;
            if (fileTotalSize > d102010UploadFileMaxSize)
            {
                //ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01421", ConfigUtil.Get(InfraConst.D102010_UPLOAD_FILE_MAX_DISP_SIZE)));
                ModelState.AddModelError("TorikomiFilePath", "");
                checkErrorFlg = true;
            }
            // 外字チェック
            // [アップロードするファイル]
            if (fileName != null && !"".Equals(fileName))
            {
                // MS932以外の文字や外字を取得。
                var exceptedString = StringUtil.CheckMS932ExceptGaiji(ChangeMacDakuten(fileName));
                if (exceptedString != null && exceptedString.Length != 0)
                {
                    ModelState.AddModelError("MessageArea", MessageUtil.Get("ME90015", "ファイル名に" + exceptedString));
                    ModelState.AddModelError("UkeireFilePath", "");
                    checkErrorFlg = true;
                }
            }
            return checkErrorFlg;
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

        #region 画面設定値保持
        /// <summary>
        /// 画面設定値保持
        /// </summary>
        /// <param name="form">画面フォーム</param>
        /// <param name="model">ビューモデル</param>
        private void SetFormToModel(D112010Model form, D112010Model model)
        {
            // アップロードするファイル
            model.UkeireFilePath = form.UkeireFilePath;
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
        private string InsertTJouken(
            D112010Model model
            , string joukenId
            , string fileHash
            , string fileZipName
            , string tempFolder
            , string targetDataKbn
            , string paramUkeireRirekiId
            , string fileName)
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

            // DbContext を一度だけ使用する
            using (var db1 = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                // トランザクションを開始
                var transaction = db1.Database.BeginTransaction();

                try
                {
                    var ukeireRireki = new T01060大量データ受入履歴
                    {
                        受入履歴id = paramUkeireRirekiId,
                        データ登録日時 = systemDate,
                        組合等コード = Syokuin.KumiaitoCd,
                        ステータス = "01",
                        対象データ区分 = targetDataKbn,
                        取込区分 = "0",
                        取込ファイル_変更前ファイル名 = fileName,
                        取込ファイル_変更後ファイル名 = fileZipName,
                        取込ファイルハッシュ値 = fileHash,
                        コメント = model.Comment,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01060大量データ受入履歴s.Add(ukeireRireki);
                    // fileNensanはそのままINSERT（分割不要）
                    var fileNensan = new T01050バッチ条件
                    {
                        バッチ条件id = joukenId,
                        連番 = ++serialNumber,
                        条件名称 = "年産",
                        表示用条件値 = md.SNensanHikiuke,
                        条件値 = md.SNensanHikiuke,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01050バッチ条件s.Add(fileNensan);

                    var ukeireRirekiId = new T01050バッチ条件
                    {
                        バッチ条件id = joukenId,
                        連番 = ++serialNumber,
                        条件名称 = "受入履歴ID",
                        表示用条件値 = paramUkeireRirekiId,
                        条件値 = paramUkeireRirekiId,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01050バッチ条件s.Add(ukeireRirekiId);

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
                    if (Directory.Exists(tempFolder))
                    {
                        // 一時フォルダを削除する
                        var parentFolder = Directory.GetParent(tempFolder).FullName;
                        Directory.Delete(parentFolder, true);
                    }
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

        #region 受入履歴検索イベント
        /// <summary>
        /// イベント名：受入履歴検索
        /// </summary>
        /// <param name="model">D112010Model (SearchCondition 部分のみバインド)</param>
        /// <returns>画面全体の View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind("SearchCondition")] D112010Model model)
        {
            
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);

            // 受入側の情報を設定
            model.VSyokuinRecords = new VSyokuin
            {
                ShishoCd = syokuin.ShishoCd,
                TodofukenCd = syokuin.TodofukenCd,
                KumiaitoCd = syokuin.KumiaitoCd,
                UserId = syokuin.UserId
            };

            logger.Debug("受入検索条件 : " + model.SearchCondition.UkeireUserId);

            // 属性チェックまたは独自チェックエラーの場合
            if (CheckSearchCondition(model))
            {
                model.SearchCondition.IsResultDisplay = false;
                // 既存のセッションモデルがあれば、取込側情報はそのまま保持する
                var sessionModel = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext) ?? new D112010Model();
                // 受入側の検索条件のみ上書き
                sessionModel.SearchCondition = model.SearchCondition;
                // 受入側の検索結果はクリア（エラーの場合）
                sessionModel.SearchResult = new D112010SearchResult();
                // 取込側（SearchConditionTorikomi, SearchResultTorikomi）は変更せず
                SessionUtil.Set(SESS_D112010, sessionModel, HttpContext);
                return View(SCREEN_ID_D112010, sessionModel);
            }

            // エラーがない場合、受入側の検索結果を更新
            var updatedModel = GetPageDataList(1, model); // GetPageDataList 内で受入側(SearchResult)が更新される

            // マージ：既存のセッションモデルから取込側情報を引き継ぐ
            var existingModel = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);
            if (existingModel != null)
            {
                updatedModel.SearchResultTorikomi = existingModel.SearchResultTorikomi;
                updatedModel.SearchConditionTorikomi = existingModel.SearchConditionTorikomi;
            }
            updatedModel.ActiveTab = "UkeireTab";

            // ここでリクエストパラメータ "searchMsg" をチェック
            if (!string.IsNullOrEmpty(Request.Form["searchMsgUkeire"]) && Request.Form["searchMsgUkeire"] == "delete")
            {
                ModelState.AddModelError("MessageArea3", MessageUtil.Get("MI00004", "削除"));
            }
            SessionUtil.Set(SESS_D112010, updatedModel, HttpContext);
            return View(SCREEN_ID_D112010, updatedModel);
        }
        #endregion

        #region 取込履歴検索イベント
        /// <summary>
        /// イベント名：取込履歴検索
        /// </summary>
        /// <param name="model">D112010Model (SearchConditionTorikomi 部分のみバインド)</param>
        /// <returns>画面全体の View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchTorikomi([Bind("SearchConditionTorikomi")] D112010Model model)
        {
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);

            // 取込側の情報を設定
            model.VSyokuinRecords = new VSyokuin
            {
                ShishoCd = syokuin.ShishoCd,
                TodofukenCd = syokuin.TodofukenCd,
                KumiaitoCd = syokuin.KumiaitoCd,
                UserId = syokuin.UserId
            };

            // 属性チェックまたは独自チェックエラーの場合
            if (CheckSearchCondition(model))
            {
                model.SearchConditionTorikomi.IsResultDisplay = false;
                // 既存のセッションモデルがあれば、受入側情報はそのまま保持する
                var sessionModel = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext) ?? new D112010Model();
                // 取込側の検索条件のみ上書き
                sessionModel.SearchConditionTorikomi = model.SearchConditionTorikomi;
                // 取込側の検索結果はクリア
                sessionModel.SearchResultTorikomi = new D112010SearchResultTorikomi();
                // 受入側は変更せず
                SessionUtil.Set(SESS_D112010, sessionModel, HttpContext);
                return View(SCREEN_ID_D112010, sessionModel);
            }

            // エラーがない場合、取込側の検索結果を更新
            var updatedModel = GetPageDataListTorikomi(1, model); // GetPageDataListTorikomi 内で取込側(SearchResultTorikomi)が更新される

            // マージ：既存のセッションモデルから受入側情報を引き継ぐ
            var existingModel = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);
            if (existingModel != null)
            {
                updatedModel.SearchResult = existingModel.SearchResult;
                updatedModel.SearchCondition = existingModel.SearchCondition;
            }
            updatedModel.ActiveTab = "TorikomiTab";
            SessionUtil.Set(SESS_D112010, updatedModel, HttpContext);
            return View(SCREEN_ID_D112010, updatedModel);
        }
        #endregion

        #region ページ分データ取得（受入）
        /// <summary>
        /// 受入側の検索結果（ページ分データ）を取得する
        /// </summary>
        private D112010Model GetPageDataList(int? pageId, D112010Model model)
        {
            // 現在の受入側の検索結果を初期化
            ModelState.Clear();
            model.SearchResult = new D112010SearchResult();
            model.SearchCondition.IsResultDisplay = true;
            // ユーザ情報をセッションから取得し、受入側のユーザ情報を再設定
            var updateKengen = ScreenSosaUtil.CanUpdate(SCREEN_ID_D112010, HttpContext);
            model.UpdateKengenFlg = updateKengen;

            // 受入側の検索結果件数を取得
            var totalCount = GetSearchResultCount(model);
            model.SearchResult.TotalCount = totalCount;
            if (totalCount == 0)
            {
                model.MessageArea2 = MessageUtil.Get("MI00011");
                ModelState.AddModelError("MessageArea2", MessageUtil.Get("MI00011"));
                // 更新後のモデルをセッションに保存
                SessionUtil.Set(SESS_D112010, model, HttpContext);
                return model;
            }

            var displayCount = GetDisplayCount(model);
            var intPageId = GetPageId((pageId ?? 1), totalCount, displayCount);
            model.SearchResult.TableRecords = GetPageData(model, displayCount * (intPageId - 1), displayCount);
            model.SearchResult.Pager = new Pagination(intPageId, displayCount, totalCount);

            // 受入側だけ更新した結果で上書き
            return model;
        }
        #endregion

        #region ページ分データ取得（取込）
        /// <summary>
        /// 取込側の検索結果（ページ分データ）を取得する
        /// </summary>
        private D112010Model GetPageDataListTorikomi(int? pageId, D112010Model model)
        {
            ModelState.Clear();
            model.SearchResultTorikomi = new D112010SearchResultTorikomi();
            model.SearchConditionTorikomi.IsResultDisplay = true;
            // ユーザ情報をセッションから取得し、受入側のユーザ情報を再設定
            var updateKengen = ScreenSosaUtil.CanUpdate(SCREEN_ID_D112010, HttpContext);
            model.UpdateKengenFlg = updateKengen;

            var totalCount = GetSearchResultCountTorikomi(model);
            model.SearchResultTorikomi.TotalCount = totalCount;
            if (totalCount == 0)
            {
                model.MessageArea2 = MessageUtil.Get("MI00011");
                ModelState.AddModelError("MessageArea4", MessageUtil.Get("MI00011"));
                return model;
            }

            var displayCount = GetDisplayCount(model);
            var intPageId = GetPageId((pageId ?? 1), totalCount, displayCount);
            model.SearchResultTorikomi.TableRecords = GetPageDataTorikomi(model, displayCount * (intPageId - 1), displayCount);
            model.SearchResultTorikomi.Pager = new Pagination(intPageId, displayCount, totalCount);

            return model;
        }
        #endregion

        #region 検索結果表示数取得メソッド
        /// <summary>
        /// 検索結果表示数取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果表示数</returns>
        private int GetDisplayCount(D112010Model model)
        {
            return model.SearchCondition.DisplayCount ?? CoreConst.PAGE_SIZE;
        }
        #endregion

        #region 検索結果件数取得メソッド
        /// <summary>
        /// 検索結果件数取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果件数</returns>
        private int GetSearchResultCount(D112010Model model)
        {
            // セッションからの必須条件（例：組合等コード、ユーザID）
            var sessionKumiaitoCd = Syokuin.KumiaitoCd;   // セッション：組合等コード
            var sessionUserId = Syokuin.UserId;           // セッション：ユーザID

            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(
                    ConfigUtil.Get("SystemKbn"),
                    Syokuin.TodofukenCd,
                    Syokuin.KumiaitoCd,
                    Syokuin.ShishoCd);
            

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                var query =
                    from t1 in db.Set<T01060大量データ受入履歴>()
                    join t2 in db.Set<M00190大量データ対象データ>()
                        on t1.対象データ区分 equals t2.対象データ区分
                    where
                        // T2.対象業務区分＝「1：引受」
                        t2.業務区分 == "1" &&
                        // セッション条件：組合等コード
                        t1.組合等コード == sessionKumiaitoCd &&
                        // T1.ステータス <> 98（削除）
                        t1.ステータス != "98" &&
                        // 画面条件：対象データ（設定されていない場合は条件を無視）
                        (string.IsNullOrEmpty(model.SearchCondition.UkeireTaishoData)
                            ? true
                            : t1.対象データ区分 == model.SearchCondition.UkeireTaishoData) &&
                        // 画面条件：受入日（開始）(設定されていない場合は条件を無視)
                        (!model.SearchCondition.UkeireDateFrom.HasValue
                            ? true
                            : t1.データ登録日時 >= model.SearchCondition.UkeireDateFrom.Value) &&
                        // 画面条件：受入日（終了）(設定されていない場合は条件を無視)
                        (!model.SearchCondition.UkeireDateTo.HasValue
                            ? true
                            : t1.データ登録日時 <= model.SearchCondition.UkeireDateTo.Value) &&
                        // 画面条件：他のユーザの受入履歴表示フラグがONの場合は画面のユーザID、OFFの場合はセッションのユーザIDで絞込
                        (model.SearchCondition.IncludeOtherUserHistoryFlg
                            ? t1.登録ユーザid == model.SearchCondition.UkeireUserId
                            : t1.登録ユーザid == sessionUserId)
                    select t1;

                return query.Count();
            }
        }

        #endregion

        #region 検索結果件数取得メソッド（取込）
        /// <summary>
        /// 検索結果件数取得メソッド（取込）
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果件数</returns>
        private int GetSearchResultCountTorikomi(D112010Model model)
        {
            // セッションからの必須条件（例：組合等コード、ユーザID）
            var sessionKumiaitoCd = Syokuin.KumiaitoCd;   // セッション：組合等コード
            var sessionUserId = Syokuin.UserId;           // セッション：ユーザID

            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(
                    ConfigUtil.Get("SystemKbn"),
                    Syokuin.TodofukenCd,
                    Syokuin.KumiaitoCd,
                    Syokuin.ShishoCd);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                var query =
                    from t1 in db.Set<T01070大量データ取込履歴>()
                    join t2 in db.Set<M00190大量データ対象データ>()
                        on t1.対象データ区分 equals t2.対象データ区分
                    where
                        // t2.対象業務区分が「1：引受」
                        t2.業務区分 == "1" &&
                        // セッション条件：組合等コード
                        t1.組合等コード == sessionKumiaitoCd &&
                        // 削除以外（ステータス <> "98"）
                        t1.ステータス != "98" &&
                        // 画面条件：対象データ（値が設定されていない場合は条件を無視）
                        (string.IsNullOrEmpty(model.SearchConditionTorikomi.TorikomiTaishoData)
                            ? true
                            : t1.対象データ区分 == model.SearchConditionTorikomi.TorikomiTaishoData) &&
                        // 画面条件：取込日（開始）(設定されていない場合は条件を無視)
                        (!model.SearchConditionTorikomi.TorikomiDateFrom.HasValue
                            ? true
                            : t1.データ登録日時 >= model.SearchConditionTorikomi.TorikomiDateFrom.Value) &&
                        // 画面条件：取込日（終了）(設定されていない場合は条件を無視)
                        (!model.SearchConditionTorikomi.TorikomiDateTo.HasValue
                            ? true
                            : t1.データ登録日時 <= model.SearchConditionTorikomi.TorikomiDateTo.Value) &&
                        // 画面条件：他のユーザの取込履歴も表示するフラグがONの場合は画面のユーザID、
                        // OFFの場合はセッションのユーザIDで絞り込む
                        (model.SearchConditionTorikomi.IncludeOtherUserHistoryFlg
                            ? t1.登録ユーザid == model.SearchConditionTorikomi.TorikomiUserId
                            : t1.登録ユーザid == sessionUserId)
                    select t1;

                return query.Count();
            }
        }

        #endregion


        #region 検索結果のページ分取得メソッド
        /// <summary>
        /// 検索結果のページ分取得メソッド
        /// <summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <returns>検索結果のページ分</returns>
        private List<D112010TableRecord> GetPageData(D112010Model model, int offset, int pageSize)
        {
            // 検索結果件数分データを取得
            var sqlResults = GetResult(model, offset, pageSize);

            return sqlResults;
        }
        #endregion

        #region 検索結果のページ分取得メソッド（取込）
        /// <summary>
        /// 検索結果のページ分取得メソッド
        /// <summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <returns>検索結果のページ分</returns>
        private List<D112010TableRecordTorikomi> GetPageDataTorikomi(D112010Model model, int offset, int pageSize)
        {
            // 検索結果件数分データを取得
            var sqlResults = GetResultTorikomi(model, offset, pageSize);

            return sqlResults;
        }
        #endregion

        #region 検索情報取得メソッド
        /// <summary>
        /// メソッド：検索情報を取得する
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <returns>検索情報</returns>
        private List<D112010TableRecord> GetResult(D112010Model model, int offset = 0, int pageSize = 10)
        {
            // セッションからの必須条件
            var sessionKumiaitoCd = Syokuin.KumiaitoCd;   // セッション：組合等コード
            var sessionUserId = Syokuin.UserId;           // セッション：ユーザID

            DbConnectionInfo dbConnectionInfo = DBUtil.GetDbConnectionInfo(
                ConfigUtil.Get("SystemKbn"),
                Syokuin.TodofukenCd,
                Syokuin.KumiaitoCd,
                Syokuin.ShishoCd);
            var hanyoKbnResults = getJigyoCommonDb()
                            .MHanyokubuns
                            .Where(t => t.KbnSbt == "batch_status")
                            .ToList();

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                var query =
                    from history in db.Set<T01060大量データ受入履歴>()
                    join t2 in db.Set<M00190大量データ対象データ>()
                        on history.対象データ区分 equals t2.対象データ区分
                    where
                        // 業務区分＝「1：引受」
                        t2.業務区分 == "1" &&
                        // セッション条件：組合等コード
                        history.組合等コード == sessionKumiaitoCd &&
                        // 削除以外（ステータス <> "98"）
                        history.ステータス != "98" &&
                        // 画面条件：対象データ（値が設定されていない場合は条件を無視）
                        (string.IsNullOrEmpty(model.SearchCondition.UkeireTaishoData)
                            ? true
                            : history.対象データ区分 == model.SearchCondition.UkeireTaishoData) &&
                        // 画面条件：受入日（開始）
                        (!model.SearchCondition.UkeireDateFrom.HasValue
                            ? true
                            : history.データ登録日時 >= model.SearchCondition.UkeireDateFrom.Value) &&
                        // 画面条件：受入日（終了）
                        (!model.SearchCondition.UkeireDateTo.HasValue
                            ? true
                            : history.データ登録日時 <= model.SearchCondition.UkeireDateTo.Value) &&
                        // 画面条件：他のユーザの受入履歴も表示するフラグにより登録ユーザIDを切り替え
                        (model.SearchCondition.IncludeOtherUserHistoryFlg
                            ? history.登録ユーザid == model.SearchCondition.UkeireUserId
                            : history.登録ユーザid == sessionUserId)
                    select new
                    {
                        history.受入履歴id,
                        history.データ登録日時,
                        t2.受入対象データ略称,
                        t2.対象データ区分,
                        history.登録ユーザid,
                        history.ステータス,
                        history.対象件数,
                        history.OK件数,
                        history.エラー件数,
                        history.OKリスト名,
                        history.エラーリスト名,
                        history.コメント,
                        // 取込区分：'1'の場合は「済」、それ以外は「未」
                        取込区分 = history.取込区分 == "1" ? "済" : "未"
                    };

                // データ登録日時の降順でソート
                var orderedQuery = query.OrderByDescending(x => x.データ登録日時);

                // ページング処理
                var finalQuery = orderedQuery.Skip(offset).Take(pageSize);

                // 匿名型から表示用のレコード型（D112010TableRecord）へ変換
                var result = finalQuery.AsEnumerable().Select(x =>
                {
                    var kbn = hanyoKbnResults.FirstOrDefault(k => k.KbnCd == x.ステータス);
                    return new D112010TableRecord
                    {
                        UkeireRirekiId = x.受入履歴id,
                        DataRegistDate = x.データ登録日時,
                        UkeireTaishoDataNm = x.受入対象データ略称,
                        TaishoDataKbn = x.対象データ区分,
                        UserId = x.登録ユーザid,
                        KbnNm = kbn != null ? kbn.KbnNm : "",
                        ProcessStatus = x.ステータス,
                        TaishoCount = x.対象件数,
                        OkCount = x.OK件数,
                        ErrorCount = x.エラー件数,
                        OkDataList = x.OKリスト名,
                        ErrorList = x.エラーリスト名,
                        Comment = x.コメント,
                        Torikomi = x.取込区分
                    };
                }).ToList();

                return result;
            }
        }
        #endregion

        #region 検索情報取得メソッド（取込）
        /// <summary>
        /// メソッド：検索情報を取得する
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <returns>検索情報</returns>
        private List<D112010TableRecordTorikomi> GetResultTorikomi(D112010Model model, int offset = 0, int pageSize = 10)
        {
            // セッションからの必須条件（組合等コード、ユーザID）
            var sessionKumiaitoCd = Syokuin.KumiaitoCd;   // セッション：組合等コード
            var sessionUserId = Syokuin.UserId;            // セッション：ユーザID

            DbConnectionInfo dbConnectionInfo = DBUtil.GetDbConnectionInfo(
                ConfigUtil.Get("SystemKbn"),
                Syokuin.TodofukenCd,
                Syokuin.KumiaitoCd,
                Syokuin.ShishoCd);

            // 共通DBから「batch_status」区分のレコードをリストとして取得
            var hanyoKbnResults = getJigyoCommonDb()
                            .MHanyokubuns
                            .Where(t => t.KbnSbt == "batch_status")
                            .ToList();

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                var query =
                    from t1 in db.Set<T01070大量データ取込履歴>()  // 取込履歴テーブル
                    join t2 in db.Set<M00190大量データ対象データ>()
                        on t1.対象データ区分 equals t2.対象データ区分
                    where
                        // 業務区分＝「1：引受」
                        t2.業務区分 == "1" &&
                        // セッション条件：組合等コード
                        t1.組合等コード == sessionKumiaitoCd &&
                        // 削除以外（ステータス <> "98"）
                        t1.ステータス != "98" &&
                        // 画面条件：対象データ（値が設定されていない場合は条件を無視）
                        (string.IsNullOrEmpty(model.SearchConditionTorikomi.TorikomiTaishoData)
                            ? true
                            : t1.対象データ区分 == model.SearchConditionTorikomi.TorikomiTaishoData) &&
                        // 画面条件：取込日（開始）(設定されていない場合は条件を無視)
                        (!model.SearchConditionTorikomi.TorikomiDateFrom.HasValue
                            ? true
                            : t1.データ登録日時 >= model.SearchConditionTorikomi.TorikomiDateFrom.Value) &&
                        // 画面条件：取込日（終了）(設定されていない場合は条件を無視)
                        (!model.SearchConditionTorikomi.TorikomiDateTo.HasValue
                            ? true
                            : t1.データ登録日時 <= model.SearchConditionTorikomi.TorikomiDateTo.Value) &&
                        // 画面条件：他のユーザの取込履歴も表示するフラグにより登録ユーザIDを切り替え
                        (model.SearchConditionTorikomi.IncludeOtherUserHistoryFlg
                            ? t1.登録ユーザid == model.SearchConditionTorikomi.TorikomiUserId
                            : t1.登録ユーザid == sessionUserId)
                    select new
                    {
                        t1.取込履歴id,
                        t1.受入履歴id,
                        t1.データ登録日時,
                        t2.受入対象データ名称,
                        t1.登録ユーザid,
                        t1.ステータス,
                        t1.対象件数,
                        t1.エラーリスト名,
                        t1.コメント
                    };

                // データ登録日時の降順でソート
                var orderedQuery = query.OrderByDescending(x => x.データ登録日時);

                // ページング処理
                var finalQuery = orderedQuery.Skip(offset).Take(pageSize);

                // 匿名型から表示用のレコード型（D112010TableRecord）へ変換
                var result = finalQuery.AsEnumerable().Select(x =>
                {
                    var kbn = hanyoKbnResults.FirstOrDefault(k => k.KbnCd == x.ステータス);
                    return new D112010TableRecordTorikomi
                    {
                        TorikomiRirekiId = x.取込履歴id,
                        UkeireRirekiId = x.受入履歴id,
                        DataRegistDate = x.データ登録日時,
                        TorikomiTaishoDataNm = x.受入対象データ名称,
                        TourokuUserId = x.登録ユーザid,
                        KbnNm = kbn != null ? kbn.KbnNm : "",
                        TaishoCount = x.対象件数,
                        ErrorListNm = x.エラーリスト名,
                        Comment = x.コメント,
                    };
                }).ToList();

                return result;
            }
        }
        #endregion


        #region 戻るイベント
        /// <summary>
        /// イベント名：戻る 
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D112010, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion

        public ActionResult DeleteLineAccept(D112010Model model)
        {
            var sessionUserId = Syokuin.UserId;
            var resultMessage = string.Empty;

            if (model.SearchResult.TableRecords == null ||
                 !model.SearchResult.TableRecords.Any(record => record.SelectCheck))
            {
                return View(SCREEN_ID_D112010, model);
            }

            // 他のユーザの受入履歴も表示するチェックボックスがオフの場合
            if (!model.SearchCondition.IncludeOtherUserHistoryFlg)
            {
                // 受入履歴検索結果の明細の中で、行選択がオンで、かつユーザIDがセッションのユーザIDと異なるレコードがある場合
                if (model.SearchResult.TableRecords != null &&
                    model.SearchResult.TableRecords.Any(record => record.SelectCheck && record.UserId != sessionUserId))
                {
                    ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME10044", "ユーザIDを比較した行"));
                    return View(SCREEN_ID_D112010, model);
                }
            }
            string refMessage = string.Empty;
            //int updateResult = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), shoriSts, acceptanceErrorContent, BATCH_USER_NAME, ref refMessage);
            //if (0 == updateResult)
            //{
            // 更新に失敗した場合
            //logger.Error(refMessage);
            //logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts, refMessage));
            //result = Constants.BATCH_EXECUT_FAILED;
            //}
            //else
            //{
            // 更新に成功した場合
            //logger.Info(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts));
            //result = Constants.BATCH_EXECUT_SUCCESS;
            //}
            // DB 接続情報（各環境に合わせる）
            // 選択された受入履歴IDのリストを作成
            var selectedHistoryIds = model.SearchResult.TableRecords
                .Where(record => record.SelectCheck)
                .Select(record => record.UkeireRirekiId)
                .ToList();

            var dbConnectionInfo = DBUtil.GetDbConnectionInfo(
                ConfigUtil.Get("SystemKbn"), Syokuin.TodofukenCd, Syokuin.KumiaitoCd, Syokuin.ShishoCd);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    

                    // リストに含まれるIDを持つエンティティを取得
                    var entitiesForDelete = db.T01060大量データ受入履歴s
                        .Where(x => selectedHistoryIds.Contains(x.受入履歴id))
                        .ToList();

                    if (entitiesForDelete.Count == 0)
                    {
                        throw new SystemException(MessageUtil.Get("ME10083"));
                    }

                    foreach (var entity in entitiesForDelete)
                    {
                        db.T01060大量データ受入履歴s.Remove(entity);
                    }
                    model.SearchResult.TableRecords.RemoveAll(record => record.SelectCheck);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                    transaction.Rollback();
                    resultMessage = MessageUtil.Get("ME10083");
                    return Json(new { message = resultMessage });
                }

            }

            var existingModel = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);
            if (existingModel != null)
            {
                model.SearchResultTorikomi = existingModel.SearchResultTorikomi;
                model.SearchConditionTorikomi = existingModel.SearchConditionTorikomi;
            }
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);

            // 取込側の情報を設定
            model.VSyokuinRecords = new VSyokuin
            {
                ShishoCd = syokuin.ShishoCd,
                TodofukenCd = syokuin.TodofukenCd,
                KumiaitoCd = syokuin.KumiaitoCd,
                UserId = syokuin.UserId
            };
            model.ActiveTab = "UkeireTab";
            resultMessage = MessageUtil.Get("MI00004", "削除");
            // 変更したモデルをセッションに再設定（必要な場合） 削除ー＞削除
            SessionUtil.Set(SESS_D112010, model, HttpContext);

            // 変更後の部分ビューを返す
            return Json(new { messageSuccess = resultMessage, deletedIds = selectedHistoryIds });
        }

        public ActionResult DeleteLineImport(D112010Model model)
        {
            var sessionUserId = Syokuin.UserId;
            var resultMessage = string.Empty;

            if (model.SearchResultTorikomi.TableRecords == null ||
                 !model.SearchResultTorikomi.TableRecords.Any(record => record.SelectCheck))
            {
                return View(SCREEN_ID_D112010, model);
            }

            // 他のユーザの受入履歴も表示するチェックボックスがオフの場合
            if (!model.SearchConditionTorikomi.IncludeOtherUserHistoryFlg)
            {
                // 受入履歴検索結果の明細の中で、行選択がオンで、かつユーザIDがセッションのユーザIDと異なるレコードがある場合
                if (model.SearchResultTorikomi.TableRecords != null &&
                    model.SearchResultTorikomi.TableRecords.Any(record => record.SelectCheck && record.TourokuUserId != sessionUserId))
                {
                    
                    ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME10044", "ユーザIDを比較した行"));
                    return View(SCREEN_ID_D112010, model);
                }
            }
            string refMessage = string.Empty;
            /*
             * TODO : バッチ予約処理修正必要
             */
            //int updateResult = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), shoriSts, acceptanceErrorContent, BATCH_USER_NAME, ref refMessage);
            //if (0 == updateResult)
            //{
            // 更新に失敗した場合
            //logger.Error(refMessage);
            //logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts, refMessage));
            //result = Constants.BATCH_EXECUT_FAILED;
            //}
            //else
            //{
            // 更新に成功した場合
            //logger.Info(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts));
            //result = Constants.BATCH_EXECUT_SUCCESS;
            //}
            // DB 接続情報（各環境に合わせる）
            // 選択された受入履歴IDのリストを作成
            var selectedKeys = model.SearchResultTorikomi.TableRecords
                .Where(r => r.SelectCheck)
                .Select(r => r.UkeireRirekiId + "_" + r.TorikomiRirekiId)
                .ToList();

            var dbConnectionInfo = DBUtil.GetDbConnectionInfo(
                ConfigUtil.Get("SystemKbn"), Syokuin.TodofukenCd, Syokuin.KumiaitoCd, Syokuin.ShishoCd);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {

                    foreach (var re in selectedKeys)
                    {
                        logger.Debug("DeleteLineImport@@@@@@@@");
                        logger.Debug(re);
                        logger.Debug("DeleteLineImport@@@@@@@@");
                    }
                    // リストに含まれるIDを持つエンティティを取得
                    var entitiesForDelete = db.T01070大量データ取込履歴s
                            .Where(x => selectedKeys.Contains(x.受入履歴id + "_" + x.取込履歴id))
                            .ToList();

                    if (entitiesForDelete.Count == 0)
                    {
                        throw new SystemException(MessageUtil.Get("ME10083"));
                    }
                    foreach (var entity in entitiesForDelete)
                    {
                        db.T01070大量データ取込履歴s.Remove(entity);
                    }
                    model.SearchResultTorikomi.TableRecords.RemoveAll(record => record.SelectCheck);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                    transaction.Rollback();
                    resultMessage = MessageUtil.Get("ME10083");
                    return Json(new { message = resultMessage });
                }

            }

            var existingModel = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);
            if (existingModel != null)
            {
                model.SearchResult = existingModel.SearchResult;
                model.SearchCondition = existingModel.SearchCondition;
            }
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);

            // 取込側の情報を設定
            model.VSyokuinRecords = new VSyokuin
            {
                ShishoCd = syokuin.ShishoCd,
                TodofukenCd = syokuin.TodofukenCd,
                KumiaitoCd = syokuin.KumiaitoCd,
                UserId = syokuin.UserId
            };
            model.ActiveTab = "TorikomiTab";
            resultMessage = MessageUtil.Get("MI00004", "削除");
            // 変更したモデルをセッションに再設定（必要な場合） 削除ー＞削除
            SessionUtil.Set(SESS_D112010, model, HttpContext);

            // 変更後の部分ビューを返す
            return Json(new { messageSuccess = resultMessage });
        }

        #region ページャーイベント（受入）
        /// <summary>
        /// イベント名：ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult PagerUkeire(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || PAGE_0 == id)
            {
                return BadRequest();
            }

            D112010Model model = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 検索結果を取得する
            model = GetPageDataList(int.Parse(id), model);
            model.ActiveTab = "UkeireTab";
            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D112010, model, HttpContext);
            return PartialViewAsJson("_D112010SearchUkeireResult", model);
        }
        #endregion

        #region ページャーイベント（取込）
        /// <summary>
        /// イベント名：ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult PagerTorikomi(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || PAGE_0 == id)
            {
                return BadRequest();
            }

            D112010Model model = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 検索結果を取得する
            model = GetPageDataListTorikomi(int.Parse(id), model);
            model.ActiveTab = "TorikomiTab";
            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D112010, model, HttpContext);
            return PartialViewAsJson("_D112010SearchTorikomiResult", model);
        }
        #endregion

        #region 受入履歴のOKリストファイルダウンロード
        /// <summary>
        /// 受入履歴のOKリストファイルダウンロード
        /// </summary>
        /// <param name="recordId">受入履歴id</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult OkLinkAcceptFileDownload(string recordId)
        {
            D112010Model model = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);
            
            // DBから対象レコードの情報を取得
            var okInfo = getJigyoDb<NskAppContext>().T01060大量データ受入履歴s
                            .Where(t => t.受入履歴id == recordId)
                            .Select(t => new {
                                t.OKリストパス,
                                t.OKリスト名,
                                t.OKリストハッシュ値
                            })
                            .FirstOrDefault();

            // ZIPファイルのパスを取得
            string zipFilePath = okInfo.OKリストパス;
            if (!System.IO.File.Exists(zipFilePath))
            {
                ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME80002"));
                model.MessageArea3 = MessageUtil.Get("ME80002");
                return View(SCREEN_ID_D112010, model);
            }
            byte[] extractedFileData;
            string expectedFileName = string.Empty;

            try
            {
                // FileStreamとZipArchiveを使い、ZIPファイルをメモリ上で展開する
                using (var zipStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read))
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    // 拡張子が".csv"のエントリを取得
                    var entry = archive.Entries.FirstOrDefault(e =>
                        Path.GetExtension(e.FullName).ToLower() == ".csv");

                    if (entry == null)
                    {
                        ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME10050"));
                        return View(SCREEN_ID_D112010, model);
                    }

                    // エントリからファイル名を取得（復号化時に使用）
                    expectedFileName = entry.Name;
                    using (var entryStream = entry.Open())
                    using (var memoryStream = new MemoryStream())
                    {
                        entryStream.CopyTo(memoryStream);
                        extractedFileData = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME01645", "ファイルの解凍"));
                return View(SCREEN_ID_D112010, model);
            }

            // 復号化（暗号化時に使用した元ファイル名をキーとして使用）
            byte[] decryptedData = CryptoUtil.Decrypt(extractedFileData, expectedFileName);
            string hashByFile = CryptoUtil.GetSHA256Hex(decryptedData);
            if (!hashByFile.Equals(okInfo.OKリストハッシュ値))
            {
                ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME10052", "ファイル名"));
                return View(SCREEN_ID_D112010, model);
            }
            logger.Debug("OkLinkAcceptFileDownload4444444");
            // ハッシュ値チェックをクリアした場合、復号化したファイルをダウンロードさせる
            // ダウンロード時のContentTypeは用途に合わせて適宜変更してください
            return File(decryptedData, "application/octet-stream", expectedFileName);
        }
        #endregion

        #region 受入履歴のErrリストファイルダウンロード
        /// <summary>
        /// 受入履歴のOKリストファイルダウンロード
        /// </summary>
        /// <param name="recordId">受入履歴id</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult ErrLinkAcceptFileDownload(string recordId)
        {
            D112010Model model = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);

            // DBから対象レコードの情報を取得
            var errInfo = getJigyoDb<NskAppContext>().T01060大量データ受入履歴s
                            .Where(t => t.受入履歴id == recordId)
                            .Select(t => new {
                                t.エラーリストパス,
                                t.エラーリスト名,
                                t.エラーリストハッシュ値
                            })
                            .FirstOrDefault();

            // ZIPファイルのパスを取得
            string zipFilePath = errInfo.エラーリストパス;
            if (!System.IO.File.Exists(zipFilePath))
            {
                ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME80002"));
                model.MessageArea3 = MessageUtil.Get("ME80002");
                return View(SCREEN_ID_D112010, model);
            }
            byte[] extractedFileData;
            string expectedFileName = string.Empty;

            try
            {
                // FileStreamとZipArchiveを使い、ZIPファイルをメモリ上で展開する
                using (var zipStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read))
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    // 拡張子が".csv"のエントリを取得
                    var entry = archive.Entries.FirstOrDefault(e =>
                        Path.GetExtension(e.FullName).ToLower() == ".csv");

                    if (entry == null)
                    {
                        ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME10050"));
                        return View(SCREEN_ID_D112010, model);
                    }

                    // エントリからファイル名を取得（復号化時に使用）
                    expectedFileName = entry.Name;
                    using (var entryStream = entry.Open())
                    using (var memoryStream = new MemoryStream())
                    {
                        entryStream.CopyTo(memoryStream);
                        extractedFileData = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME01645", "ファイルの解凍"));
                return View(SCREEN_ID_D112010, model);
            }

            // 復号化（暗号化時に使用した元ファイル名をキーとして使用）
            byte[] decryptedData = CryptoUtil.Decrypt(extractedFileData, expectedFileName);
            string hashByFile = CryptoUtil.GetSHA256Hex(decryptedData);
            if (!hashByFile.Equals(errInfo.エラーリストハッシュ値))
            {
                ModelState.AddModelError("MessageArea3", MessageUtil.Get("ME10052", "ファイル名"));
                return View(SCREEN_ID_D112010, model);
            }
            // ハッシュ値チェックをクリアした場合、復号化したファイルをダウンロードさせる
            // ダウンロード時のContentTypeは用途に合わせて適宜変更してください
            return File(decryptedData, "application/octet-stream", expectedFileName);
        }
        #endregion

        #region 取込履歴のErrリストファイルダウンロード
        /// <summary>
        /// 取込履歴のOKリストファイルダウンロード
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult ErrLinkImportFileDownload(string ukeireId, string torikomiId)
        {
            D112010Model model = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);

            // DBから対象レコードの情報を取得
            var errInfo = getJigyoDb<NskAppContext>().T01070大量データ取込履歴s
                            .Where(t => t.受入履歴id == ukeireId &&
                                        t.取込履歴id == torikomiId)
                            .Select(t => new {
                                t.エラーリストパス,
                                t.エラーリスト名,
                                t.エラーリストハッシュ値
                            })
                            .FirstOrDefault();

            // ZIPファイルのパスを取得
            string zipFilePath = errInfo.エラーリストパス;
            if (!System.IO.File.Exists(zipFilePath))
            {
                ModelState.AddModelError("MessageArea5", MessageUtil.Get("ME80002"));
                model.MessageArea5 = MessageUtil.Get("ME80002");
                return View(SCREEN_ID_D112010, model);
            }
            byte[] extractedFileData;
            string expectedFileName = string.Empty;

            try
            {
                // FileStreamとZipArchiveを使い、ZIPファイルをメモリ上で展開する
                using (var zipStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read))
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    // 拡張子が".csv"のエントリを取得
                    var entry = archive.Entries.FirstOrDefault(e =>
                        Path.GetExtension(e.FullName).ToLower() == ".csv");

                    if (entry == null)
                    {
                        ModelState.AddModelError("MessageArea5", MessageUtil.Get("ME10050"));
                        return View(SCREEN_ID_D112010, model);
                    }

                    // エントリからファイル名を取得（復号化時に使用）
                    expectedFileName = entry.Name;
                    using (var entryStream = entry.Open())
                    using (var memoryStream = new MemoryStream())
                    {
                        entryStream.CopyTo(memoryStream);
                        extractedFileData = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("MessageArea5", MessageUtil.Get("ME01645", "ファイルの解凍"));
                return View(SCREEN_ID_D112010, model);
            }

            // 復号化（暗号化時に使用した元ファイル名をキーとして使用）
            byte[] decryptedData = CryptoUtil.Decrypt(extractedFileData, expectedFileName);
            string hashByFile = CryptoUtil.GetSHA256Hex(decryptedData);
            if (!hashByFile.Equals(errInfo.エラーリストハッシュ値))
            {
                ModelState.AddModelError("MessageArea5", MessageUtil.Get("ME10052", "ファイル名"));
                return View(SCREEN_ID_D112010, model);
            }
            logger.Debug("ErrLinkImportFileDownload4444444");
            // ハッシュ値チェックをクリアした場合、復号化したファイルをダウンロードさせる
            // ダウンロード時のContentTypeは用途に合わせて適宜変更してください
            return File(decryptedData, "application/octet-stream", expectedFileName);
        }
        #endregion

        #region クリアイベント（受入）
        /// <summary>
        /// イベント名：クリア
        /// </summary>
        /// <param name="model">D112010Model</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetCriteria([Bind("SearchCondition")] D112010Model model)
        {
            // モデル状態をクリア
            ModelState.Clear();

            // 検索結果を初期化
            model.SearchResult = new D112010SearchResult();
            // 結果表示フラグを false に設定（初期状態）
            model.SearchCondition.IsResultDisplay = false;
            // ユーザ情報はセッションから再設定
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);
            // 検索条件の初期化（必要な項目をリセット）
            model.SearchCondition.UkeireUserId = syokuin.UserId;
            model.SearchCondition.IncludeOtherUserHistoryFlg = false;
            model.SearchCondition.UkeireDateFrom = null;
            model.SearchCondition.UkeireDateTo = null;

            
            model.VSyokuinRecords.ShishoCd = syokuin.ShishoCd;
            model.VSyokuinRecords.TodofukenCd = syokuin.TodofukenCd;
            model.VSyokuinRecords.KumiaitoCd = syokuin.KumiaitoCd;
            model.VSyokuinRecords.UserId = syokuin.UserId;
            // 更新済みのモデルをセッションに保存
            SessionUtil.Set(SESS_D112010, model, HttpContext);
            
            return PartialViewAsJson("_D112010SearchUkeireResult", model);
        }
        #endregion

        #region クリアイベント（取込）
        /// <summary>
        /// イベント名：クリア
        /// </summary>
        /// <param name="model">D112010Model</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetCriteriTorikomi([Bind("SearchConditionTorikomi")] D112010Model model)
        {
            // モデル状態をクリア
            ModelState.Clear();

            // 検索結果を初期化
            model.SearchResultTorikomi = new D112010SearchResultTorikomi();
            // 結果表示フラグを false に設定（初期状態）
            model.SearchConditionTorikomi.IsResultDisplay = false;
            // ユーザ情報はセッションから再設定
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);
            // 検索条件の初期化（必要な項目をリセット）
            model.SearchConditionTorikomi.TorikomiUserId = syokuin.UserId;
            model.SearchConditionTorikomi.IncludeOtherUserHistoryFlg = false;
            model.SearchConditionTorikomi.TorikomiDateFrom = null;
            model.SearchConditionTorikomi.TorikomiDateTo = null;


            model.VSyokuinRecords.ShishoCd = syokuin.ShishoCd;
            model.VSyokuinRecords.TodofukenCd = syokuin.TodofukenCd;
            model.VSyokuinRecords.KumiaitoCd = syokuin.KumiaitoCd;
            model.VSyokuinRecords.UserId = syokuin.UserId;
            // 更新済みのモデルをセッションに保存
            SessionUtil.Set(SESS_D112010, model, HttpContext);

            return PartialViewAsJson("_D112010SearchTorikomiResult", model);
        }
        #endregion

        #region 検索条件チェックメソッド
        /// <summary>
        /// 検索条件チェックメソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        private bool CheckSearchCondition(D112010Model model)
        {
            var checkFlg = false;
            // [画面：受入日（終了）]<[画面：受入日（開始）]の場合、エラーと判定する
            if (model.SearchCondition.UkeireDateFrom.HasValue && model.SearchCondition.UkeireDateTo.HasValue &&
                model.SearchCondition.UkeireDateFrom.Value > model.SearchCondition.UkeireDateTo.Value)
            {

                ModelState.AddModelError("MessageArea2", MessageUtil.Get("ME10022", "受入日（開始）", "受入日（終了）"));
                ModelState.AddModelError("SearchCondition.UkeireDateFrom", " ");
                ModelState.AddModelError("SearchCondition.UkeireDateTo", " ");
                checkFlg = true;
                return checkFlg;
            }
            return checkFlg;
        }
        #endregion

        #region バッチ登録イベント（取込）
        /// <summary>
        /// イベント名：バッチ登録
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult CreatBatchReportTorikomi(string ukeireId, string taishoDataKbn, string taishoDataNm)
        {
            logger.Debug("START CreatBatchReportTorikomi");

            var model = SessionUtil.Get<D112010Model>(SESS_D112010, HttpContext);
            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 条件IDを取得する
            // Guid.NewGuid().ToString();
            String strJoukenId = Guid.NewGuid().ToString("D");
            string strJoukenErrorMsg = string.Empty;

            long newId = getJigyoDb<NskAppContext>().T01070大量データ取込履歴s
                    .AsEnumerable() // ここでデータをクライアント側に取得
                    .Select(x => string.IsNullOrEmpty(x.取込履歴id) ? 0 : long.Parse(x.取込履歴id))
                    .DefaultIfEmpty(0)
                    .Max() + 1;

            // 条件を登録する
            //InsertTJouken(model, strJoukenId, form.TorikomiFilePath, modelVar.FileHash);
            strJoukenErrorMsg = InsertTJoukenTorikomi(
                strJoukenId
                , newId.ToString()
                , ukeireId
                , taishoDataKbn);
            if (!string.IsNullOrEmpty(strJoukenErrorMsg))
            {
                return Json(new { message = strJoukenErrorMsg });
            }

            var 複数実行禁止ID = string.Empty;
            var 予約バッチ名 = string.Empty;
            if ("基準収穫量".Equals(taishoDataNm))
            {
                複数実行禁止ID = "NSK_112041B";
                予約バッチ名 = "基準収穫量大量データ取込";
            }
            else if ("組合等類別設定".Equals(taishoDataNm))
            {
                複数実行禁止ID = "NSK_112081B";
                予約バッチ名 = "組合員等類別設定大量データ取込";
            }
            else if ("組合等類別平均単収".Equals(taishoDataNm))
            {
                複数実行禁止ID = "NSK_112121B";
                予約バッチ名 = "組合等類別平均単収大量データ取込";
            }
            else if ("加入申込書（細目・カンマ）".Equals(taishoDataNm)
                  || "加入申込書（細目・GISカンマ）".Equals(taishoDataNm)
                  || "加入申込書（細目・固定長）".Equals(taishoDataNm))
            {
                複数実行禁止ID = "NSK_112190B";
                予約バッチ名 = "加入申込書大量受入データ取込";
            }

            // バッチ予約登録
            var refMsg = string.Empty;
            long batchId = 0;
            // バッチ条件（表示用）作成
            var displayJouken = taishoDataNm;
            //ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
            // バッチ予約登録
            int? result = null;
            try
            {
                logger.Info("バッチ予約登録処理を実行する。");
                result = BatchUtil.InsertBatchYoyaku(AppConst.BatchBunrui.BATCH_BUNRUI_90_OTHER,
                "02",
                Syokuin.TodofukenCd,
                Syokuin.KumiaitoCd,
                Syokuin.ShishoCd,
                DateUtil.GetSysDateTime(),
                Syokuin.UserId,
                D112010_REPORT_YOYAKU_SHORI_NM,
                予約バッチ名,
                strJoukenId,
                displayJouken,
                AppConst.FLG_ON,
                AppConst.BatchType.BATCH_TYPE_PATROL,
                null,
                AppConst.FLG_OFF,
                ref refMsg,
                ref batchId,
                複数実行禁止ID + Syokuin.TodofukenCd
                );
            }
            catch (Exception e)
            {
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                //logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                logger.Error(MessageUtil.GetErrorMessage(e, 10));
                return Json(new { message = MessageUtil.Get("ME90005") });
            }

            // 処理結果がエラーだった場合
            if (result == 0)
            {
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , Syokuin.TodofukenCd
                , Syokuin.KumiaitoCd
                , Syokuin.ShishoCd);
                using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
                {
                    // トランザクションを開始
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        // t_01070_大量データ取込履歴 テーブルの削除
                        var recordsToDelete1 = db.T01070大量データ取込履歴s
                            .Where(r => r.受入履歴id == ukeireId &&
                                        r.取込履歴id == newId.ToString());

                        foreach (var record in recordsToDelete1)
                        {
                            db.T01070大量データ取込履歴s.Remove(record);
                        }

                        // t_01050_バッチ条件 テーブルの削除
                        var recordsToDelete2 = db.T01050バッチ条件s
                            .Where(r => r.バッチ条件id == strJoukenId);

                        foreach (var record in recordsToDelete2)
                        {
                            db.T01050バッチ条件s.Remove(record);
                        }

                        // すべてのエンティティの削除を一括保存
                        db.SaveChanges();

                        // トランザクションコミット
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }

                return Json(new { message = MessageUtil.Get("ME90005") });
            }

            // （出力メッセージ：バッチ予約登録成功）
            logger.Info("バッチ予約登録成功");
            return Json(new { message = MessageUtil.Get("MI90002", "引受大量データ受入処理", "受入対象") });
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
        private string InsertTJoukenTorikomi(
            string joukenId
            , string paramTorikomiId
            , string paramUkeireRirekiId
            , string paramTaishoDataKbn
            )
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

            // DbContext を一度だけ使用する
            using (var db1 = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                // トランザクションを開始
                var transaction = db1.Database.BeginTransaction();

                try
                {
                    var TorikomiRireki = new T01070大量データ取込履歴
                    {
                        取込履歴id = paramTorikomiId,
                        受入履歴id = paramUkeireRirekiId,
                        データ登録日時 = systemDate,
                        組合等コード = Syokuin.KumiaitoCd,
                        ステータス = "01",
                        対象データ区分 = paramTaishoDataKbn,
                        //コメント = model.Comment,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01070大量データ取込履歴s.Add(TorikomiRireki);
                    // fileNensanはそのままINSERT（分割不要）
                    var fileNensan = new T01050バッチ条件
                    {
                        バッチ条件id = joukenId,
                        連番 = ++serialNumber,
                        条件名称 = "年産",
                        表示用条件値 = md.SNensanHikiuke,
                        条件値 = md.SNensanHikiuke,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01050バッチ条件s.Add(fileNensan);

                    var torikomiId = new T01050バッチ条件
                    {
                        バッチ条件id = joukenId,
                        連番 = ++serialNumber,
                        条件名称 = "取込履歴ID",
                        表示用条件値 = "取込履歴ID",
                        条件値 = paramTorikomiId,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01050バッチ条件s.Add(torikomiId);

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
        #endregion

        /// <summary>
        /// ログインユーザ（セッションに格納されたSyokuin）が参照可能な支所一覧を取得する。
        /// </summary>
        /// <param name="context">HTTPコンテキスト</param>
        /// <returns>参照可能な支所一覧</returns>
        public static List<Shisho> GetShishoList(HttpContext context)
        {
            return SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, context);
        }

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
    }
}