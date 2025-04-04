using System.Text;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSK_P109010.Models;
using NSK_P109010.ReportCreators.NSK_109010P;
using NskAppModelLibrary.Context;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using Core = CoreLibrary.Core.Consts;
using NskCommon = NskCommonLibrary.Core.Consts;

namespace NSK_P109010.Controllers
{
    /// <summary>
    /// 加入申込書兼変更届出書
    /// </summary>
    public class NSK_109010PController : ReportController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbInfo">DB接続情報</param>
        public NSK_109010PController(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        /// <summary>
        /// 加入申込書兼変更届出書
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="args">
        /// 引数配列要素1：ユーザーID
        /// 引数配列要素2：バッチID
        /// 引数配列要素3：都道府県コード
        /// 引数配列要素4：組合等コード
        /// 引数配列要素5：支所コード
        /// 引数配列要素6：バッチ条件のキー情報
        /// </param>
        public ControllerResult ManageReports(NskAppContext dbContext, string[] args)
        {
            ControllerResult controllerResult = new();

            // 引数
            // ユーザーID
            string userId = string.Empty;
            // バッチ条件のキー情報
            string jid = string.Empty;
            // 都道府県コード
            string todofukenCd = string.Empty;
            // 組合等コード
            string kumiaitoCd = string.Empty;
            // 支所コード
            string shishoCd = string.Empty;
            // バッチID
            string bid = string.Empty;

            try
            {
                // args から 各変数へ展開する
                // ユーザーID
                if (args.Length > 0)
                {
                    userId = args[0];
                }
                // バッチ条件のキー情報
                if (args.Length > 1)
                {
                    jid = args[1];
                }
                // 都道府県コード
                if (args.Length > 2)
                {
                    todofukenCd = args[2];
                }
                // 組合等コード
                if (args.Length > 3)
                {
                    kumiaitoCd = args[3];
                }
                // 支所コード
                if (args.Length > 4)
                {
                    shishoCd = args[4];
                }
                // バッチID
                if (args.Length > 5)
                {
                    bid = args[5];
                }

                // １．引数のチェック
                IsRequired(userId, jid, todofukenCd, kumiaitoCd, shishoCd, bid, out int nBid);

                // ２．バッチ条件を取得する。
                // バッチ条件
                BatchJouken batchJouken = new();
                // ２．１．バッチ条件情報の取得
                batchJouken.GetTyouhyouSakuseiJouken(dbContext, jid);

                // 必須入力チェック
                batchJouken.IsRequired();

                // ３．帳票の出力対象データを取得する。
                // ３．１．「２．１．」で取得したデータを条件として、帳票の出力対象データを取得する。
                List<TyouhyouDataMain> tyouhyouDatasMain = GetTyouhyouDataMain(dbContext, batchJouken, todofukenCd);
                List<TyouhyouDataSub1> tyouhyouDatasSub1 = GetTyouhyouDataSub1(dbContext, batchJouken, shishoCd);
                List<TyouhyouDataSub2> tyouhyouDatasSub2 = GetTyouhyouDataSub2(dbContext, batchJouken, shishoCd);

                // ３．２．「３．１．」の取得結果が0件の場合は、
                if (tyouhyouDatasMain.Count == 0 || tyouhyouDatasSub1.Count == 0 || tyouhyouDatasSub2.Count == 0)
                {
                    // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                    // （"ME10076" 引数{0}：0)
                    throw new AppException("ME10076", MessageUtil.Get("ME10076", "0"));
                }

                // ４．帳票PDF一時出力フォルダ作成
                // ４．１．帳票PDF一時出力フォルダを作成する。
                // [FileTempFolderPath]/[変数：バッチID]_yyyyMMddHHmmss/
                string fileTempFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH),
                    bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                Directory.CreateDirectory(fileTempFolderPath);

                // ４．２．帳票出力フォルダを作成する。
                // [ReportOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss/
                string reportOutputFolder = Path.Combine(ConfigUtil.Get(Core.CoreConst.REPORT_OUTPUT_FOLDER),
                    bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                Directory.CreateDirectory(reportOutputFolder);

                // ５．一覧帳票作成
                // ５．１．帳票「引受通知書（収量建て総計）（様式第８－２号）」を作成する。
                // ５．１．１．引受通知書収量建て総計作成処理を呼び出す。
                // 引数.出力条件＝「２．１．」で取得したデータ
                // 引数.出力対象データ＝「３．１．」で取得したデータ
                NSK_109010PModel tyouhyouDatas = new()
                {
                    TyouhyouDatasMain = tyouhyouDatasMain,
                    TyouhyouDatasSub1 = tyouhyouDatasSub1,
                    TyouhyouDatasSub2 = tyouhyouDatasSub2
                };
                CreatorResult creatorResult = NSK_109010PCreator.CreateReport(batchJouken, tyouhyouDatas);

                // 作成された帳票をPDFファイルとして、「４．１．」で作成したフォルダに出力する。
                // 帳票名は[変数：条件_帳票名].PDFとする。
                string tempFileName = batchJouken.JoukenReportName + Core.CoreConst.SYMBOL_DOT + "PDF";
                string tempPath = Path.Combine(fileTempFolderPath, tempFileName);
                pdfExport.Export(creatorResult.SectionReport.Document, tempPath);

                // ６．終了処理
                // ６．１．「５．」の帳票作成処理が成功した場合
                if (creatorResult.Result == ReportConst.RESULT_SUCCESS)
                {
                    // （１）「４．１．」のフォルダ内の帳票をZipUtil.CreateZipを使用して分割（所定のサイズ以上の場合）、およびZip化（暗号化）し、
                    Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(fileTempFolderPath);
                    // ZipファイルをFolderUtil.MoveFileを使用して「２．１．」で取得したファイルパス/ファイル名に移動する。
                    // ※FolderUtil.MoveFile内で、「２．１．」で取得したファイルパス/ファイル名がシステム共通スキーマのバッチダウンロードファイルテーブルに登録される。
                    NskCommonLibrary.Core.Utility.FolderUtil.MoveFile(zipFilePath, reportOutputFolder, userId, nBid);

                    // （２）「４．１．」のフォルダを削除する。
                    if (Directory.Exists(fileTempFolderPath))
                    {
                        Directory.Delete(fileTempFolderPath, true);
                    }

                    // （３）処理結果：0（成功）を返す。
                    controllerResult.Result = ReportConst.RESULT_SUCCESS;
                }
            }
            catch (Exception ex)
            {
                // ６．２．上記までの処理で例外が発生した場合
                // 例外の内容をログに出力する。
                logger.Error(ex);

                // 処理結果：1（失敗）を返す
                controllerResult.Result = ReportConst.RESULT_FAILED;

                // 設定されたエラーメッセージを返す。
                if (ex is AppException aEx)
                {
                    controllerResult.ErrorMessageId = aEx.ErrorCode;
                }
                controllerResult.ErrorMessage = ex.Message;
            }

            return controllerResult;
        }

        /// <summary>
        /// 引数のチェック
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="jid">バッチ条件のキー情報</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="bid">バッチID</param>
        /// <param name="nBid">数値型バッチID</param>
        /// <exception cref="AppException"></exception>
        private static void IsRequired(string userId, string jid, string todofukenCd, string kumiaitoCd, string shishoCd, string bid, out int nBid)
        {
            // 必須項目が未入力の場合、エラーとし、エラーメッセージを返す。
            // １．１．ユーザIDが未入力の場合、
            if (string.IsNullOrEmpty(userId))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054" 引数{0} ：ユーザID）
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "ユーザID"));
            }

            // １．２．条件IDが未入力の場合、
            if (string.IsNullOrEmpty(jid))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                //（"ME01054" 引数{0} ：条件ID)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件ID"));
            }

            // １．３．都道府県コードが未入力の場合、
            if (string.IsNullOrEmpty(todofukenCd))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054"　引数{0} ：都道府県コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "都道府県コード"));
            }

            // １．４．組合等コードが未入力の場合、
            if (string.IsNullOrEmpty(kumiaitoCd))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                //（"ME01054" 引数{0} ：組合等コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
            }

            // １．５．支所コードが未入力の場合、
            if (string.IsNullOrEmpty(shishoCd))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                //（"ME01054" 引数{0} ：支所コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "支所コード"));
            }

            // １．６，バッチIDが未入力の場合、
            if (string.IsNullOrEmpty(bid))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054" 引数{0} ：パラメータの取得）
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチID"));
            }

            // バッチIDが数値変換不可の場合
            // 数値化したバッチID
            if (!int.TryParse(bid, out nBid))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME90012"　引数{0} ：バッチID)
                throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
            }
        }

        /// <summary>
        /// 帳票の出力対象データの取得（メインレポート用）
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jouken">バッチ条件</param>
        /// <returns></returns>
        private static List<TyouhyouDataMain> GetTyouhyouDataMain(NskAppContext dbContext, BatchJouken jouken, string todofukenCd)
        {
            StringBuilder sql = new();

            sql.Append($"	SELECT	 ");
            sql.Append($"	    t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.統計単位地域コード	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.大地区コード	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.小地区コード	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.継続特約フラグ	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.未加入フラグ	 ");
            sql.Append($"	    , m_00010_共済目的名称.共済目的名称	 ");
            sql.Append($"	    , 組合員等マスタ.daihyosha_kana As 申込者カナ	 ");
            sql.Append($"	    , 組合員等マスタ.daihyosha_nm As 申込者氏名	 ");
            sql.Append($"	    , 組合員等マスタ.postal_cd As 郵便番号 ");
            //sql.Append($"	    , 組合員等マスタ.住所１漢字 || 組合員等マスタ.住所２漢字 As 住所	 ");　// 住所修正?
            sql.Append($"	    , 組合員等マスタ.address As 住所	 ");
            sql.Append($"	    , 組合員等マスタ.tel As 電話番号 ");
            sql.Append($"	    , t_12010_引受結果.耕地番号	 ");
            sql.Append($"	    , t_12010_引受結果.分筆番号	 ");
            sql.Append($"	    , LPAD(t_11090_引受耕地.統計市町村コード, 5, '0') || 名称M市町村.shichoson_nm As 市町村名	 ");
            sql.Append($"	    , t_12010_引受結果.地名地番	 ");
            sql.Append($"	    , t_12010_引受結果.耕地面積	 ");
            sql.Append($"	    , t_12010_引受結果.引受面積	 ");
            sql.Append($"	    , t_12010_引受結果.転作等面積	 ");
            sql.Append($"	    , CASE 	 ");
            sql.Append($"	        WHEN t_12010_引受結果.共済目的コード <> '20' 	 ");
            sql.Append($"	            THEN t_12010_引受結果.類区分 	 ");
            //sql.Append($"	        ELSE NULL 	 "); // Nullは不都合なので空文字に修正
            sql.Append($"	        ELSE '' 	 ");
            sql.Append($"	        END As 類区分2	 "); // 類区分かぶりなので別名
            sql.Append($"	    , CASE 	 ");
            sql.Append($"	        WHEN t_12010_引受結果.共済目的コード <> '20' 	 ");
            sql.Append($"	            THEN m_00020_類名称.類短縮名称 	 ");
            //sql.Append($"	        ELSE NULL 	 "); // Nullは不都合なので空文字に修正
            sql.Append($"	        ELSE '' 	 ");
            sql.Append($"	        END As 類区分名称	 ");
            sql.Append($"	    , t_12010_引受結果.品種コード	 ");
            sql.Append($"	    , m_00110_品種係数.品種名等 As 品種名称	 ");
            sql.Append($"	    , m_00040_田畑名称.田畑名称 As 田畑区分	 ");
            sql.Append($"	    , t_12010_引受結果.受委託者コード As 備考	 ");
            sql.Append($"	    , t_12010_引受結果.収量等級コード As 収量等級	 ");
            sql.Append($"	    , m_10040_参酌係数.参酌係数 As 参酌	 ");
            sql.Append($"	    , 組合等マスタ.kumiaito_nm As 組合名称	 ");
            sql.Append($"	    , 組合等マスタ.kumiaitocho_nm As 組合代表者名 	 ");
            sql.Append($"	    , t_11010_個人設定.加入形態 	 ");
            sql.Append($"	    , 組合員等マスタ.koshu_bcp_kbn 	 ");
            sql.Append($"	FROM	 ");
            sql.Append($"	    t_12040_組合員等別引受情報 	 ");
            sql.Append($"	    INNER JOIN t_12010_引受結果 	 ");
            sql.Append($"	        ON t_12040_組合員等別引受情報.組合等コード = t_12010_引受結果.組合等コード 	 ");
            sql.Append($"	        AND t_12040_組合員等別引受情報.年産 = t_12010_引受結果.年産 	 ");
            sql.Append($"	        AND t_12040_組合員等別引受情報.共済目的コード = t_12010_引受結果.共済目的コード 	 ");
            sql.Append($"	        AND t_12040_組合員等別引受情報.組合員等コード = t_12010_引受結果.組合員等コード 	 ");
            sql.Append($"	    INNER JOIN t_11090_引受耕地 	 ");
            sql.Append($"	        ON t_12010_引受結果.組合等コード = t_11090_引受耕地.組合等コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.年産 = t_11090_引受耕地.年産 	 ");
            sql.Append($"	        AND t_12010_引受結果.共済目的コード = t_11090_引受耕地.共済目的コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.組合員等コード = t_11090_引受耕地.組合員等コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.耕地番号 = t_11090_引受耕地.耕地番号 	 ");
            sql.Append($"	        AND t_12010_引受結果.分筆番号 = t_11090_引受耕地.分筆番号 	 ");
            sql.Append($"	    INNER JOIN m_00010_共済目的名称 	 ");
            sql.Append($"	        ON t_12040_組合員等別引受情報.共済目的コード = m_00010_共済目的名称.共済目的コード 	 ");
            sql.Append($"	    INNER JOIN t_11010_個人設定 	 ");
            sql.Append($"	        ON t_12010_引受結果.組合等コード = t_11010_個人設定.組合等コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.年産 = t_11010_個人設定.年産 	 ");
            sql.Append($"	        AND t_12010_引受結果.共済目的コード = t_11010_個人設定.共済目的コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.組合員等コード = t_11010_個人設定.組合員等コード 	 ");
            sql.Append($"	    LEFT OUTER JOIN m_00020_類名称 	 ");
            sql.Append($"	        ON t_12010_引受結果.共済目的コード = m_00020_類名称.共済目的コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.類区分 = m_00020_類名称.類区分 	 ");
            sql.Append($"	    LEFT OUTER JOIN m_00040_田畑名称 	 ");
            sql.Append($"	        ON t_12010_引受結果.田畑区分 = m_00040_田畑名称.田畑区分 	 ");
            sql.Append($"	    LEFT OUTER JOIN m_00110_品種係数 	 ");
            sql.Append($"	        ON t_12010_引受結果.組合等コード = m_00110_品種係数.組合等コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.年産 = m_00110_品種係数.年産 	 ");
            sql.Append($"	        AND t_12010_引受結果.共済目的コード = m_00110_品種係数.共済目的コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.品種コード = m_00110_品種係数.品種コード 	 ");
            sql.Append($"	    LEFT OUTER JOIN m_10040_参酌係数 	 ");
            sql.Append($"	        ON t_12010_引受結果.組合等コード = m_10040_参酌係数.組合等コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.年産 = m_10040_参酌係数.年産 	 ");
            sql.Append($"	        AND t_12010_引受結果.共済目的コード = m_10040_参酌係数.共済目的コード 	 ");
            sql.Append($"	        AND t_12010_引受結果.参酌コード = m_10040_参酌係数.参酌コード 	 ");
            sql.Append($"	    INNER JOIN v_kumiaito 組合等マスタ 	 ");
            sql.Append($"	        ON t_12040_組合員等別引受情報.組合等コード = 組合等マスタ.kumiaito_cd 	 ");
            sql.Append($"	        AND 組合等マスタ.todofuken_cd = @都道府県コード	 ");　// 一様都道府県
            sql.Append($"	    LEFT OUTER JOIN v_nogyosha 組合員等マスタ 	 ");
            sql.Append($"	        ON t_12040_組合員等別引受情報.組合員等コード = 組合員等マスタ.kumiaiinto_cd 	 ");
            sql.Append($"	        AND 組合員等マスタ.todofuken_cd = @都道府県コード	 ");　// 一様都道府県
            sql.Append($"	    LEFT OUTER JOIN v_shichoson_nm 名称M市町村 	 ");
            sql.Append($"	        ON t_11090_引受耕地.統計市町村コード = 名称M市町村.shichoson_cd 	 ");
            sql.Append($"	        AND 名称M市町村.todofuken_cd = @都道府県コード	 ");　// 一様都道府県
            sql.Append($"	WHERE	 ");
            sql.Append($"	    t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	    AND t_12040_組合員等別引受情報.年産 = @条件_年産 	 ");
            sql.Append($"	    AND t_12040_組合員等別引受情報.共済目的コード = @条件_共済目的コード 	 ");
            sql.Append($"	    AND t_12040_組合員等別引受情報.類区分 = '0' 	 ");
            sql.Append($"	    AND t_12040_組合員等別引受情報.統計単位地域コード = '0' 	 ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("都道府県コード", todofukenCd),
                new("条件_組合等コード", jouken.JoukenKumiaitoCd),
                new("条件_年産", int.Parse(jouken.JoukenNensan)),
                new("条件_共済目的コード", jouken.JoukenKyosaiMokutekiCd)
            ];

            // ※パラメータ情報の値が空白以外の場合
            if (!string.IsNullOrEmpty(jouken.JoukenDaichiku))
            {
                sql.Append($"	    AND t_12040_組合員等別引受情報.大地区コード = @条件_大地区コード 	 ");
                parameters.Add(new("条件_大地区コード", jouken.JoukenDaichiku));
            }
            if (!string.IsNullOrEmpty(jouken.JoukenShochikuStart))
            {
                sql.Append($"	    AND t_12040_組合員等別引受情報.小地区コード >= @条件_小地区コード開始 	 ");
                parameters.Add(new("条件_小地区コード開始", jouken.JoukenShochikuStart));
            }
            if (!string.IsNullOrEmpty(jouken.JoukenShochikuEnd))
            {
                sql.Append($"	    AND t_12040_組合員等別引受情報.小地区コード <= @条件_小地区コード終了 	 ");
                parameters.Add(new("条件_小地区コード終了", jouken.JoukenShochikuEnd));
            }
            if (!string.IsNullOrEmpty(jouken.JoukenKumiaiintoCdStart))
            {
                sql.Append($"	    AND t_12040_組合員等別引受情報.組合員等コード >= @条件_組合員等コード開始 	 ");
                parameters.Add(new("条件_組合員等コード開始", jouken.JoukenKumiaiintoCdStart));
            }
            if (!string.IsNullOrEmpty(jouken.JoukenKumiaiintoCdEnd))
            {
                sql.Append($"	    AND t_12040_組合員等別引受情報.組合員等コード <= @条件_組合員等コード終了 	 ");
                parameters.Add(new("条件_組合員等コード終了", jouken.JoukenKumiaiintoCdEnd));
            }

            sql.Append($"	ORDER BY	 ");
            sql.Append($"	    t_12040_組合員等別引受情報.未加入フラグ DESC	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.大地区コード ASC	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.小地区コード ASC	 ");
            sql.Append($"	    , t_12040_組合員等別引受情報.組合員等コード ASC	 ");
            sql.Append($"	    , t_12010_引受結果.耕地番号 ASC	 ");
            sql.Append($"	    , t_12010_引受結果.分筆番号 ASC	 ");

            // SQLのクエリ結果をListに格納する
            List<TyouhyouDataMain> tyouhyouDatas = dbContext.Database.SqlQueryRaw<TyouhyouDataMain>(sql.ToString(), parameters.ToArray()).ToList();

            return tyouhyouDatas;
        }

        /// <summary>
        /// 帳票の出力対象データの取得（SUB1レポート用）
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jouken">バッチ条件</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns></returns>
        private static List<TyouhyouDataSub1> GetTyouhyouDataSub1(NskAppContext dbContext, BatchJouken jouken, string shishoCd)
        {
            StringBuilder sql = new();

            sql.Append($"	WITH main_table AS ( 	 "); 
            sql.Append($"	    SELECT	 ");
            sql.Append($"	        items.*	 ");
            sql.Append($"	        , ROW_NUMBER() OVER ( 	 ");
            sql.Append($"	            PARTITION BY	 ");
            sql.Append($"	                items.組合等コード	 ");
            sql.Append($"	                , items.共済目的コード	 ");
            sql.Append($"	                , items.年産	 ");
            sql.Append($"	                , items.引受回	 ");
            sql.Append($"	                , items.組合員等コード 	 ");
            sql.Append($"	            ORDER BY	 ");
            sql.Append($"	                items.組合等コード	 ");
            sql.Append($"	                , items.共済目的コード	 ");
            sql.Append($"	                , items.年産 	 ");
            sql.Append($"	                , items.引受回 	 ");
            sql.Append($"	                , items.組合員等コード	 ");
            sql.Append($"	                , items.類区分	 ");
            sql.Append($"	        ) recnum 	 ");
            sql.Append($"	    FROM	 ");
            sql.Append($"	        ( 	 ");
            sql.Append($"	            SELECT	 ");
            sql.Append($"	                MIN(t_12040_組合員等別引受情報.組合等コード) As 組合等コード	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.共済目的コード) As 共済目的コード	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.年産) As 年産	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.引受回) As 引受回	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.類区分) As 類区分	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.組合員等コード) As 組合員等コード	 ");
            sql.Append($"	                , MIN(m_00020_類名称.類短縮名称) As 類短縮名称	 ");
            sql.Append($"	                , MIN(m_10080_引受方式名称.引受方式名称) As 引受方式	 ");
            sql.Append($"	                , CAST(MIN(t_12040_組合員等別引受情報.補償割合コード) As Integer) / 10 As 補償割合	 ");
            sql.Append($"	                , CASE	 ");
            sql.Append($"	                    WHEN MIN(t_12040_組合員等別引受情報.特約区分) = '4'	 ");
            sql.Append($"	                        THEN '有り'	 ");
            sql.Append($"	                        ELSE '無し'	 ");
            sql.Append($"	                    END As 一筆半損特約の有無	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.特約区分) As 特約区分	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.共済金額選択順位) As 選択順位	 ");
            sql.Append($"	                , '' As 備考 	 ");　// 空文字か
            sql.Append($"	            FROM	 ");
            sql.Append($"	                t_12040_組合員等別引受情報 	 ");

            sql.Append($"	                INNER JOIN 	 "); // t_00010_引受回結合
            sql.Append($"	                    ( 	 ");
            sql.Append($"	                        SELECT 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                            , MAX(t_00010_引受回.引受回) AS 最大引受回 	 ");
            sql.Append($"	                        FROM 	 ");
            sql.Append($"	                            t_00010_引受回 	 ");
            sql.Append($"	                        GROUP BY 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                    )  T00010 	 ");
            sql.Append($"	                ON t_12040_組合員等別引受情報.組合等コード = T00010.組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = T00010.共済目的コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = T00010.年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = T00010.支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受回 = T00010.最大引受回 	 ");

            sql.Append($"	                LEFT OUTER JOIN m_00020_類名称 	 ");
            sql.Append($"	                    ON t_12040_組合員等別引受情報.共済目的コード = m_00020_類名称.共済目的コード 	 ");
            sql.Append($"	                    AND t_12040_組合員等別引受情報.類区分 = m_00020_類名称.類区分 	 ");
            sql.Append($"	                LEFT OUTER JOIN m_10080_引受方式名称 	 ");
            sql.Append($"	                    ON t_12040_組合員等別引受情報.引受方式 = m_10080_引受方式名称.引受方式 	 ");

            sql.Append($"	            WHERE	 ");
            sql.Append($"	                 t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = @条件_年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = @支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = '20' 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.類区分 <> '0' 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受対象フラグ = '1' 	 ");

            sql.Append($"	            GROUP BY	 ");
            sql.Append($"	                t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.年産 	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.引受回 	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.補償割合コード 	 ");
            sql.Append($"	            UNION ALL 	 ");
            sql.Append($"	            SELECT	 ");
            sql.Append($"	                MIN(t_12040_組合員等別引受情報.組合等コード) As 組合等コード	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.共済目的コード) As 共済目的コード	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.年産) As 年産	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.引受回) As 引受回	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.類区分) As 類区分	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.組合員等コード) As 組合員等コード	 ");
            sql.Append($"	                , MIN(m_00020_類名称.類短縮名称) As 類短縮名称	 ");
            sql.Append($"	                , MIN(m_10080_引受方式名称.引受方式名称) As 引受方式	 ");
            sql.Append($"	                , CAST(MIN(t_12040_組合員等別引受情報.補償割合コード) As Integer) / 10 As 補償割合	 ");
            sql.Append($"	                , CASE	 ");
            sql.Append($"	                    WHEN MIN(t_12040_組合員等別引受情報.特約区分) = '4'	 ");
            sql.Append($"	                        THEN '有り'	 ");
            sql.Append($"	                        ELSE '無し'	 ");
            sql.Append($"	                    END As 一筆半損特約の有無	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.特約区分) As 特約区分	 ");
            sql.Append($"	                , MIN(t_12040_組合員等別引受情報.共済金額選択順位) As 選択順位	 ");
            sql.Append($"	                , '' As 備考 	 "); // 空文字か
            sql.Append($"	            FROM	 ");
            sql.Append($"	                t_12040_組合員等別引受情報 	 ");

            sql.Append($"	                INNER JOIN 	 ");// 不具合修正
            sql.Append($"	                    ( 	 ");
            sql.Append($"	                        SELECT 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                            , MAX(t_00010_引受回.引受回) AS 最大引受回 	 ");
            sql.Append($"	                        FROM 	 ");
            sql.Append($"	                            t_00010_引受回 	 ");
            sql.Append($"	                        GROUP BY 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                    )  T00010 	 ");
            sql.Append($"	                ON t_12040_組合員等別引受情報.組合等コード = T00010.組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = T00010.共済目的コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = T00010.年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = T00010.支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受回 = T00010.最大引受回 	 ");

            sql.Append($"	                LEFT OUTER JOIN m_00020_類名称  	 ");
            sql.Append($"	                    ON t_12040_組合員等別引受情報.共済目的コード = m_00020_類名称.共済目的コード 	 ");
            sql.Append($"	                    AND t_12040_組合員等別引受情報.類区分 = m_00020_類名称.類区分 	 ");
            sql.Append($"	                LEFT OUTER JOIN m_10080_引受方式名称  	 ");
            sql.Append($"	                    ON t_12040_組合員等別引受情報.引受方式 = m_10080_引受方式名称.引受方式 	 ");

            sql.Append($"	            WHERE	 ");
            sql.Append($"	                 t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = @条件_年産	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = @支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = '11' 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受方式 <> '6' 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.類区分 <> '0' 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受対象フラグ = '1' 	 ");

            sql.Append($"	            GROUP BY	 ");
            sql.Append($"	                t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.年産 	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.補償割合コード 	 ");
            sql.Append($"	            UNION ALL 	 ");
            sql.Append($"	            SELECT	 ");
            sql.Append($"	                sui_key.組合等コード As 組合等コード	 ");
            sql.Append($"	                , sui_key.共済目的コード As 共済目的コード	 ");
            sql.Append($"	                , sui_key.年産 As 年産	 ");
            sql.Append($"	                , sui_key.引受回 As 引受回	 ");
            sql.Append($"	                , sui_key.類区分 As 類区分	 ");
            sql.Append($"	                , sui_key.組合員等コード As 組合員等コード	 ");
            sql.Append($"	                , m_00020_類名称.類短縮名称 As 類短縮名称	 ");
            sql.Append($"	                , m_10080_引受方式名称.引受方式名称 As 引受方式	 ");
            sql.Append($"	                , CAST(sui_key.補償割合コード As Integer) / 10 As 補償割合	 ");
            sql.Append($"	                , CASE	 ");
            sql.Append($"	                    WHEN sui_key.特約区分 = '4'	 ");
            sql.Append($"	                        THEN '有り'	 ");
            sql.Append($"	                        ELSE '無し'	 ");
            sql.Append($"	                    END As 一筆半損特約の有無	 ");
            sql.Append($"	                , sui_key.特約区分 As 特約区分	 ");
            sql.Append($"	                , sui_key.共済金額選択順位 As 選択順位	 ");
            sql.Append($"	                , sui_items.備考 As 備考 	 ");
            sql.Append($"	            FROM	 ");
            sql.Append($"	                ( 	 ");
            sql.Append($"	                    SELECT	 ");
            sql.Append($"	                        * 	 ");
            sql.Append($"	                    FROM	 ");
            sql.Append($"	                        ( 	 ");
            sql.Append($"	                            SELECT	 ");
            sql.Append($"	                                t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                , t_12050_組合員等別引受用途.作付時期	 ");
            sql.Append($"	                                , t_12050_組合員等別引受用途.種類区分	 ");
            sql.Append($"	                                , t_12050_組合員等別引受用途.共済金額選択順位	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.補償割合コード	 ");
            sql.Append($"	                                , ROW_NUMBER() OVER ( 	 ");
            sql.Append($"	                                    PARTITION BY	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.年産 	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.補償割合コード 	 ");
            sql.Append($"	                                    ORDER BY	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.補償割合コード	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.作付時期	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.種類区分	 ");
            sql.Append($"	                                ) srt 	 ");
            sql.Append($"	                            FROM	 ");
            sql.Append($"	                                t_12040_組合員等別引受情報 	 ");

            sql.Append($"	                INNER JOIN 	 "); // t_00010_引受回結合
            sql.Append($"	                    ( 	 ");
            sql.Append($"	                        SELECT 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                            , MAX(t_00010_引受回.引受回) AS 最大引受回 	 ");
            sql.Append($"	                        FROM 	 ");
            sql.Append($"	                            t_00010_引受回 	 ");
            sql.Append($"	                        GROUP BY 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                    )  T00010 	 ");
            sql.Append($"	                ON t_12040_組合員等別引受情報.組合等コード = T00010.組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = T00010.共済目的コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = T00010.年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = T00010.支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受回 = T00010.最大引受回 	 ");

            sql.Append($"	                                LEFT OUTER JOIN t_12050_組合員等別引受用途 	 ");
            sql.Append($"	                                    ON t_12040_組合員等別引受情報.組合等コード = t_12050_組合員等別引受用途.組合等コード 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.共済目的コード = t_12050_組合員等別引受用途.共済目的コード 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.年産 = t_12050_組合員等別引受用途.年産 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.引受回 = t_12050_組合員等別引受用途.引受回 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.組合員等コード = t_12050_組合員等別引受用途.組合員等コード 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.類区分 = t_12050_組合員等別引受用途.類区分 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.統計単位地域コード = t_12050_組合員等別引受用途.統計単位地域コード 	 ");

            sql.Append($"	                            WHERE	 ");
            sql.Append($"	                                 t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.年産 = @条件_年産 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.支所コード = @支所コード 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.共済目的コード = '11' 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.引受方式 = '6' 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.類区分 <> '0' 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.引受対象フラグ = '1'	 ");
            sql.Append($"	                        ) 	 ");

            sql.Append($"	                    WHERE	 ");
            sql.Append($"	                        srt = 1	 ");
            sql.Append($"	                ) sui_key 	 ");
            sql.Append($"	                LEFT OUTER JOIN ( 	 ");
            sql.Append($"	                    SELECT	 ");
            sql.Append($"	                        items.組合等コード	 ");
            sql.Append($"	                        , items.共済目的コード	 ");
            sql.Append($"	                        , items.年産	 ");
            sql.Append($"	                        , items.引受回	 ");
            sql.Append($"	                        , items.類区分	 ");
            sql.Append($"	                        , items.組合員等コード	 ");
            sql.Append($"	                        , items.引受方式	 ");
            sql.Append($"	                        , items.特約区分	 ");
            sql.Append($"	                        , items.補償割合コード	 ");

            //sql.Append($"	                        , LISTAGG(items.引受区分名称 + 'は第' + items.共済金額選択順 + '位', '、') WITHIN GROUP (ORDER BY items.作付時期, items.種類区分)	 ");// stringaggに修正
            sql.Append($"	                        , string_agg(items.引受区分名称 || 'は第' || items.共済金額選択順位 || '位', '、' ORDER BY items.作付時期, items.種類区分)	 ");// stringaggに修正

            sql.Append($"	                         As 備考 	 ");
            sql.Append($"	                    FROM	 ");
            sql.Append($"	                        ( 	 ");
            sql.Append($"	                            SELECT	 ");
            sql.Append($"	                                hjhy.組合等コード	 ");
            sql.Append($"	                                , hjhy.共済目的コード	 ");
            sql.Append($"	                                , hjhy.年産	 ");
            sql.Append($"	                                , hjhy.引受回	 ");
            sql.Append($"	                                , hjhy.類区分	 ");
            sql.Append($"	                                , hjhy.組合員等コード	 ");
            sql.Append($"	                                , hjhy.作付時期	 ");
            sql.Append($"	                                , hjhy.種類区分	 ");
            sql.Append($"	                                , m_10090_引受区分名称.引受区分名称	 ");
            sql.Append($"	                                , hjhy.共済金額選択順位	 ");
            sql.Append($"	                                , hjhy.引受方式	 ");
            sql.Append($"	                                , hjhy.特約区分	 ");
            sql.Append($"	                                , hjhy.補償割合コード	 ");
            sql.Append($"	                                , ROW_NUMBER() OVER ( 	 ");
            sql.Append($"	                                    PARTITION BY	 ");
            sql.Append($"	                                        hjhy.組合等コード	 ");
            sql.Append($"	                                        , hjhy.共済目的コード	 ");
            sql.Append($"	                                        , hjhy.年産	 ");
            sql.Append($"	                                        , hjhy.引受回	 ");
            sql.Append($"	                                        , hjhy.類区分	 ");
            sql.Append($"	                                        , hjhy.組合員等コード	 ");
            sql.Append($"	                                        , hjhy.引受方式	 ");
            sql.Append($"	                                        , hjhy.特約区分	 ");
            sql.Append($"	                                        , hjhy.補償割合コード 	 ");
            sql.Append($"	                                    ORDER BY	 ");
            sql.Append($"	                                        hjhy.組合等コード	 ");
            sql.Append($"	                                        , hjhy.共済目的コード	 ");
            sql.Append($"	                                        , hjhy.年産	 ");
            sql.Append($"	                                        , hjhy.引受回	 ");
            sql.Append($"	                                        , hjhy.類区分	 ");
            sql.Append($"	                                        , hjhy.組合員等コード	 ");
            sql.Append($"	                                        , hjhy.引受方式	 ");
            sql.Append($"	                                        , hjhy.特約区分	 ");
            sql.Append($"	                                        , hjhy.補償割合コード	 ");
            sql.Append($"	                                        , hjhy.作付時期	 ");
            sql.Append($"	                                        , hjhy.種類区分	 ");
            sql.Append($"	                                ) srt 	 ");
            sql.Append($"	                            FROM	 ");
            sql.Append($"	                                ( 	 ");
            sql.Append($"	                                    SELECT	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.作付時期	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.種類区分	 ");
            sql.Append($"	                                        , min(t_12050_組合員等別引受用途.共済金額選択順位) As 共済金額選択順位	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.補償割合コード 	 ");
            sql.Append($"	                                    FROM	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報 	 ");

            sql.Append($"	                INNER JOIN 	 "); // t_00010_引受回結合
            sql.Append($"	                    ( 	 ");
            sql.Append($"	                        SELECT 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                            , MAX(t_00010_引受回.引受回) AS 最大引受回 	 ");
            sql.Append($"	                        FROM 	 ");
            sql.Append($"	                            t_00010_引受回 	 ");
            sql.Append($"	                        GROUP BY 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                    )  T00010 	 ");
            sql.Append($"	                ON t_12040_組合員等別引受情報.組合等コード = T00010.組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = T00010.共済目的コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = T00010.年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = T00010.支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受回 = T00010.最大引受回 	 ");

            sql.Append($"	                                        LEFT OUTER JOIN t_12050_組合員等別引受用途 	 ");
            sql.Append($"	                                            ON t_12040_組合員等別引受情報.組合等コード = t_12050_組合員等別引受用途.組合等コード 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.共済目的コード = t_12050_組合員等別引受用途.共済目的コード 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.年産 = t_12050_組合員等別引受用途.年産 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.引受回 = t_12050_組合員等別引受用途.引受回 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.組合員等コード = t_12050_組合員等別引受用途.組合員等コード 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.類区分 = t_12050_組合員等別引受用途.類区分 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.統計単位地域コード = t_12050_組合員等別引受用途.統計単位地域コード 	 ");

            sql.Append($"	                                    WHERE	 ");
            sql.Append($"	                                         t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.年産 = @条件_年産 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.支所コード =  @支所コード 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.共済目的コード = '11' 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.引受方式 <> '6' 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.類区分 <> '0' 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.引受対象フラグ = '1' 	 ");

            sql.Append($"	                                    GROUP BY	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.作付時期	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.種類区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.補償割合コード	 ");
            sql.Append($"	                                ) hjhy 	 ");
            sql.Append($"	                                INNER JOIN m_10090_引受区分名称 	 ");
            sql.Append($"	                                    ON hjhy.共済目的コード = m_10090_引受区分名称.共済目的コード 	 ");
            sql.Append($"	                                    AND hjhy.種類区分 = m_10090_引受区分名称.種類区分	 ");
            sql.Append($"	                                    AND hjhy.作付時期 = m_10090_引受区分名称.作付時期	 ");
            sql.Append($"	                        ) items 	 ");
            sql.Append($"	                    WHERE	 ");
            sql.Append($"	                        items.srt <> 1 	 ");
            sql.Append($"	                    GROUP BY	 ");
            sql.Append($"	                        items.組合等コード	 ");
            sql.Append($"	                        , items.共済目的コード	 ");
            sql.Append($"	                        , items.年産	 ");
            sql.Append($"	                        , items.引受回	 ");
            sql.Append($"	                        , items.類区分	 ");
            sql.Append($"	                        , items.組合員等コード	 ");
            sql.Append($"	                        , items.引受方式	 ");
            sql.Append($"	                        , items.特約区分	 ");
            sql.Append($"	                        , items.補償割合コード	 ");
            sql.Append($"	                ) sui_items 	 ");
            sql.Append($"	                    ON sui_key.組合等コード = sui_items.組合等コード 	 ");
            sql.Append($"	                    AND sui_key.共済目的コード = sui_items.共済目的コード 	 ");
            sql.Append($"	                    AND sui_key.年産 = sui_items.年産 	 ");
            sql.Append($"	                    AND sui_key.引受回 = sui_items.引受回 	 ");
            sql.Append($"	                    AND sui_key.類区分 = sui_items.類区分 	 ");
            sql.Append($"	                    AND sui_key.組合員等コード = sui_items.組合員等コード 	 ");
            sql.Append($"	                    AND sui_key.引受方式 = sui_items.引受方式 	 ");
            sql.Append($"	                    AND sui_key.特約区分 = sui_items.特約区分 	 ");
            sql.Append($"	                    AND sui_key.補償割合コード = sui_items.補償割合コード 	 ");
            sql.Append($"	                LEFT OUTER JOIN m_00020_類名称 	 ");
            sql.Append($"	                    ON sui_key.共済目的コード = m_00020_類名称.共済目的コード 	 ");
            sql.Append($"	                    AND sui_key.類区分 = m_00020_類名称.類区分 	 ");
            sql.Append($"	                LEFT OUTER JOIN m_10080_引受方式名称 	 ");
            sql.Append($"	                    ON sui_key.引受方式 = m_10080_引受方式名称.引受方式 	 ");
            sql.Append($"	            UNION ALL 	 ");
            sql.Append($"	            SELECT	 ");
            sql.Append($"	                mugi_key.組合等コード As 組合等コード	 ");
            sql.Append($"	                , mugi_key.共済目的コード As 共済目的コード	 ");
            sql.Append($"	                , mugi_key.年産 As 年産	 ");
            sql.Append($"	                , mugi_key.引受回 As 引受回	 ");
            sql.Append($"	                , mugi_key.類区分 As 類区分	 ");
            sql.Append($"	                , mugi_key.組合員等コード As 組合員等コード	 ");
            sql.Append($"	                , m_00020_類名称.類短縮名称 As 類短縮名称	 ");
            sql.Append($"	                , m_10080_引受方式名称.引受方式名称 As 引受方式	 ");
            sql.Append($"	                , CAST(mugi_key.補償割合コード As Integer) / 10 As 補償割合	 ");
            sql.Append($"	                , CASE	 ");
            sql.Append($"	                    WHEN mugi_key.特約区分 = '4'	 ");
            sql.Append($"	                        THEN '有り'	 ");
            sql.Append($"	                        ELSE '無し'	 ");
            sql.Append($"	                    END As 一筆半損特約の有無	 ");
            sql.Append($"	                , mugi_key.特約区分 As 特約区分	 ");
            sql.Append($"	                , mugi_key.共済金額選択順位 As 選択順位	 ");
            sql.Append($"	                , mugi_items.備考 As 備考 	 ");
            sql.Append($"	            FROM	 ");
            sql.Append($"	                ( 	 ");
            sql.Append($"	                    SELECT	 ");
            sql.Append($"	                        * 	 ");
            sql.Append($"	                    FROM	 ");
            sql.Append($"	                        ( 	 ");
            sql.Append($"	                            SELECT	 ");
            sql.Append($"	                                t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                , t_12050_組合員等別引受用途.作付時期	 ");
            sql.Append($"	                                , t_12050_組合員等別引受用途.種類区分	 ");
            sql.Append($"	                                , t_12050_組合員等別引受用途.共済金額選択順位	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                , t_12040_組合員等別引受情報.補償割合コード	 ");
            sql.Append($"	                                , ROW_NUMBER() OVER ( 	 ");
            sql.Append($"	                                    PARTITION BY	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.年産 	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.補償割合コード 	 ");
            sql.Append($"	                                    ORDER BY	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.補償割合コード	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.作付時期	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.種類区分	 ");
            sql.Append($"	                                ) srt 	 ");
            sql.Append($"	                            FROM	 ");
            sql.Append($"	                                t_12040_組合員等別引受情報 	 ");

            sql.Append($"	                INNER JOIN 	 "); // t_00010_引受回結合
            sql.Append($"	                    ( 	 ");
            sql.Append($"	                        SELECT 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                            , MAX(t_00010_引受回.引受回) AS 最大引受回 	 ");
            sql.Append($"	                        FROM 	 ");
            sql.Append($"	                            t_00010_引受回 	 ");
            sql.Append($"	                        GROUP BY 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                    )  T00010 	 ");
            sql.Append($"	                ON t_12040_組合員等別引受情報.組合等コード = T00010.組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = T00010.共済目的コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = T00010.年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = T00010.支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受回 = T00010.最大引受回 	 ");

            sql.Append($"	                                LEFT OUTER JOIN t_12050_組合員等別引受用途 	 ");
            sql.Append($"	                                    ON t_12040_組合員等別引受情報.組合等コード = t_12050_組合員等別引受用途.組合等コード 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.共済目的コード = t_12050_組合員等別引受用途.共済目的コード 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.年産 = t_12050_組合員等別引受用途.年産 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.引受回 = t_12050_組合員等別引受用途.引受回 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.組合員等コード = t_12050_組合員等別引受用途.組合員等コード 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.類区分 = t_12050_組合員等別引受用途.類区分 	 ");
            sql.Append($"	                                    AND t_12040_組合員等別引受情報.統計単位地域コード = t_12050_組合員等別引受用途.統計単位地域コード 	 ");

            sql.Append($"	                            WHERE	 ");
            sql.Append($"	                                 t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.年産 = @条件_年産 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.支所コード = @支所コード	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.共済目的コード = '30' 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.類区分 <> '0' 	 ");
            sql.Append($"	                                AND t_12040_組合員等別引受情報.引受対象フラグ = '1'	 ");
            sql.Append($"	                        ) 	 ");

            sql.Append($"	                    WHERE	 ");
            sql.Append($"	                        srt = 1	 ");
            sql.Append($"	                ) mugi_key 	 ");
            sql.Append($"	                LEFT OUTER JOIN ( 	 ");
            sql.Append($"	                    SELECT	 ");
            sql.Append($"	                        items.組合等コード	 ");
            sql.Append($"	                        , items.共済目的コード	 ");
            sql.Append($"	                        , items.年産	 ");
            sql.Append($"	                        , items.引受回	 ");
            sql.Append($"	                        , items.類区分	 ");
            sql.Append($"	                        , items.組合員等コード	 ");
            sql.Append($"	                        , items.引受方式	 ");
            sql.Append($"	                        , items.特約区分	 ");
            sql.Append($"	                        , items.補償割合コード	 ");

            //sql.Append($"	                        , LISTAGG(items.引受区分名称 + 'は第' + items.共済金額選択順 + '位', '、') WITHIN GROUP (ORDER BY items.作付時期, items.種類区分)	 ");// stringaggに修正
            sql.Append($"	                        , string_agg(items.引受区分名称 || 'は第' || items.共済金額選択順位 || '位', '、' ORDER BY items.作付時期, items.種類区分)	 ");// stringaggに修正

            sql.Append($"	                         As 備考 	 ");
            sql.Append($"	                    FROM	 ");
            sql.Append($"	                        ( 	 ");
            sql.Append($"	                            SELECT	 ");
            sql.Append($"	                                hjhy.組合等コード	 ");
            sql.Append($"	                                , hjhy.共済目的コード	 ");
            sql.Append($"	                                , hjhy.年産	 ");
            sql.Append($"	                                , hjhy.引受回	 ");
            sql.Append($"	                                , hjhy.類区分	 ");
            sql.Append($"	                                , hjhy.組合員等コード	 ");
            sql.Append($"	                                , hjhy.作付時期	 ");
            sql.Append($"	                                , hjhy.種類区分	 ");
            sql.Append($"	                                , m_10110_用途区分名称.用途名称	 ");
            sql.Append($"	                                , m_10090_引受区分名称.引受区分名称	 ");　// 追加項目
            sql.Append($"	                                , hjhy.共済金額選択順位	 ");
            sql.Append($"	                                , hjhy.引受方式	 ");
            sql.Append($"	                                , hjhy.特約区分	 ");
            sql.Append($"	                                , hjhy.補償割合コード	 ");
            sql.Append($"	                                , ROW_NUMBER() OVER ( 	 ");
            sql.Append($"	                                    PARTITION BY	 ");
            sql.Append($"	                                        hjhy.組合等コード	 ");
            sql.Append($"	                                        , hjhy.共済目的コード	 ");
            sql.Append($"	                                        , hjhy.年産	 ");
            sql.Append($"	                                        , hjhy.引受回	 ");
            sql.Append($"	                                        , hjhy.類区分	 ");
            sql.Append($"	                                        , hjhy.組合員等コード	 ");
            sql.Append($"	                                        , hjhy.引受方式	 ");
            sql.Append($"	                                        , hjhy.特約区分	 ");
            sql.Append($"	                                        , hjhy.補償割合コード 	 ");
            sql.Append($"	                                    ORDER BY	 ");
            sql.Append($"	                                        hjhy.組合等コード	 ");
            sql.Append($"	                                        , hjhy.共済目的コード	 ");
            sql.Append($"	                                        , hjhy.年産	 ");
            sql.Append($"	                                        , hjhy.引受回	 ");
            sql.Append($"	                                        , hjhy.類区分	 ");
            sql.Append($"	                                        , hjhy.組合員等コード	 ");
            sql.Append($"	                                        , hjhy.引受方式	 ");
            sql.Append($"	                                        , hjhy.特約区分	 ");
            sql.Append($"	                                        , hjhy.補償割合コード	 ");
            sql.Append($"	                                        , hjhy.作付時期	 ");
            sql.Append($"	                                        , hjhy.種類区分	 ");
            sql.Append($"	                                ) srt 	 ");
            sql.Append($"	                            FROM	 ");
            sql.Append($"	                                ( 	 ");
            sql.Append($"	                                    SELECT	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.作付時期	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.種類区分	 ");
            sql.Append($"	                                        , min(t_12050_組合員等別引受用途.共済金額選択順位) As 共済金額選択順位	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.補償割合コード 	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.用途区分 	 ");　// 用途区分足りないので追加した
            sql.Append($"	                                    FROM	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報 	 ");


            sql.Append($"	                INNER JOIN 	 "); // t_00010_引受回結合
            sql.Append($"	                    ( 	 ");
            sql.Append($"	                        SELECT 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                            , MAX(t_00010_引受回.引受回) AS 最大引受回 	 ");
            sql.Append($"	                        FROM 	 ");
            sql.Append($"	                            t_00010_引受回 	 ");
            sql.Append($"	                        GROUP BY 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                    )  T00010 	 ");
            sql.Append($"	                ON t_12040_組合員等別引受情報.組合等コード = T00010.組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = T00010.共済目的コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = T00010.年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = T00010.支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受回 = T00010.最大引受回 	 ");

            sql.Append($"	                                        LEFT OUTER JOIN t_12050_組合員等別引受用途 	 ");
            sql.Append($"	                                            ON t_12040_組合員等別引受情報.組合等コード = t_12050_組合員等別引受用途.組合等コード 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.共済目的コード = t_12050_組合員等別引受用途.共済目的コード 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.年産 = t_12050_組合員等別引受用途.年産 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.引受回 = t_12050_組合員等別引受用途.引受回 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.組合員等コード = t_12050_組合員等別引受用途.組合員等コード 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.類区分 = t_12050_組合員等別引受用途.類区分 	 ");
            sql.Append($"	                                            AND t_12040_組合員等別引受情報.統計単位地域コード = t_12050_組合員等別引受用途.統計単位地域コード 	 ");

            sql.Append($"	                                    WHERE	 ");
            sql.Append($"	                                         t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.年産 = @条件_年産  	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.支所コード = @支所コード  	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.共済目的コード = '30' 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.類区分 <> '0' 	 ");
            sql.Append($"	                                        AND t_12040_組合員等別引受情報.引受対象フラグ = '1' 	 ");

            sql.Append($"	                                    GROUP BY	 ");
            sql.Append($"	                                        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.作付時期	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.種類区分	 ");
            sql.Append($"	                                        , t_12050_組合員等別引受用途.用途区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.引受方式	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.特約区分	 ");
            sql.Append($"	                                        , t_12040_組合員等別引受情報.補償割合コード	 ");
            sql.Append($"	                                ) hjhy 	 ");
            sql.Append($"	                                INNER JOIN m_10110_用途区分名称 	 ");
            sql.Append($"	                                    ON hjhy.共済目的コード = m_10110_用途区分名称.共済目的コード 	 ");
            sql.Append($"	                                    AND hjhy.用途区分 = m_10110_用途区分名称.用途区分	 ");
            sql.Append($"	                                LEFT OUTER JOIN m_10090_引受区分名称 	 "); // 追加結合テーブル
            sql.Append($"	                                    ON hjhy.種類区分 || hjhy.作付時期 = m_10090_引受区分名称.引受区分 	 ");
            sql.Append($"	                        ) items 	 ");
            sql.Append($"	                    WHERE	 ");
            sql.Append($"	                        items.srt <> 1 	 ");
            sql.Append($"	                    GROUP BY	 ");
            sql.Append($"	                        items.組合等コード	 ");
            sql.Append($"	                        , items.共済目的コード	 ");
            sql.Append($"	                        , items.年産	 ");
            sql.Append($"	                        , items.引受回	 ");
            sql.Append($"	                        , items.類区分	 ");
            sql.Append($"	                        , items.組合員等コード	 ");
            sql.Append($"	                        , items.引受方式	 ");
            sql.Append($"	                        , items.特約区分	 ");
            sql.Append($"	                        , items.補償割合コード	 ");
            sql.Append($"	                ) mugi_items 	 ");
            sql.Append($"	                    ON mugi_key.組合等コード = mugi_items.組合等コード 	 ");
            sql.Append($"	                    AND mugi_key.共済目的コード = mugi_items.共済目的コード 	 ");
            sql.Append($"	                    AND mugi_key.年産 = mugi_items.年産 	 ");
            sql.Append($"	                    AND mugi_key.引受回 = mugi_items.引受回 	 ");
            sql.Append($"	                    AND mugi_key.類区分 = mugi_items.類区分 	 ");
            sql.Append($"	                    AND mugi_key.組合員等コード = mugi_items.組合員等コード 	 ");
            sql.Append($"	                    AND mugi_key.引受方式 = mugi_items.引受方式 	 ");
            sql.Append($"	                    AND mugi_key.特約区分 = mugi_items.特約区分 	 ");
            sql.Append($"	                    AND mugi_key.補償割合コード = mugi_items.補償割合コード 	 ");
            sql.Append($"	                LEFT OUTER JOIN m_00020_類名称 	 ");
            sql.Append($"	                    ON mugi_key.共済目的コード = m_00020_類名称.共済目的コード 	 ");　// mugi_key修正
            sql.Append($"	                    AND mugi_key.類区分 = m_00020_類名称.類区分 	 ");　// mugi_key修正
            sql.Append($"	                LEFT OUTER JOIN m_10080_引受方式名称 	 ");
            sql.Append($"	                    ON mugi_key.引受方式 = m_10080_引受方式名称.引受方式	 ");　// mugi_key修正
            sql.Append($"	        ) items	 ");
            sql.Append($"	) 	 ");
            sql.Append($"	, seq_table AS ( 	 ");
            sql.Append($"	    SELECT DISTINCT ");
            sql.Append($"	        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	        , seq.seqnum	 ");
            sql.Append($"	    FROM	 ");
            sql.Append($"	        t_12040_組合員等別引受情報 	 ");

            sql.Append($"	                INNER JOIN 	 "); // t_00010_引受回結合
            sql.Append($"	                    ( 	 ");
            sql.Append($"	                        SELECT 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                            , MAX(t_00010_引受回.引受回) AS 最大引受回 	 ");
            sql.Append($"	                        FROM 	 ");
            sql.Append($"	                            t_00010_引受回 	 ");
            sql.Append($"	                        GROUP BY 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                    )  T00010 	 ");
            sql.Append($"	                ON t_12040_組合員等別引受情報.組合等コード = T00010.組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = T00010.共済目的コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = T00010.年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = T00010.支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受回 = T00010.最大引受回 	 ");

            sql.Append($"	    CROSS JOIN ( 	 ");
            sql.Append($"	        SELECT 	 ");
            sql.Append($"	            LEVEL As seqnum 	 ");
            sql.Append($"	        FROM 	 ");
            sql.Append($"	            GENERATE_SERIES( 1, 8 ) AS LEVEL 	 ");
            sql.Append($"	    ) seq 	 ");

            sql.Append($"	    WHERE	 ");
            sql.Append($"	         t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	        AND t_12040_組合員等別引受情報.年産 = @条件_年産 	 ");
            sql.Append($"	        AND t_12040_組合員等別引受情報.支所コード = @支所コード 	 ");

            sql.Append($"	    ORDER BY	 ");
            sql.Append($"	        t_12040_組合員等別引受情報.組合等コード ASC	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.共済目的コード ASC	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.年産 ASC	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.引受回 ASC	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.組合員等コード ASC	 ");
            sql.Append($"	        , seqnum	 ");
            sql.Append($"	) 	 ");
            sql.Append($"	SELECT	 ");
            sql.Append($"	    seq_table.組合等コード	 ");
            sql.Append($"	    , seq_table.共済目的コード	 ");
            sql.Append($"	    , seq_table.年産	 ");
            sql.Append($"	    , seq_table.引受回	 ");
            sql.Append($"	    , main_table.類区分	 ");
            sql.Append($"	    , seq_table.組合員等コード	 ");
            sql.Append($"	    , main_table.特約区分	 ");
            sql.Append($"	    , main_table.類短縮名称	 ");
            sql.Append($"	    , main_table.引受方式	 ");
            sql.Append($"	    , main_table.補償割合	 ");
            sql.Append($"	    , main_table.一筆半損特約の有無	 ");
            sql.Append($"	    , main_table.選択順位	 ");
            sql.Append($"	    , main_table.備考	 ");
            sql.Append($"	    , seq_table.seqnum 	 ");
            sql.Append($"	FROM	 ");
            sql.Append($"	    main_table 	 ");
            sql.Append($"	    LEFT OUTER JOIN seq_table 	 ");
            sql.Append($"	        ON seq_table.組合等コード = main_table.組合等コード 	 ");
            sql.Append($"	        AND seq_table.共済目的コード = main_table.共済目的コード 	 ");
            sql.Append($"	        AND seq_table.年産 = main_table.年産 	 ");
            sql.Append($"	        AND seq_table.引受回 = main_table.引受回 	 ");
            sql.Append($"	        AND seq_table.組合員等コード = main_table.組合員等コード 	 ");
            sql.Append($"	        AND seq_table.seqnum = main_table.recnum	 "); //reqnum

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("条件_組合等コード", jouken.JoukenKumiaitoCd),
                new("条件_年産", int.Parse(jouken.JoukenNensan)),
                new("支所コード", shishoCd)
            ];

            // SQLのクエリ結果をListに格納する
            List<TyouhyouDataSub1> tyouhyouDatas = dbContext.Database.SqlQueryRaw<TyouhyouDataSub1>(sql.ToString(), parameters.ToArray()).ToList();

            return tyouhyouDatas;
        }

        /// <summary>
        /// 帳票の出力対象データの取得（SUB2レポート用）
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jouken">バッチ条件</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns></returns>
        private static List<TyouhyouDataSub2> GetTyouhyouDataSub2(NskAppContext dbContext, BatchJouken jouken, string shishoCd)
        {
            StringBuilder sql = new();

            sql.Append($"	WITH main_table AS ( 	 ");
            sql.Append($"	    SELECT	 ");
            sql.Append($"	        items.*	 ");
            sql.Append($"	        , ROW_NUMBER() OVER ( 	 ");
            sql.Append($"	            PARTITION BY	 ");
            sql.Append($"	                組合等コード	 ");
            sql.Append($"	                , 共済目的コード	 ");
            sql.Append($"	                , 年産	 ");
            sql.Append($"	                , 引受回	 ");
            sql.Append($"	                , 組合員等コード 	 ");
            sql.Append($"	            ORDER BY	 ");
            sql.Append($"	                組合等コード	 ");
            sql.Append($"	                , 共済目的コード	 ");
            sql.Append($"	                , 年産	 ");
            sql.Append($"	                , 引受回	 ");
            sql.Append($"	                , 組合員等コード	 ");
            sql.Append($"	        ) recnum 	 ");
            sql.Append($"	    FROM	 ");
            sql.Append($"	        ( 	 ");
            sql.Append($"	            SELECT	 ");
            sql.Append($"	                t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.類区分	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.統計単位地域コード	 ");
            sql.Append($"	                , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	                , m_00070_収穫量確認方法名称.加入申込区分 	 ");
            sql.Append($"	                , COALESCE(m_00020_類名称.類短縮名称, '') AS 類短縮名称 ");　// 類区分追加
            sql.Append($"	            FROM	 ");
            sql.Append($"	                t_12040_組合員等別引受情報 	 ");
            sql.Append($"	                LEFT OUTER JOIN m_00070_収穫量確認方法名称 	 ");
            sql.Append($"	                    ON t_12040_組合員等別引受情報.収穫量確認方法 = m_00070_収穫量確認方法名称.収穫量確認方法 	 ");
            sql.Append($"	                LEFT OUTER JOIN m_00020_類名称 	 ");　// 類区分追加
            sql.Append($"	                    ON t_12040_組合員等別引受情報.共済目的コード = m_00020_類名称.共済目的コード 	 ");
            sql.Append($"	                    AND t_12040_組合員等別引受情報.類区分 = m_00020_類名称.類区分 	 ");
            sql.Append($"	            WHERE	 ");
            sql.Append($"	                t_12040_組合員等別引受情報.類区分 <> '0'	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受方式 = '3'	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受対象フラグ = '1'	 ");
            sql.Append($"	        ) items	 ");
            sql.Append($"	) 	 ");
            sql.Append($"	, seq_table AS ( 	 ");

            sql.Append($"	    SELECT DISTINCT	 ");
            sql.Append($"	        t_12040_組合員等別引受情報.組合等コード	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.共済目的コード	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.年産	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.引受回	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.組合員等コード	 ");
            sql.Append($"	        , seq.seqnum	 ");
            sql.Append($"	    FROM	 ");
            sql.Append($"	        t_12040_組合員等別引受情報 	 ");


            sql.Append($"	                INNER JOIN 	 "); // t_00010_引受回結合
            sql.Append($"	                    ( 	 ");
            sql.Append($"	                        SELECT 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                            , MAX(t_00010_引受回.引受回) AS 最大引受回 	 ");
            sql.Append($"	                        FROM 	 ");
            sql.Append($"	                            t_00010_引受回 	 ");
            sql.Append($"	                        GROUP BY 	 ");
            sql.Append($"	                            t_00010_引受回.組合等コード 	 ");
            sql.Append($"	                            , t_00010_引受回.共済目的コード 	 ");
            sql.Append($"	                            , t_00010_引受回.年産 	 ");
            sql.Append($"	                            , t_00010_引受回.支所コード 	 ");
            sql.Append($"	                    )  T00010 	 ");
            sql.Append($"	                ON t_12040_組合員等別引受情報.組合等コード = T00010.組合等コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.共済目的コード = T00010.共済目的コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.年産 = T00010.年産 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.支所コード = T00010.支所コード 	 ");
            sql.Append($"	                AND t_12040_組合員等別引受情報.引受回 = T00010.最大引受回 	 ");

            sql.Append($"	    CROSS JOIN ( 	 ");
            sql.Append($"	        SELECT 	 ");
            sql.Append($"	            LEVEL As seqnum 	 ");
            sql.Append($"	        FROM 	 ");
            sql.Append($"	            GENERATE_SERIES( 1, 8 ) AS LEVEL 	 ");
            sql.Append($"	    ) seq 	 ");

            sql.Append($"	    WHERE	 ");
            sql.Append($"	         t_12040_組合員等別引受情報.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	        AND t_12040_組合員等別引受情報.年産 = @条件_年産 	 ");
            sql.Append($"	        AND t_12040_組合員等別引受情報.支所コード = @支所コード  	 ");


            sql.Append($"	    ORDER BY	 ");
            sql.Append($"	        t_12040_組合員等別引受情報.組合等コード ASC	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.共済目的コード ASC	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.年産 ASC	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.引受回 ASC	 ");
            sql.Append($"	        , t_12040_組合員等別引受情報.組合員等コード ASC	 ");
            sql.Append($"	        , seqnum	 ");
            sql.Append($"	) 	 ");
            sql.Append($"	SELECT	 ");
            sql.Append($"	    seq_table.組合等コード	 ");
            sql.Append($"	    , seq_table.共済目的コード	 ");
            sql.Append($"	    , seq_table.年産	 ");
            sql.Append($"	    , seq_table.引受回	 ");
            sql.Append($"	    , main_table.類区分	 ");
            sql.Append($"	    , main_table.類短縮名称	 ");    // 類短縮名称
            sql.Append($"	    , main_table.統計単位地域コード	 ");
            sql.Append($"	    , main_table.加入申込区分	 ");
            sql.Append($"	    , seq_table.seqnum 	 ");
            sql.Append($"	FROM	 ");
            sql.Append($"	    main_table 	 ");
            sql.Append($"	    INNER JOIN seq_table 	 ");
            sql.Append($"	        ON seq_table.組合等コード = main_table.組合等コード 	 ");
            sql.Append($"	        AND seq_table.共済目的コード = main_table.共済目的コード 	 ");
            sql.Append($"	        AND seq_table.年産 = main_table.年産 	 ");
            sql.Append($"	        AND seq_table.引受回 = main_table.引受回 	 ");
            sql.Append($"	        AND seq_table.組合員等コード = main_table.組合員等コード 	 ");
            sql.Append($"	        AND seq_table.seqnum = main_table.recnum	 ");　//reqnum

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("条件_組合等コード", jouken.JoukenKumiaitoCd),
                new("条件_年産", int.Parse(jouken.JoukenNensan)),
                new("条件_共済目的コード", jouken.JoukenKyosaiMokutekiCd),
                new("支所コード", shishoCd)
            ];

            // SQLのクエリ結果をListに格納する
            List<TyouhyouDataSub2> tyouhyouDatas = dbContext.Database.SqlQueryRaw<TyouhyouDataSub2>(sql.ToString(), parameters.ToArray()).ToList();

            return tyouhyouDatas;
        }
    }
}
