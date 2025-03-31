using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F106.Models
{
    [Serializable]
    public class D106000HonshoshishoList
    {
        public D106000HonshoshishoList()
        {
            HonshoshishoCd = "";
            HonshoshishoNm = "";
        }

        [Column("本所・支所コード")]
        public string HonshoshishoCd { get; set; }
        [Column("本所・支所名称")]
        public string HonshoshishoNm { get; set; }
    }
}