using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F109.Models.D109020
{
    /// <summary>
    /// 検索条件
    /// </summary>
    public class D109020SearchCondition
    {
        /// <summary>
        /// 支所
        /// </summary>
        [Display(Name = "本所・支所")]
        [Required]
        public string ShishoCd { get; set; } = string.Empty;

        /// <summary>
        /// 支所リスト
        /// </summary>
        [NotMapped]
        public List<SelectListItem> ShishoList { get; set; } = new();

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D109020SearchCondition src)
        {
            ShishoCd = src.ShishoCd;
        }
    }
}
