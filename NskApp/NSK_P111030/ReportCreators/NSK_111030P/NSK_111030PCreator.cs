using GrapeCity.ActiveReports.SectionReportModel;
using NSK_P111030.Models;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using NskReportMain.Reports;

namespace NSK_P111030.ReportCreators.NSK_111030P
{
    /// <summary>
    /// 交付金変更内訳書（別紙）
    /// </summary>
    public class NSK_111030PCreator : ReportCreator
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static NSK_111030PCreator()
        {

        }

        /// <summary>
        /// 交付金変更内訳書（別紙）レポート出力処理
        /// </summary>
        /// <param name="outputDatas">出力対象データ</param>
        /// <returns></returns>
        public static CreatorResult CreateReport(NSK_111030PModel outputDatas)
        {
            // 変数
            // Creatorの実行結果
            CreatorResult creatorResult = new()
            {
                Result = ReportConst.RESULT_FAILED,
                SectionReport = null
            };

            // １．引数チェックする。
            IsRequired(outputDatas.TyouhyouShousaiDatas, ref creatorResult);

            // ２．帳票を作成する。
            // ２．１．BaseSectionReport()を呼び、レポートを初期化する。
            // 第１引数 レポートファイルパス  "Reports\P111030\P111030Report.rpx"
            // ２．１．１．[２．１．] の戻り値を[変数：帳票インスタンス]に設定する。
            BaseSectionReport tyouhyouInstance = new(@"Reports\P111030\P111030Report.rpx");

            // ２．２．引数：出力対象データを帳票の対応項目に設定する。
            // 帳票のデータ設定
            SetDatas(outputDatas, ref tyouhyouInstance);

            // ２．３．[変数：帳票インスタンス].Run()を呼び、設定したデータで帳票を作成する。
            tyouhyouInstance.Run();

            // ２．４．[変数：実行結果].ResultにReportConst.RESULT_SUCCESS（成功：0）を設定する。
            creatorResult.Result = ReportConst.RESULT_SUCCESS;

            // ２．５．[変数：実行結果].SectionReportに[変数：帳票インスタンス]を設定する。
            creatorResult.SectionReport = tyouhyouInstance;

            // ３．[変数：実行結果]を返す。
            return creatorResult;
        }

        /// <summary>
        /// 引数のチェック
        /// </summary>
        /// <param name="outputDatas">出力対象データ</param>
        /// <param name="createResult">実行結果</param>
        private static void IsRequired(List<TyouhyouShousaiData> outputDatas, ref CreatorResult createResult)
        {
            // １．２．出力対象データがnullまたはデータの件数が0件の場合、エラーとし、実行結果を返す。
            // [引数：出力対象データ]がnull または [引数：出力対象データ]が0件
            if (outputDatas == null || outputDatas.Count == 0)
            {
                // [変数：実行結果].CreateResultError()を呼び、チェックエラーを設定する。
                // 第1引数 "ME01054"   処理件数が0件のため処理を終了します。
                // 第2引数 string.Empty
                createResult.CreateResultError("ME01054", string.Empty);
            }
        }

        /// <summary>
        /// 出力対象データを帳票の対応項目に設定する。
        /// </summary>
        /// <param name="outputDatas">出力対象データ</param>
        /// <param name="tyouhyouInstance">帳票レポート</param>
        private static void SetDatas(NSK_111030PModel outputDatas, ref BaseSectionReport tyouhyouInstance)
        {
            // PageHeader
            // 年産
            // "GGGYY（令和YY）"
            int nendo = int.Parse(outputDatas.TyouhyouSakuseiJouken.JoukenNensan);
            string nensan = GetJapaneseDate(new DateTime(nendo, 1, 1), "ggyy");
            ((TextBox)tyouhyouInstance.Sections["PageHeader"].Controls["txt年産"]).Text = nensan;

            // 発行年月日
            // "[年号]yy年　MM月　dd日"
            DateTime dateTime = new();
            bool isDataParse = DateTime.TryParse(outputDatas.TyouhyouSakuseiJouken.JoukenHakkoDate, out dateTime);
            string dateString = "　　　年　　月　　日";
            if (isDataParse)
            {
                dateString = GetJapaneseDate(dateTime, "ggyy年　MM月　dd日");
            }
            ((Label)tyouhyouInstance.Sections["PageHeader"].Controls["lbl日付"]).Text = dateString;

            // 負担金交付区分
            // "稲 or 麦 or ' '"
            string hutankinKoufuKbn = string.Empty;
            switch (outputDatas.TyouhyouSakuseiJouken.JoukenFutankinKofuKbn)
            {
                case "1":
                    hutankinKoufuKbn = "稲";
                    break;
                case "2":
                    hutankinKoufuKbn = "麦";
                    break;
                default:
                    break;
            }
            ((TextBox)tyouhyouInstance.Sections["PageHeader"].Controls["txt負担金交付区分"]).Text = hutankinKoufuKbn;

            // 詳細データ
            tyouhyouInstance.DataSource = outputDatas.TyouhyouShousaiDatas;
        }

        /// <summary>
        /// 日付の和暦フォーマット設定
        /// </summary>
        /// <param name="date">日付</param>
        /// <param name="format">和暦フォーマット</param>
        /// <returns></returns>
        private static string GetJapaneseDate(DateTime date, string format)
        {
            // カルチャの「言語-国/地域」を「日本語-日本」に設定します。
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ja-JP");
            // 和暦を表すクラスです。
            System.Globalization.JapaneseCalendar jp = new System.Globalization.JapaneseCalendar();
            // 現在のカルチャで使用する暦を、和暦に設定します。
            ci.DateTimeFormat.Calendar = jp;

            string dateString = date.ToString(format, ci);
            return dateString;
        }
    }
}
