using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F112.Models.D112010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果明細データ）
    /// </summary>
    [Serializable]
    public class D112010TableRecordTorikomi
    {
        /// <summary>
        /// 行選択用チェックボックス
        /// </summary>
        [Display(Name = "行選択用チェックボックス")]
        public bool SelectCheck { get; set; }

        /// <summary>
        /// 取込履歴id
        /// </summary>
        [Display(Name = "取込履歴id")]
        public string TorikomiRirekiId { get; set; }

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
        /// 取込対象データ略称
        /// </summary>
        [Display(Name = "取込対象データ略称")]
        public string TorikomiTaishoDataNm { get; set; }

        /// <summary>
        /// 登録ユーザid
        /// </summary>
        [Display(Name = "登録ユーザid")]
        public string TourokuUserId { get; set; }

        /// <summary>
        /// 区分名称
        /// </summary>
        [Display(Name = "区分名称")]
        public string KbnNm { get; set; }

        /// <summary>
        /// 対象件数
        /// </summary>
        [Display(Name = "対象件数")]
        public int? TaishoCount { get; set; }

        /// <summary>
        /// エラーリスト名
        /// </summary>
        [Display(Name = "エラーリスト名")]
        public string ErrorListNm { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        [Display(Name = "コメント")]
        public string Comment { get; set; }
    }
}
