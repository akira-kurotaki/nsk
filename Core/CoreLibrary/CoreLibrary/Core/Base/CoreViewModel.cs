using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Core.Base
{
    /// <summary>
    /// 画面モデル親クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/08
    /// 作成者：Nakamura Koichi
    /// </remarks>
    [Serializable]
    public abstract class CoreViewModel
    {
        #region プロパティ
        /// <summary>
        /// 自ウィンドウID
        /// </summary>
        [NotMapped]
        public string SelfWindowId { get; set; } = string.Empty;

        /// <summary>
        /// 子ウィンドウID連番
        /// </summary>
        [NotMapped] 
        public int ChildWindowIdNum { get; set; } = 0;

        /// <summary>
        /// 複数タブ対応のため、画面に埋め込むPAGE_TOKEN
        /// </summary>
        [NotMapped] 
        public string PageToken { get; set; }
        #endregion
    }
}