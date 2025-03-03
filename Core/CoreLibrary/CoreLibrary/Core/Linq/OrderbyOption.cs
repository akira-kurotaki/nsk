using CoreLibrary.Core.Consts;

namespace CoreLibrary.Core.Linq
{
    /// <summary>
    /// ソート順設定オプションモデル
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/26
    /// 作成者：Rou I
    /// </remarks>
    public class OrderbyOption
    {
        /// <summary>
        /// 要素名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// ソート順タイプ
        /// </summary>
        public CoreConst.SortOrder SortOrder { get; set; }
    }
}