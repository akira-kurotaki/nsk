namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// 画面操作権限
    /// </summary>
    [Serializable]
    public class ScreenSosaKengen
    {
        /// <summary>
        /// 画面機能コード
        /// </summary>
        public string ScreenKinoCd { get; set; }

        /// <summary>
        /// 参照不可フラグ
        /// 0:false、1:true
        /// </summary>
        public string NoReferFlg { get; set; }

        /// <summary>
        /// 更新不可フラグ
        /// 0:false、1:true
        /// </summary>
        public string NoUpdateFlg { get; set; }
    }
}
