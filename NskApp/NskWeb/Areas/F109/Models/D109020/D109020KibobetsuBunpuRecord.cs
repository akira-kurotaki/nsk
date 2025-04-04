using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F109.Models.D109020
{
    /// <summary>
    /// 規模分布状況データレコード
    /// </summary>
    public class D109020KibobetsuBunpuRecord
    {
        /// <summary>面積区分</summary>
        public string MensekiKbn { get; set; } = string.Empty;
        /// <summary>面積上限</summary>
        [Display(Name = "面積上限")]
        [Required]
        [Numeric]
        [WithinDigitLength(5)]
        public string MensekiJogen { get; set; } = string.Empty;

        /// <summary>xmin</summary>
        public uint? Xmin { get; set; }
    }
}