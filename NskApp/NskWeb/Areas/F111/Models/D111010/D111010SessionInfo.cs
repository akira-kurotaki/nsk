using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using NskWeb.Areas.F000.Models.D000000;
using NskWeb.Common.Consts;
using NskWeb.Areas.F111.Consts;
using static NskCommonLibrary.Core.Consts.CoreConst;
using CoreLibrary.Core.Extensions;

namespace NskWeb.Areas.F111.Models.D111010
{
    /// <summary>
    /// セッション情報
    /// </summary>
    public class D111010SessionInfo
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; } = string.Empty;
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; } = string.Empty;
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
        /// 負担金交付区分コード
        /// </summary>
        public string FutankinKofuKbnCd { get; set; } = string.Empty;
        /// <summary>
        /// 負担金交付区分
        /// </summary>
        public string FutankinKofuKbn { get; set; } = string.Empty;

        /// <summary>
        /// セッション情報取得
        /// </summary>
        /// <param name="context"></param>
        public void GetInfo(HttpContext context)
        {
            // ２．１．セッションから「都道府県コード」「組合等コード」「年産」「共済目的コード」「支所コード」を取得する。
            Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context) ?? new Syokuin();

            NSKPortalInfoModel potalModel = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, context);

            // 「都道府県コード」
            TodofukenCd = syokuin.TodofukenCd;
            // 「組合等コード」
            KumiaitoCd = syokuin.KumiaitoCd;
            // 「年産」
            Nensan = int.TryParse(potalModel?.SNensanHikiuke, out int nensan) ? nensan : 0;// SessionUtil.Get<int>("NENSAN", context);
            // 「共済目的」
            KyosaiMokutekiCd = potalModel?.SKyosaiMokutekiCd;
            // 「支所コード」
            ShishoCd = syokuin.ShishoCd;

            // ２．２．[負担金交付区分]を設定する。
            if (KyosaiMokutekiCd.Equals(Convert.ToInt32(KyosaiMokutekiCdNumber.Suitou).ToString())
                 ||KyosaiMokutekiCd.Equals(Convert.ToInt32(KyosaiMokutekiCdNumber.Rikutou).ToString()))
            {
                FutankinKofuKbn = FutankinKofuKbnNumber.Ine.ToDescription();
                FutankinKofuKbnCd = ((int)FutankinKofuKbnNumber.Ine).ToString();
            }
            else if (KyosaiMokutekiCd.Equals(Convert.ToInt32(KyosaiMokutekiCdNumber.Mugi).ToString()))
            {
                FutankinKofuKbn = FutankinKofuKbnNumber.Mugi.ToDescription();
                FutankinKofuKbnCd = ((int)FutankinKofuKbnNumber.Mugi).ToString();
            }

        }
    }
}
