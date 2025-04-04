using NskReportMain.Models.NSK_P107020;
using NskReportMain.Reports;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using GrapeCity.ActiveReports;
using NskAppModelLibrary.Models;
using CoreLibrary.Core.Consts;
using NskCoreConst = NskCommonLibrary.Core.Consts.CoreConst;

namespace NskReportMain.ReportCreators.NSK_P107020
{
    /// <summary>
    /// P107020_耕地等情報（６号）
    /// </summary>
    public class NSK_P107020Creator : ReportCreator
    {
        #region メンバー定数
        /// <summary>
        /// 加入形態「個人」
        /// </summary>
        private static readonly string KANYU_KEITAI_KOJIN = "1";

        /// <summary>
        /// 加入形態「法人」
        /// </summary>
        private static readonly string KANYU_KEITAI_HOJIN = "2";

        /// <summary>
        /// 加入形態「農作物資格団体」
        /// </summary>
        private static readonly string KANYU_KEITAI_DANTAI = "10";

        #endregion

        #region P107020_耕地等情報（６号）
        /// <summary>
        /// P107020_耕地等情報（６号）
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="reportJoukens">帳票条件</param>
        /// <param name="model">出力対象データ</param>
        /// <returns>実行結果</returns>
        public CreatorResult CreateReport(
            string joukenId,
            List<T01050バッチ条件> reportJoukens,
            List<NSK_P107020Model> model)
        {
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P107020_耕地等情報（６号）",
                string.Empty));

            // 実行結果
            CreatorResult result = new CreatorResult();

            // 出力条件がない場合、エラー
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
            report = new BaseSectionReport(@"Reports\P107020\P107020Report.rpx");
            // 帳票のデータ設定
            SetData(model, ref report);
            // 帳票を呼び出す
            report.Run();

            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P107020_耕地等情報（６号）"));

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
        /// <param name="model"></param>
        /// <param name="rpt"></param>
        private void SetData(List<NSK_P107020Model> model, ref SectionReport rpt)
        {
            #region 編集

            for (int i = 0; i < model.Count; i++)
            {
                var item = model[i];

                // 耕地番号
                item.耕地番号 = item.耕地番号 is null ? string.Empty : item.耕地番号.PadLeft(5, '0');

                // 分筆番号
                item.分筆番号 = item.分筆番号 is null ? string.Empty : item.分筆番号.PadLeft(4, '0');

                // 加入形態
                // 個人チェックボックス
                item.個人 = KANYU_KEITAI_KOJIN.Equals(item.加入形態);

                // 法人チェックボックス
                item.法人 = KANYU_KEITAI_HOJIN.Equals(item.加入形態);

                // 農作物資格団体チェックボックス
                item.農作物資格団体 = KANYU_KEITAI_DANTAI.Equals(item.加入形態);

                // 類区分
                // 共済目的コードが「陸稲」の場合以外、出力
                if (((int)NskCoreConst.KyosaiMokutekiCdNumber.Rikutou).ToString().Equals(item.共済目的コード) == false)
                {
                    //類区分 + "類" + 
                    item.類区分_表示 = item.類区分 + "類 " + item.類短縮名称;
                }

                // 品種又は転作作物名等
                item.品種又は転作作物名等 = item.品種コード + CoreConst.HALF_WIDTH_SPACE + item.品種名等;

                // 組合員等コード
                item.組合員等コード = item.組合員等コード is null ? string.Empty :  item.組合員等コード.PadLeft(13, '0');

                // ページ右
                item.ページ右 = (item.大地区コード is null ? string.Empty : item.大地区コード.PadLeft(2, '0')) + CoreConst.HALF_WIDTH_SPACE +
                    (item.小地区コード is null ? string.Empty : item.小地区コード.PadLeft(4, '0')) + CoreConst.HALF_WIDTH_SPACE +
                    item.組合員等コード;

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
