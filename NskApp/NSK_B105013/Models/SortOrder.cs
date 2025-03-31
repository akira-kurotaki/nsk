namespace NSK_B105013.Models
{
    /// <summary>
    /// 出力順モデル
    /// </summary>
    public class SortOrder
    {
        /// <summary>
        /// 出力順の項目キー
        /// </summary>
        public string OrderByKey { get; set; } = string.Empty;

        /// <summary>
        /// 出力順の指定（降順：０、昇順：１）
        /// </summary>
        public string OrderBy { get; set; } = string.Empty;
    }
}
