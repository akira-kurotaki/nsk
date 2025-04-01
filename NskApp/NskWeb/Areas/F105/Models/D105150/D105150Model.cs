using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using NskWeb.Areas.F105.Consts;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105150
{
    [Serializable]
    public class D105150Model : CoreViewModel
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
        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string KumiaiinToCd { get; set; } = string.Empty;
        /// <summary>
        /// 氏名
        /// </summary>
        public string FullNm { get; set; } = string.Empty;
        /// <summary>
        /// 電話番号
        /// </summary>
        public string Tel { get; set; } = string.Empty;
        /// <summary>
        /// 支所
        /// </summary>
        public string ShishoNm { get; set; } = string.Empty;
        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; } = string.Empty;
        /// <summary>
        /// 市町村
        /// </summary>
        public string ShichosonNm { get; set; } = string.Empty;
        /// <summary>
        /// 市町村コード
        /// </summary>
        public string ShichosonCd { get; set; } = string.Empty;
        /// <summary>
        /// 大地区
        /// </summary>
        public string DaichikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 大地区コード
        /// </summary>
        public string DaichikuCd { get; set; } = string.Empty;
        /// <summary>
        /// 小地区
        /// </summary>
        public string ShochikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 小地区コード
        /// </summary>
        public string ShochikuCd { get; set; } = string.Empty;
        /// <summary>
        /// 合併時識別
        /// </summary>
        public string GappeijiShikibetu { get; set; } = string.Empty;

        /// <summary>
        /// 引受方式コード
        /// </summary>
        public string HikiukeHoushikiCd { get; set; } = string.Empty;
        /// <summary>
        /// 引受方式
        /// </summary>
        public string HikiukeHoushikiNm { get; set; } = string.Empty;
        /// <summary>
        /// 類区分コード
        /// </summary>
        public string RuiKbn { get; set; } = string.Empty;
        /// <summary>
        /// 類区分
        /// </summary>
        public string RuiKbnNm { get; set; }
        #endregion

        /// <summary>
        /// 条件選択
        /// </summary>
        [Display(Name = "検索条件")]
        public D105150SearchCondition SearchCondition { get; set; } = new();


        #region "基準収穫量設定"
        public D105150KijunSyukakuryoSettei KijunSyukakuryoSettei { get; set; } = new();
        #endregion


        /// <summary>
        /// 画面権限
        /// </summary>
        public F105Const.Authority DispKengen { get; set; } = F105Const.Authority.None;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105150Model() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105150Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.SearchCondition = new(syokuin, shishoList);
        }
    }
}
