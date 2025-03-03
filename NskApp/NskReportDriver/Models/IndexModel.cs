using System.ComponentModel.DataAnnotations;

namespace RealtimeReportDriver.Models
{
    public class IndexModel
    {
        public IndexModel()
        {
            UserId = "0000000001";
        }

        [Required]
        [Display(Name = "帳票制御ID")]
        public string ReportControlId { get; set; }
        public IEnumerable<ReportControlIdOptionModel> ReportControlIdOptions { get; set; }

        [Required]
        [Display(Name = "ユーザID")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "条件ID")]
        public int? JoukenId { get; set; }

        [Required]
        [Display(Name = "都道府県コード")]
        public string TodofukenCd { get; set; }

        [Display(Name = "組合等コード")]
        public string KumiaitoCd { get; set; }

        [Display(Name = "支所コード")]
        public string ShishoCd { get; set; }

        [Display(Name = "利用可能支所一覧")]
        public List<string> ShishoList { get; set; }
    }

    public class ReportControlIdOptionModel
    {
        // 選択肢の値をセット
        public string ControlId { get; set; }

        // 選択肢の名をセット
        public string ControlName { get; set; }

        // 選択肢として表示するテキスト
        public string DisplayText
        {
            get
            {
                return $"{this.ControlId}：{this.ControlName}";
            }
        }
    }
}