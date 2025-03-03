namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// 職員マスタDTOクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/21
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class Syokuin
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// ユーザ管理番号
        /// </summary>
        public int UserKanriNo { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; }

        /// <summary>
        /// ユーザ管理権限
        /// </summary>
        public string UserKanriKengen { get; set; }

        /// <summary>
        /// バッチ管理権限
        /// </summary>
        public string BatchKanriKengen { get; set; }

        /// <summary>
        /// 氏名
        /// </summary>
        public string FullNm { get; set; }

        /// <summary>
        /// フリガナ
        /// </summary>
        public string FullKana { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// システム利用者区分
        /// </summary>
        public string SystemRiyoKbn { get; set; }

        /// <summary>
        /// 利用権限
        /// </summary>
        public string Kengen { get; set; }

        /// <summary>
        /// 利用権限_加入状況管理
        /// </summary>
        public string KjkKengen { get; set; }

        /// <summary>
        /// 利用権限_農業者情報管理
        /// </summary>
        public string FimKengen { get; set; }

        /// <summary>
        /// 利用権限_農作物共済
        /// </summary>
        public string NskKengen { get; set; }

        /// <summary>
        /// 利用権限_建物共済
        /// </summary>
        public string SmlKengen { get; set; }

        /// <summary>
        /// 利用権限_収入保険
        /// </summary>
        public string SynKengen { get; set; }

        /// <summary>
        /// 利用権限_園芸施設共済（共通申請）
        /// </summary>
        public string KyotsuEngKengen { get; set; }

        /// <summary>
        /// 利用権限_家畜共済（共通申請）
        /// </summary>
        public string KyotsuKtkKengen { get; set; }

        /// <summary>
        /// 利用権限_農作物共済（共通申請連携）
        /// </summary>
        public string KyotsuRenkeiNskKengen { get; set; }

        /// <summary>
        /// 利用権限_畑作物共済（共通申請連携）
        /// </summary>
        public string KyotsuRenkeiHatKengen { get; set; }

        /// <summary>
        /// 利用権限_果樹共済（共通申請連携）
        /// </summary>
        public string KyotsuRenkeiKjuKengen { get; set; }

        /// <summary>
        /// 画面操作権限ID
        /// </summary>
        public int? ScreenSosaKengenId { get; set; }

        /// <summary>
        /// 支所グループID
        /// </summary>
        public int? ShishoGroupId { get; set; }

        /// <summary>
        /// ログイン日時
        /// </summary>
        public DateTime? LoginDate { get; set; }
    }
}