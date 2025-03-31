using CoreLibrary.Core.Validator;
using NskWeb.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105030
{
    public class D105030KikenDankaiKbnRecord : BasePagerRecord
    {
        /// <summary>統計単位地域コード</summary>
        [Display(Name = "統計単位地域コード")]
        [Required]
        [Numeric]
        [WithinDigitLength(5)]
        public string TokeiTaniChiikiCd { get; set; } = string.Empty;
        /// <summary>統計単位地域名</summary>
        public string TokeiTaniChiikiNm { get; set; } = string.Empty;
        /// <summary>危険段階区分（危険段階料率）</summary>
        [Display(Name = "危険段階区分（危険段階料率）")]
        [Required]
        public string KikenDankaiKbn { get; set; } = string.Empty;
        /// <summary>xmin</summary>
        public uint? Xmin { get; set; }

        /// <summary>
        /// srcオブジェクトとの比較
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool Compare(D105030KikenDankaiKbnRecord src)
        {
            return (
                ($"{this.TokeiTaniChiikiCd}" == $"{src.TokeiTaniChiikiCd}") &&
                ($"{this.KikenDankaiKbn}" == $"{src.KikenDankaiKbn}")
            );
        }
    }
}