using Microsoft.AspNetCore.Mvc.Rendering;
using NskWeb.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F105.Models.D105074
{
    public class D105074YotobetsuTankaRecord : BasePagerRecord
    {
        /// <summary>作付時期</summary>
        [Display(Name = "作付時期")]
        [Required]
        public string SakutsukeJiki { get; set; } = string.Empty;
        /// <summary>用途区分</summary>
        [Display(Name = "用途区分")]
        [Required]
        public string YotoKbn { get; set; } = string.Empty;
        /// <summary>適用単価</summary>
        [Display(Name = "適用単価")]
        [Required]
        public decimal? TekiyoTanka { get; set; }
        /// <summary>xmin</summary>
        public uint? Xmin { get; set; }


        /// <summary>用途区分ドロップダウンリスト選択値</summary>
        [NotMapped]
        public List<SelectListItem> YotoKbnLists { get; set; } = new();
        /// <summary>適用単価ドロップダウンリスト選択値</summary>
        [NotMapped]
        public List<SelectListItem> TekiyoTankaLists { get; set; } = new();

        /// <summary>
        /// srcオブジェクトとの比較
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool Compare(D105074YotobetsuTankaRecord src)
        {
            return (
                ($"{this.SakutsukeJiki}" == $"{src.SakutsukeJiki}") &&
                ($"{this.YotoKbn}" == $"{src.YotoKbn}") &&
                ($"{this.TekiyoTanka}" == $"{src.TekiyoTanka}")
            );
        }
    }
}