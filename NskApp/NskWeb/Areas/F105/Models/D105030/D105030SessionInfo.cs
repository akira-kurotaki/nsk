using CoreLibrary.Core.Utility;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105036;
using NskWeb.Common.Models;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// セッション情報
    /// </summary>
    public class D105030SessionInfo : BaseSessionInfo
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

        /// <summary>
        /// セッション情報取得
        /// </summary>
        /// <param name="context"></param>
        public void GetInfo(HttpContext context)
        {
            // ２．１．セッションから「組合等コード」「都道府県コード」「年産」「共済目的」「組合員等コード」「氏名」「電話番号」「支所コード」「支所名」
            // 「市町村コード」「市町村名」「大地区コード」「大地区名」「小地区コード」「小地区名」「合併時識別コード」を取得する。
            D105036ParamModel d105036Param = SessionUtil.Get<D105036ParamModel>(F105Const.SESS_D105036_PARAMS, context) ?? new ();

            // 「組合等コード」
            KumiaitoCd = d105036Param.KumiaitoCd;
            // 「都道府県コード」
            TodofukenCd = d105036Param.TodofukenCd;
            // 「年産」
            Nensan = d105036Param.Nensan;
            // 「共済目的」
            KyosaiMokutekiCd = d105036Param.KyosaiMokutekiCd;
            // 「組合員等コード」
            KumiaiintoCd = d105036Param.KumiaiintoCd;
            // 「氏名」
            FullNm = d105036Param.FullNm;
            // 「電話番号」
            Tel = d105036Param.Tel;
            // 「支所コード」
            ShishoCd = d105036Param.ShishoCd;
            // 「支所名」
            ShishoNm = d105036Param.ShishoNm;
            // 「市町村コード」
            ShichosonCd = d105036Param.ShichosonCd;
            // 「市町村名」
            ShichosonNm = d105036Param.ShichosonNm;
            // 「大地区コード」
            DaichikuCd = d105036Param.DaichikuCd;
            // 「大地区名」
            DaichikuNm = d105036Param.DaichikuNm;
            // 「小地区コード」
            ShochikuCd = d105036Param.ShochikuCd;
            // 「小地区名」
            ShochikuNm = d105036Param.ShochikuNm;
            // 「合併時識別コード」
            GappeiShikibetsuCd = d105036Param.GappeiShikibetsuCd;
        }
    }
}
