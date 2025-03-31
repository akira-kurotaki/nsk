using System.ComponentModel.DataAnnotations;
using CoreLibrary.Core.Validator;
using NskWeb.Common.Models;

namespace NskWeb.Areas.F105.Models.D105190
{
    public class D105190ResultRecord : BasePagerRecord
    {
        /// <summary>組合員等コード</summary>
        [Display(Name = "組合員等コード")]
        [NumberSign(13)]
        [Required]
        public string KumiaiintoCd { get; set; } = string.Empty;

        /// <summary>氏名</summary>
        [Display(Name = "氏名")]
        public string FullNm { get; set; } = string.Empty;

        /// <summary>共済金額</summary>
        [Display(Name = "共済金額")]
        [NumberSign(13)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal? KyousaiKingaku { get; set; } = 0;

        public uint? Xmin { get; set; }


        /// <summary>
        /// srcオブジェクトとの比較
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool Compare(D105190ResultRecord src)
        {
            return (
                ($"{this.KumiaiintoCd}" == $"{src.KumiaiintoCd}") &&
                ($"{this.KyousaiKingaku}" == $"{src.KyousaiKingaku}")
            );
        }
    }
}