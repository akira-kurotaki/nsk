using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using NskWeb.Areas.F000.Models.D000000;
using NskWeb.Common.Consts;

namespace NskWeb.Areas.F109.Models.D109020
{
    /// <summary>
    /// セッション情報
    /// </summary>
    public class D109020SessionInfo
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
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; } = string.Empty;

        /// <summary>
        /// 引受計算支所実行単位区分_引受
        /// </summary>
        public string HikiukeJikkoTanniKbnHikiuke { get; set; } = string.Empty;

        /// <summary>
        /// ユーザーID
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 利用可能支所一覧
        /// </summary>
        public List<Shisho> RiyokanoShishoList = new();

        /// <summary>
        /// セッション情報取得
        /// </summary>
        /// <param name="context"></param>
        public void GetInfo(HttpContext context)
        {
            // ２．１．セッションから「都道府県コード」「組合等コード」「支所コード」「利用可能支所コード」「ユーザーID」
            // 「画面操作権限区分コード」「共済目的コード」「引受年産」を取得する。
            Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context) ?? new Syokuin();

            NSKPortalInfoModel potalModel = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, context);

            // 「都道府県コード」
            TodofukenCd = syokuin.TodofukenCd;
            // 「組合等コード」
            KumiaitoCd = syokuin.KumiaitoCd;
            // 「支所コード」
            ShishoCd = syokuin.ShishoCd;

            // 「利用可能支所一覧」
            RiyokanoShishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, context) ?? new();
            // 「ユーザーID」
            UserId = syokuin.UserId;
            // 「共済目的コード」
            KyosaiMokutekiCd = potalModel?.SKyosaiMokutekiCd;
            // 「引受年産」
            Nensan = int.TryParse(potalModel?.SNensanHikiuke, out int nensan) ? nensan : 0;

            // 「引受計算支所実行単位区分_引受」
            HikiukeJikkoTanniKbnHikiuke = potalModel?.SHikiukeJikkoTanniKbnHikiuke;
        }
    }
}
