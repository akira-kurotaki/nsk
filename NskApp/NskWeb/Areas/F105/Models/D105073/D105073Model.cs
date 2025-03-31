using CoreLibrary.Core.Base;
using NskWeb.Areas.F105.Consts;

namespace NskWeb.Areas.F105.Models.D105073
{
    [Serializable]
    public class D105073Model : CoreViewModel
    {
        /// <summary>
        /// メッセージエリア1
        /// </summary>
        public string MessageArea1 { get; set; } = string.Empty;

        /// <summary>
        /// メッセージエリア2
        /// </summary>
        public string MessageArea2 { get; set; } = string.Empty;

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


        #region "危険段階"
        public D105073KikenDankaiKbn KikenDankaiKbn { get; set; } = new();
        #endregion




        /// <summary>
        /// 画面権限
        /// </summary>
        public F105Const.Authority DispKengen { get; set; } = F105Const.Authority.None;

    }
}
