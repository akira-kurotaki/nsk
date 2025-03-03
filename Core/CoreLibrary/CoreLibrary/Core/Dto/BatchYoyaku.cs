namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// バッチ予約状況
    /// </summary>
    [Serializable]
    public class BatchYoyaku
    {
        /// <summary>
        /// バッチID
        /// </summary>
        public long BatchId { get; set; }

        /// <summary>
        /// バッチ分類
        /// </summary>
        public string BatchBunrui { get; set; }

        /// <summary>
        /// システム区分
        /// </summary>
        public string SystemKbn { get; set; }

        /// <summary>
        /// システム区分名称
        /// </summary>
        public string SystemKbnNm { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        public string TodofukenNm { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 組合等正式名称
        /// </summary>
        public string KumiaitoNm { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; }

        /// <summary>
        /// 支所名
        /// </summary>
        public string ShishoNm { get; set; }

        /// <summary>
        /// 予約日時
        /// </summary>
        public DateTime? BatchYoyakuDate { get; set; }

        /// <summary>
        /// 予約ユーザID
        /// </summary>
        public string BatchYoyakuId { get; set; }

        /// <summary>
        /// 予約を実行した処理名
        /// </summary>
        public string BatchYoyakuShori { get; set; }

        /// <summary>
        /// バッチ名
        /// </summary>
        public string BatchNm { get; set; }

        /// <summary>
        /// バッチ条件
        /// </summary>
        public string BatchJoken { get; set; }

        /// <summary>
        /// バッチ条件（画面表示用）
        /// </summary>
        public string BatchJokenDisp { get; set; }

        /// <summary>
        /// 複数実行禁止フラグ
        /// </summary>
        public string MultiRunNgFlg { get; set; }

        /// <summary>
        /// 複数実行禁止ID
        /// </summary>
        public string MultiRunNgId { get; set; }

        /// <summary>
        /// バッチ種類
        /// </summary>
        public string BatchType { get; set; }

        /// <summary>
        /// 定時実行バッチ予約日時
        /// </summary>
        public DateTime? BatchScheDatetime { get; set; }

        /// <summary>
        /// ロック対象フラグ
        /// </summary>
        public string LockTargetFlg { get; set; }

        /// <summary>
        /// バッチステータス
        /// </summary>
        public string BatchStatus { get; set; }

        /// <summary>
        /// バッチ開始日時
        /// </summary>
        public DateTime? BatchStartDate { get; set; }

        /// <summary>
        /// バッチ完了日時
        /// </summary>
        public DateTime? BatchEndDate { get; set; }

        /// <summary>
        /// 実行サーバ
        /// </summary>
        public string BatchRunServer { get; set; }

        /// <summary>
        /// エラー情報
        /// </summary>
        public string ErrorInfo { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public int? Renban { get; set; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// ハッシュ値
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileNm { get; set; }

        /// <summary>
        /// 登録ユーザID
        /// </summary>
        public string InsertUserId { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// 更新ユーザID
        /// </summary>
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
