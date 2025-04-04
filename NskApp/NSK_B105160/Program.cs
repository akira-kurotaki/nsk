using System.Text;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NLog;
using NSK_B105160.Models;
using NskAppModelLibrary.Context;
using static CoreLibrary.Core.Utility.BatchUtil;
using Core = CoreLibrary.Core.Consts;
using NskCommon = NskCommonLibrary.Core.Consts;

namespace NSK_B105160
{
    /// <summary>
    /// 基準収穫量設定一覧表（災害収入、品質）
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
        /// 基準収穫量設定一覧表（災害収入、品質）
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

                // ３．パラメータ情報のチェック
                // ３．１．必須チェック

                string requiedError = string.Empty;

                if (string.IsNullOrEmpty(bid))
                {
                    // ３．１．１．[変数：バッチID]が未入力の場合
                    requiedError = "バッチID";

                }
                else if (string.IsNullOrEmpty(todofukenCd))
                {
                    // ３．１．２．[変数：都道府県コード]が未入力の場合
                    requiedError = "都道府県コード";

                }
                else if (string.IsNullOrEmpty(kumiaitoCd))
                {
                    // ３．１．３．[変数：組合等コード]が未入力の場合
                    requiedError = "組合等コード";

                }
                else if (string.IsNullOrEmpty(shishoCd))
                {
                    // ３．１．４．[変数：支所コード]が未入力の場合
                    requiedError = "支所コード";

                }
                else if (string.IsNullOrEmpty(jid))
                {
                    // ３．１．５．[変数：バッチ条件のキー情報]が未入力の場合
                    requiedError = "バッチ条件のキー情報";
                }

