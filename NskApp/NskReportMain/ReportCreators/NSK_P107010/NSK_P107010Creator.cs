using NskReportMain.Models.NSK_P107010;
using NskReportMain.Reports;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using GrapeCity.ActiveReports;
using NskAppModelLibrary.Models;
using CoreLibrary.Core.Utility;
using System.Data;
using NskCommonLibrary.Core.Consts;
using JoukenNameConst = NskCommonLibrary.Core.Consts.JoukenNameConst;
using GrapeCity.ActiveReports.Document.Drawing;

namespace NskReportMain.ReportCreators.NSK_P107010
{
    /// <summary>
    /// P107010_加入承諾書兼共済掛金等払込通知書（６号）
    /// </summary>
    public class NSK_P107010Creator : ReportCreator
    {

        #region メンバー定数
        /// <summary>
        /// 組合長印の押印「有」
        /// </summary>
        private static readonly string JOUKEN_KUMIAICHO_STAMP_ARI = "1";

        /// <summary>
        /// 文言no「文書欄の上段用文言（徴収用）」
        /// </summary>
        private static readonly Decimal MONGON_NO_CHOCHU_UPPER = 1;

        /// <summary>
        /// 文言no「文書欄の耕地等情報用文言（徴収用）」
        /// </summary>
        private static readonly Decimal MONGON_NO_CHOCHU_KOUCHI = 2;

        /// <summary>
        /// 文言no「文書欄の下段用文言（徴収用）」
        /// </summary>
        private static readonly Decimal MONGON_NO_CHOCHU_LOWER = 3;

        /// <summary>
        /// 文言no「文書欄の上段用文言（還付用）」
        /// </summary>
        private static readonly Decimal MONGON_NO_KANPU_UPPER = 4;

        /// <summary>
        /// 文言no「文書欄の耕地等情報用文言（還付用）」
        /// </summary>
        private static readonly Decimal MONGON_NO_KANPU_KOUCHI = 5;

        /// <summary>
        /// 文言no「文書欄の下段用文言（還付用）」
        /// </summary>
        private static readonly Decimal MONGON_NO_KANPU_LOWER = 6;

        /// <summary>
        /// 特約区分「半損特約」
        /// </summary>
        private static readonly string TOKUYAKU_KBN_HANSON = "4";

        #endregion

        #region P107010_加入承諾書兼共済掛金等払込通知書（６号）
        /// <summary>
        /// P107010_加入承諾書兼共済掛金等払込通知書（６号）
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="reportJoukens">T01050バッチ条件モデルリスト</param>
        /// <param name="mongonModel">M00210様式文言モデルリスト</param>
        /// <param name="headerModel">組合員等コードヘッダーモデルリスト</param>
        /// <param name="sub1Model">加入承諾書兼共済掛金等払込通知書Sub1モデルリスト</param>
        /// <param name="sub2Model">組加入承諾書兼共済掛金等払込通知書Sub2モデルリスト</param>
        /// <returns>実行結果</returns>
        public CreatorResult CreateReport(
            string joukenId,
            List<T01050バッチ条件> reportJoukens,
            List<M00210様式文言> mongonModel, 
            List<NSK_P107010HeaderModel> headerModel,
            List<NSK_P107010Sub1Model> sub1Model,
            List<NSK_P107010Sub2Model> sub2Model
            )
        {
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P107010_加入承諾書兼共済掛金等払込通知書（６号）",
                string.Empty));

            // 実行結果
            CreatorResult result = new CreatorResult();

            // 出力条件がない場合、エラーとし、エラーメッセージを返す
            if (reportJoukens == null || reportJoukens.Count == 0)
            {
                return result.CreateResultError("ME01054", ReportConst.PARAM_NAME_OUTPUT_JOUKEN);
            }

            // 様式文言
            // 出力対象データがないの場合、エラーとし、エラーメッセージを返す
            if (mongonModel == null || mongonModel.Count == 0)
            {
                return result.CreateResultError("ME01054", ReportConst.PARAM_NAME_OUTPUT_DATA);
            }

            // 組合員等コードヘッダー
            // 出力対象データがないの場合、エラーとし、エラーメッセージを返す
            if (headerModel == null || headerModel.Count == 0)
            {
                return result.CreateResultError("ME01054", ReportConst.PARAM_NAME_OUTPUT_DATA);
            }

            // 加入承諾書兼共済掛金等払込通知書Sub1
            // 出力対象データがないの場合、エラーとし、エラーメッセージを返す
            if (sub1Model == null || sub1Model.Count == 0)
            {
                return result.CreateResultError("ME01054", ReportConst.PARAM_NAME_OUTPUT_DATA);
            }

