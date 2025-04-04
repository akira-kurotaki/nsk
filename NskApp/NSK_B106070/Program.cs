using System.Text;
using Microsoft.EntityFrameworkCore;
using NLog;
using Npgsql;
using Core = CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommon = NskCommonLibrary.Core.Consts;
using NskCommonUtil = NskCommonLibrary.Core.Utility;
using NSK_B106070.Models;
using ModelLibrary.Context;

namespace NSK_B106070
{
    /// <summary>
    /// 組合員等別引受計算結果一覧表
    /// </summary>
    class Program
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 引受方式　「地域インデックス」
        /// </summary>
        private static readonly string HIKIUKE_LOCAL_IDX = "6";

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
        /// 組合員等別引受計算結果一覧表
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
            // バッチID(数値)
            long nBid = 0;
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

                //３．１．６．[変数：バッチ条件のキー情報] が未入力の場合
                if (string.IsNullOrEmpty(jid))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
                }

                //３．１．７．[変数：ユーザIDのキー情報] が未入力の場合
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
                    NskCommon.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,    // 共済目的
                    NskCommon.JoukenNameConst.JOUKEN_SHISHO,                // 支所
                    NskCommon.JoukenNameConst.JOUKEN_DAICHIKU,              // 大地区
                    NskCommon.JoukenNameConst.JOUKEN_SHOCHIKU_START,        // 小地区（開始）
                    NskCommon.JoukenNameConst.JOUKEN_SHOCHIKU_END,          // 小地区（終了）
                    NskCommon.JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,   // 組合員等コードFrom
                    NskCommon.JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,     // 組合員等コードTo
                    NskCommon.JoukenNameConst.JOUKEN_HIKIUKE_KAI,           // 引受回
                    NskCommon.JoukenNameConst.JOUKEN_ORDER_BY_KEY1,         // 出力順1
                    NskCommon.JoukenNameConst.JOUKEN_ORDER_BY1,             // 昇順・降順1
                    NskCommon.JoukenNameConst.JOUKEN_ORDER_BY_KEY2,         // 出力順2
                    NskCommon.JoukenNameConst.JOUKEN_ORDER_BY2,             // 昇順・降順2
                    NskCommon.JoukenNameConst.JOUKEN_ORDER_BY_KEY3,         // 出力順3
                    NskCommon.JoukenNameConst.JOUKEN_ORDER_BY3,             // 昇順・降順3
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
                            case NskCommon.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD:   // 共済目的　※必須
                                batchJoken.JokenKyosaiMokutekiCd = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_SHISHO:               // 支所　※必須
                                batchJoken.JokenShisho = joken.条件値;
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
                            case NskCommon.JoukenNameConst.JOUKEN_HIKIUKE_KAI:          // 引受回
                                batchJoken.JokenHikiukeKai = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_ORDER_BY_KEY1:        // 出力順1
                                batchJoken.JokenShuturyokujun1 = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_ORDER_BY1:            // 昇順・降順1
                                batchJoken.JokenShojunKojun1 = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_ORDER_BY_KEY2:        // 出力順2
                                batchJoken.JokenShuturyokujun2 = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_ORDER_BY2:            // 昇順・降順2
                                batchJoken.JokenShojunKojun2 = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_ORDER_BY_KEY3:        // 出力順3
                                batchJoken.JokenShuturyokujun3 = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_ORDER_BY3:            // 昇順・降順3
                                batchJoken.JokenShojunKojun3 = joken.条件値;
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

                    // ５．２．５．[変数：引受回]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenHikiukeKai))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：引受回)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "引受回"));
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

                    // ６．９．[配列：バッチ条件] から支所コードが取得できた場合
                    if (!string.IsNullOrEmpty(batchJoken.JokenShisho))
                    {
                        // 「検索条件支所コード存在情報」を取得する。
                        int kensakuJokenShisho = GetShishoCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, batchJoken.JokenShisho);

                        // ６．１０．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (kensakuJokenShisho == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005"　引数{0} ：支所コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "支所コード"));
                        }
                    }

                    // ６．１１．[配列：バッチ条件]から大地区コードが取得できた場合
                    if (!string.IsNullOrEmpty(batchJoken.JokenDaichiku))
                    {
                        // 「大地区コード存在情報」を取得する。
                        int daichiku = GetDaichikuCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, batchJoken.JokenDaichiku);

                        // ６．１２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (daichiku == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005　引数{0} ：大地区コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "大地区コード"));
                        }
                    }

                    // ６．１３．[配列：バッチ条件]から小地区（開始）が取得できた場合
                    if (!string.IsNullOrEmpty(batchJoken.JokenShochikuStart))
                    {
                        // 「小地区（開始）存在情報」を取得する。
                        int daichiku = GetShochikuCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, batchJoken.JokenDaichiku, batchJoken.JokenShochikuStart);

                        // ６．１４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (daichiku == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005　引数{0} ：小地区（開始）)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "小地区コード（開始）"));
                        }
                    }

                    // ６．１５．[配列：バッチ条件]から小地区（終了）が取得できた場合
                    if (!string.IsNullOrEmpty(batchJoken.JokenShochikuEnd))
                    {
                        // 「小地区（終了）存在情報」を取得する。
                        int daichiku = GetShochikuCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, batchJoken.JokenDaichiku, batchJoken.JokenShochikuEnd);

                        // ６．１６．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (daichiku == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005　引数{0} ：小地区（終了）)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "小地区コード（終了）"));
                        }
                    }

                    // ７．組合員等別引受計算結果一覧表の取得（ログ出力：あり）
                    // ７．１．「組合員等別引受計算結果一覧表」を取得する。
                    List<SaimokuDataRecord> saimokuDatas = GetSaimokuData(dbContext, todofukenCd, kumiaitoCd, batchJoken);

                    // ７．２．取得した件数が0件の場合
                    if (saimokuDatas.Count == 0)
                    {
                        //  [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME10076" 引数{0}：0)
                        throw new AppException("ME10076", MessageUtil.Get("ME10076", "０"));
                    }

                    // ７．３．取得した件数が0件以外の場合
                    // ８．ファイルを作成する。（ログ出力：あり）
                    // ファイル出力開始ログ（BEGIN CSV作成 バッチ名）
                    logger.Info(
                        Core.CoreConst.LOG_START_KEYWORD + Core.CoreConst.HALF_WIDTH_SPACE +
                        "CSV作成" + Core.CoreConst.HALF_WIDTH_SPACE +
                        "B106070_組合員等別引受計算結果一覧表");

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
                        if (((int)Core.CoreConst.CharacterCode.SJIS).ToString().Equals(batchJoken.JokenMojiCd))
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
                                "#年産",
                                "引受回",
                                "共済目的コード",
                                "共済目的",
                                "支所コード",
                                "支所名",
                                "大地区コード",
                                "大地区名",
                                "小地区コード",
                                "小地区名",
                                "組合員等コード",
                                "組合員等名",
                                "類区分",
                                "類名称",
                                "引受方式",
                                "引受方式名称",
                                "補償割合コード",
                                "補償割合短縮名称",
                                "特約区分",
                                "特約区分名称",
                                "統計単位地域／危険段階地域",
                                "危険段階区分",
                                "共済金額単価",
                                "引受筆数",
                                "引受面積",
                                "基準収穫量",
                                "引受収量",
                                "共済金額",
                                "基準共済掛金",
                                "農家負担共済金",
                                "一般賦課金",
                                "防災賦課金",
                                "特別賦課金",
                                "組合員等割",
                                "共通申請等割引額",
                                "賦課金計",
                                "調定額",
                                "地域集団コード",
                                "地域集団名",
                                "賦課金大量データ取込フラグ",
                                "賦課金大量データ取込日時"
                            ];
                            // 配列の内容を書き込む
                            writer.Write(CsvUtil.GetLine(saimokuKoumokuName.ToArray()));

                            // 2行目以降の出力
                            // 取得した細目データを読み込む
                            foreach (SaimokuDataRecord saimokuRecord in saimokuDatas)
                            {
                                // 統計単位地域／危険段階地域
                                string tokeiKiken = string.Empty;
                                if ((saimokuRecord.引受方式 ?? string.Empty).Equals(HIKIUKE_LOCAL_IDX))
                                {
                                    tokeiKiken = string.IsNullOrEmpty(saimokuRecord.統計単位地域コード) ? string.Empty : saimokuRecord.統計単位地域コード.PadLeft(5, '0');
                                }
                                else if(!string.IsNullOrEmpty(saimokuRecord.危険段階地域区分) && saimokuRecord.危険段階地域区分 != "0")
                                {
                                    tokeiKiken = string.IsNullOrEmpty(saimokuRecord.危険段階地域区分) ? string.Empty : saimokuRecord.危険段階地域区分.PadLeft(3, '0');
                                }
                                    
                                List<string> saimokuDataRecord =
                                [
                                    saimokuRecord.年産.ToString(),
                                    saimokuRecord.引受回.ToString(),
                                    saimokuRecord.共済目的コード,
                                    saimokuRecord.共済目的,
                                    saimokuRecord.支所コード,
                                    saimokuRecord.支所名,
                                    saimokuRecord.大地区コード,
                                    saimokuRecord.大地区名,
                                    saimokuRecord.小地区コード,
                                    saimokuRecord.小地区名,
                                    saimokuRecord.組合員等コード,
                                    saimokuRecord.組合員等氏名漢字,
                                    saimokuRecord.類区分,
                                    saimokuRecord.類区分名,
                                    saimokuRecord.引受方式,
                                    saimokuRecord.引受方式名,
                                    saimokuRecord.補償割合コード,
                                    saimokuRecord.補償割合短縮名称,
                                    saimokuRecord.特約区分,
                                    saimokuRecord.特約区分名称,
                                    tokeiKiken,
                                    saimokuRecord.統計単位地域コード,
                                    saimokuRecord.危険段階地域区分,
                                    saimokuRecord.危険段階区分,
                                    saimokuRecord.共済金額単価.ToString(),
                                    saimokuRecord.引受筆数.ToString(),
                                    saimokuRecord.引受面積.ToString(),
                                    saimokuRecord.基準収穫量.ToString(),
                                    saimokuRecord.引受収量.ToString(),
                                    saimokuRecord.共済金額.ToString(),
                                    saimokuRecord.基準共済掛金.ToString(),
                                    saimokuRecord.農家負担共済掛金.ToString(),
                                    saimokuRecord.一般賦課金.ToString(),
                                    saimokuRecord.防災賦課金.ToString(),
                                    saimokuRecord.特別賦課金.ToString(),
                                    saimokuRecord.組合員等割.ToString(),
                                    saimokuRecord.共通申請等割引額.ToString(),
                                    saimokuRecord.賦課金計.ToString(),
                                    saimokuRecord.調定額.ToString(),
                                    saimokuRecord.地域集団コード,
                                    saimokuRecord.地域集団名,
                                    saimokuRecord.賦課金大量データ取込フラグ,
                                    (saimokuRecord.賦課金大量データ取込日時.HasValue ? ((DateTime)saimokuRecord.賦課金大量データ取込日時).ToString("yyyy/MM/dd HH:mm:ss") : string.Empty).ToString()
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
                    "B106070_組合員等別引受計算結果一覧表");

                // ９．バッチ実行状況更新
                result = SetBatchYoyakuSts(nBid, errorMessage, status, batchYoyakuId, result);
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
        /// 「小地区コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="daichikuCd">小地区コード</param>
        /// <param name="shochikuCd">小地区コード</param>
        /// <returns></returns>
        private static int GetShochikuCdSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string daichikuCd, string shochikuCd)
        {
            int daichiku = dbContext.VShochikuNms
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.DaichikuCd == daichikuCd && x.ShochikuCd == shochikuCd)
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
        private static List<SaimokuDataRecord> GetSaimokuData(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, BatchJoken batchJoken)
        {
            StringBuilder sql = new();

            sql.Append($@"SELECT ");
            sql.Append($@"       @バッチ条件年産 AS 年産 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.引受回 AS 引受回 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.共済目的コード AS 共済目的コード ");
            sql.Append($@"       , m_00010_共済目的名称.共済目的名称 AS 共済目的 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.支所コード AS 支所コード ");
            sql.Append($@"       , v_shisho_nm.shisho_nm AS 支所名 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.大地区コード AS 大地区コード ");
            sql.Append($@"       , v_daichiku_nm.daichiku_nm AS 大地区名 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.小地区コード AS 小地区コード ");
            sql.Append($@"       , v_shochiku_nm.shochiku_nm AS 小地区名 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.組合員等コード AS 組合員等コード ");
            sql.Append($@"       , v_nogyosha.hojin_full_nm AS 組合員等氏名漢字 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.類区分 AS 類区分 ");
            sql.Append($@"       , m_00020_類名称.類短縮名称 AS 類区分名 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.引受方式 AS 引受方式 ");
            sql.Append($@"       , m_10080_引受方式名称.引受方式名称 AS 引受方式名 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.補償割合コード AS 補償割合コード ");
            sql.Append($@"       , m_20030_補償割合名称.補償割合短縮名称 AS 補償割合短縮名称 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.特約区分 AS 特約区分 ");
            sql.Append($@"       , m_10100_特約区分名称.特約区分名称 AS 特約区分名称 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.統計単位地域コード AS 統計単位地域コード ");
            sql.Append($@"       , t_12040_組合員等別引受情報.危険段階地域区分 AS 危険段階地域区分 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.危険段階区分 AS 危険段階区分 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.共済金額単価 AS 共済金額単価 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.引受筆数 AS 引受筆数 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.引受面積計 AS 引受面積 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.基準収穫量計 AS 基準収穫量 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.引受収量 AS 引受収量 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.共済金額 AS 共済金額 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.基準共済掛金 AS 基準共済掛金 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.組合員等負担共済掛金 AS 農家負担共済掛金 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.一般賦課金 AS 一般賦課金 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.防災賦課金 AS 防災賦課金 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.特別賦課金 AS 特別賦課金 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.組合員等割 AS 組合員等割 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.共通申請等割引額 AS 共通申請等割引額 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.賦課金計 AS 賦課金計 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.納入額 AS 調定額 ");
            sql.Append($@"       , t_11010_個人設定.地域集団コード AS 地域集団コード ");
            sql.Append($@"       , 地域集団.daihyosha_nm AS 地域集団名 ");
            sql.Append($@"       , t_12040_組合員等別引受情報.賦課金大量データ取込フラグ AS 賦課金大量データ取込フラグ ");
            sql.Append($@"       , t_12040_組合員等別引受情報.賦課金大量データ取込日時 AS 賦課金大量データ取込日時 ");
            sql.Append($@"   FROM ");
            sql.Append($@"       t_12040_組合員等別引受情報 ");
            sql.Append($@"       LEFT JOIN m_00010_共済目的名称 ");
            sql.Append($@"           ON m_00010_共済目的名称.共済目的コード = t_12040_組合員等別引受情報.共済目的コード ");
            sql.Append($@"       LEFT JOIN m_00020_類名称 ");
            sql.Append($@"           ON m_00020_類名称.共済目的コード = t_12040_組合員等別引受情報.共済目的コード ");
            sql.Append($@"           AND m_00020_類名称.類区分 = t_12040_組合員等別引受情報.類区分 ");
            sql.Append($@"       LEFT JOIN v_nogyosha ");
            sql.Append($@"           ON v_nogyosha.kumiaito_cd = t_12040_組合員等別引受情報.組合等コード ");
            sql.Append($@"           AND v_nogyosha.kumiaiinto_cd = t_12040_組合員等別引受情報.組合員等コード ");
            sql.Append($@"       LEFT JOIN v_shisho_nm ");
            sql.Append($@"           ON v_shisho_nm.kumiaito_cd = t_12040_組合員等別引受情報.組合等コード ");
            sql.Append($@"           AND v_shisho_nm.shisho_cd = t_12040_組合員等別引受情報.支所コード ");
            sql.Append($@"       LEFT JOIN v_daichiku_nm ");
            sql.Append($@"           ON v_daichiku_nm.kumiaito_cd = t_12040_組合員等別引受情報.組合等コード ");
            sql.Append($@"           AND v_daichiku_nm.daichiku_cd = t_12040_組合員等別引受情報.大地区コード ");
            sql.Append($@"       LEFT JOIN v_shochiku_nm ");
            sql.Append($@"           ON v_shochiku_nm.kumiaito_cd = t_12040_組合員等別引受情報.組合等コード ");
            sql.Append($@"           AND v_shochiku_nm.daichiku_cd = t_12040_組合員等別引受情報.大地区コード ");
            sql.Append($@"           AND v_shochiku_nm.shochiku_cd = t_12040_組合員等別引受情報.小地区コード ");
            sql.Append($@"       LEFT JOIN m_10080_引受方式名称 ");
            sql.Append($@"           ON m_10080_引受方式名称.引受方式 = t_12040_組合員等別引受情報.引受方式 ");
            sql.Append($@"       LEFT JOIN m_20030_補償割合名称 ");
            sql.Append($@"           ON m_20030_補償割合名称.補償割合コード = t_12040_組合員等別引受情報.補償割合コード ");
            sql.Append($@"       LEFT JOIN m_10100_特約区分名称 ");
            sql.Append($@"           ON m_10100_特約区分名称.特約区分 = t_12040_組合員等別引受情報.特約区分 ");
            sql.Append($@"       LEFT JOIN t_11010_個人設定 ");
            sql.Append($@"           ON t_11010_個人設定.組合等コード = t_12040_組合員等別引受情報.組合等コード ");
            sql.Append($@"           AND t_11010_個人設定.年産 = t_12040_組合員等別引受情報.年産 ");
            sql.Append($@"           AND t_11010_個人設定.共済目的コード = t_12040_組合員等別引受情報.共済目的コード ");
            sql.Append($@"           AND t_11010_個人設定.組合員等コード = t_12040_組合員等別引受情報.組合員等コード ");
            sql.Append($@"       LEFT JOIN v_nogyosha AS 地域集団");
            sql.Append($@"           ON v_nogyosha.kumiaito_cd = t_11010_個人設定.組合等コード ");
            sql.Append($@"           AND v_nogyosha.kumiaiinto_cd = t_11010_個人設定.地域集団コード ");
            sql.Append($@"   WHERE ");
            sql.Append($@"    1 = 1 ");
            sql.Append($@"    AND t_12040_組合員等別引受情報.組合等コード = @組合等コード ");
            sql.Append($@"    AND t_12040_組合員等別引受情報.年産 = @バッチ条件年産 ");
            sql.Append($@"    AND t_12040_組合員等別引受情報.引受回 = @バッチ条件引受回 ");
            sql.Append($@"    AND t_12040_組合員等別引受情報.共済目的コード = @バッチ条件共済目的コード ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件支所コード <> '00' AND @バッチ条件支所コード <> '' THEN t_12040_組合員等別引受情報.支所コード = @バッチ条件支所コード ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件大地区コード <> '' THEN t_12040_組合員等別引受情報.大地区コード = @バッチ条件大地区コード ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件小地区From <> '' THEN t_12040_組合員等別引受情報.小地区コード >= @バッチ条件小地区From ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件小地区To <> '' THEN t_12040_組合員等別引受情報.小地区コード <= @バッチ条件小地区To ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件組合員等コードFrom <> '' THEN t_12040_組合員等別引受情報.組合員等コード >= @バッチ条件組合員等コードFrom ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");
            sql.Append($@"    AND CASE ");
            sql.Append($@"        WHEN @バッチ条件組合員等コードTo <> '' THEN t_12040_組合員等別引受情報.組合員等コード <= @バッチ条件組合員等コードTo ");
            sql.Append($@"        ELSE 1 = 1 ");
            sql.Append($@"    END ");

            if (!(String.IsNullOrEmpty(batchJoken.JokenShuturyokujun1) && String.IsNullOrEmpty(batchJoken.JokenShuturyokujun2) && String.IsNullOrEmpty(batchJoken.JokenShuturyokujun3)))
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

                if (sortOrders.Count == 0)
                {
                    sql.Append($@"  t_12040_組合員等別引受情報.組合等コード ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.年産 ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.共済目的コード ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.類区分 ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.支所コード ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.大地区コード ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.小地区コード ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.収量等級コード ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.品種コード ASC, ");
                    sql.Append($@"  t_12040_組合員等別引受情報.参酌コード ASC ");
                }
                else
                {
                    foreach (SortOrder sort in sortOrders)
                    {
                        if (!String.IsNullOrEmpty(sort.OrderByKey))
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                            }
                            else
                            {
                                sql.Append($"   , ");
                            }

                            if (sort.OrderByKey == batchJoken.JokenShuturyokujun1)
                            {
                                sql.Append($"   @出力順1 ");
                            }
                            else if (sort.OrderByKey == batchJoken.JokenShuturyokujun2)
                            {
                                sql.Append($"   @出力順2 ");
                            }
                            else if (sort.OrderByKey == batchJoken.JokenShuturyokujun3)
                            {
                                sql.Append($"   @出力順3 ");
                            }

                            switch ((Core.CoreConst.SortOrder)int.Parse(sort.OrderBy))
                            {
                                case Core.CoreConst.SortOrder.ASC:
                                    sql.Append($"   ASC ");
                                    break;
                                case Core.CoreConst.SortOrder.DESC:
                                    sql.Append($"   DESC ");
                                    break;
                            }
                        }
                    }
                }
            }

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("バッチ条件年産", int.Parse(batchJoken.JokenNensan)),
                new("バッチ条件共済目的コード", batchJoken.JokenKyosaiMokutekiCd),
                new("バッチ条件組合員等コードFrom", batchJoken.JokenKumiaiintoCdStart),
                new("バッチ条件組合員等コードTo", batchJoken.JokenKumiaiintoCdEnd),
                new("バッチ条件大地区コード", batchJoken.JokenDaichiku),
                new("バッチ条件小地区From", batchJoken.JokenShochikuStart),
                new("バッチ条件小地区To", batchJoken.JokenShochikuEnd),
                new("バッチ条件支所コード", batchJoken.JokenShisho),
                new("バッチ条件引受回", int.Parse(batchJoken.JokenHikiukeKai)),
                new("都道府県コード", todofukenCd),
                new("組合等コード", kumiaitoCd),
                new("出力順1", batchJoken.JokenShuturyokujun1),
                new("出力順2", batchJoken.JokenShuturyokujun2),
                new("出力順3", batchJoken.JokenShuturyokujun3)
            ];

            // SQLのクエリ結果をListに格納する
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
