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
using NSK_B105011.Models;

namespace NSK_B105011
{
    /// <summary>
    /// 細目データ出力処理カンマ編集データ
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
        /// 細目データ出力処理カンマ編集データ
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
            // [変数：ステータス]       STATUS_SUCCESS = "03"（成功）
            string status = NskCommon.CoreConst.STATUS_SUCCESS;
            // 処理結果（正常：0、エラー：1）
            int result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
            // バッチ予約ユーザID
            string batchYoyakuId = string.Empty;

            try
            {
                //１．設定ファイルから、以下の内容を取得し、グローバル変数へ保存する。
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
                            throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間ファイルパス"));
                        }
                    }
                }
                else
                {
                    // エラーメッセージをログに出力し、処理を中断する。
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
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME01645" 引数{0} ：パラメータの取得）
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // ３．１．２．[変数：バッチID]が数値変換不可の場合
                // 数値化したバッチID
                int nBid = 0;
                if (!int.TryParse(bid, out nBid))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    // （"ME90012"　引数{0} ：バッチID)
                    throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
                }

                // ３．１．３．[変数：都道府県コード]が未入力の場合
                if (string.IsNullOrEmpty(todofukenCd))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    // （"ME01054"　引数{0} ：都道府県コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "都道府県コード"));
                }

                //３．１．４．[変数：組合等コード] が未入力の場合
                if (string.IsNullOrEmpty(kumiaitoCd))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01054" 引数{0} ：組合等コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
                }

                //３．１．５．[変数：支所コード] が未入力の場合
                if (string.IsNullOrEmpty(shishoCd))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01054" 引数{0} ：支所コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "支所コード"));
                }

                //３．１．６．[変数：バッチ条件のキー情報] が未入力の場合
                if (string.IsNullOrEmpty(jid))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
                }

                // バッチ予約状況取得引数の設定
                BatchUtil.GetBatchYoyakuListParam param = new()
                {
                    SystemKbn = Core.CoreConst.SystemKbn.Nsk,
                    TodofukenCd = todofukenCd,
                    KumiaitoCd = kumiaitoCd,
                    ShishoCd = shishoCd,
                    BatchId = (long)nBid
                };
                // 総件数取得フラグ
                bool boolAllCntFlg = false;
                // 件数（出力パラメータ）
                int intAllCnt = 0;
                // エラーメッセージ（出力パラメータ）
                string message = string.Empty;
                // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
                List<BatchYoyaku> batchYoyakuList = BatchUtil.GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message);

                // バッチ予約が存在しない場合、
                if (batchYoyakuList.Count == 0)
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：パラメータの取得)
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // バッチ予約が存在する場合
                foreach (BatchYoyaku batchYoyaku in batchYoyakuList)
                {
                    // [引数：バッチID]に一致する場合
                    if (batchYoyaku.BatchId == nBid)
                    {
                        // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                        // バッチ予約ユーザID = バッチ予約情報.予約ユーザID
                        batchYoyakuId = batchYoyaku.BatchYoyakuId;
                    }
                    // [引数：バッチID]に一致するバッチ予約状況が取得できない場合、
                    else
                    {
                        // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                        //（"ME01645" 引数{0} ：パラメータの取得)
                        throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                    }
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
                    //５．バッチ条件を取得
                    // ５．２．バッチ条件情報のチェック
                    // ５．２．１．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータのを条件値を変数に設定する。
                    // バッチ条件情報
                    BatchJoken batchJoken = new();
                    batchJoken.GetBatchJoukens(dbContext, jid);

                    // 必須入力チェック
                    batchJoken.IsRequired();

                    // ６．コードの整合性チェック
                    batchJoken.IsConsistency(dbContext, todofukenCd, kumiaitoCd, shishoCd);

                    // ７．データ検索SQLを実行（ログ出力：あり）
                    // ７．１．対象データの取得
                    // ７．１．１．「細目データ（カンマ編集データ）」を取得する。
                    List<SaimokuDataRecord> saimokuDatas = GetSaimokuData(dbContext, todofukenCd, kumiaitoCd, batchJoken);

                    // ７．２．取得した件数が0件の場合
                    if (saimokuDatas.Count == 0)
                    {
                        //  [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME10076" 引数{0}：0)
                        throw new AppException("ME10076", MessageUtil.Get("ME10076", "0"));
                    }

                    // ７．３．取得した件数が0件以外の場合
                    // ７．３．１．ZIPファイル格納先パスを作成して変数に設定する
                    // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                    string zipFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.CSV_OUTPUT_FOLDER), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    Directory.CreateDirectory(zipFolderPath);

                    // ８．細目データ出力処理
                    // ８．１．[変数：文字コード]で指定した文字コードの出力用細目データファイル作成
                    // 一時領域にデータ一時出力フォルダとファイルを作成する
                    // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/
                    string tempFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    Directory.CreateDirectory(tempFolderPath);
                    // ファイル名：[変数：条件_ファイル名].txt
                    string fileName = batchJoken.JokenFileName + NskCommon.CoreConst.FILE_EXTENSION_TXT;
                    string filePath = Path.Combine(tempFolderPath, fileName);

                    // ファイルの生成
                    using (FileStream fs = File.Create(filePath))
                    {
                        // ８．２．変数に格納したバッチ条件に沿った細目データ出力
                        // 該当する条件の形式で出力用細目データファイルに細目データを出力する。
                        // 年産（nullの場合は空にする）
                        string jokenNensan = batchJoken.JokenNensan ?? string.Empty;

                        // バッチ条件：共済目的コード（nullの場合は空にする）
                        string jokenKyosaiMokutekitoCd = batchJoken.JokenKyosaiMokutekitoCd ?? string.Empty;

                        // 抽出区分
                        string tyushutuKubun = string.Empty;

                        // 範囲パラメータ1
                        string haniParam1 = string.Empty;

                        // 範囲パラメータ2
                        string haniParam2 = string.Empty;

                        // 範囲パラメータ3
                        string haniParam3 = string.Empty;

                        // 文字コード
                        Encoding encoding = Encoding.Default;
                        switch (int.Parse(batchJoken.JokenMojiCd))
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
                            // 範囲レコード指定
                            // [変数：バッチ条件市町村コード] が NULL 以外 の場合
                            if (!string.IsNullOrEmpty(batchJoken.JokenShichoson))
                            {
                                // [変数：バッチ条件大地区コード] が NULL 以外 の場合
                                if (!string.IsNullOrEmpty(batchJoken.JokenDaichiku))
                                {
                                    // [変数：バッチ条件小地区From] が NULL 以外 かつ [変数：バッチ条件小地区To] が NULL 以外 の場合
                                    if (!(string.IsNullOrEmpty(batchJoken.JokenShochikuStart) || string.IsNullOrEmpty(batchJoken.JokenShochikuEnd)))
                                    {
                                        // [変数：バッチ条件組合員等コードFrom] が NULL 以外 または [変数：バッチ条件組合員等コードTo] が NULL 以外 の場合
                                        if (!(string.IsNullOrEmpty(batchJoken.JokenKumiaiintoCdStart) && string.IsNullOrEmpty(batchJoken.JokenKumiaiintoCdEnd)))
                                        {
                                            // 範囲レコード(組合員等選択)
                                            tyushutuKubun = NskCommon.CoreConst.TYUSHUTU_KUBUN_KUMIAIINTO_SENTAKU;
                                            haniParam1 = batchJoken.JokenKumiaiintoCdStart;
                                            // [変数：バッチ条件組合員等コードTo] が NULL の場合、haniParam2を空にする
                                            haniParam2 = batchJoken.JokenKumiaiintoCdEnd ?? string.Empty;
                                        }
                                        // [変数：バッチ条件組合員等コードFrom] が NULL かつ [変数：バッチ条件組合員等コードTo] が NULL の場合
                                        else
                                        {
                                            // 範囲レコード(地区選択)
                                            tyushutuKubun = NskCommon.CoreConst.TYUSHUTU_KUBUN_CHIKU_SENTAKU;
                                            haniParam1 = batchJoken.JokenDaichiku;
                                            haniParam2 = batchJoken.JokenShochikuStart;
                                            haniParam3 = batchJoken.JokenShochikuEnd;
                                        }

                                    }
                                    // [変数：バッチ条件大地区コード] が NULL または [変数：バッチ条件大地区コード] が NULL の場合
                                    else
                                    {
                                        // 範囲レコード(地区選択)
                                        tyushutuKubun = NskCommon.CoreConst.TYUSHUTU_KUBUN_CHIKU_SENTAKU;
                                        haniParam1 = batchJoken.JokenDaichiku;
                                        //  [変数：バッチ条件小地区From] が NULL の場合、haniParam2を空にする
                                        haniParam2 = batchJoken.JokenShochikuStart ?? string.Empty;
                                        // haniParam2に値が設定された場合、haniParam3にも値が設定される
                                        if (!string.IsNullOrEmpty(haniParam2))
                                        {
                                            //  [変数：バッチ条件小地区To] が NULL の場合、haniParam3を空にする
                                            haniParam3 = batchJoken.JokenShochikuEnd ?? string.Empty;
                                        }
                                    }
                                }
                                // [変数：バッチ条件大地区コード] が NULL の場合
                                else
                                {
                                    // 範囲レコード(市町村選択)
                                    tyushutuKubun = NskCommon.CoreConst.TYUSHUTU_KUBUN_SHITYOSON_SENTAKU;
                                    haniParam1 = batchJoken.JokenShichoson;
                                }
                            }
                            // [変数：バッチ条件市町村コード] が NULL の場合
                            else
                            {
                                // 範囲レコード(全件)
                                tyushutuKubun = NskCommon.CoreConst.TYUSHUTU_KUBUN_ZENKEN;
                                haniParam2 = "全件";
                            }
                            // 範囲レコードの内容を配列にまとめる
                            List<string> haniRecords =
                            [
                                "範囲",
                                jokenNensan,
                                jokenKyosaiMokutekitoCd,
                                tyushutuKubun,
                                haniParam1,
                                haniParam2,
                                haniParam3,
                                DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss")
                            ];
                            // 配列の内容を書き込む
                            writer.Write(CsvUtil.GetLine(haniRecords.ToArray()));

                            // 2行目の出力
                            // 項目名の内容を配列にまとめる
                            List<string> saimokuKoumokuName =
                            [
                                "共済目的コード",
                                "組合員等コード",
                                "耕地番号",
                                "分筆番号",
                                "類区分",
                                "地名地番",
                                "耕地面積",
                                "引受面積",
                                "転作等面積",
                                "受委託区分",
                                "備考",
                                "田畑区分",
                                "区分コード",
                                "種類コード",
                                "品種コード",
                                "収量等級コード",
                                "参酌コード",
                                "基準単収",
                                "基準収穫量",
                                "修正日付",
                                "計算日付",
                                "年産",
                                "実量基準単収",
                                "統計市町村コード",
                                "統計地域コード",
                                "統計単収",
                                "麦用途区分",
                                "産地銘柄コード",
                                "受委託者コード"
                            ];
                            // 配列の内容を書き込む
                            writer.Write(CsvUtil.GetLine(saimokuKoumokuName.ToArray()));

                            // 3行目の出力
                            // 3行目の内容を配列にまとめる
                            List<string> dataStart =
                            [
                                NskCommon.CoreConst.DATA_START,
                                DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss")
                            ];
                            // 配列の内容を書き込む
                            writer.Write(CsvUtil.GetLine(dataStart.ToArray()));

                            // 4行目以降の出力
                            // 取得した細目データを読み込む
                            foreach (SaimokuDataRecord saimokuRecord in saimokuDatas)
                            {
                                List<string> saimokuDataRecord =
                                [
                                    saimokuRecord.共済目的コード,
                                    saimokuRecord.組合員等コード,
                                    saimokuRecord.耕地番号,
                                    saimokuRecord.分筆番号,
                                    saimokuRecord.類区分,
                                    saimokuRecord.地名地番,
                                    saimokuRecord.耕地面積.ToString(),
                                    saimokuRecord.引受面積.ToString(),
                                    saimokuRecord.転作等面積.ToString(),
                                    saimokuRecord.受委託区分,
                                    saimokuRecord.備考,
                                    saimokuRecord.田畑区分,
                                    saimokuRecord.区分コード,
                                    saimokuRecord.種類コード,
                                    saimokuRecord.品種コード,
                                    saimokuRecord.収量等級コード,
                                    saimokuRecord.参酌コード,
                                    saimokuRecord.基準単収.ToString(),
                                    saimokuRecord.基準収穫量.ToString(),
                                    saimokuRecord.更新日時.ToString("yyyy/MM/dd"),
                                    saimokuRecord.計算日付.ToString("yyyy/MM/dd"),
                                    saimokuRecord.年産.ToString(),
                                    saimokuRecord.実量基準単収.ToString(),
                                    saimokuRecord.統計市町村コード,
                                    saimokuRecord.統計単位地域コード,
                                    saimokuRecord.統計単収.ToString(),
                                    saimokuRecord.用途区分,
                                    saimokuRecord.産地別銘柄コード,
                                    saimokuRecord.受委託者コード
                                ];

                                // 配列の内容を書き込む
                                writer.Write(CsvUtil.GetLine(saimokuDataRecord.ToArray()));
                            }

                            // 最終行の出力
                            // 最終行の内容を配列にまとめる
                            List<string> dataEnd =
                            [
                                NskCommon.CoreConst.DATA_END,
                                DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss")
                            ];
                            // 配列の内容を書き込む
                            writer.Write(CsvUtil.GetLine(dataEnd.ToArray()));
                        }
                    }

                    // ８．３．Zip暗号化を行う。
                    // ８．３．１．データ一時出力フォルダ内のファイルを共通部品「ZipUtil.CreateZip」でZip化（暗号化）し
                    Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                    //Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                    //Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                    // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                    // [変数：ZIPファイル格納先パス] とファイル名でパスを登録します
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

                // ９．バッチ実行状況更新
                string refMessage = string.Empty;

                // ９．１．『バッチ実行状況更新』インターフェースに引数を渡す。
                // ９．２．『バッチ実行状況更新』インターフェースから戻り値を受け取る。
                if (BatchUtil.UpdateBatchYoyakuSts(nBid, status, errorMessage, batchYoyakuId, ref refMessage) == BatchUtil.RET_FAIL)
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
            }
            catch (Exception ex)
            {
                // １０．エラー処理
                // エラーメッセージをERRORログに出力する
                logger.Error(ex.Message);
                // １０．１．例外（エクセプション）の場合
                //  [変数：処理ステータス] に"99"（エラー）を設定
                status = NskCommon.CoreConst.STATUS_ERROR;

                //  [変数：エラーメッセージ] にエラーメッセージを設定
                // （"MF00001")
                errorMessage = MessageUtil.Get("MF00001");

                // １０．２．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
                string refMessage = string.Empty;
                BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), status, errorMessage, batchYoyakuId, ref refMessage);
            }

            Environment.ExitCode = result;
        }

        /// <summary>
        /// 細目データの取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コードコード</param>
        /// <param name="batchJoken">バッチ条件</param>
        /// <returns></returns>
        private static List<SaimokuDataRecord> GetSaimokuData(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, BatchJoken batchJoken)
        {
            StringBuilder sql = new();

            sql.Append($@"SELECT ");
            sql.Append($@"    T1.共済目的コード, ");
            sql.Append($@"    T1.組合員等コード, ");
            sql.Append($@"    T1.耕地番号, ");
            sql.Append($@"    T1.分筆番号, ");
            sql.Append($@"    T1.類区分,");
            sql.Append($@"    T1.地名地番, ");
            sql.Append($@"    T1.耕地面積, ");
            sql.Append($@"    T1.引受面積, ");
            sql.Append($@"    T1.転作等面積, ");
            sql.Append($@"    T1.受委託区分, ");
            sql.Append($@"    T1.備考,");
            sql.Append($@"    T1.田畑区分, ");
            sql.Append($@"    T1.区分コード, ");
            sql.Append($@"    T1.種類コード, ");
            sql.Append($@"    T1.品種コード, ");
            sql.Append($@"    T1.収量等級コード, ");
            sql.Append($@"    T1.参酌コード, ");
            sql.Append($@"    T2.基準単収, ");
            sql.Append($@"    T2.基準収穫量, ");
            sql.Append($@"    T2.更新日時, ");
            sql.Append($@"    T2.計算日付, ");
            sql.Append($@"    T1.年産, ");
            sql.Append($@"    T1.実量基準単収, ");
            sql.Append($@"    T1.統計市町村コード, ");
            sql.Append($@"    T1.統計単位地域コード, ");
            sql.Append($@"    T1.統計単収, ");
            sql.Append($@"    T1.用途区分,");
            sql.Append($@"    T1.産地別銘柄コード, ");
            sql.Append($@"    T1.受委託者コード  ");
            sql.Append($@"FROM ");
            sql.Append($@"    t_11090_引受耕地 T1 ");
            sql.Append($@"    INNER JOIN t_12010_引受結果 T2 ");
            sql.Append($@"        ON T2.組合等コード = T1.組合等コード ");
            sql.Append($@"        AND T2.年産 = T1.年産 ");
            sql.Append($@"        AND T2.共済目的コード = T1.共済目的コード ");
            sql.Append($@"        AND T2.組合員等コード = T1.組合員等コード ");
            sql.Append($@"        AND T2.耕地番号 = T1.耕地番号 ");
            sql.Append($@"        AND T2.分筆番号 = T1.分筆番号 ");
            sql.Append($@"    INNER JOIN v_nogyosha T3 ");
            sql.Append($@"        ON T3.kumiaiinto_cd = T1.組合員等コード ");
            sql.Append($@"WHERE ");
            sql.Append($@"    1 = 1 ");
            sql.Append($@"    AND T1.組合等コード = @パッチ条件組合等コード ");
            sql.Append($@"    AND T1.年産 = @バッチ条件年産 ");
            sql.Append($@"    AND T1.共済目的コード = @バッチ条件共済目的コード ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件組合員等コードFrom <> '' THEN T1.組合員等コード >= @バッチ条件組合員等コードFrom ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件組合員等コードTo <> '' THEN T1.組合員等コード <= @バッチ条件組合員等コードTo ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件大地区コード <> '' THEN T3.daichiku_cd = @バッチ条件大地区コード ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件小地区From <> '' THEN T3.shochiku_cd >= @バッチ条件小地区From ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件小地区To <> '' THEN T3.shochiku_cd <= @バッチ条件小地区To ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件市町村コード <> '' THEN T3.shichoson_cd = @バッチ条件市町村コード ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件支所コード <> '00' THEN T3.shisho_cd = @バッチ条件支所コード ");
            sql.Append($@"        ELSE T3.shisho_cd IN ( ");
                sql.Append($@"            SELECT shisho_cd ");
                sql.Append($@"            FROM v_shisho_nm ");
                sql.Append($@"            WHERE todofuken_cd = @都道府県コード ");
                sql.Append($@"            AND kumiaito_cd = @組合等コード ");
                sql.Append($@"            AND shisho_cd <> '00') ");
            sql.Append($@"    END ");

            if (!(string.IsNullOrEmpty(batchJoken.JokenShuturyokujun1) && string.IsNullOrEmpty(batchJoken.JokenShuturyokujun2) && string.IsNullOrEmpty(batchJoken.JokenShuturyokujun3)))
            {
                // 初回の判定
                bool isFirst = true;
                // ソート順と出力順のリスト化
                List<SortOrder> sortOrders =
                [
                    new() { OrderByKey = batchJoken.JokenShuturyokujun1, OrderBy = batchJoken.JokenShojunKojun1 },
                    new() { OrderByKey = batchJoken.JokenShuturyokujun2, OrderBy = batchJoken.JokenShojunKojun2 },
                    new() { OrderByKey = batchJoken.JokenShuturyokujun3, OrderBy = batchJoken.JokenShojunKojun3 }
                ];

                sql.Append($"ORDER BY ");

                foreach (SortOrder sort in sortOrders)
                {
                    if (!string.IsNullOrEmpty(sort.OrderByKey))
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            sql.Append($"   , ");
                        }

                        if (sort.OrderByKey.Equals(batchJoken.JokenShuturyokujun1))
                        {
                            sql.Append($"   @出力順1 ");
                        }
                        else if (sort.OrderByKey.Equals(batchJoken.JokenShuturyokujun2))
                        {
                            sql.Append($"   @出力順2 ");
                        }
                        else if (sort.OrderByKey.Equals(batchJoken.JokenShuturyokujun3))
                        {
                            sql.Append($"   @出力順3 ");
                        }

                        switch (int.Parse(sort.OrderBy))
                        {
                            case (int)Core.CoreConst.SortOrder.ASC:
                                sql.Append($"   ASC ");
                                break;
                            case (int)Core.CoreConst.SortOrder.DESC:
                                sql.Append($"   DESC ");
                                break;
                        }
                    }
                }
            }

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("バッチ条件年産", int.Parse(batchJoken.JokenNensan)),
                new("バッチ条件共済目的コード", batchJoken.JokenKyosaiMokutekitoCd),
                new("パッチ条件組合等コード", batchJoken.JokenKumiaitoCd),
                new("バッチ条件組合員等コードFrom", batchJoken.JokenKumiaiintoCdStart),
                new("バッチ条件組合員等コードTo", batchJoken.JokenKumiaiintoCdEnd),
                new("バッチ条件大地区コード", batchJoken.JokenDaichiku),
                new("バッチ条件小地区From", batchJoken.JokenShochikuStart),
                new("バッチ条件小地区To", batchJoken.JokenShochikuEnd),
                new("バッチ条件市町村コード", batchJoken.JokenShichoson),
                new("バッチ条件支所コード", batchJoken.JokenShisho),
                new("都道府県コード", todofukenCd),
                new("組合等コード", kumiaitoCd),
                new("出力順1", batchJoken.JokenShuturyokujun1),
                new("出力順2", batchJoken.JokenShuturyokujun2),
                new("出力順3", batchJoken.JokenShuturyokujun3)
            ];

            // SQLのクエリ結果をListに格納する
            List<SaimokuDataRecord> saimokuData = dbContext.Database.SqlQueryRaw<SaimokuDataRecord>(sql.ToString(), parameters.ToArray()).ToList();

            return saimokuData;
        }
    }
}
