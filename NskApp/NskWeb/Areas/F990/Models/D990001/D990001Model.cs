using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F990.Models.D990001
{
    /// <summary>
    /// 都道府県ドロップダウンリスト
    /// </summary>
    [Serializable]
    public class D990001Model
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D990001Model()
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">職員マスタ</param>
        public D990001Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("", syokuin, shishoList);
        }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }
    }
}
