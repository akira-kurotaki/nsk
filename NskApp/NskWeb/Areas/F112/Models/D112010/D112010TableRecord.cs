using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F112.Models.D112010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果明細データ）
    /// </summary>
    [Serializable]
    public class D112010TableRecord
    {
        /// <summary>
        /// 行選択用チェックボックス
        /// </summary>
        [Display(Name = "行選択用チェックボックス")]
        public bool SelectCheck { get; set; }

        /// <summary>
        /// 受入履歴id
        /// </summary>
        [Display(Name = "受入履歴id")]
        public string UkeireRirekiId { get; set; }

        /// <summary>
        /// データ登録日時
        /// </summary>
        [Display(Name = "データ登録日時")]
        [DataType(DataType.Date)]
        public DateTime? DataRegistDate { get; set; }

        /// <summary>
        /// 受入対象データ略称
        /// </summary>
        [Display(Name = "受入対象データ略称")]
        public string UkeireTaishoDataNm { get; set; }

        /// <summary>
        /// 対象データ区分
        /// </summary>
        [Display(Name = "対象データ区分")]
        public string TaishoDataKbn { get; set; }

        /// <summary>
        /// 区分名称
        /// </summary>
        [Display(Name = "区分名称")]
        public string KbnNm { get; set; }

        /// <summary>
        /// ユーザID
        /// </summary>
        [Display(Name = "ユーザID")]
        public string UserId { get; set; }

        /// <summary>
        /// 処理状況
        /// </summary>
        [Display(Name = "処理状況")]
        public string ProcessStatus { get; set; }

        /// <summary>
        /// 対象件数
        /// </summary>
        [Display(Name = "対象件数")]
        public int? TaishoCount { get; set; }

        /// <summary>
        /// OK件数
        /// </summary>
        [Display(Name = "OK件数")]
        public int? OkCount { get; set; }

        /// <summary>
        /// エラー件数
        /// </summary>
        [Display(Name = "エラー件数")]
        public int? ErrorCount { get; set; }

        /// <summary>
        /// OKデータリスト
        /// </summary>
        [Display(Name = "OKデータリスト")]
        public string OkDataList { get; set; }

        /// <summary>
        /// エラーリスト
        /// </summary>
        [Display(Name = "エラーリスト")]
        public string ErrorList { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        [Display(Name = "コメント")]
        public string Comment { get; set; }

        /// <summary>
        /// 取込
        /// </summary>
        [Display(Name = "取込")]
        public string Torikomi { get; set; }

    }
}
