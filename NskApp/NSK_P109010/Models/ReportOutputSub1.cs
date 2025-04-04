namespace NSK_P109010.Models
{
    /// <summary>
    /// 実際に帳票レポートに出力するデータ（SUB1）
    /// </summary>
    public class ReportOutputSub1
    {
        public string 類短縮名称 { get; set; } = string.Empty;
        public string 引受方式 { get; set; } = string.Empty;
        public string 補償割合 { get; set; } = string.Empty;
        public string 一筆半損特約の有無 { get; set; } = string.Empty;
        public string 選択順位 { get; set; } = string.Empty;
        public string 備考 { get; set; } = string.Empty;
    }
}
