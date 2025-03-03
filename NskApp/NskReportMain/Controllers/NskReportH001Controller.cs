using NskAppModelLibrary.Context;
using NskReportMain.Common;
using NskReportMain.Models.P1001;
using NskReportMain.ReportCreators.P1001;
using NskReportMain.Reports;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using GrapeCity.DataVisualization.TypeScript;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelLibrary.Models;
using Npgsql;
using NpgsqlTypes;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportLibrary.Core.Utility;
using System.ComponentModel;
using System.Text;

namespace NskReportMain.Controllers
{
    public class NskReportH001Controller : ReportController
    {
        #region メンバー定数
        /// <summary>
        /// ロガー出力情報
        /// </summary>
        private static readonly string LOGGER_INFO_STR = "H001_加入者情報（一覧）制御処理";

        /// <summary>
        /// 表示順
        /// </summary>
        private static readonly Dictionary<string, string> allSortDic = new Dictionary<string, string>
            {
                { DisplaySortType.Nendo.ToString(), "t_kanyusha.nendo" },
                { DisplaySortType.TodofukenCd.ToString(), "v_nogyosha.todofuken_cd" },
                { DisplaySortType.KumiaitoCd.ToString(), "v_nogyosha.kumiaito_cd" },
                { DisplaySortType.ShishoCd.ToString(), "v_nogyosha.shisho_cd" },
                { DisplaySortType.ShichosonCd.ToString(), "v_nogyosha.shichoson_cd" },
                { DisplaySortType.DaichikuCd.ToString(), "v_nogyosha.daichiku_cd" },
                { DisplaySortType.ShochikuCd.ToString(), "v_nogyosha.shochiku_cd" },
                { DisplaySortType.HojinFullNm.ToString(), "v_nogyosha.hojin_full_nm" },
                { DisplaySortType.KanyushaCd.ToString(), "t_kanyusha.kanyusha_cd" },
            };

        /// <summary>
        /// ソート順
        /// </summary>
        [Flags]
        private enum DisplaySortType
        {
            [Description("対象年度")]
            Nendo,
            [Description("都道府県")]
            TodofukenCd,
            [Description("組合等")]
            KumiaitoCd,
            [Description("支所")]
            ShishoCd,
            [Description("加入者管理コード")]
            KanyushaCd,
            [Description("市町村")]
            ShichosonCd,
            [Description("大地区")]
            DaichikuCd,
            [Description("小地区")]
            ShochikuCd,
            [Description("氏名又は法人名")]
            HojinFullNm,
        }
        #endregion

        public NskReportH001Controller(DbConnectionInfo dbInfo) : base(dbInfo)
        {
        }

        #region H001_加入者情報（一覧）制御処理
        /// <summary>
        /// H001_加入者情報（一覧）制御処理
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">利用可能支所一覧</param>
        /// <param name="batchFlag">バッチフラグ（リアルタイム帳票：0、バッチ帳票：1）</param>
        /// <param name="filePath">帳票パス</param>
        /// <param name="batchId">バッチID</param>/// 
        /// <returns>実行結果</returns>
        public ControllerResult ManageReports(string userId, int joukenId, string todofukenCd, string kumiaitoCd, string shishoCd, List<string> shishoList, int batchFlag, string filePath, long? batchId)
        {
            // ログ出力(開始)
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CONTROLLER,
                joukenId,
                LOGGER_INFO_STR,
                string.Join(ReportConst.PARAM_SEPARATOR, new string[]{
                        ReportConst.PARAM_NAME_USER_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + userId,
                        ReportConst.PARAM_NAME_JOUKEN_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + joukenId,
                        ReportConst.PARAM_NAME_TODOFUKEN_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + todofukenCd,
                        ReportConst.PARAM_NAME_KUMIAITO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + kumiaitoCd,
                        ReportConst.PARAM_NAME_SHISHO_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR + shishoCd,
                        ReportConst.PARAM_NAME_SHISHO_LIST_CD + ReportConst.PARAM_NAME_VALUE_SEPARATOR +
                        (shishoList.IsNullOrEmpty() ? string.Empty : string.Join(ReportConst.SYMBOL_COMMA, shishoList)),
                        ReportConst.PARAM_NAME_BATCH_FLAG + ReportConst.PARAM_NAME_VALUE_SEPARATOR + batchFlag,
                        ReportConst.PARAM_NAME_FILE_PATH + ReportConst.PARAM_NAME_VALUE_SEPARATOR + filePath,
                        ReportConst.PARAM_NAME_BATCH_ID + ReportConst.PARAM_NAME_VALUE_SEPARATOR + (batchId == null ? string.Empty : batchId.ToString())})));

