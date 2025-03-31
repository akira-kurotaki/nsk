using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000020
{
    /// <summary>
    /// 組合員等コード検索(子画面)（検索結果取得）
    /// </summary>
    [Serializable]
    public class D000020TableRecord
    {
        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Display(Name = "組合員等コード")]
        public string KumiaiintoCd { get; set; }

        /// <summary>
        /// 氏名
        /// </summary>
        [Display(Name = "氏名")]
        public string Name { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [Display(Name = "住所")]
        public string Post { get; set; }

        /// <summary>
        /// 氏名
        /// </summary>
        [Display(Name = "電話番号")]
        public string Tel { get; set; }
    }
}
