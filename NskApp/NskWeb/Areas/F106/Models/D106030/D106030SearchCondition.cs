using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F106.Models.D106030.D106030SearchCondition;

namespace NskWeb.Areas.F106.Models.D106030
{
    /// <summary>
    /// 引受計算処理（麦）画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D106030SearchCondition
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D106030SearchCondition()
        {
            HonshoshishoList = new List<SelectListItem>();
            HonshoshishoModelList = new List<D106000HonshoshishoList>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D106030SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
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
        [WithinStringLength(20)]
        public string KyosaiMokutekiNm { get; set; }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Display(Name = "組合等コード")]
        public string KumiaitoCd { get; set; }
        
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
        public List<D106000HonshoshishoList> HonshoshishoModelList { get; set; }

        /// <summary>
        /// 選択支所コード
        /// </summary>
        [Display(Name = "選択支所コード")]
        [Required]
        public string SelectShishoCd { get; set; }

        /// <summary>
        /// 選択支所名称
        /// </summary>
        [Display(Name = "選択支所名称")]
        public string SelectShishoNm { get; set; }

    }
}
