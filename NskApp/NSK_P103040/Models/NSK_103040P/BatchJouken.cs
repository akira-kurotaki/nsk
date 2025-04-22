using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;

namespace NSK_P103040.Models.P103040
{
    /// <summary>
    /// P1001の帳票用モデル（条件部）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class BatchJouken
    {
        /// <summary>
        /// 条件_年産
        /// </summary>
        public string joukenNensan { get; set; } = string.Empty;

        /// <summary>
        /// 条件_共済目的コード
        /// </summary>
        public string JoukenKyosaiMokutekiCd { get; set; } = string.Empty;

        /// <summary>
        /// 条件_類区分
        /// </summary>
        public string joukenRuikbn { get; set; } = string.Empty;

        /// <summary>
        /// 条件_帳票名
        /// </summary>
        public string joukenReportName { get; set; } = string.Empty;

        /// <summary>
        /// 帳票作成条件の取得
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件のキー情報</param>
        /// <exception cref="AppException"></exception>
        public void GetTyouhyouSakuseiJouken(NskAppContext dbContext, string jid)
        {
            var validJoukenNames = new[]
             {
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_NENSAN,
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_RUI_KBN,
                NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_REPORT_NAME
            };

            var batchJoukenDict = dbContext.Set<T01050バッチ条件>()
                                    .AsNoTracking()
                                    .Where(b => b.バッチ条件id == jid && validJoukenNames.Contains(b.条件名称))
                                    .ToDictionary(b => b.条件名称, b => b.条件値);

            joukenNensan = batchJoukenDict.GetValueOrDefault(NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_NENSAN) ?? "";
            JoukenKyosaiMokutekiCd = batchJoukenDict.GetValueOrDefault(NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD) ?? "";
            joukenRuikbn = batchJoukenDict.GetValueOrDefault(NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_RUI_KBN) ?? "";
            joukenReportName = batchJoukenDict.GetValueOrDefault(NskCommonLibrary.Core.Consts.JoukenNameConst.JOUKEN_REPORT_NAME) ?? "";
        }

        /// <summary>
        /// 必須入力チェック
        /// </summary>
        public void IsRequired()
        {
            // [変数：条件_組合等コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(joukenNensan))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力
                // （"ME01054" 引数{0} ：年産)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "年産"));
            }

            // [変数：条件_共済目的コード]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(JoukenKyosaiMokutekiCd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力
                // （"ME01054"　引数{0} ：組合等コード)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
            }

            // [変数：条件_帳票名]がnullまたは空文字の場合
            if (string.IsNullOrEmpty(joukenReportName))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力
                // （"ME01054"　引数{0} ：帳票名)
                throw new AppException("ME01054", MessageUtil.Get("ME01054", "帳票名"));
            }
        }
    }
}
