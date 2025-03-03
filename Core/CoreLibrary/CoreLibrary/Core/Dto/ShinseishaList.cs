namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// 申請者情報一覧マスタDTOクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2020/12/1
    /// 作成者：Nakamura Koichi
    /// </remarks>
    [Serializable]
    public class ShinseishaList
    {
        /// <summary>
        /// 氏名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 氏名カナ
        /// </summary>
        public string NameKana { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 都道府県
        /// </summary>
        public string Todofuken { get; set; }

        /// <summary>
        /// 市町村名
        /// </summary>
        public string Shichoson { get; set; }

        /// <summary>
        /// 番地等
        /// </summary>
        public string Banchi { get; set; }

        /// <summary>
        /// 連絡先電話番号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 生年月日
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
    }
}