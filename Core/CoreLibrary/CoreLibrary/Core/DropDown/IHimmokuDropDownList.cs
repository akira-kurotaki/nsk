namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 品目・種類・用途ドロップダウンリスト用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/21
    /// 作成者：Rou I
    /// </remarks>
    public interface IHimmokuDropDownList
    {
        /// <summary>
        /// 都道府県
        /// </summary>
        string TodofukenCd { get; }

        /// <summary>
        /// 組合等
        /// </summary>
        string KumiaitoCd { get; }

        /// <summary>
        /// 支所
        /// </summary>
        string ShishoCd { get; }

        /// <summary>
        /// 品目
        /// </summary>
        string HimmokuCd { get; }

        /// <summary>
        /// 種類
        /// </summary>
        string ShuruiCd { get; }

        /// <summary>
        /// 種類(入力)
        /// </summary>
        string Shurui { get; }

        /// <summary>
        /// 用途
        /// </summary>
        string YotoCd { get; }

    }
}