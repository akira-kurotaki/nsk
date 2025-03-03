using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F02.Models.D0201
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果明細データ）
    /// </summary>
    [Serializable]
    public class D0201TableRecord
    {
        /// <summary>
        /// 行選択用チェックボックス
        /// </summary>
        [Display(Name = "行選択用チェックボックス")]
        public bool SelectCheck { get; set; }

        /// <summary>
        /// 農業者ID
        /// </summary>
        [Display(Name = "農業者ID")]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 対象年度
        /// </summary>
        [Display(Name = "対象年度")]
        public short? Nendo { get; set; }

        /// <summary>
        /// 加入者管理コード
        /// </summary>
        [Display(Name = "加入者管理コード")]
        public string KanyushaCd { get; set; }

        /// <summary>
        /// 都道府県
        /// </summary>
        [Display(Name = "都道府県")]
        public string TodofukenNm { get; set; }

        /// <summary>
        /// 組合等
        /// </summary>
        [Display(Name = "組合等")]
        public string KumiaitoNm { get; set; }

        /// <summary>
        /// 支所
        /// </summary>
        [Display(Name = "支所")]
        public string ShishoNm { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        [Display(Name = "氏名又は法人名")]
        public string HojinFullNm { get; set; }
    }
}
