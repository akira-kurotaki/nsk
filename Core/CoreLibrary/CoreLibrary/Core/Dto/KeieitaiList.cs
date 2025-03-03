namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// 経営体情報一覧マスタDTOクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2020/12/1
    /// 作成者：Nakamura Koichi
    /// </remarks>
    [Serializable]
    public class KeieitaiList
    {
        /// <summary>
        /// 経営体ID
        /// </summary>
        public string KeieitaiId { get; set; }

        /// <summary>
        /// 法人番号
        /// </summary>
        public string HojinNo { get; set; }

        /// <summary>
        /// 個人・法人
        /// </summary>
        public string KojinHojin { get; set; }

        /// <summary>
        /// 法人名/屋号
        /// </summary>
        public string HojinNmYago { get; set; }

        /// <summary>
        /// 法人名/屋号カナ
        /// </summary>
        public string HojinNmYagoKana { get; set; }

        /// <summary>
        /// 個人事業主管理番号
        /// </summary>
        public string KojinKanriNo { get; set; }

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
        /// 郵便番号
        /// </summary>
        public string PostalCd { get; set; }

        /// <summary>
        /// (問合先)電話番号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// (問合先)FAX番号
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// (問合先)メールアドレス
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 代表者氏名
        /// </summary>
        public string DaihyoshaNm { get; set; }

        /// <summary>
        /// 代表者氏名カナ
        /// </summary>
        public string DaihyoshaNmKana { get; set; }

        /// <summary>
        /// 代表者生年月日
        /// </summary>
        public DateTime? DaihyoshaDateOfBirth { get; set; }

        /// <summary>
        /// 代表者性別
        /// </summary>
        public string DaihyoshaGender { get; set; }

        /// <summary>
        /// 業種
        /// </summary>
        public string Gyoshu { get; set; }

        /// <summary>
        /// eMAFF種別
        /// </summary>
        public string EMAFFType { get; set; }

        /// <summary>
        /// 身元確認システム
        /// </summary>
        public string IDSystem { get; set; }

        /// <summary>
        /// 身元確認状況
        /// </summary>
        public string IDStatus { get; set; }

        /// <summary>
        /// 身元確認組織
        /// </summary>
        public string IDOrganization { get; set; }

        /// <summary>
        /// 身元確認担当者
        /// </summary>
        public string IDTantosha { get; set; }

        /// <summary>
        /// 身元確認日時
        /// </summary>
        public DateTime? IDDateTime { get; set; }

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