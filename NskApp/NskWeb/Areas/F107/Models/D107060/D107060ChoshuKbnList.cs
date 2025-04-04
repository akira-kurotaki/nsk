using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F107.Models
{
    [Serializable]
    public class D107060ChoshuKbnList
    {
        public D107060ChoshuKbnList()
        {
            ChoshuKbnCd = null;
            ChoshuKbnNm = "";
        }

        [Column("徴収区分コード")]
        public decimal? ChoshuKbnCd { get; set; }
        [Column("徴収区分名")]
        public string ChoshuKbnNm { get; set; }
    }
}