            // 加入承諾書兼共済掛金等払込通知書Sub2
            // 出力対象データがないの場合、エラーとし、エラーメッセージを返す
            if (sub2Model == null || sub2Model.Count == 0)
            {
                return result.CreateResultError("ME01054", ReportConst.PARAM_NAME_OUTPUT_DATA);
            }

            // 帳票を作成する
            report = new BaseSectionReport(@"Reports\P107010\P107010Report.rpx");
            // ActiveReport内スクリプトのクラス参照を設定する
            report.AddScriptReference("@NskReportMain.dll");
            // 帳票のデータ設定
            SetHeaderData(reportJoukens, mongonModel, headerModel, ref report);  // 組合員等コードヘッダー
            SetSub1Data(sub1Model, ref report);                                  // 加入承諾書兼共済掛金等払込通知書Sub1
            SetSub2Data(sub2Model, ref report);                                  // 加入承諾書兼共済掛金等払込通知書Sub2
            // 帳票を呼び出す
            report.Run();

            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P107010_加入承諾書兼共済掛金等払込通知書（６号）"));

            // 作成された帳票を返却する
            result.Result = ReportConst.RESULT_SUCCESS;
            result.SectionReport = report;
            return result;
        }
        #endregion

        #region 組合員等コードヘッダーのデータ編集、レポートに設定
        /// <summary>
        /// 組合員等コードヘッダーのデータ編集、レポートに設定
        /// </summary>
        /// <param name="reportJoukens">T01050バッチ条件モデルリスト</param>
        /// <param name="mongonModel">M00210様式文言モデルリスト</param>
        /// <param name="headerModel">組合員等コードヘッダーモデルリスト</param>
        /// <param name="rpt">帳票オブジェクト</param>
        private void SetHeaderData(
            List<T01050バッチ条件> reportJoukens,
            List<M00210様式文言> mongonModel,
            List<NSK_P107010HeaderModel> headerModel,
            ref SectionReport rpt)
        {

            #region 共通値取得

            // 年産
            string nensan = string.Empty;
            var jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_NENSAN).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                nensan = DateUtil.GetReportJapaneseYear(int.Parse(jouken.条件値));
            }

            // 申込日
            string hakkoDate = string.Empty;
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_HAKKO_DATE).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                DateTime dt;
                DateTime.TryParse(jouken.条件値, out dt);
                hakkoDate = DateUtil.GetReportJapaneseDate(dt);
            }

            // 組合代表者印 ファイル
            string filePath = Path.Combine(headerModel[0].ファイルパス, headerModel[0].ファイル名);
            Image daihyoStampFile = Image.FromFile(filePath);

            // 組合代表者印 表示有無
            bool stampVisible = false;
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KUMIAICHO_STAMP).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                stampVisible = JOUKEN_KUMIAICHO_STAMP_ARI.Equals(jouken.条件値.ToString());
            }

            // 文書欄の上段用文言（徴収用）
            string choshuUpperStr = string.Empty;
            var mongon = mongonModel.Where(t => t.文言no == MONGON_NO_CHOCHU_UPPER).SingleOrDefault();
            if (!string.IsNullOrEmpty(mongon?.文言))
            {
                choshuUpperStr = mongon.文言;
            }

            // 文書欄の耕地等情報用文言（徴収用）
            string choshuKouchiStr = string.Empty;
            mongon = mongonModel.Where(t => t.文言no == MONGON_NO_CHOCHU_KOUCHI).SingleOrDefault();
            if (!string.IsNullOrEmpty(mongon?.文言))
            {
                choshuKouchiStr = mongon.文言;
            }

            // 文書欄の下段用文言（徴収用）
            string choshuLowerStr = string.Empty;
            mongon = mongonModel.Where(t => t.文言no == MONGON_NO_CHOCHU_LOWER).SingleOrDefault();
            if (!string.IsNullOrEmpty(mongon?.文言))
            {
                choshuLowerStr = mongon.文言;
            }

            // 文書欄の上段用文言（還付用）
            string kanpuUpperStr = string.Empty;
            mongon = mongonModel.Where(t => t.文言no == MONGON_NO_KANPU_UPPER).SingleOrDefault();
            if (!string.IsNullOrEmpty(mongon?.文言))
            {
                kanpuUpperStr = mongon.文言;
            }

            // 文書欄の耕地等情報用文言（還付用）
            string kanpuKouchiStr = string.Empty;
            mongon = mongonModel.Where(t => t.文言no == MONGON_NO_KANPU_KOUCHI).SingleOrDefault();
            if (!string.IsNullOrEmpty(mongon?.文言))
            {
                kanpuKouchiStr = mongon.文言;
            }

            // 文書欄の下段用文言（還付用）
            string kanpuLowerStr = string.Empty;
            mongon = mongonModel.Where(t => t.文言no == MONGON_NO_KANPU_LOWER).SingleOrDefault();
            if (!string.IsNullOrEmpty(mongon?.文言))
            {
                kanpuLowerStr = mongon.文言;
            }

            // 共済関係成立日
            string seiritsuDate = string.Empty;
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KYOSAI_SEIRITSU_DATE).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                DateTime dt;
                DateTime.TryParse(jouken.条件値, out dt);
                seiritsuDate = DateUtil.GetReportJapaneseDate(dt);
            }

            // 払込期限
            string haraikomiKigen = string.Empty;
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_HARAIKOMI_KIGEN).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                DateTime dt;
                DateTime.TryParse(jouken.条件値, out dt);
                haraikomiKigen = DateUtil.GetReportJapaneseDate(dt);
            }

            // 口座振替日
            string furikaeDate = string.Empty;
            jouken = reportJoukens.Where(t => t.条件名称 == JoukenNameConst.JOUKEN_KOZA_FURIKAE_DATE).SingleOrDefault();
            if (!string.IsNullOrEmpty(jouken?.条件値))
            {
                DateTime dt;
                DateTime.TryParse(jouken.条件値, out dt);
                furikaeDate = DateUtil.GetReportJapaneseDate(dt);
            }

            #endregion


            #region 明細編集

            for (int i = 0; i < headerModel.Count; i++)
            {
                var item = headerModel[i];

                // 組合等コード
                item.組合等コード = item.組合等コード is null ? string.Empty : item.組合等コード.PadLeft(3, '0');

                // 共済目的コード
                item.共済目的コード = item.共済目的コード is null ? string.Empty : item.共済目的コード.PadLeft(2, '0');

                // 支所コード
                item.支所コード = item.支所コード is null ? string.Empty : item.支所コード.PadLeft(2, '0');

                // 組合員等コード
                item.組合員等コード = item.組合員等コード is null ? string.Empty : item.組合員等コード.PadLeft(13, '0');

                // 年産
                item.年産_表示 = nensan;

                // 申込者氏名
                item.氏名又は法人名 = item.氏名又は法人名 + "  様";

                // 申込日
                item.申込日_表示 = hakkoDate;

                // 組合代表者印_表示有無
                item.組合代表者印_表示有無 = stampVisible;

                // 共済関係成立日
                item.共済関係成立日_表示 = seiritsuDate;

                // 負担共済掛金事務費賦課金の合計_表示
                item.負担共済掛金事務費賦課金の合計_表示 = item.組合員等負担共済掛金 + item.賦課金計;

                // 既納入額
                item.既納入額_表示 = item.今回迄徴収額 - item.今回迄引受解除徴収賦課金額;

                // 納入額
                item.納入額_表示 = (item.組合員等負担共済掛金 + item.賦課金計 - (item.今回迄徴収額 - item.今回迄引受解除徴収賦課金額));

                // 払込期限
                item.払込期限_表示 = haraikomiKigen;

                // 口座振替日
                item.口座振替日_表示 = furikaeDate;

                // 自動継続の有_表示有無
                item.自動継続特約の有_表示有無 = ReportConst.FLG_ON.Equals(item.継続特約フラグ);

                // 自動継続の無_表示有無
                item.自動継続特約の無_表示有無 = ReportConst.FLG_OFF.Equals(item.継続特約フラグ);

                // 文書文言、サブレポート表示有無
                if (0 <= item.納入額_表示)
                {
                    // 徴収用文言を設定
                    item.文書_上段_表示 = choshuUpperStr;
                    item.文書_耕地等情報_表示 = choshuKouchiStr;
                    item.文書_下段_表示 = choshuUpperStr;

                    // サブレポートは表示
                    item.SUB1_表示有無 = true;
                    item.SUB2_表示有無 = true;
                }
                else
                {
                    // 還付用文言を設定
                    item.文書_上段_表示 = kanpuUpperStr;
                    item.文書_耕地等情報_表示 = kanpuKouchiStr;
                    item.文書_下段_表示 = kanpuLowerStr;

                    // サブレポートは非表示
                    item.SUB1_表示有無 = false;
                    item.SUB2_表示有無 = false;
                }

                // ページ
                item.ページ = (item.大地区コード is null ? string.Empty : item.大地区コード.PadLeft(2, '0')) + CoreConst.HALF_WIDTH_SPACE +
                    (item.小地区コード is null ? string.Empty : item.小地区コード.PadLeft(4, '0')) + CoreConst.HALF_WIDTH_SPACE +
                    item.組合員等コード;

                // 配列へ再設定
                headerModel[i] = item;
            }
            #endregion

            // 組合代表印ファイル 引き渡し（ActiveReportスクリプトで設定）
            rpt.AddNamedItem("daihyoStampFile", daihyoStampFile);

            //データソース設定
            rpt.DataSource = headerModel;
        }
        #endregion

        #region 加入承諾書兼共済掛金等払込通知書Sub1のデータ編集、レポートに設定
        /// <summary>
        /// 組合員等コードヘッダーのデータ編集、レポートに設定
        /// </summary>
        /// <param name="sub1Model">加入承諾書兼共済掛金等払込通知書Sub1モデルリスト</param>
        /// <param name="rpt">帳票オブジェクト</param>
        private void SetSub1Data(
            List<NSK_P107010Sub1Model> sub1Model,
            ref SectionReport rpt)
        {
            #region 明細編集

            for (int i = 0; i < sub1Model.Count; i++)
            {
                var item = sub1Model[i];

                // 組合等コード
                item.組合等コード = item.組合等コード is null ? string.Empty : item.組合等コード.PadLeft(3, '0');

                // 共済目的コード
                item.共済目的コード = item.共済目的コード is null ? string.Empty : item.共済目的コード.PadLeft(2, '0');

                // 支所コード
                item.支所コード = item.支所コード is null ? string.Empty : item.支所コード.PadLeft(2, '0');

                // 組合員等コード
                item.組合員等コード = item.組合員等コード is null ? string.Empty : item.組合員等コード.PadLeft(13, '0');

                // 一筆半損特約の有無
                item.一筆半損特約の有無_表示 = TOKUYAKU_KBN_HANSON.Equals(item.特約区分) ? "有り": "無し";

                // 全相殺方式等の収穫量の確認方法（数字を①～⑥に変換）
                string kbnStr = string.Empty;
                switch (item.加入申込区分)
                {
                    case "1":
                        kbnStr = "①";
                        break;
                    case "2":
                        kbnStr = "②";
                        break;
                    case "3":
                        kbnStr = "③";
                        break;
                    case "4":
                        kbnStr = "④";
                        break;
                    case "5":
                        kbnStr = "⑤";
                        break;
                    case "6":
                        kbnStr = "⑥";
                        break;
                }
                item.全相殺方式等の収穫量の確認方法_表示 = kbnStr;

                // 配列へ再設定
                sub1Model[i] = item;
            }
            #endregion

            //データ引き渡し(ActiveReportスクリプトでデータソース設定)
            rpt.AddNamedItem("sub1Model", sub1Model);
        }
        #endregion

        #region 加入承諾書兼共済掛金等払込通知書Sub2のデータ編集、レポートに設定
        /// <summary>
        /// 組合員等コードヘッダーのデータ編集、レポートに設定
        /// </summary>
        /// <param name="sub2Model">加入承諾書兼共済掛金等払込通知書Sub2モデルリスト</param>
        /// <param name="rpt">帳票オブジェクト</param>
        private void SetSub2Data(
            List<NSK_P107010Sub2Model> sub2Model,
            ref SectionReport rpt)
        {
            #region 明細編集

            for (int i = 0; i < sub2Model.Count; i++)
            {
                var item = sub2Model[i];

                // 組合等コード
                item.組合等コード = item.組合等コード is null ? string.Empty : item.組合等コード.PadLeft(3, '0');

                // 共済目的コード
                item.共済目的コード = item.共済目的コード is null ? string.Empty : item.共済目的コード.PadLeft(2, '0');

                // 支所コード
                item.支所コード = item.支所コード is null ? string.Empty : item.支所コード.PadLeft(2, '0');

                // 組合員等コード
                item.組合員等コード = item.組合員等コード is null ? string.Empty : item.組合員等コード.PadLeft(13, '0');

                // 危険段階地域区分、危険段階コード
                item.危険段階区分_表示 = string.IsNullOrEmpty(item.危険段階地域区分) ? item.危険段階区分 : item.危険段階地域区分 + " " + item.危険段階区分;

                // 配列へ再設定
                sub2Model[i] = item;
            }
            #endregion

            //データ引き渡し(ActiveReportスクリプトでデータソース設定)
            rpt.AddNamedItem("sub2Model", sub2Model);
        }
        #endregion    
    }
}
