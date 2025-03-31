using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_B105110.Models
{
    /// <summary>
    /// 条件バッチ
    /// </summary>
    public class BatchJoken
    {
        /// <summary>
        /// 年産
        /// </summary>
        public string Nensan { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的
        /// </summary>
        public string KyosaiMokuteki { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string Ruikubun { get; set; } = string.Empty;

        /// <summary>
        /// 大地区
        /// </summary>
        public string Daichiku { get; set; } = string.Empty;

        /// <summary>
        /// 小地区（開始）
        /// </summary>
        public string ShochikuStart { get; set; } = string.Empty;

        /// <summary>
        /// 小地区（終了）
        /// </summary>
        public string ShochikuEnd { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コード（開始）
        /// </summary>
        public string KumiaiintoCdStart { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コード（終了）
        /// </summary>
        public string KumiaiintoCdEnd { get; set; } = string.Empty;

        /// <summary>
        /// 出力順キー1
        /// </summary>
        public string OrderByKey1 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順1
        /// </summary>
        public string OrderBy1 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順キー2
        /// </summary>
        public string OrderByKey2 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順2
        /// </summary>
        public string OrderBy2 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順キー3
        /// </summary>
        public string OrderByKey3 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順3
        /// </summary>
        public string OrderBy3 { get; set; } = string.Empty;

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 文字コード
        /// </summary>
        public string MojiCd { get; set; } = string.Empty;


        // 整合性チェックで取得した名称
        public string KyosaiMokutekiName { get; set; } = string.Empty;
        public string RuiName { get; set; } = string.Empty;
        public string DaichikuName { get; set; } = string.Empty;
        public string ShochikuNameStart { get; set; } = string.Empty;
        public string ShochikuNameEnd { get; set; } = string.Empty;

        /// <summary>
        /// バッチ条件を取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件ID</param>
        public void GetBatchJokens(NskAppContext dbContext, string jid)
        {
            // 条件名称リスト
            List<string> jokenNames =
            [
                JoukenNameConst.JOUKEN_NENSAN,                  // 年産
                JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,         // 共済目的
                JoukenNameConst.JOUKEN_RUI_KBN,                 // 類区分
                JoukenNameConst.JOUKEN_DAICHIKU,                // 大地区
                JoukenNameConst.JOUKEN_SHOCHIKU_START,          // 小地区（開始）
                JoukenNameConst.JOUKEN_SHOCHIKU_END,            // 小地区（終了）
                JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,     // 組合員等コード（開始）
                JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,       // 組合員等コード（終了）
                JoukenNameConst.JOUKEN_ORDER_BY_KEY1,           // 出力順キー1
                JoukenNameConst.JOUKEN_ORDER_BY1,               // 出力順1
                JoukenNameConst.JOUKEN_ORDER_BY_KEY2,           // 出力順キー2
                JoukenNameConst.JOUKEN_ORDER_BY2,               // 出力順2
                JoukenNameConst.JOUKEN_ORDER_BY_KEY3,           // 出力順キー3
                JoukenNameConst.JOUKEN_ORDER_BY3,               // 出力順3
                JoukenNameConst.JOUKEN_FILE_NAME,               // ファイル名
                JoukenNameConst.JOUKEN_MOJI_CD                  // 文字コード
            ];

            // ３．１．３．１．t_01050_バッチ条件から[引数：バッチ条件のキー情報]および下記「条件名称」に一致する「バッチ条件情報」を取得する。
            List<T01050バッチ条件> batchJokens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && jokenNames.Contains(x.条件名称))
                .ToList();

            // ３．１．４．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータのを条件値を変数に設定する。
            // バッチ条件情報

            // 条件値のリストからバッチ条件情報への値設定
            foreach (T01050バッチ条件 joken in batchJokens)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_NENSAN:                 // 年産　※必須
                        Nensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:        // 共済目的　※必須
                        KyosaiMokuteki = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_RUI_KBN:                // 類区分　※必須
                        Ruikubun = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_DAICHIKU:               // 大地区
                        Daichiku = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_START:         // 小地区（開始）
                        ShochikuStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_END:           // 小地区（終了）
                        ShochikuEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START:    // 組合員等コード（開始）
                        KumiaiintoCdStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END:      // 組合員等コード（終了）
                        KumiaiintoCdEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY1:          // 出力順キー1
                        OrderByKey1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY1:              // 出力順1
                        OrderBy1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY2:          // 出力順キー2
                        OrderByKey2 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY2:              // 出力順2
                        OrderBy2 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY3:          // 出力順キー3
                        OrderByKey3 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY3:              // 出力順3
                        OrderBy3 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_FILE_NAME:              // ファイル名　※必須
                        FileName = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_MOJI_CD:                // 文字コード　※必須
                        MojiCd = joken.条件値;
                        break;
                }

            }
        }

        /// <summary>
        /// 必須入力チェック
        /// </summary>
        /// <returns></returns>
        public bool IsRequired()
        {
            // ３．１．３．２．データが取得できない場合（[引数：バッチ条件のキー情報]が登録されていない場合）、
            if (    string.IsNullOrEmpty(Nensan)
                 || string.IsNullOrEmpty(KyosaiMokuteki)
                 || string.IsNullOrEmpty(Ruikubun)
                 || string.IsNullOrEmpty(Daichiku)
                 || string.IsNullOrEmpty(ShochikuStart)
                 || string.IsNullOrEmpty(ShochikuEnd)
                 || string.IsNullOrEmpty(KumiaiintoCdStart)
                 || string.IsNullOrEmpty(KumiaiintoCdEnd)
                 || string.IsNullOrEmpty(OrderByKey1)
                 || string.IsNullOrEmpty(OrderBy1)
                 || string.IsNullOrEmpty(OrderByKey2)
                 || string.IsNullOrEmpty(OrderBy2)
                 || string.IsNullOrEmpty(OrderByKey3)
                 || string.IsNullOrEmpty(OrderBy3)
                 || string.IsNullOrEmpty(FileName)
                 || string.IsNullOrEmpty(MojiCd))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 整合性チェック
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public bool IsConsistency(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, ref string errorMessage)
        {
            // 共済目的名称
            string? kyosaiMokutekiName = dbContext.M00010共済目的名称s
                .Where(x => x.共済目的コード == KyosaiMokuteki)
                .Select(x => x.共済目的名称)
                .FirstOrDefault();
            // 共済目的コードマスタチェック
            if (string.IsNullOrEmpty(kyosaiMokutekiName))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「７.」へ進む。
                // （"ME91003"、引数{0}：共済目的コード、引数{1}：共済目的マスタ)
                errorMessage = MessageUtil.Get("ME91003", "共済目的コード", "共済目的マスタ");
                return false;
            }
            KyosaiMokutekiName = kyosaiMokutekiName;

            // 類短縮名称
            string? ruiName = dbContext.M00020類名称s
                .Where(x => x.共済目的コード == KyosaiMokuteki && x.類区分 == Ruikubun)
                .Select(x => x.類名称)
                .FirstOrDefault();
            // 類区分マスタチェック
            if (string.IsNullOrEmpty(ruiName))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「７.」へ進む。
                // （"ME91003"、引数{0}：類区分、引数{1}：類名称)
                errorMessage = MessageUtil.Get("ME91003", "類区分", "類名称");
                return false;
            }
            RuiName = ruiName;

            // 大地区名
            string? daichikuName = dbContext.VDaichikuNms
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.DaichikuCd == Daichiku)
                .Select(x => x.DaichikuNm)
                .FirstOrDefault();
            // 大地区コードマスタチェック
            if (string.IsNullOrEmpty(daichikuName))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「７.」へ進む。
                // （"ME91003"、引数{0}：大地区コード、引数{1}：大地区マスタ)
                errorMessage = MessageUtil.Get("ME91003", "大地区コード", "大地区マスタ");
                return false;
            }
            DaichikuName = daichikuName;

            // 小地区名（開始）
            string? shochikuNameStart = dbContext.VShochikuNms
                            .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.DaichikuCd == Daichiku && x.ShochikuCd == ShochikuStart)
                            .Select(x => x.ShochikuNm)
                            .FirstOrDefault();            // 小地区コード（開始）マスタチェック
            if (string.IsNullOrEmpty(shochikuNameStart))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「７.」へ進む。
                // （"ME91003"、引数{0}：小地区コードFrom、引数{1}：小地区マスタ)
                errorMessage = MessageUtil.Get("ME91003", "小地区コードFrom", "小地区マスタ");
                return false;
            }
            ShochikuNameStart = shochikuNameStart;

            // 小地区名（終了）
            string? shochikuNameEnd = dbContext.VShochikuNms
                            .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.DaichikuCd == Daichiku && x.ShochikuCd == ShochikuEnd)
                            .Select(x => x.ShochikuNm)
                            .FirstOrDefault();            // 小地区コード（開始）マスタチェック
            // 小地区コード（終了）マスタチェック
            if (string.IsNullOrEmpty(shochikuNameEnd))
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「７.」へ進む。
                // （"ME91003"、引数{0}：小地区コードTo、引数{1}：小地区マスタ)
                errorMessage = MessageUtil.Get("ME91003", "小地区コードTo", "小地区マスタ");
                return false;
            }
            ShochikuNameEnd = shochikuNameEnd;

            errorMessage = string.Empty;
            return true;
        }
    }
}