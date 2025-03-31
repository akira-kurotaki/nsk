using NskAppModelLibrary.Context;
using CoreLibrary.Core.Base;
using System.ComponentModel.DataAnnotations;
using CoreLibrary.Core.Dto;
using NskWeb.Areas.F105.Consts;

namespace NskWeb.Areas.F105.Models.D105036
{
    [Serializable]
    public class D105036Model : CoreViewModel
    {
        /// <summary>
        /// メッセージエリア1
        /// </summary>
        public string MessageArea1 { get; set; } = string.Empty;


        #region "ヘッダ部"
        /// <summary>
        /// 年産
        /// </summary>
        public string Nensan { get; set; } = string.Empty;
        /// <summary>
        /// 共済目的
        /// </summary>
        public string KyosaiMokuteki { get; set; } = string.Empty;
        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string KyosaiMokutekiCd { get; set; } = string.Empty;
        #endregion

        #region "検索条件"
        /// <summary>
        /// 引受情報入力タブ：条件選択
        /// </summary>
        [Display(Name = "検索条件")]
        public D105036SearchCondition SearchCondition { get; set; } = new();
        #endregion

        #region "検索結果"
        /// <summary>
        /// 検索結果
        /// </summary>
        public D105036SearchResult SearchResult { get; set; } = new();
        #endregion

        /// <summary>
        /// 画面権限
        /// </summary>
        public F105Const.Authority DispKengen { get; set; } = F105Const.Authority.None;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105036Model() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105036Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.SearchCondition = new(syokuin, shishoList);
        }
    }
}
