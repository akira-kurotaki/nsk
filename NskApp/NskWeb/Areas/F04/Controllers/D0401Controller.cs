using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F04.Consts;
using NskWeb.Areas.F04.Models.D0401;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Pager;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using ModelLibrary.Models;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NskWeb.Areas.F04.Controllers
{
    /// <summary>
    /// ファイル取込
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [Area("F04")]
    public class D0401Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D0401)
        /// </summary>
        private static readonly string SCREEN_ID_D0401 = "D0401";

        /// <summary>
        /// セッションキー(D0401)
        /// </summary>
        private static readonly string SESS_D0401 = SCREEN_ID_D0401 + "_" + "SCREEN";

        /// <summary>
        /// セッションキー(PAGE_TOKEN）
        /// </summary>
        public static readonly string SESS_D0401_PAGE_TOKEN = SCREEN_ID_D0401 + "_" + "PAGE_TOKEN";

        /// <summary>
        /// 部分ビュー名
        /// </summary>
        private static readonly string PARTIAL_VIEW_NAME = "_D0401SearchResult";

        /// <summary>
        /// ページ0
        /// </summary>
        private static readonly string PAGE_0 = "0";

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D0401Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期化
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {
            // セッションに検索条件がある場合
            if (SessionUtil.Get<D0401Model>(SESS_D0401, HttpContext) is D0401Model model)
            {
                if (model.SearchResult.Pager == null)
                {
                    // ページャーが空の場合
                    return View(SCREEN_ID_D0401, GetFileTorikomiList(1, model));
                }

                return View(SCREEN_ID_D0401, GetFileTorikomiList(model.SearchResult.Pager.CurrentPage, model));
            }

            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
            // 画面項目の初期化
            model = new D0401Model(Syokuin, shishoList);

            // 都道府県
            model.TodofukenDropDownList.TodofukenCd = Syokuin.TodofukenCd;
            // 組合等
            model.TodofukenDropDownList.KumiaitoCd = Syokuin.KumiaitoCd;

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 取込対象リスト
            model.MTorikomiList = getJigyoDb<NskAppContext>().MTorikomis.ToList();

            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D0401, model, HttpContext);

            // パンくずリストを変更する
            //SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>() { "D000000" }, HttpContext);

            // ファイル取込画面を表示する
            return View(SCREEN_ID_D0401, Init(model));
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
            SessionUtil.Remove(SESS_D0401, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion

        #region 初期設定メソッド
        /// <summary>
        /// メソッド：初期設定
        /// </summary>
        ///<param name="model">ビューモデル</param>
        /// <returns>モデル</returns>
        private D0401Model Init(D0401Model model)
        {
            // 「ファイル取込一覧」を取得する
            model = GetFileTorikomiList(1, model);
            return model;
        }
        #endregion

        #region 再表示イベント
        /// <summary>
        /// イベント名：再表示
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost, ActionName("Reload")]
        [Button(ButtonName = "Reload")]
        public ActionResult Reload(D0401Model model)
        {
            // 検索して、画面に返す
            return View(SCREEN_ID_D0401, Init(model));
        }
        #endregion

        #region 削除イベント
        /// <summary>
        /// イベント名：削除 
        /// </summary>
        /// <param name="batchId">バッチID</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string batchId)
        {
            // 異常の場合
            if (!Regex.IsMatch(batchId, @"^[0-9]+$") || !Regex.IsMatch(batchId, @"^[0-9]+$"))
            {
                return BadRequest();
            }

            // セッションから検索条件を取得する
            var model = SessionUtil.Get<D0401Model>(SESS_D0401, HttpContext);

            // セッションに自画面のデータが存在しないの場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // メッセージをクリアする
            model.MessageArea2 = string.Empty;
            model.ErrorMessageArea2 = string.Empty;
            ModelState.Remove("MessageArea2");
            ModelState.Remove("ErrorMessageArea2");

            D0401TableRecord record = GetTorikomiManage(batchId);

            D0401TableRecord dispRecord = null;
            foreach (var val in model.SearchResult.TableRecords)
            {
                if (val.BatchId == batchId)
                {
                    dispRecord = val;
                    break;
                }
            }

            if (dispRecord == null || record == null || dispRecord.TorikomiSts != record.TorikomiSts)
            {
                var message = MessageUtil.Get("ME91012");
                model.ErrorMessageArea2 = message;
            }
            else
            {

            }


            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D0401, model, HttpContext);

            // 画面に返す
            return View(SCREEN_ID_D0401, model);
        }
        #endregion

        #region ファイル取込一覧取得メソッド
        /// <summary>
        /// メソッド：「ファイル取込一覧」を取得する
        /// </summary>
        /// <param name="pageId">ページID</param>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果モデル</returns>
        private D0401Model GetFileTorikomiList(int pageId, D0401Model model)
        {
            // 検索結果件数を取得する
            int totalCount = GetSearchResultCount();

            // 検索件数を画面表示用モデルに設定する
            model.SearchResult.TotalCount = totalCount;

            // 検索結果が0件の場合
            if (totalCount == 0)
            {
                // 画面メッセージエリア2にメッセージ設定
                ModelState.AddModelError("MessageArea2", MessageUtil.Get("MI00012"));
                // 0件の場合、検索結果をクリアする
                model.SearchResult.TableRecords = new List<D0401TableRecord>();
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D0401, model, HttpContext);
                return model;
            }
            else
            {
                // メッセージをクリアする
                model.ErrorMessageArea2 = string.Empty;
                model.MessageArea2 = string.Empty;
                ModelState.Remove("MessageArea2");
                ModelState.Remove("ErrorMessageArea2");
            }
            // 検索結果表示数の取得
            int displayCount = CoreConst.PAGE_SIZE;

            // ページIDの取得
            int intPageId = GetPageId(pageId, totalCount, displayCount);

            // 検索結果ページ分の取得
            model.SearchResult.TableRecords = GetPageData(model, displayCount * (intPageId - 1), displayCount);
            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();
            // ページャーの初期化
            model.SearchResult.Pager = new Pagination(intPageId, displayCount, totalCount);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D0401, model, HttpContext);

            return model;
        }
        #endregion

        #region ファイル取込一覧の件数取得メソッド
        /// <summary>
        /// メソッド：「ファイル取込一覧の件数取得」を取得する
        /// </summary>
        /// <returns>”未削除”のデータ数</returns>
        private int GetSearchResultCount()
        {
            Syokuin syokuin = Syokuin;

            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>
            {
                new("@TodofukenCd", syokuin.TodofukenCd),
                new("@KumiaitoCd", syokuin.KumiaitoCd)
            };

            // 件数取得
            sql.Append(
                "SELECT COUNT(1) AS \"Value\" " +
                "FROM t_torikomi_manage " +
                "WHERE " +
                "  ( " +
                "    delete_flg is null " +
                "    OR delete_flg <> '1' " +
                "  ) " +
                "  AND todofuken_cd = @TodofukenCd " +
                "  AND kumiaito_cd = @KumiaitoCd ");

            // 支所
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                sql.Append("  AND shisho_cd = ANY (@ShishoList) ");
                parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).ToList()
                });
            }
            else if (!string.IsNullOrEmpty(syokuin.ShishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append("  AND shisho_cd = @ShishoCd ");
                parameters.Add(new NpgsqlParameter("@ShishoCd", syokuin.ShishoCd));
            }

            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<int>(sql.ToString(), parameters.ToArray()).Single();
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
        private List<D0401TableRecord> GetPageData(D0401Model model, int offset, int pageSize)
        {
            // 検索結果件数分データを取得
            var sqlResults = GetResult(model, offset, pageSize);

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
        private List<D0401TableRecord> GetResult(D0401Model model, int offset = 0, int pageSize = 10)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetTableCondition(sql);
            // 検索条件を取得する
            GetSearchCondition(sql, parameters);

            // ソート順を追加する
            sql.Append(
                "ORDER BY " +
                "  t_torikomi_manage.insert_date DESC, " +
                " . batch_id DESC ");

            // ページ分追加
            sql.Append(" LIMIT @PageSize ");
            sql.Append(" OFFSET @Offset ");
            parameters.Add(new NpgsqlParameter("@PageSize", pageSize));
            parameters.Add(new NpgsqlParameter("@Offset", offset));

            logger.Info("ファイル取込一覧取得処理を実行します。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D0401TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region 取込管理（データ反映対象）の取得
        /// <summary>
        /// 取込管理（データ反映対象）の取得
        /// </summary>
        /// <param name="batch_id">バッチID</param>
        /// <returns>取込管理</returns>
        private D0401TableRecord GetTorikomiManage(string batch_id)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>
            {
                new("@BatchId", batch_id)
            };

            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetTableCondition(sql);

            sql.Append(
                "WHERE " +
                "  t_torikomi_manage.batch_id = @BatchId");

            logger.Info("取込管理（データ反映対象）を取得します。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D0401TableRecord>(sql.ToString(), parameters.ToArray()).SingleOrDefault();
        }
        #endregion

        #region 検索項目取得
        /// <summary>
        /// 検索項目を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetCondition(StringBuilder sql)
        {
            sql.Append(
                "SELECT " +
                "  t_torikomi_manage.batch_id AS BatchId, " +
                "  t_torikomi_manage.torikomi_cd AS TorikomiCd, " +
                "  m_torikomi.torikomi_nm AS TorikomiNm, " +
                "  t_torikomi_manage.todofuken_cd AS Todofuken_Cd, " +
                "  v_todofuken.todofuken_nm AS TodofukenNm, " +
                "  t_torikomi_manage.kumiaito_cd AS KumiaitoCd, " +
                "  v_kumiaito.kumiaito_nm AS KumiaitoNm, " +
                "  t_torikomi_manage.shisho_cd AS ShishoCd, " +
                "  v_shisho_nm.shisho_nm AS ShishoNm, " +
                "  t_torikomi_manage.torikomi_file_path AS TorikomiFilePath, " +
                "  t_torikomi_manage.torikomi_file_nm AS TorikomiFileNm, " +
                "  t_torikomi_manage.hash AS Hash, " +
                "  t_torikomi_manage.torikomi_sts AS TorikomiSts, " +
                "  t_torikomi_manage.torikomi_date AS TorikomiDate, " +
                "  t_torikomi_manage.lock_date AS LockDate, " +
                "  t_torikomi_manage.hanei_date AS HaneiDate ");
        }
        #endregion

        #region 検索テーブル取得
        /// <summary>
        /// 検索テーブルを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private void GetTableCondition(StringBuilder sql)
        {
            sql.Append(
                "FROM " +
                "  t_torikomi_manage " +
                "  LEFT JOIN m_torikomi " +
                "    ON t_torikomi_manage.torikomi_cd = m_torikomi.torikomi_cd " +
                "  LEFT JOIN v_todofuken " +
                "    ON t_torikomi_manage.todofuken_cd = v_todofuken.todofuken_cd " +
                "  LEFT JOIN v_kumiaito " +
                "    ON t_torikomi_manage.todofuken_cd = v_kumiaito.todofuken_cd " +
                "    AND t_torikomi_manage.kumiaito_cd = v_kumiaito.kumiaito_cd " +
                "  LEFT JOIN v_shisho_nm " +
                "    ON v_shisho_nm.todofuken_cd = v_shisho_nm.todofuken_cd " +
                "    AND v_shisho_nm.kumiaito_cd = v_shisho_nm.kumiaito_cd " +
                "    AND v_shisho_nm.shisho_cd = v_shisho_nm.shisho_cd ");
        }
        #endregion

        #region 検索条件取得
        /// <summary>
        /// 検索条件を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="model">モデル</param>
        /// <param name="parameters">パラメータ</param>
        private void GetSearchCondition(StringBuilder sql, List<NpgsqlParameter> parameters)
        {
            Syokuin syokuin = Syokuin;

            sql.Append(
                "WHERE " +
                "  (delete_flg IS NULL OR delete_flg <> '1') " +
                "  AND t_torikomi_manage.todofuken_cd = @TodofukenCd " +
                "  AND t_torikomi_manage.kumiaito_cd = @KumiaitoCd ");

            // 支所
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                sql.Append("  AND t_torikomi_manage.shisho_cd = ANY (@ShishoList) ");
                parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).ToList()
                });
            }
            else if (!string.IsNullOrEmpty(syokuin.ShishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append("  AND t_torikomi_manage.shisho_cd = @ShishoCd ");
                parameters.Add(new NpgsqlParameter("@ShishoCd", syokuin.ShishoCd));
            }

            parameters.Add(new NpgsqlParameter("@TodofukenCd", syokuin.TodofukenCd));
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));

        }
        #endregion

        #region アップロードイベント(ブラウザのERR_UPLOAD_FILE_CHANGED異常」キャッチするため)
        /// <summary>
        /// アップロード「OK」を押下時
        /// </summary>
        /// <param name="form">画面の入力値</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpLoadTest()
        {
            // セッションから画面モデルを取得する
            var model = SessionUtil.Get<D0401Model>(SESS_D0401, HttpContext);

            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00001", "セッションから画面モデルを取得できない"));
            }
            return View(SCREEN_ID_D0401, model);
        }
        #endregion

        #region ファイル取込「OK」を押下時

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import([Bind("TorikomiCd, TodofukenDropDownList, FilePath, PageToken")] D0401Model form)
        {
            // セッションから画面モデルを取得する
            var model = SessionUtil.Get<D0401Model>(SESS_D0401, HttpContext);
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面モデルを取得できない"));
            }

            // 属性チェックエラーの場合
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91009"));
                // 画面入力値を保持する
                SetFormToModel(form, model);

                // 画面情報をセッションに保存する
                SessionUtil.Set(SESS_D0401, model, HttpContext);

                return View(SCREEN_ID_D0401, model);
            }

            // 独自チェックエラーの場合
            if (IsPrivateCheckError(form))
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91009"));
                // 画面入力値を保持する
                SetFormToModel(form, model);

                // 画面情報をセッションに保存する
                //SessionUtil.Set(SESS_D0008, model);
                SessionUtil.Set(SESS_D0401, model, HttpContext);

                return View(SCREEN_ID_D0401, model);
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
            var modelVar = new D0401ModelVar
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

            // 一時フォルダー(定数ID：D0401UploadTempFolder)\yyyyMMdd\hhmmss\[セッション：ユーザ情報.ユーザID]
            var yyyyMMddHHmmssStr = systemDate.ToString("yyyyMMddHHmmss");
            var tempFolder = Path.Combine(ConfigUtil.Get(InfraConst.D0401_UPLOAD_TEMP_FOLDER), yyyyMMddHHmmssStr, userId);

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

            try
            {
                // ウィルスチェックのため、ウィルスチェック待ち時間分だけ待つ
                var uploadWaitTime = int.Parse(ConfigUtil.Get(CoreConst.UPLOAD_WAIT_TIME)) * 1000;
                Thread.Sleep(uploadWaitTime);

                // ファイルがなければ、ファイルが駆除されたのでエラーとする
                if (!System.IO.File.Exists(tempFilePath))
                {
                    if (Directory.Exists(tempFolder))
                    {
                        Directory.Delete(tempFolder, true);
                    }
                    ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90017", fileNameWithExtension));

                    // 画面入力値を保持する
                    SetFormToModel(form, model);

                    // 画面情報をセッションに保存する
                    SessionUtil.Set(SESS_D0401, model, HttpContext);

                    return View(SCREEN_ID_D0401, model);
                }

                // ファイル暗号化
                byte[] fileData = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    uploadFile.CopyTo(ms);
                    fileData = ms.ToArray();
                }
                var encryptData = CryptoUtil.Encrypt(fileData, fileNameWithExtension);

                // 暗号化したファイルをサーバに書き込み
                modelVar.UploadFolder = ConfigUtil.Get(InfraConst.D0401_UPLOAD_FOLDER) + @"\" + yyyyMMddHHmmssStr + @"\" + userId + @"\" + modelVar.FileName;
                modelVar.FileSavePath = Path.Combine(modelVar.UploadFolder, uniqueFileName);

                // アップロードファイルをアップロードフォルダに保存する 
                if (!Directory.Exists(modelVar.UploadFolder))
                {
                    Directory.CreateDirectory(modelVar.UploadFolder);
                }
                using (var fileStream = new FileStream(modelVar.FileSavePath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(encryptData, 0, encryptData.Length);
                }
                modelVar.FileHash = CryptoUtil.GetSHA256Hex(fileData);

                // 一時フォルダを削除する
                Directory.Delete(tempFolder, true);
            }
            catch (Exception e)
            {
                logger.Error(e);
                if (Directory.Exists(tempFolder))
                {
                    // 一時フォルダを削除する
                    Directory.Delete(tempFolder, true);
                }
                throw;
            }

            var torikomiNm = getJigyoDb<NskAppContext>().MTorikomis.Where(m => m.TorikomiCd == form.TorikomiCd).Select(m => m.TorikomiNm).Single();
            Syokuin syokuin = Syokuin;

            // バッチ条件（表示用）
            var batchJokenDispSb = new StringBuilder();
            batchJokenDispSb.Append("取込対象：" + torikomiNm + Environment.NewLine);
            batchJokenDispSb.Append("対象年度：" + NendoUtil.GetNendoDisp1(form.Nendo) + Environment.NewLine);
            batchJokenDispSb.Append("都道府県：" + syokuin.TodofukenCd + CoreConst.SEPARATOR + TodofukenUtil.GetTodofukenNm(syokuin.TodofukenCd) + Environment.NewLine);
            batchJokenDispSb.Append("組合等：" + syokuin.KumiaitoCd + CoreConst.SEPARATOR + KumiaitoUtil.GetKumiaitoNm(syokuin.TodofukenCd, syokuin.KumiaitoCd) + Environment.NewLine);
            batchJokenDispSb.Append("支所：" + form.TodofukenDropDownList.ShishoCd + ShishoUtil.GetShishoNm(syokuin.TodofukenCd, syokuin.KumiaitoCd, form.TodofukenDropDownList.ShishoCd) +   Environment.NewLine);
            batchJokenDispSb.Append("アップロードファイル：" + form.FilePath);

            // ファイル取込のバッチ予約を登録する。
            string message = string.Empty;
            long batchId = 0L;
            var insertResult = BatchUtil.InsertBatchYoyaku(AppConst.BatchBunrui.BATCH_BUNRUI_04_FILE_IMPORT,
                                                            ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                                                            Syokuin.TodofukenCd,
                                                            Syokuin.KumiaitoCd,
                                                            form.TodofukenDropDownList.ShishoCd,
                                                            systemDate,
                                                            Syokuin.UserId,
                                                            "D0402_Import",
                                                            "ファイル取込",
                                                            null,
                                                            batchJokenDispSb.ToString(),
                                                            "0",
                                                            AppConst.BatchType.BATCH_TYPE_PATROL,
                                                            null,
                                                            "1",
                                                            ref message,
                                                            ref batchId);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 処理結果がエラーだった場合
            if (insertResult == 0)
            {
                logger.Info("バッチ予約登録失敗");
                var errMsg = MessageUtil.Get("ME90008", message);
                logger.Error(errMsg);
                model.MessageArea1 = errMsg;
                ModelState.AddModelError("MessageArea1", errMsg);
            }
            else
            {
                // 取込管理登録処理
                InsertTorikomiManage(form, batchId, importFileName, modelVar.FileHash, systemDate);

                model = Init(model);

                // アップロード処理完了メッセージ
                model.MessageArea1 = MessageUtil.Get("MI90002", "ファイル取込", "ファイル取込一覧にて、ステータスが一時テーブルへの取込済みになったら、データ反映を行ってください。（再表示ボタンを押下すると一覧が更新されます。）");
                ModelState.AddModelError("MessageArea1", model.MessageArea1);
            }

            // 画面情報をセッションに保存する
            SessionUtil.Set(SESS_D0401, model, HttpContext);

            // ファイル取込画面を表示する
            return View(SCREEN_ID_D0401, model);
        }
        #endregion

        #region 画面設定値保持
        /// <summary>
        /// 画面設定値保持
        /// </summary>
        /// <param name="form">画面フォーム</param>
        /// <param name="model">ビューモデル</param>
        private void SetFormToModel(D0401Model form, D0401Model model)
        {
            // アップロードするファイル
            model.FilePath = form.FilePath;
            model.FileSize = form.FileSize;
            model.FileSizeStr = ChangeFileSizeToStr(model.FileSize);
        }
        #endregion

        #region 独自チェック
        /// <summary>
        /// イベント：取込
        /// </summary>
        /// <param name="form">画面の入力値</param>
        /// <returns>エラーが存在するかどうか</returns>
        private bool IsPrivateCheckError(D0401Model form)
        {
            var checkErrorFlg = false;

            // 拡張子チェックエラーファイル名
            var errorfileName = "";
            // 各ファイルサイズの合計値
            var fileTotalSize = 0;
            // ファイル対象リスト
            var files = Request.Form.Files;
            // D0401アップロード可能な拡張子
            var allowedExtension = ConfigUtil.Get(InfraConst.D0401_UPLOAD_FILE_EXTENSION);
            var extensions = allowedExtension.Split(',');

            // 禁止文字チェック
            var isInvalidCharsExist = false;

            // ファイル名文字数チェック
            var isFileNameTooLong = false;

            // [アップロードするファイル]にファイルが存在しない場合
            if (Request.Form.Files.Count == 0)
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91005", "アップロードするファイル"));
                ModelState.AddModelError("FilePath", " ");

                checkErrorFlg = true;
                return checkErrorFlg;
            }

            var uploadFile = Request.Form.Files[0];
            var fileName = uploadFile.FileName;
            var fileSize = uploadFile.Length;
            var fileExtension = Path.GetExtension(fileName).Replace(".", "");
            var fileNameWithoutPath = Path.GetFileName(fileName);

            // ［画面：アップロードファイル］にzip以外の拡張子が含まれている場合（大文字、小文字を区別しない）
            if (!extensions.Contains(fileExtension.ToLower()))
            {
                errorfileName = fileNameWithoutPath;
            }
            fileTotalSize = fileTotalSize + (int)fileSize;

            // ファイル名に使用できない文字を取得
            var invalidChars = Path.GetInvalidFileNameChars();
            if (-1 < fileNameWithoutPath.IndexOfAny(invalidChars))
            {
                isInvalidCharsExist = true;
            }

            // ファイル名が100文字を超える場合
            if (fileName.Length > 100)
            {
                isFileNameTooLong = true;
            }

            // [アップロードするファイル]に「(定数ID：D0401UploadFileExtension)の値」以外の拡張子が含まれている場合
            // エラーファイル名は空ではない場合
            if ("" != errorfileName)
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91006", errorfileName));
                ModelState.AddModelError("FilePath", " ");
                checkErrorFlg = true;
            }

            // [アップロードするファイル]の合計サイズが「(定数ID：D0401UploadFileMaxSize)の値」を超えている場合
            var d0401UploadFileMaxSize = int.Parse(ConfigUtil.Get(InfraConst.D0401_UPLOAD_FILE_MAX_SIZE));
            if (fileTotalSize > d0401UploadFileMaxSize)
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91007", ConfigUtil.Get(InfraConst.D0401_UPLOAD_FILE_MAX_DISP_SIZE)));
                ModelState.AddModelError("FilePath", " ");
                checkErrorFlg = true;
            }

            // [アップロードするファイル]のファイル名が100文字を超える場合
            if (isFileNameTooLong)
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91008", "100"));
                ModelState.AddModelError("FilePath", " ");
                checkErrorFlg = true;
            }

            // 禁止文字チェック
            if (isInvalidCharsExist)
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91010", new string[] { "ファイル名", "ファイル名に「¥　/　:　*　?　\"　<　>　| 」は使用できません。" }));
                ModelState.AddModelError("FilePath", " ");
                checkErrorFlg = true;
            }

            // 外字チェック
            // [アップロードするファイル]
            if (form.FilePath != null && !"".Equals(form.FilePath))
            {
                // MS932以外の文字や外字を取得。
                var exceptedString = StringUtil.CheckMS932ExceptGaiji(ChangeMacDakuten(form.FilePath));
                if (exceptedString != null && exceptedString.Length != 0)
                {
                    ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91011", exceptedString));
                    ModelState.AddModelError("FilePath", " ");
                    checkErrorFlg = true;
                }
            }

            // 入力値権限チェック
            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            // セッション：支所グループに値が設定されている、かつ、[画面：支所コード]がセッション：支所グループに含まれていない場合
            if (!shishoList.IsNullOrEmpty())
            {
                if (shishoList.Select(i => i.ShishoCd).ToList().Contains(form.TodofukenDropDownList.ShishoCd))
                {
                    ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91004", "取込対象範囲"));
                    ModelState.AddModelError("TodofukenDropDownList_ShishoCd", " ");
                    checkErrorFlg = true;
                }
            }

            return checkErrorFlg;
        }
        #endregion

        #region ファイルサイズの単位を追加
        /// <summary>
        /// ファイルサイズの単位を追加
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        private string ChangeFileSizeToStr(int? fileSize)
        {
            var fileSizeStr = "";
            if (fileSize == null)
            {
                return fileSizeStr;
            }
            double tempFileSize = fileSize.Value;
            // ファイルサイズが1024*1024以上の場合：MB
            if (tempFileSize >= F04Const.FILE_SIZE_1024 * F04Const.FILE_SIZE_1024)
            {
                // ファイルサイズ/1024/1024 MByte　（小数第２位を切り上げ）
                var fileSizeMb = (Math.Ceiling((tempFileSize / (F04Const.FILE_SIZE_1024 * F04Const.FILE_SIZE_1024)) * 10)) / 10;
                fileSizeStr = fileSizeMb + F04Const.FILE_SIZE_TANI_MB;
            }
            // ファイルサイズが1024以上の場合：KB
            else if (tempFileSize >= F04Const.FILE_SIZE_1024)
            {
                // ファイルサイズ/1024 KB　（小数第２位を四捨五入する）
                var fileSizeKb = (Math.Ceiling((tempFileSize / F04Const.FILE_SIZE_1024) * 10)) / 10;
                fileSizeStr = fileSizeKb + F04Const.FILE_SIZE_TANI_KB;
            }
            // それ以外：B
            else
            {
                fileSizeStr = tempFileSize + F04Const.FILE_SIZE_TANI_B;
            }
            return fileSizeStr;
        }
        #endregion

        #region 「、」でエラーファイル名を連結する
        /// <summary>
        /// 「、」でエラーファイル名を連結する
        /// </summary>
        /// <param name="errorfileName">エラーファイル名連結結果</param>
        /// <param name="errorfileNameSub">一部エラーファイル名</param>
        /// <returns>エラーファイル名連結結果</returns>
        private string ContactErrorFileName(string errorfileName, string errorfileNameSub)
        {
            if ("" == errorfileName)
            {
                errorfileName = errorfileNameSub;
            }
            else
            {
                errorfileName += ("、" + errorfileNameSub);
            }

            return errorfileName;
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

        #region  [変数：ファイル情報リスト]を繰り返し、 [ファイル情報.一時フォルダ]に対応するフォルダーと[ファイル情報.ファイルの保存先] に対応するフォルダーを削除する
        /// <summary>
        ///  [変数：ファイル情報リスト]を繰り返し、 [ファイル情報.一時フォルダ]に対応するフォルダーと[ファイル情報.ファイルの保存先] に対応するフォルダーを削除する
        /// </summary>
        /// <param name="modelVarList">ファイル情報モデルリスト</param>
        /// <param name="deleteUploadFolderFlag">「ファイルの保存先」を削除かどうか</param>
        private void DeleteFile(List<D0401ModelVar> modelVarList, bool deleteUploadFolderFlag)
        {
            foreach (var deleteModelVar in modelVarList)
            {
                if (deleteModelVar.TempFolder != null && Directory.Exists(deleteModelVar.TempFolder))
                {
                    // [ファイル情報.一時フォルダ]に対応するフォルダーを削除する
                    Directory.Delete(deleteModelVar.TempFolder, true);
                }
                if (deleteUploadFolderFlag)
                {
                    if (deleteModelVar.UploadFolder != null && Directory.Exists(deleteModelVar.UploadFolder))
                    {
                        // [ファイル情報.ファイルの保存先] に対応するフォルダーを削除する
                        Directory.Delete(deleteModelVar.UploadFolder, true);
                    }
                }
            }
        }
        #endregion

        #region 取込管理登録処理
        /// <summary>
        /// 取込管理登録処理
        /// </summary>
        /// <param name="form">ビューモデル</param>
        /// <param name="batchId">バッチID</param>
        /// <param name="torikomiFileNm">ファイル名</param>
        /// <param name="hash">ハッシュ値</param>
        /// <param name="systemDate">システム日時</param>
        private void InsertTorikomiManage(D0401Model form, long batchId, string torikomiFileNm, string hash,DateTime systemDate)
        {
            Syokuin syokuin = Syokuin;
            var db = getJigyoDb<NskAppContext>();
            using (var tx = db.Database.BeginTransaction()) {
                TTorikomiManage torikomiManage = new()
                {
                    // バッチID
                    BatchId = batchId,
                    // 取込対象コード
                    TorikomiCd = form.TorikomiCd,
                    // 対象年度
                    Nendo = short.Parse(form.Nendo),
                    // 都道府県コード
                    TodofukenCd = form.TodofukenDropDownList.TodofukenCd,
                    // 組合等コード
                    KumiaitoCd = form.TodofukenDropDownList.KumiaitoCd,
                    // 支所コード
                    ShishoCd = form.TodofukenDropDownList.ShishoCd,
                    // 取込ファイルパス
                    TorikomiFilePath = form.FilePath,
                    // 取込ファイル名
                    TorikomiFileNm = torikomiFileNm,
                    // ハッシュ値
                    Hash = hash,
                    // 取込ステータス
                    TorikomiSts = AppConst.TorikomiSts.WAITING_IMPORT,
                    // 取込完了日時
                    TorikomiDate = null,
                    // ロック開始日時
                    LockDate = null,
                    // 反映完了日時
                    HaneiDate = null,
                    // 削除フラグ
                    DeleteFlg = "0",
                    // 登録ユーザID
                    InsertUserId = syokuin.UserId,
                    // 登録日時
                    InsertDate = systemDate,
                    // 更新ユーザID
                    UpdateUserId = syokuin.UserId,
                    // 更新日時
                    UpdateDate = systemDate,
                };

                db.TTorikomiManages.Add(torikomiManage);

                try
                {
                    logger.Info(string.Format("バッチID：{0}を登録します。", batchId));
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
            #endregion
        }
    }
}
