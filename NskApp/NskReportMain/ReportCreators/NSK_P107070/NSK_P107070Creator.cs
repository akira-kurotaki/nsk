using NskReportMain.Models.NSK_P107070;
using NskReportMain.Reports;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using GrapeCity.ActiveReports;
using CoreLibrary.Core.Utility;
using JoukenNameConst = NskCommonLibrary.Core.Consts.JoukenNameConst;
using NskAppModelLibrary.Models;
using GrapeCity.ActiveReports.SectionReportModel;


namespace NskReportMain.ReportCreators.NSK_P107070
{

    /// <summary>
    /// P107070_徴収管理簿
    /// </summary>
    public class NSK_P107070Creator : ReportCreator
    {

        #region メンバー定数
        /// <summary>
        /// 振込引落区分コード「現金」
        /// </summary>
        private static readonly string HIKIOTOSHI_KBN_CD_CASH = "4";

        /// <summary>
        /// 備考欄 振込引落区分が「現金」時の出力文言
        /// </summary>
        private static readonly string BIKO_STR_HIKIOTOSHI_CASH = "*";

        /// <summary>
        /// 備考欄 引受解除時の出力文言
        /// </summary>
        private static readonly string BIKO_STR_HIKIUKE_KAIJYO = "引受解除";

        /// <summary>
        /// 条件名値：現金徴収のみ「有」
        /// </summary>
        public static readonly string JOUKEN_GENKIN_CHOSHU_ARI = "1";

        /// <summary>
        /// 条件名値：現金徴収のみ「無」
        /// </summary>
        public static readonly string JOUKEN_GENKIN_CHOSHU_NASHI = "0";

        #endregion

        #region P107070_徴収管理簿
        /// <summary>
        /// P107070_徴収管理簿
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="reportJoukens">帳票条件</param>
        /// <param name="model">出力対象データ</param>
        /// <returns>実行結果</returns>
        public CreatorResult CreateReport(
            string joukenId,
            string kumiaitoCd,
            List<T01050バッチ条件> reportJoukens,
            List<NSK_P107070Model> model)
        {
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P107070_徴収管理簿",
                string.Empty));

            // 実行結果
            CreatorResult result = new CreatorResult();

            // 出力条件がない場合、エラーとし、エラーメッセージを返す
            if (reportJoukens == null || reportJoukens.Count == 0)
            {
                return result.CreateResultError("ME01054", ReportConst.PARAM_NAME_OUTPUT_JOUKEN);
            }

            // 出力対象データがないの場合、エラーとし、エラーメッセージを返す
            if (model == null || model.Count == 0)
            {
                return result.CreateResultError("ME01054", ReportConst.PARAM_NAME_OUTPUT_DATA);
            }

            // 帳票を作成する
            report = new BaseSectionReport(@"Reports\P107070\P107070Report.rpx");
            // 帳票のデータ設定
            SetData(kumiaitoCd, reportJoukens, model, ref report);
            // 帳票を呼び出す
            report.Run();

            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P107070_徴収管理簿"));

