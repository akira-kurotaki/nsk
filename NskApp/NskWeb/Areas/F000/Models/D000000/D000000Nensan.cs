using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F000.Models.D000000
{
    [Serializable]
    public class D000000Nensan 
    {
        public D000000Nensan()
        {
            SKyosaiMokutekiCd = "";
            SNensan = "";
        }

        [Column("共済目的コード")]
        public string  SKyosaiMokutekiCd{ get; set; }
        [Column("年産")]
        public string SNensan { get; set; }
    }
}