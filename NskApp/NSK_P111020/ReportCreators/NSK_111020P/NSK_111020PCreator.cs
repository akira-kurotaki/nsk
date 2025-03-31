using GrapeCity.ActiveReports.SectionReportModel;
using NSK_P111020.Models;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using NskReportMain.Reports;
using NskCommonLibrary.Core.Consts;

namespace NSK_P111020.ReportCreators.NSK_111020P
{
    /// <summary>
    /// 交付金申請書（別記様式第１号）
    /// </summary>
    public class NSK_111020PCreator : ReportCreator
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static NSK_111020PCreator()
        {

        }

        /// <summary>
        /// 交付金申請書（別記様式第１号）レポート出力処理
        /// </summary>
        /// <param name="outputDatas">出力対象データ</param>
        /// <returns></returns>
        public static CreatorResult CreateReport(NSK_111020PModel outputDatas)
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
            // 第１引数 レポートファイルパス  "Reports\P111020\P111020Report.rpx"]
            // ２．１．１．[２．１．] の戻り値を[変数：帳票インスタンス]に設定する。
            BaseSectionReport tyouhyouInstance = new(@"Reports\P111020\P111020Report.rpx");

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

        private static void SetDatas(NSK_111020PModel data, ref BaseSectionReport tyouhyouInstance)
        {
            // 文書番号
            // "第NNNNN号"
            string bunshoNo = "　　　　　";
            if (!string.IsNullOrEmpty(data.TyouhyouSakuseiJouken.JoukenBunshoNo))
            {
                bunshoNo = data.TyouhyouSakuseiJouken.JoukenBunshoNo.Substring(data.TyouhyouSakuseiJouken.JoukenBunshoNo.Length - 5);
            }
            ((TextBox)tyouhyouInstance.Sections["PageHeader"].Controls["txt文書番号"]).Text = bunshoNo;

            // 発行年月日
            // "[年号]yy年　MM月　dd日"
            DateTime datetime = new();
            bool isDataParse = DateTime.TryParse(data.TyouhyouSakuseiJouken.JoukenHakkoDate, out datetime);
            string dateString = "　　　年　　月　　日";
            if (isDataParse)
            {
                dateString = GetJapaneseDate(datetime, "ggyy年　MM月　dd日");
            }
            ((Label)tyouhyouInstance.Sections["PageHeader"].Controls["lbl日付"]).Text = dateString;

            foreach (TyouhyouShousaiData shousaidata in data.TyouhyouShousaiDatas)
            {
                // PageHeader
                // タイトル
                // "yyyy年度農作物共済に係る特定組合等（組合等）交付金 + 交付申請書 or 返還申請書"
                string nengou = GetJapaneseDate(new DateTime(shousaidata.年産, 1, 1), "ggyy");
                ((TextBox)tyouhyouInstance.Sections["PageHeader"].Controls["Fタイトル"]).Text = nengou + shousaidata.タイトル;

                // 組合等長名
                if (string.IsNullOrEmpty(shousaidata.組合等長名))
                {
                    // "組合長理事（市町村長）　〇〇〇　〇〇〇　印"　※後から筆記で記入できるように空ける
                    shousaidata.組合等長名 = "　　　　　　　　";
                }

                // 文言
                // "　[年号]yy年産水稲及び陸稲（麦）に係る特定組合等（組合等）交付金については、下記の実績に基づき"
                // "金ZZZ,ZZZ,ZZZ,ZZ9円の + 交付を受けたく or 返還をしたく + 申請する。"
                ((TextBox)tyouhyouInstance.Sections["PageHeader"].Controls["txt文言"]).Text = "　" + nengou + shousaidata.文言;

                // 見出し（今回交付申請額又は今回返還申請額）
                // 今回交付申請額又は今回返還申請額
                string txtMidashi = string.Empty;
                string midashiValue = string.Empty;
                if (shousaidata.今回交付申請額又は今回返還申請額 > 0)
                {
                    txtMidashi = "今回交付申請額";
                    midashiValue = shousaidata.今回交付申請額又は今回返還申請額.ToString("N0");
                }
                else
                {
                    txtMidashi = "今回返還申請額";
                    midashiValue = Math.Abs(shousaidata.今回交付申請額又は今回返還申請額).ToString("N0");
                }
                ((TextBox)tyouhyouInstance.Sections["PageHeader"].Controls["txt見出し"]).Text = "\n" + txtMidashi;
                ((TextBox)tyouhyouInstance.Sections["詳細"].Controls["F今回交付申請額又は今回返還申請額"]).Text = midashiValue;
            }

            // 詳細データ
            tyouhyouInstance.DataSource = data.TyouhyouShousaiDatas;
        }

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
