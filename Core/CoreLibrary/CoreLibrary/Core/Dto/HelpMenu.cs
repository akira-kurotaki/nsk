using ModelLibrary.Models;

namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// ヘルプメニューのDTOクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/04/16
    /// 作成者：MATSUBAYASHI Atsushi
    public class HelpMenuDto : MHelpMenu
    {
        /// <summary>
        /// ハッシュ値
        /// </summary>
        public string Hash { get; set;}
    }
}