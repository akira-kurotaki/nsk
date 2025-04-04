using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F107.Models
{
    [Serializable]
    public class D107060ChoshuYoteiList
    {
        public D107060ChoshuYoteiList()
        {
            KbnCd = "";
            KbnNm = "";
        }

        [Column("区分コード")]
        public string KbnCd { get; set; }
        [Column("区分名称")]
        public string KbnNm { get; set; }
    }
}