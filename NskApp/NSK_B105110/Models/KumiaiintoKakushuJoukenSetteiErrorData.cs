namespace NSK_B105110.Models
{
    /// <summary>
    /// 組合員等各種条件設定エラーデータ
    /// </summary>
    public class KumiaiintoKakushuJoukenSetteiErrorData
    {
        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等氏名漢字
        /// </summary>
        public string 組合員等氏名漢字 { get; set; } = string.Empty;

        /// <summary>
        /// 用途区分
        /// </summary>
        public string 用途区分 { get; set; } = string.Empty;

        /// <summary>
        /// 用途名称
        /// </summary>
        public string 用途名称 { get; set; } = string.Empty;

        /// <summary>
        /// ERRMSG
        /// </summary>
        public string ERRMSG { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的名称
        /// </summary>
        public string 共済目的名称 { get; set; } = string.Empty;

        /// <summary>
        /// 類短縮名称
        /// </summary>
        public string 類短縮名称 { get; set; } = string.Empty;

        /// <summary>
        /// 大地区名
        /// </summary>
        public string 大地区名 { get; set; } = string.Empty;

        /// <summary>
        /// 小地区名
        /// </summary>
        public string 小地区名 { get; set; } = string.Empty;
    }
}
