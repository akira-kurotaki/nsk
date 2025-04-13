using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F104.Models.D104010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果明細データ）
    /// </summary>
    [Serializable]
    public class D104010TableRecord
    {
        /// <summary>
        /// 行選択用チェックボックス
        /// </summary>
        [Display(Name = "行選択用チェックボックス")]
        public bool SelectCheck { get; set; }

        /// <summary>
        /// 識別子
        /// </summary>
        [Display(Name = "識別子")]
        public string RecordId { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Display(Name = "組合員等コード")]
        public string kumiaiintoCd { get; set; }

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
        /// 引受回
        /// </summary>
        [ScaffoldColumn(false)]
        [Display(Name = "引受回")]
        public short? hikiukekai { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [ScaffoldColumn(false)]
        [Display(Name = "類区分")]
        public string ruikbn { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [ScaffoldColumn(false)]
        [Display(Name = "統計単位地域コード")]
        public string toukeiTaniChiikiCd { get; set; }

        /// <summary>
        /// 解除確定フラグ
        /// </summary>
        [Display(Name = "解除確定フラグ")]
        public string kaijyoKakuteiFlg { get; set; }

        /// <summary>
        /// 行追加フラグ
        /// </summary>
        [Display(Name = "行追加フラグ")]
        public string rowAddFlg { get; set; }

        /// <summary>
        /// 行削除フラグ
        /// </summary>
        [Display(Name = "行削除フラグ")]
        public string rowDeleteFlg { get; set; }
    }
}
