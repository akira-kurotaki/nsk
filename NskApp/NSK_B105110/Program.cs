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
using NSK_B105110.Models;

namespace NSK_B105110
{
    /// <summary>
    /// 組合員等各種条件設定エラーリスト
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
        /// 組合員等各種条件設定エラーリスト
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

                // ３．引数のチェック
                // ３．１．必須チェック
                // ３．１．１．以下の引数が未入力の場合、[変数：エラーメッセージ]を設定し、「７.」へ進む。
                if (string.IsNullOrEmpty(bid) || string.IsNullOrEmpty(todofukenCd) || string.IsNullOrEmpty(kumiaitoCd) || string.IsNullOrEmpty(shishoCd) || string.IsNullOrEmpty(jid))
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「７.」へ進む。
                    // （"ME01645" 引数{0} ：パラメータの取得）
                    errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                    throw new AppException("ME01645", errorMessage);
                }

                // [変数：バッチID]が数値変換不可の場合
                // 数値化したバッチID
                int nBid = 0;
                if (!int.TryParse(bid, out nBid))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「７．」へ進む。
                    // （"ME90012"　引数{0} ：バッチID)
                    errorMessage = MessageUtil.Get("ME90012", "バッチID");
                    throw new AppException("ME90012", errorMessage);
                }

                // ３．１．２．バッチ予約状況取得
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
                    errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                    throw new AppException("ME01645", errorMessage);
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
                    // バッチ条件情報
                    BatchJoken batchJoken = new();

                    // ３．１．３．１．t_01050_バッチ条件から[引数：バッチ条件のキー情報]および下記「条件名称」に一致する「バッチ条件情報」を取得する。
                    batchJoken.GetBatchJokens(dbContext, jid);

                    // 必須入力チェック
                    if (!batchJoken.IsRequired())
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「７.」へ進む。
                        // （"ME01645"　引数{0} ：パラメータの取得)
                        errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                        throw new AppException("ME01645", errorMessage);
                    }

                    // ４．コードの整合性チェック
                    // ４．１．整合性チェックでエラーの場合、[変数：エラーメッセージ]を設定し、「７.」へ進む。
                    // ４．２．整合性チェックで取得した値を変数に設定する。
                    if (!batchJoken.IsConsistency(dbContext, todofukenCd, kumiaitoCd, ref errorMessage))
                    {
                        throw new AppException("ME91003", errorMessage);
                    }

                    // ５．データ検索SQLを実行（ログ出力：あり）
                    // ５．１．「組合員等各種条件設定エラーリストデータ」を取得する。
                    List<KumiaiintoKakushuJoukenSetteiErrorData> errorDatas = GetErrorListData(dbContext, todofukenCd, kumiaitoCd, batchJoken);

                    // ５．２．取得した件数が0件の場合
                    if (errorDatas.Count == 0)
                    {
                        //  [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「７.」へ進む。
                        // （"ME10076" 引数{0}：0)
                        errorMessage = MessageUtil.Get("ME10076", "0");
                        throw new AppException("ME10076", errorMessage);
                    }

                    // ５．３．処理対象のデータを変数に設定する。
                    // ５．３．１．ZIPファイル格納先パスを作成して変数に設定する
                    // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                    string zipFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.CSV_OUTPUT_FOLDER), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));

                    // ５．３．２．作成したZIPファイル格納先パスにZIPファイル格納フォルダを作成する
                    Directory.CreateDirectory(zipFolderPath);

                    // ６．組合員等各種条件設定エラーリストデータ出力処理
                    // ６．１．出力用組合員等各種条件設定エラーリストデータファイル作成
                    // 一時領域にデータ一時出力フォルダとファイルを作成する
                    // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/
                    string tempFolderPath = Path.Combine(ConfigUtil.Get(NskCommon.CoreConst.FILE_TEMP_FOLDER_PATH), bid + Core.CoreConst.SYMBOL_UNDERSCORE + DateUtil.GetSysDateTime().ToString("yyyyMMddHHmmss"));
                    Directory.CreateDirectory(tempFolderPath);

                    // ファイル名：[変数：条件_ファイル名].txt
                    string fileName = batchJoken.FileName + NskCommon.CoreConst.FILE_EXTENSION_TXT;
                    string filePath = Path.Combine(tempFolderPath, fileName);

                    // ６．２．組合員等各種条件設定エラーリストデータ出力
                    // ファイル設計書に沿って組合員等各種条件設定エラーリストデータファイルに組合員等各種条件設定エラーリストデータを出力する。
                    // ファイルの生成
                    using (FileStream fs = File.Create(filePath))
                    {
                        string ruiKubun = batchJoken.Ruikubun;
                        string ruiName = batchJoken.RuiName;

                        // 共済目的コードの確認
                        NskCommon.CoreConst.KyosaiMokutekiCdNumber kyosaiMokutekiCd = (NskCommon.CoreConst.KyosaiMokutekiCdNumber)int.Parse(batchJoken.KyosaiMokuteki);
                        // ファイル書き込みの内容を変化させる
                        switch (kyosaiMokutekiCd)
                        {
                            case NskCommon.CoreConst.KyosaiMokutekiCdNumber.Suitou:
                                foreach (KumiaiintoKakushuJoukenSetteiErrorData errorData in errorDatas)
                                {
                                    errorData.用途区分 = string.Empty;
                                    errorData.用途名称 = string.Empty;
                                }
                                break;
                            case NskCommon.CoreConst.KyosaiMokutekiCdNumber.Rikutou:
                                ruiKubun = string.Empty;
                                ruiName = string.Empty;
                                foreach (KumiaiintoKakushuJoukenSetteiErrorData errorData in errorDatas)
                                {
                                    errorData.用途区分 = string.Empty;
                                    errorData.用途名称 = string.Empty;
                                }
                                break;
                            case NskCommon.CoreConst.KyosaiMokutekiCdNumber.Mugi:
                                break;
                        }

                        // この時、ファイルの文字コードとして[変数：文字コード]に該当する文字エンコーディングを使用する。
                        // 文字コード
                        Encoding encoding = Encoding.Default;
                        switch (int.Parse(batchJoken.MojiCd))
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
                            // 1行目以降の出力
                            List<string> header =
                            [
                                "各種条件設定エラーリスト",
                                batchJoken.Nensan,
                                batchJoken.KyosaiMokuteki,
                                batchJoken.KyosaiMokutekiName,
                                kumiaitoCd,
                                ruiKubun,
                                ruiName,
                                batchJoken.Daichiku,
                                batchJoken.DaichikuName,
                                batchJoken.ShochikuStart,
                                batchJoken.ShochikuNameStart,
                                batchJoken.ShochikuEnd,
                                batchJoken.ShochikuNameEnd,
                                batchJoken.KumiaiintoCdStart,
                                batchJoken.KumiaiintoCdEnd,
                                DateUtil.GetReportJapaneseDate(DateUtil.GetSysDateTime())
                            ];
                            // 配列の内容を書き込む
                            writer.Write(CsvUtil.GetLine(header.ToArray()));

                            // 2行目以降の出力
                            foreach (KumiaiintoKakushuJoukenSetteiErrorData eData in errorDatas)
                            {
                                List<string> data =
                                [
                                    eData.組合員等コード,
                                    eData.組合員等氏名漢字,
                                    eData.用途区分,
                                    eData.用途名称,
                                    eData.ERRMSG
                                ];
                                // 読込データを1行ずつ書き込む
                                writer.Write(CsvUtil.GetLine(data.ToArray()));
                            }
                        }
                    }

                    // ６．３．Zip暗号化を行う。
                    // ６．３．１．「６．１．」のフォルダ内のテキストをZip化（暗号化）し、
                    Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
                    // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
                    // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
                    // [変数：ZIPファイル格納先パス] とファイル名でパスを登録します。
                    FolderUtil.MoveFile(zipFilePath, zipFolderPath, batchYoyakuId, nBid);

                    // ６．３．２．「６．１．」のフォルダを削除する。
                    if (Directory.Exists(tempFolderPath))
                    {
                        Directory.Delete(tempFolderPath, true);
                    }

                    // ６．４．処理正常終了時
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
        /// 「組合員等各種条件設定エラーリストデータ」の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="joken">バッチ条件</param>
        /// <returns></returns>
        private static List<KumiaiintoKakushuJoukenSetteiErrorData> GetErrorListData(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, BatchJoken joken)
        {
            StringBuilder sql = new();

            sql.Append($"SELECT ");
            sql.Append($"    T1.組合員等コード, ");
            sql.Append($"    T2.hojin_full_nm As 組合員等氏名漢字, ");
            sql.Append($"    T1.用途区分, ");
            sql.Append($"    T3.用途名称,");
            sql.Append($"    T1.引受ERRメッセージ || T1.評価ERRメッセージ As ERRMSG, ");
            sql.Append($"    T4.共済目的名称, ");
            sql.Append($"    T5.類短縮名称, ");
            sql.Append($"    T6.daichiku_nm As 大地区名, ");
            sql.Append($"    T7.shochiku_nm As 小地区名 ");

            sql.Append($"FROM ");
            sql.Append($"    t_12030_用途チェック T1 ");
            sql.Append($"    LEFT OUTER JOIN m_00010_共済目的名称 T4 ");
            sql.Append($"        ON T1.共済目的コード = T4.共済目的コード ");
            sql.Append($"    LEFT OUTER JOIN m_00020_類名称 T5 ");
            sql.Append($"        ON T1.共済目的コード = T5.共済目的コード ");
            sql.Append($"        AND T1.類区分 = T5.類区分 ");
            sql.Append($"    LEFT OUTER JOIN v_daichiku_nm T6 ");
            sql.Append($"        ON T6.todofuken_cd = @都道府県コード ");
            sql.Append($"        AND T1.組合等コード = T6.kumiaito_cd ");
            sql.Append($"        AND T1.大地区コード = T6.daichiku_cd ");
            sql.Append($"    LEFT OUTER JOIN v_shochiku_nm T7 ");
            sql.Append($"        ON T7.todofuken_cd = @都道府県コード ");
            sql.Append($"        AND T1.組合等コード = T7.kumiaito_cd ");
            sql.Append($"        AND T1.大地区コード = T7.daichiku_cd ");
            sql.Append($"        AND T1.小地区コード = T7.shochiku_cd ");
            sql.Append($"    LEFT OUTER JOIN v_nogyosha T2 ");
            sql.Append($"        ON T1.組合員等コード = T2.kumiaiinto_cd ");
            sql.Append($"    LEFT OUTER JOIN m_10110_用途区分名称 T3 ");
            sql.Append($"        ON T1.共済目的コード = T3.共済目的コード ");
            sql.Append($"        AND T1.用途区分 = T3.用途区分 ");

            sql.Append($"WHERE ");
            sql.Append($"        T1.年産 = @年産 ");
            sql.Append($"    AND T1.共済目的コード = @共済目的コード ");
            sql.Append($"    AND T1.組合等コード = @組合等コード ");
            sql.Append($"    AND T1.類区分 = @類区分 ");
            sql.Append($"    AND T1.引受ERRフラグ = '1' ");
            sql.Append($"    OR  T1.評価ERRフラグ = '1' ");
            sql.Append($"    OR  T1.引受WARフラグ = '1' ");
            sql.Append($"    OR  T1.評価WARフラグ = '1' ");

            if (!string.IsNullOrEmpty(joken.Daichiku))
            {
                sql.Append($"    AND T1.大地区コード = @大地区コード ");

                if (!string.IsNullOrEmpty(joken.ShochikuStart))
                {
                    sql.Append($"    AND T1.小地区コード >= @小地区コードFrom ");
                }

                if (!string.IsNullOrEmpty(joken.ShochikuEnd))
                {
                    sql.Append($"    AND T1.小地区コード <= @小地区コードTo ");
                }
            }

            if (!string.IsNullOrEmpty(joken.KumiaiintoCdStart))
            {
                sql.Append($"    AND T1.組合員等コード >= @組合員等コードFrom ");
            }

            if (!string.IsNullOrEmpty(joken.KumiaiintoCdEnd))
            {
                sql.Append($"    AND T1.組合員等コード <= @組合員等コードTo ");
            }

            if (!(string.IsNullOrEmpty(joken.OrderByKey1) && string.IsNullOrEmpty(joken.OrderByKey2) && string.IsNullOrEmpty(joken.OrderByKey3)))
            {
                // 初回の判定
                bool isFirst = true;
                // ソート順と出力順のリスト化
                List<SortOrder> sortOrders =
                [
                    new() { OrderByKey = joken.OrderByKey1, OrderBy = joken.OrderBy1 },
                    new() { OrderByKey = joken.OrderByKey2, OrderBy = joken.OrderBy2 },
                    new() { OrderByKey = joken.OrderByKey3, OrderBy = joken.OrderBy3 }
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

                        if (sort.OrderByKey == joken.OrderByKey1)
                        {
                            sql.Append($"   @出力順1 ");
                        }
                        else if (sort.OrderByKey == joken.OrderByKey2)
                        {
                            sql.Append($"   @出力順2 ");
                        }
                        else if (sort.OrderByKey == joken.OrderByKey3)
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

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("都道府県コード", todofukenCd),
                new("年産", int.Parse(joken.Nensan)),
                new("共済目的コード", joken.KyosaiMokuteki),
                new("組合等コード", kumiaitoCd),
                new("類区分", joken.Ruikubun),
                new("大地区コード", joken.Daichiku),
                new("小地区コードFrom", joken.ShochikuStart),
                new("小地区コードTo", joken.ShochikuEnd),
                new("組合員等コードFrom", joken.KumiaiintoCdStart),
                new("組合員等コードTo", joken.KumiaiintoCdEnd),
                new("出力順1", joken.OrderByKey1),
                new("出力順2", joken.OrderByKey2),
                new("出力順3", joken.OrderByKey3)
            ];

            // SQLのクエリ結果をListに格納する
            List<KumiaiintoKakushuJoukenSetteiErrorData> ErrorListData = dbContext.Database.SqlQueryRaw<KumiaiintoKakushuJoukenSetteiErrorData>(sql.ToString(), parameters.ToArray()).ToList();

            return ErrorListData;
        }
    }
}
