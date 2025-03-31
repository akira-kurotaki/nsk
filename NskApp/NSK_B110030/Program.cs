using System.Text;
using Microsoft.EntityFrameworkCore;
using NLog;
using Npgsql;
using Core = CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskCommon = NskCommonLibrary.Core.Consts;
using NSK_B110030.Models;

namespace NSK_B110030
{
    /// <summary>
    /// 危険段階連係データ作成（見込共済金額）
    /// </summary>
    class Program
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Program()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 危険段階連係データ作成（見込共済金額）
        /// </summary>
        /// <param name="args">
        /// 引数配列要素1：バッチID
        /// 引数配列要素2：都道府県コード
        /// 引数配列要素3：組合等コード
        /// 引数配列要素4：支所コード
        /// 引数配列要素5：バッチ条件のキー情報
        /// </param>
        public static void Main(string[] args)
        {
            // 引数
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

            // [変数：エラーメッセージ] string.Empty
            string errorMessage = string.Empty;
            // [変数：ステータス] STATUS_SUCCESS = "03"（成功）
            string status = NskCommon.CoreConst.STATUS_SUCCESS;
            // 処理結果（正常：0、エラー：1）
            int result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
            // バッチ予約ユーザID
            string batchYoyakuId = string.Empty;

            try
            {
                // １．設定ファイルから、以下の内容を取得し、グローバル変数へ保存する。
                // システム時間フラグ    （検索キー：SysDateTimeFlag）
                // appsetting.jsonの設定にあるシステム時間フラグ
                bool sysDateTimeFlag = false;
                if (bool.TryParse(ConfigUtil.Get(Core.CoreConst.SYS_DATE_TIME_FLAG), out sysDateTimeFlag))
                {
                    // システム時間ファイルパス （検索キー：SysDateTimePath）※本項目は「システム時間フラグ」が"true"で取得できた場合のみ対象
                    // appsetting.jsonの設定にあるシステム時間ファイルパス
                    string sysDateTimePath = string.Empty;
                    if (sysDateTimeFlag)
                    {
                        // システム時間ファイルパス
                        sysDateTimePath = ConfigUtil.Get(Core.CoreConst.SYS_DATE_TIME_PATH);

                        if (string.IsNullOrEmpty(sysDateTimePath))
                        {
                            // エラーメッセージをログに出力し、処理を中断する。
                            // （"ME90015"、{0}：システム時間ファイルパス）
                            logger.Error(MessageUtil.Get("ME90015", "システム時間ファイルパス"));
                            result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
                            Environment.ExitCode = result;
                            return;
                        }
                    }
                }
                else
                {
                    // エラーメッセージをログに出力し、処理を中断する。
                    // （"ME90015"、{0}：システム時間フラグ）
                    logger.Error(MessageUtil.Get("ME90015", "システム時間フラグ"));
                    result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
                    Environment.ExitCode = result;
                    return;
                }

                // ２．システム日付の設定を行う。
                // ２．１．システム日付の設定
                // （ア）[変数：システム日付] に以下の条件に従って設定を行う。
                // （１）[グローバル変数：システム時間フラグ] がtrueの場合、[グローバル変数：システム時間ファイルパス] の年月日＋マシン時間を設定する。
                // （２）上記以外、マシン時間を設定する。
                // [変数：システム日付]
                DateTime systemDate = DateUtil.GetSysDateTime(); // DateUtil.GetSysDateTime()内で全てやっている

                // args から 各変数へ展開する
                // バッチID
                if (args.Length > 0)
                {
                    bid = args[0];
                }
                // 都道府県コード
                if (args.Length > 1)
                {
                    todofukenCd = args[1];
                }
                // 組合等コード
                if (args.Length > 2)
                {
                    kumiaitoCd = args[2];
                }
                // 支所コード
                if (args.Length > 3)
                {
                    shishoCd = args[3];
                }
                // バッチ条件のキー情報
                if (args.Length > 4)
                {
                    jid = args[4];
                }

                // ３．１．必須チェック
                // ３．１．１．以下の引数が未入力の場合、[変数：エラーメッセージ]を設定し、「７.」へ進む。
                if (string.IsNullOrEmpty(bid) || !int.TryParse(bid, out int  nBid) || string.IsNullOrEmpty(todofukenCd) || string.IsNullOrEmpty(kumiaitoCd) || string.IsNullOrEmpty(shishoCd) || string.IsNullOrEmpty(jid))
                {
                    // [変数：エラーメッセージ]に、以下のエラーメッセージを設定
                    // （"ME01645" 引数{0} ：パラメータの取得）
                    errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                    throw new AppException("ME01645", errorMessage);
                }

                // ３．１．２．バッチ予約状況取得
                // バッチ予約状況取得引数の設定
                BatchUtil.GetBatchYoyakuListParam batchYoyakuListparam = new()
                {
                    SystemKbn = Core.CoreConst.SystemKbn.Nsk,
                    TodofukenCd = todofukenCd,
                    KumiaitoCd = kumiaitoCd,
                    ShishoCd = shishoCd,
                    BatchId = nBid
                };
                // 総件数取得フラグ
                bool boolAllCntFlg = false;
                // 件数（出力パラメータ）
                int intAllCnt = 0;
                // エラーメッセージ（出力パラメータ）
                string message = string.Empty;
                // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
                List<BatchYoyaku> batchYoyakuList = BatchUtil.GetBatchYoyakuList(batchYoyakuListparam, boolAllCntFlg, ref intAllCnt, ref message);

                // バッチ予約が存在しない場合、
                if (batchYoyakuList.Count == 0)
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：パラメータの取得)
                    errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                    throw new AppException("ME01645", errorMessage);
                }