                if (!requiedError.Equals(string.Empty))
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME01054" 引数{0} ：未入力チェックNGだった項目名）
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", requiedError));
                }
                
                // バッチIDの数値変換
                if (!int.TryParse(bid, out nBid))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    // （"ME90012"　引数{0} ：バッチID)
                    throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
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

                // バッチ予約状況取得
                // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
                BatchYoyaku? batchYoyakuList = GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message).FirstOrDefault();

                // [引数：バッチID]に一致するバッチ予約状況が取得できない場合、[変数：エラーメッセージ]を設定し、「１０．」へ進む。
                if (batchYoyakuList is not null && intAllCnt != 0)
                {

                    // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                    if (batchYoyakuList.BatchYoyakuId != string.Empty)
                    {
                        batchYoyakuId = batchYoyakuList.BatchYoyakuId;
                    }
                    else
                    {
                        // エラーメッセージをログに出力する。
                        // （"ME01645"、{0}：パラメータの取得）
                        throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                    }
                }
                else
                {
                    // エラーメッセージをログに出力する。
                    // （"ME01645"、{0}：パラメータの取得）
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // ５．バッチ条件を取得

                using (NskAppContext dbContext = new(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt("CommandTimeout")))
                {
                    BatchJoken batchJoken = new();

                    // ５．１．バッチ条件情報の取得
                    // 条件値のリスト
                    batchJoken.GetBatchJoken(dbContext, jid);

                    // ５．２．２．[変数：年産]がnullまたは空文字の場合、
                    if (string.IsNullOrEmpty(batchJoken.JokenNensan))
                    {
                        // [変数：エラーメッセージ]を設定し、「１０．」へ進む。
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "年産"));
                    }

                    // ５．２．３．[変数：共済目的コード]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenKyosaiMokutekiCd))
                    {
                        // [変数：エラーメッセージ]を設定し、「１０．」へ進む。
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "共済目的コード"));
                    }

                    // ５．２．４．[変数：文字コード]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenMojiCd))
                    {
                        // [変数：エラーメッセージ]を設定し、「１０．」へ進む。
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "文字コード"));
                    }


                    // ６．コードの整合性チェック

                    // [変数：都道府県コード]
                    // ６．１．「都道府県コード存在情報」を取得する。
                    // ６．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (BatchJoken.GetTodofukenCdSonzaiJoho(dbContext, todofukenCd) == 0)
                    {
                        // マスタなし
                        // [変数：エラーメッセージ]を設定し、「１０.」へ進む。
                        throw new AppException("ME10005", MessageUtil.Get("ME10005", "都道府県コード"));
                    }

                    // [変数：共済目的コード]
                    // ６．３．[配列：バッチ条件]から共済目的コードが取得できた場合、「共済目的コード存在情報」を取得する。
                    // ６．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (BatchJoken.GetKyosaiMokutekiCdSonzaiJoho(dbContext, batchJoken.JokenKyosaiMokutekiCd) == 0)
                    {
                        // マスタなし
                        // [変数：エラーメッセージ]を設定し、「１０.」へ進む。
                        throw new AppException("ME10005", MessageUtil.Get("ME10005", "共済目的コード"));
                    }

                    // [変数：組合等コード]
                    // ６．５．[変数：組合等コード]が入力されている場合、「組合等コード存在情報」を取得する。
                    // ６．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (BatchJoken.GetKumiaitoCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd) == 0)
                    {
                        // マスタなし
                        // [変数：エラーメッセージ]を設定し、「１０.」へ進む。
                        throw new AppException("ME10005", MessageUtil.Get("ME10005", "組合等コード"));
                    }

                    // [変数：大地区コード]
                    // ６．７．[配列：バッチ条件]から大地区コードが取得できた場合、「大地区コード存在情報」を取得する。
                    // ６．８．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (!string.IsNullOrWhiteSpace(batchJoken.JokenDaichiku) && BatchJoken.GetDaichikuSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, batchJoken.JokenDaichiku) == 0)
                    {
                        // マスタなし
                        // [変数：エラーメッセージ]を設定し、「１０.」へ進む。
                        throw new AppException("ME10005", MessageUtil.Get("ME10005", "大地区コード"));
                    }


                    // ７．データ検索SQLを実行（ログ出力：あり）
                    List<KijunShukakuryoSetteiRecord> KijunShukakuryoSettei = BatchJoken.GetKijunShukakuryoSettei(dbContext, todofukenCd, kumiaitoCd, shishoCd, batchJoken);

                    // ７．３．取得した件数が0件の場合
                    if (KijunShukakuryoSettei.Count == 0)
                    {
                        //   [変数：エラーメッセージ] に以下のメッセージを設定し、「１０．」へ進む。
                        throw new AppException("ME10076", MessageUtil.Get("ME10076", "0"));
                    }

                    // ７．３．取得した件数が0件以外の場合
                    // ７．３．１．ZIPファイル格納先パスを作成して変数に設定する
                    // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                    string zipFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.CSV_OUTPUT_FOLDER), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    // ７．３．２．作成したZIPファイル格納先パスにZIPファイル格納フォルダを作成する
                    Directory.CreateDirectory(zipFolderPath);

                    // ８．共済金額一覧表データ出力処理
                    // ８．１．データ一時出力フォルダ作成
                    // 一時領域にデータ一時出力フォルダとファイルを作成する
                    // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/ 
                    string tempFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    Directory.CreateDirectory(tempFolderPath);

                    // ８．２．ファイル名が未入力の場合、規定のファイル名を設定する。
                    string fileName = string.Empty;
                    if (string.IsNullOrWhiteSpace(batchJoken.JokenFileName))
                    {
                        // 規定ファイル名：[基準収穫量設定一覧表（災害収入、品質）]+"_"+出力日時（yyyyMMddHHmmss）++"TXT"
                        fileName = "[基準収穫量設定一覧表（災害収入、品質）]_" + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss") + NskCommon.CoreConst.FILE_EXTENSION_TXT;
                    }
                    else
                    {
                        fileName = batchJoken.JokenFileName + NskCommon.CoreConst.FILE_EXTENSION_TXT;
                    }
                    
                    string filePath = Path.Combine(tempFolderPath, fileName);

                    // ８．３．取得したデータを[引数：文字コード]の文字コードに変換する。
                    // ８．４．[配列：バッチ条件]の抽出区分に沿った共済金額一覧表データ出力
                    // 該当する条件の形式で共済金額一覧表データファイルに
                    // 共済金額一覧表データを出力する。
                    // [TODO] 抽出区分とは？

                    using (FileStream fs = File.Create(filePath))
                    {

                        // 文字コード
                        Encoding encoding = Encoding.Default;
                        if (Convert.ToInt32(Core.CoreConst.CharacterCode.UTF8).ToString().Equals(batchJoken.JokenMojiCd))
                        {
                            encoding = Encoding.UTF8;
                        }
                        else if (Convert.ToInt32(Core.CoreConst.CharacterCode.SJIS).Equals(batchJoken.JokenMojiCd))
                        {
                            encoding = Encoding.GetEncoding("Shift_JIS");
                        }


                        // ファイルの書き込み
                        using (StreamWriter writer = new(fs, encoding))
                        {

                            // ヘッダ書き込みフラグ初期化
                            bool headerFlg = true;

                            // 取得した基準収穫量設定一覧表（災害収入、品質）データを読み込む
                            foreach (KijunShukakuryoSetteiRecord KijunShukakuryoSetteiRecord in KijunShukakuryoSettei)
                            {

                                // ヘッダ部（1行目）の出力
                                // 1レコード目の場合、ヘッダ部を出力
                                if (headerFlg) {

                                    // ヘッダ部の内容を配列にまとめる
                                    List<string> haniRecords =
                                    [
                                        "組合等コード",
                                        "組合等名",
                                        "年産",
                                        "共済目的コード",
                                        "共済目的名",
                                        "引受方式",
                                        "引受方式名称",
                                        "支所コード",
                                        "支所名",
                                        "大地区コード",
                                        "大地区名",
                                        "小地区コード",
                                        "小地区名",
                                        "組合員等コード",
                                        "組合員等氏名",
                                        "類区分",
                                        "類区分名",
                                        "産地別銘柄コード",
                                        "産地別銘柄名称",
                                        "営農対象外フラグ",
                                        "平均単収",
                                        "規格別割合（規格1）",
                                        "規格別割合（規格2）",
                                        "規格別割合（規格3）",
                                        "規格別割合（規格4）",
                                        "規格別割合（規格5）",
                                        "規格別割合（規格6）",
                                        "規格別割合（規格7）",
                                        "規格別割合（規格8）",
                                        "規格別割合（規格9）",
                                        "規格別割合（規格10）",
                                        "規格別割合（規格11）",
                                        "規格別割合（規格12）",
                                        "規格別割合（規格13）",
                                        "規格別割合（規格14）",
                                        "規格別割合（規格15）",
                                        "規格別割合（規格16）",
                                        "規格別割合（規格17）",
                                        "規格別割合（規格18）",
                                        "規格別割合（規格19）",
                                        "規格別割合（規格20）",
                                        "規格別割合（規格21）",
                                        "規格別割合（規格22）",
                                        "規格別割合（規格23）",
                                        "規格別割合（規格24）",
                                        "規格別割合（規格25）",
                                        "規格別割合（規格26）",
                                        "規格別割合（規格27）",
                                        "規格別割合（規格28）",
                                        "規格別割合（規格29）",
                                        "規格別割合（規格30）",
                                        "規格別割合（規格31）",
                                        "規格別割合（規格32）",
                                        "規格別割合（規格33）",
                                        "規格別割合（規格34）",
                                        "規格別割合（規格35）",
                                        "規格別割合（規格36）",
                                        "規格別割合（規格37）",
                                        "規格別割合（規格38）",
                                        "規格別割合（規格39）",
                                        "規格別割合（規格40）"
                                    ];
                                    // 配列の内容を書き込む
                                    writer.Write(CsvUtil.GetLine(haniRecords.ToArray()));

                                    // 2レコード以降にヘッダを出力しないようフラグ更新
                                    headerFlg = false;
                                }

                                // データ部(2行目以降)の出力
                                // データ部の内容を配列にまとめる
                                List<string> KijunShukakuryoSetteiDataRecord =
                                [
                                    // 組合等コード
                                    KijunShukakuryoSetteiRecord.組合等コード,
                                    // 組合等名
                                    KijunShukakuryoSetteiRecord.組合等名,
                                    // 年産
                                    KijunShukakuryoSetteiRecord.年産.ToString(),
                                    // 共済目的コード
                                    KijunShukakuryoSetteiRecord.共済目的コード,
                                    // 共済目的名
                                    KijunShukakuryoSetteiRecord.共済目的名,
                                    // 引受方式
                                    KijunShukakuryoSetteiRecord.引受方式,
                                    // 引受方式名称
                                    KijunShukakuryoSetteiRecord.引受方式名称,
                                    // 支所コード
                                    KijunShukakuryoSetteiRecord.支所コード,
                                    // 支所名
                                    KijunShukakuryoSetteiRecord.支所名,
                                    // 大地区コード
                                    KijunShukakuryoSetteiRecord.大地区コード,
                                    // 大地区名
                                    KijunShukakuryoSetteiRecord.大地区名,
                                    // 小地区コード
                                    KijunShukakuryoSetteiRecord.小地区コード,
                                    // 小地区名
                                    KijunShukakuryoSetteiRecord.小地区名,
                                    // 組合員等コード
                                    KijunShukakuryoSetteiRecord.組合員等コード,
                                    // 組合員等氏名
                                    KijunShukakuryoSetteiRecord.組合員等氏名,
                                    // 類区分
                                    KijunShukakuryoSetteiRecord.類区分,
                                    // 類区分名
                                    KijunShukakuryoSetteiRecord.類区分名,
                                    // 産地別銘柄コード
                                    KijunShukakuryoSetteiRecord.産地別銘柄コード,
                                    // 産地別銘柄名称
                                    KijunShukakuryoSetteiRecord.産地別銘柄名称,
                                    // 営農対象外フラグ
                                    KijunShukakuryoSetteiRecord.営農対象外フラグ,
                                    // 平均単収
                                    KijunShukakuryoSetteiRecord.平均単収.ToString(),
                                    // 規格別割合（規格1）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格1.ToString(),
                                    // 規格別割合（規格2）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格2.ToString(),
                                    // 規格別割合（規格3）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格3.ToString(),
                                    // 規格別割合（規格4）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格4.ToString(),
                                    // 規格別割合（規格5）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格5.ToString(),
                                    // 規格別割合（規格6）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格6.ToString(),
                                    // 規格別割合（規格7）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格7.ToString(),
                                    // 規格別割合（規格8）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格8.ToString(),
                                    // 規格別割合（規格9）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格9.ToString(),
                                    // 規格別割合（規格10）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格10.ToString(),
                                    // 規格別割合（規格11）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格11.ToString(),
                                    // 規格別割合（規格12）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格12.ToString(),
                                    // 規格別割合（規格13）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格13.ToString(),
                                    // 規格別割合（規格14）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格14.ToString(),
                                    // 規格別割合（規格15）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格15.ToString(),
                                    // 規格別割合（規格16）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格16.ToString(),
                                    // 規格別割合（規格17）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格17.ToString(),
                                    // 規格別割合（規格18）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格18.ToString(),
                                    // 規格別割合（規格19）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格19.ToString(),
                                    // 規格別割合（規格20）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格20.ToString(),
                                    // 規格別割合（規格21）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格21.ToString(),
                                    // 規格別割合（規格22）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格22.ToString(),
                                    // 規格別割合（規格23）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格23.ToString(),
                                    // 規格別割合（規格24）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格24.ToString(),
                                    // 規格別割合（規格25）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格25.ToString(),
                                    // 規格別割合（規格26）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格26.ToString(),
                                    // 規格別割合（規格27）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格27.ToString(),
                                    // 規格別割合（規格28）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格28.ToString(),
                                    // 規格別割合（規格29）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格29.ToString(),
                                    // 規格別割合（規格30）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格30.ToString(),
                                    // 規格別割合（規格31）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格31.ToString(),
                                    // 規格別割合（規格32）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格32.ToString(),
                                    // 規格別割合（規格33）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格33.ToString(),
                                    // 規格別割合（規格34）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格34.ToString(),
                                    // 規格別割合（規格35）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格35.ToString(),
                                    // 規格別割合（規格36）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格36.ToString(),
                                    // 規格別割合（規格37）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格37.ToString(),
                                    // 規格別割合（規格38）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格38.ToString(),
                                    // 規格別割合（規格39）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格39.ToString(),
                                    // 規格別割合（規格40）
                                    KijunShukakuryoSetteiRecord.規格別割合_規格40.ToString()
                                ];

                                // 配列の内容を書き込む
                                writer.Write(CsvUtil.GetLine(KijunShukakuryoSetteiDataRecord.ToArray()));
                            }
                        }
                    }

                    // ８．５．Zip暗号化を行う。
                    // ８．５．１．「８．１．」のフォルダ内のテキストをZip化（暗号化）し、
                    Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                    // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                    // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                    // [変数：ZIPファイル格納先パス]とファイル名でパスを登録します。
                    NskCommonLibrary.Core.Utility.FolderUtil.MoveFile(zipFilePath, zipFolderPath, bid, nBid);

                    // ８．５．２．「８．１．」のフォルダを削除する。
                    if (Directory.Exists(tempFolderPath))
                    { 
                        Directory.Delete(tempFolderPath, true);
                    }

                    // ８．６．処理正常終了時
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
                    //  [変数：エラーメッセージ] に予期せぬエラーメッセージを設定
                    // （"MF00001")
                    errorMessage = MessageUtil.Get("MF00001");
                }
                else
                {
                    //  [変数：エラーメッセージ] にエラーメッセージを設定
                    errorMessage = ex.Message;
                }

                // ログにエラーメッセージを出力する。
                logger.Error(ex, errorMessage);

            }
            catch (Exception ex)
            {

                // [変数：エラーメッセージ] に予期せぬエラーメッセージを設定
                errorMessage = MessageUtil.Get("MF00001");
                // [変数：ステータス]を「99：エラー」に更新する。
                status = BATCH_STATUS_ERROR;

                // ログにエラーメッセージを出力する。
                logger.Error(errorMessage);

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
