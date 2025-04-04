using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using NskCommonLibrary.Core.Consts;
using NskWeb.Areas.F000.Models.D000000;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105070;
using NskWeb.Common.Consts;
using NskWeb.Common.Models;

namespace NskWeb.Areas.F105.Models.D105150
{
    /// <summary>
    /// セッション情報
    /// </summary>
    public class D105150SessionInfo : BaseSessionInfo
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
        /// 利用可能な支所一覧
        /// </summary>
        public List<Shisho> ShishoList { get; set; } = new();

        /// <summary>
        /// セッション情報取得
        /// </summary>
        /// <param name="context"></param>
        public void GetInfo(HttpContext context)
        {
            // ２．１．セッションから「組合等コード」「都道府県コード」「年産」「共済目的」「組合員等コード」「氏名」「電話番号」「支所コード」「支所名」
            // 「市町村コード」「市町村名」「大地区コード」「大地区名」「小地区コード」「小地区名」「合併時識別コード」を取得する。
            Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context) ?? new Syokuin();

            NSKPortalInfoModel potalModel = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, context);

            // 「組合等コード」
            KumiaitoCd = syokuin.KumiaitoCd;
            // 「都道府県コード」
            TodofukenCd = syokuin.TodofukenCd;
            // 「年産」
            Nensan = int.TryParse(potalModel?.SNensanHikiuke, out int nensan) ? nensan : 0;
            // 「共済目的」
            KyosaiMokutekiCd = potalModel?.SKyosaiMokutekiCd;

            // 利用可能な支所一覧
            ShishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, context) ?? new();
        }
    }
}