                // バッチ予約が存在する場合
                foreach (BatchYoyaku batchYoyaku in batchYoyakuList)
                {
                    // [引数：バッチID]に一致する場合
                    if (batchYoyaku.BatchId == nBid)
                    {
                        // ３．１．２．２．取得した「バッチ予約状況」から値を取得し変数に設定する。
                        // [変数：バッチ予約ユーザID] = [バッチ予約情報：予約ユーザID]
                        batchYoyakuId = batchYoyaku.BatchYoyakuId;
                    }
                    // [引数：バッチID]に一致するバッチ予約状況が取得できない場合、
                    else
                    {
                        // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                        //（"ME01645" 引数{0} ：パラメータの取得)
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }
                }

                // DB接続
                // ※「共通機能設計_070_DB切替」の「バッチのDB接続先取得処理」を参照。
                // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                // DB接続情報
                DbConnectionInfo? dbInfo = null;
                dbInfo = DBUtil.GetDbConnectionInfo(Core.CoreConst.SystemKbn.Nsk, todofukenCd, kumiaitoCd, shishoCd);
                if (dbInfo == null)
                {
                    errorMessage = MessageUtil.Get("ME90014");
                    throw new AppException("ME90014", errorMessage);
                }

                using (NskAppContext dbContext = new(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt(Core.CoreConst.COMMAND_TIMEOUT)))
                {
                    // ３．１．３．「バッチ条件情報」の取得
                    // ３．１．４．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータのを条件値を変数に設定する。
                    // バッチ条件情報
                    BatchJouken batchJouken = new();
                    batchJouken.GetBatchJoukens(dbContext, jid, batchJouken);

                    // 必須入力チェック
                    if (!batchJouken.IsRequired())
                    {
                        // [変数：エラーメッセージ]に、以下のエラーメッセージを設定し、「７.」へ進む。
                        // （"ME01645"　引数{0} ：パラメータの取得)
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }

                    // ４．コードの整合性チェック
                    if (!batchJouken.IsConsistency(dbContext))
                    {
                        // [変数：エラーメッセージ]に、以下のエラーメッセージを設定し、「７.」へ進む。
                        // （"ME91003"、引数{0}：共済目的コード、引数{1}：共済目的マスタ)
                        errorMessage = MessageUtil.Get("ME91003", "共済目的コード", "共済目的マスタ");
                        throw new AppException("ME91003", errorMessage);
                    }

                    // ５．データ検索SQLを実行（ログ出力：あり）
                    // ５．１．「危険段階連係データ」を取得する。
                    List<KikenDankaiRenkeiData> dangerStageRenkeiData = GetDangerStageRenkeiData(dbContext, batchJouken);

                    // ５．２．取得した件数が0件の場合
                    // [変数：エラーメッセージ]を設定し、「７.」へ進む。
                    if (dangerStageRenkeiData.Count == 0)
                    {
                        // [変数：エラーメッセージ]に、以下のエラーメッセージを設定し、「７.」へ進む。
                        // （"MI10009" 引数{0}：0)
                        errorMessage = MessageUtil.Get("MI00011");
                        throw new AppException("MI00011", errorMessage);
                    }

                    // ５．３．取得した件数が0件以外の場合
                    // ５．３．１．ZIPファイル格納先パスを作成して変数に設定する
                    // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                    string zipFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.CSV_OUTPUT_FOLDER), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    Directory.CreateDirectory(zipFolderPath);

                    // ６．危険段階連係データ出力処理
                    // ６．１．[変数：条件_文字コード]で指定した文字コードの出力用危険段階連係データファイル作成
                    // 一時領域にデータ一時出力フォルダとファイルを作成する
                    // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/
                    string tempFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    Directory.CreateDirectory(tempFolderPath);
                    // ファイル名：KKN_DAN_MST.TXT
                    string fileName = "KKN_DAN_MST.TXT";
                    string filePath = Path.Combine(tempFolderPath, fileName);

                    // ６．２．危険段階連係データ出力
                    // ファイル設計書に沿って出力用危険段階連係データファイルに危険段階連係データ出力を出力する。
                    // ファイルの生成
                    using (FileStream fs = File.Create(filePath))
                    {
                        // 共済目的コードの確認
                        int kyosaiMokutekiCd = int.Parse(batchJouken.JoukenKyosaiMokutekiCd);
                        // ファイル書き込みの内容を変化させる
                        switch (kyosaiMokutekiCd)
                        {
                            case (int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Rikutou:
                                foreach (KikenDankaiRenkeiData data in dangerStageRenkeiData)
                                {
                                    if (data.統計単位地域コード.Equals(todofukenCd + "000"))
                                    {
                                        data.統計単位地域コード = "0";
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        // 文字コード
                        Encoding encoding = Encoding.Default;
                        switch (int.Parse(batchJouken.JoukenMojiCd))
                        {
                            case (int)Core.CoreConst.CharacterCode.UTF8:
                                encoding = Encoding.UTF8;
                                break;
                            case (int)Core.CoreConst.CharacterCode.SJIS:
                                encoding = Encoding.GetEncoding("Shift_JIS");
                                break;
                        }

                        // ファイルの書き込み
                        using (StreamWriter writer = new(fs, encoding))
                        {
                            // 1行目の出力
                            List<string> Header =
                                [
                                    "共済事業コード",
                                    "共済目的コード",
                                    "年産",
                                    "合併時識別コード",
                                    "統計単位地域コード",
                                    "組合員等コード",
                                    "類区分",
                                    "共済掛金区分",
                                    "支払共済金",
                                    "共済金額",
                                    "共済掛金標準率",
                                    "共済金額_未経過",
                                    "告示料率_未経過"
                                ];
                            // 配列の内容を書き込む
                            writer.Write(CsvUtil.GetLine(Header.ToArray()));

                            // 2行目以降の出力
                            foreach (KikenDankaiRenkeiData data in dangerStageRenkeiData)
                            {
                                string kyousaiJigyouCd = "1";
                                string shiharaiKyousaiKin = "0";
                                string kyousaiKingakuMikeika = "0";
                                string kokujiKarituMikeika = "0";
                                List<string> dataRecord =
                                    [
                                        kyousaiJigyouCd,
                                        data.共済目的コード,
                                        data.年産.ToString(),
                                        data.合併時識別コード,
                                        data.統計単位地域コード,
                                        data.組合員等コード,
                                        data.類区分,
                                        data.共済掛金区分,
                                        shiharaiKyousaiKin,
                                        data.共済金額.ToString(),
                                        data.共済掛金標準率.ToString(),
                                        kyousaiKingakuMikeika,
                                        kokujiKarituMikeika
                                    ];
                                // 配列の内容を書き込む
                                writer.Write(CsvUtil.GetLine(dataRecord.ToArray()));
                            }
                        }
                    }

                    // Zip暗号化を行う。
                    // データ一時出力フォルダ内のファイルを共通部品「ZipUtil.CreateZip」でZip化（暗号化）し
                    Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                    // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                    // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                    // [変数：ZIPファイル格納先パス] とファイル名でパスを登録します。
                    FolderUtil.MoveFile(zipFilePath, zipFolderPath, batchYoyakuId, nBid);

                    // 「６．１．」のフォルダを削除する。
                    if (Directory.Exists(tempFolderPath))
                    {
                        Directory.Delete(tempFolderPath, true);
                    }

                    // 処理正常終了時
                    // [変数：処理ステータス] に"03"（成功）を設定
                    status = NskCommon.CoreConst.STATUS_SUCCESS;

                    // [変数：エラーメッセージ] に正常終了メッセージを設定
                    // （"MI10005"：処理が正常に終了しました。)
                    errorMessage = MessageUtil.Get("MI10005");
                    logger.Info(errorMessage);


                }
            }
            catch (Exception ex)
            {
                // ７．「３．」、「４．」、「５．」でエラーとなった場合
                // [変数：ステータス]を「99：エラー」に更新する。
                status = NskCommon.CoreConst.STATUS_ERROR;

                //  [変数：エラーメッセージ] が空文字（NULL）の場合
                if (string.IsNullOrEmpty(errorMessage))
                {
                    //  [変数：エラーメッセージ] にエラーメッセージを設定
                    // （"MF00001")
                    errorMessage = MessageUtil.Get("MF00001");
                }

                // ログにエラーメッセージを出力する。
                logger.Error(errorMessage);
            }

            // ８．バッチ実行状況更新
            string refMessage = string.Empty;

            // ８．１．バッチ実行状況更新を更新する。
            // バッチ実行状況更新（BatchUtil.UpdateBatchYoyakuSts()）を呼び出し、ステータスを更新する。
            if (BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), status, errorMessage, batchYoyakuId, ref refMessage) == BatchUtil.RET_FAIL)
            {
                // （１）失敗の場合
                logger.Error(string.Format(NskCommon.CoreConst.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, refMessage));
                result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
            }
            else
            {
                // （２）成功の場合
                logger.Info(string.Format(NskCommon.CoreConst.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, refMessage));
                result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
            }

            // 処理を終了する。
            Environment.ExitCode = result;
        }

