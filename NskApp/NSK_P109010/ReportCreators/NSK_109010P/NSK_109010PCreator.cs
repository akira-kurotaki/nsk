using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using GrapeCity.ActiveReports.SectionReportModel;
using NSK_P109010.Models;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using NskReportMain.Reports;

namespace NSK_P109010.ReportCreators.NSK_109010P
{
    /// <summary>
    /// 引受通知書収量建て総計作成処理
    /// </summary>
    public class NSK_109010PCreator : ReportCreator
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static NSK_109010PCreator()
        {

        }

        /// <summary>
        /// 引受通知書収量建て総計作成処理
        /// </summary>
        /// <param name="jouken">出力条件</param>
        /// <param name="outputDatas">出力対象データ</param>
        /// <returns></returns>
        public static CreatorResult CreateReport(BatchJouken jouken, NSK_109010PModel outputDatas)
        {
            // 変数
            // Creatorの実行結果
            CreatorResult creatorResult = new()
            {
                Result = ReportConst.RESULT_FAILED,
                SectionReport = null
            };

            // １．引数チェックする。
            IsRequired(jouken, outputDatas);

            // ２．帳票を作成する。
            // 帳票オブジェクト
            BaseSectionReport reportMain = new(@"Reports\P109010\P109010ReportMain.rpx");
            BaseSectionReport reportSub1 = new(@"Reports\P109010\P109010ReportSub1.rpx");
            BaseSectionReport reportSub2 = new(@"Reports\P109010\P109010ReportSub2.rpx");

            // 帳票のデータ設定
            SetDatas(jouken, outputDatas, ref reportMain, reportSub1, reportSub2);

            // ２．３．設定したデータで帳票を作成する。
            if (jouken.JoukenMoushikomishoShutsuryoku.Equals(CoreConst.FLG_ON))
            {
                reportMain.MaxPages = 1;
            }
            reportMain.Run();

            // [変数：実行結果].ResultにReportConst.RESULT_SUCCESS（成功：0）を設定する。
            // [変数：実行結果].SectionReportに[変数：帳票インスタンス]を設定する。
            creatorResult.Result = ReportConst.RESULT_SUCCESS;
            creatorResult.SectionReport = reportMain;

            // ３．[変数：実行結果]を返す。
            return creatorResult;
        }

