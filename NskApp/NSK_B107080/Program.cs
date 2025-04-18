using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using NLog;
using Npgsql;
using NSK_B107080.Models;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using System.Text;
using Core = CoreLibrary.Core.Consts;
using NskCommon = NskCommonLibrary.Core.Consts;
using NskCommonUtil = NskCommonLibrary.Core.Utility;

namespace NSK_B107080
{
    /// <summary>
    /// 徴収管理簿
    /// </summary>
    class Program
    {
        #region メンバー定数
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 自動振替処理の有無 「有」
        /// </summary>
        private static readonly string JIDO_FURIKAE_CD_ARI = "1";

        /// <summary>
        /// 自動振替処理の有無 「無」
        /// </summary>
        private static readonly string JIDO_FURIKAE_CD_NASHI = "2";

        /// <summary>
        /// 振込引落区分コード「現金」
        /// </summary>
        private static readonly string HIKIOTOSHI_KBN_CD_CASH = "4";

        /// <summary>
        /// 自動振替フラグ 「有」
        /// </summary>
        private static readonly string JIDO_FURIKAE_FLG_ARI = "1";

        /// <summary>
        /// 自動振替フラグ 「無」
        /// </summary>
        private static readonly string JIDO_FURIKAE_FLG_NASHI = "2";

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Program()
        {
            // Npgsql 6.0での「timestamp with time zone」非互換対応
            // Npgsql 6.0より前の動作に戻す
            // https://www.npgsql.org/doc/types/datetime.html#timestamps-and-timezones
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // SJIS(Shift_JIS)を使用可能にする
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 徴収管理簿
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
            // ユーザID
            string user_id = string.Empty;

            // [変数：エラーメッセージ] string.Empty
            string errorMessage = string.Empty;
            // [変数：ステータス]       STATUS_SUCCESS = "03"（成功）
            string status = NskCommon.CoreConst.STATUS_SUCCESS;
            // 処理結果（正常：0、エラー：1）
            int result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
            // バッチ予約ユーザID
            string batchYoyakuId = string.Empty;
            // バッチID(数値)
            long nBid = 0;

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
                // ユーザIDのキー情報
                if (args.Length > 5)
                {
                    user_id = args[5];
                }

                // ３．引数のチェック
                // ３．１．必須チェック
                // ３．１．１．[変数：バッチID] が未入力の場合
                if (string.IsNullOrEmpty(bid))
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME01645" 引数{0} ：パラメータの取得）
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチID"));
                }

                // ３．１．２．[変数：バッチID]が数値変換不可の場合
                // 数値化したバッチID
                if (!long.TryParse(bid, out nBid))
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

                //３．１．５．[変数：バッチ条件のキー情報] が未入力の場合
                if (string.IsNullOrEmpty(jid))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
                }

