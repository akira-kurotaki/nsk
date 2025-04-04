using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_P109010.Models
{
    /// <summary>
    /// バッチ条件
    /// </summary>
    public class BatchJouken
    {
        /// <summary>
        /// 条件_組合等コード
        /// </summary>
        public string JoukenKumiaitoCd { get; set; } = string.Empty;

        /// <summary>
        /// 条件_年産
        /// </summary>
        public string JoukenNensan { get; set; } = string.Empty;

        /// <summary>
        /// 条件_共済目的コード
        /// </summary>
        public string JoukenKyosaiMokutekiCd { get; set; } = string.Empty;

        /// <summary>
        /// 条件_大地区コード
        /// </summary>
        public string JoukenDaichiku { get; set; } = string.Empty;

        /// <summary>
        /// 条件_小地区コード開始
        /// </summary>
        public string JoukenShochikuStart { get; set; } = string.Empty;

        /// <summary>
        /// 条件_小地区コード終了
        /// </summary>
        public string JoukenShochikuEnd { get; set; } = string.Empty;

        /// <summary>
        /// 条件_組合員等コード開始
        /// </summary>
        public string JoukenKumiaiintoCdStart { get; set; } = string.Empty;

        /// <summary>
        /// 条件_組合員等コード終了
        /// </summary>
        public string JoukenKumiaiintoCdEnd { get; set; } = string.Empty;

        /// <summary>
        /// 条件_加入申込書のみ出力
        /// </summary>
        public string JoukenMoushikomishoShutsuryoku { get; set; } = string.Empty;

        /// <summary>
        /// 条件_類別明細の任意出力
        /// </summary>
        public string JoukenRuibetsuNiniShutsuryoku { get; set; } = string.Empty;

        /// <summary>
        /// 条件_文書番号
        /// </summary>
        public string JoukenBunshoNo { get; set; } = string.Empty;

        /// <summary>
        /// 条件_発行年月日
        /// </summary>
        public string JoukenHakkoDate { get; set; } = string.Empty;

        /// <summary>
        /// 条件_帳票名
        /// </summary>
        public string JoukenReportName { get; set; } = string.Empty;

        /// <summary>
        /// バッチ条件情報の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件のキー情報</param>
        /// <exception cref="AppException"></exception>
        public void GetTyouhyouSakuseiJouken(NskAppContext dbContext, string jid)
        {
            // ２．１．１．条件名定数から以下の項目を取得し、設定値をList<string>に格納する。
            // 条件名称
            List<string> joukenNames =
            [
                    JoukenNameConst.JOUKEN_KUMIAITO,                    // 組合等コード
                    JoukenNameConst.JOUKEN_NENSAN,                      // 年産
                    JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,             // 共済目的コード
                    JoukenNameConst.JOUKEN_DAICHIKU,                    // 大地区コード
                    JoukenNameConst.JOUKEN_SHOCHIKU_START,              // 小地区コード（開始）
                    JoukenNameConst.JOUKEN_SHOCHIKU_END,                // 小地区コード（終了）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,         // 組合員等コード（開始）
                    JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,           // 組合員等コード（終了）
                    JoukenNameConst.JOUKEN_MOUSHIKOMISHO_SHUTSURYOKU,   // 加入申込書のみ出力
                    JoukenNameConst.JOUKEN_RUIBETSU_NINI_SHUTSURYOKU,   // 類別明細の任意出力
                    JoukenNameConst.JOUKEN_BUNSHO_NO,                   // 文書番号
                    JoukenNameConst.JOUKEN_HAKKO_DATE,                  // 発行年月日
                    JoukenNameConst.JOUKEN_REPORT_NAME                  // 帳票名
            ];

            // ２．１．２．[変数：条件ID]とListをキーにバッチ条件テーブルから「バッチ条件情報」を取得する。
            List<T01050バッチ条件> batchJouken = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && joukenNames.Contains(x.条件名称))
                .ToList();
            // ２．１．３．「バッチ条件情報」が0件の場合は
            if (batchJouken.Count == 0)
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01645"　引数{0} ：パラメータの取得）
                throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
            }

            // ２．２．バッチ条件情報のチェック
            // ２．２．１．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータを条件値を変数に設定する。
            foreach (T01050バッチ条件 jouken in batchJouken)
            {
                switch (jouken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_KUMIAITO:                       // 組合等コード       ※必須
                        JoukenKumiaitoCd = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_NENSAN:                         // 年産               ※必須
                        JoukenNensan = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:                // 共済目的コード      ※必須
                        JoukenKyosaiMokutekiCd = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_DAICHIKU:                       // 大地区コード
                        JoukenDaichiku = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_START:                 // 小地区コード（開始）
                        JoukenShochikuStart = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_END:                   // 小地区コード（終了）
                        JoukenShochikuEnd = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START:            // 組合員等コード（開始）
                        JoukenKumiaiintoCdStart = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END:              // 組合員等コード（終了）
                        JoukenKumiaiintoCdEnd = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_MOUSHIKOMISHO_SHUTSURYOKU:      // 加入申込書のみ出力
                        JoukenMoushikomishoShutsuryoku = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_RUIBETSU_NINI_SHUTSURYOKU:      // 類別明細の任意出力
                        JoukenRuibetsuNiniShutsuryoku = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_BUNSHO_NO:                      // 文書番号
                        JoukenBunshoNo = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_HAKKO_DATE:                     // 発行年月日
                        JoukenHakkoDate = jouken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_REPORT_NAME:                    // 帳票名              ※必須
                        JoukenReportName = jouken.条件値;
                        break;
                }
            }
        }

        public void IsRequired()
        {
            // ２．２．２．[変数：条件_組合等コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKumiaitoCd))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054" 引数{0} ：条件_組合等コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_組合等コード"));
            }

            // ２．２．３．[変数：条件_年産]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenNensan))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054"　引数{0} ：条件_年産)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_年産"));
            }

            // ２．２．４．[変数：条件_共済目的コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKyosaiMokutekiCd))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054"　引数{0} ：条件_共済目的コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_共済目的コード"));
            }

            // ２．２．５．[変数：条件_帳票名]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenReportName))
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。　※1
                // （"ME01054"　引数{0} ：条件_帳票名)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "条件_帳票名"));
            }
        }
    }
}
