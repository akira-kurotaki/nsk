using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using ModelLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F000.Models.D000022.D000022SearchCondition;

namespace NskWeb.Areas.F000.Models.D000022
{
    /// <summary>
    /// 産地別銘柄コード検索(子画面)画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D000022SearchCondition
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
        /// 産地別銘柄名称
        /// </summary>
        [Display(Name = "産地別銘柄名称")]
        [WithinStringLength(30)]
        public string SanchiMeigaraNm { get; set; }

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        [Display(Name = "産地別銘柄コード")]
        public string SanchiMeigaraCd { get; set; }

        /// <summary>
        /// 産地別銘柄コード(開始)
        /// </summary>
        [Display(Name = "産地別銘柄コード(開始)")]
        [Numeric]
        [WithinDigitLength(5)]
        public string SanchiMeigaraCdFrom { get; set; }

        /// <summary>
        /// 産地別銘柄コード(終了)
        /// </summary>
        [Display(Name = "産地別銘柄コード(終了)")]
        [Numeric]
        [WithinDigitLength(5)]
        public string SanchiMeigaraCdTo { get; set; }

        /// <summary>
        /// 産地別銘柄コード(～)
        /// </summary>
        [Display(Name = "～")]
        public string SanchiMeigaraCdTilde { get; set; }

        /// <summary>
        /// 検索子画面からの戻り値をセットする項目のキー
        /// </summary>
        public string RetKey { get; set; }
    }
}