            // 実行結果
            var result = new ControllerResult();
            try
            {
                // 引数チェック
                result = CheckParams(result, userId, joukenId, todofukenCd, kumiaitoCd, shishoCd, shishoList, batchFlag, filePath);
                if (ReportConst.RESULT_FAILED.Equals(result.Result))
                {
                    return result;
                }

                // 帳票出力条件を取得する。
                // 条件IDをキーに帳票条件テーブルより帳票出力条件を取得する。
                // 取得結果が0件の場合はエラーとし、エラーメッセージを返す。
                var reportJoukens = GetReportJouken(getJigyoDb<NskAppContext>(), joukenId);
                if (reportJoukens.IsNullOrEmpty())
                {
                    return result.ControllerResultError(string.Empty, "ME90010");
                }

                // ループの外に持ち出すために、帳票データを格納するリアルタイム出力用帳票データを作成する。（初期値＝空）
                var p1001Model = new P1001Model();
                // P1001の検索条件
                var p1001SearchCondition = SetSearchCondition(reportJoukens);
                p1001Model.P1001SearchCondition = p1001SearchCondition;

                // 帳票出力対象とする農業者IDを取得する。
                // 引数：バッチフラグが0（リアルタイム）の場合
                if (ReportConst.REPORT_REALTIME.Equals(batchFlag))
                {
                    // 帳票オブジェクト
                    var report = new BaseSectionReport();
                    // 取得したデータから作成対象とする帳票データを取得する。
                    var p1001TableRecords = GetRealReportDataList(reportJoukens, todofukenCd, kumiaitoCd, shishoCd, shishoList, ReportConst.REPORT_REALTIME);
                    // 取得結果が0件の場合は、エラーとし、エラーメッセージを返す。
                    // （メッセージID:MI00012）
                    if (p1001TableRecords.IsNullOrEmpty())
                    {
                        return result.ControllerResultError(string.Empty, "MI00012");
                    }

                    // データ編集処理
                    p1001TableRecords.ForEach(t => { t.NendoDisp3 = NendoUtil.GetNendoDisp3(t.Nendo); });
                    p1001Model.OutputYMD = DateUtil.GetOutputYMD();
                    p1001Model.OutputHM = DateUtil.GetOutputHM();
                    p1001Model.P1001TableRecordList = p1001TableRecords;

                    result = CreateP1001(p1001Model, joukenId, ref report);
                    if (result.Result == ReportConst.RESULT_FAILED)
                    {
                        return result;
                    }

                    // 処理結果を返す
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        pdfExport.Export(report.Document, memoryStream);

                        result.Result = ReportConst.RESULT_SUCCESS;
                        result.ReportData = memoryStream;
                    }
                }

                // 引数：バッチフラグが1（バッチ）の場合
                if (ReportConst.REPORT_BATCH.Equals(batchFlag))
                {
                    var dicNendoNogyoshaId = new Dictionary<List<int>, short>();
                    // 取得したデータから作成対象とする帳票データの集約単位リストを取得する。
                    // 取得結果が0件の場合は、エラーとし、エラーメッセージを返す。
                    // （メッセージID:MI00012）
                    var nendoList = GetNendoList(reportJoukens, todofukenCd, kumiaitoCd, shishoCd, shishoList, ReportConst.REPORT_BATCH);
                    if (nendoList.IsNullOrEmpty())
                    {
                        return result.ControllerResultError(string.Empty, "MI00012");
                    }

                    var splitZumiNogyoshaIdList = new List<List<int>>();
                    // 集約単位リストの数分、以下の処理を繰り返す。
                    foreach (var nendo in nendoList)
                    {
                        // 帳票データの農業者IDを取得する。
                        var nendoNogyoshaIdList = GetNendoNogyoshaIdList(reportJoukens, todofukenCd, kumiaitoCd, shishoCd, shishoList, ReportConst.REPORT_BATCH, nendo);

                        // 処理分割件数（定数ID：H001SplitCount）
                        var splitCount = ConfigUtil.GetInt(ReportConst.H001_SPLIT_COUNT);
                        var spiltList = SplitList(nendoNogyoshaIdList, splitCount);
                        // 帳票出力用年度情報を保存
                        foreach (var item in spiltList)
                        {
                            dicNendoNogyoshaId.Add(item, nendo);
                        }
                        splitZumiNogyoshaIdList.AddRange(spiltList);
                    }

                    // 帳票PDF一時出力フォルダ作成
                    // 引数：バッチフラグが1（バッチ）の場合
                    // 以下の帳票PDF一時出力フォルダを作成する。
                    // 定数取得(（定数ID：PrintTempFolder）)\ユーザID_グローバル一意識別子(GUID)\（引数：帳票パスのファイル名（拡張子を除く））
                    // （例）引数：帳票パスがC:\aaa\bbb\ccc\tyouhyou.zipの場合
                    // 出力フォルダは、定数取得(（定数ID：PrintTempFolder）)\ユーザID_91b84368272946718288a66acc9e078f\tyouhyou
                    var printTempFolder = FolderUtil.CreatePrintTempFolder(userId, filePath);
                    // 年度マスタデータを取得する。
                    var nendolist = NendoUtil.GetNendoList().ToList();

                    //  分割後農業者IDリストの数分、以下の処理を繰り返す。
                    for (var i = 0; i < splitZumiNogyoshaIdList.Count; i++)
                    {
                        // 帳票オブジェクト
                        var report = new BaseSectionReport();
                        var nogyoIdList = splitZumiNogyoshaIdList[i];
                        // 分割後農業者IDリスト内の農業者IDを条件に、「加入者情報（一覧）」の帳票データを取得する。
                        var p1001TableRecords = GetReportData(reportJoukens, nogyoIdList, dicNendoNogyoshaId);
                        // データ編集処理
                        p1001TableRecords.forEach(t =>
                        {
                            var nendo = nendolist.Where(n => n.Nendo == t.Nendo).FirstOrDefault();
                            t.NendoDisp3 = (nendo?.NendoDisp3 ?? string.Empty);
                        });
                        p1001Model.OutputYMD = DateUtil.GetOutputYMD();
                        p1001Model.OutputHM = DateUtil.GetOutputHM();
                        p1001Model.P1001TableRecordList = p1001TableRecords;

                        result = CreateP1001(p1001Model, joukenId, ref report);
                        if (result.Result == ReportConst.RESULT_FAILED)
                        {
                            return result;
                        }

                        // 分割後農業者IDリストの数が1つの場合
                        if (splitZumiNogyoshaIdList.Count == 1)
                        {
                            // ファイル名は「10_加入者情報（一覧）_yyyy.PDF」とする。
                            pdfExport.Export(report.Document, Path.Combine(printTempFolder, "10_加入者情報（一覧）_" + dicNendoNogyoshaId[nogyoIdList].toString() + ReportConst.REPORT_EXTENSION));
                        }
                        else
                        {
                            // ファイル名は「10_加入者情報（一覧）_yyyy_分割後農業者IDリスト連番.PDF」とする。
                            pdfExport.Export(report.Document, Path.Combine(printTempFolder, "10_加入者情報（一覧）_" + dicNendoNogyoshaId[nogyoIdList].toString() +
                                ReportConst.SYMBOL_UNDERSCORE + (i + 1).ToString("D3") + ReportConst.REPORT_EXTENSION));
                        }
                    }

                    // バッチフラグが1（バッチ）である場合
                    // PDF出力用一時フォルダをzip化（暗号化）する。
                    var zipFilePath = ZipUtil.CreateZip(printTempFolder);

                    // Zipファイルを引数：帳票パスに移動する
                    FolderUtil.MoveFile(zipFilePath, filePath, userId, (batchId ?? 0));

                    // PDF出力用一時フォルダを削除する
                    FolderUtil.DeleteTempFolder(printTempFolder);
                }

