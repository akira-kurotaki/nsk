using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using ModelLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F000.Models.D000021.D000021SearchCondition;

namespace NskWeb.Areas.F000.Models.D000021
{
    /// <summary>
    /// 品種コード検索(子画面)画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D000021SearchCondition
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
        /// 品種名称
        /// </summary>
        [Display(Name = "品種名称")]
        [WithinStringLength(20)]
        public string HinshuNm { get; set; }

        /// <summary>
        /// 品種コード
        /// </summary>
        [Display(Name = "品種コード")]
        public string HinshuCd { get; set; }

        /// <summary>
        /// 品種コード(開始)
        /// </summary>
        [Display(Name = "品種コード(開始)")]
        [Numeric]
        [WithinDigitLength(3)]
        public string HinshuCdFrom { get; set; }

        /// <summary>
        /// 品種コード(終了)
        /// </summary>
        [Display(Name = "品種コード(終了)")]
        [Numeric]
        [WithinDigitLength(3)]
        public string HinshuCdTo { get; set; }

        /// <summary>
        /// 品種コード(～)
        /// </summary>
        [Display(Name = "～")]
        public string HinshuCdTilde { get; set; }

        /// <summary>
        /// 検索子画面からの戻り値をセットする項目のキー
        /// </summary>
        public string RetKey { get; set; }
    }
}