            // 作成された帳票を返却する
            result.Result = ReportConst.RESULT_SUCCESS;
            result.SectionReport = report;
            return result;
        }
        #endregion

        #region P107070_徴収管理簿レポートデータ設定処理
        /// <summary>
        /// セクションレポートのコンポーネントに値を設定する
        /// </summary>
        /// 
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="reportJoukens">帳票条件</param>
        /// <param name="model"></param>
        /// <param name="rpt"></param>
        private void SetData(
            string kumiaitoCd,
            List<T01050バッチ条件> reportJoukens,
            List<NSK_P107070Model> model,
            ref SectionReport rpt)
        {
            #region 各明細共通値を取得

            // 年産
            string nensan = string.Empty;
            var jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_NENSAN).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                nensan = DateUtil.GetReportJapaneseYear(int.Parse(jouken.条件値));
            }

            // 現金等予定者（有の場合、表示、　無の場合、非表示）
            // 注意書　　　（有の場合、非表示、無の場合、表示）
            string genChoshuFlg = "";
            bool genYotei = false;
            bool chuiGaki = false;
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_GENKIN_CHOSHU).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                genChoshuFlg = jouken.条件値;
                genYotei = JOUKEN_GENKIN_CHOSHU_ARI.Equals(jouken.条件値);
                chuiGaki = JOUKEN_GENKIN_CHOSHU_NASHI.Equals(jouken.条件値);
            }
            ((Label)rpt.Sections["PageHeader"].Controls["lblCashYotei"]).Visible = genYotei;
            ((Label)rpt.Sections["PageFooter"].Controls["lblChuigaki"]).Visible = chuiGaki;

            // 日付
            string outDate = DateUtil.GetReportJapaneseDate(DateUtil.GetSysDateTime());

            #endregion

            #region 編集

            for (int i = 0; i < model.Count; i++)
            {
                var item = model[i];

                // 年産_表示
                item.年産_表示 = nensan;

                // 日付_表示
                item.日付_表示 = outDate;

                // 組合等コード_表示
                item.組合等コード_表示 = kumiaitoCd;

                // 大地区コード
                item.大地区コード = item.大地区コード is null ? string.Empty : item.大地区コード.PadLeft(2, '0');

                // 小地区コード
                item.小地区コード = item.小地区コード is null ? string.Empty : item.小地区コード.PadLeft(4, '0');

                // 組合員等コード
                item.組合員等コード = item.組合員等コード is null ? string.Empty : item.組合員等コード.PadLeft(13, '0');

                // 特別防災賦課金
                item.特別防災賦課金 = item.特別賦課金 + item.防災賦課金;

                // 徴収済額
                item.徴収済額 = item.前回迄徴収額 - item.前回迄引受解除徴収賦課金額;

                // 今回徴収額
                item.今回徴収額 = (item.今回迄徴収額 - item.前回迄徴収額) > 0 ? (item.今回迄徴収額 - item.前回迄徴収額) : 0;

                // 還付額
                item.還付額 = (item.納入額 - (item.前回迄徴収額 - item.今回迄引受解除徴収賦課金額)) < 0 ? ((item.前回迄徴収額 - item.今回迄引受解除徴収賦課金額) - item.納入額) : 0;

                // 徴収年月日（表示用）
                item.徴収年月日_表示 = item.納入額 == ((item.今回迄徴収額 - item.今回迄引受解除徴収賦課金額))  ? DateUtil.GetJapaneseDate(item.徴収年月日) : string.Empty;

                // 未納額
                item.未納額 = (item.納入額 - (item.今回迄徴収額 - item.今回迄引受解除徴収賦課金額)) > 0 ? (item.納入額 - (item.今回迄徴収額 - item.今回迄引受解除徴収賦課金額)) : 0;

                // TOTAL徴収額
                item.TOTAL徴収額 = item.今回迄徴収額 - item.今回迄引受解除徴収賦課金額;

                // 備考
                string bikoStr = string.Empty;
                //　「現金徴収のみ」が「無」の場合、かつ、振込引落区分コードが4(現金)の場合
                if (JOUKEN_GENKIN_CHOSHU_NASHI.Equals(genChoshuFlg) && HIKIOTOSHI_KBN_CD_CASH.Equals(item.振込引落区分コード))
                {
                    bikoStr += BIKO_STR_HIKIOTOSHI_CASH;
                }
                // 解除フラグ=1の場合
                if (ReportConst.FLG_ON.Equals(item.解除フラグ))
                {
                    bikoStr += BIKO_STR_HIKIUKE_KAIJYO;
                }
                item.備考 = bikoStr;

                // 配列へ再設定
                model[i] = item;
            }

            //データソース設定
            rpt.DataSource = model;
            #endregion

        }
        #endregion
    }
}
