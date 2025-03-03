using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NskReportFontConfirmation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 帳票デザインファイル選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "*.rpx|*.rpx";
            ofd.Title = "帳票デザインファイルを選択してください";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                TxtFilePath.Text = ofd.FileName;
            }
        }

        /// <summary>
        /// 帳票フォント書体・フォントサイズ整理を実行する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcute_Click(object sender, EventArgs e)
        {
            try
            {
                TxtLog.Text = string.Empty;

                // 選択したzipファイルパス
                var filePath = TxtFilePath.Text;

                // 入力チェック
                TxtLog.Text += "入力チェック中...";
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("帳票デザインファイルを選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(filePath))
                {
                    MessageBox.Show("選択した帳票デザインファイルが存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TxtLog.Text += "\r\n" + "ソースコード解析中...";

                // ソースファイルの行単位リスト
                var sectionControls = ReadFile(filePath);

                // フォーマット済みのフォント
                List<SourceDatas> fontList = new List<SourceDatas>();

                foreach (var item in sectionControls)
                {
                    List<SourceDatas> fontListSec = new List<SourceDatas>();
                    List<string> blockSource = item.Value;
                    foreach (var lineSource in blockSource)
                    {
                        if (lineSource.Trim().StartsWith(@"<Control"))
                        {
                            var top = string.Empty;
                            var left = string.Empty;
                            var dataField = string.Empty;
                            var text = string.Empty;
                            var fontFamily = string.Empty;
                            var fontSize = string.Empty;

                            string pattern = @"(\w+)=""([^""]*)""";
                            MatchCollection matches = Regex.Matches(lineSource.Trim(), pattern);
                            foreach (Match match in matches)
                            {
                                var lineSourceItem = match.Value;
                                var lineTango = lineSourceItem.Split('=');
                                if (lineTango[0].Trim().StartsWith("Top"))
                                {
                                    top = TwipsToCm(lineTango[1].Trim('"').Trim());
                                }

                                if (lineTango[0].Trim().StartsWith("Left"))
                                {
                                    left = TwipsToCm(lineTango[1].Trim('"').Trim());
                                }

                                if (lineTango[0].Trim().StartsWith("Name"))
                                {
                                    dataField = lineTango[1].Trim('"').Trim();
                                }

                                if (lineTango[0].Trim().StartsWith("DataField"))
                                {
                                    dataField = lineTango[1].Trim('"').Trim();
                                }

                                if (lineTango[0].Trim().StartsWith("Text") || lineTango[0].Trim().StartsWith("Caption"))
                                {
                                    text = lineTango[1];
                                }

                                if (lineTango[0].Trim().StartsWith("Style"))
                                {
                                    var styleList = lineTango[1].Trim('"').Split(';');
                                    foreach (var styleItem in styleList)
                                    {
                                        if (styleItem.Trim().StartsWith("font-family"))
                                        {
                                            fontFamily = styleItem.Split(':')[1].Trim();
                                        }
                                        if (styleItem.Trim().StartsWith("font-size"))
                                        {
                                            fontSize = styleItem.Split(':')[1].Trim();
                                        }
                                    }
                                }
                            }

                            fontListSec.Add(new SourceDatas()
                            {
                                top = top,
                                left = left,
                                dataField = dataField,
                                text = text,
                                fontSize = fontSize,
                                fontFamily = fontFamily,
                            });
                        }
                    }

                    var fontListOrdered = new List<SourceDatas>();
                    fontListOrdered = fontListSec.OrderBy(t => decimal.Parse(t.top)).ThenBy(t => decimal.Parse(t.left)).ToList();
                    fontList.AddRange(fontListOrdered);
                }

                TxtLog.Text += "\r\n" + "ソースコード解析完了...";
                TxtLog.Text += "\r\n" + "解析ファイル作成中...";

                OutPutEx(fontList, filePath);

                MessageBox.Show("処理完了。");
            }
            catch (Exception ex)
            {
                TxtLog.Text += "\r\n" + ex.Message;
                TxtLog.Text += ex.InnerException == null ? string.Empty : "\r\n" + ex.InnerException.ToString();
                TxtLog.Text += ex.StackTrace == null ? string.Empty : "\r\n" + ex.StackTrace;
            }
        }

        /// <summary>
        /// ソースファイル読み込み
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private Dictionary<int, List<string>> ReadFile(string filePath)
        {
            var sectionControls = new Dictionary<int, List<string>>();

            string lineSource = "";

            using (StreamReader sr = new StreamReader(
                filePath, Encoding.GetEncoding("utf-8")))
            {
                var sectionKey = 1;
                var sectionFlag = false;

                while ((lineSource = sr.ReadLine()) != null)
                {
                    // セクション単位のコントローラ情報取得↓
                    if (lineSource.Trim().Contains(@"<Section Type="))
                    {
                        if (sectionFlag == true) {
                            sectionKey++;
                        }
                        sectionFlag = true;
                        sectionControls.Add(sectionKey, new List<string>());
                    }
                    else if (sectionFlag == true)
                    {
                        if (lineSource.Contains(@"</Section>") || lineSource.Contains(@"</Sections>"))
                        {
                            sectionFlag = false;
                            sectionKey++;
                        }
                        else
                        {
                            sectionControls[sectionKey].Add(lineSource.Trim());
                        }
                    }
                    // セクション単位のコントローラ情報取得↑
                }
            }

            return sectionControls;
        }

        /// <summary>
        /// twipからセンチに変換
        /// </summary>
        /// <param name="inch"></param>
        /// <returns></returns>
        public static string TwipsToCm(string twipsStr)
        {
            double twips;
            if (!double.TryParse(twipsStr, out twips))
            {
                throw new ArgumentException("Invalid twips value", nameof(twipsStr));
            }

            // 1 twip = 1/1440 インチ
            // 1 インチ = 2.54 cm
            double inches = twips / 1440.0;
            double cm = inches * 2.54;

            return Math.Round(cm, 3).ToString();
        }

        /// <summary>
        /// Excel出力
        /// </summary>
        /// <param name="fontList"></param>
        private void OutPutEx(List<SourceDatas> fontList, string filePath)
        {
            //Excelオブジェクトの初期化
            Microsoft.Office.Interop.Excel.Application ExcelApp = null;
            Microsoft.Office.Interop.Excel.Workbooks wbs = null;
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            Microsoft.Office.Interop.Excel.Sheets shs = null;
            Microsoft.Office.Interop.Excel.Worksheet ws = null;

            try
            {
                //Excelシートのインスタンスを作る
                ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                ExcelApp.Visible = false;
                wbs = ExcelApp.Workbooks;
                wb = wbs.Add();

                shs = wb.Sheets;
                ws = shs[1];
                ws.Select(Type.Missing);
                ws.Rows.AutoFit();
                ws.Range["A1", "H10000"].WrapText = true;
                ws.Range["A1", "H10000"].Font.Name = "ＭＳ Ｐゴシック";
                ws.Range["A1", "H10000"].Font.Size = "8";
                ws.Range["A2", "F2"].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                ws.Range["G2", "H2"].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                ws.Range["A2", "H2"].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                ws.Cells[1, 5] = "法定帳票本文：11ポイント\r\n一覧帳票本文：10ポイント\r\n上記以外のは黄色掛け";
                ws.Cells[1, 7] = "確認し、確認結果を入力する";
                ws.Cells[1, 8] = "確認し、確認結果を入力する";

                ws.Cells[1, 1] = "位置の単位はtwipsからセンチ(cm)に変換したため、誤差がある";
                ws.Cells[1, 2] = "位置の単位はtwipsからセンチ(cm)に変換したため、誤差がある";
                ws.Cells[2, 1] = "縦位置（センチ）";
                ws.Cells[2, 2] = "横位置（センチ）";
                ws.Cells[1, 3] = "DataFieldがある場合DataField\r\nDataFieldがない場合Name";
                ws.Cells[2, 3] = "帳票要素Name/DataField";
                ws.Cells[2, 4] = "テキスト内容";
                ws.Cells[2, 5] = "フォントサイズ";
                ws.Cells[2, 6] = "フォント書体";
                ws.Cells[2, 7] = "帳票標準確認";
                ws.Cells[2, 8] = "設計書確認";

                ws.Cells[2, 1].ColumnWidth = 12;
                ws.Cells[2, 2].ColumnWidth = 12;
                ws.Cells[2, 3].ColumnWidth = 20;
                ws.Cells[2, 4].ColumnWidth = 50;
                ws.Cells[2, 5].ColumnWidth = 15;
                ws.Cells[2, 6].ColumnWidth = 15;
                ws.Cells[2, 7].ColumnWidth = 15;
                ws.Cells[2, 8].ColumnWidth = 15;

                for (int i = 0; i < fontList.Count; i++)
                {
                    ws.Cells[i + 3, 1] = fontList[i].top;
                    ws.Cells[i + 3, 2] = fontList[i].left;
                    ws.Cells[i + 3, 3] = fontList[i].dataField;
                    ws.Cells[i + 3, 4] = fontList[i].text;

                    ws.Cells[i + 3, 5] = fontList[i].fontSize;
                    // 法定帳票の場合
                    if (this.checkBox1.Checked) {
                        if (!"11pt".Equals(fontList[i].fontSize))
                        {
                            ws.Cells[i + 3, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                    }
                    else
                    {
                        if (!"10pt".Equals(fontList[i].fontSize))
                        {
                            ws.Cells[i + 3, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                    }

                    ws.Cells[i + 3, 6] = fontList[i].fontFamily;
                    if (!fontList[i].fontFamily.StartsWith("ＭＳ ゴシック"))
                    {
                        ws.Cells[i + 3, 6].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    }
                }

                //excelファイルの保存
                var excelFile = filePath.Replace("rpx", "xlsx");
                wb.SaveAs(excelFile);
                wb.Close(false);
                ExcelApp.Quit();

                TxtLog.Text += "\r\n" + "解析ファイル作成完了...";
                TxtLog.Text += "\r\n" + "解析ファイルパス：" + excelFile;
            }
            finally
            {
                //Excelのオブジェクトを開放し忘れているとプロセスが落ちないため注意
                Marshal.ReleaseComObject(ws);
                Marshal.ReleaseComObject(shs);
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(wbs);
                Marshal.ReleaseComObject(ExcelApp);
                ws = null;
                shs = null;
                wb = null;
                wbs = null;
                ExcelApp = null;

                GC.Collect();
            }
        }
    }

    public class SourceDatas
    {
        public string top { get; set; }
        public string left { get; set; }
        public string dataField { get; set; }
        public string text { get; set; }
        public string fontSize { get; set; }
        public string fontFamily { get; set; }
        public string contrlType { get; set; }
    }
}