        /// <summary>
        /// 引数のチェック
        /// </summary>
        /// <param name="jouken">出力条件</param>
        /// <param name="outputDatas">出力対象データ</param>
        private static void IsRequired(BatchJouken jouken, NSK_109010PModel outputDatas)
        {
            // １．１．出力条件が未入力の場合、
            if (jouken == null)
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054"、引数{0} ： "出力条件")
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "出力条件"));
            }

            // １．２．出力対象データがnullまたはデータの件数が0件の場合、
            if (outputDatas == null)
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054"、引数{0} ： "出力対象データ")
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "出力対象データ"));
            }
            if (outputDatas.TyouhyouDatasMain.Count == 0 || outputDatas.TyouhyouDatasSub1.Count == 0 || outputDatas.TyouhyouDatasSub2.Count == 0)
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054"、引数{0} ： "出力対象データ")
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "出力対象データ"));
            }
        }

        /// <summary>
        /// 出力対象データを帳票の対応項目に設定する。
        /// </summary>
        /// <param name="jouken">出力条件</param>
        /// <param name="outputDatas">出力対象データ</param>
        /// <param name="reportMain">メインレポート</param>
        /// <param name="reportSub1">SUB1レポート</param>
        /// <param name="reportSub2">SUB2レポート</param>
        private static void SetDatas(BatchJouken jouken, NSK_109010PModel outputDatas, ref BaseSectionReport reportMain, BaseSectionReport reportSub1, BaseSectionReport reportSub2)
        {
            // ２．１．引数：出力条件を帳票の対応項目に設定する。
            // 「基本設計_帳票項目定義書_NSK_109010P_加入申込書兼変更届出書」の備考欄にデータ編集の記載がある項目についてはこのタイミングでデータ編集を行い、対応項目に設定する。
            // ◆組合員等コードヘッダー
            if (jouken.JoukenMoushikomishoShutsuryoku.Equals(CoreConst.FLG_ON))    // 0:無　1:有
            {
                ((GroupHeader)reportMain.Sections["GroupHeader2"]).Visible = false;
                ((Detail)reportMain.Sections["詳細"]).Visible = false;
            }

            // ２．２．引数：出力条件を帳票の対応項目に設定する。
            // 「基本設計_帳票項目定義書_NSK_109010P_加入申込書兼変更届出書」の備考欄にデータ編集の記載がある項目についてはこのタイミングでデータ編集を行い、対応項目に設定する。
            string tokuyakuflag = string.Empty;
            List<ReportOutputMain> outputMainLists = new();
            foreach (TyouhyouDataMain dataMain in outputDatas.TyouhyouDatasMain)
            {
                tokuyakuflag = dataMain.継続特約フラグ;
                // 1.申込者欄
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt申込者カナ"]).Text = dataMain.申込者カナ;
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt申込者カナ2"]).Text = dataMain.申込者カナ;
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt申込者氏名"]).Text = dataMain.申込者氏名;
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt申込者氏名2"]).Text = dataMain.申込者氏名;
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt郵便番号"]).Text = 
                    "（〒　" + dataMain.郵便番号.Substring(0, 3) + "　-　" + dataMain.郵便番号.Substring(dataMain.郵便番号.Length - 4, 4) + "　）";
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt郵便番号2"]).Text =
                    "（〒　" + dataMain.郵便番号.Substring(0, 3) + "　-　" + dataMain.郵便番号.Substring(dataMain.郵便番号.Length - 4, 4) + "　）";
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt住所"]).Text = dataMain.住所;
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt住所2"]).Text = dataMain.住所;
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt電話番号"]).Text = dataMain.電話番号;
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt電話番号2"]).Text = dataMain.電話番号;

                // ◆組合員等コードヘッダー
                string jidouKeizokuTokuyaku = string.Empty;
                if (dataMain.継続特約フラグ.Equals(CoreConst.FLG_ON))
                {
                    jidouKeizokuTokuyaku = "有り";
                }
                else
                {
                    jidouKeizokuTokuyaku = "無し";
                }
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt自動継続特約"]).Text = jidouKeizokuTokuyaku;

                // 加入形態未取得状態
                switch (dataMain.加入形態)
                {
                    case "1":     // 個人
                        ((Label)reportMain.Sections["GroupHeader1"].Controls["lblChkBx個人"]).Text = "■";
                        ((Label)reportMain.Sections["GroupHeader1"].Controls["lblChkBx個人2"]).Text = "■";
                        break;
                    case "2":     // 農作物共済資格団体
                        ((Label)reportMain.Sections["GroupHeader1"].Controls["lblChkBx農作物共済資格団体"]).Text = "■";
                        ((Label)reportMain.Sections["GroupHeader1"].Controls["lblChkBx農作物共済資格団体2"]).Text = "■";
                        break;
                    case "10":    // 法人
                        ((Label)reportMain.Sections["GroupHeader1"].Controls["lblChkBx法人"]).Text = "■";
                        ((Label)reportMain.Sections["GroupHeader1"].Controls["lblChkBx法人2"]).Text = "■";
                        break;
                    default:    // 上記以外
                        break;
                }

                // 耕種BCP区分
                if (dataMain.koshu_bcp_kbn.Equals(CoreConst.FLG_ON))
                {
                    ((CheckBox)reportSub2.Sections["レポートフッター"].Controls["chkbx農業版ＢＣＰの実施"]).Checked = true;
                }

                // ◆加入申込書兼変更届出書Sub1（２．引受方式等）
                if (jouken.JoukenRuibetsuNiniShutsuryoku.Equals(CoreConst.FLG_OFF))
                {
                    if (!dataMain.継続特約フラグ.Equals(CoreConst.FLG_ON))
                    {
                        ((TextBox)reportSub1.Sections["詳細"].Controls["txt類区分"]).Visible = false;
                        ((TextBox)reportSub1.Sections["詳細"].Controls["txt引受方式"]).Visible = false;
                        ((TextBox)reportSub1.Sections["詳細"].Controls["txt補償割合"]).Visible = false;
                        ((TextBox)reportSub1.Sections["詳細"].Controls["txt一筆半損特約の有無"]).Visible = false;
                        ((TextBox)reportSub1.Sections["詳細"].Controls["txt選択順位"]).Visible = false;
                        ((TextBox)reportSub1.Sections["詳細"].Controls["txt備考"]).Visible = false;
                    }
                }

                // データ存在無かつデータ表示設定の場合
                if (outputDatas.TyouhyouDatasSub1.Count == 0 && dataMain.継続特約フラグ.Equals(CoreConst.FLG_ON))
                {
                    ((Line)reportSub2.Sections["詳細"].Controls["斜線_ヘッダー"]).Visible = true;
                    ((Line)reportSub2.Sections["詳細"].Controls["斜線_詳細"]).Visible = true;
                    ((Line)reportSub2.Sections["詳細"].Controls["斜線_フッター1"]).Visible = true;
                    ((Line)reportSub2.Sections["詳細"].Controls["斜線_フッター2"]).Visible = true;
                }

                // ◆その他のデータ
                // 見出し
                // 和暦年産＋"年産"＋"　"＋共済目的名称＋"共済加入申込書兼変更届出書"
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt見出し"]).Text =
                    DateUtil.GetReportJapaneseYear(new DateTime(dataMain.年産, 1, 1)) + "年産　" + dataMain.共済目的名称 + "共済加入申込書兼変更届出書";

                // 宛名
                // 組合代表者名＋"長"
                ((TextBox)reportMain.Sections["GroupHeader1"].Controls["txt宛名"]).Text = dataMain.組合代表者名 + "長";

                // 乾燥調製作業の受託者チェック
                ((CheckBox)reportSub2.Sections["レポートフッター"].Controls["chkbx乾燥調製作業の受託者"]).Text =
                    "　当該乾燥調製作業の受託者（又は売渡受託者等）が、当該申込者が作業を委託した農作物の乾燥調製結果に関する書類（又は売渡受託者等が保管する帳簿、伝票その他必要な資料の写し）を、"
                    + dataMain.組合名称 + "に提供又は提示することの同意を得ていることを確約します。";

                // 基準単収の設定方法チェック
                ((CheckBox)reportSub2.Sections["レポートフッター"].Controls["chkbx基準単収の設定方法"]).Text =
                    "　災害が近年連続して発生している等により近年の上記証明書類により基準単収を設定することが適当ではないことから、"
                    + dataMain.組合名称 + "が別の方法により本年産の基準単収を設定することを申し出ます。";

                // ◆詳細
                string ruiKbn = string.Empty;
                if (!dataMain.共済目的コード.Equals("20"))
                {
                    ruiKbn = dataMain.類区分 + "類" + dataMain.類区分名称;
                }

                // 品種
                // [品種コード] + " " + [品種名称]
                outputMainLists.Add(new ReportOutputMain()
                {
                    耕地番号 = dataMain.耕地番号,
                    分筆番号 = dataMain.分筆番号,
                    市町村名 = dataMain.市町村名,
                    地名地番 = dataMain.地名地番,
                    耕地面積 = dataMain.耕地面積.ToString(),
                    引受面積 = dataMain.引受面積.ToString(),
                    転作等面積 = dataMain.転作等面積.ToString(),
                    類区分 = ruiKbn,
                    品種 = dataMain.品種コード + "　" + dataMain.品種名称,
                    田畑区分 = dataMain.田畑区分,
                    備考 = dataMain.備考,
                    収量等級 = dataMain.収量等級,
                    参酌 = dataMain.参酌.ToString()
                });
            }

            // その他のデータ（詳細）
            // 詳細データの要素数上限
            int ARRAY_INDEX_NUM_MAIN = 34;
            int ARRAY_INDEX_NUM_SUB1 = 6;
            int ARRAY_INDEX_NUM_SUB2 = 8;

            ReportOutputMain[] ArrayMain = new ReportOutputMain[ARRAY_INDEX_NUM_MAIN];
            // 過不足要素を調整
            if (outputMainLists.Count <= ARRAY_INDEX_NUM_MAIN)
            {
                // 要素数が等しい、または少ない場合
                outputMainLists.CopyTo(ArrayMain);
                for (int i = outputMainLists.Count; i < ARRAY_INDEX_NUM_MAIN; i++)
                {
                    // ここで既定値を設定
                    ArrayMain[i] = new ReportOutputMain();
                }
            }
            else
            {
                // 要素数が多い場合
                for (int i = 0; i < outputMainLists.Count; i++)
                {
                    ArrayMain[i] = outputMainLists[i];
                }
            }
            reportMain.DataSource = ArrayMain;

            List<ReportOutputSub1> outputSub1Lists = new();
            foreach (TyouhyouDataSub1 dataSub1 in outputDatas.TyouhyouDatasSub1)
            {
                // 一筆半損特約の有無
                string tokuyakuUmu = string.Empty;
                if (dataSub1.特約区分.Equals("4"))
                {
                    tokuyakuUmu = "有り";
                }
                else
                {
                    tokuyakuUmu = "無し";
                }

                outputSub1Lists.Add(new ReportOutputSub1()
                {
                    類短縮名称 = dataSub1.類短縮名称,
                    引受方式 = dataSub1.引受方式,
                    補償割合 = (dataSub1.補償割合 / 10).ToString(),
                    一筆半損特約の有無 = tokuyakuUmu,
                    選択順位 = dataSub1.選択順位.ToString(),
                    備考 = dataSub1.備考
                });
            }
            ReportOutputSub1[] ArraySub1 = new ReportOutputSub1[ARRAY_INDEX_NUM_SUB1];
            // 過不足要素を調整
            if (outputSub1Lists.Count <= ARRAY_INDEX_NUM_SUB1)
            {
                // 要素数が等しい、または少ない場合
                outputSub1Lists.CopyTo(ArraySub1);
                for (int i = outputSub1Lists.Count; i < ARRAY_INDEX_NUM_SUB1; i++)
                {
                    // ここで既定値を設定
                    ArraySub1[i] = new ReportOutputSub1();
                }
            }
            else
            {
                // 要素数が多い場合
                for (int i = 0; i < ARRAY_INDEX_NUM_SUB1; i++)
                {
                    ArraySub1[i] = outputSub1Lists[i];
                }
            }
            reportSub1.DataSource = ArraySub1;

            // ◆加入申込書兼変更届出書Sub2（３．全相殺方式）
            List<ReportOutputSub2> outputSub2Lists = new();
            foreach (TyouhyouDataSub2 dataSub2 in outputDatas.TyouhyouDatasSub2)
            {
                ReportOutputSub2 outputSub2 = new ReportOutputSub2();
                switch (dataSub2.加入申込区分)
                {
                    case "1":
                        if (jouken.JoukenRuibetsuNiniShutsuryoku.Equals(CoreConst.FLG_OFF) && !tokuyakuflag.Equals(CoreConst.FLG_ON))
                        {

                        }
                        else
                        {
                            outputSub2.類短縮名称 = dataSub2.類短縮名称;
                            outputSub2.受託者から = "〇";
                        }
                        break;
                    case "2":
                        outputSub2.類短縮名称 = dataSub2.類短縮名称;
                        outputSub2.加入者から = "〇";
                        break;
                    case "3":
                        outputSub2.類短縮名称 = dataSub2.類短縮名称;
                        outputSub2.売渡受託者等証明 = "〇";
                        break;
                    case "4":
                        outputSub2.類短縮名称 = dataSub2.類短縮名称;
                        outputSub2.青色申告書 = "〇";
                        break;
                    case "5":
                        outputSub2.類短縮名称 = dataSub2.類短縮名称;
                        outputSub2.その他 = "〇";
                        break;
                    case "6":
                        outputSub2.類短縮名称 = dataSub2.類短縮名称;
                        outputSub2.確定申告 = "〇";
                        break;
                    default:
                        outputSub2.類短縮名称 = dataSub2.類短縮名称;
                        break;
                }
                outputSub2Lists.Add(outputSub2);
            }
            ReportOutputSub2[] ArraySub2 = new ReportOutputSub2[ARRAY_INDEX_NUM_SUB2];
            // 過不足要素を調整
            if (outputDatas.TyouhyouDatasSub2.Count <= ARRAY_INDEX_NUM_SUB2)
            {
                // 要素数が等しい、または少ない場合
                outputSub2Lists.CopyTo(ArraySub2);
                for (int i = outputSub2Lists.Count; i < ARRAY_INDEX_NUM_SUB2; i++)
                {
                    // ここで既定値を設定
                    ArraySub2[i] = new ReportOutputSub2();
                }
            }
            else
            {
                // 要素数が多い場合
                for (int i = 0; i < ARRAY_INDEX_NUM_SUB2; i++)
                {
                    ArraySub2[i] = outputSub2Lists[i];
                }
            }
            reportSub2.DataSource = ArraySub2;

            // 帳票を結合させる
            ((SubReport)reportMain.Sections["GroupHeader1"].Controls["subForm1"]).Report = reportSub1;
            ((SubReport)reportMain.Sections["GroupHeader1"].Controls["subForm2"]).Report = reportSub2;
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
