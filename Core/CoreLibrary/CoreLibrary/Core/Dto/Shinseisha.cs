namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// 申請者マスタDTOクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2020/12/1
    /// 作成者：Nakamura Koichi
    /// </remarks>
    [Serializable]
    public class Shinseisha
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Shinseisha()
        {
            KeieitaiList = new KeieitaiList();
            ShinseishaList = new ShinseishaList();
        }

        /// <summary>
        /// アカウント管理番号
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// eMAFF種別
        /// </summary>
        public string EMAFFType { get; set; }

        /// <summary>
        /// アカウント区分
        /// </summary>
        public string AccountKbn { get; set; }

        /// <summary>
        /// 経営体情報一覧
        /// </summary>
        public KeieitaiList KeieitaiList { get; set; }

        /// <summary>
        /// 申請者情報一覧
        /// </summary>
        public ShinseishaList ShinseishaList { get; set; }

        /// <summary>
        /// 申請者名（文字数固定）
        /// </summary>
        public string NameDisp {
            get
            {
                int maxlen = 10;
                return (maxlen - ShinseishaList.Name.Length >= 0) ? ShinseishaList.Name.PadLeft(maxlen) : ShinseishaList.Name.Remove(maxlen);
            }
        }

        /// <summary>
        /// エラーコード
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}