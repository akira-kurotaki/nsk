using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F111.Models.D111050
{
    
    public class D111050KoufuChoshuExists
    {

        /// <summary>交付徴収レコード存在</summary>
        [Display(Name = "交付徴収レコード存在")]
        public bool RecordExists { get; set; }

    }
}
