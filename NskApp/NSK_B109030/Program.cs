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
using NSK_B109030.Models;

namespace NSK_B109030
{
    /// <summary>
    /// 規模別分布状況データ作成収量建て
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
        /// 規模別分布状況データ作成収量建て
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
                                // 以下のエラーメッセージをログに出力し、「９.」へ進む。
                                // （"ME90015"、{0}：システム時間ファイルパス）
                                throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間ファイルパス"));
                            }
                        }
                    }
                    else
                    {
                        // 以下のエラーメッセージをログに出力し、「９.」へ進む。
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

                    // ３．引数のチェック
                    // ３．１．必須チェック
                    // ３．１．１．[変数：バッチID] が未入力の場合
                    if (string.IsNullOrEmpty(bid))
                    {
                        // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「９.」へ進む。
                        // （"ME01054" 引数{0} ：バッチID）
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチID"));
                    }

                    // ３．１．２．[変数：バッチID]が数値変換不可の場合
                    // 数値化したバッチID
                    if (!int.TryParse(bid, out int nBid))
                    {
                        // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「９．」へ進む。
                        // （"ME90012"　引数{0} ：バッチID)
                        throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
                    }

                    // ３．１．３．[変数：都道府県コード]が未入力の場合
                    if (string.IsNullOrEmpty(todofukenCd))
                    {
                        // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「９．」へ進む。
                        // （"ME01054"　引数{0} ：都道府県コード)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "都道府県コード"));
                    }

                    //３．１．４．[変数：組合等コード] が未入力の場合
                    if (string.IsNullOrEmpty(kumiaitoCd))
                    {
                        //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「９．」へ進む。
                        //（"ME01054" 引数{0} ：組合等コード)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
                    }

                    //３．１．５．[変数：支所コード] が未入力の場合
                    if (string.IsNullOrEmpty(shishoCd))
                    {
                        //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「９．」へ進む。
                        //（"ME01054" 引数{0} ：支所コード)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "支所コード"));
                    }

                    //３．１．６．[変数：バッチ条件のキー情報] が未入力の場合
                    if (string.IsNullOrEmpty(jid))
                    {
                        //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「９．」へ進む。
                        //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
                    }

                    // バッチ予約状況取得
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
                    List<BatchYoyaku> batchYoyakus = BatchUtil.GetBatchYoyakuList(batchYoyakuListparam, boolAllCntFlg, ref intAllCnt, ref message);

                    // バッチ予約が存在しない場合、
                    if (batchYoyakus.Count == 0)
                    {
                        // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「９．」へ進む。
                        //（"ME01645" 引数{0} ：パラメータの取得)
                        throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                    }

                    // バッチ予約が存在する場合
                    BatchYoyaku? batchYoyaku = batchYoyakus.FirstOrDefault(x => x.BatchId == nBid);
                    // [引数：バッチID]に一致する場合
                    if (batchYoyaku is not null)
                    {
                        // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                        // バッチ予約ユーザID = バッチ予約情報.予約ユーザID
                        batchYoyakuId = batchYoyaku.BatchYoyakuId;
                    }
                    // [引数：バッチID]に一致するバッチ予約状況が取得できない場合、
                    else
                    {
                        // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「９．」へ進む。
                        //（"ME01645" 引数{0} ：パラメータの取得)
                        throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                    }

                    // ４．DB接続
                    // ※「共通機能設計_070_DB切替」の「バッチのDB接続先取得処理」を参照。
                    // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                    // DB接続情報
                    DbConnectionInfo? dbInfo = null;
                    dbInfo = DBUtil.GetDbConnectionInfo(Core.CoreConst.SystemKbn.Nsk, todofukenCd, kumiaitoCd, shishoCd);
                    if (dbInfo == null)
                    {
                        throw new AppException("ME90014", MessageUtil.Get("ME90014"));
                    }

                    using (NskAppContext dbContext = new(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt(Core.CoreConst.COMMAND_TIMEOUT)))
                    {
                        // ５．バッチ条件を取得
                        // ５．１．バッチ条件情報の取得
                        // バッチ条件情報
                        BatchJouken batchJouken = new();
                        batchJouken.GetBatchJoukens(dbContext, jid);

                        // ６．コードの整合性チェック
                        batchJouken.IsConsistency(dbContext, todofukenCd, kumiaitoCd, shishoCd);

                        // ７．データ検索SQLを実行（ログ出力：あり）
                        // ７．１．「規模別分布」を取得
                        // ７．１．１．「規模別分布」を取得する。
                        List<KibobetuBunpuData> kibobetuBunpuDatas = GetKibobetuBunpuData(dbContext, batchJouken, todofukenCd);

                        // ７．２．対象データの件数を確認
                        // ７．２．１．取得した件数が0件の場合
                        if (kibobetuBunpuDatas.Count == 0)
                        {
                            // [変数：エラーメッセージ]に、以下のエラーメッセージを設定し、「９.」へ進む。
                            // （"ME10076" 引数{0}：0)
                            throw new AppException("ME10076", MessageUtil.Get("ME10076", "0"));
                        }

                        // ７．２．２．取得した件数が0件以外の場合
                        // ７．２．２．１．ZIPファイル格納先パスを作成して変数に設定する
                        // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                        string zipFolderPath = Path.Combine(
                            ConfigUtil.Get(NskCommon.CoreConst.CSV_OUTPUT_FOLDER),
                                bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                        // ７．２．２．２．作成したZIPファイル格納先パスにZIPファイル格納フォルダを作成する
                        Directory.CreateDirectory(zipFolderPath);

                        // ８．規模別分布状況データ出力処理
                        // ８．１．[変数：文字コード]で指定した文字コードの出力用規模別分布状況データファイル作成
                        // 一時領域にデータ一時出力フォルダとファイルを作成する
                        // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/
                        string tempFolderPath = Path.Combine(
                            ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH),
                                bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                        Directory.CreateDirectory(tempFolderPath);
                        // ファイル名：11XX01YY.TXT（XX：共済目的コード、YY：連合会番号）
                        string fileName = "11" + batchJouken.JoukenKyosaiMokutekiCd + "01" + todofukenCd + Core.CoreConst.SYMBOL_DOT + "TXT";
                        string filePath = Path.Combine(tempFolderPath, fileName);

                        // 文字コード
                        // 使用する文字コードを変化させる
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

                        // いずれかの共済目的コードである場合、出力可能（初期値：true）
                        // 11（水稲） or 20（陸稲） or 30（麦）
                        bool isCanWrite = true;
                        switch (int.Parse(batchJouken.JoukenKyosaiMokutekiCd))
                        {
                            case (int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Suitou:
                                break;
                            case (int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Rikutou:
                                break;
                            case (int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Mugi:
                                break;
                            default:
                                isCanWrite = false;
                                break;
                        }

                        // ８．２．[配列：バッチ条件]の抽出区分に沿った規模別分布状況データ出力
                        // 該当する条件の形式で出力用規模別分布状況データファイルに規模別分布状況データを出力する。
                        // ファイルの生成
                        if (isCanWrite)
                        {
                            using (FileStream fs = File.Create(filePath))
                            {
                                // ファイルの書き込み
                                using (StreamWriter writer = new(fs, encoding))
                                {
                                    // 組合等略称
                                    string kumiaitoRyakushou = string.Empty;
                                    // 3行目以降の出力内容
                                    List<List<string>> dataRecords = new();

                                    // 3行目以降の内容
                                    foreach (KibobetuBunpuData data in kibobetuBunpuDatas)
                                    {
                                        kumiaitoRyakushou = data.組合等略称;
                                        List<string> dataRecord =
                                        [
                                            data.共済目的コード,
                                            data.類区分,
                                            todofukenCd,
                                            data.組合等コード,
                                            data.合併特例有無フラグ,
                                            data.合併時識別コード,
                                            data.大地区コード,
                                            data.引受面積区分.ToString(),
                                            data.引受戸数.ToString(),
                                            data.引受面積.ToString(),
                                            data.年産.ToString()
                                        ];
                                        dataRecords.Add(dataRecord);
                                    }

                                    // 1行目の内容
                                    string ruiKbn = string.Empty;
                                    List<string> header =
                                    [
                                        "NSK",
                                        "規模別分布状況",
                                        kumiaitoRyakushou,
                                        "農林水産省",
                                        DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss"),
                                        kibobetuBunpuDatas.Count.ToString(),
                                        batchJouken.JoukenKyosaiMokutekiCd,
                                        "0",
                                        todofukenCd
                                    ];

                                    // 2行目の内容
                                    List<string> dataStartRowValues =
                                    [
                                        NskCommon.CoreConst.DATA_START,
                                        DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss")
                                    ];

                                    // 最終行の内容
                                    List<string> dataEndRowValues =
                                    [
                                        NskCommon.CoreConst.DATA_END,
                                        DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss")
                                    ];

                                    // 配列の内容を出力する
                                    writer.Write(CsvUtil.GetLine(header.ToArray()));
                                    writer.Write(CsvUtil.GetLine(dataStartRowValues.ToArray()));
                                    foreach (List<string> dataRecord in dataRecords)
                                    {
                                        writer.Write(CsvUtil.GetLine(dataRecord.ToArray()));
                                    }
                                    writer.Write(CsvUtil.GetLine(dataEndRowValues.ToArray()));
                                }
                            }
                        }

                        // ８．３．Zip暗号化を行う。
                        // ８．３．１．データ一時出力フォルダ内のファイルを共通部品「ZipUtil.CreateZip」でZip化（暗号化）し
                        Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                        // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                        // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                        // [変数：ZIPファイル格納先パス] とファイル名でパスを登録します。
                        FolderUtil.MoveFile(zipFilePath, zipFolderPath, batchYoyakuId, nBid);

                        // ８．３．２．「８．１．」のフォルダを削除する。
                        if (Directory.Exists(tempFolderPath))
                        {
                            Directory.Delete(tempFolderPath, true);
                        }

                        // ８．４．処理正常終了時
                        // [変数：処理ステータス] に"03"（成功）を設定
                        status = NskCommon.CoreConst.STATUS_SUCCESS;

                        // [変数：エラーメッセージ] に正常終了メッセージを設定
                        // （"MI10005"：処理が正常に終了しました。)
                        errorMessage = MessageUtil.Get("MI10005");
                        logger.Info(errorMessage);
                    }
                }
                catch (AppException aEx)
                {
                    // 処理異常終了時
                    // [変数：処理ステータス] に"99"（エラー）を設定
                    status = NskCommon.CoreConst.STATUS_ERROR;

                    // [変数：エラーメッセージ] にエラーメッセージを設定
                    errorMessage = aEx.Message;
                    // エラーメッセージをERRORログに出力する
                    logger.Error(errorMessage);
                }

                // ９．バッチ実行状況更新
                // ９．１．バッチ実行状況更新を更新する。
                // バッチ実行状況更新（BatchUtil.UpdateBatchYoyakuSts()）を呼び出し、ステータスを更新する。
                string refMessage = string.Empty;
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

                // ９．３．処理を終了する。
                Environment.ExitCode = result;
            }
            catch (Exception ex)
            {
                // １０．エラー処理
                // １０．１．例外（エクセプション）の場合
                // [変数：ステータス]を「99：エラー」に更新する。
                status = NskCommon.CoreConst.STATUS_ERROR;

                // [変数：エラーメッセージ] にエラーメッセージを設定
                // （"MF00001"：予期せぬエラーが発生しました。システム管理者に連絡してください。)
                errorMessage = MessageUtil.Get("MF00001");

                // １０．２．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
                string refMessage = string.Empty;
                BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), status, errorMessage, batchYoyakuId, ref refMessage);
            }
        }

        /// <summary>
        /// 「規模別分布」の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="batchJouken">バッチ条件</param>
        /// <returns></returns>
        private static List<KibobetuBunpuData> GetKibobetuBunpuData(NskAppContext dbContext, BatchJouken batchJouken, string todofukenCd)
        {
            StringBuilder sql = new();

            sql.Append($"SELECT ");
            sql.Append($"    T1.組合等コード ");
            sql.Append($"    , T1.年産 ");
            sql.Append($"    , T1.共済目的コード ");
            sql.Append($"    , T1.類区分 ");
            sql.Append($"    , T1.合併時識別コード ");
            sql.Append($"    , T1.大地区コード ");
            sql.Append($"    , T1.引受面積区分 ");
            sql.Append($"    , T1.合併特例有無フラグ ");
            sql.Append($"    , T1.引受戸数 ");
            sql.Append($"    , T1.引受面積 ");
            sql.Append($"    , T2.kumiaito_rnm AS 組合等略称 ");
            sql.Append($"FROM ");
            sql.Append($"    t_14010_規模別分布 T1 ");
            sql.Append($"    INNER JOIN v_kumiaito T2 ");
            sql.Append($"        ON T2.kumiaito_cd = T1.組合等コード ");
            sql.Append($"        AND T2.todofuken_cd = @都道府県コード ");
            sql.Append($"WHERE ");
            sql.Append($"    T1.組合等コード = @条件_組合等コード ");
            sql.Append($"    AND T1.年産 = @条件_年産 ");
            sql.Append($"    AND T1.共済目的コード = @条件_共済目的コード ");
            sql.Append($"    AND CASE ");
            sql.Append($"        WHEN @条件_類区分 <> '' ");
            sql.Append($"            THEN T1.類区分 = @条件_類区分 ");
            sql.Append($"            ELSE 1 = 1 ");
            sql.Append($"        END ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("都道府県コード", todofukenCd),
                new("条件_組合等コード", batchJouken.JoukenKumiaitoCd),
                new("条件_年産", int.Parse(batchJouken.JoukenNensan)),
                new("条件_共済目的コード", batchJouken.JoukenKyosaiMokutekiCd),
                new("条件_類区分", batchJouken.JoukenRuiKbn)
            ];

            if (!string.IsNullOrEmpty(batchJouken.JoukenOrderByKey1))
            {
                sql.Append($"ORDER BY @条件_出力順キー1 ");
                parameters.Add(new("条件_出力順キー1", batchJouken.JoukenOrderByKey1));

                switch (int.Parse(batchJouken.JoukenOrderBy1))
                {
                    case (int)Core.CoreConst.SortOrder.DESC:
                        sql.Append($"    DESC ");
                        break;
                    case (int)Core.CoreConst.SortOrder.ASC:
                        sql.Append($"    ASC ");
                        break;
                }
            }

            // SQLのクエリ結果をListに格納する
            List<KibobetuBunpuData> datas = dbContext.Database.SqlQueryRaw<KibobetuBunpuData>(sql.ToString(), parameters.ToArray()).ToList();

            return datas;
        }
    }
}
