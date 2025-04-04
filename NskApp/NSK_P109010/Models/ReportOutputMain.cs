namespace NSK_P109010.Models
{
    /// <summary>
    /// 実際に帳票レポートに出力するデータ（MAIN）
    /// </summary>
    public class ReportOutputMain
    {
        public string 耕地番号 {  get; set; } = string.Empty;
        public string 分筆番号 {  set; get; } = string.Empty;
        public string 市町村名 { set; get; } = string.Empty;
        public string 地名地番 { set; get; } = string.Empty;
        public string 耕地面積 { set; get; } = string.Empty;
        public string 引受面積 { set; get; } = string.Empty;
        public string 転作等面積 { set; get; } = string.Empty;
        public string 類区分 { set; get; } = string.Empty;
        public string 品種 { set; get; } = string.Empty;
        public string 田畑区分 { set; get; } = string.Empty;
        public string 備考 { set; get; } = string.Empty;
        public string 収量等級 { set; get; } = string.Empty;
        public string 参酌 { set; get; } = string.Empty;
    }
}
