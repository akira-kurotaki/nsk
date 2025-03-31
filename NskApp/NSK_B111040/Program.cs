using System.Text;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NLog;
using NSK_B111040.Models;
using NskAppModelLibrary.Context;
using static CoreLibrary.Core.Utility.BatchUtil;
using Core = CoreLibrary.Core.Consts;
using NskCommon = NskCommonLibrary.Core.Consts;

namespace NSK_B111040
{
    /// <summary>
    /// 交付金申請書データ作成連合会交付金申請合計部
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
            // SJIS(Shift_JIS)を使用可能にする
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 交付金申請書データ作成連合会交付金申請合計部
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
            string status = BATCH_STATUS_SUCCESS;
            // 処理結果（正常：0、エラー：1）
            int result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
            // バッチ予約ユーザID
            string batchYoyakuId = string.Empty;
            // 数値化したバッチID
            int nBid = 0;

            try
            {
                //１．設定ファイルから、以下の内容を取得し、グローバル変数へ保存する。

                // DBデフォルトスキーマ  （検索キー：DefaultSchema）
                // DB接続時にDefaultSchemaに接続されるため１．では取得なし

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
                            errorMessage = MessageUtil.Get("ME90015", "システム時間ファイルパス");
                            throw new AppException("ME90015", errorMessage);
                        }
                    }
                }
                else
                {
                    // エラーメッセージをログに出力し、処理を中断する。
                    // （"ME90015"、{0}：システム時間フラグ）
                    errorMessage = MessageUtil.Get("ME90015", "システム時間フラグ");
                    throw new AppException("ME90015", errorMessage);
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

                // ３．パラメータ情報のチェック
                // ３．１．必須チェック
                // ３．１．１．以下の引数が未入力、または[引数：バッチID]が数値変換不可の場合
                // [引数：バッチID]
                // [引数：都道府県コード]
                // [引数：組合等コード]
                // [引数：支所コード]
                // [引数：バッチ条件のキー情報]
                if (string.IsNullOrEmpty(bid) || string.IsNullOrEmpty(todofukenCd) || string.IsNullOrEmpty(kumiaitoCd)
                    || string.IsNullOrEmpty(shishoCd) || string.IsNullOrEmpty(jid))
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME01645" 引数{0} ：パラメータの取得）
                    errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                    throw new AppException("ME01645", errorMessage);
                }
                else if (!int.TryParse(bid, out nBid))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    // （"ME90012"　引数{0} ：バッチID)
                    errorMessage = MessageUtil.Get("ME90012", "バッチID");
                    throw new AppException("ME90012", errorMessage);
                }

                // ４．DB接続
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

                // ５．バッチ条件を取得
                // バッチ予約状況取得引数の設定
                GetBatchYoyakuListParam param = new()
                {
                    SystemKbn = Core.CoreConst.SystemKbn.Nsk,
                    TodofukenCd = todofukenCd,
                    KumiaitoCd = kumiaitoCd,
                    ShishoCd = shishoCd,
                    BatchId = nBid,
                    CntStart = 0,
                    CntEnd = 0
                };

                var intAllCnt = 0;
                string? message = null;
                bool boolAllCntFlg = false;

                // ５．１．バッチ予約状況取得
                // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
                BatchYoyaku? batchYoyakuList = GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message).FirstOrDefault();

                // ５．１．１．[引数：バッチID]に一致するバッチ予約状況が取得できない場合、[変数：エラーメッセージ]を設定し、「１０．」へ進む。
                if (batchYoyakuList is not null && intAllCnt != 0)
                {

                    // ５．１．２．取得した「バッチ予約状況」から値を取得し変数に設定する。
                    if (batchYoyakuList.BatchYoyakuId != string.Empty)
                    {
                        batchYoyakuId = batchYoyakuList.BatchYoyakuId;
                    }
                    else
                    {
                        // エラーメッセージをログに出力する。
                        // （"ME01645"、{0}：パラメータの取得）
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }
                }
                else
                {
                    // エラーメッセージをログに出力する。
                    // （"ME01645"、{0}：パラメータの取得）
                    errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                    throw new AppException("ME01645", errorMessage);
                }

                using (NskAppContext dbContext = new(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt("CommandTimeout")))
                {

                    BatchJoken batchJoken = new();

                    // ５．２．バッチ条件情報の取得
                    // 条件値のリスト
                    batchJoken.GetBatchJoken(dbContext, jid);

                    // ５．３．２．[変数：条件_組合等コード]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenKumiaitoCd))
                    {
                        // [変数：エラーメッセージ]を設定し、「１０．」へ進む。
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }

                    // ５．３．３．[変数：条件_年産]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenNensan))
                    {
                        // [変数：エラーメッセージ]を設定し、「１０．」へ進む。
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }

                    // ５．３．４．[変数：条件_負担金交付区分]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenFutankinKofuKbn))
                    {
                        // [変数：エラーメッセージ]を設定し、「１０．」へ進む。
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }

                    // ５．３．５．[変数：条件_交付回]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenKoufuKai))
                    {
                        // [変数：エラーメッセージ]を設定し、「１０．」へ進む。
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }

                    // ５．３．６．[変数：条件_文字コード]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenMojiCd))
                    {
                        // [変数：エラーメッセージ]を設定し、「１０．」へ進む。
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }


                    // ６．コードの整合性チェック

                    // [変数：都道府県コード]
                    // ６．１．「都道府県コード存在情報」を取得する。
                    // ６．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (BatchJoken.GetTodofukenCdSonzaiJoho(dbContext, todofukenCd) == 0)
                    {
                        // マスタなし
                        // [変数：エラーメッセージ]を設定し、「１０.」へ進む。
                        errorMessage = MessageUtil.Get("ME10005", "都道府県コード");
                        throw new AppException("ME10005", errorMessage);
                    }

                    // [変数：組合等コード]
                    // ６．３．[変数：組合等コード]が入力されている場合、「組合等コード存在情報」を取得する。
                    // ６．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (BatchJoken.GetKumiaitoCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd) == 0)
                    {
                        // マスタなし
                        // [変数：エラーメッセージ]を設定し、「１０.」へ進む。
                        errorMessage = MessageUtil.Get("ME10005", "組合等コード");
                        throw new AppException("ME10005", errorMessage);
                    }

                    // [変数：支所コード]
                    // ６．５．[変数：支所コード]が入力されている場合、「支所コード存在情報」を取得する。
                    // ６．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (BatchJoken.GetShishoCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, shishoCd) == 0)
                    {
                        // マスタなし
                        // [変数：エラーメッセージ]を設定し、「１０.」へ進む。
                        errorMessage = MessageUtil.Get("ME10005", "支所コード");
                        throw new AppException("ME10005", errorMessage);
                    }

                    // [変数：条件_組合等コード]
                    // ６．７．[変数：条件_組合等コード]が入力されている場合、「組合等コード存在情報」を取得する。
                    // ６．８．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (BatchJoken.GetKumiaitoCdSonzaiJoho(dbContext, todofukenCd, batchJoken.JokenKumiaitoCd) == 0)
                    {
                        // マスタなし
                        // [変数：エラーメッセージ]を設定し、「１０.」へ進む。
                        errorMessage = MessageUtil.Get("ME10005", "条件_組合等コード");
                        throw new AppException("ME10005", errorMessage);
                    }


                    // ７．データ検索SQLを実行（ログ出力：あり）
                    // ７．１．対象データの取得
                    // ７．１．１．「交付金申請書データ」を取得する。
                    List<KoufuKinShinseiRecord> koufuKinShinsei = BatchJoken.GetKoufuKinShinsei(dbContext, todofukenCd, kumiaitoCd, batchJoken);

                    // ７．２．取得した件数が0件の場合
                    if (koufuKinShinsei.Count == 0)
                    {
                        //   [変数：エラーメッセージ] に以下のメッセージを設定し、「１０．」へ進む。
                        errorMessage = MessageUtil.Get("ME10076", "０");
                        throw new AppException("ME10076", errorMessage);
                    }

                    // ７．３．取得した件数が0件以外の場合
                    // ７．３．１．ZIPファイル格納先パスを作成して変数に設定する
                    // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                    string zipFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.CSV_OUTPUT_FOLDER), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    // ７．３．２．作成したZIPファイル格納先パスにZIPファイル格納フォルダを作成する
                    Directory.CreateDirectory(zipFolderPath);

                    // ８．連合会交付金申請合計部データ出力処理
                    // ８．１．[変数：条件_文字コード]で指定した文字コードの連合会交付金申請合計部データファイル作成
                    // 一時領域にデータ一時出力フォルダとファイルを作成する
                    // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/ 
                    string tempFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    Directory.CreateDirectory(tempFolderPath);
                    // ファイル名："340" & [変数：条件_負担金交付区分] & "01" & [変数：条件_組合等コード] & ".TXT"
                    string fileName = "340X01YY".Replace("X", batchJoken.JokenFutankinKofuKbn).Replace("YY", batchJoken.JokenKumiaitoCd) + NskCommon.CoreConst.FILE_EXTENSION_TXT;
                    string filePath = Path.Combine(tempFolderPath, fileName);

                    // ８．２．取得した連合会交付金申請合計部データを連合会交付金申請合計部データファイルにカンマ区切りで出力
                    // ファイル設計書に沿って連合会交付金申請合計部ファイルに連合会交付金申請合計部データを出力する。
                    // この時、ファイルの文字コードとして[変数：条件_文字コード]に該当する文字エンコーディングを使用する。
                    // ファイルの生成
                    using (FileStream fs = File.Create(filePath))
                    {

                        // 文字コード
                        Encoding encoding = Encoding.Default;
                        if (Convert.ToInt32(Core.CoreConst.CharacterCode.UTF8).ToString().Equals(batchJoken.JokenMojiCd))
                        {
                            encoding = Encoding.UTF8;
                        }
                        else if (Convert.ToInt32(Core.CoreConst.CharacterCode.SJIS).ToString().Equals(batchJoken.JokenMojiCd))
                        {
                            encoding = Encoding.GetEncoding("Shift_JIS");
                        }


                        // ファイルの書き込み
                        using (StreamWriter writer = new(fs, encoding))
                        {

                            // ヘッダ書き込みフラグ初期化
                            bool headerFlg = true;

                            // 取得した交付金申請書データを読み込む
                            foreach (KoufuKinShinseiRecord koufuKinShinseiRecord in koufuKinShinsei)
                            {

                                // ヘッダ部（1行目）の出力
                                // 1レコード目の場合、ヘッダ部を出力
                                if (headerFlg)
                                {

                                    // ヘッダ部の内容を配列にまとめる
                                    List<string> haniRecords =
                                    [
                                        // システム名略称
                                        "NSKR",
                                        // データ名称
                                        "連合会交付金申請合計部",
                                        // 連合会略称
                                        koufuKinShinseiRecord.連合会略称,
                                        // 省庁名
                                        "農林水産省",
                                        // 日時
                                        systemDate.ToString("yyyy/MM/dd HH:mm:ss"),
                                        // 件数
                                        $"{koufuKinShinsei.Count}",
                                        // 負担金交付区分
                                        koufuKinShinseiRecord.負担金交付区分.ToString(),
                                        // 類区分
                                        "0",
                                        // 連合会コード
                                        todofukenCd
                                    ];
                                    // 配列の内容を書き込む
                                    writer.Write(CsvUtil.GetLine(haniRecords.ToArray()));

                                    // 2レコード以降にヘッダを出力しないようフラグ更新
                                    headerFlg = false;
                                }

                                // データ部(2行目以降)の出力
                                // データ部の内容を配列にまとめる
                                List<string> koufuKinShinseiDataRecord =
                                [
                                    // 負担金交付区分
                                    koufuKinShinseiRecord.負担金交付区分,
                                    // 連合会番号
                                    todofukenCd,
                                    // 交付回
                                    koufuKinShinseiRecord.交付回.ToString(),
                                    // 組合等交付金の金額
                                    koufuKinShinseiRecord.組合等交付金の金額.ToString(),
                                    // 既受領交付金の金額
                                    koufuKinShinseiRecord.既受領交付金の金額.ToString(),
                                    // 今回交付申請額
                                    koufuKinShinseiRecord.今回交付申請額.ToString(),
                                    // 年産
                                    koufuKinShinseiRecord.年産.ToString()
                                ];

                                // 配列の内容を書き込む
                                writer.Write(CsvUtil.GetLine(koufuKinShinseiDataRecord.ToArray()));
                            }
                        }
                    }

                    // ８．３．Zip暗号化を行う。
                    // ８．３．１．「８．１．」のフォルダ内のテキストをZip化（暗号化）し、
                        Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                    // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                    // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                    // [変数：ZIPファイル格納先パス]とファイル名でパスを登録します。
                    NskCommonLibrary.Core.Utility.FolderUtil.MoveFile(zipFilePath, zipFolderPath, bid, nBid);

                    // ８．３．２．「８．１．」のフォルダを削除する。
                    if (Directory.Exists(tempFolderPath))
                    {
                        Directory.Delete(tempFolderPath, true);
                    }

                    // ８．４．処理正常終了時
                    // [変数：処理ステータス] に"03"（成功）を設定
                    status = BATCH_STATUS_SUCCESS;

                    // [変数：エラーメッセージ] に正常終了メッセージを設定
                    // （"MI10005"：処理が正常に終了しました。)
                    errorMessage = MessageUtil.Get("MI10005");
                }

            }
            catch (AppException ex)
            {

                // １０．エラー処理
                // １０．１．例外（エクセプション）の場合
                // エラーメッセージをERRORログに出力する
                // [変数：ステータス]を「99：エラー」に更新する。
                status = BATCH_STATUS_ERROR;

                if (string.IsNullOrEmpty(ex.Message))
                {
                    //  [変数：エラーメッセージ] にエラーメッセージを設定
                    // （"MF00001")
                    errorMessage = MessageUtil.Get("MF00001");
                }
                else
                {
                    errorMessage = ex.Message;
                }

                // ログにエラーメッセージを出力する。
                logger.Error(ex, errorMessage);

            }
            catch (Exception ex)
            {

                // １０．エラー処理
                // １０．１．例外（エクセプション）の場合
                // エラーメッセージをERRORログに出力する
                // [変数：ステータス]を「99：エラー」に更新する。
                status = BATCH_STATUS_ERROR;
                //  [変数：エラーメッセージ] にエラーメッセージを設定
                // （"MF00001")
                errorMessage = MessageUtil.Get("MF00001");

                // ログにエラーメッセージを出力する。
                logger.Error(ex, errorMessage);

            }

            // ９．バッチ実行状況更新
            string refMessage = string.Empty;

            // ９．１．『バッチ実行状況更新』インターフェースに引数を渡す。
            // バッチ実行状況更新（BatchUtil.UpdateBatchYoyakuSts()）を呼び出し、ステータスを更新する。
            // ９．２．『バッチ実行状況更新』インターフェースから戻り値を受け取る。
            if (UpdateBatchYoyakuSts(nBid, status, errorMessage, batchYoyakuId, ref refMessage) == RET_FAIL)
            {
                // バッチ実行状況更新結果 = 失敗
                // ERRORログ出力
                logger.Error(refMessage);
                logger.Error(string.Format(NskCommon.CoreConst.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, errorMessage));
                result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
            }
            else
            {
                // バッチ実行状況更新結果 = 成功
                // INFOログ出力
                logger.Info(string.Format(NskCommon.CoreConst.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, errorMessage));
                result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
            }

            Environment.ExitCode = result;
        }

    }

}
