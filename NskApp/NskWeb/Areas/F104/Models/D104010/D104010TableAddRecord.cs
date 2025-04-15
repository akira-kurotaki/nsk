using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F104.Models.D104010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果明細データ）
    /// </summary>
    [Serializable]
    public class D104010TableAddRecord
    {
        /// <summary>
        /// 行選択用チェックボックス
        /// </summary>
        [Display(Name = "行選択用チェックボックス")]
        public bool SelectCheck { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Display(Name = "組合等コード")]
        public string kumiaitoCd { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        [Display(Name = "氏名又は法人名")]
        public string HojinFullNm { get; set; }

        /// <summary>
        /// 賦課金計
        /// </summary>
        [Display(Name = "賦課金計")]
        public Decimal? hukakinkei { get; set; }

        /// <summary>
        /// 引受解除日付
        /// </summary>
        [Display(Name = "引受解除日付")]
        [DataType(DataType.Date)]
        public DateTime? hikiukeKaijyoDate { get; set; }

        /// <summary>
        /// 解除申出日付
        /// </summary>
        [Display(Name = "解除申出日付")]
        [DataType(DataType.Date)]
        public DateTime? kanjyoMousideDate { get; set; }

        /// <summary>
        /// 解除理由コード
        /// </summary>
        [Display(Name = "解除理由コード")]
        public string kaijyoRiyuCd { get; set; }

        /// <summary>
        /// 解除理由名称
        /// </summary>
        [Display(Name = "解除理由名称")]
        public string kaijyoRiyuMeisho { get; set; }

        /// <summary>
        /// 引受解除返還賦課金額
        /// </summary>
        [Display(Name = "引受解除返還賦課金額")]
        public Decimal? hikiukeKaijyoHenkanHukakingaku { get; set; }

        /// <summary>
        /// 新行フラグ
        /// </summary>
        [Display(Name = "新行フラグ")]
        public string newRowFlg { get; set; }
    }
}
