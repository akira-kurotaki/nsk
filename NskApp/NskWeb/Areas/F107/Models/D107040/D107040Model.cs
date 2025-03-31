
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F107.Models.D107040
{
    /// <summary>
    /// 消込み処理(還付・自動)画面項目モデル
    /// </summary>
    [Serializable]
    public class D107040Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D107040Model()
        {
            this.EntryCondition = new D107040EntryCondition();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D107040Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.EntryCondition = new D107040EntryCondition(syokuin, shishoList);
        }

        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        [Display(Name = "検索条件")]
        public D107040EntryCondition EntryCondition { get; set; }
    }
}