        /// <summary>
        /// 「危険段階連係データ」の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jouken">バッチ条件</param>
        /// <returns></returns>
        private static List<KikenDankaiRenkeiData> GetDangerStageRenkeiData(NskAppContext dbContext, BatchJouken jouken)
        {
            StringBuilder sql = new();

            sql.Append($"SELECT ");
            sql.Append($"   T1.共済目的コード ");
            sql.Append($",  T1.年産 ");
            sql.Append($",  T1.合併時識別コード ");
            sql.Append($",  T1.統計単位地域コード ");
            sql.Append($",  T1.組合員等コード ");
            sql.Append($",  T1.類区分 ");
            sql.Append($",  Left(T1.類区分, 2) || T1.引受方式 || T1.特約区分 || T1.補償割合コード As 共済掛金区分 ");
            sql.Append($",  T1.共済金額 ");
            sql.Append($",  T1.共済掛金標準率 ");

            sql.Append($"FROM ");
            sql.Append($"   t_12040_組合員等別引受情報 T1 ");

            sql.Append($"LEFT OUTER JOIN ");
            sql.Append($"   t_00010_引受回 T2 ");
            sql.Append($"ON ");
            sql.Append($"       T1.組合等コード = T2.組合等コード ");
            sql.Append($"   AND T1.共済目的コード = T2.共済目的コード ");
            sql.Append($"   AND T1.年産 = T2.年産 ");
            sql.Append($"   AND T1.引受回 = T2.引受回 ");

            sql.Append($"WHERE ");
            sql.Append($"       T1.類区分 > '0' ");
            sql.Append($"   AND T1.引受対象フラグ = '1' ");
            sql.Append($"   AND T1.組合等コード = @条件_組合等コード ");
            sql.Append($"   AND T1.共済目的コード = @条件_共済目的コード ");
            sql.Append($"   AND T1.年産 = @条件_年産 ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("条件_組合等コード", jouken.JoukenKumiaitoCd),
                new("条件_共済目的コード", jouken.JoukenKyosaiMokutekiCd),
                new("条件_年産", int.Parse(jouken.JoukenNensan))
            ];

            // SQLのクエリ結果をListに格納する
            List<KikenDankaiRenkeiData> dangerStageData = dbContext.Database.SqlQueryRaw<KikenDankaiRenkeiData>(sql.ToString(), parameters.ToArray()).ToList();

            return dangerStageData;
        }
    }
}
