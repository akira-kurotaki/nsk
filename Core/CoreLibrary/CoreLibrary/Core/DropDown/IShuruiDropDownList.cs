namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 種類・品目・用途ドロップダウンリスト用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2018/05/16
    /// 作成者：Rou I
    /// </remarks>
    public interface IShuruiDropDownList
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
        /// 種類
        /// </summary>
        string ShuruiCd { get; }

        /// <summary>
        /// 品目
        /// </summary>
        string HimmokuCd { get; }

        /// <summary>
        /// 用途
        /// </summary>
        string YotoCd { get; }

        /// <summary>
        /// プログラムモード
        /// </summary>
        //ShuruiDropDownListUtil.ProgramMode? ProgramMode { get; }
        string ProgramMode { get; }

    }
}