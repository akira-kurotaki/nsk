using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using ModelLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F112.Models.D112010.D112010SearchCondition;


namespace NskWeb.Areas.F112.Models.D112010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D112010SearchCondition
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D112010SearchCondition()
        {
            this.TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D112010SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.TodofukenDropDownList = new TodofukenDropDownList("SearchCondition", syokuin, shishoList);
        }

        /// <summary>
        /// 検索条件
        /// </summary>
        [Display(Name = "検索条件")]
        public D112010SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// 検索結果表示フラグ
        /// </summary>
        [Display(Name = "検索結果表示フラグ")]
        public bool IsResultDisplay { get; set; }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 受入日（開始）
        /// </summary>
        [Display(Name = "受入日")]
        [DataType(DataType.Date)]
        public DateTime? UkeireDateFrom { get; set; }

        /// <summary>
        /// 受入日（終了）
        /// </summary>
        [Display(Name = "受入日（終了）")]
        [DataType(DataType.Date)]
        public DateTime? UkeireDateTo { get; set; }

        /// <summary>
        /// 受入ユーザID
        /// </summary>
        [Display(Name = "受入ユーザID")]
        public string UkeireUserId { get; set; }

        /// <summary>
        /// 受入ユーザID
        /// </summary>
        [Display(Name = "受入ユーザID")]
        public string UkeireTaishoData { get; set; }

        /// <summary>
        /// 表示数
        /// </summary>
        [Display(Name = "表示数")]
        public int? DisplayCount { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// 加入者管理コード（開始）
        /// </summary>
        [Display(Name = "加入者管理コード（開始）")]
        [Numeric]
        [FullStringLength(13)]
        public string KanyushaCdFrom { get; set; }

        /// <summary>
        /// 加入者管理コード（終了）
        /// </summary>
        [Display(Name = "加入者管理コード（終了）")]
        [Numeric]
        [FullStringLength(13)]
        public string KanyushaCdTo { get; set; }

        /// <summary>
        /// 他のユーザの取込履歴フラグ
        /// </summary>
        public bool IncludeOtherUserHistoryFlg { get; set; }
    }
}
