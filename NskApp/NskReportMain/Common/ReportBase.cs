using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskReportLibrary.Core.Consts;

namespace NskReportMain.Common
{
    /// <summary>
    /// 帳票ベースクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/11
    /// 作成者：KAN RI
    /// </remarks>
    public partial class ReportBase : GrapeCity.ActiveReports.SectionReport
    {
        /// <summary>
        /// 1ページに収まる件数
        /// </summary>
        protected int pageRecordCount = 0;

        /// <summary>
        /// カレントレコードの件数目
        /// </summary>
        protected int recordIndex = 0;

        /// <summary>
        /// 帳票にバインドするデータソースのデータ総件数
        /// </summary>
        protected int recordCount = 0;

        public ReportBase()
        {
            ReportStart += new EventHandler(ReportBase_ReportStart);
        }

        /// <summary>
        /// ReportStartイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReportBase_ReportStart(object sender, EventArgs e)
        {
            // 帳票デザインに影響（プレビューの赤線が出る）しないように、一旦コメントアウトする
            // 仮想プリンタを設定する（帳票出力の高速化のため）
            Document.Printer.PrinterName = string.Empty;
        }

        /// <summary>
        /// 会長の印影のパスを取得するメソッド
        /// </summary>
        /// <returns>会長の印影のパス</returns>
        //protected string GetKaichouImagePath()
        //{
        //    var path = string.Empty;

        //    //if (File.Exists(Path.Combine(HostingEnvironment.ApplicationPhysicalPath ?? string.Empty, @"bin\Images\kaichou.jpg")))
        //    //{
        //    //    // リアルタイム帳票の場合
        //    //    path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath ?? string.Empty, @"bin\Images\kaichou.jpg");
        //    //}
        //    //else
        //    //{
        //    //    // バッチ帳票の場合
        //    //    FileInfo file = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //    //    path = Path.Combine(file.DirectoryName, @"Images\kaichou.jpg");
        //    //}

        //    // ★ひとまず設定ファイルから取得するように修正する。設定ファイルにはフルパスを設定するようにした方が良いか？動作確認未★
        //    FileInfo file = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory);
        //    path = Path.Combine(file.DirectoryName, ConfigUtil.Get("KaichouImagePath"));

        //    if (!File.Exists(path))
        //    {
        //        throw new AppException("ME01136", MessageUtil.Get("ME01136", ReportConst.STRING_KAICHO));
        //    }

        //    return path;
        //}

        /// <summary>
        /// 連合会の印影のパスを取得するメソッド
        /// </summary>
        /// <returns>連合会の印影のパス</returns>
        //protected string GetRengoukaiImagePath()
        //{
        //    var path = string.Empty;

        //    //if (File.Exists(Path.Combine(HostingEnvironment.ApplicationPhysicalPath ?? string.Empty, @"bin\Images\rengoukai.jpg")))
        //    //{
        //    //    // リアルタイム帳票の場合
        //    //    path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath ?? string.Empty, @"bin\Images\rengoukai.jpg");
        //    //}
        //    //else
        //    //{
        //    //    // バッチ帳票の場合
        //    //    FileInfo file = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //    //    path = Path.Combine(file.DirectoryName, @"Images\rengoukai.jpg");
        //    //}

        //    // ★ひとまず設定ファイルから取得するように修正する。設定ファイルにはフルパスを設定するようにした方が良いか？動作確認未★
        //    FileInfo file = new FileInfo(System.AppDomain.CurrentDomain.BaseDirectory);
        //    path = Path.Combine(file.DirectoryName, ConfigUtil.Get("RengoukaiImagePath"));

        //    if (!File.Exists(path))
        //    {
        //        throw new AppException("ME01136", MessageUtil.Get("ME01136", ReportConst.STRING_REGOKAI));
        //    }

        //    return path;
        //}
    }
}
