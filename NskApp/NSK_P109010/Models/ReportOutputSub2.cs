namespace NSK_P109010.Models
{
    /// <summary>
    /// 実際に帳票レポートに出力するデータ（SUB2）
    /// </summary>
    public class ReportOutputSub2
    {
        public string 類短縮名称 { get; set; } = string.Empty;
        public string 受託者から { get; set; } = string.Empty;
        public string 加入者から { get; set; } = string.Empty;
        public string 売渡受託者等証明 { get; set; } = string.Empty;
        public string 青色申告書 { get; set; } = string.Empty;
        public string 確定申告 { get; set; } = string.Empty;
        public string その他 { get; set; } = string.Empty;
    }
}
