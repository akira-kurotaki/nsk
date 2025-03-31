using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using NskWeb.Areas.F000.Models.D000000;
using NskWeb.Areas.F111.Consts;
using NskWeb.Areas.F111.Models.D111010;
using NskWeb.Common.Consts;
using static NskCommonLibrary.Core.Consts.CoreConst;

namespace NskWeb.Areas.F111.Models.D111050
{
    /// <summary>
    /// セッション情報
    /// </summary>
    public class D111050SessionInfo
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
        /// 負担金交付区分コード
        /// </summary>
        public string FutankinKofuKbnCd { get; set; } = string.Empty;
        /// <summary>
        /// 年産
        /// </summary>
        public int Nensan { get; set; }
        /// <summary>
        /// 交付回
        /// </summary>
        public int Koufukai { get; set; }
        /// <summary>
        /// 負担金交付区分
        /// </summary>
        public string FutankinKofuKbn { get; set; } = string.Empty;
        /// <summary>
        /// 対象共済目的コード
        /// </summary>
        public string TaishouKyosaiMokutekiCd { get; set; } = string.Empty;
        /// <summary>
        /// セッション情報取得
        /// </summary>
        /// <param name="context"></param>
        public void GetInfo(HttpContext context)
        {
            // １．１．セッションから「都道府県コード」「組合等コード」「負担金交付区分」「年産」を取得する。
            Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context) ?? new Syokuin();

            NSKPortalInfoModel potalModel = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, context);

            D111010ParamModel d111010ParamModel = SessionUtil.Get<D111010ParamModel>(F111Const.SESS_D111010_PARAMS, context);

            // 「都道府県コード」
            TodofukenCd = syokuin.TodofukenCd;
            // 「組合等コード」
            KumiaitoCd = syokuin.KumiaitoCd;
            // 「年産」
            Nensan = int.TryParse(potalModel?.SNensanHikiuke, out int nensan) ? nensan : 0;
            // 「交付回」
            Koufukai = d111010ParamModel.Koufukai;
            // 「負担金交付区分」
            FutankinKofuKbnCd = d111010ParamModel?.FutankinKofuKbnCd;

            // １．２．[負担金交付区分]を設定する。
            if (FutankinKofuKbnCd.Equals(Convert.ToInt32(FutankinKofuKbnNumber.Ine).ToString()))
            {
                FutankinKofuKbn = FutankinKofuKbnNumber.Ine.ToDescription();
                TaishouKyosaiMokutekiCd = ((int)KyosaiMokutekiCdNumber.Suitou).ToString() + "," + ((int)KyosaiMokutekiCdNumber.Rikutou).ToString();
            }
            else
            {
                FutankinKofuKbn = FutankinKofuKbnNumber.Mugi.ToDescription();
                TaishouKyosaiMokutekiCd = ((int)KyosaiMokutekiCdNumber.Mugi).ToString();
            }

        }
    }
}
