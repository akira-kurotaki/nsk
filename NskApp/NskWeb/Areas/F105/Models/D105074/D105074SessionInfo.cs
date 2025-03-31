using CoreLibrary.Core.Utility;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105070;
using NskWeb.Common.Models;

namespace NskWeb.Areas.F105.Models.D105074
{
    /// <summary>
    /// セッション情報
    /// </summary>
    public class D105074SessionInfo : BaseSessionInfo
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; } = string.Empty;
        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; } = string.Empty;
        /// <summary>
        /// 年産
        /// </summary>
        public int Nensan { get; set; }
        /// <summary>
        /// 共済目的
        /// </summary>
        public string KyosaiMokutekiCd { get; set; } = string.Empty;
        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string KumiaiintoCd { get; set; } = string.Empty;
        /// <summary>
        /// 氏名
        /// </summary>
        public string FullNm { get; set; } = string.Empty;
        /// <summary>
        /// 電話番号
        /// </summary>
        public string Tel { get; set; } = string.Empty;
        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; } = string.Empty;
        /// <summary>
        /// 支所名
        /// </summary>
        public string ShishoNm { get; set; } = string.Empty;
        /// <summary>
        /// 市町村コード
        /// </summary>
        public string ShichosonCd { get; set; } = string.Empty;
        /// <summary>
        /// 市町村名
        /// </summary>
        public string ShichosonNm { get; set; } = string.Empty;
        /// <summary>
        /// 大地区コード
        /// </summary>
        public string DaichikuCd { get; set; } = string.Empty;
        /// <summary>
        /// 大地区名
        /// </summary>
        public string DaichikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 小地区コード
        /// </summary>
        public string ShochikuCd { get; set; } = string.Empty;
        /// <summary>
        /// 小地区名
        /// </summary>
        public string ShochikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 合併時識別コード
        /// </summary>
        public string GappeiShikibetsuCd { get; set; } = string.Empty;

        /// <summary>類区分</summary>
        public string RuiKbn { get; set; } = string.Empty;
        /// <summary>類区分名称</summary>
        public string RuiKbnNm { get; set; }
        /// <summary>引受方式</summary>
        public string HikiukeHoushikiCd { get; set; } = string.Empty;
        /// <summary>引受方式名称</summary>
        public string HikiukeHoushikiNm { get; set; } = string.Empty;
        /// <summary>補償割合</summary>
        public string HoshoWariai { get; set; } = string.Empty;
        /// <summary>一筆半損特約</summary>
        public string IppitsuHansonTokuyaku { get; set; } = string.Empty;
        /// <summary>担手農家区分</summary>
        public string NinaiteKbn { get; set; } = string.Empty;

        /// <summary>
        /// セッション情報取得
        /// </summary>
        /// <param name="context"></param>
        public void GetInfo(HttpContext context)
        {
            // ２．１．セッションから「組合等コード」「都道府県コード」「年産」「共済目的」「組合員等コード」「氏名」「電話番号」「支所コード」「支所名」	
	        // 「市町村コード」「市町村名」「大地区コード」「大地区名」「小地区コード」「小地区名」「合併時識別コード」
	        // 「引受方式」「引受方式名称」「類区分」「類区分名称」「担手農家区分」を取得する。

            D105070ParamModel d105070Param = SessionUtil.Get<D105070ParamModel>(F105Const.SESS_D105070_PARAMS, context) ?? new();

            // 「組合等コード」
            KumiaitoCd = d105070Param.KumiaitoCd;
            // 「都道府県コード」
            TodofukenCd = d105070Param.TodofukenCd;
            // 「年産」
            Nensan = d105070Param.Nensan;
            // 「共済目的」
            KyosaiMokutekiCd = d105070Param.KyosaiMokutekiCd;
            // 「組合員等コード」
            KumiaiintoCd = d105070Param.KumiaiintoCd;
            // 「氏名」
            FullNm = d105070Param.FullNm;
            // 「電話番号」
            Tel = d105070Param.Tel;
            // 「支所コード」
            ShishoCd = d105070Param.ShishoCd;
            // 「支所名」
            ShishoNm = d105070Param.ShishoNm;
            // 「市町村コード」
            ShichosonCd = d105070Param.ShichosonCd;
            // 「市町村名」
            ShichosonNm = d105070Param.ShichosonNm;
            // 「大地区コード」
            DaichikuCd = d105070Param.DaichikuCd;
            // 「大地区名」
            DaichikuNm = d105070Param.DaichikuNm;
            // 「小地区コード」
            ShochikuCd = d105070Param.ShochikuCd;
            // 「小地区名」
            ShochikuNm = d105070Param.ShochikuNm;
            // 「合併時識別コード」
            GappeiShikibetsuCd = d105070Param.GappeiShikibetsuCd;

            // 「引受方式」
            HikiukeHoushikiCd = d105070Param.HikiukeHoushikiCd;
            // 「引受方式名称」
            HikiukeHoushikiNm = d105070Param.HikiukeHoushikiNm;
            // 「類区分」
            RuiKbn = d105070Param.RuiKbn;
            // 「類区分名称」
            RuiKbnNm = d105070Param.RuiKbnNm;
            // 「一筆半損特約」
            IppitsuHansonTokuyaku = d105070Param.IppitsuHansonTokuyaku;
            // 「補償割合コード」
            HoshoWariai = d105070Param.HoshoWariai;
            // 「担手農家区分」
            NinaiteKbn = d105070Param.NinaiteKbn;
        }
    }
}
