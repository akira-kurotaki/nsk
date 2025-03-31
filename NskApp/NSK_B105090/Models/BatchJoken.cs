using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using ModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskCommonLibrary.Core.Consts;

namespace NSK_B105090.Models
{
    /// <summary>
    /// バッチ条件
    /// </summary>
    public class BatchJoken
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public string Nensan { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的
        /// </summary>
        public string KyosaiMokutekiCd { get; set; } = string.Empty;

        /// <summary>
        /// 大地区
        /// </summary>
        public string DaichikuCd { get; set; } = string.Empty;

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
        /// 類区分
        /// </summary>
        public string RuiKbn { get; set; } = string.Empty;

        /// <summary>
        /// 引受方式コード
        /// </summary>
        public string HikiukeHoushikiCd { get; set; } = string.Empty;

        /// <summary>
        /// 特約区分
        /// </summary>
        public string TokuyakuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 補償割合
        /// </summary>
        public string HoshoWariaiCd { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時（開始）
        /// </summary>
        public string UpdateDateStart { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時（終了）
        /// </summary>
        public string UpdateDateEnd { get; set; } = string.Empty;

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


        /// <summary>
        /// 共済目的名称
        /// </summary>
        public string KyosaiMokutekiNm { get; set; } = string.Empty;
        /// <summary>
        /// 大地区名称
        /// </summary>
        public string DaichikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 小地区（開始）名称
        /// </summary>
        public string ShochikuNmStart { get; set; } = string.Empty;
        /// <summary>
        /// 小地区（終了）名称
        /// </summary>
        public string ShochikuNmEnd { get; set; } = string.Empty;

        /// <summary>
        /// バッチ条件を取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="jid">バッチ条件ID</param>
        public void GetBatchJokens(NskAppContext dbContext, string jid, ref string errMsg)
        {
            // ５．１．バッチ条件情報の取得

	        // ５．１．１．条件名定数から以下の項目を取得し、設定値をList<string> に格納する。
            // 条件名称リスト
            List<string> jokenNames =
            [
                JoukenNameConst.JOUKEN_KUMIAITO_CD,           // 組合等コード
                JoukenNameConst.JOUKEN_NENSAN,                // 年産
                JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI,       // 共済目的
                JoukenNameConst.JOUKEN_DAICHIKU,              // 大地区
                JoukenNameConst.JOUKEN_SHOCHIKU_START,        // 小地区（開始）
                JoukenNameConst.JOUKEN_SHOCHIKU_END,          // 小地区（終了）
                JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START,   // 組合員等コード（開始）
                JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END,     // 組合員等コード（終了）
                JoukenNameConst.JOUKEN_RUI_KBN,               // 類区分
                JoukenNameConst.JOUKEN_HIKIUKE_HOUSHIKI_CD,   // 引受方式
                JoukenNameConst.JOUKEN_TOKUYAKU_KBN,          // 特約区分
                JoukenNameConst.JOUKEN_HOSHO_WARIAI_CD,       // 補償割合
                JoukenNameConst.JOUKEN_UPDATE_DATE_START,     // 更新日時（開始）
                JoukenNameConst.JOUKEN_UPDATE_DATE_END,       // 更新日時（終了）
                JoukenNameConst.JOUKEN_ORDER_BY_KEY1,         // 出力順キー1
                JoukenNameConst.JOUKEN_ORDER_BY1,             // 出力順1
                JoukenNameConst.JOUKEN_ORDER_BY_KEY2,         // 出力順キー2
                JoukenNameConst.JOUKEN_ORDER_BY2,             // 出力順2
                JoukenNameConst.JOUKEN_ORDER_BY_KEY3,         // 出力順キー3
                JoukenNameConst.JOUKEN_ORDER_BY3,             // 出力順3
                JoukenNameConst.JOUKEN_FILE_NAME,             // ファイル名
                JoukenNameConst.JOUKEN_MOJI_CD                // 文字コード
            ];

            // ５．１．２．[変数：バッチ条件のキー情報]とListをキーにバッチ条件テーブルから「バッチ条件情報」を取得する。
            List<T01050バッチ条件> batchJokens = dbContext.T01050バッチ条件s
                .Where(x => x.バッチ条件id == jid && jokenNames.Contains(x.条件名称))?
                .ToList() ?? new();

            // ５．１．３．「バッチ条件情報」が0件の場合、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
            if (batchJokens.Count == 0)
            {
                // （"ME01645"　引数{0} ：パラメータの取得)
                errMsg = MessageUtil.Get("ME01645", "パラメータの取得");
                throw new AppException("ME01645", errMsg);
            }

            // ５．２．バッチ条件情報のチェック
            // ５．２．１．取得した「バッチ条件情報」のうち条件名称が下記と一致するデータのを条件値を変数に設定する。

            foreach (T01050バッチ条件 joken in batchJokens)
            {
                switch (joken.条件名称)
                {
                    case JoukenNameConst.JOUKEN_KUMIAITO_CD:          // 組合等コード　※必須
                        KumiaitoCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_NENSAN:               // 年産　※必須
                        Nensan = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI:      // 共済目的　※必須
                        KyosaiMokutekiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_DAICHIKU:             // 大地区
                        DaichikuCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_START:       // 小地区（開始）
                        ShochikuStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_SHOCHIKU_END:         // 小地区（終了）
                        ShochikuEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_START:  // 組合員等コード（開始）
                        KumiaiintoCdStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_KUMIAIINTO_CD_END:    // 組合員等コード（終了）
                        KumiaiintoCdEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_RUI_KBN:              // 類区分
                        RuiKbn = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_HIKIUKE_HOUSHIKI_CD:  // 引受方式
                        HikiukeHoushikiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_TOKUYAKU_KBN:         // 特約区分
                        HikiukeHoushikiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_HOSHO_WARIAI_CD:      // 補償割合
                        HikiukeHoushikiCd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_UPDATE_DATE_START:    // 更新日時（開始）
                        UpdateDateStart = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_UPDATE_DATE_END:      // 更新日時（終了）
                        UpdateDateEnd = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY1:        // 出力順キー1
                        OrderByKey1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY1:            // 出力順1
                        OrderBy1 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY2:        // 出力順キー2
                        OrderByKey2 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY2:            // 出力順2
                        OrderBy2 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY_KEY3:        // 出力順キー3
                        OrderByKey3 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_ORDER_BY3:            // 出力順3
                        OrderBy3 = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_FILE_NAME:            // ファイル名　※必須
                        FileName = joken.条件値;
                        break;
                    case JoukenNameConst.JOUKEN_MOJI_CD:              // 文字コード　※必須
                        MojiCd = joken.条件値;
                        break;
                }

            }
        }

        /// <summary>
        /// 必須入力チェック
        /// </summary>
        public void IsRequired(ref string errMsg)
        {
            // ５．２．２．[変数：条件_組合等コード]がnullまたは空文字の場合、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
            if (string.IsNullOrEmpty(KumiaitoCd))
            {
                // "ME01054"：{0}が引数に設定されていません。システム管理者に連絡してください。		
                // {0}：条件_組合等コード
                errMsg = MessageUtil.Get("ME01054", "条件_組合等コード");
                throw new AppException("ME01054", errMsg);
            }

            // ５．２．３．[変数：条件_年産]がnullまたは空文字の場合、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
            if (string.IsNullOrEmpty(Nensan))
            {
                // "ME01054"：{0}が引数に設定されていません。システム管理者に連絡してください。		
                // {0}：条件_年産
                errMsg = MessageUtil.Get("ME01054", "条件_年産");
                throw new AppException("ME01054", errMsg);
            }

            // ５．２．４．[変数：条件_共済目的コード]がnullまたは空文字の場合、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
            if (string.IsNullOrEmpty(KyosaiMokutekiCd))
            {
                // "ME01054"：{0}が引数に設定されていません。システム管理者に連絡してください。		
                // {0}：条件_共済目的コード
                errMsg = MessageUtil.Get("ME01054", "条件_共済目的コード");
                throw new AppException("ME01054", errMsg);
            }

            // ５．２．５．[変数：条件_ファイル名]がnullまたは空文字の場合、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
            if (string.IsNullOrEmpty(FileName))
            {
                // "ME01054"：{0}が引数に設定されていません。システム管理者に連絡してください。		
                // {0}：条件_ファイル名
                errMsg = MessageUtil.Get("ME01054", "条件_ファイル名");
                throw new AppException("ME01054", errMsg);
            }

            // ５．２．６．[変数：条件_文字コード]がnullまたは空文字の場合、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
            if (string.IsNullOrEmpty(MojiCd))
            {
                // "ME01054"：{0}が引数に設定されていません。システム管理者に連絡してください。		
                // {0}：条件_文字コード
                errMsg = MessageUtil.Get("ME01054", "条件_文字コード");
                throw new AppException("ME01054", errMsg);
            }
        }

        /// <summary>
        /// 整合性チェック
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        public void IsConsistency(NskAppContext dbContext, Arguments arg, ref string errMsg)
        {
            // ６．７．[配列：バッチ条件]から組合等コードが取得できた場合、「検索条件組合等コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(KumiaitoCd))
            {
                int kumiaitoCdCnt = dbContext.VKumiaitos.Count(x =>
                    (x.KumiaitoCd == KumiaitoCd) &&
                    (x.TodofukenCd == arg.TodofukenCd));

                // ６．８．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (kumiaitoCdCnt == 0)
                {
                    //（"ME10005"　引数{0} ：条件_組合等コード)
                    errMsg = MessageUtil.Get("ME10005", "条件_組合等コード");
                    throw new AppException("ME10005", errMsg);
                }
            }

            // ６．９．[配列：バッチ条件]から共済目的コードが取得できた場合、「共済目的コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(KyosaiMokutekiCd))
            {
                M00010共済目的名称? kyosaiMokuteki = dbContext.M00010共済目的名称s.FirstOrDefault(x =>
                    x.共済目的コード == KyosaiMokutekiCd);

                // ６．１０．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (kyosaiMokuteki is null)
                {
                    //（"ME10005"　引数{0} ：条件_共済目的コード)
                    errMsg = MessageUtil.Get("ME10005", "条件_共済目的コード");
                    throw new AppException("ME10005", errMsg);
                }

                KyosaiMokutekiNm = kyosaiMokuteki.共済目的名称;
            }

            // ６．１１．[配列：バッチ条件]から大地区コードが取得できた場合、「大地区コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(DaichikuCd))
            {
                VDaichikuNm? daichiku = dbContext.VDaichikuNms.FirstOrDefault(x => 
                    (x.TodofukenCd == arg.TodofukenCd) &&
                    (x.KumiaitoCd == arg.KumiaitoCd) &&
                    (x.DaichikuCd == DaichikuCd));

                // ６．１２．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (daichiku is null)
                {
                    //（"ME10005"　引数{0} ：条件_大地区コード)
                    errMsg = MessageUtil.Get("ME10005", "条件_大地区コード");
                    throw new AppException("ME10005", errMsg);
                }

                DaichikuNm = daichiku.DaichikuNm;
            }

            // ６．１３．[配列：バッチ条件]から類区分が取得できた場合、「類区分存在情報」を取得する。
            if (!string.IsNullOrEmpty(RuiKbn))
            {
                int ruiKbnCnt = dbContext.M00020類名称s.Count(x =>
                    (x.共済目的コード == KyosaiMokutekiCd) &&
                    (x.類区分 == RuiKbn));

                // ６．１４．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (ruiKbnCnt == 0)
                {
                    //（"ME10005"　引数{0} ：条件_類区分)
                    errMsg = MessageUtil.Get("ME10005", "条件_類区分");
                    throw new AppException("ME10005", errMsg);
                }
            }

            // ６．１５．[配列：バッチ条件]から引受方式が取得できた場合、「引受方式存在情報」を取得する。
            if (!string.IsNullOrEmpty(HikiukeHoushikiCd))
            {
                int HikiukeHoushikiCdCnt = dbContext.M10080引受方式名称s.Count(x =>
                    x.引受方式 == HikiukeHoushikiCd);

                // ６．１６．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (HikiukeHoushikiCdCnt == 0)
                {
                    //（"ME10005"　引数{0} ：条件_引受方式)
                    errMsg = MessageUtil.Get("ME10005", "条件_引受方式");
                    throw new AppException("ME10005", errMsg);
                }
            }

            // ６．１７．[配列：バッチ条件]から特約区分が取得できた場合、「特約区分存在情報」を取得する。
            if (!string.IsNullOrEmpty(TokuyakuKbn))
            {
                int tokuyakuKbnCnt = dbContext.M10100特約区分名称s.Count(x =>
                    x.特約区分 == TokuyakuKbn);

                // ６．１６．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (tokuyakuKbnCnt == 0)
                {
                    //（"ME10005"　引数{0} ：条件_特約区分)
                    errMsg = MessageUtil.Get("ME10005", "条件_特約区分");
                    throw new AppException("ME10005", errMsg);
                }
            }

            // ６．１９．[配列：バッチ条件]から補償割合が取得できた場合、「補償割合存在情報」を取得する。
            if (!string.IsNullOrEmpty(HoshoWariaiCd))
            {
                int hoshoWariaiCnt = dbContext.M20030補償割合名称s.Count(x =>
                    x.補償割合コード == HoshoWariaiCd);

                // ６．２０．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (hoshoWariaiCnt == 0)
                {
                    //（"ME10005"　引数{0} ：条件_補償割合)
                    errMsg = MessageUtil.Get("ME10005", "条件_補償割合");
                    throw new AppException("ME10005", errMsg);
                }
            }

            // ６．２１．[配列：バッチ条件]から小地区（開始）が取得できた場合、「小地区コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(ShochikuStart))
            {
                VShochikuNm? shochiku = dbContext.VShochikuNms.FirstOrDefault(x =>
                    (x.TodofukenCd == arg.TodofukenCd) &&
                    (x.KumiaitoCd == arg.KumiaitoCd) &&
                    (x.DaichikuCd == DaichikuCd) &&
                    (x.ShochikuCd == ShochikuStart));

                // ６．２２．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (shochiku is null)
                {
                    //（"ME10005"　引数{0} ：条件_小地区コード（開始）)
                    errMsg = MessageUtil.Get("ME10005", "条件_小地区コード（開始）");
                    throw new AppException("ME10005", errMsg);
                }

                ShochikuNmStart = shochiku.ShochikuNm;
            }

            // ６．２３．[配列：バッチ条件]から小地区（終了）が取得できた場合、「小地区コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(ShochikuEnd))
            {
                VShochikuNm? shochiku = dbContext.VShochikuNms.FirstOrDefault(x =>
                    (x.TodofukenCd == arg.TodofukenCd) &&
                    (x.KumiaitoCd == arg.KumiaitoCd) &&
                    (x.DaichikuCd == DaichikuCd) &&
                    (x.ShochikuCd == ShochikuEnd));

                // ６．２４．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (shochiku is null)
                {
                    //（"ME10005"　引数{0} ：条件_小地区コード（終了）)
                    errMsg = MessageUtil.Get("ME10005", "条件_小地区コード（終了）");
                    throw new AppException("ME10005", errMsg);
                }

                ShochikuNmEnd = shochiku.ShochikuNm;
            }
        }
    }
}