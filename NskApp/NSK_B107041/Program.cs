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
using NSK_B107041.Models;
using ModelLibrary.Context;
using Npgsql.Internal.Postgres;
using NpgsqlTypes;

namespace NSK_B107041
{
    /// <summary>
    /// 消込み処理（自動）（インタフェース）
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
            // Npgsql 6.0での「timestamp with time zone」非互換対応
            // Npgsql 6.0より前の動作に戻す
            // https://www.npgsql.org/doc/types/datetime.html#timestamps-and-timezones
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // SJIS(Shift_JIS)を使用可能にする
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 消込み処理（自動）（インタフェース）
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
                    BatchId = nBid
                };

                SystemCommonContext db = new();
                ModelLibrary.Models.TBatchYoyaku? batchYoyaku = db.TBatchYoyakus.FirstOrDefault(x =>
                    (x.BatchId == param.BatchId) &&
                    (x.DeleteFlg == BatchUtil.DELETE_FLG_NOT_DELETED));

                // バッチ予約が存在する場合
                if (batchYoyaku != null)
                {
                    if (batchYoyaku.BatchId == nBid)
                    {
                        // [引数：バッチID]に一致する場合
                        // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                        // バッチ予約ユーザID = バッチ予約情報.予約ユーザID
                        batchYoyakuId = batchYoyaku.BatchYoyakuId;
                    }
                    else
                    {
                        // [引数：バッチID]に一致するバッチ予約状況が取得できない場合、
                        // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                        //（"ME01645" 引数{0} ：パラメータの取得)
                        throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                    }
                }
                else
                {
                    // バッチ予約が存在しない場合、
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
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

                //５．バッチ条件を取得
                //５．１．バッチ条件情報の取得
                //５．１．１．条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
                List<string> jokenNames =
                [
                    NskCommon.JoukenNameConst.JOUKEN_NENSAN,                // 年産
                    NskCommon.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,    // 共済目的コード
                    NskCommon.JoukenNameConst.JOUKEN_SHISHO,                // 支所
                    NskCommon.JoukenNameConst.JOUKEN_HIKIUKE_KAI,           // 引受回
                    NskCommon.JoukenNameConst.JOUKEN_TAISHO_FURIKAE_DATE,   // 対象データ振替日
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
                                batchJoken.JokenKyosaiMokutekitoCd = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_SHISHO:                    // 支所　※必須
                                batchJoken.JokenShisho = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_HIKIUKE_KAI:          // 引受回
                                batchJoken.JokenHikiukeKai = joken.条件値;
                                break;
                            case NskCommon.JoukenNameConst.JOUKEN_TAISHO_FURIKAE_DATE:  // 対象データ振替日
                                batchJoken.JokenTaishoFurikaeDate = joken.条件値;
                                break;
                        }
                    }

                    // ５．２．２．[変数：条件_年産]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenNensan))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：条件_年産)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_年産"));
                    }

                    // ５．２．３．[変数：条件_共済目的コード]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenKyosaiMokutekitoCd))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：条件_共済目的コード)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_共済目的コード"));
                    }

                    // ５．２．４．[変数：条件_支所コード]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenShisho))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：条件_支所コード)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_支所コード"));
                    }

                    // ５．２．５．[変数：条件_引受回]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenHikiukeKai))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：条件_引受回)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_引受回"));
                    }

                    // ５．２．６．[変数：条件_対象データ振替日]がnullまたは空文字の場合
                    if (string.IsNullOrEmpty(batchJoken.JokenTaishoFurikaeDate))
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME01054"　引数{0} ：条件_対象データ振替日)
                        throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_対象データ振替日"));
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
                    if (!string.IsNullOrEmpty(batchJoken.JokenKyosaiMokutekitoCd))
                    {
                        // 「共済目的コード存在情報」を取得する。
                        int kyosaiMokuteki = GetKyosaiMokutekiCDSonzaiJoho(dbContext, batchJoken.JokenKyosaiMokutekitoCd);

                        // ６．８．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (kyosaiMokuteki == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005"　引数{0} ：条件_共済目的コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_共済目的コード"));
                        }
                    }

                    // ６．９．[配列：バッチ条件] から支所コードが取得できた場合
                    if (!string.IsNullOrEmpty(batchJoken.JokenShisho))
                    {
                        // 「検索条件支所コード存在情報」を取得する。
                        int kensakuJokenShisho = GetKensakuJokenShishoCDSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, batchJoken.JokenShisho);

                        // ６．１０．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (kensakuJokenShisho == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005"　引数{0} ：条件_支所コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "条件_支所コード"));
                        }
                    }

                    // ７．口座振替結果を取得
                    // ７．１．「口座振替結果」を取得する。
                    List<SubSaimokuDataRecord> subSaimokuDatas = GetSubSaimokuData(dbContext, kumiaitoCd, batchJoken);

                    // ７．２．「口座振替結果」の取得した件数が0件の場合
                    if (subSaimokuDatas.Count == 0)
                    {
                        //  [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME10076" 引数{0}：0)
                        throw new AppException("ME10076", MessageUtil.Get("ME10076", "０"));
                    }

                    // ７．３．「口座振替結果」の取得した件数が0件以外の場合
                    foreach (SubSaimokuDataRecord subSimokuRecord in subSaimokuDatas)
                    {
                        // ８．消込み処理実施
                        List<MainSaimokuDataRecord> mainSaimokuDatas = GetMainSaimokuData(dbContext, kumiaitoCd, subSimokuRecord.組合員等コード, batchJoken);

                        foreach (MainSaimokuDataRecord mainSaimokuRecord in mainSaimokuDatas)
                        {
                            // ８．１．取得した「組合員等別引受・徴収情報」で、引受回 = [変数：選択引受回] のレコードに徴収情報が存在する場合 （徴収組合員等コードが存在する場合）
                            if (string.IsNullOrEmpty(mainSaimokuRecord.組合員等コード) && !mainSaimokuRecord.引受回.Equals(batchJoken.JokenHikiukeKai)){
                                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                                // （"ME10070"　引数{0} ：条件_支所コード)
                                throw new AppException("ME10070", MessageUtil.Get("ME10070", mainSaimokuRecord.組合員等コード, subSimokuRecord.振込金額.ToString()));
                            }

                            // ８．２．取得した「組合員等別引受・徴収情報」で、引受回 > [変数：選択引受回] のレコードに徴収情報が存在する場合（徴収組合員等コードが存在する場合）
                            if (string.IsNullOrEmpty(mainSaimokuRecord.組合員等コード) && mainSaimokuRecord.引受回 <= int.Parse(batchJoken.JokenHikiukeKai))
                            {
                                // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                                // （"ME10069"　引数{0} ：条件_支所コード)
                                throw new AppException("ME10069", MessageUtil.Get("ME10069", mainSaimokuRecord.組合員等コード, subSimokuRecord.振込金額.ToString()));
                            }

                            // ８．３．上記エラーがない場合、取得した「組合員等別引受・徴収情報」から、以下の計算により今回の徴収金額を求める。
                            decimal 過去の徴収金額計 = mainSaimokuDatas.Where(x => x.引受回 != int.Parse(batchJoken.JokenHikiukeKai)).Sum(x => x.徴収金額);
                            decimal 引受解除徴収賦課金額計 = mainSaimokuRecord.引受解除返還賦課金額 - (mainSaimokuRecord.賦課金計 
                                - mainSaimokuDatas.Where(x => x.引受回 != int.Parse(batchJoken.JokenHikiukeKai)).Sum(x => x.内賦課金)
                                - mainSaimokuDatas.Where(x => x.引受回 != int.Parse(batchJoken.JokenHikiukeKai)).Sum(x => x.引受解除徴収賦課金額));
                            decimal 今回の徴収金額 = mainSaimokuRecord.納入額 - (過去の徴収金額計 - 引受解除徴収賦課金額計);
                            if (今回の徴収金額 == subSimokuRecord.振込金額)
                            {
                                decimal 組合員等負担共済掛金差額 = mainSaimokuRecord.組合員等負担共済掛金
                                    - mainSaimokuDatas.Where(x => x.引受回 != int.Parse(batchJoken.JokenHikiukeKai)).Sum(x => x.内農家負担掛金);
                                decimal 賦課金計差額 = mainSaimokuRecord.賦課金計 - (
                                    - mainSaimokuDatas.Where(x => x.引受回 < int.Parse(batchJoken.JokenHikiukeKai)).Sum(x => x.内賦課金)
                                    - mainSaimokuDatas.Where(x => x.引受回 < int.Parse(batchJoken.JokenHikiukeKai)).Sum(x => x.引受解除徴収賦課金額));

                                int 実行結果 = Add消込み情報(dbContext, kumiaitoCd, subSimokuRecord.組合員等コード, batchYoyakuId, batchJoken, subSimokuRecord.振込金額, 組合員等負担共済掛金差額, 賦課金計差額);
                            }
                        }
                    }

                    // ８．４．処理正常終了時
                    // [変数：処理ステータス] に"03"（成功）を設定
                    status = NskCommon.CoreConst.STATUS_SUCCESS;

                    // [変数：エラーメッセージ] に正常終了メッセージを設定
                    // （"MI10005"：処理が正常に終了しました。)
                    errorMessage = MessageUtil.Get("MI10005");
                }

                // ９．バッチ実行状況更新
                string refMessage = string.Empty;

                // ９．１．『バッチ実行状況更新』インターフェースに引数を渡す。
                // ９．２．『バッチ実行状況更新』インターフェースから戻り値を受け取る。
                if (BatchUtil.UpdateBatchYoyakuSts(nBid, status, errorMessage, batchYoyakuId, ref refMessage) == BatchUtil.RET_FAIL)
                {
                    // （１）失敗の場合
                    logger.Error(refMessage);
                    logger.Error(string.Format(NskCommon.CoreConst.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, errorMessage));
                    result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
                }
                else
                {
                    // （２）成功の場合
                    logger.Info(string.Format(NskCommon.CoreConst.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, errorMessage));
                    result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
                }

                // ９．３．処理を終了する。
            }
            catch (Exception ex)
            {
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
                }

                // １０．２．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
                string refMessage = string.Empty;
                BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), status, errorMessage, batchYoyakuId, ref refMessage);
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
        /// 検索条件：支所コード存在情報の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns></returns>
        private static int GetKensakuJokenShishoCDSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            int shisho = 0;

            if (shishoCd != "00")
            {
                shisho = dbContext.VShishoNms
                    .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.ShishoCd == shishoCd)
                    .Count();
            }
            else
            {
                shisho = dbContext.VShishoNms
                    .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd
                        && dbContext.VShishoNms
                            .Where(y => y.TodofukenCd == todofukenCd && y.KumiaitoCd == kumiaitoCd && y.ShishoCd != "00")
                            .Select(y => y.ShishoCd)
                            .ToList()
                    .Contains(x.ShishoCd))
                    .Count();
            }

            return shisho;
        }

        /// <summary>
        /// 細目データの取得（組合員等別引受・徴収情報）
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コードコード</param>
        /// <param name="batchJoken">バッチ条件</param>
        /// <returns></returns>
        private static List<MainSaimokuDataRecord> GetMainSaimokuData(NskAppContext dbContext, string kumiaitoCd, string subKumiaitoCd, BatchJoken batchJoken)
        {
            StringBuilder sql = new();

            sql.Append($@"SELECT ");
            sql.Append($@"    HJ.組合員等コード, ");
            sql.Append($@"    HJ.引受回, ");
            sql.Append($@"    COALESCE(HJ.組合員等負担共済掛金, 0) AS 組合員等負担共済掛金, ");
            sql.Append($@"    COALESCE(HJ.賦課金計, 0) AS 賦課金計, ");
            sql.Append($@"    COALESCE(HJ.引受解除返還賦課金額, 0) AS 引受解除返還賦課金額, ");
            sql.Append($@"    COALESCE(HJ.納入額, 0) AS 納入額, ");
            sql.Append($@"    CJ.組合員等コード AS 徴収組合員等コード, ");
            sql.Append($@"    COALESCE(CJ.徴収金額, 0) AS 徴収金額, ");
            sql.Append($@"    COALESCE(CJ.内農家負担掛金, 0) AS 内農家負担掛金, ");
            sql.Append($@"    COALESCE(CJ.内賦課金, 0) AS 内賦課金, ");
            sql.Append($@"    COALESCE(CJ.引受解除徴収賦課金額, 0) AS 引受解除徴収賦課金額 ");
            sql.Append($@"FROM ");
            sql.Append($@"    t_12040_組合員等別引受情報  HJ ");
            sql.Append($@"    LEFT JOIN t_12090_組合員等別徴収情報  CJ ");
            sql.Append($@"        ON HJ.組合等コード = CJ.組合等コード ");
            sql.Append($@"        AND HJ.年産 = CJ.年産 ");
            sql.Append($@"        AND HJ.共済目的コード = CJ.共済目的コード ");
            sql.Append($@"        AND HJ.引受回 = CJ.引受回 ");
            sql.Append($@"        AND HJ.組合員等コード = CJ.組合員等コード ");
            sql.Append($@"WHERE ");
            sql.Append($@"    1 = 1 ");
            sql.Append($@"    AND HJ.組合等コード = @組合等コード ");
            sql.Append($@"    AND HJ.年産 = @バッチ条件年産 ");
            sql.Append($@"    AND HJ.共済目的コード = @バッチ条件共済目的コード ");
            sql.Append($@"    AND HJ.支所コード = @バッチ条件支所コード ");
            sql.Append($@"    AND HJ.組合員等コード = @口座振替結果組合員等コード ");
            sql.Append($@"    AND HJ.類区分 = '0' ");
            sql.Append($@"    AND HJ.統計単位地域コード = '0' ");
            sql.Append($@"ORDER BY ");
            sql.Append($@"    HJ.引受回 ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("バッチ条件年産", int.Parse(batchJoken.JokenNensan)),
                new("バッチ条件共済目的コード", batchJoken.JokenKyosaiMokutekitoCd),
                new("バッチ条件支所コード", batchJoken.JokenShisho),
                new("組合等コード", kumiaitoCd),
                new("口座振替結果組合員等コード", subKumiaitoCd),
            ];

            // SQLのクエリ結果をListに格納する
            List<MainSaimokuDataRecord> saimokuData = dbContext.Database.SqlQueryRaw<MainSaimokuDataRecord>(sql.ToString(), parameters.ToArray()).ToList();

            return saimokuData;
        }

        /// <summary>
        /// 細目データの取得（口座振替結果）
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <returns></returns>
        private static List<SubSaimokuDataRecord> GetSubSaimokuData(NskAppContext dbContext, string kumiaitoCd, BatchJoken batchJoken)
        {
            StringBuilder sql = new();

            sql.Append($@"SELECT ");
            sql.Append($@"    T1.組合員等コード, ");
            sql.Append($@"    COALESCE(T1.振込金額, 0) AS 振込金額 ");
            sql.Append($@"FROM ");
            sql.Append($@"    v_30020_口座振替結果 T1 ");
            sql.Append($@"WHERE ");
            sql.Append($@"    1 = 1 ");
            sql.Append($@"    AND T1.組合等コード = @組合等コード ");
            sql.Append($@"    AND T1.年産 = @バッチ条件年産 ");
            sql.Append($@"    AND T1.共済目的コード = @バッチ条件共済目的コード ");
            sql.Append($@"    AND 共済事業コード = '01' ");
            sql.Append($@"    AND 金額区分 = '1' ");
            sql.Append($@"    AND 振替日 = @バッチ条件対象データ振替日 ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("組合等コード", kumiaitoCd),
                new("バッチ条件年産", int.Parse(batchJoken.JokenNensan)),
                new("バッチ条件共済目的コード", batchJoken.JokenKyosaiMokutekitoCd),
                new("バッチ条件対象データ振替日",NpgsqlDbType.TimestampTz){ Value = DateTime.Parse(batchJoken.JokenTaishoFurikaeDate) }
            ];
            var aaa = DateTime.Parse(batchJoken.JokenTaishoFurikaeDate);

            // SQLのクエリ結果をListに格納する
            List<SubSaimokuDataRecord> saimokuData = dbContext.Database.SqlQueryRaw<SubSaimokuDataRecord>(sql.ToString(), parameters.ToArray()).ToList();

            return saimokuData;
        }

        private static int Add消込み情報(NskAppContext dbContext, string kumiaitoCd, string subKumiaitoCd, string batchYoyakuId, BatchJoken batchJoken, decimal 徴収金額, decimal 内農家負担掛金, decimal 内賦課金)
        {
            StringBuilder sql = new();

            sql.Append($@"INSERT INTO t_12090_組合員等別徴収情報 ( ");
            sql.Append($@"    組合等コード, ");
            sql.Append($@"    年産, ");
            sql.Append($@"    共済目的コード, ");
            sql.Append($@"    引受回, ");
            sql.Append($@"    組合員等コード, ");
            sql.Append($@"    徴収区分コード, ");
            sql.Append($@"    自動振替フラグ, ");
            sql.Append($@"    徴収理由コード, ");
            sql.Append($@"    徴収年月日, ");
            sql.Append($@"    徴収者, ");
            sql.Append($@"    徴収金額, ");
            sql.Append($@"    内農家負担掛金, ");
            sql.Append($@"    内賦課金, ");
            sql.Append($@"    引受解除徴収賦課金額, ");
            sql.Append($@"    登録日時, ");
            sql.Append($@"    登録ユーザid, ");
            sql.Append($@"    更新日時, ");
            sql.Append($@"    更新ユーザid ");
            sql.Append($@") VALUES ( ");
            sql.Append($@"    @組合等コード, ");
            sql.Append($@"    @バッチ条件年産, ");
            sql.Append($@"    @バッチ条件共済目的コード, ");
            sql.Append($@"    @バッチ条件引受回, ");
            sql.Append($@"    @組合員等コード, ");
            sql.Append($@"    '1', ");
            sql.Append($@"    '1', ");
            sql.Append($@"    '0', ");
            sql.Append($@"    @バッチ条件対象データ振替日, ");
            sql.Append($@"    null, ");
            sql.Append($@"    @口座振替結果振込金額, ");
            sql.Append($@"    @組合員等負担共済掛金差額, ");
            sql.Append($@"    @賦課金計差額, ");
            sql.Append($@"    0, ");
            sql.Append($@"    @システム日時, ");
            sql.Append($@"    @ユーザid, ");
            sql.Append($@"    @システム日時, ");
            sql.Append($@"    @ユーザid ");
            sql.Append($@") ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("組合等コード", kumiaitoCd),
                new("組合員等コード", subKumiaitoCd),
                new("バッチ条件年産", int.Parse(batchJoken.JokenNensan)),
                new("バッチ条件共済目的コード", batchJoken.JokenKyosaiMokutekitoCd),
                new("バッチ条件引受回", int.Parse(batchJoken.JokenHikiukeKai)),
                new("バッチ条件対象データ振替日",NpgsqlDbType.TimestampTz){ Value = DateTime.Parse(batchJoken.JokenTaishoFurikaeDate) },
                new("口座振替結果振込金額", 徴収金額),
                new("組合員等負担共済掛金差額", 内農家負担掛金),
                new("賦課金計差額", 内賦課金),
                new("ユーザid", batchYoyakuId),
                new("システム日時", DateUtil.GetSysDateTime())
            ];

            int result = dbContext.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());

            return result;
        }
    }
}
