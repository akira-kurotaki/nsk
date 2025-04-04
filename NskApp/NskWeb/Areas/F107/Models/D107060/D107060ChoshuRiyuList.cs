using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F107.Models
{
    [Serializable]
    public class D107060ChoshuRiyuList
    {
        public D107060ChoshuRiyuList()
        {
            ChoshuRiyuCd = null;
            ChoshuRiyuNm = "";
        }

        [Column("徴収理由コード")]
        public decimal? ChoshuRiyuCd { get; set; }
        [Column("徴収理由名")]
        public string ChoshuRiyuNm { get; set; }
    }
}