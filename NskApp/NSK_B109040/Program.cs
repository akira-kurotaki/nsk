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
using NSK_B109040.Models;

namespace NSK_B109040
{
    /// <summary>
    /// 規模別分布状況データ作成（内部参照用）
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
        /// 規模別分布状況データ作成（内部参照用）
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
                                // 以下のエラーメッセージをログに出力し、「１０.」へ進む。
                                // （"ME90015"、{0}：システム時間ファイルパス）
                                throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間ファイルパス"));
                            }
                        }
                    }
                    else
                    {
                        // 以下のエラーメッセージをログに出力し、「１０.」へ進む。
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
                        // （"ME01054" 引数{0} ：バッチID）
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチID"));
                    }

                    // ３．１．２．[変数：バッチID]が数値変換不可の場合
                    // 数値化したバッチID
                    if (!int.TryParse(bid, out int nBid))
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
                    List<BatchYoyaku> batchYoyakuList = BatchUtil.GetBatchYoyakuList(batchYoyakuListparam, boolAllCntFlg, ref intAllCnt, ref message);

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
                        // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                        // [変数：バッチ予約ユーザID] = [バッチ予約情報：予約ユーザID]
                        batchYoyakuId = batchYoyaku.BatchYoyakuId;
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
                        // ７．１．「組合員等別引受情報」を取得
                        // ７．１．１．「組合員等別引受情報」を取得する。
                        List<KumiaiintoBetuHikiukeData> kumiaiintobetuHikiukeDatas = GetKumiaiintobetuHikiukeData(dbContext, batchJouken, todofukenCd, kumiaitoCd);

                        // ７．２．「組合員等別引受情報」のデータ有無を確認
                        // ７．２．１．取得したデータ件数が0件の場合
                        if (kumiaiintobetuHikiukeDatas.Count == 0)
                        {
                            // [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10076" 引数{0}：0)
                            throw new AppException("ME10076", MessageUtil.Get("ME10076", "0"));
                        }

                        // ７．３．「規模別面積区分情報」を取得
                        // ７．３．１．「規模別面積区分情報」を取得する。
                        List<KibobetuMensekiKbnData> kibobetuBunpuDatas = GetKibobetuMensekiKbnData(dbContext, batchJouken, todofukenCd, kumiaitoCd);

                        // ７．４．「規模別面積区分情報」のデータ有無を確認
                        // ７．４．１．取得したデータ件数が0件の場合
                        if (kibobetuBunpuDatas.Count == 0)
                        {
                            // [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10076" 引数{0}：0)
                            throw new AppException("ME10076", MessageUtil.Get("ME10076", "0"));
                        }

                        // ７．４．２．取得したデータ件数が0件以外の場合
                        // ７．４．２．１．ZIPファイル格納先パスを作成して変数に設定する
                        // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                        string zipFolderPath = Path.Combine(
                            ConfigUtil.Get(NskCommon.CoreConst.CSV_OUTPUT_FOLDER),
                                bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                        // ７．４．２．２．作成したZIPファイル格納先パスにZIPファイル格納フォルダを作成する
                        Directory.CreateDirectory(zipFolderPath);

                        // ７．５．「規模別面積区分情報」を内部配列に展開
                        KibobetuMensekiKbnData[] kibobetuBunpuDatasArray = new KibobetuMensekiKbnData[NskCommon.CoreConst.HAIRETU_YOUSO_NUM];
                        kibobetuBunpuDatas.CopyTo(kibobetuBunpuDatasArray, 0);
                        // ７．５．１．取得した「規模別面積区分情報」を１件づつ内部配列に設定する。
                        // 規模別面積区分（ｎ） = [規模別面積区分情報.規模別面積区分]
                        int[] kibobetuMensekiKbn = new int[NskCommon.CoreConst.HAIRETU_YOUSO_NUM];
                        // 対象面積上限（ｎ） = [規模別面積区分情報.対象面積上限]
                        int[] taishouMensekiJougen = new int[NskCommon.CoreConst.HAIRETU_YOUSO_NUM];
                        // 対象面積下限（ｎ）
                        int[] taishouMensekiKagen = new int[NskCommon.CoreConst.HAIRETU_YOUSO_NUM];
                        // 引受戸数計（ｎ） = 0
                        int[] hikiukeKosukei = new int[NskCommon.CoreConst.HAIRETU_YOUSO_NUM];
                        // 引受面積計（ｎ） = 0
                        decimal[] hikiukeMensekiKei = new decimal[NskCommon.CoreConst.HAIRETU_YOUSO_NUM];

                        for (int n = 0; n < NskCommon.CoreConst.HAIRETU_YOUSO_NUM; n++)
                        {
                            kibobetuMensekiKbn[n] = int.Parse(kibobetuBunpuDatasArray[n].規模別面積区分);
                            taishouMensekiJougen[n] = int.Parse(kibobetuBunpuDatasArray[n].対象面積上限);
                            if (kibobetuMensekiKbn[n] == 1)
                            {
                                taishouMensekiKagen[n] = 0;
                            }
                            else
                            {
                                taishouMensekiKagen[n] = taishouMensekiJougen[n - 1] + 1;
                            }
                            hikiukeKosukei[n] = 0;
                            hikiukeMensekiKei[n] = 0;
                        }

                        // ８．規模別面積区分別の集計（ファイル出力用集計）				
                        // ８．１．「組合員等別引受情報」を順次１件づつ読み込む。			
                        // ８．１．１．規模別面積区分毎の振り分け
                        // 組合員等別引受情報」の[引受面積区分]を内部配列の「対象面積上限」～「対象面積下限」と比較する。	
                        // 該当する規模別面積区分の「引受戸数計」と「引受面積計」に集計する
                        // 引受戸数計（ｎ）：引受戸数計（ｎ）＋「組合員等別引受情報.引受戸数計」
                        // 引受面積計（ｎ）：引受面積計（ｎ）＋「組合員等別引受情報.引受面積計」
                        // 引受戸数計（ｎ）と引受面積計（ｎ）用のカウント：i
                        int i = 0;
                        int[] kibobetuKbn = new int[NskCommon.CoreConst.HAIRETU_YOUSO_NUM];
                        int[] taishouJougen = new int[NskCommon.CoreConst.HAIRETU_YOUSO_NUM];
                        foreach (KumiaiintoBetuHikiukeData data in kumiaiintobetuHikiukeDatas)
                        {
                            for (int n = 0; n < NskCommon.CoreConst.HAIRETU_YOUSO_NUM; n++)
                            {
                                if (data.引受面積区分 <= taishouMensekiJougen[n] && data.引受面積区分 >= taishouMensekiKagen[n])
                                {
                                    hikiukeKosukei[n] = hikiukeKosukei[n] + data.引受戸数計;
                                    hikiukeMensekiKei[n] = hikiukeMensekiKei[n] + data.引受面積計;
                                    kibobetuKbn[i] = kibobetuMensekiKbn[n];
                                    taishouJougen[i] = taishouMensekiJougen[n];
                                    i++;
                                    break;
                                }
                            }
                        }

                        // ８．２．データ件数の取得
                        // 内部配列で引受戸数計（ｎ）が０以外の配列件数をカウントする。
                        int nonZeroConter = 0;
                        foreach (int koseki in hikiukeKosukei)
                        {
                            if (koseki != 0)
                            {
                                nonZeroConter++;
                            }
                        }

                        // ９．規模別分布状況データ出力処理
                        // ９．１．[変数：文字コード] で指定した文字コードの出力用規模別分布状況データファイル作成
                        // 一時領域にデータ一時出力フォルダとファイルを作成する
                        // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/
                        string tempFolderPath = Path.Combine(
                            ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH),
                                bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                        Directory.CreateDirectory(tempFolderPath);
                        // ファイル名：NKXX01YY.TXT：引受方式が半相殺、全相殺、地域インデックス（XX：共済目的コード、YY：都道府県コード）
                        string fileName = "NK" + batchJouken.JoukenKyosaiMokutekiCd + "01" + todofukenCd + Core.CoreConst.SYMBOL_DOT + "TXT";
                        string filePath = Path.Combine(tempFolderPath, fileName);

                        // ９．２．[配列：バッチ条件] の抽出区分に沿った規模別分布状況データ出力
                        // 該当する条件の形式で出力用規模別分布状況データファイルに規模別分布状況データを出力する。
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

                        // ファイルの生成
                        if (isCanWrite)
                        {
                            using (FileStream fs = File.Create(filePath))
                            {
                                // ファイルの書き込み
                                using (StreamWriter writer = new(fs, encoding))
                                {
                                    // 内部配列に修正
                                    KumiaiintoBetuHikiukeData[] array = kumiaiintobetuHikiukeDatas.ToArray();
                                    // 組合等略称
                                    string kumiaitoRyakushou = string.Empty;
                                    // 3行目以降の出力内容
                                    List<List<string>> dataRecords = new();

                                    // 3行目以降の内容
                                    for (int n = 0; n < array.Length; n++)
                                    {
                                        kumiaitoRyakushou = array[n].組合等略称;
                                        List<string> dataRecord =
                                        [
                                            array[n].共済目的コード,
                                            array[n].年産.ToString(),
                                            array[n].引受回.ToString(),
                                            "0",
                                            array[n].組合等コード,
                                            array[n].支所コード,
                                            // todo: 設計書修正完了次第修正　No.１４４
                                            array[n].大地区コード,
                                            kibobetuKbn[n].ToString(),
                                            taishouJougen[n].ToString(),
                                            array[n].引受戸数計.ToString(),
                                            array[n].引受面積計.ToString()
                                        ];
                                        dataRecords.Add(dataRecord);
                                    }

                                    // 1行目の内容
                                    List<string> header =
                                    [
                                        "NSK",
                                        "規模別分布状況ﾃﾞｰﾀ",
                                        kumiaitoCd,
                                        kumiaitoRyakushou,
                                        DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss"),
                                        kumiaiintobetuHikiukeDatas.Count.ToString(),
                                        batchJouken.JoukenKyosaiMokutekiCd,
                                        "0",
                                        kumiaitoCd,
                                        batchJouken.JoukenNensan
                                    ];

                                    // 2行目の内容
                                    List<string> dataStart =
                                    [
                                        NskCommon.CoreConst.DATA_START,
                                        DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss")
                                    ];

                                    // 最終行の内容
                                    List<string> dataEnd =
                                    [
                                        NskCommon.CoreConst.DATA_END,
                                        DateUtil.GetSysDateTime().ToString("yyyy/MM/dd HH:mm:ss")
                                    ];

                                    // 配列の内容を出力する
                                    writer.Write(CsvUtil.GetLine(header.ToArray()));
                                    writer.Write(CsvUtil.GetLine(dataStart.ToArray()));
                                    foreach (List<string> dataRecord in dataRecords)
                                    {
                                        writer.Write(CsvUtil.GetLine(dataRecord.ToArray()));
                                    }
                                    writer.Write(CsvUtil.GetLine(dataEnd.ToArray()));
                                }
                            }
                        }

                        // ９．３．Zip暗号化を行う。		
                        // ９．３．１．データ一時出力フォルダ内のファイルを共通部品「ZipUtil.CreateZip」でZip化（暗号化）し
                        Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                        // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                        // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                        // [変数：ZIPファイル格納先パス]とファイル名でパスを登録します。
                        FolderUtil.MoveFile(zipFilePath, zipFolderPath, batchYoyakuId, nBid);

                        // ９．３．２．「９．１．」のフォルダを削除する。	
                        if (Directory.Exists(tempFolderPath))
                        {
                            Directory.Delete(tempFolderPath, true);
                        }

                        // ９．４．処理正常終了時
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
                    if (aEx.ErrorCode.Equals("ME10076"))
                    {
                        // 処理異常終了時（ME10076）
                        // [変数：処理ステータス]  に"03"（成功）を設定
                        status = NskCommon.CoreConst.STATUS_SUCCESS;
                    }
                    else
                    {
                        // 処理異常終了時
                        // [変数：処理ステータス] に"99"（エラー）を設定
                        status = NskCommon.CoreConst.STATUS_ERROR;
                    }

                    // [変数：エラーメッセージ] にエラーメッセージを設定
                    errorMessage = aEx.Message;
                    // エラーメッセージをERRORログに出力する
                    logger.Error(errorMessage);
                }

                // １０．バッチ実行状況更新
                // １０．１．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
                // １０．２．共通機能の「バッチ実行状況更新」から戻り値を受け取る。
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

                // １０．３．処理を終了する。
                Environment.ExitCode = result;
            }
            catch (Exception ex)
            {
                // １１．エラー処理
                // １１．１．例外（エクセプション）の場合
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
        /// 「組合員等別引受情報」の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="batchJouken">バッチ条件</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns></returns>
        private static List<KumiaiintoBetuHikiukeData> GetKumiaiintobetuHikiukeData(NskAppContext dbContext, BatchJouken batchJouken, string todofukenCd, string kumiaitoCd)
        {
            StringBuilder sql = new();

            sql.Append($"	SELECT	 ");
            sql.Append($"	    T1.共済目的コード	 ");
            sql.Append($"	    , T1.年産	 ");
            sql.Append($"	    , T1.組合等コード	 ");
            sql.Append($"	    , T1.支所コード 	 ");
            sql.Append($"	    , MAX(T1.引受回) AS 引受回	 ");
            sql.Append($"	    , TRUNC(SUM(T1.引受面積計)) AS 引受面積区分	 ");
            sql.Append($"	    , COUNT(T1.組合等コード) AS 引受戸数計	 ");
            sql.Append($"	    , SUM(T1.引受面積計) AS 引受面積計	 ");
            sql.Append($"	    , T3.kumiaito_rnm AS 組合等略称 	 ");
            // todo: 設計書修正完了次第修正　No.１４４
            sql.Append($"	    , T1.大地区コード ");
            sql.Append($"	FROM	 ");
            sql.Append($"	    t_12040_組合員等別引受情報 T1 	 ");
            sql.Append($"	    INNER JOIN ( 	 ");
            sql.Append($"	        SELECT	 ");
            sql.Append($"	            組合等コード	 ");
            sql.Append($"	            , 年産	 ");
            sql.Append($"	            , 共済目的コード	 ");
            sql.Append($"	            , 支所コード	 ");
            sql.Append($"	            , MAX(引受回) AS 最大引受回 	 ");
            sql.Append($"	        FROM	 ");
            sql.Append($"	            t_00010_引受回 	 ");
            sql.Append($"	        WHERE	 ");
            sql.Append($"	            引受計算実施日 IS NOT NULL 	 ");
            sql.Append($"	        GROUP BY	 ");
            sql.Append($"	            組合等コード	 ");
            sql.Append($"	            , 年産	 ");
            sql.Append($"	            , 共済目的コード	 ");
            sql.Append($"	            , 支所コード	 ");
            sql.Append($"	    ) T2	 ");
            sql.Append($"	        ON T1.共済目的コード = T2.共済目的コード 	 ");
            sql.Append($"	        AND T1.年産 = T2.年産 	 ");
            sql.Append($"	        AND T1.組合等コード = T2.組合等コード 	 ");
            sql.Append($"	        AND T1.支所コード = T2.支所コード 	 ");
            sql.Append($"	        AND T1.引受回 = T2.最大引受回 	 ");
            sql.Append($"	    INNER JOIN v_kumiaito T3 	 ");
            sql.Append($"	        ON T3.kumiaito_cd = T1.組合等コード 	 ");
            sql.Append($"	        AND T3.todofuken_cd = @都道府県コード  	 ");
            sql.Append($"	WHERE	 ");
            sql.Append($"	    T1.共済目的コード = @条件_共済目的コード 	 ");
            sql.Append($"	    AND T1.年産 = @条件_年産 	 ");
            sql.Append($"	    AND T1.組合等コード = @条件_組合等コード 	 ");
            sql.Append($"	    AND CASE 	 ");
            sql.Append($"	        WHEN @条件_支所コード <> '00' 	 ");
            sql.Append($"	            THEN T1.支所コード = @条件_支所コード 	 ");
            sql.Append($"	        ELSE T1.支所コード IN ( 	 ");
            sql.Append($"	            SELECT	 ");
            sql.Append($"	                shisho_cd 	 ");
            sql.Append($"	            FROM	 ");
            sql.Append($"	                v_shisho_nm 	 ");
            sql.Append($"	            WHERE	 ");
            sql.Append($"	                todofuken_cd = @都道府県コード 	 ");
            sql.Append($"	                AND kumiaito_cd = @組合等コード 	 ");
            sql.Append($"	                AND shisho_cd <> '00'	 ");
            sql.Append($"	        ) 	 ");
            sql.Append($"	        END 	 ");
            sql.Append($"	    AND T1.類区分 = '0' 	 ");
            sql.Append($"	GROUP BY	 ");
            sql.Append($"	    T1.共済目的コード	 ");
            sql.Append($"	    , T1.年産	 ");
            sql.Append($"	    , T1.組合等コード	 ");
            sql.Append($"	    , T1.支所コード	 ");
            sql.Append($"	    , T1.引受回	 ");
            sql.Append($"	    , TRUNC(T1.引受面積計)	 ");
            sql.Append($"	    , T3.kumiaito_rnm	 ");
            // todo: 設計書修正完了次第修正　No.１４４
            sql.Append($"	    , T1.大地区コード ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("都道府県コード", todofukenCd),
                new("組合等コード", kumiaitoCd),
                new("条件_組合等コード", batchJouken.JoukenKumiaitoCd),
                new("条件_年産", int.Parse(batchJouken.JoukenNensan)),
                new("条件_共済目的コード", batchJouken.JoukenKyosaiMokutekiCd),
                new("条件_支所コード", batchJouken.JoukenShishoCd)
            ];

            // SQLのクエリ結果をListに格納する
            List<KumiaiintoBetuHikiukeData> datas = dbContext.Database.SqlQueryRaw<KumiaiintoBetuHikiukeData>(sql.ToString(), parameters.ToArray()).ToList();

            return datas;
        }

        /// <summary>
        /// 「規模別面積区分情報」の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="batchJouken">バッチ条件</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns></returns>
        private static List<KibobetuMensekiKbnData> GetKibobetuMensekiKbnData(NskAppContext dbContext, BatchJouken batchJouken, string todofukenCd, string kumiaitoCd)
        {
            StringBuilder sql = new();

            sql.Append($"	SELECT 	");
            sql.Append($"	    T1.共済目的コード 	");
            sql.Append($"	    , T1.年産 	");
            sql.Append($"	    , T1.組合等コード 	");
            sql.Append($"	    , T1.支所コード 	");
            sql.Append($"	    , T1.規模別面積区分 	");
            sql.Append($"	    , T1.対象面積上限 	");
            sql.Append($"	FROM 	");
            sql.Append($"	    t_14070_規模別面積区分情報 T1 	");
            sql.Append($"	WHERE 	");
            sql.Append($"	    T1.組合等コード = @条件_組合等コード 	");
            sql.Append($"	    AND T1.年産 = @条件_年産 	");
            sql.Append($"	    AND T1.共済目的コード = @条件_共済目的コード 	");
            sql.Append($"	    AND CASE 	");
            sql.Append($"	        WHEN @条件_支所コード <> '00' 	");
            sql.Append($"	            THEN T1.支所コード = @条件_支所コード 	");
            sql.Append($"	        ELSE T1.支所コード IN ( 	");
            sql.Append($"	            SELECT 	");
            sql.Append($"	                支所コード 	");
            sql.Append($"	            FROM 	");
            sql.Append($"	                v_shisho_nm 	");
            sql.Append($"	            WHERE 	");
            sql.Append($"	                todofuken_cd = @都道府県コード 	");
            sql.Append($"	                AND kumiaito_cd = @組合等コード 	");
            sql.Append($"	                AND shisho_cd <> '00' ) 	");
            sql.Append($"	        END 	");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("都道府県コード", todofukenCd),
                new("組合等コード", kumiaitoCd),
                new("条件_組合等コード", batchJouken.JoukenKumiaitoCd),
                new("条件_年産", int.Parse(batchJouken.JoukenNensan)),
                new("条件_共済目的コード", batchJouken.JoukenKyosaiMokutekiCd),
                new("条件_支所コード", batchJouken.JoukenShishoCd)
            ];

            // SQLのクエリ結果をListに格納する
            List<KibobetuMensekiKbnData> datas = dbContext.Database.SqlQueryRaw<KibobetuMensekiKbnData>(sql.ToString(), parameters.ToArray()).ToList();

            return datas;
        }
    }
}
