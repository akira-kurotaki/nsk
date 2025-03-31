using NskReportMain.Common;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;
using NskReportLibrary.Core.Base;
using System.Text;
using NSK_P111020.Models;
using NSK_P111020.ReportCreators.NSK_111020P;
using NskReportLibrary.Core.Consts;

namespace NSK_P111020.Controllers
{
    /// <summary>
    /// 交付金申請書（別記様式第１号）帳票制御
    /// </summary>
    public class NSK_111020PController : ReportController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbInfo">DB接続情報</param>
        public NSK_111020PController(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        /// <summary>
        /// 交付金申請書（別記様式第１号）制御処理
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
            // バッチID
            string bid = string.Empty;
            // 都道府県コード
            string todofukenCd = string.Empty;
            // 組合等コード
            string kumiaitoCd = string.Empty;
            // 支所コード
            string shishoCd = string.Empty;
            // バッチ条件のキー情報
            string jid = string.Empty;

            try
            {
                // args から 各変数へ展開する
                // ユーザーID
                if (args.Length > 0)
                {
                    userId = args[0];
                }
                // バッチID
                if (args.Length > 1)
                {
                    bid = args[1];
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
                // バッチ条件のキー情報
                if (args.Length > 5)
                {
                    jid = args[5];
                }

                // １．引数のチェック
                IsRequired(userId, bid, todofukenCd, kumiaitoCd, shishoCd, jid, out int nBid);

                // ２．データ検索SQLを実行（ログ出力：あり）
                // 帳票作成条件
                TyouhyouSakuseiJouken tyouhyouSakuseiJouken = new();
                // ２．１．帳票作成条件の取得
                tyouhyouSakuseiJouken.GetTyouhyouSakuseiJouken(dbContext, jid);

                // 必須入力チェック
                tyouhyouSakuseiJouken.IsRequired();

                // ３．コードの整合性チェック
                tyouhyouSakuseiJouken.IsConsistency(dbContext, todofukenCd);

                // ４．帳票の出力対象データを取得する。
                // ４．１．帳票の出力対象データを取得する。
                List<TyouhyouShousaiData> tyouhyouOutputDatas = GetTyouhyouData(dbContext, tyouhyouSakuseiJouken, todofukenCd);

                // ４．２．取得した件数が0件の場合
                if (tyouhyouOutputDatas.Count == 0)
                {
                    // [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「６．」へ進む。
                    // （"ME10076" 引数{0}：0)
                    throw new AppException("ME10076", MessageUtil.Get("ME10076", "0"));
                }

                // ４．３．取得した件数が0件以外の場合
                // ４．３．１．ZIPファイル格納先パスを作成して変数に設定する
                // [変数：ZIPファイル格納先パス]    ←   [設定ファイル：ReportOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                string zipFileFolderPath = Path.Combine(ConfigUtil.Get(CoreLibrary.Core.Consts.CoreConst.REPORT_OUTPUT_FOLDER),
                    bid + CoreLibrary.Core.Consts.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                // ４．３．２．作成したZIPファイル格納先パスにZIPファイル格納フォルダを作成する
                Directory.CreateDirectory(zipFileFolderPath);

                // ５．帳票作成
                // ５．１．一時領域に帳票PDF一時出力フォルダを作成する。
                // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/
                string tempFileFolderPath = Path.Combine(ConfigUtil.Get(NskCommonLibrary.Core.Consts.CoreConst.FILE_TEMP_FOLDER_PATH),
                        bid + CoreLibrary.Core.Consts.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                Directory.CreateDirectory(tempFileFolderPath);

                // ５．２．帳票「交付金申請書（別記様式第１号）」を作成する。
                // ５．２．１．交付金申請書（別記様式第１号）出力処理を呼び出す。
                // 引数{0}：「４．１．」で取得したデータ
                NSK_111020PModel tyouhyouDatas = new()
                {
                    TyouhyouSakuseiJouken = tyouhyouSakuseiJouken,
                    TyouhyouShousaiDatas = tyouhyouOutputDatas
                };
                CreatorResult creatorResult = NSK_111020PCreator.CreateReport(tyouhyouDatas);
                // 作成された帳票をPDFファイルとして、帳票PDF一時出力フォルダに出力する。
                // ファイル名は「[変数：条件_帳票名].PDF」とする。
                string tempFileName = tyouhyouSakuseiJouken.JoukenReportName + ReportConst.REPORT_EXTENSION;
                string tempPath = Path.Combine(tempFileFolderPath, tempFileName);
                pdfExport.Export(creatorResult.SectionReport.Document, tempPath);

                // ５．３．Zip暗号化を行う。
                // ５．３．１．「５．１．」のフォルダ内のPDFをZip化（暗号化）し、
                Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFileFolderPath);
                // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                // [変数：ZIPファイル格納先パス] とファイル名でパスを登録します。
                FolderUtil.MoveFile(zipFilePath, zipFileFolderPath, userId, nBid);

                // 「５．２．」のフォルダを削除する。
                if (Directory.Exists(tempFileFolderPath))
                {
                    Directory.Delete(tempFileFolderPath, true);
                }

                // ６．終了処理
                if (creatorResult.Result == ReportConst.RESULT_SUCCESS)
                {
                    controllerResult.Result = ReportConst.RESULT_SUCCESS;
                }
                else
                {
                    controllerResult.Result = ReportConst.RESULT_FAILED;
                    // [変数：エラーメッセージ] に以下のメッセージを設定
                    // （"ME01645" 引数{0}：交付金申請書（別記様式第１号）の出力)
                    controllerResult.ErrorMessageId = "ME01645";
                    controllerResult.ErrorMessage = MessageUtil.Get("ME01645", "交付金申請書（別記様式第１号）の出力");
                }
            }
            catch (Exception ex)
            {
                // 例外の内容をログに出力する。
                logger.Error(ex);
                controllerResult.Result = ReportConst.RESULT_FAILED;
                // [変数：エラーメッセージ] に以下のメッセージを設定
                // （"MF00001":予期せぬエラーが発生しました。システム管理者に連絡してください。）
                controllerResult.ErrorMessageId = "MF00001";
                controllerResult.ErrorMessage = MessageUtil.Get("MF00001");
            }

            return controllerResult;
        }

        /// <summary>
        /// 引数のチェック
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <param name="bid">バッチID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="jid">バッチ条件のキー情報</param>
        /// <param name="nBid">数値型バッチID</param>
        /// <exception cref="AppException"></exception>
        private static void IsRequired(string userId, string bid, string todofukenCd, string kumiaitoCd, string shishoCd, string jid, out int nBid)
        {
            // １．１．必須項目が未入力の場合、エラーとし、エラーメッセージを返す。
            // [変数：ユーザID] が未入力の場合
            if (string.IsNullOrEmpty(userId))
            {
                // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                // （"ME01054" 引数{0} ：ユーザID）
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "ユーザID"));
            }

