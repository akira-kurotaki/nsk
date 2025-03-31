using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models;
using NskWeb.Areas.F106.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F107.Models.D107040.D107040EntryCondition;

namespace NskWeb.Areas.F107.Models.D107040
{
    /// <summary>
    /// 消込み処理(自動)(インタフェース）画面項目モデル
    /// </summary>
    [Serializable]
    public class D107040EntryCondition
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D107040EntryCondition()
        {
            HonshoshishoList = new List<SelectListItem>();
            HonshoshishoModelList = new List<D107000HonshoshishoList>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D107040EntryCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
        }

        /// <summary>
        /// 年産
        /// </summary>
        [Display(Name = "年産")]
        [FullStringLength(4)]
        public string Nensan { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Display(Name = "共済目的コード")]
        [FullStringLength(2)]
        public string KyosaiMokutekiCd { get; set; }
        
        /// <summary>
        /// 共済目的名称
        /// </summary>
        [Display(Name = "共済目的名称")]
        [FullStringLength(20)]
        public string KyosaiMokutekiNm { get; set; }
        
        /// <summary>
        /// 引受回
        /// </summary>
        [Display(Name = "引受回")]
        [Required]
        [Numeric]
        [WithinDigitLength(2)]
        public int? CurentHikiukeCnt { get; set; }

        /// <summary>
        /// 引受回退避
        /// </summary>
        [Display(Name = "引受回退避")]
        [Numeric]
        public int? CurentHikiukeCntHidden { get; set; }

        /// <summary>
        /// 対象データ振替日
        /// </summary>
        [Display(Name = "対象データ振替日")]
        [Required]
        [DateGYMD]
        public DateTime? TaisyoFurikaeDate { get; set; }

        /// <summary>
        /// 引受計算支所実行単位区分_引受
        /// </summary>
        [Display(Name = "引受計算支所実行単位区分_引受")]
        public string ShishoJikkoHikiukeKbn { get; set; }

        /// <summary>
        /// 本所・支所リスト
        /// </summary>
        public List<SelectListItem> HonshoshishoList { get; set; }
        /// <summary>
        /// 本所・支所モデルリスト
        /// </summary>
        public List<D107000HonshoshishoList> HonshoshishoModelList { get; set; }

        /// <summary>
        /// 選択支所コード
        /// </summary>
        [Display(Name = "選択支所コード")]
        [Required]
        public string SelectShishoCd { get; set; }
    }
}
