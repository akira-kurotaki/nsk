using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F01.Models.D0101
{
    /// <summary>
    /// 加入者一覧(検索結果詳細)
    /// </summary>
    [Serializable]
    public class D0101TableRecord
    {
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
        /// 都道府県名称
        /// </summary>
        [Display(Name = "都道府県名称")]
        public string TodofukenNm { get; set; }

        /// <summary>
        /// 組合等名称
        /// </summary>
        [Display(Name = "組合等名称")]
        public string KumiaitoNm { get; set; }

        /// <summary>
        /// 支所名称
        /// </summary>
        [Display(Name = "支所名称")]
        public string ShishoNm { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        [Display(Name = "氏名又は法人名")]
        public string HojinFullNm { get; set; }
    }
}
