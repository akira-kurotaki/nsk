using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F000.Models.D000000
{
    [Serializable]
    public class D000000SagyoJokyo
    {
        public D000000SagyoJokyo()
        {
        }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("支所コード")]
        public string 支所コード { get; set; }

        /// <summary>
        /// 支所
        /// </summary>
        [Column("shisho_nm")]
        public string 支所 { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        [Column("引受回")]
        public short 引受回 { get; set; }

        /// <summary>
        /// 引受計算実施日
        /// </summary>
        [Column("引受計算実施日")]
        //public DateTime? 引受計算実施日 { get; set; }
        public string 引受計算実施日 { get; set; }

        /// <summary>
        /// 引受確定実施日
        /// </summary>
        [Column("引受確定実施日")]
        //public DateTime? 引受確定実施日 { get; set; }
        public string 引受確定実施日 { get; set; }

        /// <summary>
        /// 当初評価高計算実施日
        /// </summary>
        [Column("当初評価高計算実施日")]
        //public DateTime? 当初評価高計算実施日 { get; set; }
        public string 当初評価高計算実施日 { get; set; }
    }
}