                result.Result = ReportConst.RESULT_SUCCESS;
            }
            catch (Exception)
            {
                // 法定帳票などと制御クラスの構造を統一するため、ここで例外をキャッチする
                throw;
            }

            // ログ出力(終了)
            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CONTROLLER,
                joukenId,
                LOGGER_INFO_STR));

            return result;
        }
        #endregion

        #region 「P1001_加入者情報（一覧）」を作成するメソッド
        /// <summary>
        /// 「P1001_加入者情報（一覧）」を作成するメソッド
        /// </summary>
        /// <param name="model">帳票モデルリスト</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="report">帳票オブジェクト</param>
        /// <returns>実行結果</returns>
        private ControllerResult CreateP1001(
            P1001Model model,
            int joukenId,
            ref BaseSectionReport report)
        {
            // 実行結果
            var result = new ControllerResult();
            P1001Creator p1001Creator = new P1001Creator();
            CreatorResult creatorResult = p1001Creator.CreateReport(joukenId, model);
            // 失敗した場合
            if (creatorResult.Result == ReportConst.RESULT_FAILED)
            {
                return result.ControllerResultError(creatorResult.ErrorMessage, creatorResult.ErrorMessageId);
            }

            // ページ番号を描画する
            creatorResult.SectionReport = ReportPagerUtil.DrawReportPageNumber(creatorResult.SectionReport, ReportConst.REPORT_BOTTOM_MARGIN_STANDARD + ReportConst.REPORT_RIGHT_MARGIN_FIVE * 2);
            // 出力レポートにP1001_加入者情報（一覧）を入れる。
            report.Document.Pages.AddRange(creatorResult.SectionReport.Document.Pages);

            return result;
        }
        #endregion

        #region 引数チェック
        /// <summary>
        /// H001_加入者情報（一覧）引数チェック
        /// </summary>
        /// <param name="result">実行結果</param>
        /// <param name="userId">ユーザーID</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">利用可能支所一覧</param>
        /// <param name="batchFlag">バッチフラグ（リアルタイム帳票：0、バッチ帳票：1）</param>
        /// <param name="filePath">帳票パス</param>
        /// <returns>実行結果</returns>
        private ControllerResult CheckParams(ControllerResult result, string userId, int joukenId, string todofukenCd, string kumiaitoCd, string shishoCd,
            List<string> shishoList, int batchFlag, string filePath)
        {
            // ■１．引数チェックする
            // ユーザIDがないの場合、エラーとし、エラーメッセージを返す
            if (string.IsNullOrEmpty(userId))
            {
                return result.ControllerResultError(string.Empty, "ME90009", ReportConst.PARAM_NAME_USER_ID);
            }

            // 条件IDがないの場合、エラーとし、エラーメッセージを返す
            if (joukenId == 0)
            {
                return result.ControllerResultError(string.Empty, "ME90009", ReportConst.PARAM_NAME_JOUKEN_ID);
            }

            // バッチフラグが0（リアルタイム）、または1（バッチ）以外の場合、エラーとし、エラーメッセージを返す
            if (!ReportConst.REPORT_REALTIME.Equals(batchFlag) && !ReportConst.REPORT_BATCH.Equals(batchFlag))
            {
                return result.ControllerResultError(string.Empty, "ME90012", ReportConst.PARAM_NAME_BATCH_FLAG);
            }

            // バッチフラグが1（バッチ）、且つ帳票パスがない場合、エラーとし、エラーメッセージを返す
            if (batchFlag == ReportConst.REPORT_BATCH && string.IsNullOrEmpty(filePath))
            {
                return result.ControllerResultError(string.Empty, "ME90009", ReportConst.PARAM_NAME_FILE_PATH);
            }

            // バッチフラグが0（リアルタイム）の場合
            if (ReportConst.REPORT_REALTIME.Equals(batchFlag))
            {
                // 都道府県コードがない場合、エラーとし、エラーメッセージを返す。
                if (string.IsNullOrEmpty(todofukenCd))
                {
                    return result.ControllerResultError(string.Empty, "ME90009", ReportConst.PARAM_NAME_TODOFUKEN_CD);
                }

                // 組合等コードがない場合、エラーとし、エラーメッセージを返す。
                if (string.IsNullOrEmpty(kumiaitoCd))
                {
                    return result.ControllerResultError(string.Empty, "ME90009", ReportConst.PARAM_NAME_KUMIAITO_CD);
                }

                // バッチフラグが0（リアルタイム）の場合、コードの整合性チェックをする
                // 「都道府県コード存在情報」を取得する。
                // （１）データが取得できない場合（該当データがマスタデータに登録されていない場合）、エラーとし、エラーメッセージを返す。
                // （"{0}に、{1}に存在しないコードが存在します。"　引数{0} ：都道府県コード、引数{1} ：都道府県マスタ)
                logger.Info("「都道府県コード存在情報」を取得する。");
                var todofukenCnt = getJigyoDb<NskAppContext>().VTodofukens.Where(t => t.TodofukenCd == todofukenCd).Count();
                if (todofukenCnt == 0)
                {
                    return result.ControllerResultError(string.Empty, "ME91003", ReportConst.PARAM_NAME_TODOFUKEN_CD, "都道府県マスタ");
                }

                // 「組合等コード存在情報」を取得する。
                // （１）データが取得できない場合（該当データがマスタデータに登録されていない場合）、エラーとし、エラーメッセージを返す。
                // （"{0}に、{1}に存在しないコードが存在します。"　引数{0} ：組合等コード、引数{1} ：組合等マスタ)
                logger.Info("「組合等コード存在情報」を取得する。");
                var kumiaitoCnt = getJigyoDb<NskAppContext>().VKumiaitos.Where(t => t.TodofukenCd == todofukenCd && t.KumiaitoCd == kumiaitoCd).Count();
                if (kumiaitoCnt == 0)
                {
                    return result.ControllerResultError(string.Empty, "ME91003", ReportConst.PARAM_NAME_KUMIAITO_CD, "組合等マスタ");
                }

                // 引数：利用可能支所一覧が入力されている場合、「利用可能支所一覧存在情報」を取得する。
                // （１）データが取得できない場合（該当データがマスタデータに登録されていない場合）、エラーとし、エラーメッセージを返す。
                // （"{0}に、{1}に存在しないコードが存在します。"　引数{0} ：利用可能支所一覧、引数{1} ：名称M支所)
                if (!shishoList.IsNullOrEmpty())
                {
                    logger.Info("「利用可能支所一覧存在情報」を取得する。");
                    var shishoCnt = getJigyoDb<NskAppContext>().VShishoNms.Where(t => t.TodofukenCd == todofukenCd && t.KumiaitoCd == kumiaitoCd && shishoList.Contains(t.ShishoCd)).Count();
                    if (shishoCnt == 0)
                    {
                        return result.ControllerResultError(string.Empty, "ME91003", ReportConst.PARAM_NAME_SHISHO_LIST_CD, "名称M支所");
                    }
                }

                // 引数：支所コードが入力されている場合、「支所コード存在情報」を取得する。
                // （１）データが取得できない場合（該当データがマスタデータに登録されていない場合）、エラーとし、エラーメッセージを返す。
                //（"{0}に、{1}に存在しないコードが存在します。"　引数{0} ：支所コード、引数{1} ：名称M支所)
                if (!string.IsNullOrEmpty(shishoCd))
                {
                    logger.Info("「支所コード存在情報」を取得する。");
                    var shishoCnt = getJigyoDb<NskAppContext>().VShishoNms.Where(t => t.TodofukenCd == todofukenCd && t.KumiaitoCd == kumiaitoCd && t.ShishoCd == shishoCd).Count();
                    if (shishoCnt == 0)
                    {
                        return result.ControllerResultError(string.Empty, "ME91003", ReportConst.PARAM_NAME_SHISHO_CD, "名称M支所");
                    }
                }
            }

            return result;
        }
        #endregion

        #region リアルタイム帳票データ取得メソッド
        /// <summary>
        /// 作成対象とするリアルタイム帳票データを取得する。
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">利用可能支所一覧</param>
        /// <param name="batchFlag">バッチフラグ（リアルタイム帳票：0、バッチ帳票：1）</param>
        /// <returns>帳票データ</returns>
        private List<P1001TableRecord> GetRealReportDataList(List<TReportJouken> reportJoukens, string todofukenCd, string kumiaitoCd, string shishoCd, List<string> shishoList, int batchFlag)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetTableCondition(sql);
            // 検索条件を取得する
            GetSearchCondition(sql, reportJoukens, todofukenCd, kumiaitoCd, shishoCd, shishoList, batchFlag, parameters);
            // ソート順を追加する
            GetOrderby(ref sql, reportJoukens);

            logger.Info("作成対象とする帳票データを取得する。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<P1001TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region 帳票データの集約単位リスト取得メソッド
        /// <summary>
        /// 作成対象とする帳票データの集約単位リストを取得する。
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">利用可能支所一覧</param>
        /// <param name="batchFlag">バッチフラグ（リアルタイム帳票：0、バッチ帳票：1）</param>
        /// <returns>年度リスト</returns>
        private List<short> GetNendoList(List<TReportJouken> reportJoukens, string todofukenCd, string kumiaitoCd, string shishoCd, List<string> shishoList, int batchFlag)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            sql.Append("SELECT ");
            sql.Append("    t_kanyusha.nendo AS \"Value\" ");
            // 検索テーブルを取得する
            GetTableCondition(sql);
            // 検索条件を取得する
            GetSearchCondition(sql, reportJoukens, todofukenCd, kumiaitoCd, shishoCd, shishoList, batchFlag, parameters);
            // group by
            sql.Append("    GROUP BY t_kanyusha.nendo ");
            // ソート順
            sql.Append("    ORDER BY t_kanyusha.nendo ASC ");

            logger.Info("作成対象とする帳票データの集約単位リストを取得する。（集約条件：t_kanyusha.nendo）");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<short>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region 帳票データの農業者ID取得メソッド
        /// <summary>
        /// 特定年度の帳票データの農業者IDを取得する。
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">利用可能支所一覧</param>
        /// <param name="batchFlag">バッチフラグ（リアルタイム帳票：0、バッチ帳票：1）</param>
        /// <param name="nendo">年度</param>
        /// <returns>特定年度の帳票データの農業者ID</returns>
        private List<int> GetNendoNogyoshaIdList(List<TReportJouken> reportJoukens, string todofukenCd, string kumiaitoCd, string shishoCd, List<string> shishoList, int batchFlag, short nendo)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            sql.Append("SELECT ");
            sql.Append("    v_nogyosha.nogyosha_id AS \"Value\" ");
            // 検索テーブルを取得する
            GetTableCondition(sql);
            // 検索条件を取得する
            GetSearchCondition(sql, reportJoukens, todofukenCd, kumiaitoCd, shishoCd, shishoList, batchFlag, parameters, nendo);
            // ソート順を追加する
            GetOrderby(ref sql, reportJoukens);

            logger.Info("分割条件（年度）の帳票データの農業者IDを取得する。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<int>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region 「加入者情報（一覧）」の帳票データ取得メソッド
        /// <summary>
        /// 「加入者情報（一覧）」の帳票データを取得する。
        /// </summary>
        /// <param name="reportJoukens">条件情報</param>
        /// <param name="nogyoshaIdList">農業者IDリスト</param>
        /// <param name="dicNendoNogyoshaId">分割後農業者IDリスト</param>
        /// <returns>帳票データ</returns>
        private List<P1001TableRecord> GetReportData(List<TReportJouken> reportJoukens, List<int> nogyoshaIdList, Dictionary<List<int>, short> dicNendoNogyoshaId)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // 検索項目を取得する
            GetCondition(sql);
            // 検索テーブルを取得する
            GetTableCondition(sql);

            // 検索条件を取得する
            sql.Append("WHERE v_nogyosha.nogyosha_id = ANY(@NogyoshaIdList) ");
            parameters.Add(new NpgsqlParameter("@NogyoshaIdList", NpgsqlDbType.Array | NpgsqlDbType.Integer)
            {
                Value = nogyoshaIdList
            });

            // 年度指定がある
            sql.Append("AND t_kanyusha.nendo = @Nendo ");
            parameters.Add(new NpgsqlParameter("@Nendo", dicNendoNogyoshaId[nogyoshaIdList]));

            // 農業者ID、年度
            var jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_NOGYOSHA_ID_TARGET_NENDO).FirstOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var tulpTableKey = TupleTableKey(jouken?.JoukenValue);
                sql.Append("AND (v_nogyosha.nogyosha_id, t_kanyusha.nendo) IN (");
                string parameterList = string.Join(", ", tulpTableKey.Select((t, i) => $"(@nogyosha_id{i}, @nendo{i})"));
                sql.Append(parameterList);
                sql.Append(")");
                for (int i = 0; i < tulpTableKey.Count; i++)
                {
                    parameters.Add(new NpgsqlParameter($"@nogyosha_id{i}", tulpTableKey[i].Item1));
                    parameters.Add(new NpgsqlParameter($"@nendo{i}", tulpTableKey[i].Item2));
                }
            }

            // ソート順を追加する
            GetOrderby(ref sql, reportJoukens);

            logger.Info("「加入者情報（一覧）」の帳票データを取得する。");
            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<P1001TableRecord>(sql.ToString(), parameters.ToArray()).ToList();
        }
        #endregion

        #region 検索項目取得
        /// <summary>
        /// 検索項目を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetCondition(StringBuilder sql)
        {
            sql.Append("SELECT ");
            sql.Append("    t_kanyusha.nendo AS Nendo ");
            sql.Append("    , '' AS NendoDisp3 ");
            sql.Append("    , v_nogyosha.hojin_full_nm AS HojinFullNm ");
            sql.Append("    , t_kanyusha.kanyusha_cd AS KanyushaCd ");
            sql.Append("    , t_kanyusha.kouchi_address AS KochiAddress ");
        }
        #endregion

        #region 検索テーブル取得
        /// <summary>
        /// 検索テーブルを取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        private void GetTableCondition(StringBuilder sql)
        {
            sql.Append("FROM ");
            sql.Append("    v_nogyosha ");
            sql.Append("    LEFT JOIN t_kanyusha ");
            sql.Append("        ON v_nogyosha.nogyosha_id = t_kanyusha.nogyosha_id ");
        }
        #endregion

        #region 検索条件設定処理
        /// <summary>
        /// 検索条件を設定する。
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="reportJoukens">検索条件</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">利用可能支所一覧</param>
        /// <param name="batchFlag">バッチフラグ（リアルタイム帳票：0、バッチ帳票：1）</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="nendo">検索年度</param>
        private void GetSearchCondition(StringBuilder sql, List<TReportJouken> reportJoukens, string todofukenCd, string kumiaitoCd, string shishoCd,
            List<string> shishoList, int batchFlag, List<NpgsqlParameter> parameters, short? nendo = null)
        {
            sql.Append("WHERE '1' = '1'");

            // バッチフラグが0（リアルタイム）の場合
            if (ReportConst.REPORT_REALTIME.Equals(batchFlag))
            {
                // 都道府県
                sql.Append("AND v_nogyosha.todofuken_cd = @TodofukenCd ");
                parameters.Add(new NpgsqlParameter("@TodofukenCd", todofukenCd));

                // 組合等
                sql.Append("AND v_nogyosha.kumiaito_cd = @KumiaitoCd ");
                parameters.Add(new NpgsqlParameter("@KumiaitoCd", kumiaitoCd));

                // 支所
                if (!shishoList.IsNullOrEmpty())
                {
                    // 引数：利用可能支所一覧がある場合
                    sql.Append("AND v_nogyosha.shisho_cd = ANY (@ShishoList) ");
                    parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                    {
                        Value = shishoList
                    });
                }
                else if (!string.IsNullOrEmpty(shishoCd))
                {
                    // 引数：利用可能支所一覧がない、かつ、引数：支所コードが空でない場合
                    sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_1 ");
                    parameters.Add(new NpgsqlParameter("@ShishoCd_1", shishoCd));
                }
            }

            TReportJouken jouken = null;
            // バッチフラグが1（バッチ）の場合
            if (ReportConst.REPORT_BATCH.Equals(batchFlag))
            {
                // 都道府県
                jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_TODOFUKEN).SingleOrDefault();
                if (!string.IsNullOrEmpty(jouken?.JoukenValue))
                {
                    sql.Append("AND v_nogyosha.todofuken_cd = @TodofukenCd ");
                    parameters.Add(new NpgsqlParameter("@TodofukenCd", jouken.JoukenValue));
                }

                // 組合等
                jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_KUMIAITO).SingleOrDefault();
                if (!string.IsNullOrEmpty(jouken?.JoukenValue))
                {
                    sql.Append("AND v_nogyosha.kumiaito_cd = @KumiaitoCd ");
                    parameters.Add(new NpgsqlParameter("@KumiaitoCd", jouken.JoukenValue));
                }

                // 利用可能支所一覧
                jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHISHO_LIST).SingleOrDefault();
                if (!string.IsNullOrEmpty(jouken?.JoukenValue))
                {
                    sql.Append("AND v_nogyosha.shisho_cd = ANY (@ShishoList) ");
                    parameters.Add(new NpgsqlParameter("@ShishoList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                    {
                        Value = new List<string>(jouken.JoukenValue.Split(",")),
                    });
                }

                // 利用可能支所一覧がない
                if (string.IsNullOrEmpty(jouken?.JoukenValue))
                {
                    // 支所（ログインユーザのセッション）
                    jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SESSION_SHISHO).SingleOrDefault();
                    if (!string.IsNullOrEmpty(jouken?.JoukenValue))
                    {
                        // 利用可能支所一覧がない、かつ、支所コードが空でない場合
                        sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_1 ");
                        parameters.Add(new NpgsqlParameter("@ShishoCd_1", jouken.JoukenValue));
                    }
                }
            }

            // 年度指定がない
            if (nendo == null)
            {
                // 対象年度（開始）
                jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_TARGET_NENDO_FROM).SingleOrDefault();
                if (!string.IsNullOrEmpty(jouken?.JoukenValue))
                {
                    sql.Append("AND t_kanyusha.nendo >= @NendoFrom ");
                    parameters.Add(new NpgsqlParameter("@NendoFrom", short.Parse(jouken.JoukenValue)));
                }

                // 対象年度（終了）
                jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_TARGET_NENDO_TO).SingleOrDefault();
                if (!string.IsNullOrEmpty(jouken?.JoukenValue))
                {
                    sql.Append("AND t_kanyusha.nendo <= @NendoTo ");
                    parameters.Add(new NpgsqlParameter("@NendoTo", short.Parse(jouken.JoukenValue)));
                }
            }
            else
            {
                // 年度指定がある
                sql.Append("AND t_kanyusha.nendo = @Nendo ");
                parameters.Add(new NpgsqlParameter("@Nendo", nendo));
            }

            // 農業者ID、年度
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_NOGYOSHA_ID_TARGET_NENDO).FirstOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var tulpTableKey = TupleTableKey(jouken?.JoukenValue);
                sql.Append("AND (v_nogyosha.nogyosha_id, t_kanyusha.nendo) IN (");
                string parameterList = string.Join(", ", tulpTableKey.Select((t, i) => $"(@nogyosha_id{i}, @nendo{i})"));
                sql.Append(parameterList);
                sql.Append(")");
                for (int i = 0; i < tulpTableKey.Count; i++)
                {
                    parameters.Add(new NpgsqlParameter($"@nogyosha_id{i}", tulpTableKey[i].Item1));
                    parameters.Add(new NpgsqlParameter($"@nendo{i}", tulpTableKey[i].Item2));
                }
            }

            // 支所
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHISHO).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                sql.Append("AND v_nogyosha.shisho_cd = @ShishoCd_2 ");
                parameters.Add(new NpgsqlParameter("@ShishoCd_2", jouken.JoukenValue));
            }

            // 市町村
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHICHOSON).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                sql.Append("AND v_nogyosha.shichoson_cd = @ShichosonCd ");
                parameters.Add(new NpgsqlParameter("@ShichosonCd", jouken.JoukenValue));
            }

            // 大地区
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_DAICHIKU).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                sql.Append("AND v_nogyosha.daichiku_cd = @DaichikuCd ");
                parameters.Add(new NpgsqlParameter("@DaichikuCd", jouken.JoukenValue));
            }

            // 小地区（開始）
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHOCHIKU_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                sql.Append("AND v_nogyosha.shochiku_cd >= @ShochikuCdFrom ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdFrom", jouken.JoukenValue));
            }

            // 小地区（終了）
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHOCHIKU_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                sql.Append("AND v_nogyosha.shochiku_cd <= @ShochikuCdTo ");
                parameters.Add(new NpgsqlParameter("@ShochikuCdTo", jouken.JoukenValue));
            }

            // 加入者管理コード（開始）
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_KANYUSHA_CD_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                sql.Append("AND t_kanyusha.kanyusha_cd >= @KanyushaCdFrom ");
                parameters.Add(new NpgsqlParameter("@KanyushaCdFrom", jouken.JoukenValue));
            }

            // 加入者管理コード（終了）
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_KANYUSHA_CD_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                sql.Append("AND t_kanyusha.kanyusha_cd <= @KanyushaCdTo ");
                parameters.Add(new NpgsqlParameter("@KanyushaCdTo", jouken.JoukenValue));
            }

            // 氏名又は法人名
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_HOJIN_FULL_NM).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                // 全角スペース、半角スペースを除く
                var hojinFullNmInput = jouken?.JoukenValue.Replace("　", "").Replace(" ", "");

                sql.Append("AND replace(replace(v_nogyosha.hojin_full_nm,'　',''),' ','') LIKE '%' || @HojinFullNm || '%'  ESCAPE '\\' ");
                parameters.Add(new NpgsqlParameter("@HojinFullNm", StringUtil.RemoveEscapeChar(hojinFullNmInput)));
            }
        }
        #endregion

        #region クエリ式にソート順設定
        /// <summary>
        /// ソート順設定処理
        /// </summary>
        /// <param name="sb">sql</param>
        /// <param name="reportJoukens">検索条件情報</param>
        private void GetOrderby(ref StringBuilder sb, List<TReportJouken> reportJoukens)
        {
            // 表示順１
            var joukenOrderKey1 = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_ORDER_BY_KEY1).SingleOrDefault();
            var joukenOrderBy1 = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_ORDER_BY1).SingleOrDefault();

            // 表示順２
            var joukenOrderKey2 = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_ORDER_BY_KEY2).SingleOrDefault();
            var joukenOrderBy2 = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_ORDER_BY2).SingleOrDefault();

            // 表示順３
            var joukenOrderKey3 = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_ORDER_BY_KEY3).SingleOrDefault();
            var joukenOrderBy3 = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_ORDER_BY3).SingleOrDefault();

            sb.Append("ORDER BY ");
            sb.Append("t_kanyusha.nendo ASC, ");
            sb.Append("v_nogyosha.todofuken_cd ASC, ");
            sb.Append("v_nogyosha.kumiaito_cd ASC, ");
            sb.Append("v_nogyosha.shisho_cd ASC, ");
            sb.Append("v_nogyosha.daichiku_cd ASC, ");
            sb.Append("v_nogyosha.shochiku_cd ASC, ");

            var constOrderList = new List<string>() { "t_kanyusha.nendo", "v_nogyosha.todofuken_cd", "v_nogyosha.kumiaito_cd", "v_nogyosha.shisho_cd", "v_nogyosha.daichiku_cd", "v_nogyosha.shochiku_cd" };
            var orderList = new List<string>();
            // 表示順キー１が空ではない場合
            if (!string.IsNullOrEmpty(joukenOrderKey1?.JoukenValue) && !constOrderList.Contains(allSortDic[joukenOrderKey1.JoukenValue]))
            {
                sb.Append(allSortDic[joukenOrderKey1.JoukenValue] + ReportConst.HALF_WIDTH_SPACE + joukenOrderBy1.JoukenValue + ReportConst.SYMBOL_COMMA);
                orderList.Add(allSortDic[joukenOrderKey1.JoukenValue]);
            }

            // 表示順キー２が空ではない場合
            if (!string.IsNullOrEmpty(joukenOrderKey2?.JoukenValue) && !constOrderList.Contains(allSortDic[joukenOrderKey2.JoukenValue]))
            {
                sb.Append(allSortDic[joukenOrderKey2.JoukenValue] + ReportConst.HALF_WIDTH_SPACE + joukenOrderBy2.JoukenValue + ReportConst.SYMBOL_COMMA);
                orderList.Add(allSortDic[joukenOrderKey2.JoukenValue]);
            }

            // 表示順キー３が空ではない場合
            if (!string.IsNullOrEmpty(joukenOrderKey3?.JoukenValue) && !constOrderList.Contains(allSortDic[joukenOrderKey3.JoukenValue]))
            {
                sb.Append(allSortDic[joukenOrderKey3.JoukenValue] + ReportConst.HALF_WIDTH_SPACE + joukenOrderBy3.JoukenValue + ReportConst.SYMBOL_COMMA);
                orderList.Add(allSortDic[joukenOrderKey3.JoukenValue]);
            }

            // 表示順キー１～３に、加入者管理コードが存在する場合はソート順から除く
            if (!orderList.Contains("t_kanyusha.kanyusha_cd"))
            {
                sb.Append("t_kanyusha.kanyusha_cd ASC ");
            }
            else
            {
                // 最後のコンマを削除する。
                sb = new StringBuilder(sb.ToString().TrimEnd(',', ' '));
            }
        }
        #endregion

        #region 帳票出力用データ設定（検索条件）
        /// <summary>
        /// 帳票出力用検索条件データ設定
        /// </summary>
        /// <param name="reportJoukens">条件表からの検索条件</param>
        /// <returns>帳票出力用検索条件データ</returns>
        private P1001SearchCondition SetSearchCondition(List<TReportJouken> reportJoukens)
        {
            var p1001SearchCondition = new P1001SearchCondition();
            // 都道府県
            var jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_TODOFUKEN).SingleOrDefault();
            var todohukenCd = jouken?.JoukenValue;
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var cdName = TodofukenUtil.GetTodofukenNm(jouken.JoukenValue);
                p1001SearchCondition.JoukenTodofukenCd = jouken.JoukenValue;
                p1001SearchCondition.JoukenTodofukenNm = cdName;
            }

            // 組合等
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_KUMIAITO).SingleOrDefault();
            var kumiaitoCd = jouken?.JoukenValue;
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var cdName = KumiaitoUtil.GetKumiaitoNm(todohukenCd, jouken.JoukenValue);
                p1001SearchCondition.JoukenKumiaitoCd = jouken.JoukenValue;
                p1001SearchCondition.JoukenKumiaitoNm = cdName;
            }

            // 支所
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHISHO).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var cdName = ShishoUtil.GetShishoNm(todohukenCd, kumiaitoCd, jouken.JoukenValue);
                p1001SearchCondition.JoukenShishoCd = jouken.JoukenValue;
                p1001SearchCondition.JoukenShishoNm = cdName;
            }

            // 市町村
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHICHOSON).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var cdName = ShichosonUtil.GetShichosonNm(todohukenCd, kumiaitoCd, jouken.JoukenValue);
                p1001SearchCondition.JoukenShichosonCd = jouken.JoukenValue;
                p1001SearchCondition.JoukenShichosonNm = cdName;
            }

            // 大地区
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_DAICHIKU).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var cdName = DaichikuUtil.GetDaichikuNm(todohukenCd, kumiaitoCd, jouken.JoukenValue);
                p1001SearchCondition.JoukenDaichikuCd = jouken.JoukenValue;
                p1001SearchCondition.JoukenDaichikuNm = cdName;
            }

            // 大地区
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_DAICHIKU).SingleOrDefault();
            var daichikuCd = jouken?.JoukenValue;
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var cdName = DaichikuUtil.GetDaichikuNm(todohukenCd, kumiaitoCd, jouken.JoukenValue);
                p1001SearchCondition.JoukenDaichikuCd = jouken.JoukenValue;
                p1001SearchCondition.JoukenDaichikuNm = cdName;
            }

            // 小地区（開始）
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHOCHIKU_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var cdName = ShochikuUtil.GetShochikuNm(todohukenCd, kumiaitoCd, daichikuCd, jouken.JoukenValue);
                p1001SearchCondition.JoukenShochikuCdStart = jouken.JoukenValue;
                p1001SearchCondition.JoukenShochikuNmStart = cdName;
            }

            // 小地区（終了）
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_SHOCHIKU_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                var cdName = ShochikuUtil.GetShochikuNm(todohukenCd, kumiaitoCd, daichikuCd, jouken.JoukenValue);
                p1001SearchCondition.JoukenShochikuCdEnd = jouken.JoukenValue;
                p1001SearchCondition.JoukenShochikuNmEnd = cdName;
            }

            // 加入者管理コード（開始）
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_KANYUSHA_CD_START).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                p1001SearchCondition.JoukenKanyushaCdStart = jouken.JoukenValue;
            }

            // 加入者管理コード（終了）
            jouken = reportJoukens.Where(t => t.JoukenNm == JoukenNameConst.JOUKEN_KANYUSHA_CD_END).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.JoukenValue))
            {
                p1001SearchCondition.JoukenKanyushaCdEnd = jouken.JoukenValue;
            }

            return p1001SearchCondition;
        }
        #endregion
    }
}