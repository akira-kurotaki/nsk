using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using ModelLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F000.Models.D000023.D000023SearchCondition;

namespace NskWeb.Areas.F000.Models.D000023
{
    /// <summary>
    /// 統計単位地域コード検索(子画面)画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D000023SearchCondition
    {
        /// <summary>
        /// 年産
        /// </summary>
        [Display(Name = "年産：")]
        public string Nensan { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Display(Name = "共済目的コード")]
        public string KyosaiMokutekiCd { get; set; }

        /// <summary>
        /// 共済目的名称
        /// </summary>
        [Display(Name = "共済目的：")]
        public string KyosaiMokutekiNm { get; set; }

        /// <summary>
        /// 統計単位地域名称
        /// </summary>
        [Display(Name = "統計単位地域名称")]
        [WithinStringLength(10)]
        public string TokeiTaniChikiNm { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Display(Name = "統計単位地域コード")]
        public string TokeiTaniChikiCd { get; set; }

        /// <summary>
        /// 統計単位地域コード(開始)
        /// </summary>
        [Display(Name = "統計単位地域コード(開始)")]
        [Numeric]
        [WithinDigitLength(5)]
        public string TokeiTaniChikiCdFrom { get; set; }

        /// <summary>
        /// 統計単位地域コード(終了)
        /// </summary>
        [Display(Name = "統計単位地域コード(終了)")]
        [Numeric]
        [WithinDigitLength(5)]
        public string TokeiTaniChikiCdTo { get; set; }

        /// <summary>
        /// 統計単位地域コード(～)
        /// </summary>
        [Display(Name = "～")]
        public string TokeiTaniChikiCdTilde { get; set; }
    }
}
