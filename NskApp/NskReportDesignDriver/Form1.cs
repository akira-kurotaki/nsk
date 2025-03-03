using NskReportMain.Models.P1001;
using NskReportMain.Models.P1002;
using NskReportMain.ReportCreators.P1001;
using NskReportMain.ReportCreators.P1002;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Utility;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NskReportLibrary.Core.Consts;
using NskReportLibrary.Core.Utility;
using NskReportMain.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NskReportDesignDriver
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// テストパタン
        /// </summary>
        private decimal testPartern;

        /// <summary>
        /// ロガー
        /// </summary>
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// PDFを作成するクラス
        /// </summary>
        public PdfExport pdfExport = new PdfExport();

        /// <summary>
        /// アプリのDebugパス
        /// </summary>
        public string applicationDirectory = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ActiveControl = numericUpDown1;
            testPartern = numericUpDown1.Value;
            var applicationLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
            applicationDirectory = Path.GetDirectoryName(applicationLocation);
        }

        private void viewer1_Load(object sender, EventArgs e)
        {

        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            testPartern = numericUpDown1.Value;
        }

        /// <summary>
        /// テストデータ出力
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        private void OutputData<T>(List<T> data)
        {
            if (data == null)
            {
                return;
            }

            int i = 0;
            foreach (var itemData in data)
            {
                i += 1;

                StringBuilder sb = new StringBuilder();
                sb.Append($"◆◆◆データ {i.ToString()} 件目◆◆◆" + "\r\n");

                var j = 0;
                foreach (var item in itemData.GetType().GetProperties())
                {
                    j += 1;
                    // プロパティ名を表示
                    sb.Append($"{item.Name}: {item.GetValue(itemData)}" + (itemData.GetType().GetProperties().Length == j ? "" : "\r\n"));
                }

                logger.Debug(sb.ToString());
            }
        }

        /// <summary>
        /// Modelデータ出力
        /// </summary>
        /// <typeparam name="itemData"></typeparam>
        /// <param name="data"></param>
        private void OutputModel<T>(T itemData, bool IsSearch)
        {
            if (itemData == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            if (IsSearch)
            {
                sb.Append("◆◆◆検索条件START◆◆◆" + "\r\n");
            }
            else
            {
                sb.Append("◆◆◆帳票データSTART◆◆◆" + "\r\n");
            }
            var j = 0;
            foreach (var item in itemData.GetType().GetProperties())
            {
                j += 1;
                // プロパティ名を表示
                sb.Append($"{item.Name}: {item.GetValue(itemData)}" + (itemData.GetType().GetProperties().Length == j ? "" : "\r\n"));
            }
            if (IsSearch)
            {
                sb.Append("\r\n" + "◆◆◆検索条件END◆◆◆");
            }
            else
            {
                sb.Append("\r\n" + "◆◆◆帳票データEND◆◆◆");
            }
            logger.Debug(sb.ToString());
        }

        /// <summary>
        /// 帳票出力するフォルダ作成
        /// </summary>
        /// <param name="directory"></param>
        private void CreateReportDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        #region P1001
        /// <summary>
        /// P1001テストソース
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P1001_Click(object sender, EventArgs e)
        {
            try
            {
                // 帳票用モデル作成
                P1001Model model = new P1001Model();
                model.OutputYMD = DateUtil.GetOutputYMD();
                model.OutputHM = DateUtil.GetOutputHM();
                P1001SearchCondition P1001SearchCondition = new P1001SearchCondition();
                List<P1001TableRecord> P1001TableRecordList = new List<P1001TableRecord>(1000000);
                var fileName = string.Empty;

                // 全桁ではない場合
                if (testPartern == 0)
                {
                    fileName = "全て空欄の場合";
                    //保険年度
                    // P1001SearchCondition.JoukenNendo = "R02年度";
                    /*// 都道府県
                    P1001SearchCondition.JoukenTodofukenCd = "14";
                    // 都道府県名称
                    P1001SearchCondition.JoukenTodofukenNm = "神奈川県";
                    // 組合等コード
                    P1001SearchCondition.JoukenKumiaitoCd = "014";
                    // 組合等名称
                    P1001SearchCondition.JoukenKumiaitoNm = "神奈川県農業共済組合○○○○○○○○○⑩○○○○⑤";
                    // 支所コード
                    P1001SearchCondition.JoukenShishoCd = "02";
                    // 支所名称
                    P1001SearchCondition.JoukenShishoNm = "西部支所○○○○○⑥";
                    //市町村コード
                    P1001SearchCondition.JoukenShichosonCd = "14028";
                    //市町村名
                    P1001SearchCondition.JoukenShichosonNm = "足柄下郡湯河原町○○";
                    // 大地区コード
                    P1001SearchCondition.JoukenDaichikuCd = "03";
                    // 大地区名称
                    P1001SearchCondition.JoukenDaichikuNm = "横須賀市①横須賀市①";
                    //小地区コード（開始）
                    P1001SearchCondition.JoukenShochikuCdStart = "0101";
                    //小地区名（開始）
                    P1001SearchCondition.JoukenShochikuNmStart = "小地区開始小地区開始";
                    //小地区コード（終了）
                    P1001SearchCondition.JoukenShochikuCdEnd = "0102";
                    //小地区名（終了）
                    P1001SearchCondition.JoukenShochikuNmEnd = "小地区終了小地区終了";
                    // 加入者管理コード（開始）
                    P1001SearchCondition.JoukenKanyushaCdStart = "1234567890123";
                    // 加入者管理コード（終了）
                    P1001SearchCondition.JoukenKanyushaCdEnd = "1234567890999";*/

                    // 1件
                    for (int i = 1; i <= 1; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            // NendoDisp3 = "R03年度",
                            // KanyushaCd = "1234567890124",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }
                }
                else if (testPartern == 1)
                {
                    fileName = "改ページ確認";
                    // 全桁の場合
                    //保険年度
                    P1001SearchCondition.JoukenNendo = "";
                    // 都道府県
                    P1001SearchCondition.JoukenTodofukenCd = "14";
                    // 都道府県名称
                    P1001SearchCondition.JoukenTodofukenNm = "神奈川県";
                    // 組合等コード
                    P1001SearchCondition.JoukenKumiaitoCd = "014";
                    // 組合等名称
                    P1001SearchCondition.JoukenKumiaitoNm = "神奈川県農業共済組合○○○○○○○○○⑩○○○○⑤";
                    // 支所コード
                    P1001SearchCondition.JoukenShishoCd = "02";
                    // 支所名称
                    P1001SearchCondition.JoukenShishoNm = "西部支所○○○○○⑥";
                    //市町村コード
                    P1001SearchCondition.JoukenShichosonCd = "14028";
                    //市町村名
                    P1001SearchCondition.JoukenShichosonNm = "足柄下郡湯河原町○○";
                    // 大地区コード
                    P1001SearchCondition.JoukenDaichikuCd = "03";
                    // 大地区名称
                    P1001SearchCondition.JoukenDaichikuNm = "横須賀市①横須賀市①";
                    //小地区コード（開始）
                    P1001SearchCondition.JoukenShochikuCdStart = "0101";
                    //小地区名（開始）
                    P1001SearchCondition.JoukenShochikuNmStart = "小地区開始小地区開始";
                    //小地区コード（終了）
                    P1001SearchCondition.JoukenShochikuCdEnd = "0102";
                    //小地区名（終了）
                    P1001SearchCondition.JoukenShochikuNmEnd = "小地区終了小地区終了";
                    // 加入者管理コード（開始）
                    P1001SearchCondition.JoukenKanyushaCdStart = "1234567890123";
                    // 加入者管理コード（終了）
                    P1001SearchCondition.JoukenKanyushaCdEnd = "1234567890999";

                    /* // 1000000件
                     for (int i = 12; i <= 10000; i++)
                     {                        
                         P1001TableRecordList.Add(new P1001TableRecord() { NendoDisp3 = "H31-R01年度", });
                     }*/

                    // 1件
                    for (int i = 1; i <= 1; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "R02年度",
                            KanyushaCd = "1234567890121",
                            HojinFullNm = "氏名又は法人名○①①氏名又は法人名○①①氏名又は法人名○①①",
                            KochiAddress = "耕地住所○耕地住所⑩耕地住所○耕地住所⑩耕地住所○耕地住所⑩耕地住所○耕地住所⑩",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }

                    // 6件
                    for (int i = 1; i <= 6; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "",
                            KanyushaCd = "1234567890125",
                            HojinFullNm = "氏名又は法人名",
                            KochiAddress = "耕地住所○耕地住所⑩",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }

                    // 10件
                    for (int i = 1; i <= 10; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "R03年度",
                            KanyushaCd = "1234567890124",
                            HojinFullNm = "氏名又は法人名②②②",
                            KochiAddress = "耕地住所⑤耕地住所★",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }


                    // 11件
                    for (int i = 1; i <= 11; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "H31-R01年度",
                            KanyushaCd = "1234567890123",
                            HojinFullNm = "ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○",
                            KochiAddress = "ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }

                    // 12件
                    for (int i = 1; i <= 12; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "R04年度",
                            KanyushaCd = "1234567890125",
                            HojinFullNm = "氏名又は法人名①①①氏名又は法人名①①①氏名又は法人名①①①",
                            KochiAddress = "耕地住所⑤耕地住所⑩耕地住所⑤耕地住所⑩耕地住所⑤耕地住所⑩耕地住所⑤耕地住所⑩",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }

                    // 22件
                    for (int i = 1; i <= 22; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "H30年度",
                            KanyushaCd = "1234567890120",
                            HojinFullNm = "氏名又は法人名★①①氏名又は法人名★①①氏名又は法人名★①①",
                            KochiAddress = "耕地住所⑤耕地住所※耕地住所⑤耕地住所※耕地住所⑤耕地住所※耕地住所⑤耕地住所※",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }

                    // 23件
                    for (int i = 1; i <= 23; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "H29年度",
                            KanyushaCd = "1234567890129",
                            HojinFullNm = "氏名又は法人名★①⑩氏名又は法人名★①⑩氏名又は法人名★①⑩",
                            KochiAddress = "耕地住所５耕地住所※耕地住所５耕地住所※耕地住所５耕地住所※耕地住所５耕地住所※",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }
                }
                else if (testPartern == 2)
                {
                    fileName = "全桁ではない、空欄ではない場合";
                    // 全桁の場合
                    //保険年度
                    P1001SearchCondition.JoukenNendo = "";
                    // 都道府県
                    P1001SearchCondition.JoukenTodofukenCd = "14";
                    // 都道府県名称
                    P1001SearchCondition.JoukenTodofukenNm = "神奈川県";
                    // 組合等コード
                    P1001SearchCondition.JoukenKumiaitoCd = "014";
                    // 組合等名称
                    P1001SearchCondition.JoukenKumiaitoNm = "神奈川県農業共済組合";
                    // 支所コード
                    P1001SearchCondition.JoukenShishoCd = "02";
                    // 支所名称
                    P1001SearchCondition.JoukenShishoNm = "西部支所";
                    //市町村コード
                    P1001SearchCondition.JoukenShichosonCd = "14028";
                    //市町村名
                    P1001SearchCondition.JoukenShichosonNm = "足柄下郡湯河原町";
                    // 大地区コード
                    P1001SearchCondition.JoukenDaichikuCd = "03";
                    // 大地区名称
                    P1001SearchCondition.JoukenDaichikuNm = "横須賀市";
                    //小地区コード（開始）
                    P1001SearchCondition.JoukenShochikuCdStart = "0101";
                    //小地区名（開始）
                    P1001SearchCondition.JoukenShochikuNmStart = "小地区開始";
                    //小地区コード（終了）
                    P1001SearchCondition.JoukenShochikuCdEnd = "0102";
                    //小地区名（終了）
                    P1001SearchCondition.JoukenShochikuNmEnd = "小地区終了";
                    // 加入者管理コード（開始）
                    P1001SearchCondition.JoukenKanyushaCdStart = "1234567890123";
                    // 加入者管理コード（終了）
                    P1001SearchCondition.JoukenKanyushaCdEnd = "1234567890999";

                    for (int i = 1; i <= 3; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "R01年度",
                            KanyushaCd = "1234567890123",
                            HojinFullNm = "氏名又は法人名",
                            KochiAddress = "耕地住所⑤耕地住所⑩",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }
                }
                /*else if (testPartern == 2)
                {
                    // 固定出力項目（出力年月日）がない場合
                    model.OutputYMD = string.Empty;

                    fileName = " 固定出力項目（出力年月日）がない場合";
                    //保険年度
                    P1001SearchCondition.JoukenNendo = "R02年度";
                    // 1件
                    for (int i = 1; i <= 1; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "R03年度",
                            KanyushaCd = "1234567890124",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }
                }
                else if (testPartern == 3)
                {
                    // 固定出力項目（出力時分）がない場合
                    model.OutputHM = string.Empty;

                    fileName = "固定出力項目（出力時分）がない場合";
                    //保険年度
                    P1001SearchCondition.JoukenNendo = "R02年度";
                    // 1件
                    for (int i = 1; i <= 1; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "R03年度",
                            KanyushaCd = "1234567890124",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }
                }
                else if (testPartern == 4)
                {
                    // 出力条件がない場合
                    fileName = "出力条件がない場合";
                    P1001SearchCondition = null;
                    // 1件
                    for (int i = 1; i <= 1; i++)
                    {
                        P1001TableRecord P1001TableRecord = new P1001TableRecord
                        {
                            NendoDisp3 = "R03年度",
                            KanyushaCd = "1234567890124",
                        };
                        P1001TableRecordList.Add(P1001TableRecord);
                    }
                }
                else if (testPartern == 5)
                {
                    // 出力対象データがない場合(出力対象=null)
                    fileName = "出力対象データがない場合(出力対象=null)";
                    //保険年度
                    P1001SearchCondition.JoukenNendo = "R02年度";
                    P1001TableRecordList = null;
                }
                else if (testPartern == 6)
                {
                    // 出力対象データがない場合(出力対象件数＝0)
                    fileName = "出力対象データがない場合(出力対象件数＝0)";
                    //保険年度
                    P1001SearchCondition.JoukenNendo = "R02年度";
                }*/

                model.P1001SearchCondition = P1001SearchCondition;
                model.P1001TableRecordList = P1001TableRecordList;
                // LOG START
                OutputModel(model, false);
                OutputModel(model.P1001SearchCondition, true);
                OutputData(model.P1001TableRecordList);
                // LOG END

                // 帳票作成
                var p1001Creator = new P1001Creator();
                var P1001CreatorResult = p1001Creator.CreateReport(0, model);
                P1001CreatorResult.SectionReport = ReportPagerUtil.DrawReportPageNumber(P1001CreatorResult.SectionReport, ReportConst.REPORT_BOTTOM_MARGIN_STANDARD + ReportConst.REPORT_RIGHT_MARGIN_FIVE * 2);

                // 失敗の場合
                if (ReportConst.RESULT_FAILED.Equals(P1001CreatorResult.Result))
                {
                    MessageBox.Show("メッセージID：" + P1001CreatorResult.ErrorMessageId + Environment.NewLine + "メッセージ内容：" + P1001CreatorResult.ErrorMessage);
                }
                else
                {
                    // 帳票出力
                    var directory = Path.Combine(@"C:\Users\999000_0F4724\Desktop\yangyan\P1001\output");
                    CreateReportDirectory(directory);

                    pdfExport.Export(
                        P1001CreatorResult.SectionReport.Document,
                        Path.Combine(directory, fileName + ".pdf"));
                    p1001Creator.DisposeReportMemory();

                    MessageBox.Show("完了");
                }
            }
            catch (Exception ex)
            {
                logger.Error(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region P1002
        /// <summary>
        /// P1002テストソース
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P1002_Click(object sender, EventArgs e)
        {
            try
            {
                var startTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                // 帳票用モデル作成
                List<P1002Model> modelList = new List<P1002Model>();
                var fileName = string.Empty;
                DateTime sysDate = DateUtil.GetSysDateTime();

                // 全桁ではない場合
                if (testPartern == 0)
                {
                    fileName = "全て空欄_チェックボックスチェックOFFの場合";
                    P1002Model model = new P1002Model() {};
                    modelList.Add(model);
                }
                else if (testPartern == 1)
                {
                    fileName = "全桁_チェックボックスチェックONの場合";
                    P1002Model model = new P1002Model()
                    {
                        NendoJa = "令和11",
                        KanyuShinseiYmdJaYY = "令和10",
                        KanyuShinseiYmdMM = DateUtil.FormatMonthByDateTime(sysDate),
                        KanyuShinseiYmdDD = DateUtil.FormatDayByDateTime(sysDate),
                        KochiAddress = "耕地住所ＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○",
                        HojinFullNm = "氏名又は法人名ＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○",
                        KanyushaCd = "1234567890123",
                        TodofukenNm = "都道府県",
                        KumiaitoNm = "組合等ＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮ",
                        ShishoNm = "支所ＮＮＮＮＮＮＮ○",
                        ShichosonNm = "市町村ＮＮＮＮＮＮ○",
                        DaichikuNm = "大地区ＮＮＮＮＮＮ○",
                        ShochikuNm = "小地区ＮＮＮＮＮＮ○",
                        // 耕地面積
                        KouchiMenseki = (decimal)123456789.9,
                        // 耕地形態(畑)
                        IsKouchiKeitaiCdHata = true,
                        // 耕地形態(田)
                        IsKouchiKeitaiCdTa = true,
                        // 耕地形態(その他)
                        IsKouchiKeitaiCdOther = true,
                        // 耕地形態(未選択)
                        IsKouchiKeitaiCdNone = true,
                        // 個人情報の取扱いについて
                        KojinjohoToriatsukaiFlg = true,
                        Biko = "備考ＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○" +
                        "ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○" +
                        "ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○"
                    };

                    modelList.Add(model);
                }
                else if (testPartern == 2)
                {
                    fileName = "全桁ではない、空欄ではない場合";
                    P1002Model model = new P1002Model()
                    {
                        NendoJa = DateUtil.GetReportJapaneseYear(sysDate),
                        KanyuShinseiYmdJaYY = DateUtil.GetReportJapaneseYear(sysDate),
                        KanyuShinseiYmdMM = DateUtil.FormatMonthByDateTime(sysDate),
                        KanyuShinseiYmdDD = DateUtil.FormatDayByDateTime(sysDate),
                        KochiAddress = "ＮＮＮＮＮＮＮＮＮ○",
                        HojinFullNm = "ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○",
                        KanyushaCd = "1234567890123",
                        TodofukenNm = "都道府",
                        KumiaitoNm = "組合等ＮＮＮＮＮ",
                        ShishoNm = "支所ＮＮ○",
                        ShichosonNm = "市町村ＮＮ○",
                        DaichikuNm = "大地区ＮＮ○",
                        ShochikuNm = "小地区Ｎ○",
                        // 耕地面積
                        KouchiMenseki = 12345,
                        // 耕地形態(畑)
                        IsKouchiKeitaiCdHata = true,
                        // 耕地形態(田)
                        IsKouchiKeitaiCdTa = false,
                        // 耕地形態(その他)
                        IsKouchiKeitaiCdOther = true,
                        // 耕地形態(未選択)
                        IsKouchiKeitaiCdNone = false,
                        // 個人情報の取扱いについて
                        KojinjohoToriatsukaiFlg = true,
                        Biko = "備考ＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○ＮＮＮＮＮＮＮＮＮ○",
                    };

                    modelList.Add(model);
                }
                else if (testPartern == 3)
                {
                    // 出力対象データがない場合(出力対象=null)
                    fileName = "出力対象データがない場合(出力対象=null)";
                    modelList = null;
                }
                else if (testPartern == 4)
                {
                    // 出力対象データがない場合(出力対象件数＝0)
                    fileName = "出力対象データがない場合(出力対象件数＝0)";
                }

                // LOG START
                logger.Debug("-----------" + fileName + "----BEGIN-----" + startTime);
                OutputData(modelList);
                // LOG END

                // 帳票作成
                var p1002Creator = new P1002Creator();
                var P1002CreatorResult = p1002Creator.CreateReport(0, modelList);
                // ページ番号を描画する
                P1002CreatorResult.SectionReport = ReportPagerUtil.DrawReportPageNumber(P1002CreatorResult.SectionReport, ReportConst.REPORT_BOTTOM_MARGIN_STANDARD);

                // 失敗の場合
                if (ReportConst.RESULT_FAILED.Equals(P1002CreatorResult.Result))
                {
                    MessageBox.Show("メッセージID：" + P1002CreatorResult.ErrorMessageId + Environment.NewLine + "メッセージ内容：" + P1002CreatorResult.ErrorMessage);
                }
                else
                {
                    // 帳票出力
                    var directory = Path.Combine(@"C:\Users\999000_0F4724\Desktop\yangyan\P1002\output");
                    CreateReportDirectory(directory);

                    pdfExport.Export(
                        P1002CreatorResult.SectionReport.Document,
                        Path.Combine(directory, fileName + ".pdf"));
                    p1002Creator.DisposeReportMemory();
                    logger.Debug("-----------" + fileName + "----END-------" + startTime);
                    MessageBox.Show("完了");
                }
            }
            catch (Exception ex)
            {
                logger.Error(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