            // [変数：バッチID] が未入力の場合
            if (string.IsNullOrEmpty(bid))
            {
                // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                // （"ME01054" 引数{0} ：バッチID）
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチID"));
            }

            // [変数：バッチID]が数値変換不可の場合
            // 数値化したバッチID：nBid
            if (!int.TryParse(bid, out nBid))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、処理を終了する。
                // （"ME90012"　引数{0} ：バッチID)
                throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
            }

            // [変数：都道府県コード]が未入力の場合
            if (string.IsNullOrEmpty(todofukenCd))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、処理を終了する。
                // （"ME01054"　引数{0} ：都道府県コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "都道府県コード"));
            }

            // [変数：組合等コード] が未入力の場合
            if (string.IsNullOrEmpty(kumiaitoCd))
            {
                //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                //（"ME01054" 引数{0} ：組合等コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
            }

            // [変数：支所コード] が未入力の場合
            if (string.IsNullOrEmpty(shishoCd))
            {
                //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                //（"ME01054" 引数{0} ：支所コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "支所コード"));
            }

            // [変数：バッチ条件のキー情報] が未入力の場合
            if (string.IsNullOrEmpty(jid))
            {
                //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、処理を終了する。
                //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
            }
        }

        /// <summary>
        /// 帳票の出力対象データの取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="tyouhyouSakuseiJouken">帳票作成条件</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <returns></returns>
        private static List<TyouhyouShousaiData> GetTyouhyouData(NskAppContext dbContext, TyouhyouSakuseiJouken tyouhyouSakuseiJouken, string todofukenCd)
        {
            StringBuilder sql = new();

            sql.Append($"SELECT ");
            sql.Append($"   CASE ");
            sql.Append($"       WHEN T1.組合等交付金の金額 - T2.既受領交付金の金額 > 0 ");
            sql.Append($"           THEN '年度農作物共済に係る特定組合等（組合等）交付金交付申請書' ");
            sql.Append($"       ELSE '年度農作物共済に係る特定組合等（組合等）交付金返還申請書' ");
            sql.Append($"   END AS タイトル ");
            sql.Append($",  T3.kumiaito_nm AS 組合等正式名称");
            sql.Append($",  T3.kumiaitocho_nm AS 組合等長名 ");
            sql.Append($",  T1.年産 AS 年産 ");
            sql.Append($",  T1.組合等交付金の金額 - T2.既受領交付金の金額 AS 今回申請額 ");
            sql.Append($",  CASE ");
            sql.Append($"       WHEN T1.組合等交付金の金額 - T2.既受領交付金の金額 > 0 ");
            sql.Append($"           THEN '年産水稲及び陸稲（麦）に係る特定組合等（組合等）交付金については、下記の実績に基づき金' ");
            sql.Append($"                || TO_CHAR(T1.組合等交付金の金額 - T2.既受領交付金の金額, 'FM999,999,999,999') || '円の交付を受けたく申請する。' ");
            sql.Append($"           ELSE '年産水稲及び陸稲（麦）に係る特定組合等（組合等）交付金については、下記の実績に基づき金' ");
            sql.Append($"                || TO_CHAR(ABS(T1.組合等交付金の金額 - T2.既受領交付金の金額), 'FM999,999,999,999') || '円の返還をしたく申請する。' ");
            sql.Append($"   END AS 文言 ");
            sql.Append($",  CASE T1.負担金交付区分 ");     //※取得必須
            sql.Append($"       WHEN '1' THEN '稲' ");
            sql.Append($"       WHEN '2' THEN '麦' ");
            sql.Append($"       ELSE '' ");
            sql.Append($"   END AS 負担金交付区分 ");
            sql.Append($",  T1.引受面積 ");
            sql.Append($",  T1.引受収量 ");
            sql.Append($",  T1.共済金額 ");
            sql.Append($",  T1.（組合等別連合会）保険料 AS 組合等別連合会保険料 "); // ※(Ａ)項目
            sql.Append($",  T1.組合等別国庫負担金 ");        // ※(Ｂ)項目
            sql.Append($",  T1.特定組合等（組合等）負担金交付対象金額 AS 特定組合等組合等負担金交付対象金額");  // ※(Ｃ)項目 = (Ｂ) - (Ａ)の計算
            sql.Append($",  T1.徴収すべき共済掛金 ");    // ※(Ｄ)項目
            sql.Append($",  T1.左のうち徴収済額 ");     // ※(E)項目
            sql.Append($",  T1.共済掛金徴収割合 ");     // ※(F)項目 = (Ｅ) / (Ｄ)の計算結果
            sql.Append($",  T1.組合等交付金の金額 AS 特定組合等組合等交付金の金額 ");    // ※(G)項目 = (Ｃ) × (Ｆ)の計算結果
            sql.Append($",  T2.既受領交付金の金額 ");    // ※(Ｈ)項目
            sql.Append($",  T1.組合等交付金の金額 - T2.既受領交付金の金額 AS 今回交付申請額又は今回返還申請額 ");   // (Ｇ) - (Ｈ)の計算結果

            sql.Append($"FROM ");
            sql.Append($"   ( ");

            sql.Append($"SELECT ");
            sql.Append($"   組合等コード ");
            sql.Append($",  年産 ");
            sql.Append($",  負担金交付区分 ");
            sql.Append($",  交付回 ");
            sql.Append($",  引受面積 ");
            sql.Append($",  引受収量 ");
            sql.Append($",  共済金額 ");
            sql.Append($",  保険料総額 AS （組合等別連合会）保険料 ");   // ※(Ａ)項目
            sql.Append($",  組合等別国庫負担金 ");   // ※(Ｂ)項目
            sql.Append($",  組合等別国庫負担金 - 保険料総額 AS 特定組合等（組合等）負担金交付対象金額 ");    // ※(Ｃ)項目
            sql.Append($",  組合員等負担共済掛金 AS 徴収すべき共済掛金 ");     // ※(Ｄ)項目
            sql.Append($",  組合員等負担共済掛金徴収済額 AS 左のうち徴収済額 ");  // ※(E)項目
            sql.Append($",  CASE ");
            sql.Append($"       WHEN 組合員等負担共済掛金徴収済額 > 0 ");
            sql.Append($"           THEN TRUNC (組合員等負担共済掛金徴収済額 / 組合員等負担共済掛金 * 100, 2) ");
            sql.Append($"       ELSE 0 ");
            sql.Append($"   END AS 共済掛金徴収割合 ");     // ※(F)項目
            sql.Append($",  組合等交付金の金額 ");   // ※(G)項目

            sql.Append($"FROM ");
            sql.Append($"   t_15020_組合等交付 ");

            sql.Append($"   )  T1 ");

            sql.Append($"LEFT OUTER JOIN ");
            sql.Append($"   ( ");

            sql.Append($"SELECT ");
            sql.Append($"   組合等コード ");
            sql.Append($",  年産 ");
            sql.Append($",  負担金交付区分 ");
            sql.Append($",  SUM(組合等交付金の金額) AS 既受領交付金の金額 ");

            sql.Append($"FROM ");
            sql.Append($"   t_15020_組合等交付 ");

            sql.Append($"WHERE ");
            sql.Append($"   交付回 <= @条件_交付回 - 1 ");

            sql.Append($"GROUP BY ");
            sql.Append($"   組合等コード ");
            sql.Append($",  年産 ");
            sql.Append($",  負担金交付区分 ");

            sql.Append($"   )  T2 ");
            sql.Append($"ON ");
            sql.Append($"       T2.組合等コード = T1.組合等コード ");
            sql.Append($"   AND T2.年産 = T1.年産 ");
            sql.Append($"   AND T2.負担金交付区分 = T1.負担金交付区分 ");

            sql.Append($"INNER JOIN ");
            sql.Append($"   v_kumiaito T3 ");
            sql.Append($"ON ");
            sql.Append($"       T3.kumiaito_cd = T1.組合等コード ");
            sql.Append($"   AND T3.todofuken_cd = @条件_都道府県コード ");

            sql.Append($"WHERE ");
            sql.Append($"       T1.組合等コード = @条件_組合等コード ");
            sql.Append($"   AND T1.年産 = @条件_年産 ");
            sql.Append($"   AND T1.負担金交付区分 = @条件_負担金交付区分 ");
            sql.Append($"   AND T1.交付回 = @条件_交付回 ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("条件_組合等コード", tyouhyouSakuseiJouken.JoukenKumiaitoCd),
                new("条件_年産", int.Parse(tyouhyouSakuseiJouken.JoukenNensan)),
                new("条件_負担金交付区分", tyouhyouSakuseiJouken.JoukenFutankinKofuKbn),
                new("条件_交付回", int.Parse(tyouhyouSakuseiJouken.JoukenKoufuKai)),
                new("条件_都道府県コード", todofukenCd)
            ];

            object joukenBunshoNm = DBNull.Value;
            if (!string.IsNullOrEmpty(tyouhyouSakuseiJouken.JoukenBunshoNo))
            {
                joukenBunshoNm = tyouhyouSakuseiJouken.JoukenBunshoNo;
            }
            parameters.Add(new("条件_文書番号", joukenBunshoNm));

            object joukenHakkouDate = DBNull.Value;
            if (!string.IsNullOrEmpty(tyouhyouSakuseiJouken.JoukenHakkoDate))
            {
                joukenHakkouDate = tyouhyouSakuseiJouken.JoukenHakkoDate;
            }
            parameters.Add(new("条件_発行年月日", joukenHakkouDate));

            // SQLのクエリ結果をListに格納する
            List<TyouhyouShousaiData> tyouhyouDatas = dbContext.Database.SqlQueryRaw<TyouhyouShousaiData>(sql.ToString(), parameters.ToArray()).ToList();

            return tyouhyouDatas;
        }
    }
}