                //３．１．６．[変数：ユーザIDのキー情報] が未入力の場合
                if (string.IsNullOrEmpty(user_id))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "ユーザID"));
                }

                // バッチ予約状況取得引数の設定
                BatchUtil.GetBatchYoyakuListParam param = new()
                {
                    BatchId = nBid
                };

                SystemCommonContext db = new();
                ModelLibrary.Models.TBatchYoyaku? batchYoyaku = db.TBatchYoyakus.FirstOrDefault(x =>
                    (x.BatchId == param.BatchId) &&
                    (x.DeleteFlg == BatchUtil.DELETE_FLG_NOT_DELETED));

                // バッチ予約が存在する場合
                if (batchYoyaku != null)
                {
                    // [引数：バッチID]に一致する場合
                    // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                    // バッチ予約ユーザID = バッチ予約情報.予約ユーザID
                    batchYoyakuId = batchYoyaku.BatchYoyakuId;
                }
                else
                {
                    // バッチ予約が存在しない場合、
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：予約ユーザID)
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "予約ユーザIDの取得"));
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

                //５．バッチ条件を取得
                //５．１．バッチ条件情報の取得
                //５．１．１．条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
                List<string> jokenNames =
                [
                    NskCommon.JoukenNameConst.JOUKEN_NENSAN,                // 年産
                    NskCommon.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,    // 共済目的コード
                    NskCommon.JoukenNameConst.JOUKEN_DAICHIKU,              // 大地区
                    NskCommon.JoukenNameConst.JOUKEN_SHOCHIKU_START,        // 小地区（開始）
                    NskCommon.JoukenNameConst.JOUKEN_SHOCHIKU_END,          // 小地区（終了）
                    NskCommon.JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,   // 組合員等コードFrom
                    NskCommon.JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,     // 組合員等コードTo
                    NskCommon.JoukenNameConst.JOUKEN_FILE_NAME,             // ファイル名
                    NskCommon.JoukenNameConst.JOUKEN_MOJI_CD                // 文字コード
                ];

                using (NskAppContext dbContext = new(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt(Core.CoreConst.COMMAND_TIMEOUT)))
                {
                    // ５．１．２．[変数：バッチ条件のキー情報] とListをキーにバッチ条件テーブルから「バッチ条件情報」を取得する。
                    // バッチ条件プロパティモデルは作成しない
                    List<T01050バッチ条件> batchJokens = GetbatchJoken(dbContext, jid, jokenNames);
                    // ５．１．３．「バッチ条件情報」が0件の場合
                    if (batchJokens.Count == 0)
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01645" 引数{ 0} ：パラメータの取得)
                        throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                    }

                    // ５．２．バッチ条件情報のチェック
                    // ５．２．１．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータのを条件値を変数に設定する。
                    // バッチ条件情報
                    BatchJoken batchJoken = new();

                    // 条件値のリストからバッチ条件情報への値設定
                    foreach (T01050バッチ条件 joken in batchJokens)
                    {
                        switch (joken.条件名称)
                        {
                            case NskCommon.JoukenNameConst.JOUKEN_NENSAN:               // 年産　※必須
                                batchJoken.JokenNensan = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD:   // 共済目的コード　※必須
                                batchJoken.JokenKyosaiMokutekiCd = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_DAICHIKU:             // 大地区
                                batchJoken.JokenDaichiku = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_SHOCHIKU_START:       // 小地区（開始）
                                batchJoken.JokenShochikuStart = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_SHOCHIKU_END:         // 小地区（終了）
                                batchJoken.JokenShochikuEnd = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START:  // 組合員等コードFrom
                                batchJoken.JokenKumiaiintoCdStart = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END:    // 組合員等コードTo
                                batchJoken.JokenKumiaiintoCdEnd = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_FILE_NAME:            // ファイル名　※必須
                                batchJoken.JokenFileName = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_MOJI_CD:              // 文字コード　※必須
                                batchJoken.JokenMojiCd = joken.条件値;
                                break;
                        }
                    }

                    // ５．２．２．[変数：年産]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenNensan))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：年産)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "年産"));
                    }

                    // ５．２．３．[変数：共済目的コード]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenKyosaiMokutekiCd))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：共済目的コード)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "共済目的コード"));
                    }

                    // ５．２．６．[変数：ファイル名]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenFileName))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：ファイル名)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "ファイル名"));
                    }

                    // ５．２．７．[変数：文字コード]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenMojiCd))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：文字コード)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "文字コード"));
                    }

                    // ６．コードの整合性チェック
                    // ６．１．「都道府県コード存在情報」を取得する。
                    int todofuken = GetTodofukenCdSonzaiJoho(dbContext, todofukenCd);

                    // ６．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (todofuken == 0)
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME10005"　引数{0} ：都道府県コード)
                        throw new AppException("ME10005", MessageUtil.Get("ME10005", "都道府県コード"));
                    }

                    // ６．３．[変数：組合等コード] が入力されている場合
                    if (!string.IsNullOrEmpty(kumiaitoCd))
                    {
                        // 「組合等コード存在情報」を取得する。
                        int kumiaito = GetKumiaitoCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd);

                        // ６．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (kumiaito == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005"　引数{0} ：組合等コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "組合等コード"));
                        }
                    }

                    // ６．５．[変数：支所コード]が入力されている場合
                    if (!string.IsNullOrEmpty(shishoCd))
                    {
                        // 「支所コード存在情報」を取得する。
                        int shisho = GetShishoCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, shishoCd);

                        // ６．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (shisho == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005"　引数{0} ：支所コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "支所コード"));
                        }
                    }

                    // ６．７．[配列：バッチ条件]から共済目的コードが取得できた場合
                    if (!string.IsNullOrEmpty(batchJoken.JokenKyosaiMokutekiCd))
                    {
                        // 「共済目的コード存在情報」を取得する。
                        int kyosaiMokuteki = GetKyosaiMokutekiCDSonzaiJoho(dbContext, batchJoken.JokenKyosaiMokutekiCd);

                        // ６．８．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (kyosaiMokuteki == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005"　引数{0} ：共済目的コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "共済目的コード"));
                        }
                    }

                    // ６．９．[配列：バッチ条件]から大地区コードが取得できた場合
                    if (!string.IsNullOrEmpty(batchJoken.JokenDaichiku))
                    {
                        // 「大地区コード存在情報」を取得する。
                        int daichiku = GetDaichikuCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, batchJoken.JokenDaichiku);

                        // ６．１０．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (daichiku == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005　引数{0} ：大地区コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "大地区コード"));
                        }
                    }

                    // ７．組合員等別引受計算結果一覧表の取得（ログ出力：あり）
                    // ７．１．「組合員等別引受計算結果一覧表」を取得する。
                    List<SaimokuDataRecord> saimokuDatas = GetSaimokuData(dbContext, todofukenCd, kumiaitoCd, shishoCd, batchJoken);

                    // ７．２．取得した件数が0件の場合
                    if (saimokuDatas.Count == 0)
                    {
                        //  [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME10076" 引数{0}：0)
                        throw new AppException("ME10076", MessageUtil.Get("ME10076", "0"));
                    }

                    // ７．３．取得した件数が0件以外の場合
                    // ８．ファイルを作成する。（ログ出力：あり）
                    // ファイル出力開始ログ（BEGIN CSV作成 バッチ名）
                    logger.Info(
                        Core.CoreConst.LOG_START_KEYWORD + Core.CoreConst.HALF_WIDTH_SPACE +
                        "CSV作成" + Core.CoreConst.HALF_WIDTH_SPACE +
                        "B107080_徴収管理簿");

                    // ８．１．ZIPファイル格納先パスを作成して変数に設定する
                    // ８．２．作成したZIPファイル格納先パスにZIPファイル格納フォルダを作成する
                    // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                    string zipFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.CSV_OUTPUT_FOLDER), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));

                    // ８．３．「７．１．」で取得したデータを[引数：文字コード]に文字変換する。
                    // [変数：文字コード]で指定した文字コードの出力用細目データファイル作成
                    // 一時領域にデータ一時出力フォルダとファイルを作成する
                    // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/
                    string tempFolderPath = NskCommonUtil.FolderUtil.CreateTempFolder(DateUtil.GetSysDateTime(), bid);
                    // ファイル名：[変数：ファイル名].csv
                    string fileName = batchJoken.JokenFileName + Core.CoreConst.FILE_EXTENSION_CSV;
                    string filePath = Path.Combine(tempFolderPath, fileName);

                    // ファイルの生成
                    using (FileStream fs = File.Create(filePath))
                    {
                        // 文字コード
                        Encoding encoding = Encoding.Default;
                        if (((int)Core.CoreConst.CharacterCode.UTF8).ToString().Equals(batchJoken.JokenMojiCd))
                        {
                            encoding = Encoding.UTF8;
                        }
                        else if (((int)Core.CoreConst.CharacterCode.SJIS).ToString().Equals(batchJoken.JokenMojiCd))
                        {
                            encoding = Encoding.GetEncoding("Shift_JIS");
                        }

                        // ファイルの書き込み
                        using (StreamWriter writer = new(fs, encoding))
                        {
                            // 1行目の出力
                            // 項目名の内容を配列にまとめる
                            List<string> saimokuKoumokuName =
                            [
                                "年産",
                                "共済目的",
                                "日付",
                                "組合等コード",
                                "組合等名",
                                "支所コード",
                                "支所名",
                                "大地区コード",
                                "大地区名",
                                "小地区コード",
                                "小地区名",
                                "引受回",
                                "組合員等コード",
                                "組合員等名",
                                "金融機関コード",
                                "口座番号",
                                "組合員等負担掛金",
                                "一般賦課金",
                                "組合員等割賦課金",
                                "特別防災賦課金",
                                "賦課金計",
                                "調定額",
                                "徴収済額",
                                "今回徴収予定額",
                                "今回徴収額",
                                "還付額",
                                "徴収年月日（還付年月日）",
                                "未納額",
                                "TOTAL徴収額",
                                "徴収予定区分",
                                "自動振替予定",
                                "徴収区分",
                                "徴収理由",
                                "徴収者",
                                "自動振替",
                                "地域集団コード",
                                "地域集団名",
                                "備考"
                            ];
                            // 配列の内容を書き込む
                            writer.Write(CsvUtil.GetLine(saimokuKoumokuName.ToArray()));

                            // 2行目以降の出力
                            // 取得した細目データを読み込む
                            foreach (SaimokuDataRecord saimokuRecord in saimokuDatas)
                            {
                                List<string> saimokuDataRecord =
                                [
                                    batchJoken.JokenNensan,
                                    saimokuRecord.共済目的名称,
                                    // 日付
                                    systemDate.ToString("yyyy/MM/dd"),
                                    kumiaitoCd,
                                    saimokuRecord.組合等正式名称,
                                    saimokuRecord.支所コード,
                                    saimokuRecord.支所名,
                                    saimokuRecord.大地区コード,
                                    saimokuRecord.大地区名,
                                    saimokuRecord.小地区コード,
                                    saimokuRecord.小地区名,
                                    saimokuRecord.引受回 is null ? "" : saimokuRecord.引受回.ToString().PadLeft(2, '0'),
                                    saimokuRecord.組合員等コード,
                                    saimokuRecord.組合員等名,
                                    saimokuRecord.金融機関コード,
                                    saimokuRecord.口座番号,
                                    saimokuRecord.組合員等負担共済掛金.ToString(),
                                    (saimokuRecord.一般賦課金 ?? 0).ToString().PadLeft(8, '0'),
                                    (saimokuRecord.特別賦課金 ?? 0).ToString().PadLeft(8, '0'),
                                    // 特別防災賦課金
                                    ((saimokuRecord.特別賦課金 ?? 0) + (saimokuRecord.防災賦課金 ?? 0)).ToString().PadLeft(8, '0'),
                                    (saimokuRecord.賦課金計 ?? 0).ToString(),
                                    (saimokuRecord.納入額 ?? 0).ToString().PadLeft(8, '0'),
                                    // 徴収済額
                                    ((saimokuRecord.前回迄徴収額 ?? 0) - (saimokuRecord.前回迄引受解除徴収賦課金額 ?? 0)).ToString().PadLeft(8, '0'),
                                    // 今回徴収予定額
                                    ((saimokuRecord.納入額 ?? 0) - (saimokuRecord.前回迄徴収額 ?? 0) - (saimokuRecord.今回迄引受解除徴収賦課金額 ?? 0)).ToString().PadLeft(8, '0'),
                                    // 今回徴収額
                                    (((saimokuRecord.今回迄徴収額 ?? 0) - (saimokuRecord.前回迄徴収額 ?? 0)) > 0 ? 
                                        ((saimokuRecord.今回迄徴収額 ?? 0) - (saimokuRecord.前回迄徴収額 ?? 0)) : 0).ToString().PadLeft(8, '0'),
                                    // 還付額
                                    (((saimokuRecord.納入額 ?? 0) - (saimokuRecord.前回迄徴収額 ?? 0) - (saimokuRecord.今回迄引受解除徴収賦課金額 ?? 0)) < 0 ?
                                        ((saimokuRecord.前回迄徴収額 ?? 0) - (saimokuRecord.今回迄引受解除徴収賦課金額 ?? 0) - (saimokuRecord.納入額 ?? 0)) : 0).ToString().PadLeft(8, '0'),
                                    // 徴収年月日（還付年月日）
                                    (saimokuRecord.納入額 ?? 0) == ((saimokuRecord.今回迄徴収額 ?? 0) - (saimokuRecord.今回迄引受解除徴収賦課金額 ?? 0)) ? 
                                        (saimokuRecord.徴収年月日.HasValue ? ((DateTime)saimokuRecord.徴収年月日).ToString("yyyy/MM/dd") : string.Empty) :  string.Empty,
                                    // 未納額
                                    (((saimokuRecord.納入額 ?? 0) - (saimokuRecord.今回迄徴収額 ?? 0) - (saimokuRecord.今回迄引受解除徴収賦課金額 ?? 0)) > 0 ?
                                        ((saimokuRecord.納入額 ?? 0) - (saimokuRecord.今回迄徴収額 ?? 0) - (saimokuRecord.今回迄引受解除徴収賦課金額 ?? 0)) : 0).ToString().PadLeft(8, '0'),
                                    // TOTAL徴収額
                                    ((saimokuRecord.今回迄徴収額 ?? 0) - (saimokuRecord.今回迄引受解除徴収賦課金額 ?? 0)).ToString().PadLeft(8, '0'),
                                    saimokuRecord.区分名称,
                                    (saimokuRecord.自動振替処理の有無 ?? string.Empty).Equals(JIDO_FURIKAE_CD_ARI) ? "有" : (saimokuRecord.自動振替処理の有無 ?? string.Empty).Equals(JIDO_FURIKAE_CD_NASHI) ? "無" : string.Empty,
                                    saimokuRecord.徴収区分名_表示,
                                    saimokuRecord.徴収理由名,
                                    saimokuRecord.徴収者,
                                    (saimokuRecord.自動振替フラグ ?? string.Empty).Equals(JIDO_FURIKAE_FLG_ARI) ? "有" : (saimokuRecord.自動振替フラグ ?? string.Empty).Equals(JIDO_FURIKAE_FLG_NASHI) ? "無" : string.Empty,
                                    saimokuRecord.地域集団コード,
                                    // 備考
                                    ((saimokuRecord.振込引落区分コード ?? string.Empty).Equals(HIKIOTOSHI_KBN_CD_CASH) ? "現金" : string.Empty) +
                                        ((saimokuRecord.解除フラグ ?? string.Empty).Equals(Core.CoreConst.FLG_ON) ? "解除" : string.Empty)
                                ];

                                // 配列の内容を書き込む
                                writer.Write(CsvUtil.GetLine(saimokuDataRecord.ToArray()));
                            }
                        }
                    }

                    // ８．５．Zip暗号化を行う。
                    // ８．５．１．データ一時出力フォルダ内のファイルを共通部品「ZipUtil.CreateZip」でZip化（暗号化）し
                    Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                    // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                    // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                    // [変数：ZIPファイル格納先パス] とファイル名でパスを登録します。
                    NskCommonUtil.FolderUtil.MoveFile(zipFilePath, zipFolderPath, user_id, nBid);

                    // ８．５．２．一時フォルダを削除する。
                    if (Directory.Exists(tempFolderPath))
                    {
                        NskCommonUtil.FolderUtil.DeleteTempFolder(tempFolderPath);
                    }

                    // ８．６．処理正常終了時
                    // [変数：処理ステータス] に"03"（成功）を設定
                    status = NskCommon.CoreConst.STATUS_SUCCESS;

                    // [変数：エラーメッセージ] に正常終了メッセージを設定
                    // （"MI10005"：処理が正常に終了しました。)
                    errorMessage = MessageUtil.Get("MI10005");
                }

                // ファイル出力終了ログ(END CSV作成 バッチ名)
                logger.Info(
                    Core.CoreConst.LOG_END_KEYWORD + Core.CoreConst.HALF_WIDTH_SPACE +
                    "CSV作成" + Core.CoreConst.HALF_WIDTH_SPACE +
                    "B107080_徴収管理簿");

                // ９．バッチ実行状況更新
                result = SetBatchYoyakuSts(nBid, errorMessage, status, batchYoyakuId, result);

                // ９．３．処理を終了する。
            }
            catch (Exception ex)
            {
                long.TryParse(bid, out nBid);

                // １０．エラー処理
                // １０．１．例外（エクセプション）の場合
                //  [変数：処理ステータス] に"99"（エラー）を設定
                status = NskCommon.CoreConst.STATUS_ERROR;
                // 処理結果（異常：1）
                result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;

                // 各処理でエラーの場合
                if (ex is AppException)
                {
                    // エラーログ設定
                    logger.Error(ex.Message);

                    // ９．バッチ実行状況更新
                    SetBatchYoyakuSts(nBid, ex.Message, status, batchYoyakuId, result);
                }
                // 各処理でのエラー以外（例外）の場合
                else
                {
                    //  [変数：エラーメッセージ] にエラーメッセージを設定
                    // （"MF00001")
                    errorMessage = MessageUtil.Get("MF00001");

                    // 例外の内容をログに出力する。
                    logger.Error(ex.Message + string.Join(string.Empty, new string[]
                    {
                        ex.InnerException == null ? string.Empty : Core.CoreConst.NEW_LINE_SEPARATOR + ex.InnerException.ToString(),
                        string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : Core.CoreConst.NEW_LINE_SEPARATOR + ex.StackTrace
                    }));

                    // １０．２．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
                    string refMessage = string.Empty;
                    BatchUtil.UpdateBatchYoyakuSts(nBid, status, errorMessage, batchYoyakuId, ref refMessage);
                }
            }

            Environment.ExitCode = result;
        }

        /// <summary>
        /// 「バッチ条件情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">変数：バッチ条件ID</param>
        /// <param name="jokenNames">条件名のList</param>
        /// <returns></returns>
        private static List<T01050バッチ条件> GetbatchJoken(NskAppContext dbContext, string jid, List<string> jokenNames)
        {
            List<T01050バッチ条件> batchJokens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && jokenNames.Contains(x.条件名称))
                .ToList();

            return batchJokens;
        }

        /// <summary>
        /// 「都道府県コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        private static int GetTodofukenCdSonzaiJoho(NskAppContext dbContext, string todofukenCd)
        {
            int todofuken = dbContext.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Count();

            return todofuken;
        }

        /// <summary>
        /// 「組合等コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns></returns>
        private static int GetKumiaitoCdSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd)
        {
            int kumiaito = dbContext.VKumiaitos
                 .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd)
                 .Count();

            return kumiaito;
        }

        /// <summary>
        /// 「支所コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns></returns>
        private static int GetShishoCdSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            int shisho = dbContext.VShishoNms
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.ShishoCd == shishoCd)
                .Count();

            return shisho;
        }

        /// <summary>
        /// 共済目的コード存在情報の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="kyosaiMokutekiCd">共済目的コード</param>
        /// <returns></returns>
        private static int GetKyosaiMokutekiCDSonzaiJoho(NskAppContext dbContext, string kyosaiMokutekiCd)
        {
            int kyosaiMokuteki = dbContext.M00010共済目的名称s
                .Where(x => x.共済目的コード == kyosaiMokutekiCd)
                .Count();

            return kyosaiMokuteki;
        }

        /// <summary>
        /// 「大地区コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">大地区コード</param>
        /// <returns></returns>
        private static int GetDaichikuCdSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string daichikuCd)
        {
            int daichiku = dbContext.VDaichikuNms
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.DaichikuCd == daichikuCd)
                .Count();

            return daichiku;
        }

        /// <summary>
        /// 細目データの取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コードコード</param>
        /// <param name="batchJoken">バッチ条件</param>
        /// <returns></returns>
        private static List<SaimokuDataRecord> GetSaimokuData(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd, BatchJoken batchJoken)
        {
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // WITH句
            sql.Append("WITH HJ AS ( ");
            sql.Append("    SELECT");
            sql.Append("        t_12040_組合員等別引受情報.*");
            sql.Append("    FROM");
            sql.Append("        t_12040_組合員等別引受情報");
            sql.Append("    INNER JOIN (");
            sql.Append("        SELECT ");
            sql.Append("            t_00010_引受回.組合等コード");
            sql.Append("            , t_00010_引受回.年産");
            sql.Append("            , t_00010_引受回.共済目的コード");
            sql.Append("            , t_00010_引受回.支所コード");
            sql.Append("            , MAX(t_00010_引受回.引受回) AS \"引受回\" ");
            sql.Append("        FROM");
            sql.Append("            t_00010_引受回");
            sql.Append("        GROUP BY ");
            sql.Append("            t_00010_引受回.組合等コード");
            sql.Append("            , t_00010_引受回.年産");
            sql.Append("            , t_00010_引受回.共済目的コード");
            sql.Append("            , t_00010_引受回.支所コード");
            sql.Append("        ) HK ");
            sql.Append("    ON  t_12040_組合員等別引受情報.組合等コード = HK.組合等コード");
            sql.Append("    AND t_12040_組合員等別引受情報.年産 = HK.年産");
            sql.Append("    AND t_12040_組合員等別引受情報.共済目的コード = HK.共済目的コード");
            sql.Append("    AND t_12040_組合員等別引受情報.支所コード = HK.支所コード");
            sql.Append("    AND t_12040_組合員等別引受情報.引受回 = HK.引受回");
            sql.Append("    WHERE t_12040_組合員等別引受情報.類区分 = '0' ");
            sql.Append("    AND   t_12040_組合員等別引受情報.統計単位地域コード = '0' ");
            sql.Append(") ");

            // 取得項目
            sql.Append("SELECT ");
            sql.Append("    m_00010_共済目的名称.共済目的名称 AS \"共済目的名称\" ");
            sql.Append("    , v_kumiaito.kumiaito_nm AS \"組合等正式名称\" ");
            sql.Append("    , v_shisho_nm.shisho_nm AS \"支所コード\" ");
            sql.Append("    , HJ.支所コード AS \"支所名\" ");
            sql.Append("    , v_daichiku_nm.daichiku_cd AS \"大地区コード\" ");
            sql.Append("    , v_daichiku_nm.daichiku_nm AS \"大地区名\" ");
            sql.Append("    , v_shochiku_nm.shochiku_cd AS \"小地区コード\" ");
            sql.Append("    , v_shochiku_nm.shochiku_nm AS \"小地区名\" ");
            sql.Append("    , HJ.引受回 AS \"引受回\" ");
            sql.Append("    , HJ.組合員等コード AS \"組合員等コード\" ");
            sql.Append("    , NOGYOSHA1.hojin_full_nm AS \"組合員等名\" ");
            sql.Append("    , v_nogyosha_kinyukikan.kinyukikan_cd AS \"金融機関コード\" ");
            sql.Append("    , v_nogyosha_kinyukikan.koza_num AS \"口座番号\" ");
            sql.Append("    , COALESCE(HJ.組合員等負担共済掛金, 0) AS \"組合員等負担共済掛金\" ");
            sql.Append("    , COALESCE(HJ.一般賦課金, 0) AS \"一般賦課金\" ");
            sql.Append("    , COALESCE(HJ.組合員等割, 0) AS \"組合員等割\" ");
            sql.Append("    , COALESCE(HJ.特別賦課金, 0) AS \"特別賦課金\" ");
            sql.Append("    , COALESCE(HJ.防災賦課金, 0) AS \"防災賦課金\" ");
            sql.Append("    , COALESCE(HJ.賦課金計, 0) AS \"賦課金計\" ");
            sql.Append("    , COALESCE(HJ.納入額, 0) AS \"納入額\" ");
            sql.Append("    , COALESCE(CJ.前回迄徴収額, 0) AS \"前回迄徴収額\" ");
            sql.Append("    , COALESCE(CJ.今回迄徴収額, 0) AS \"今回迄徴収額\" ");
            sql.Append("    , COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) AS \"前回迄引受解除徴収賦課金額\" ");
            sql.Append("    , COALESCE(HJ.賦課金計, 0) - COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) AS \"賦課金計差額\" ");
            sql.Append("    , CASE WHEN");
            sql.Append("              HJ.解除フラグ = '1'");
            sql.Append("            THEN");
            sql.Append("              COALESCE(HJ.引受解除返還賦課金額, 0) - COALESCE(HJ.賦課金計, 0) - COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) ");   //t_12040_組合員等別引受情報.引受解除返還賦課金額 -  賦課金計差額
            sql.Append("            ELSE");
            sql.Append("              0");
            sql.Append("      END AS \"今回引受解除徴収賦課金額\" ");
            sql.Append("    , COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) ");    //CJ.前回迄引受解除徴収賦課金額 + 今回引受解除徴収賦課金額
            sql.Append("          + CASE WHEN");
            sql.Append("                    HJ.解除フラグ = '1'");
            sql.Append("                  THEN");
            sql.Append("                    COALESCE(HJ.引受解除返還賦課金額, 0) - COALESCE(HJ.賦課金計, 0) - COALESCE(CJ.前回迄内賦課金, 0) - COALESCE(CJ.前回迄引受解除徴収賦課金額, 0) ");   //t_12040_組合員等別引受情報.引受解除返還賦課金額 -  賦課金計差額
            sql.Append("                  ELSE");
            sql.Append("                    0");
            sql.Append("             END ");
            sql.Append("      AS \"今回迄引受解除徴収賦課金額\" ");
            sql.Append("    , t_12090_組合員等別徴収情報.徴収年月日 AS \"徴収年月日\" ");
            sql.Append("    , v_nogyosha_kinyukikan.furikomi_hikiotoshi_cd AS \"振込引落区分コード\" ");
            sql.Append("    , v_hanyokubun.kbn_nm AS \"区分名称\" ");
            sql.Append("    , v_nogyosha_kinyukikan.jidofurikae_cd AS \"自動振替処理の有無\" ");
            sql.Append("    , t_12090_組合員等別徴収情報.徴収区分コード AS \"徴収区分コード\" ");
            sql.Append("    , v_choshu_kbn.choshu_kbn_display AS \"徴収区分名_表示\" ");
            sql.Append("    , t_12090_組合員等別徴収情報.徴収理由コード AS \"徴収理由コード\" ");
            sql.Append("    , v_choshu_riyu.choshu_riyu_nm AS \"徴収理由名\" ");
            sql.Append("    , t_12090_組合員等別徴収情報.徴収者 AS \"徴収者\" ");
            sql.Append("    , t_12090_組合員等別徴収情報.自動振替フラグ AS \"自動振替フラグ\" ");
            sql.Append("    , t_11010_個人設定.地域集団コード AS \"地域集団コード\" ");
            sql.Append("    , NOGYOSHA2.hojin_full_nm AS \"地域集団名\" ");
            sql.Append("    , HJ.解除フラグ AS \"解除フラグ\" ");

            // 検索テーブル：
            sql.Append("FROM ");
            sql.Append("    HJ ");
            sql.Append("LEFT JOIN ( ");
            sql.Append("    SELECT ");
            sql.Append("        t_12090_組合員等別徴収情報.組合等コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.年産 ");
            sql.Append("        , t_12090_組合員等別徴収情報.共済目的コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.組合員等コード ");
            sql.Append("        , MAX(t_12090_組合員等別徴収情報.引受回) AS \"徴収引受回\" ");
            sql.Append("        , SUM(t_12090_組合員等別徴収情報.徴収金額) AS \"今回迄徴収額\" ");
            sql.Append("        , SUM(CASE WHEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.引受回 < HJ.引受回 ");
            sql.Append("                   THEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.徴収金額 ");
            sql.Append("                   ELSE 0 END) AS \"前回迄徴収額\" ");
            sql.Append("        , SUM(CASE WHEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.引受回 < HJ.引受回 ");
            sql.Append("                   THEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.内賦課金 ");
            sql.Append("                   ELSE 0 END) AS \"前回迄内賦課金\" ");
            sql.Append("        , SUM(CASE WHEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.引受回 < HJ.引受回 ");
            sql.Append("                   THEN ");
            sql.Append("                      t_12090_組合員等別徴収情報.引受解除徴収賦課金額 ");
            sql.Append("                   ELSE 0 END) AS \"前回迄引受解除徴収賦課金額\" ");
            sql.Append("    FROM t_12090_組合員等別徴収情報 ");
            sql.Append("    LEFT JOIN HJ ");
            sql.Append("        ON  t_12090_組合員等別徴収情報.組合等コード = HJ.組合等コード ");
            sql.Append("        AND t_12090_組合員等別徴収情報.年産 = HJ.年産 ");
            sql.Append("        AND t_12090_組合員等別徴収情報.共済目的コード = HJ.共済目的コード ");
            sql.Append("        AND t_12090_組合員等別徴収情報.組合員等コード = HJ.組合員等コード ");
            sql.Append("    GROUP BY ");
            sql.Append("        t_12090_組合員等別徴収情報.組合等コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.年産 ");
            sql.Append("        , t_12090_組合員等別徴収情報.共済目的コード ");
            sql.Append("        , t_12090_組合員等別徴収情報.組合員等コード ");
            sql.Append("    ) AS CJ ");
            sql.Append("    ON HJ.組合等コード = CJ.組合等コード ");
            sql.Append("    AND HJ.年産 = CJ.年産 ");
            sql.Append("    AND HJ.共済目的コード = CJ.共済目的コード ");
            sql.Append("    AND HJ.組合員等コード = CJ.組合員等コード ");
            sql.Append("LEFT JOIN t_12090_組合員等別徴収情報 ");
            sql.Append("    ON CJ.組合等コード = t_12090_組合員等別徴収情報.組合等コード ");
            sql.Append("    AND CJ.年産 = t_12090_組合員等別徴収情報.年産 ");
            sql.Append("    AND CJ.共済目的コード = t_12090_組合員等別徴収情報.共済目的コード ");
            sql.Append("    AND CJ.徴収引受回 = t_12090_組合員等別徴収情報.引受回 ");
            sql.Append("    AND CJ.組合員等コード = t_12090_組合員等別徴収情報.組合員等コード ");
            sql.Append("LEFT JOIN v_nogyosha AS NOGYOSHA1 ");
            sql.Append("    ON HJ.組合等コード = NOGYOSHA1.kumiaito_cd ");
            sql.Append("    AND HJ.組合員等コード = NOGYOSHA1.kumiaiinto_cd ");
            sql.Append("LEFT JOIN v_shisho_nm ");
            sql.Append("    ON HJ.組合等コード = v_shisho_nm.kumiaito_cd ");
            sql.Append("    AND HJ.支所コード = v_shisho_nm.shisho_cd ");
            sql.Append("LEFT JOIN v_daichiku_nm ");
            sql.Append("    ON HJ.組合等コード = v_daichiku_nm.kumiaito_cd ");
            sql.Append("    AND HJ.大地区コード = v_daichiku_nm.daichiku_cd ");
            sql.Append("LEFT JOIN v_shochiku_nm ");
            sql.Append("    ON HJ.組合等コード = v_shochiku_nm.kumiaito_cd ");
            sql.Append("    AND HJ.大地区コード = v_shochiku_nm.daichiku_cd ");
            sql.Append("    AND HJ.小地区コード = v_shochiku_nm.shochiku_cd ");
            sql.Append("LEFT JOIN v_nogyosha_kinyukikan ");
            sql.Append("    ON NOGYOSHA1.nogyosha_id = v_nogyosha_kinyukikan.nogyosha_id ");
            sql.Append("INNER JOIN (");
            sql.Append("    SELECT ");
            sql.Append("       共済事業コード ");
            sql.Append("       , 共済目的コード_fim ");
            sql.Append("       , 振込区分 ");
            sql.Append("    FROM m_00220_共済目的対応 AS KT1 ");
            sql.Append("    WHERE 共済目的コード_NSK = @KyosaiMokutekiCd ");
            sql.Append("    AND 採用順位 = (");
            sql.Append("                    SELECT ");
            sql.Append("                       MIN (採用順位) ");
            sql.Append("                    FROM m_00220_共済目的対応 AS KT2 ");
            sql.Append("                    WHERE KT2.共済事業コード = KT1.共済事業コード ");
            sql.Append("                    AND KT2.共済目的コード_fim = KT1.共済目的コード_fim ");
            sql.Append("         ) ");
            sql.Append("    ) AS KT ");
            sql.Append("    ON  v_nogyosha_kinyukikan.kyosai_jigyo_cd = KT.共済事業コード ");
            sql.Append("    AND v_nogyosha_kinyukikan.kyosai_mokutekito_cd = KT.共済目的コード_fim ");
            sql.Append("    AND v_nogyosha_kinyukikan.furikomi_hikiotoshi_cd = KT.振込区分 ");
            sql.Append("LEFT JOIN v_hanyokubun ");
            sql.Append("    ON v_nogyosha_kinyukikan.furikomi_hikiotoshi_cd = v_hanyokubun.kbn_cd ");
            sql.Append("    AND v_hanyokubun.kbn_sbt = \"furikomi_hikiotoshi_cd\" ");
            sql.Append("INNER JOIN m_00010_共済目的名称 ");
            sql.Append("    ON HJ.共済目的コード = m_00010_共済目的名称.共済目的コード ");
            sql.Append("INNER JOIN v_kumiaito ");
            sql.Append("    ON HJ.組合等コード = v_kumiaito.kumiaito_cd ");
            sql.Append("LEFT JOIN v_choshu_kbn ");
            sql.Append("    ON t_12090_組合員等別徴収情報.徴収区分コード = cast(v_choshu_kbn.choshu_kbn_cd as character varying) ");
            sql.Append("LEFT JOIN v_choshu_riyu ");
            sql.Append("    ON  t_12090_組合員等別徴収情報.徴収区分コード = cast(v_choshu_riyu.choshu_kbn_cd as character varying) ");
            sql.Append("    AND t_12090_組合員等別徴収情報.徴収理由コード = cast(v_choshu_riyu.choshu_riyu_cd as character varying) ");
            sql.Append("LEFT JOIN t_11010_個人設定 ");
            sql.Append("    ON  HJ.組合等コード = t_11010_個人設定.組合等コード ");
            sql.Append("    AND HJ.年産 = t_11010_個人設定.年産 ");
            sql.Append("    AND HJ.共済目的コード = t_11010_個人設定.共済目的コード ");
            sql.Append("    AND HJ.組合員等コード = t_11010_個人設定.組合員等コード ");
            sql.Append("LEFT JOIN v_nogyosha AS NOGYOSHA2 ");
            sql.Append("    ON  t_11010_個人設定.組合等コード = NOGYOSHA2.kumiaito_cd ");
            sql.Append("    AND t_11010_個人設定.組合員等コード = NOGYOSHA2.kumiaiinto_cd ");

            // 検索条件
            sql.Append("WHERE 1 = 1 ");

            // 組合等コード
            if (!string.IsNullOrEmpty(kumiaitoCd))
            {
                sql.Append("AND HJ.組合等コード = @KumiaitoCd ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd", kumiaitoCd));
            }

            // 年産
            if (!string.IsNullOrEmpty(batchJoken.JokenNensan))
            {
                sql.Append("AND HJ.年産 = @Nensan ");
                parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(batchJoken.JokenNensan)));
            }

            // 共済目的コード
            if (!string.IsNullOrEmpty(batchJoken.JokenKyosaiMokutekiCd))
            {
                sql.Append("AND HJ.共済目的コード = @KyosaiMokutekiCd ");
                parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", batchJoken.JokenKyosaiMokutekiCd));
            }

            // 支所
            if (!string.IsNullOrEmpty(shishoCd))
            {
                sql.Append("AND HJ.支所コード = @ShishoCd ");
                parameters.Add(new NpgsqlParameter("@ShishoCd", shishoCd));
            }

            // 大地区
            if (!string.IsNullOrEmpty(batchJoken.JokenDaichiku))
            {
                sql.Append("AND HJ.大地区コード = @DaichikuCd ");
                parameters.Add(new NpgsqlParameter("@DaichikuCd", batchJoken.JokenDaichiku));
            }

            // 小地区（開始）
            if (!string.IsNullOrEmpty(batchJoken.JokenShochikuStart))
            {
                sql.Append("AND HJ.小地区コード >= @ShochikuCdFrom ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdFrom", batchJoken.JokenShochikuStart));
            }

            // 小地区（終了）
            if (!string.IsNullOrEmpty(batchJoken.JokenShochikuEnd))
            {
                sql.Append("AND HJ.小地区コード <= @ShochikuCdTo ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdTo", batchJoken.JokenShochikuEnd));
            }

            // 組合員等コード（開始）
            if (!string.IsNullOrEmpty(batchJoken.JokenKumiaiintoCdStart))
            {
                sql.Append("AND HJ.組合員等コード >= @KumiaiintoCdFrom ");
                parameters.Add(new NpgsqlParameter("@KumiaiintoCdFrom", batchJoken.JokenKumiaiintoCdStart));
            }

            // 組合員等コード（終了）
            if (!string.IsNullOrEmpty(batchJoken.JokenKumiaiintoCdEnd))
            {
                sql.Append("AND HJ.組合員等コード <= @KumiaiintoCdTo ");
                parameters.Add(new NpgsqlParameter("@KumiaiintoCdTo", batchJoken.JokenKumiaiintoCdEnd));
            }

            sql.Append("AND (COALESCE(HJ.納入額, 0) > 0 OR COALESCE(CJ.前回迄徴収額, 0) > 0) ");

            sql.Append($@"ORDER BY ");
            sql.Append($@"  HJ.支所コード       ASC ");
            sql.Append($@"  , HJ.大地区コード   ASC ");
            sql.Append($@"  , HJ.小地区コード   ASC ");
            sql.Append($@"  , HJ.組合員等コード ASC ");

            logger.Info("作成対象とするCSVデータを取得する。");
            List<SaimokuDataRecord> saimokuData = dbContext.Database.SqlQueryRaw<SaimokuDataRecord>(sql.ToString(), parameters.ToArray()).ToList();

            return saimokuData;
        }

        /// <summary>
        /// ９．バッチ実行状況更新
        /// </summary>
        /// <param name="bid">バッチID</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <param name="status">ステータス</param>
        /// <param name="batchYoyakuId">バッチ予約ID</param>
        /// <param name="result">バッチ予約ID</param>
        /// <returns></returns>
        private static int SetBatchYoyakuSts(long bid, string errorMessage, string status, string batchYoyakuId, int result)
        {
            string refMessage = string.Empty;

            // ９．１．『バッチ実行状況更新』インターフェースに引数を渡す。
            // ９．２．『バッチ実行状況更新』インターフェースから戻り値を受け取る。
            if (BatchUtil.UpdateBatchYoyakuSts(bid, status, errorMessage, batchYoyakuId, ref refMessage) == BatchUtil.RET_FAIL)
            {
                // （１）失敗の場合
                logger.Error(string.Format(NskCommon.CoreConst.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, errorMessage));
                result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
            }
            else
            {
                // （２）成功の場合
                logger.Info(string.Format(NskCommon.CoreConst.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid.ToString(), status, errorMessage));
            }

            // ９．３．処理を終了する。
            return result;
        }
    }
}
